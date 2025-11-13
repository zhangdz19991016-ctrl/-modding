using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class OverrideDeathSceneRouting : MonoBehaviour
{
	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0002B620 File Offset: 0x00029820
	// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0002B627 File Offset: 0x00029827
	public static OverrideDeathSceneRouting Instance { get; private set; }

	// Token: 0x06000A06 RID: 2566 RVA: 0x0002B62F File Offset: 0x0002982F
	private void OnEnable()
	{
		if (OverrideDeathSceneRouting.Instance != null)
		{
			Debug.LogError("存在多个OverrideDeathSceneRouting实例");
		}
		OverrideDeathSceneRouting.Instance = this;
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0002B64E File Offset: 0x0002984E
	private void OnDisable()
	{
		if (OverrideDeathSceneRouting.Instance == this)
		{
			OverrideDeathSceneRouting.Instance = null;
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0002B663 File Offset: 0x00029863
	public string GetSceneID()
	{
		return this.sceneID;
	}

	// Token: 0x040008DE RID: 2270
	[SceneID]
	[SerializeField]
	private string sceneID;
}
