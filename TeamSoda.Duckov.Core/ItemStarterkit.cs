using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class ItemStarterkit : InteractableBase
{
	// Token: 0x06000722 RID: 1826 RVA: 0x0002049F File Offset: 0x0001E69F
	protected override bool IsInteractable()
	{
		return !this.picked && this.cached;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x000204B8 File Offset: 0x0001E6B8
	private UniTask CacheItems()
	{
		ItemStarterkit.<CacheItems>d__10 <CacheItems>d__;
		<CacheItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CacheItems>d__.<>4__this = this;
		<CacheItems>d__.<>1__state = -1;
		<CacheItems>d__.<>t__builder.Start<ItemStarterkit.<CacheItems>d__10>(ref <CacheItems>d__);
		return <CacheItems>d__.<>t__builder.Task;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x000204FB File Offset: 0x0001E6FB
	protected override void Awake()
	{
		base.Awake();
		SavesSystem.OnCollectSaveData += this.Save;
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00020525 File Offset: 0x0001E725
	protected override void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		base.OnDestroy();
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0002054F File Offset: 0x0001E74F
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		this.picked = false;
		this.Save();
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0002055E File Offset: 0x0001E75E
	private void Save()
	{
		SavesSystem.Save<bool>(this.saveKey, this.picked);
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00020574 File Offset: 0x0001E774
	private void Load()
	{
		this.picked = SavesSystem.Load<bool>(this.saveKey);
		base.MarkerActive = !this.picked;
		if (this.notPickedItem)
		{
			GameObject gameObject = this.notPickedItem;
			if (gameObject != null)
			{
				gameObject.SetActive(!this.picked);
			}
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(this.picked);
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000205E6 File Offset: 0x0001E7E6
	protected override void Start()
	{
		base.Start();
		this.Load();
		if (!this.picked)
		{
			this.CacheItems().Forget();
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00020608 File Offset: 0x0001E808
	protected override void OnInteractFinished()
	{
		foreach (Item item in this.itemsCache)
		{
			ItemUtilities.SendToPlayerCharacter(item, false);
		}
		this.picked = true;
		base.MarkerActive = !this.picked;
		this.itemsCache.Clear();
		this.OnPicked();
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00020684 File Offset: 0x0001E884
	private void OnPicked()
	{
		if (this.notPickedItem)
		{
			this.notPickedItem.SetActive(false);
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(true);
		}
		if (this.pickFX)
		{
			this.pickFX.SetActive(true);
		}
		NotificationText.Push(this.notificationTextKey.ToPlainText());
	}

	// Token: 0x040006C8 RID: 1736
	[ItemTypeID]
	[SerializeField]
	private List<int> items;

	// Token: 0x040006C9 RID: 1737
	[SerializeField]
	private GameObject notPickedItem;

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private GameObject pickedItem;

	// Token: 0x040006CB RID: 1739
	[SerializeField]
	private GameObject pickFX;

	// Token: 0x040006CC RID: 1740
	private List<Item> itemsCache;

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	private string notificationTextKey;

	// Token: 0x040006CE RID: 1742
	private bool caching;

	// Token: 0x040006CF RID: 1743
	private bool cached;

	// Token: 0x040006D0 RID: 1744
	private bool picked;

	// Token: 0x040006D1 RID: 1745
	private string saveKey = "StarterKit_Picked";
}
