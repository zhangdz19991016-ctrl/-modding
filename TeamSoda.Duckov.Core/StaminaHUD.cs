using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BF RID: 191
public class StaminaHUD : MonoBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001BD93 File Offset: 0x00019F93
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0001BDA0 File Offset: 0x00019FA0
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
		float a = this.characterMainControl.CurrentStamina / this.characterMainControl.MaxStamina;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
			if (Mathf.Approximately(a, 1f))
			{
				this.targetAlpha = 0f;
			}
			else
			{
				this.targetAlpha = 1f;
			}
		}
		this.UpdateAlpha(Time.unscaledDeltaTime);
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0001BE4C File Offset: 0x0001A04C
	private void SetColor()
	{
		float h;
		float s;
		float v;
		Color.RGBToHSV(this.glowColor.Evaluate(this.percent), out h, out s, out v);
		s = 0.4f;
		v = 1f;
		Color color = Color.HSVToRGB(h, s, v);
		this.fillImage.color = color;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0001BE96 File Offset: 0x0001A096
	private void UpdateAlpha(float deltaTime)
	{
		if (this.targetAlpha != this.canvasGroup.alpha)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, this.targetAlpha, 5f * deltaTime);
		}
	}

	// Token: 0x040005CB RID: 1483
	private CharacterMainControl characterMainControl;

	// Token: 0x040005CC RID: 1484
	private float percent;

	// Token: 0x040005CD RID: 1485
	public CanvasGroup canvasGroup;

	// Token: 0x040005CE RID: 1486
	private float targetAlpha;

	// Token: 0x040005CF RID: 1487
	public ProceduralImage fillImage;

	// Token: 0x040005D0 RID: 1488
	public Gradient glowColor;
}
