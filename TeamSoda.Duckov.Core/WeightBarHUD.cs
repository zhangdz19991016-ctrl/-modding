using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D3 RID: 211
public class WeightBarHUD : MonoBehaviour
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001DA2B File Offset: 0x0001BC2B
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0001DA38 File Offset: 0x0001BC38
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
		}
		float totalWeight = this.characterMainControl.CharacterItem.TotalWeight;
		float a = this.characterMainControl.MaxWeight;
		if (!Mathf.Approximately(totalWeight, this.weight) || !Mathf.Approximately(a, this.maxWeight))
		{
			this.weight = totalWeight;
			this.maxWeight = a;
			this.percent = this.weight / this.maxWeight;
			this.weightText.text = string.Format(this.weightTextFormat, this.weight, this.maxWeight);
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0001DB10 File Offset: 0x0001BD10
	private void SetColor()
	{
		Color color;
		if (this.percent < 0.25f)
		{
			color = this.lightColor;
		}
		else if (this.percent < 0.75f)
		{
			color = this.normalColor;
		}
		else if (this.percent < 1f)
		{
			color = this.heavyColor;
		}
		else
		{
			color = this.overWeightColor;
		}
		float h;
		float num;
		float v;
		Color.RGBToHSV(color, out h, out num, out v);
		Color color2 = color;
		if (num > 0.4f)
		{
			num = 0.4f;
			v = 1f;
			color2 = Color.HSVToRGB(h, num, v);
		}
		this.glow.Color = color;
		this.fillImage.color = color2;
		this.weightText.color = color;
	}

	// Token: 0x04000651 RID: 1617
	private CharacterMainControl characterMainControl;

	// Token: 0x04000652 RID: 1618
	private float percent;

	// Token: 0x04000653 RID: 1619
	private float weight;

	// Token: 0x04000654 RID: 1620
	private float maxWeight;

	// Token: 0x04000655 RID: 1621
	public ProceduralImage fillImage;

	// Token: 0x04000656 RID: 1622
	public TrueShadow glow;

	// Token: 0x04000657 RID: 1623
	public Color lightColor;

	// Token: 0x04000658 RID: 1624
	public Color normalColor;

	// Token: 0x04000659 RID: 1625
	public Color heavyColor;

	// Token: 0x0400065A RID: 1626
	public Color overWeightColor;

	// Token: 0x0400065B RID: 1627
	public TextMeshProUGUI weightText;

	// Token: 0x0400065C RID: 1628
	public string weightTextFormat = "{0:0.#}/{1:0.#}kg";
}
