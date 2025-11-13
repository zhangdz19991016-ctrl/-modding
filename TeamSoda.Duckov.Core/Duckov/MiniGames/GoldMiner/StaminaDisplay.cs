using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AF RID: 687
	public class StaminaDisplay : MonoBehaviour
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x000536E4 File Offset: 0x000518E4
		private void FixedUpdate()
		{
			this.Refresh();
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x000536EC File Offset: 0x000518EC
		private void Refresh()
		{
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			float stamina = run.stamina;
			float value = run.maxStamina.Value;
			float value2 = run.extraStamina.Value;
			if (stamina > 0f)
			{
				float num = stamina / value;
				this.fill.fillAmount = num;
				this.fill.color = this.normalColor.Evaluate(num);
				this.text.text = string.Format("{0:0.0}", stamina);
				return;
			}
			float num2 = value2 + stamina;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			float fillAmount = num2 / value2;
			this.fill.fillAmount = fillAmount;
			this.fill.color = this.extraColor;
			this.text.text = string.Format("{0:0.00}", num2);
		}

		// Token: 0x040010A5 RID: 4261
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010A6 RID: 4262
		[SerializeField]
		private Image fill;

		// Token: 0x040010A7 RID: 4263
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040010A8 RID: 4264
		[SerializeField]
		private Gradient normalColor;

		// Token: 0x040010A9 RID: 4265
		[SerializeField]
		private Color extraColor = Color.red;
	}
}
