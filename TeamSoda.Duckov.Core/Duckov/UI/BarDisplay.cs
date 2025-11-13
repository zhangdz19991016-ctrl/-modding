using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000394 RID: 916
	public class BarDisplay : MonoBehaviour
	{
		// Token: 0x06002001 RID: 8193 RVA: 0x00070374 File Offset: 0x0006E574
		private void Awake()
		{
			this.fill.fillAmount = 0f;
			this.ApplyLook();
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0007038C File Offset: 0x0006E58C
		public void Setup(string labelText, Color color, float current, float max, string format = "0.#", float min = 0f)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, format, min);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000703A3 File Offset: 0x0006E5A3
		public void Setup(string labelText, Color color, int current, int max, int min = 0)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, min);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000703B8 File Offset: 0x0006E5B8
		public void SetupLook(string labelText, Color color)
		{
			this.labelText = labelText;
			this.color = color;
			this.ApplyLook();
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000703CE File Offset: 0x0006E5CE
		private void ApplyLook()
		{
			this.text_Label.text = this.labelText.ToPlainText();
			this.fill.color = this.color;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000703F8 File Offset: 0x0006E5F8
		public void SetValue(float current, float max, string format = "0.#", float min = 0f)
		{
			this.text_Current.text = current.ToString(format);
			this.text_Max.text = max.ToString(format);
			float num = max - min;
			float endValue = 1f;
			if (num > 0f)
			{
				endValue = (current - min) / num;
			}
			this.fill.DOKill(false);
			this.fill.DOFillAmount(endValue, this.animateDuration).SetEase(Ease.OutCubic);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0007046C File Offset: 0x0006E66C
		public void SetValue(int current, int max, int min = 0)
		{
			this.text_Current.text = current.ToString();
			this.text_Max.text = max.ToString();
			int num = max - min;
			float endValue = 1f;
			if (num > 0)
			{
				endValue = (float)(current - min) / (float)num;
			}
			this.fill.DOKill(false);
			this.fill.DOFillAmount(endValue, this.animateDuration).SetEase(Ease.OutCubic);
		}

		// Token: 0x040015D3 RID: 5587
		[SerializeField]
		private string labelText;

		// Token: 0x040015D4 RID: 5588
		[SerializeField]
		private Color color = Color.red;

		// Token: 0x040015D5 RID: 5589
		[SerializeField]
		private float animateDuration = 0.25f;

		// Token: 0x040015D6 RID: 5590
		[SerializeField]
		private TextMeshProUGUI text_Label;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private TextMeshProUGUI text_Current;

		// Token: 0x040015D8 RID: 5592
		[SerializeField]
		private TextMeshProUGUI text_Max;

		// Token: 0x040015D9 RID: 5593
		[SerializeField]
		private Image fill;
	}
}
