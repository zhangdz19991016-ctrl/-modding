using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class LabelAndValue : MonoBehaviour
{
	// Token: 0x06000AA1 RID: 2721 RVA: 0x0002ECD0 File Offset: 0x0002CED0
	public void Setup(string label, string value, Polarity valuePolarity)
	{
		this.labelText.text = label;
		this.valueText.text = value;
		Color color = this.colorNeutral;
		switch (valuePolarity)
		{
		case Polarity.Negative:
			color = this.colorNegative;
			break;
		case Polarity.Neutral:
			color = this.colorNeutral;
			break;
		case Polarity.Positive:
			color = this.colorPositive;
			break;
		}
		this.valueText.color = color;
	}

	// Token: 0x0400094E RID: 2382
	[SerializeField]
	private TextMeshProUGUI labelText;

	// Token: 0x0400094F RID: 2383
	[SerializeField]
	private TextMeshProUGUI valueText;

	// Token: 0x04000950 RID: 2384
	[SerializeField]
	private Color colorNeutral;

	// Token: 0x04000951 RID: 2385
	[SerializeField]
	private Color colorPositive;

	// Token: 0x04000952 RID: 2386
	[SerializeField]
	private Color colorNegative;
}
