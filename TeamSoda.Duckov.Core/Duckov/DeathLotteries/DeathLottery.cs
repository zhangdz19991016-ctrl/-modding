using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000307 RID: 775
	public class DeathLottery : MonoBehaviour
	{
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06001941 RID: 6465 RVA: 0x0005C56C File Offset: 0x0005A76C
		// (remove) Token: 0x06001942 RID: 6466 RVA: 0x0005C5A0 File Offset: 0x0005A7A0
		public static event Action<DeathLottery> OnRequestUI;

		// Token: 0x06001943 RID: 6467 RVA: 0x0005C5D3 File Offset: 0x0005A7D3
		public void RequestUI()
		{
			Action<DeathLottery> onRequestUI = DeathLottery.OnRequestUI;
			if (onRequestUI == null)
			{
				return;
			}
			onRequestUI(this);
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x0005C5E5 File Offset: 0x0005A7E5
		public int MaxChances
		{
			get
			{
				return this.costs.Length;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x0005C5EF File Offset: 0x0005A7EF
		public static uint CurrentDeadCharacterToken
		{
			get
			{
				return SavesSystem.Load<uint>("DeadCharacterToken");
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x0005C5FB File Offset: 0x0005A7FB
		private string SelectNotificationFormat
		{
			get
			{
				return this.selectNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x0005C608 File Offset: 0x0005A808
		public DeathLottery.OptionalCosts[] Costs
		{
			get
			{
				return this.costs;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0005C610 File Offset: 0x0005A810
		public List<Item> ItemInstances
		{
			get
			{
				return this.itemInstances;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x0005C618 File Offset: 0x0005A818
		public DeathLottery.Status CurrentStatus
		{
			get
			{
				if (this.loading)
				{
					return default(DeathLottery.Status);
				}
				if (!this.status.valid)
				{
					return default(DeathLottery.Status);
				}
				return this.status;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x0005C654 File Offset: 0x0005A854
		public int RemainingChances
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x0005C662 File Offset: 0x0005A862
		public bool Loading
		{
			get
			{
				return this.loading;
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0005C66C File Offset: 0x0005A86C
		private UniTask Load()
		{
			DeathLottery.<Load>d__31 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<DeathLottery.<Load>d__31>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0005C6B0 File Offset: 0x0005A8B0
		private UniTask LoadItemInstances()
		{
			DeathLottery.<LoadItemInstances>d__32 <LoadItemInstances>d__;
			<LoadItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadItemInstances>d__.<>4__this = this;
			<LoadItemInstances>d__.<>1__state = -1;
			<LoadItemInstances>d__.<>t__builder.Start<DeathLottery.<LoadItemInstances>d__32>(ref <LoadItemInstances>d__);
			return <LoadItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0005C6F4 File Offset: 0x0005A8F4
		private void ClearItemInstances()
		{
			for (int i = 0; i < this.itemInstances.Count; i++)
			{
				Item item = this.itemInstances[i];
				if (!(item.ParentItem != null))
				{
					item.DestroyTree();
				}
			}
			this.itemInstances.Clear();
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0005C743 File Offset: 0x0005A943
		[ContextMenu("ForceCreateNewStatus")]
		private void ForceCreateNewStatus()
		{
			if (this.Loading)
			{
				return;
			}
			this.ForceCreateNewStatusTask().Forget();
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0005C75C File Offset: 0x0005A95C
		private UniTask ForceCreateNewStatusTask()
		{
			DeathLottery.<ForceCreateNewStatusTask>d__35 <ForceCreateNewStatusTask>d__;
			<ForceCreateNewStatusTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForceCreateNewStatusTask>d__.<>4__this = this;
			<ForceCreateNewStatusTask>d__.<>1__state = -1;
			<ForceCreateNewStatusTask>d__.<>t__builder.Start<DeathLottery.<ForceCreateNewStatusTask>d__35>(ref <ForceCreateNewStatusTask>d__);
			return <ForceCreateNewStatusTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0005C7A0 File Offset: 0x0005A9A0
		private UniTask CreateNewStatus()
		{
			DeathLottery.<CreateNewStatus>d__36 <CreateNewStatus>d__;
			<CreateNewStatus>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateNewStatus>d__.<>4__this = this;
			<CreateNewStatus>d__.<>1__state = -1;
			<CreateNewStatus>d__.<>t__builder.Start<DeathLottery.<CreateNewStatus>d__36>(ref <CreateNewStatus>d__);
			return <CreateNewStatus>d__.<>t__builder.Task;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0005C7E3 File Offset: 0x0005A9E3
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0005C7F6 File Offset: 0x0005A9F6
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0005C803 File Offset: 0x0005AA03
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0005C816 File Offset: 0x0005AA16
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			SavesSystem.Save<DeathLottery.Status>("DeathLottery/status", this.status);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0005C834 File Offset: 0x0005AA34
		private UniTask<List<Item>> SelectCandidates(Item deadCharacter)
		{
			DeathLottery.<SelectCandidates>d__42 <SelectCandidates>d__;
			<SelectCandidates>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
			<SelectCandidates>d__.<>4__this = this;
			<SelectCandidates>d__.deadCharacter = deadCharacter;
			<SelectCandidates>d__.<>1__state = -1;
			<SelectCandidates>d__.<>t__builder.Start<DeathLottery.<SelectCandidates>d__42>(ref <SelectCandidates>d__);
			return <SelectCandidates>d__.<>t__builder.Task;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0005C880 File Offset: 0x0005AA80
		private bool CanBeACandidate(Item item)
		{
			if (item == null)
			{
				return false;
			}
			foreach (Tag item2 in this.excludeTags)
			{
				if (item.Tags.Contains(item2))
				{
					return false;
				}
			}
			foreach (Tag item3 in this.requireTags)
			{
				if (item.Tags.Contains(item3))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0005C8EC File Offset: 0x0005AAEC
		public UniTask<bool> Select(int index, Cost payWhenSucceed)
		{
			DeathLottery.<Select>d__44 <Select>d__;
			<Select>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Select>d__.<>4__this = this;
			<Select>d__.index = index;
			<Select>d__.payWhenSucceed = payWhenSucceed;
			<Select>d__.<>1__state = -1;
			<Select>d__.<>t__builder.Start<DeathLottery.<Select>d__44>(ref <Select>d__);
			return <Select>d__.<>t__builder.Task;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0005C940 File Offset: 0x0005AB40
		internal DeathLottery.OptionalCosts GetCost()
		{
			if (!this.status.valid)
			{
				return default(DeathLottery.OptionalCosts);
			}
			if (this.status.SelectedCount >= this.Costs.Length)
			{
				return default(DeathLottery.OptionalCosts);
			}
			return this.Costs[this.status.SelectedCount];
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0005C9B7 File Offset: 0x0005ABB7
		[CompilerGenerated]
		internal static void <Select>g__SendToPlayer|44_0(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacter(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x04001255 RID: 4693
		public const int MaxCandidateCount = 8;

		// Token: 0x04001256 RID: 4694
		[SerializeField]
		[LocalizationKey("Default")]
		private string selectNotificationFormatKey = "DeathLottery_SelectNotification";

		// Token: 0x04001257 RID: 4695
		[SerializeField]
		private Tag[] requireTags;

		// Token: 0x04001258 RID: 4696
		[SerializeField]
		private Tag[] excludeTags;

		// Token: 0x04001259 RID: 4697
		[SerializeField]
		private RandomContainer<DeathLottery.dummyItemEntry> dummyItems;

		// Token: 0x0400125A RID: 4698
		[SerializeField]
		private DeathLottery.OptionalCosts[] costs;

		// Token: 0x0400125B RID: 4699
		private DeathLottery.Status status;

		// Token: 0x0400125C RID: 4700
		private List<Item> itemInstances = new List<Item>();

		// Token: 0x0400125D RID: 4701
		private bool loading;

		// Token: 0x02000599 RID: 1433
		[Serializable]
		private struct dummyItemEntry
		{
			// Token: 0x04002024 RID: 8228
			[ItemTypeID]
			public int typeID;
		}

		// Token: 0x0200059A RID: 1434
		[Serializable]
		public struct OptionalCosts
		{
			// Token: 0x04002025 RID: 8229
			[SerializeField]
			public Cost costA;

			// Token: 0x04002026 RID: 8230
			[SerializeField]
			public bool useCostB;

			// Token: 0x04002027 RID: 8231
			[SerializeField]
			public Cost costB;
		}

		// Token: 0x0200059B RID: 1435
		[Serializable]
		public struct Status
		{
			// Token: 0x1700077B RID: 1915
			// (get) Token: 0x060028F3 RID: 10483 RVA: 0x00097637 File Offset: 0x00095837
			public int SelectedCount
			{
				get
				{
					return this.selectedItems.Count;
				}
			}

			// Token: 0x04002028 RID: 8232
			public bool valid;

			// Token: 0x04002029 RID: 8233
			public uint deadCharacterToken;

			// Token: 0x0400202A RID: 8234
			public List<int> selectedItems;

			// Token: 0x0400202B RID: 8235
			public List<ItemTreeData> candidates;
		}
	}
}
