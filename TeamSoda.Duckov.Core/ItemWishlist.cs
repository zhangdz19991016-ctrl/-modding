using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class ItemWishlist : MonoBehaviour
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00037F4A File Offset: 0x0003614A
	// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00037F51 File Offset: 0x00036151
	public static ItemWishlist Instance { get; private set; }

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06000D60 RID: 3424 RVA: 0x00037F5C File Offset: 0x0003615C
	// (remove) Token: 0x06000D61 RID: 3425 RVA: 0x00037F90 File Offset: 0x00036190
	public static event Action<int> OnWishlistChanged;

	// Token: 0x06000D62 RID: 3426 RVA: 0x00037FC4 File Offset: 0x000361C4
	private void Awake()
	{
		ItemWishlist.Instance = this;
		QuestManager.onQuestListsChanged += this.OnQuestListChanged;
		BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		SavesSystem.OnCollectSaveData += this.Save;
		UIInputManager.OnWishlistHoveringItem += this.OnWishlistHoveringItem;
		this.Load();
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00038021 File Offset: 0x00036221
	private void OnDestroy()
	{
		QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		SavesSystem.OnCollectSaveData -= this.Save;
		UIInputManager.OnWishlistHoveringItem -= this.OnWishlistHoveringItem;
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00038058 File Offset: 0x00036258
	private void OnWishlistHoveringItem(UIInputEventData data)
	{
		if (!ItemHoveringUI.Shown)
		{
			return;
		}
		int displayingItemID = ItemHoveringUI.DisplayingItemID;
		if (this.IsManuallyWishlisted(displayingItemID))
		{
			ItemWishlist.RemoveFromWishlist(displayingItemID);
		}
		else
		{
			ItemWishlist.AddToWishList(displayingItemID);
		}
		ItemHoveringUI.NotifyRefreshWishlistInfo();
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x00038090 File Offset: 0x00036290
	private void Load()
	{
		this.manualWishList.Clear();
		List<int> list = SavesSystem.Load<List<int>>("ItemWishlist_Manual");
		if (list != null)
		{
			this.manualWishList.AddRange(list);
		}
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x000380C2 File Offset: 0x000362C2
	private void Save()
	{
		SavesSystem.Save<List<int>>("ItemWishlist_Manual", this.manualWishList);
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x000380D4 File Offset: 0x000362D4
	private void Start()
	{
		this.CacheQuestItems();
		this.CacheBuildingItems();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x000380E2 File Offset: 0x000362E2
	private void OnQuestListChanged(QuestManager obj)
	{
		this.CacheQuestItems();
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x000380EA File Offset: 0x000362EA
	private void OnBuildingListChanged()
	{
		this.CacheBuildingItems();
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x000380F2 File Offset: 0x000362F2
	private void CacheQuestItems()
	{
		this._questRequiredItems = QuestManager.GetAllRequiredItems().ToHashSet<int>();
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00038104 File Offset: 0x00036304
	private void CacheBuildingItems()
	{
		this._buildingRequiredItems.Clear();
		foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
		{
			if (buildingInfo.RequirementsSatisfied() && buildingInfo.TokenAmount + buildingInfo.CurrentAmount < buildingInfo.maxAmount)
			{
				foreach (Cost.ItemEntry itemEntry in buildingInfo.cost.items)
				{
					this._buildingRequiredItems.Add(itemEntry.id);
				}
			}
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x00038193 File Offset: 0x00036393
	private IEnumerable<int> IterateAll()
	{
		foreach (int num in this.manualWishList)
		{
			yield return num;
		}
		List<int>.Enumerator enumerator = default(List<int>.Enumerator);
		IEnumerable<int> allRequiredItems = QuestManager.GetAllRequiredItems();
		foreach (int num2 in allRequiredItems)
		{
			yield return num2;
		}
		IEnumerator<int> enumerator2 = null;
		yield break;
		yield break;
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x000381A3 File Offset: 0x000363A3
	public bool IsQuestRequired(int itemTypeID)
	{
		return this._questRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x000381B1 File Offset: 0x000363B1
	public bool IsManuallyWishlisted(int itemTypeID)
	{
		return this.manualWishList.Contains(itemTypeID);
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x000381BF File Offset: 0x000363BF
	public bool IsBuildingRequired(int itemTypeID)
	{
		return this._buildingRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x000381D0 File Offset: 0x000363D0
	public static void AddToWishList(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return;
		}
		if (ItemWishlist.Instance.manualWishList.Contains(itemTypeID))
		{
			return;
		}
		ItemWishlist.Instance.manualWishList.Add(itemTypeID);
		Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
		if (onWishlistChanged == null)
		{
			return;
		}
		onWishlistChanged(itemTypeID);
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0003821E File Offset: 0x0003641E
	public static bool RemoveFromWishlist(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return false;
		}
		bool flag = ItemWishlist.Instance.manualWishList.Remove(itemTypeID);
		if (flag)
		{
			Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
			if (onWishlistChanged == null)
			{
				return flag;
			}
			onWishlistChanged(itemTypeID);
		}
		return flag;
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00038254 File Offset: 0x00036454
	public static ItemWishlist.WishlistInfo GetWishlistInfo(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return default(ItemWishlist.WishlistInfo);
		}
		bool isManuallyWishlisted = ItemWishlist.Instance.IsManuallyWishlisted(itemTypeID);
		bool isQuestRequired = ItemWishlist.Instance.IsQuestRequired(itemTypeID);
		bool isBuildingRequired = ItemWishlist.Instance.IsBuildingRequired(itemTypeID);
		return new ItemWishlist.WishlistInfo
		{
			itemTypeID = itemTypeID,
			isManuallyWishlisted = isManuallyWishlisted,
			isQuestRequired = isQuestRequired,
			isBuildingRequired = isBuildingRequired
		};
	}

	// Token: 0x04000B7E RID: 2942
	private List<int> manualWishList = new List<int>();

	// Token: 0x04000B7F RID: 2943
	private HashSet<int> _questRequiredItems = new HashSet<int>();

	// Token: 0x04000B80 RID: 2944
	private HashSet<int> _buildingRequiredItems = new HashSet<int>();

	// Token: 0x04000B82 RID: 2946
	private const string SaveKey = "ItemWishlist_Manual";

	// Token: 0x020004D9 RID: 1241
	public struct WishlistInfo
	{
		// Token: 0x04001D25 RID: 7461
		public int itemTypeID;

		// Token: 0x04001D26 RID: 7462
		public bool isManuallyWishlisted;

		// Token: 0x04001D27 RID: 7463
		public bool isQuestRequired;

		// Token: 0x04001D28 RID: 7464
		public bool isBuildingRequired;
	}
}
