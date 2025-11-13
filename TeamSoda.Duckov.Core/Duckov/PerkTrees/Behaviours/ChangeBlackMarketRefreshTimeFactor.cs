using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025C RID: 604
	public class ChangeBlackMarketRefreshTimeFactor : PerkBehaviour
	{
		// Token: 0x060012F6 RID: 4854 RVA: 0x00047B81 File Offset: 0x00045D81
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestRefreshTime += this.HandleEvent;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00047B9A File Offset: 0x00045D9A
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestRefreshTime -= this.HandleEvent;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00047BB3 File Offset: 0x00045DB3
		private void HandleEvent(BlackMarket.OnRequestRefreshTimeFactorEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.amount);
		}

		// Token: 0x04000E4E RID: 3662
		[SerializeField]
		private float amount = -0.1f;
	}
}
