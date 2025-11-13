using System;
using ItemStatsSystem;

// Token: 0x020000EF RID: 239
public class ItemSetting_Bullet : ItemSettingBase
{
	// Token: 0x060007F4 RID: 2036 RVA: 0x00023E86 File Offset: 0x00022086
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsBullet", true, true);
	}
}
