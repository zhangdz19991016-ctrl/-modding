using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000371 RID: 881
	[MenuPath("医疗/药")]
	public class Drug : UsageBehavior
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0006C8F8 File Offset: 0x0006AAF8
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = string.Format("{0} : {1}", this.healValueDescriptionKey.ToPlainText(), this.healValue);
				if (this.useDurability)
				{
					result.description += string.Format(" ({0} : {1})", this.durabilityUsageDescriptionKey.ToPlainText(), this.durabilityUsage);
				}
				return result;
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0006C974 File Offset: 0x0006AB74
		public override bool CanBeUsed(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return characterMainControl && this.CheckCanHeal(characterMainControl);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0006C9A0 File Offset: 0x0006ABA0
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			float num = (float)this.healValue;
			if (this.useDurability && item.UseDurability)
			{
				float num2 = this.durabilityUsage;
				if (this.canUsePart)
				{
					num = characterMainControl.Health.MaxHealth - characterMainControl.Health.CurrentHealth;
					if (num > (float)this.healValue)
					{
						num = (float)this.healValue;
					}
					num2 = num / (float)this.healValue * this.durabilityUsage;
					if (num2 > item.Durability)
					{
						num2 = item.Durability;
						num = (float)this.healValue * item.Durability / this.durabilityUsage;
					}
					Debug.Log(string.Format("治疗：{0}耐久消耗：{1}", num, num2));
					item.Durability -= num2;
				}
			}
			this.Heal(characterMainControl, item, num);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0006CA80 File Offset: 0x0006AC80
		private bool CheckCanHeal(CharacterMainControl character)
		{
			return this.healValue <= 0 || character.Health.CurrentHealth < character.Health.MaxHealth;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0006CAA8 File Offset: 0x0006ACA8
		private void Heal(CharacterMainControl character, Item selfItem, float _healValue)
		{
			if (_healValue > 0f)
			{
				character.AddHealth((float)Mathf.CeilToInt(_healValue));
				return;
			}
			if (_healValue < 0f)
			{
				DamageInfo damageInfo = new DamageInfo(null);
				damageInfo.damageValue = -_healValue;
				damageInfo.damagePoint = character.transform.position;
				damageInfo.damageNormal = Vector3.up;
				character.Health.Hurt(damageInfo);
			}
		}

		// Token: 0x040014ED RID: 5357
		public int healValue;

		// Token: 0x040014EE RID: 5358
		[LocalizationKey("Default")]
		public string healValueDescriptionKey = "Usage_HealValue";

		// Token: 0x040014EF RID: 5359
		[LocalizationKey("Default")]
		public string durabilityUsageDescriptionKey = "Usage_Durability";

		// Token: 0x040014F0 RID: 5360
		public bool useDurability;

		// Token: 0x040014F1 RID: 5361
		public float durabilityUsage;

		// Token: 0x040014F2 RID: 5362
		public bool canUsePart;
	}
}
