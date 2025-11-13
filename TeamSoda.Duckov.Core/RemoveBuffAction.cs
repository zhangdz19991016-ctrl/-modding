using System;
using ItemStatsSystem;

// Token: 0x02000086 RID: 134
public class RemoveBuffAction : EffectAction
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001615A File Offset: 0x0001435A
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

	// Token: 0x060004D8 RID: 1240 RVA: 0x00016178 File Offset: 0x00014378
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.RemoveBuff(this.buffID, this.removeOneLayer);
	}

	// Token: 0x04000414 RID: 1044
	public int buffID;

	// Token: 0x04000415 RID: 1045
	public bool removeOneLayer;
}
