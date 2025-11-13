using System;
using ItemStatsSystem;

// Token: 0x020000F6 RID: 246
public class ItemSetting_NightVision : ItemSettingBase
{
	// Token: 0x06000819 RID: 2073 RVA: 0x00024767 File Offset: 0x00022967
	public override void OnInit()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot += this.OnplugedIntoSlot;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0002478D File Offset: 0x0002298D
	private void OnplugedIntoSlot(Item item)
	{
		this.nightVisionOn = true;
		this.SyncModifiers();
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0002479C File Offset: 0x0002299C
	private void OnDestroy()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot -= this.OnplugedIntoSlot;
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x000247C2 File Offset: 0x000229C2
	public void ToggleNightVison()
	{
		this.nightVisionOn = !this.nightVisionOn;
		this.SyncModifiers();
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000247D9 File Offset: 0x000229D9
	private void SyncModifiers()
	{
		if (!this._item)
		{
			return;
		}
		this._item.Modifiers.ModifierEnable = this.nightVisionOn;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x000247FF File Offset: 0x000229FF
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsNightVision", true, true);
	}

	// Token: 0x04000790 RID: 1936
	private bool nightVisionOn = true;
}
