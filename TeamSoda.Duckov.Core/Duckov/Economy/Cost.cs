using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x0200032A RID: 810
	[Serializable]
	public struct Cost
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x00062DC1 File Offset: 0x00060FC1
		public bool Enough
		{
			get
			{
				return EconomyManager.IsEnough(this, true, true);
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00062DD0 File Offset: 0x00060FD0
		public bool IsFree
		{
			get
			{
				return this.money <= 0L && (this.items == null || this.items.Length == 0);
			}
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00062DF2 File Offset: 0x00060FF2
		public bool Pay(bool accountAvaliable = true, bool cashAvaliable = true)
		{
			return EconomyManager.Pay(this, accountAvaliable, cashAvaliable);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00062E04 File Offset: 0x00061004
		public static Cost FromString(string costDescription)
		{
			int num = 0;
			List<Cost.ItemEntry> list = new List<Cost.ItemEntry>();
			foreach (string text in costDescription.Split(',', StringSplitOptions.None))
			{
				string[] array2 = text.Split(":", StringSplitOptions.None);
				if (array2.Length != 2)
				{
					Debug.LogError("Invalid cost description: " + text + "\n" + costDescription);
				}
				else
				{
					string text2 = array2[0].Trim();
					int num2;
					if (!int.TryParse(array2[1].Trim(), out num2))
					{
						Debug.LogError("Invalid cost description: " + text);
					}
					else if (text2 == "money")
					{
						num = num2;
					}
					else
					{
						int num3 = ItemAssetsCollection.TryGetIDByName(text2);
						if (num3 <= 0)
						{
							Debug.LogError("Invalid item name " + text2);
						}
						else
						{
							list.Add(new Cost.ItemEntry
							{
								id = num3,
								amount = (long)num2
							});
						}
					}
				}
			}
			return new Cost
			{
				money = (long)num,
				items = list.ToArray()
			};
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x00062F15 File Offset: 0x00061115
		public static bool TaskPending
		{
			get
			{
				return Cost.ReturnTaskLocks.Count > 0;
			}
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00062F24 File Offset: 0x00061124
		internal UniTask Return(bool directToBuffer = false, bool toPlayerInventory = false, int amountFactor = 1, List<Item> generatedItemsBuffer = null)
		{
			Cost.<Return>d__12 <Return>d__;
			<Return>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Return>d__.<>4__this = this;
			<Return>d__.directToBuffer = directToBuffer;
			<Return>d__.toPlayerInventory = toPlayerInventory;
			<Return>d__.amountFactor = amountFactor;
			<Return>d__.generatedItemsBuffer = generatedItemsBuffer;
			<Return>d__.<>1__state = -1;
			<Return>d__.<>t__builder.Start<Cost.<Return>d__12>(ref <Return>d__);
			return <Return>d__.<>t__builder.Task;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00062F90 File Offset: 0x00061190
		public Cost(long money, [TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] ValueTuple<int, long>[] items)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00062FF7 File Offset: 0x000611F7
		public Cost(long money)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[0];
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0006300C File Offset: 0x0006120C
		public Cost([TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] params ValueTuple<int, long>[] items)
		{
			this.money = 0L;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x0400135B RID: 4955
		public long money;

		// Token: 0x0400135C RID: 4956
		public Cost.ItemEntry[] items;

		// Token: 0x0400135D RID: 4957
		private static List<object> ReturnTaskLocks = new List<object>();

		// Token: 0x020005CE RID: 1486
		[Serializable]
		public struct ItemEntry
		{
			// Token: 0x040020CD RID: 8397
			[ItemTypeID]
			public int id;

			// Token: 0x040020CE RID: 8398
			public long amount;
		}
	}
}
