using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A9 RID: 681
	public class LevelSettlementUI : MonoBehaviour
	{
		// Token: 0x06001649 RID: 5705 RVA: 0x00052B8C File Offset: 0x00050D8C
		internal void Reset()
		{
			if (this.clearIndicator != null)
			{
				this.clearIndicator.SetActive(false);
			}
			if (this.clearIndicator != null)
			{
				this.failIndicator.SetActive(false);
			}
			this.money = 0;
			this.score = 0;
			this.factor = 0f;
			this.RefreshTexts();
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00052BEC File Offset: 0x00050DEC
		public void SetTargetScore(int targetScore)
		{
			this.targetScore = targetScore;
			this.RefreshTexts();
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00052BFB File Offset: 0x00050DFB
		public void StepResolveEntity(GoldMinerEntity entity)
		{
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00052BFD File Offset: 0x00050DFD
		public void StepResult(bool clear)
		{
			this.clearIndicator.SetActive(clear);
			this.failIndicator.SetActive(!clear);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00052C1C File Offset: 0x00050E1C
		public void Step(int money, float factor, int score)
		{
			bool flag = money > this.money;
			bool flag2 = factor > this.factor;
			bool flag3 = score > this.score;
			this.money = money;
			this.factor = factor;
			this.score = score;
			this.RefreshTexts();
			if (flag)
			{
				this.moneyPunch.Punch();
			}
			if (flag2)
			{
				this.factorPunch.Punch();
			}
			if (flag3)
			{
				this.scorePunch.Punch();
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00052C8C File Offset: 0x00050E8C
		private void RefreshTexts()
		{
			if (this.levelText == null)
			{
				Debug.LogWarning("Text is missing, abort.");
				return;
			}
			this.levelText.text = string.Format("LEVEL {0}", this.goldMiner.run.level + 1);
			this.targetScoreText.text = string.Format("{0}", this.targetScore);
			this.moneyText.text = string.Format("${0}", this.money);
			this.factorText.text = string.Format("{0}", this.factor);
			this.scoreText.text = string.Format("{0}", this.score);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00052D5E File Offset: 0x00050F5E
		public void Show()
		{
			if (this.goldMiner.isBeingDestroyed)
			{
				return;
			}
			this.fadeGroup.Show();
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00052D79 File Offset: 0x00050F79
		public void Hide()
		{
			if (this.goldMiner.isBeingDestroyed)
			{
				return;
			}
			this.fadeGroup.Hide();
		}

		// Token: 0x04001078 RID: 4216
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x04001079 RID: 4217
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400107A RID: 4218
		[SerializeField]
		private PunchReceiver moneyPunch;

		// Token: 0x0400107B RID: 4219
		[SerializeField]
		private PunchReceiver factorPunch;

		// Token: 0x0400107C RID: 4220
		[SerializeField]
		private PunchReceiver scorePunch;

		// Token: 0x0400107D RID: 4221
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x0400107E RID: 4222
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x0400107F RID: 4223
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001080 RID: 4224
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x04001081 RID: 4225
		[SerializeField]
		private TextMeshProUGUI targetScoreText;

		// Token: 0x04001082 RID: 4226
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x04001083 RID: 4227
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x04001084 RID: 4228
		private int targetScore;

		// Token: 0x04001085 RID: 4229
		private int money;

		// Token: 0x04001086 RID: 4230
		private int score;

		// Token: 0x04001087 RID: 4231
		private float factor;
	}
}
