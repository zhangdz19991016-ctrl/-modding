using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029D RID: 669
	public class AddStrengthPotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x06001612 RID: 5650 RVA: 0x000520A0 File Offset: 0x000502A0
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000520EE File Offset: 0x000502EE
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.strengthPotion += this.amount;
		}

		// Token: 0x04001044 RID: 4164
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001045 RID: 4165
		[SerializeField]
		private int amount = 1;
	}
}
