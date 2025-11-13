using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BD RID: 189
public class HealthHUD : MonoBehaviour
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001BBBD File Offset: 0x00019DBD
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0001BBCC File Offset: 0x00019DCC
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
		float num = this.characterMainControl.Health.MaxHealth;
		float currentHealth = this.characterMainControl.Health.CurrentHealth;
		float a = currentHealth / num;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
			}
			else
			{
				this.backgroundImage.color = this.backgroundColor;
			}
		}
		if (num != this.maxHealth || currentHealth != this.currenthealth)
		{
			this.maxHealth = num;
			this.currenthealth = currentHealth;
			this.text.text = this.currenthealth.ToString("0.#") + " / " + this.maxHealth.ToString("0.#");
		}
	}

	// Token: 0x040005BF RID: 1471
	private CharacterMainControl characterMainControl;

	// Token: 0x040005C0 RID: 1472
	private float percent = -1f;

	// Token: 0x040005C1 RID: 1473
	private float maxHealth;

	// Token: 0x040005C2 RID: 1474
	private float currenthealth;

	// Token: 0x040005C3 RID: 1475
	public ProceduralImage fillImage;

	// Token: 0x040005C4 RID: 1476
	public ProceduralImage backgroundImage;

	// Token: 0x040005C5 RID: 1477
	public Color backgroundColor;

	// Token: 0x040005C6 RID: 1478
	public Color emptyBackgroundColor;

	// Token: 0x040005C7 RID: 1479
	public TextMeshProUGUI text;
}
