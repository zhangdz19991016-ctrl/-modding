using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Scenes;
using Duckov.Utilities;
using Duckov.Weathers;
using FX;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000065 RID: 101
public class Health : MonoBehaviour
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060003AD RID: 941 RVA: 0x00010182 File Offset: 0x0000E382
	// (set) Token: 0x060003AC RID: 940 RVA: 0x00010179 File Offset: 0x0000E379
	public bool showHealthBar
	{
		get
		{
			return this._showHealthBar;
		}
		set
		{
			this._showHealthBar = value;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060003AE RID: 942 RVA: 0x0001018A File Offset: 0x0000E38A
	public bool Hidden
	{
		get
		{
			return this.TryGetCharacter() && this.characterCached.Hidden;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060003AF RID: 943 RVA: 0x000101A8 File Offset: 0x0000E3A8
	public float MaxHealth
	{
		get
		{
			float num;
			if (this.item)
			{
				num = this.item.GetStatValue(this.maxHealthHash);
			}
			else
			{
				num = (float)this.defaultMaxHealth;
			}
			if (!Mathf.Approximately(this.lastMaxHealth, num))
			{
				this.lastMaxHealth = num;
				UnityEvent<Health> onMaxHealthChange = this.OnMaxHealthChange;
				if (onMaxHealthChange != null)
				{
					onMaxHealthChange.Invoke(this);
				}
			}
			return num;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001020C File Offset: 0x0000E40C
	public bool IsMainCharacterHealth
	{
		get
		{
			return !(LevelManager.Instance == null) && !(LevelManager.Instance.MainCharacter == null) && !(LevelManager.Instance.MainCharacter != this.TryGetCharacter());
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003B1 RID: 945 RVA: 0x0001024B File Offset: 0x0000E44B
	// (set) Token: 0x060003B2 RID: 946 RVA: 0x00010254 File Offset: 0x0000E454
	public float CurrentHealth
	{
		get
		{
			return this._currentHealth;
		}
		set
		{
			float currentHealth = this._currentHealth;
			this._currentHealth = value;
			if (this._currentHealth != currentHealth)
			{
				UnityEvent<Health> onHealthChange = this.OnHealthChange;
				if (onHealthChange == null)
				{
					return;
				}
				onHealthChange.Invoke(this);
			}
		}
	}

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x060003B3 RID: 947 RVA: 0x0001028C File Offset: 0x0000E48C
	// (remove) Token: 0x060003B4 RID: 948 RVA: 0x000102C0 File Offset: 0x0000E4C0
	public static event Action<Health, DamageInfo> OnHurt;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060003B5 RID: 949 RVA: 0x000102F4 File Offset: 0x0000E4F4
	// (remove) Token: 0x060003B6 RID: 950 RVA: 0x00010328 File Offset: 0x0000E528
	public static event Action<Health, DamageInfo> OnDead;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060003B7 RID: 951 RVA: 0x0001035C File Offset: 0x0000E55C
	// (remove) Token: 0x060003B8 RID: 952 RVA: 0x00010390 File Offset: 0x0000E590
	public static event Action<Health> OnRequestHealthBar;

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x000103C3 File Offset: 0x0000E5C3
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060003BA RID: 954 RVA: 0x000103CB File Offset: 0x0000E5CB
	public bool Invincible
	{
		get
		{
			return this.invincible;
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000103D4 File Offset: 0x0000E5D4
	public CharacterMainControl TryGetCharacter()
	{
		if (this.characterCached != null)
		{
			return this.characterCached;
		}
		if (!this.hasCharacter)
		{
			return null;
		}
		if (!this.item)
		{
			this.hasCharacter = false;
			return null;
		}
		this.characterCached = this.item.GetCharacterMainControl();
		if (!this.characterCached)
		{
			this.hasCharacter = true;
		}
		return this.characterCached;
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060003BC RID: 956 RVA: 0x00010441 File Offset: 0x0000E641
	public float BodyArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.bodyArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060003BD RID: 957 RVA: 0x00010467 File Offset: 0x0000E667
	public float HeadArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.headArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00010490 File Offset: 0x0000E690
	public float ElementFactor(ElementTypes type)
	{
		float num = 1f;
		if (!this.item)
		{
			return num;
		}
		Weather currentWeather = TimeOfDayController.Instance.CurrentWeather;
		bool isBaseLevel = LevelManager.Instance.IsBaseLevel;
		switch (type)
		{
		case ElementTypes.physics:
			num = this.item.GetStat(this.Hash_ElementFactor_Physics).Value;
			break;
		case ElementTypes.fire:
			num = this.item.GetStat(this.Hash_ElementFactor_Fire).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num -= 0.15f;
			}
			break;
		case ElementTypes.poison:
			num = this.item.GetStat(this.Hash_ElementFactor_Poison).Value;
			break;
		case ElementTypes.electricity:
			num = this.item.GetStat(this.Hash_ElementFactor_Electricity).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num += 0.2f;
			}
			break;
		case ElementTypes.space:
			num = this.item.GetStat(this.Hash_ElementFactor_Space).Value;
			break;
		case ElementTypes.ghost:
			num = this.item.GetStat(this.Hash_ElementFactor_Ghost).Value;
			break;
		}
		return num;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000105A1 File Offset: 0x0000E7A1
	private void Start()
	{
		if (this.autoInit)
		{
			this.Init();
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000105B1 File Offset: 0x0000E7B1
	public void SetItemAndCharacter(Item _item, CharacterMainControl _character)
	{
		this.item = _item;
		if (_character)
		{
			this.hasCharacter = true;
			this.characterCached = _character;
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000105D0 File Offset: 0x0000E7D0
	public void Init()
	{
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = this.MaxHealth;
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x000105EB File Offset: 0x0000E7EB
	public void AddBuff(Buff buffPfb, CharacterMainControl fromWho, int overrideFromWeaponID = 0)
	{
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (characterMainControl == null)
		{
			return;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, overrideFromWeaponID);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00010600 File Offset: 0x0000E800
	private void Update()
	{
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00010604 File Offset: 0x0000E804
	public bool Hurt(DamageInfo damageInfo)
	{
		if (MultiSceneCore.Instance != null && MultiSceneCore.Instance.IsLoading)
		{
			return false;
		}
		if (this.invincible)
		{
			return false;
		}
		if (this.isDead)
		{
			return false;
		}
		if (damageInfo.buff != null && UnityEngine.Random.Range(0f, 1f) < damageInfo.buffChance)
		{
			this.AddBuff(damageInfo.buff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		bool flag = LevelManager.Rule.AdvancedDebuffMode;
		if (LevelManager.Instance.IsBaseLevel)
		{
			flag = false;
		}
		float num = 0.2f;
		float num2 = 0.12f;
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (!this.IsMainCharacterHealth)
		{
			num = 0.1f;
			num2 = 0.1f;
		}
		if (flag && UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance * num)
		{
			this.AddBuff(GameplayDataSettings.Buffs.BoneCrackBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (flag && UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance * num2)
		{
			this.AddBuff(GameplayDataSettings.Buffs.WoundBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (UnityEngine.Random.Range(0f, 1f) < damageInfo.bleedChance)
		{
			if (flag)
			{
				this.AddBuff(GameplayDataSettings.Buffs.UnlimitBleedBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
			else
			{
				this.AddBuff(GameplayDataSettings.Buffs.BleedSBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
		}
		bool flag2 = UnityEngine.Random.Range(0f, 1f) < damageInfo.critRate;
		damageInfo.crit = (flag2 ? 1 : 0);
		if (!damageInfo.ignoreDifficulty && this.team == Teams.player)
		{
			damageInfo.damageValue *= LevelManager.Rule.DamageFactor_ToPlayer;
		}
		float num3 = damageInfo.damageValue * (flag2 ? damageInfo.critDamageFactor : 1f);
		if (damageInfo.damageType != DamageTypes.realDamage && !damageInfo.ignoreArmor)
		{
			float num4 = flag2 ? this.HeadArmor : this.BodyArmor;
			if (characterMainControl && LevelManager.Instance.IsRaidMap)
			{
				Item item = flag2 ? characterMainControl.GetHelmatItem() : characterMainControl.GetArmorItem();
				if (item)
				{
					item.Durability = Mathf.Max(0f, item.Durability - damageInfo.armorBreak);
				}
			}
			float num5 = 1f;
			if (num4 > 0f)
			{
				num5 = 2f / (Mathf.Clamp(num4 - damageInfo.armorPiercing, 0f, 999f) + 2f);
			}
			if (characterMainControl && !characterMainControl.IsMainCharacter && damageInfo.fromCharacter && !damageInfo.fromCharacter.IsMainCharacter)
			{
				CharacterRandomPreset characterPreset = damageInfo.fromCharacter.characterPreset;
				CharacterRandomPreset characterPreset2 = characterMainControl.characterPreset;
				if (characterPreset && characterPreset2)
				{
					num5 *= characterPreset.aiCombatFactor / characterPreset2.aiCombatFactor;
				}
			}
			num3 *= num5;
		}
		if (damageInfo.elementFactors.Count <= 0)
		{
			damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.physics, 1f));
		}
		float num6 = 0f;
		foreach (ElementFactor elementFactor in damageInfo.elementFactors)
		{
			float factor = elementFactor.factor;
			float num7 = this.ElementFactor(elementFactor.elementType);
			float num8 = num3 * factor * num7;
			if (num8 < 1f && num8 > 0f && num7 > 0f && factor > 0f)
			{
				num8 = 1f;
			}
			if (num8 > 0f && !this.Hidden && PopText.instance)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook elementDamagePopTextLook = GameplayDataSettings.UIStyle.GetElementDamagePopTextLook(elementFactor.elementType);
				float size = flag2 ? elementDamagePopTextLook.critSize : elementDamagePopTextLook.normalSize;
				Color color = elementDamagePopTextLook.color;
				PopText.Pop(num8.ToString("F1"), damageInfo.damagePoint + Vector3.up * 2f, color, size, flag2 ? GameplayDataSettings.UIStyle.CritPopSprite : null);
			}
			num6 += num8;
		}
		damageInfo.finalDamage = num6;
		if (this.CurrentHealth < damageInfo.finalDamage)
		{
			damageInfo.finalDamage = this.CurrentHealth + 1f;
		}
		this.CurrentHealth -= damageInfo.finalDamage;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		Action<Health, DamageInfo> onHurt = Health.OnHurt;
		if (onHurt != null)
		{
			onHurt(this, damageInfo);
		}
		if (this.isDead)
		{
			return true;
		}
		if (this.CurrentHealth <= 0f)
		{
			bool flag3 = true;
			if (!LevelManager.Instance.IsRaidMap)
			{
				flag3 = false;
			}
			if (!flag3)
			{
				this.SetHealth(1f);
			}
		}
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = 0f;
			this.isDead = true;
			if (LevelManager.Instance.MainCharacter != this.TryGetCharacter())
			{
				this.DestroyOnDelay().Forget();
			}
			if (this.item != null && this.team != Teams.player && damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				EXPManager.AddExp(this.item.GetInt("Exp", 0));
			}
			UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
			if (onDeadEvent != null)
			{
				onDeadEvent.Invoke(damageInfo);
			}
			Action<Health, DamageInfo> onDead = Health.OnDead;
			if (onDead != null)
			{
				onDead(this, damageInfo);
			}
			base.gameObject.SetActive(false);
			if (damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				Debug.Log("Killed by maincharacter");
			}
		}
		return true;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00010BEC File Offset: 0x0000EDEC
	public void RequestHealthBar()
	{
		if (this.showHealthBar && LevelManager.LevelInited)
		{
			Action<Health> onRequestHealthBar = Health.OnRequestHealthBar;
			if (onRequestHealthBar == null)
			{
				return;
			}
			onRequestHealthBar(this);
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00010C0D File Offset: 0x0000EE0D
	private void OnDestroy()
	{
		this.hasBeenDestroied = true;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00010C18 File Offset: 0x0000EE18
	public UniTask DestroyOnDelay()
	{
		Health.<DestroyOnDelay>d__69 <DestroyOnDelay>d__;
		<DestroyOnDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DestroyOnDelay>d__.<>4__this = this;
		<DestroyOnDelay>d__.<>1__state = -1;
		<DestroyOnDelay>d__.<>t__builder.Start<Health.<DestroyOnDelay>d__69>(ref <DestroyOnDelay>d__);
		return <DestroyOnDelay>d__.<>t__builder.Task;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00010C5B File Offset: 0x0000EE5B
	public void AddHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, this.CurrentHealth + healthValue);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00010C76 File Offset: 0x0000EE76
	public void SetHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, healthValue);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00010C8A File Offset: 0x0000EE8A
	public void SetInvincible(bool value)
	{
		this.invincible = value;
	}

	// Token: 0x040002C4 RID: 708
	public Teams team;

	// Token: 0x040002C5 RID: 709
	public bool hasSoul = true;

	// Token: 0x040002C6 RID: 710
	private Item item;

	// Token: 0x040002C7 RID: 711
	private int maxHealthHash = "MaxHealth".GetHashCode();

	// Token: 0x040002C8 RID: 712
	private float lastMaxHealth;

	// Token: 0x040002C9 RID: 713
	private bool _showHealthBar;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private int defaultMaxHealth;

	// Token: 0x040002CB RID: 715
	private float _currentHealth;

	// Token: 0x040002CC RID: 716
	public UnityEvent<Health> OnHealthChange;

	// Token: 0x040002CD RID: 717
	public UnityEvent<Health> OnMaxHealthChange;

	// Token: 0x040002D3 RID: 723
	public float healthBarHeight = 2f;

	// Token: 0x040002D4 RID: 724
	private bool isDead;

	// Token: 0x040002D5 RID: 725
	public bool autoInit = true;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	private bool DestroyOnDead = true;

	// Token: 0x040002D7 RID: 727
	[SerializeField]
	private float DeadDestroyDelay = 0.5f;

	// Token: 0x040002D8 RID: 728
	private bool inited;

	// Token: 0x040002D9 RID: 729
	private bool invincible;

	// Token: 0x040002DA RID: 730
	private bool hasCharacter = true;

	// Token: 0x040002DB RID: 731
	private CharacterMainControl characterCached;

	// Token: 0x040002DC RID: 732
	private int bodyArmorHash = "BodyArmor".GetHashCode();

	// Token: 0x040002DD RID: 733
	private int headArmorHash = "HeadArmor".GetHashCode();

	// Token: 0x040002DE RID: 734
	private int Hash_ElementFactor_Physics = "ElementFactor_Physics".GetHashCode();

	// Token: 0x040002DF RID: 735
	private int Hash_ElementFactor_Fire = "ElementFactor_Fire".GetHashCode();

	// Token: 0x040002E0 RID: 736
	private int Hash_ElementFactor_Poison = "ElementFactor_Poison".GetHashCode();

	// Token: 0x040002E1 RID: 737
	private int Hash_ElementFactor_Electricity = "ElementFactor_Electricity".GetHashCode();

	// Token: 0x040002E2 RID: 738
	private int Hash_ElementFactor_Space = "ElementFactor_Space".GetHashCode();

	// Token: 0x040002E3 RID: 739
	private int Hash_ElementFactor_Ghost = "ElementFactor_Ghost".GetHashCode();

	// Token: 0x040002E4 RID: 740
	private bool hasBeenDestroied;
}
