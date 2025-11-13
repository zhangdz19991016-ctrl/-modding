using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C0 RID: 192
public class WaterHUD : MonoBehaviour
{
	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001BEDB File Offset: 0x0001A0DB
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
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
		float a = this.characterMainControl.CurrentWater / this.characterMainControl.MaxWater;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
				return;
			}
			this.backgroundImage.color = this.backgroundColor;
		}
	}

	// Token: 0x040005D1 RID: 1489
	private CharacterMainControl characterMainControl;

	// Token: 0x040005D2 RID: 1490
	private float percent = -1f;

	// Token: 0x040005D3 RID: 1491
	public ProceduralImage fillImage;

	// Token: 0x040005D4 RID: 1492
	public ProceduralImage backgroundImage;

	// Token: 0x040005D5 RID: 1493
	public Color backgroundColor;

	// Token: 0x040005D6 RID: 1494
	public Color emptyBackgroundColor;
}
