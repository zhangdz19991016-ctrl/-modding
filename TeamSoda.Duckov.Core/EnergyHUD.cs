using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BC RID: 188
public class EnergyHUD : MonoBehaviour
{
	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001BAF6 File Offset: 0x00019CF6
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001BB04 File Offset: 0x00019D04
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
		float a = this.characterMainControl.CurrentEnergy / this.characterMainControl.MaxEnergy;
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

	// Token: 0x040005B9 RID: 1465
	private CharacterMainControl characterMainControl;

	// Token: 0x040005BA RID: 1466
	private float percent = -1f;

	// Token: 0x040005BB RID: 1467
	public ProceduralImage fillImage;

	// Token: 0x040005BC RID: 1468
	public ProceduralImage backgroundImage;

	// Token: 0x040005BD RID: 1469
	public Color backgroundColor;

	// Token: 0x040005BE RID: 1470
	public Color emptyBackgroundColor;
}
