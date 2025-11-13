using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class ItemAgent_MeleeWeapon : DuckovItemAgent
{
	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x00023039 File Offset: 0x00021239
	public float Damage
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DamageHash);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002304B File Offset: 0x0002124B
	public float CritRate
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritRateHash);
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002305D File Offset: 0x0002125D
	public float CritDamageFactor
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritDamageFactorHash);
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002306F File Offset: 0x0002126F
	public float ArmorPiercing
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.ArmorPiercingHash);
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00023081 File Offset: 0x00021281
	public float AttackSpeed
	{
		get
		{
			return Mathf.Max(0.1f, base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackSpeedHash));
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0002309D File Offset: 0x0002129D
	public float AttackRange
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackRangeHash);
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x000230AF File Offset: 0x000212AF
	public float DealDamageTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DealDamageTimeHash);
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000230C1 File Offset: 0x000212C1
	public float StaminaCost
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.StaminaCostHash);
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000230D3 File Offset: 0x000212D3
	public float BleedChance
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.BleedChanceHash);
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000230E5 File Offset: 0x000212E5
	public float MoveSpeedMultiplier
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.MoveSpeedMultiplierHash);
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000230F7 File Offset: 0x000212F7
	public float CharacterDamageMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.MeleeDamageMultiplier;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00023117 File Offset: 0x00021317
	public float CharacterCritRateGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritRateGain;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00023137 File Offset: 0x00021337
	public float CharacterCritDamageGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritDamageGain;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00023157 File Offset: 0x00021357
	public string SoundKey
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this.soundKey))
			{
				return "Default";
			}
			return this.soundKey;
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00023174 File Offset: 0x00021374
	private int UpdateColliders()
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		return Physics.OverlapSphereNonAlloc(base.Holder.transform.position, this.AttackRange + 0.05f, this.colliders, GameplayDataSettings.Layers.damageReceiverLayerMask);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000231CB File Offset: 0x000213CB
	public void CheckAndDealDamage()
	{
		this.CheckCollidersInRange(true);
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000231D5 File Offset: 0x000213D5
	public bool AttackableTargetInRange()
	{
		return this.CheckCollidersInRange(false) > 0;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000231E4 File Offset: 0x000213E4
	private int CheckCollidersInRange(bool dealDamage)
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		int num = this.UpdateColliders();
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.colliders[i];
			DamageReceiver component = collider.GetComponent<DamageReceiver>();
			if (!(component == null) && Team.IsEnemy(component.Team, base.Holder.Team))
			{
				Health health = component.health;
				if (health)
				{
					CharacterMainControl characterMainControl = health.TryGetCharacter();
					if (characterMainControl == base.Holder || (characterMainControl && characterMainControl.Dashing))
					{
						goto IL_30A;
					}
				}
				Vector3 vector = collider.transform.position - base.Holder.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				vector.Normalize();
				if (Vector3.Angle(vector, base.Holder.CurrentAimDirection) < 90f || magnitude < 0.6f)
				{
					num2++;
					if (dealDamage)
					{
						DamageInfo damageInfo = new DamageInfo(base.Holder);
						damageInfo.damageValue = this.Damage * this.CharacterDamageMultiplier;
						damageInfo.armorPiercing = this.ArmorPiercing;
						damageInfo.critDamageFactor = this.CritDamageFactor * (1f + this.CharacterCritDamageGain);
						damageInfo.critRate = this.CritRate * (1f + this.CharacterCritRateGain);
						damageInfo.crit = -1;
						damageInfo.damageNormal = -base.Holder.modelRoot.right;
						damageInfo.damagePoint = collider.transform.position - vector * 0.2f;
						damageInfo.damagePoint.y = base.transform.position.y;
						damageInfo.fromWeaponItemID = base.Item.TypeID;
						damageInfo.bleedChance = this.BleedChance;
						if (this.setting)
						{
							damageInfo.isExplosion = this.setting.dealExplosionDamage;
							damageInfo.elementFactors.Add(new ElementFactor(this.setting.element, 1f));
							damageInfo.buff = this.setting.buff;
							damageInfo.buffChance = this.setting.buffChance;
						}
						component.Hurt(damageInfo);
						component.AddBuff(GameplayDataSettings.Buffs.Pain, base.Holder);
						if (this.hitFx)
						{
							UnityEngine.Object.Instantiate<GameObject>(this.hitFx, damageInfo.damagePoint, Quaternion.LookRotation(damageInfo.damageNormal, Vector3.up));
						}
						if (base.Holder && base.Holder == CharacterMainControl.Main)
						{
							Vector3 a = base.Holder.modelRoot.right;
							a += UnityEngine.Random.insideUnitSphere * 0.3f;
							a.Normalize();
							CameraShaker.Shake(a * 0.05f, CameraShaker.CameraShakeTypes.meleeAttackHit);
						}
					}
				}
			}
			IL_30A:;
		}
		return num2;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00023507 File Offset: 0x00021707
	private void Update()
	{
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00023509 File Offset: 0x00021709
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.setting = base.Item.GetComponent<ItemSetting_MeleeWeapon>();
	}

	// Token: 0x04000753 RID: 1875
	public GameObject hitFx;

	// Token: 0x04000754 RID: 1876
	public GameObject slashFx;

	// Token: 0x04000755 RID: 1877
	public float slashFxDelayTime = 0.05f;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	private string soundKey = "Default";

	// Token: 0x04000757 RID: 1879
	private Collider[] colliders;

	// Token: 0x04000758 RID: 1880
	private ItemSetting_MeleeWeapon setting;

	// Token: 0x04000759 RID: 1881
	private static int DamageHash = "Damage".GetHashCode();

	// Token: 0x0400075A RID: 1882
	private static int CritRateHash = "CritRate".GetHashCode();

	// Token: 0x0400075B RID: 1883
	private static int CritDamageFactorHash = "CritDamageFactor".GetHashCode();

	// Token: 0x0400075C RID: 1884
	private static int ArmorPiercingHash = "ArmorPiercing".GetHashCode();

	// Token: 0x0400075D RID: 1885
	private static int AttackSpeedHash = "AttackSpeed".GetHashCode();

	// Token: 0x0400075E RID: 1886
	private static int AttackRangeHash = "AttackRange".GetHashCode();

	// Token: 0x0400075F RID: 1887
	private static int DealDamageTimeHash = "DealDamageTime".GetHashCode();

	// Token: 0x04000760 RID: 1888
	private static int StaminaCostHash = "StaminaCost".GetHashCode();

	// Token: 0x04000761 RID: 1889
	private static int BleedChanceHash = "BleedChance".GetHashCode();

	// Token: 0x04000762 RID: 1890
	private static int MoveSpeedMultiplierHash = "MoveSpeedMultiplier".GetHashCode();
}
