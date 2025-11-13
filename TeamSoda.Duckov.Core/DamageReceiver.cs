using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006F RID: 111
public class DamageReceiver : MonoBehaviour
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x00012808 File Offset: 0x00010A08
	public Teams Team
	{
		get
		{
			if (!this.useSimpleHealth && this.health)
			{
				return this.health.team;
			}
			if (this.useSimpleHealth && this.simpleHealth)
			{
				return this.simpleHealth.team;
			}
			return Teams.all;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x0600042D RID: 1069 RVA: 0x00012858 File Offset: 0x00010A58
	public bool IsMainCharacter
	{
		get
		{
			return !this.useSimpleHealth && this.health && this.health.IsMainCharacterHealth;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001287C File Offset: 0x00010A7C
	public bool IsDead
	{
		get
		{
			return this.health && this.health.IsDead;
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00012898 File Offset: 0x00010A98
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("DamageReceiver");
		if (this.health)
		{
			this.health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x000128D8 File Offset: 0x00010AD8
	private void OnDestroy()
	{
		if (this.health)
		{
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00012903 File Offset: 0x00010B03
	public bool Hurt(DamageInfo damageInfo)
	{
		damageInfo.toDamageReceiver = this;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		if (this.health)
		{
			this.health.Hurt(damageInfo);
		}
		return true;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001293C File Offset: 0x00010B3C
	public bool AddBuff(Buff buffPfb, CharacterMainControl fromWho)
	{
		if (this.useSimpleHealth)
		{
			return false;
		}
		if (!this.health)
		{
			return false;
		}
		CharacterMainControl characterMainControl = this.health.TryGetCharacter();
		if (!characterMainControl)
		{
			return false;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, 0);
		return true;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00012982 File Offset: 0x00010B82
	public void OnDead(DamageInfo dmgInfo)
	{
		base.gameObject.SetActive(false);
		UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent == null)
		{
			return;
		}
		onDeadEvent.Invoke(dmgInfo);
	}

	// Token: 0x04000337 RID: 823
	public bool useSimpleHealth;

	// Token: 0x04000338 RID: 824
	public Health health;

	// Token: 0x04000339 RID: 825
	public HealthSimpleBase simpleHealth;

	// Token: 0x0400033A RID: 826
	public bool isHalfObsticle;

	// Token: 0x0400033B RID: 827
	public UnityEvent<DamageInfo> OnHurtEvent;

	// Token: 0x0400033C RID: 828
	public UnityEvent<DamageInfo> OnDeadEvent;
}
