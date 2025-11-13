using System;
using System.Collections.Generic;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x02000129 RID: 297
[CreateAssetMenu]
public class SceneInfoCollection : ScriptableObject
{
	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0002A794 File Offset: 0x00028994
	internal static SceneInfoCollection Instance
	{
		get
		{
			GameplayDataSettings.SceneManagementData sceneManagement = GameplayDataSettings.SceneManagement;
			if (sceneManagement == null)
			{
				return null;
			}
			return sceneManagement.SceneInfoCollection;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x0002A7A6 File Offset: 0x000289A6
	public static List<SceneInfoEntry> Entries
	{
		get
		{
			if (SceneInfoCollection.Instance == null)
			{
				return null;
			}
			return SceneInfoCollection.Instance.entries;
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0002A7C4 File Offset: 0x000289C4
	public SceneInfoEntry InstanceGetSceneInfo(string id)
	{
		return this.entries.Find((SceneInfoEntry e) => e.ID == id);
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0002A7F8 File Offset: 0x000289F8
	public string InstanceGetSceneID(int buildIndex)
	{
		SceneInfoEntry sceneInfoEntry = this.entries.Find((SceneInfoEntry e) => e != null && e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == buildIndex);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.ID;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0002A835 File Offset: 0x00028A35
	internal string InstanceGetSceneID(SceneReference sceneRef)
	{
		if (sceneRef.UnsafeReason != SceneReferenceUnsafeReason.None)
		{
			return null;
		}
		return this.InstanceGetSceneID(sceneRef.BuildIndex);
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002A850 File Offset: 0x00028A50
	internal SceneReference InstanceGetSceneReferencce(string requireSceneID)
	{
		SceneInfoEntry sceneInfoEntry = this.InstanceGetSceneInfo(requireSceneID);
		if (sceneInfoEntry == null)
		{
			return null;
		}
		return sceneInfoEntry.SceneReference;
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0002A870 File Offset: 0x00028A70
	public static SceneInfoEntry GetSceneInfo(string sceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneInfo(sceneID);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0002A88C File Offset: 0x00028A8C
	public static string GetSceneID(SceneReference sceneRef)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(sceneRef);
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0002A8A8 File Offset: 0x00028AA8
	public static string GetSceneID(int buildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.InstanceGetSceneID(buildIndex);
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002A8C4 File Offset: 0x00028AC4
	internal static int GetBuildIndex(string overrideSceneID)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return -1;
		}
		SceneInfoEntry sceneInfoEntry = SceneInfoCollection.Instance.InstanceGetSceneInfo(overrideSceneID);
		if (sceneInfoEntry == null)
		{
			return -1;
		}
		return sceneInfoEntry.BuildIndex;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002A8F8 File Offset: 0x00028AF8
	internal static SceneInfoEntry GetSceneInfo(int sceneBuildIndex)
	{
		if (SceneInfoCollection.Instance == null)
		{
			return null;
		}
		return SceneInfoCollection.Instance.entries.Find((SceneInfoEntry e) => e.BuildIndex == sceneBuildIndex);
	}

	// Token: 0x04000896 RID: 2198
	public const string BaseSceneID = "Base";

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	private List<SceneInfoEntry> entries;
}
