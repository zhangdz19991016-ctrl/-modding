using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

namespace Duckov.Bitcoins
{
	// Token: 0x02000312 RID: 786
	public class BitcoinMiner : MonoBehaviour
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0005E5DB File Offset: 0x0005C7DB
		// (set) Token: 0x060019FA RID: 6650 RVA: 0x0005E5E2 File Offset: 0x0005C7E2
		public static BitcoinMiner Instance { get; private set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x0005E5EA File Offset: 0x0005C7EA
		private double Progress
		{
			get
			{
				return this.work;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0005E5F2 File Offset: 0x0005C7F2
		private static double K_1_12
		{
			get
			{
				if (BitcoinMiner._cached_k == null)
				{
					BitcoinMiner._cached_k = new double?((BitcoinMiner.wps_12 - BitcoinMiner.wps_1) / 11.0);
				}
				return BitcoinMiner._cached_k.Value;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0005E62C File Offset: 0x0005C82C
		public double WorkPerSecond
		{
			get
			{
				if (this.IsInventoryFull)
				{
					return 0.0;
				}
				if (this.cachedPerformance < 1f)
				{
					return (double)this.cachedPerformance * BitcoinMiner.wps_1;
				}
				return BitcoinMiner.wps_1 + (double)(this.cachedPerformance - 1f) * BitcoinMiner.K_1_12;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0005E67F File Offset: 0x0005C87F
		public double HoursPerCoin
		{
			get
			{
				return this.workPerCoin / 3600.0 / this.WorkPerSecond;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0005E698 File Offset: 0x0005C898
		public bool IsInventoryFull
		{
			get
			{
				return !(this.item == null) && this.item.Inventory.GetFirstEmptyPosition(0) < 0;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x0005E6BE File Offset: 0x0005C8BE
		public TimeSpan TimePerCoin
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds(this.workPerCoin / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0005E6E9 File Offset: 0x0005C8E9
		public TimeSpan RemainingTime
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds((this.workPerCoin - this.work) / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0005E71C File Offset: 0x0005C91C
		// (set) Token: 0x06001A03 RID: 6659 RVA: 0x0005E761 File Offset: 0x0005C961
		private DateTime LastUpdateDateTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastUpdateDateTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					this.lastUpdateDateTimeRaw = DateTime.UtcNow.ToBinary();
					dateTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0005E770 File Offset: 0x0005C970
		private void Awake()
		{
			if (BitcoinMiner.Instance != null)
			{
				Debug.LogError("存在多个BitcoinMiner");
				return;
			}
			BitcoinMiner.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0005E7A1 File Offset: 0x0005C9A1
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005E7B4 File Offset: 0x0005C9B4
		private void Start()
		{
			this.Load();
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0005E7BC File Offset: 0x0005C9BC
		// (set) Token: 0x06001A08 RID: 6664 RVA: 0x0005E7C4 File Offset: 0x0005C9C4
		public bool Loading { get; private set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0005E7CD File Offset: 0x0005C9CD
		// (set) Token: 0x06001A0A RID: 6666 RVA: 0x0005E7D5 File Offset: 0x0005C9D5
		public bool Initialized { get; private set; }

		// Token: 0x06001A0B RID: 6667 RVA: 0x0005E7E0 File Offset: 0x0005C9E0
		private UniTask Setup(BitcoinMiner.SaveData data)
		{
			BitcoinMiner.<Setup>d__43 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.data = data;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<BitcoinMiner.<Setup>d__43>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0005E82C File Offset: 0x0005CA2C
		private UniTask Initialize()
		{
			BitcoinMiner.<Initialize>d__44 <Initialize>d__;
			<Initialize>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Initialize>d__.<>4__this = this;
			<Initialize>d__.<>1__state = -1;
			<Initialize>d__.<>t__builder.Start<BitcoinMiner.<Initialize>d__44>(ref <Initialize>d__);
			return <Initialize>d__.<>t__builder.Task;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0005E870 File Offset: 0x0005CA70
		private void Load()
		{
			if (SavesSystem.KeyExisits("BitcoinMiner_Data"))
			{
				BitcoinMiner.SaveData data = SavesSystem.Load<BitcoinMiner.SaveData>("BitcoinMiner_Data");
				this.Setup(data).Forget();
				return;
			}
			this.Initialize().Forget();
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0005E8AC File Offset: 0x0005CAAC
		private void Save()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			BitcoinMiner.SaveData value = new BitcoinMiner.SaveData
			{
				itemData = ItemTreeData.FromItem(this.item),
				work = this.work,
				lastUpdateDateTimeRaw = this.lastUpdateDateTimeRaw,
				cachedPerformance = this.cachedPerformance
			};
			SavesSystem.Save<BitcoinMiner.SaveData>("BitcoinMiner_Data", value);
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0005E91C File Offset: 0x0005CB1C
		private void UpdateWork()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			double totalSeconds = (DateTime.UtcNow - this.LastUpdateDateTime).TotalSeconds;
			double num = this.WorkPerSecond * totalSeconds;
			bool isInventoryFull = this.IsInventoryFull;
			if (this.work < 0.0)
			{
				this.work = 0.0;
			}
			this.work += num;
			if (this.work >= this.workPerCoin && !this.CreatingCoin)
			{
				if (!isInventoryFull)
				{
					this.CreateCoin().Forget();
				}
				else
				{
					this.work = this.workPerCoin;
				}
			}
			this.cachedPerformance = this.item.GetStatValue("Performance".GetHashCode());
			this.LastUpdateDateTime = DateTime.UtcNow;
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0005E9EA File Offset: 0x0005CBEA
		// (set) Token: 0x06001A11 RID: 6673 RVA: 0x0005E9F2 File Offset: 0x0005CBF2
		public bool CreatingCoin { get; private set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0005E9FB File Offset: 0x0005CBFB
		public Item Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x0005EA03 File Offset: 0x0005CC03
		public float NormalizedProgress
		{
			get
			{
				return (float)(this.work / this.workPerCoin);
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x0005EA13 File Offset: 0x0005CC13
		public double Performance
		{
			get
			{
				if (this.Item == null)
				{
					return 0.0;
				}
				return (double)this.Item.GetStatValue("Performance".GetHashCode());
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005EA44 File Offset: 0x0005CC44
		private UniTask CreateCoin()
		{
			BitcoinMiner.<CreateCoin>d__60 <CreateCoin>d__;
			<CreateCoin>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCoin>d__.<>4__this = this;
			<CreateCoin>d__.<>1__state = -1;
			<CreateCoin>d__.<>t__builder.Start<BitcoinMiner.<CreateCoin>d__60>(ref <CreateCoin>d__);
			return <CreateCoin>d__.<>t__builder.Task;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0005EA87 File Offset: 0x0005CC87
		private void FixedUpdate()
		{
			this.UpdateWork();
		}

		// Token: 0x040012C1 RID: 4801
		[SerializeField]
		[ItemTypeID]
		private int minerItemID = 397;

		// Token: 0x040012C2 RID: 4802
		[SerializeField]
		[ItemTypeID]
		private int coinItemID = 388;

		// Token: 0x040012C3 RID: 4803
		[SerializeField]
		private double workPerCoin = 1.0;

		// Token: 0x040012C4 RID: 4804
		private Item item;

		// Token: 0x040012C5 RID: 4805
		private double work;

		// Token: 0x040012C6 RID: 4806
		private static readonly double wps_1 = 2.3148148148148147E-05;

		// Token: 0x040012C7 RID: 4807
		private static readonly double wps_12 = 5.555555555555556E-05;

		// Token: 0x040012C8 RID: 4808
		private static double? _cached_k;

		// Token: 0x040012C9 RID: 4809
		[DateTime]
		private long lastUpdateDateTimeRaw;

		// Token: 0x040012CA RID: 4810
		private float cachedPerformance;

		// Token: 0x040012CD RID: 4813
		public const string SaveKey = "BitcoinMiner_Data";

		// Token: 0x040012CE RID: 4814
		private const string PerformaceStatKey = "Performance";

		// Token: 0x020005AB RID: 1451
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04002070 RID: 8304
			public ItemTreeData itemData;

			// Token: 0x04002071 RID: 8305
			public double work;

			// Token: 0x04002072 RID: 8306
			public float cachedPerformance;

			// Token: 0x04002073 RID: 8307
			public long lastUpdateDateTimeRaw;
		}
	}
}
