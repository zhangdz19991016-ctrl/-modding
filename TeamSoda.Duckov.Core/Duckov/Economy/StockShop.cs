using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x0200032B RID: 811
	public class StockShop : MonoBehaviour, IMerchant, ISaveDataProvider
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001B4A RID: 6986 RVA: 0x00063080 File Offset: 0x00061280
		public string MerchantID
		{
			get
			{
				return this.merchantID;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x00063088 File Offset: 0x00061288
		public string OpinionKey
		{
			get
			{
				return "Opinion_" + this.merchantID;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x0006309A File Offset: 0x0006129A
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x000630A7 File Offset: 0x000612A7
		// (set) Token: 0x06001B4E RID: 6990 RVA: 0x000630BE File Offset: 0x000612BE
		private int Opinion
		{
			get
			{
				return Mathf.Clamp(CommonVariables.GetInt(this.OpinionKey, 0), -100, 100);
			}
			set
			{
				CommonVariables.SetInt(this.OpinionKey, value);
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001B4F RID: 6991 RVA: 0x000630CC File Offset: 0x000612CC
		public string PurchaseNotificationTextFormat
		{
			get
			{
				return this.purchaseNotificationTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001B50 RID: 6992 RVA: 0x000630D9 File Offset: 0x000612D9
		public bool AccountAvaliable
		{
			get
			{
				return this.accountAvaliable;
			}
		}

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06001B51 RID: 6993 RVA: 0x000630E4 File Offset: 0x000612E4
		// (remove) Token: 0x06001B52 RID: 6994 RVA: 0x00063118 File Offset: 0x00061318
		public static event Action<StockShop> OnAfterItemSold;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06001B53 RID: 6995 RVA: 0x0006314C File Offset: 0x0006134C
		// (remove) Token: 0x06001B54 RID: 6996 RVA: 0x00063180 File Offset: 0x00061380
		public static event Action<StockShop, Item> OnItemPurchased;

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06001B55 RID: 6997 RVA: 0x000631B4 File Offset: 0x000613B4
		// (remove) Token: 0x06001B56 RID: 6998 RVA: 0x000631E8 File Offset: 0x000613E8
		public static event Action<StockShop, Item, int> OnItemSoldByPlayer;

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x0006321C File Offset: 0x0006141C
		public TimeSpan TimeSinceLastRefresh
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return DateTime.UtcNow - dateTime;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001B58 RID: 7000 RVA: 0x0006326C File Offset: 0x0006146C
		public TimeSpan NextRefreshETA
		{
			get
			{
				TimeSpan timeSinceLastRefresh = this.TimeSinceLastRefresh;
				TimeSpan timeSpan = TimeSpan.FromTicks(this.refreshAfterTimeSpan) - timeSinceLastRefresh;
				if (timeSpan < TimeSpan.Zero)
				{
					timeSpan = TimeSpan.Zero;
				}
				return timeSpan;
			}
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000632A8 File Offset: 0x000614A8
		private UniTask<Item> GetItemInstance(int typeID)
		{
			StockShop.<GetItemInstance>d__40 <GetItemInstance>d__;
			<GetItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<GetItemInstance>d__.<>4__this = this;
			<GetItemInstance>d__.typeID = typeID;
			<GetItemInstance>d__.<>1__state = -1;
			<GetItemInstance>d__.<>t__builder.Start<StockShop.<GetItemInstance>d__40>(ref <GetItemInstance>d__);
			return <GetItemInstance>d__.<>t__builder.Task;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000632F4 File Offset: 0x000614F4
		public Item GetItemInstanceDirect(int typeID)
		{
			Item result;
			if (this.itemInstances.TryGetValue(typeID, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x00063314 File Offset: 0x00061514
		private void Awake()
		{
			this.InitializeEntries();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00063344 File Offset: 0x00061544
		private void InitializeEntries()
		{
			StockShopDatabase.MerchantProfile merchantProfile = StockShopDatabase.Instance.GetMerchantProfile(this.merchantID);
			if (merchantProfile == null)
			{
				Debug.Log("未配置商人 " + this.merchantID);
				return;
			}
			foreach (StockShopDatabase.ItemEntry cur in merchantProfile.entries)
			{
				this.entries.Add(new StockShop.Entry(cur));
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000633CC File Offset: 0x000615CC
		private string SaveKey
		{
			get
			{
				return "StockShop_" + this.merchantID;
			}
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000633E0 File Offset: 0x000615E0
		private void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			StockShop.SaveData dataRaw = SavesSystem.Load<StockShop.SaveData>(this.SaveKey);
			this.SetupSaveData(dataRaw);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x00063410 File Offset: 0x00061610
		private void Save()
		{
			StockShop.SaveData saveData = this.GenerateSaveData() as StockShop.SaveData;
			if (saveData == null)
			{
				Debug.LogError("没法正确生成StockShop的SaveData");
				return;
			}
			SavesSystem.Save<StockShop.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00063443 File Offset: 0x00061643
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x00063468 File Offset: 0x00061668
		private void Start()
		{
			this.CacheItemInstances().Forget();
			if (this.refreshStockOnStart)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000634A4 File Offset: 0x000616A4
		private UniTask CacheItemInstances()
		{
			StockShop.<CacheItemInstances>d__50 <CacheItemInstances>d__;
			<CacheItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CacheItemInstances>d__.<>4__this = this;
			<CacheItemInstances>d__.<>1__state = -1;
			<CacheItemInstances>d__.<>t__builder.Start<StockShop.<CacheItemInstances>d__50>(ref <CacheItemInstances>d__);
			return <CacheItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000634E8 File Offset: 0x000616E8
		internal void RefreshIfNeeded()
		{
			TimeSpan t = TimeSpan.FromTicks(this.refreshAfterTimeSpan);
			DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
			if (dateTime > DateTime.UtcNow)
			{
				dateTime = DateTime.UtcNow;
				this.lastTimeRefreshedStock = dateTime.ToBinary();
			}
			DateTime t2 = DateTime.UtcNow - TimeSpan.FromDays(2.0);
			if (dateTime < t2)
			{
				this.lastTimeRefreshedStock = t2.ToBinary();
			}
			if (DateTime.UtcNow - dateTime > t)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00063588 File Offset: 0x00061788
		private void DoRefreshStock()
		{
			bool advancedDebuffMode = LevelManager.Rule.AdvancedDebuffMode;
			foreach (StockShop.Entry entry in this.entries)
			{
				if (entry.Possibility > 0f && entry.Possibility < 1f && UnityEngine.Random.Range(0f, 1f) > entry.Possibility)
				{
					entry.Show = false;
					entry.CurrentStock = 0;
				}
				else
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(entry.ItemTypeID);
					if (!advancedDebuffMode && metaData.tags.Contains(GameplayDataSettings.Tags.AdvancedDebuffMode))
					{
						entry.Show = false;
						entry.CurrentStock = 0;
					}
					else
					{
						entry.Show = true;
						entry.CurrentStock = entry.MaxStock;
					}
				}
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x00063670 File Offset: 0x00061870
		public bool Busy
		{
			get
			{
				return this.buying || this.selling;
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00063688 File Offset: 0x00061888
		public UniTask<bool> Buy(int itemTypeID, int amount = 1)
		{
			StockShop.<Buy>d__57 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.itemTypeID = itemTypeID;
			<Buy>d__.amount = amount;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<StockShop.<Buy>d__57>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000636DC File Offset: 0x000618DC
		private UniTask<bool> BuyTask(int itemTypeID, int amount = 1)
		{
			StockShop.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.amount = amount;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShop.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x00063730 File Offset: 0x00061930
		internal UniTask Sell(Item target)
		{
			StockShop.<Sell>d__59 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.target = target;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<StockShop.<Sell>d__59>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0006377B File Offset: 0x0006197B
		public void ShowUI()
		{
			if (!StockShopView.Instance)
			{
				return;
			}
			this.RefreshIfNeeded();
			StockShopView.Instance.SetupAndShow(this);
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0006379C File Offset: 0x0006199C
		public int ConvertPrice(Item item, bool selling = false)
		{
			int num = item.GetTotalRawValue();
			if (!selling)
			{
				StockShop.Entry entry = this.entries.Find((StockShop.Entry e) => e != null && e.ItemTypeID == item.TypeID);
				if (entry != null)
				{
					num = Mathf.FloorToInt((float)num * entry.PriceFactor);
				}
			}
			if (selling)
			{
				float factor = this.sellFactor;
				StockShop.OverrideSellingPriceEntry overrideSellingPriceEntry = this.overrideSellingPrice.Find((StockShop.OverrideSellingPriceEntry e) => e.typeID == item.TypeID);
				if (overrideSellingPriceEntry != null)
				{
					factor = overrideSellingPriceEntry.factor;
				}
				return Mathf.FloorToInt((float)num * factor);
			}
			return num;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0006382C File Offset: 0x00061A2C
		public object GenerateSaveData()
		{
			StockShop.SaveData saveData = new StockShop.SaveData();
			saveData.lastTimeRefreshedStock = this.lastTimeRefreshedStock;
			foreach (StockShop.Entry entry in this.entries)
			{
				saveData.stockCounts.Add(new StockShop.SaveData.StockCountEntry
				{
					itemTypeID = entry.ItemTypeID,
					stock = entry.CurrentStock
				});
			}
			return saveData;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000638B4 File Offset: 0x00061AB4
		public void SetupSaveData(object dataRaw)
		{
			StockShop.SaveData saveData = dataRaw as StockShop.SaveData;
			if (saveData == null)
			{
				return;
			}
			this.lastTimeRefreshedStock = saveData.lastTimeRefreshedStock;
			using (List<StockShop.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StockShop.Entry cur = enumerator.Current;
					StockShop.SaveData.StockCountEntry stockCountEntry = saveData.stockCounts.Find((StockShop.SaveData.StockCountEntry e) => e != null && e.itemTypeID == cur.ItemTypeID);
					if (stockCountEntry != null)
					{
						cur.Show = (stockCountEntry.stock > 0);
						cur.CurrentStock = stockCountEntry.stock;
					}
				}
			}
		}

		// Token: 0x0400135E RID: 4958
		[SerializeField]
		private string merchantID = "Albert";

		// Token: 0x0400135F RID: 4959
		[LocalizationKey("Default")]
		public string DisplayNameKey;

		// Token: 0x04001360 RID: 4960
		[TimeSpan]
		[SerializeField]
		private long refreshAfterTimeSpan;

		// Token: 0x04001361 RID: 4961
		[SerializeField]
		private string purchaseNotificationTextFormatKey = "UI_StockShop_PurchasedNotification";

		// Token: 0x04001362 RID: 4962
		[SerializeField]
		private bool accountAvaliable;

		// Token: 0x04001363 RID: 4963
		[SerializeField]
		private bool returnCash;

		// Token: 0x04001364 RID: 4964
		[SerializeField]
		private bool refreshStockOnStart;

		// Token: 0x04001365 RID: 4965
		public float sellFactor = 0.5f;

		// Token: 0x04001366 RID: 4966
		public List<StockShop.Entry> entries = new List<StockShop.Entry>();

		// Token: 0x04001367 RID: 4967
		public List<StockShop.OverrideSellingPriceEntry> overrideSellingPrice = new List<StockShop.OverrideSellingPriceEntry>();

		// Token: 0x0400136B RID: 4971
		[DateTime]
		[SerializeField]
		private long lastTimeRefreshedStock;

		// Token: 0x0400136C RID: 4972
		private Dictionary<int, Item> itemInstances = new Dictionary<int, Item>();

		// Token: 0x0400136D RID: 4973
		private bool buying;

		// Token: 0x0400136E RID: 4974
		private bool selling;

		// Token: 0x020005D0 RID: 1488
		public class Entry
		{
			// Token: 0x06002974 RID: 10612 RVA: 0x00099C8A File Offset: 0x00097E8A
			public Entry(StockShopDatabase.ItemEntry cur)
			{
				this.entry = cur;
			}

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x06002975 RID: 10613 RVA: 0x00099CA0 File Offset: 0x00097EA0
			public int MaxStock
			{
				get
				{
					if (this.entry.maxStock < 1)
					{
						this.entry.maxStock = 1;
					}
					return this.entry.maxStock;
				}
			}

			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x06002976 RID: 10614 RVA: 0x00099CC7 File Offset: 0x00097EC7
			public int ItemTypeID
			{
				get
				{
					return this.entry.typeID;
				}
			}

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x06002977 RID: 10615 RVA: 0x00099CD4 File Offset: 0x00097ED4
			public bool ForceUnlock
			{
				get
				{
					return (!GameMetaData.Instance.IsDemo || !this.entry.lockInDemo) && this.entry.forceUnlock;
				}
			}

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x06002978 RID: 10616 RVA: 0x00099CFC File Offset: 0x00097EFC
			public float PriceFactor
			{
				get
				{
					return this.entry.priceFactor;
				}
			}

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06002979 RID: 10617 RVA: 0x00099D09 File Offset: 0x00097F09
			public float Possibility
			{
				get
				{
					return this.entry.possibility;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x0600297A RID: 10618 RVA: 0x00099D16 File Offset: 0x00097F16
			// (set) Token: 0x0600297B RID: 10619 RVA: 0x00099D1E File Offset: 0x00097F1E
			public bool Show
			{
				get
				{
					return this.show;
				}
				set
				{
					this.show = value;
				}
			}

			// Token: 0x1700079A RID: 1946
			// (get) Token: 0x0600297C RID: 10620 RVA: 0x00099D27 File Offset: 0x00097F27
			// (set) Token: 0x0600297D RID: 10621 RVA: 0x00099D2F File Offset: 0x00097F2F
			public int CurrentStock
			{
				get
				{
					return this.currentStock;
				}
				set
				{
					this.currentStock = value;
					Action<StockShop.Entry> action = this.onStockChanged;
					if (action == null)
					{
						return;
					}
					action(this);
				}
			}

			// Token: 0x140000FF RID: 255
			// (add) Token: 0x0600297E RID: 10622 RVA: 0x00099D4C File Offset: 0x00097F4C
			// (remove) Token: 0x0600297F RID: 10623 RVA: 0x00099D84 File Offset: 0x00097F84
			public event Action<StockShop.Entry> onStockChanged;

			// Token: 0x040020DD RID: 8413
			private StockShopDatabase.ItemEntry entry;

			// Token: 0x040020DE RID: 8414
			[SerializeField]
			private bool show = true;

			// Token: 0x040020DF RID: 8415
			[SerializeField]
			private int currentStock;
		}

		// Token: 0x020005D1 RID: 1489
		[Serializable]
		public class OverrideSellingPriceEntry
		{
			// Token: 0x040020E1 RID: 8417
			[ItemTypeID]
			public int typeID;

			// Token: 0x040020E2 RID: 8418
			public float factor = 0.5f;
		}

		// Token: 0x020005D2 RID: 1490
		[Serializable]
		private class SaveData
		{
			// Token: 0x040020E3 RID: 8419
			[DateTime]
			public long lastTimeRefreshedStock;

			// Token: 0x040020E4 RID: 8420
			public List<StockShop.SaveData.StockCountEntry> stockCounts = new List<StockShop.SaveData.StockCountEntry>();

			// Token: 0x02000684 RID: 1668
			public class StockCountEntry
			{
				// Token: 0x0400238F RID: 9103
				public int itemTypeID;

				// Token: 0x04002390 RID: 9104
				public int stock;
			}
		}
	}
}
