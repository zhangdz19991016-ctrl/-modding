using System;
using ItemStatsSystem;

// Token: 0x02000084 RID: 132
public class HealAction : EffectAction
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060004CF RID: 1231 RVA: 0x00015F36 File Offset: 0x00014136
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00015F70 File Offset: 0x00014170
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.Health.AddHealth((float)this.healValue);
	}

	// Token: 0x04000409 RID: 1033
	private CharacterMainControl _mainControl;

	// Token: 0x0400040A RID: 1034
	public int healValue = 10;
}
