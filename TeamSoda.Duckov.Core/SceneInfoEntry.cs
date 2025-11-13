using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x0200012A RID: 298
[Serializable]
public class SceneInfoEntry
{
	// Token: 0x060009C5 RID: 2501 RVA: 0x0002A944 File Offset: 0x00028B44
	public SceneInfoEntry()
	{
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002A94C File Offset: 0x00028B4C
	public SceneInfoEntry(string id, SceneReference sceneReference)
	{
		this.id = id;
		this.sceneReference = sceneReference;
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002A962 File Offset: 0x00028B62
	public int BuildIndex
	{
		get
		{
			if (this.sceneReference.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return -1;
			}
			return this.sceneReference.BuildIndex;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002A97E File Offset: 0x00028B7E
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002A986 File Offset: 0x00028B86
	public SceneReference SceneReference
	{
		get
		{
			return this.sceneReference;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x0002A98E File Offset: 0x00028B8E
	public string Description
	{
		get
		{
			return this.description.ToPlainText();
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x0002A99B File Offset: 0x00028B9B
	public string DisplayName
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return this.displayName.ToPlainText();
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002A9BC File Offset: 0x00028BBC
	public string DisplayNameRaw
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return this.displayName;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x0002A9D8 File Offset: 0x00028BD8
	public bool IsLoaded
	{
		get
		{
			return this.sceneReference != null && this.sceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && this.sceneReference.LoadedScene.isLoaded;
		}
	}

	// Token: 0x04000898 RID: 2200
	[SerializeField]
	private string id;

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private SceneReference sceneReference;

	// Token: 0x0400089A RID: 2202
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayName;

	// Token: 0x0400089B RID: 2203
	[LocalizationKey("Default")]
	[SerializeField]
	private string description;
}
