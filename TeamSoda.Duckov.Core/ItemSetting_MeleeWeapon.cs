using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class ItemSetting_MeleeWeapon : ItemSettingBase
{
	// Token: 0x06000816 RID: 2070 RVA: 0x00024748 File Offset: 0x00022948
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00024750 File Offset: 0x00022950
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsMeleeWeapon", true, true);
	}

	// Token: 0x0400078C RID: 1932
	public bool dealExplosionDamage;

	// Token: 0x0400078D RID: 1933
	public ElementTypes element;

	// Token: 0x0400078E RID: 1934
	[Range(0f, 1f)]
	public float buffChance;

	// Token: 0x0400078F RID: 1935
	public Buff buff;
}
