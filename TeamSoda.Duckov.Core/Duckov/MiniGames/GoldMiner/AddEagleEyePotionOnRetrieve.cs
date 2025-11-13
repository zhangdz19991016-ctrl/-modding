using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029C RID: 668
	public class AddEagleEyePotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x00052028 File Offset: 0x00050228
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00052076 File Offset: 0x00050276
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.eagleEyePotion += this.amount;
		}

		// Token: 0x04001042 RID: 4162
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001043 RID: 4163
		[SerializeField]
		private int amount = 1;
	}
}
