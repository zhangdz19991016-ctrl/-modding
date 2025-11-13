using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.BlackMarkets
{
	// Token: 0x0200030C RID: 780
	public class BlackMarket : MonoBehaviour
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0005D1EB File Offset: 0x0005B3EB
		// (set) Token: 0x06001987 RID: 6535 RVA: 0x0005D1F2 File Offset: 0x0005B3F2
		public static BlackMarket Instance { get; private set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0005D1FA File Offset: 0x0005B3FA
		// (set) Token: 0x06001989 RID: 6537 RVA: 0x0005D20D File Offset: 0x0005B40D
		public int RefreshChance
		{
			get
			{
				return Mathf.Min(this.refreshChance, this.MaxRefreshChance);
			}
			set
			{
				this.refreshChance = value;
				Action<BlackMarket> action = BlackMarket.onRefreshChanceChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x0600198A RID: 6538 RVA: 0x0005D228 File Offset: 0x0005B428
		// (remove) Token: 0x0600198B RID: 6539 RVA: 0x0005D25C File Offset: 0x0005B45C
		public static event Action<BlackMarket> onRefreshChanceChanged;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x0600198C RID: 6540 RVA: 0x0005D290 File Offset: 0x0005B490
		// (remove) Token: 0x0600198D RID: 6541 RVA: 0x0005D2C4 File Offset: 0x0005B4C4
		public static event Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> onRequestMaxRefreshChance;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x0600198E RID: 6542 RVA: 0x0005D2F8 File Offset: 0x0005B4F8
		// (remove) Token: 0x0600198F RID: 6543 RVA: 0x0005D32C File Offset: 0x0005B52C
		public static event Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> onRequestRefreshTime;

		// Token: 0x06001990 RID: 6544 RVA: 0x0005D35F File Offset: 0x0005B55F
		public static void NotifyMaxRefreshChanceChanged()
		{
			BlackMarket.dirty = true;
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x0005D368 File Offset: 0x0005B568
		public int MaxRefreshChance
		{
			get
			{
				if (BlackMarket.dirty)
				{
					BlackMarket.OnRequestMaxRefreshChanceEventContext onRequestMaxRefreshChanceEventContext = new BlackMarket.OnRequestMaxRefreshChanceEventContext();
					onRequestMaxRefreshChanceEventContext.Add(1);
					Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> action = BlackMarket.onRequestMaxRefreshChance;
					if (action != null)
					{
						action(onRequestMaxRefreshChanceEventContext);
					}
					this.cachedMaxRefreshChance = onRequestMaxRefreshChanceEventContext.Value;
				}
				return this.cachedMaxRefreshChance;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x0005D3AC File Offset: 0x0005B5AC
		private TimeSpan TimeToRefresh
		{
			get
			{
				BlackMarket.OnRequestRefreshTimeFactorEventContext onRequestRefreshTimeFactorEventContext = new BlackMarket.OnRequestRefreshTimeFactorEventContext();
				Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> action = BlackMarket.onRequestRefreshTime;
				if (action != null)
				{
					action(onRequestRefreshTimeFactorEventContext);
				}
				float num = Mathf.Max(onRequestRefreshTimeFactorEventContext.Value, 0.01f);
				return TimeSpan.FromTicks((long)((float)this.timeToRefresh * num));
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0005D3F0 File Offset: 0x0005B5F0
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x0005D3FD File Offset: 0x0005B5FD
		private DateTime LastRefreshedTime
		{
			get
			{
				return DateTime.FromBinary(this.lastRefreshedTimeRaw);
			}
			set
			{
				this.lastRefreshedTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x0005D40C File Offset: 0x0005B60C
		private TimeSpan TimeSinceLastRefreshedTime
		{
			get
			{
				if (DateTime.UtcNow < this.LastRefreshedTime)
				{
					this.LastRefreshedTime = DateTime.UtcNow;
				}
				return DateTime.UtcNow - this.LastRefreshedTime;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x0005D43B File Offset: 0x0005B63B
		public TimeSpan RemainingTimeBeforeRefresh
		{
			get
			{
				return this.TimeToRefresh - this.TimeSinceLastRefreshedTime;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x0005D44E File Offset: 0x0005B64E
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Demands
		{
			get
			{
				if (this._demands_readonly == null)
				{
					this._demands_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.demands);
				}
				return this._demands_readonly;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x0005D46F File Offset: 0x0005B66F
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Supplies
		{
			get
			{
				if (this._supplies_readonly == null)
				{
					this._supplies_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.supplies);
				}
				return this._supplies_readonly;
			}
		}

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06001999 RID: 6553 RVA: 0x0005D490 File Offset: 0x0005B690
		// (remove) Token: 0x0600199A RID: 6554 RVA: 0x0005D4C8 File Offset: 0x0005B6C8
		public event Action onAfterGenerateEntries;

		// Token: 0x0600199B RID: 6555 RVA: 0x0005D500 File Offset: 0x0005B700
		private ItemFilter ContructRandomFilter()
		{
			Tag random = this.tags.GetRandom(0f);
			int random2 = this.qualities.GetRandom(0f);
			if (GameMetaData.Instance.IsDemo)
			{
				this.excludeTags.Add(GameplayDataSettings.Tags.LockInDemoTag);
			}
			return new ItemFilter
			{
				requireTags = new Tag[]
				{
					random
				},
				excludeTags = this.excludeTags.ToArray(),
				minQuality = random2,
				maxQuality = random2
			};
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0005D58C File Offset: 0x0005B78C
		public UniTask<bool> Buy(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Buy>d__59 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.entry = entry;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<BlackMarket.<Buy>d__59>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0005D5D8 File Offset: 0x0005B7D8
		public UniTask<bool> Sell(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Sell>d__60 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.entry = entry;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<BlackMarket.<Sell>d__60>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0005D624 File Offset: 0x0005B824
		private void GenerateDemandsAndSupplies()
		{
			this.demands.Clear();
			this.supplies.Clear();
			int num = 0;
			for (int i = 0; i < this.demandsCount; i++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建需求失败。尝试次数超过100次。");
					break;
				}
				int[] array = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array.Length == 0)
				{
					i--;
				}
				else
				{
					int random = array.GetRandom<int>();
					ItemAssetsCollection.GetMetaData(random);
					int random2 = this.demandAmountRand.GetRandom(0f);
					float random3 = this.demandFactorRand.GetRandom(0f);
					int random4 = this.demandBatchCountRand.GetRandom(0f);
					BlackMarket.DemandSupplyEntry item = new BlackMarket.DemandSupplyEntry
					{
						itemID = random,
						remaining = random2,
						priceFactor = random3,
						batchCount = random4
					};
					this.demands.Add(item);
				}
			}
			num = 0;
			for (int j = 0; j < this.suppliesCount; j++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建供应失败。尝试次数超过100次。");
					break;
				}
				int[] array2 = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array2.Length == 0)
				{
					j--;
				}
				else
				{
					int candidate = array2.GetRandom<int>();
					if (this.demands.Any((BlackMarket.DemandSupplyEntry e) => e.ItemID == candidate))
					{
						j--;
					}
					else
					{
						ItemAssetsCollection.GetMetaData(candidate);
						int random5 = this.supplyAmountRand.GetRandom(0f);
						float random6 = this.supplyFactorRand.GetRandom(0f);
						int random7 = this.supplyBatchCountRand.GetRandom(0f);
						BlackMarket.DemandSupplyEntry item2 = new BlackMarket.DemandSupplyEntry
						{
							itemID = candidate,
							remaining = random5,
							priceFactor = random6,
							batchCount = random7
						};
						this.supplies.Add(item2);
					}
				}
			}
			Action action = this.onAfterGenerateEntries;
			if (action != null)
			{
				action();
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LevelManager.Instance.SaveMainCharacter();
			SavesSystem.CollectSaveData();
			SavesSystem.SaveFile(true);
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0005D838 File Offset: 0x0005BA38
		public void PayAndRegenerate()
		{
			if (this.RefreshChance <= 0)
			{
				return;
			}
			int num = this.RefreshChance;
			this.RefreshChance = num - 1;
			this.GenerateDemandsAndSupplies();
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0005D868 File Offset: 0x0005BA68
		private void FixedUpdate()
		{
			if (this.RefreshChance >= this.MaxRefreshChance)
			{
				this.LastRefreshedTime = DateTime.UtcNow;
				return;
			}
			TimeSpan timeSpan = this.TimeSinceLastRefreshedTime;
			if (timeSpan > this.TimeToRefresh)
			{
				while (timeSpan > this.TimeToRefresh)
				{
					timeSpan -= this.TimeToRefresh;
					this.RefreshChance++;
					if (this.RefreshChance >= this.MaxRefreshChance)
					{
						break;
					}
				}
				if (timeSpan > TimeSpan.Zero)
				{
					this.LastRefreshedTime = DateTime.UtcNow - timeSpan;
				}
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0005D8FB File Offset: 0x0005BAFB
		private void Awake()
		{
			BlackMarket.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0005D914 File Offset: 0x0005BB14
		private void Start()
		{
			this.Load();
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0005D91C File Offset: 0x0005BB1C
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0005D930 File Offset: 0x0005BB30
		private void Save()
		{
			BlackMarket.SaveData value = new BlackMarket.SaveData(this);
			SavesSystem.Save<BlackMarket.SaveData>("BlackMarket_Data", value);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0005D950 File Offset: 0x0005BB50
		private void Load()
		{
			BlackMarket.SaveData saveData = SavesSystem.Load<BlackMarket.SaveData>("BlackMarket_Data");
			if (!saveData.valid)
			{
				this.GenerateDemandsAndSupplies();
				return;
			}
			this.demands.Clear();
			this.demands.AddRange(saveData.demands);
			this.supplies.Clear();
			this.supplies.AddRange(saveData.supplies);
			this.lastRefreshedTimeRaw = saveData.lastRefreshedTimeRaw;
			this.refreshChance = saveData.refreshChance;
		}

		// Token: 0x0400127E RID: 4734
		[SerializeField]
		private int demandsCount = 3;

		// Token: 0x0400127F RID: 4735
		[SerializeField]
		private int suppliesCount = 3;

		// Token: 0x04001280 RID: 4736
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x04001281 RID: 4737
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x04001282 RID: 4738
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001283 RID: 4739
		[SerializeField]
		private RandomContainer<int> demandAmountRand;

		// Token: 0x04001284 RID: 4740
		[SerializeField]
		private RandomContainer<float> demandFactorRand;

		// Token: 0x04001285 RID: 4741
		[SerializeField]
		private RandomContainer<int> demandBatchCountRand;

		// Token: 0x04001286 RID: 4742
		[SerializeField]
		private RandomContainer<int> supplyAmountRand;

		// Token: 0x04001287 RID: 4743
		[SerializeField]
		private RandomContainer<float> supplyFactorRand;

		// Token: 0x04001288 RID: 4744
		[SerializeField]
		private RandomContainer<int> supplyBatchCountRand;

		// Token: 0x04001289 RID: 4745
		[SerializeField]
		[TimeSpan]
		private long timeToRefresh;

		// Token: 0x0400128A RID: 4746
		[SerializeField]
		private int refreshChance;

		// Token: 0x0400128E RID: 4750
		private static bool dirty = true;

		// Token: 0x0400128F RID: 4751
		private int cachedMaxRefreshChance = -1;

		// Token: 0x04001290 RID: 4752
		[DateTime]
		private long lastRefreshedTimeRaw;

		// Token: 0x04001291 RID: 4753
		private List<BlackMarket.DemandSupplyEntry> demands = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x04001292 RID: 4754
		private List<BlackMarket.DemandSupplyEntry> supplies = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x04001293 RID: 4755
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _demands_readonly;

		// Token: 0x04001294 RID: 4756
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _supplies_readonly;

		// Token: 0x04001296 RID: 4758
		private const string SaveKey = "BlackMarket_Data";

		// Token: 0x020005A3 RID: 1443
		public class OnRequestMaxRefreshChanceEventContext
		{
			// Token: 0x1700077C RID: 1916
			// (get) Token: 0x06002902 RID: 10498 RVA: 0x000982EA File Offset: 0x000964EA
			public int Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06002903 RID: 10499 RVA: 0x000982F2 File Offset: 0x000964F2
			public void Add(int count = 1)
			{
				this.value += count;
			}

			// Token: 0x04002055 RID: 8277
			private int value;
		}

		// Token: 0x020005A4 RID: 1444
		public class OnRequestRefreshTimeFactorEventContext
		{
			// Token: 0x1700077D RID: 1917
			// (get) Token: 0x06002905 RID: 10501 RVA: 0x0009830A File Offset: 0x0009650A
			public float Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06002906 RID: 10502 RVA: 0x00098312 File Offset: 0x00096512
			public void Add(float count = -0.1f)
			{
				this.value += count;
			}

			// Token: 0x04002056 RID: 8278
			private float value = 1f;
		}

		// Token: 0x020005A5 RID: 1445
		[Serializable]
		public class DemandSupplyEntry
		{
			// Token: 0x1700077E RID: 1918
			// (get) Token: 0x06002908 RID: 10504 RVA: 0x00098335 File Offset: 0x00096535
			public int ItemID
			{
				get
				{
					return this.itemID;
				}
			}

			// Token: 0x1700077F RID: 1919
			// (get) Token: 0x06002909 RID: 10505 RVA: 0x0009833D File Offset: 0x0009653D
			internal ItemMetaData ItemMetaData
			{
				get
				{
					return ItemAssetsCollection.GetMetaData(this.itemID);
				}
			}

			// Token: 0x17000780 RID: 1920
			// (get) Token: 0x0600290A RID: 10506 RVA: 0x0009834A File Offset: 0x0009654A
			public int Remaining
			{
				get
				{
					return this.remaining;
				}
			}

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x0600290B RID: 10507 RVA: 0x00098352 File Offset: 0x00096552
			public int TotalPrice
			{
				get
				{
					return Mathf.FloorToInt((float)this.ItemMetaData.priceEach * this.priceFactor * (float)this.ItemMetaData.defaultStackCount * (float)this.batchCount);
				}
			}

			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x0600290C RID: 10508 RVA: 0x00098381 File Offset: 0x00096581
			public Cost BuyCost
			{
				get
				{
					return new Cost((long)this.TotalPrice);
				}
			}

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x0600290D RID: 10509 RVA: 0x0009838F File Offset: 0x0009658F
			public Cost SellCost
			{
				get
				{
					return new Cost(new ValueTuple<int, long>[]
					{
						new ValueTuple<int, long>(this.ItemMetaData.id, (long)(this.ItemMetaData.defaultStackCount * this.batchCount))
					});
				}
			}

			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x0600290E RID: 10510 RVA: 0x000983C8 File Offset: 0x000965C8
			public string ItemDisplayName
			{
				get
				{
					return this.ItemMetaData.DisplayName;
				}
			}

			// Token: 0x140000FE RID: 254
			// (add) Token: 0x0600290F RID: 10511 RVA: 0x000983E4 File Offset: 0x000965E4
			// (remove) Token: 0x06002910 RID: 10512 RVA: 0x0009841C File Offset: 0x0009661C
			public event Action<BlackMarket.DemandSupplyEntry> onChanged;

			// Token: 0x06002911 RID: 10513 RVA: 0x00098451 File Offset: 0x00096651
			internal void NotifyChange()
			{
				Action<BlackMarket.DemandSupplyEntry> action = this.onChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}

			// Token: 0x04002057 RID: 8279
			[SerializeField]
			[ItemTypeID]
			internal int itemID;

			// Token: 0x04002058 RID: 8280
			[SerializeField]
			internal int remaining;

			// Token: 0x04002059 RID: 8281
			[SerializeField]
			internal float priceFactor;

			// Token: 0x0400205A RID: 8282
			[SerializeField]
			internal int batchCount;
		}

		// Token: 0x020005A6 RID: 1446
		[Serializable]
		public struct SaveData
		{
			// Token: 0x06002913 RID: 10515 RVA: 0x0009846C File Offset: 0x0009666C
			public SaveData(BlackMarket blackMarket)
			{
				this.valid = true;
				this.lastRefreshedTimeRaw = blackMarket.lastRefreshedTimeRaw;
				this.demands = blackMarket.demands.ToArray();
				this.supplies = blackMarket.supplies.ToArray();
				this.refreshChance = blackMarket.refreshChance;
			}

			// Token: 0x0400205C RID: 8284
			public bool valid;

			// Token: 0x0400205D RID: 8285
			public long lastRefreshedTimeRaw;

			// Token: 0x0400205E RID: 8286
			public int refreshChance;

			// Token: 0x0400205F RID: 8287
			public BlackMarket.DemandSupplyEntry[] demands;

			// Token: 0x04002060 RID: 8288
			public BlackMarket.DemandSupplyEntry[] supplies;
		}
	}
}
