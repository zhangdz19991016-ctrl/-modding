using System;
using Duckov.Buffs;
using ItemStatsSystem;

// Token: 0x02000081 RID: 129
public class AddBuffAction : EffectAction
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00015DA2 File Offset: 0x00013FA2
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

	// Token: 0x060004C7 RID: 1223 RVA: 0x00015DC0 File Offset: 0x00013FC0
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.AddBuff(this.buffPfb, this.MainControl, 0);
	}

	// Token: 0x04000404 RID: 1028
	public Buff buffPfb;
}
