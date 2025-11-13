using System;
using ItemStatsSystem;

// Token: 0x02000082 RID: 130
public class CostStaminaAction : EffectAction
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00015DF0 File Offset: 0x00013FF0
	private CharacterMainControl MainControl
	{
		get
		{
			Effect master = base.Master;
			if (master == null)
			{
				return null;
			}
			Item item = master.Item;
			if (item == null)
			{
				return null;
			}
			return item.GetCharacterMainControl();
		}
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00015E0E File Offset: 0x0001400E
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.UseStamina(this.staminaCost);
	}

	// Token: 0x04000405 RID: 1029
	public float staminaCost;
}
