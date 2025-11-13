using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025B RID: 603
	public class AddBlackMarketRefreshChance : PerkBehaviour
	{
		// Token: 0x060012F1 RID: 4849 RVA: 0x00047B0E File Offset: 0x00045D0E
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestMaxRefreshChance += this.HandleEvent;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00047B27 File Offset: 0x00045D27
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestMaxRefreshChance -= this.HandleEvent;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00047B40 File Offset: 0x00045D40
		private void HandleEvent(BlackMarket.OnRequestMaxRefreshChanceEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.addAmount);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00047B6B File Offset: 0x00045D6B
		protected override void OnUnlocked()
		{
			BlackMarket.NotifyMaxRefreshChanceChanged();
		}

		// Token: 0x04000E4D RID: 3661
		[SerializeField]
		private int addAmount = 1;
	}
}
