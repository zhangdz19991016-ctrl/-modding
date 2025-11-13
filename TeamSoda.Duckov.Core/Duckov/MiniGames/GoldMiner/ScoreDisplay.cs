using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AE RID: 686
	public class ScoreDisplay : MonoBehaviour
	{
		// Token: 0x0600167F RID: 5759 RVA: 0x0005355C File Offset: 0x0005175C
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000535B7 File Offset: 0x000517B7
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.Refresh();
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000535BF File Offset: 0x000517BF
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x000535C8 File Offset: 0x000517C8
		private void Refresh()
		{
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			int num = 0;
			float num2 = run.scoreFactorBase.Value + run.levelScoreFactor;
			int targetScore = run.targetScore;
			foreach (GoldMinerEntity goldMinerEntity in this.master.resolvedEntities)
			{
				int num3 = Mathf.CeilToInt((float)goldMinerEntity.Value * run.charm.Value);
				if (num3 != 0)
				{
					num += num3;
				}
			}
			this.moneyText.text = string.Format("${0}", num);
			this.factorText.text = string.Format("{0}", num2);
			this.scoreText.text = string.Format("{0}", Mathf.CeilToInt((float)num * num2));
			this.targetScoreText.text = string.Format("{0}", targetScore);
		}

		// Token: 0x040010A0 RID: 4256
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010A1 RID: 4257
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x040010A2 RID: 4258
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x040010A3 RID: 4259
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x040010A4 RID: 4260
		[SerializeField]
		private TextMeshProUGUI targetScoreText;
	}
}
