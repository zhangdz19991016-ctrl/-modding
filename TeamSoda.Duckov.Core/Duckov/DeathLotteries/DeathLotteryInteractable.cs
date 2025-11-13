using System;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000308 RID: 776
	public class DeathLotteryInteractable : InteractableBase
	{
		// Token: 0x0600195C RID: 6492 RVA: 0x0005C9D3 File Offset: 0x0005ABD3
		protected override bool IsInteractable()
		{
			return !(this.deathLottery == null) && this.deathLottery.CurrentStatus.valid && !this.deathLottery.Loading;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0005CA09 File Offset: 0x0005AC09
		protected override void OnInteractFinished()
		{
			this.deathLottery.RequestUI();
		}

		// Token: 0x0400125E RID: 4702
		[SerializeField]
		private DeathLottery deathLottery;
	}
}
