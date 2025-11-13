using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029B RID: 667
	public class AddBombOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x00051FB0 File Offset: 0x000501B0
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00051FFE File Offset: 0x000501FE
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.bomb += this.amount;
		}

		// Token: 0x04001040 RID: 4160
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001041 RID: 4161
		[SerializeField]
		private int amount = 1;
	}
}
