using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000066 RID: 102
public class HealthSimpleBase : MonoBehaviour
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060003CC RID: 972 RVA: 0x00010D69 File Offset: 0x0000EF69
	public float HealthValue
	{
		get
		{
			return this.healthValue;
		}
	}

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060003CD RID: 973 RVA: 0x00010D74 File Offset: 0x0000EF74
	// (remove) Token: 0x060003CE RID: 974 RVA: 0x00010DAC File Offset: 0x0000EFAC
	public event Action<DamageInfo> OnHurtEvent;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060003CF RID: 975 RVA: 0x00010DE4 File Offset: 0x0000EFE4
	// (remove) Token: 0x060003D0 RID: 976 RVA: 0x00010E18 File Offset: 0x0000F018
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthHit;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060003D1 RID: 977 RVA: 0x00010E4C File Offset: 0x0000F04C
	// (remove) Token: 0x060003D2 RID: 978 RVA: 0x00010E84 File Offset: 0x0000F084
	public event Action<DamageInfo> OnDeadEvent;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060003D3 RID: 979 RVA: 0x00010EBC File Offset: 0x0000F0BC
	// (remove) Token: 0x060003D4 RID: 980 RVA: 0x00010EF0 File Offset: 0x0000F0F0
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthDead;

	// Token: 0x060003D5 RID: 981 RVA: 0x00010F23 File Offset: 0x0000F123
	private void Awake()
	{
		this.healthValue = this.maxHealthValue;
		this.dmgReceiver.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00010F50 File Offset: 0x0000F150
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (this.onlyReceiveExplosion && !dmgInfo.isExplosion)
		{
			return;
		}
		float num = 1f;
		bool flag = UnityEngine.Random.Range(0f, 1f) <= dmgInfo.critRate;
		dmgInfo.crit = (flag ? 1 : 0);
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			num = this.damageMultiplierIfNotMainCharacter;
		}
		this.healthValue -= (flag ? dmgInfo.critDamageFactor : 1f) * dmgInfo.damageValue * num;
		Action<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthHit = HealthSimpleBase.OnSimpleHealthHit;
		if (onSimpleHealthHit != null)
		{
			onSimpleHealthHit(this, dmgInfo);
		}
		if (this.healthValue <= 0f)
		{
			this.Dead(dmgInfo);
		}
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001101C File Offset: 0x0000F21C
	private void Dead(DamageInfo dmgInfo)
	{
		this.dmgReceiver.OnDead(dmgInfo);
		Action<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent != null)
		{
			onDeadEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthDead = HealthSimpleBase.OnSimpleHealthDead;
		if (onSimpleHealthDead == null)
		{
			return;
		}
		onSimpleHealthDead(this, dmgInfo);
	}

	// Token: 0x040002E5 RID: 741
	public Teams team;

	// Token: 0x040002E6 RID: 742
	public bool onlyReceiveExplosion;

	// Token: 0x040002E7 RID: 743
	public float maxHealthValue = 250f;

	// Token: 0x040002E8 RID: 744
	private float healthValue;

	// Token: 0x040002E9 RID: 745
	public DamageReceiver dmgReceiver;

	// Token: 0x040002ED RID: 749
	public float damageMultiplierIfNotMainCharacter = 1f;
}
