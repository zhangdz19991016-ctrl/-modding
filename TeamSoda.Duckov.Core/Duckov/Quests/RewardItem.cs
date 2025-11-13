using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033E RID: 830
	public class RewardItem : Reward
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x000681C7 File Offset: 0x000663C7
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000681CF File Offset: 0x000663CF
		public override bool Claiming
		{
			get
			{
				return this.claiming;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x000681D7 File Offset: 0x000663D7
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x00068207 File Offset: 0x00066407
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x00068214 File Offset: 0x00066414
		public override string Description
		{
			get
			{
				return string.Format("{0} x{1}", this.CachedMeta.DisplayName, this.amount);
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00068244 File Offset: 0x00066444
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00068251 File Offset: 0x00066451
		public override void SetupSaveData(object data)
		{
			this.claimed = (bool)data;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0006825F File Offset: 0x0006645F
		public override void OnClaim()
		{
			if (this.claimed)
			{
				return;
			}
			if (this.claiming)
			{
				return;
			}
			this.claiming = true;
			this.GenerateAndGiveItems().Forget();
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x00068288 File Offset: 0x00066488
		private UniTask GenerateAndGiveItems()
		{
			RewardItem.<GenerateAndGiveItems>d__18 <GenerateAndGiveItems>d__;
			<GenerateAndGiveItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<GenerateAndGiveItems>d__.<>4__this = this;
			<GenerateAndGiveItems>d__.<>1__state = -1;
			<GenerateAndGiveItems>d__.<>t__builder.Start<RewardItem.<GenerateAndGiveItems>d__18>(ref <GenerateAndGiveItems>d__);
			return <GenerateAndGiveItems>d__.<>t__builder.Task;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000682CB File Offset: 0x000664CB
		private void SendItemToPlayerStorage(Item item)
		{
			PlayerStorage.Push(item, true);
		}

		// Token: 0x04001400 RID: 5120
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x04001401 RID: 5121
		public int amount = 1;

		// Token: 0x04001402 RID: 5122
		private bool claimed;

		// Token: 0x04001403 RID: 5123
		private bool claiming;

		// Token: 0x04001404 RID: 5124
		private ItemMetaData? _cachedMeta;
	}
}
