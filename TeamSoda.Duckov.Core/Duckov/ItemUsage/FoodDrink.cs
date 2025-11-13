using System;
using ItemStatsSystem;
using SodaCraft.Localizations;

namespace Duckov.ItemUsage
{
	// Token: 0x02000372 RID: 882
	[MenuPath("食物/食物")]
	public class FoodDrink : UsageBehavior
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x0006CB30 File Offset: 0x0006AD30
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				if (this.energyValue != 0f && this.waterValue != 0f)
				{
					result.description = string.Concat(new string[]
					{
						this.energyKey.ToPlainText(),
						": ",
						this.energyValue.ToString(),
						"  ",
						this.waterKey.ToPlainText(),
						": ",
						this.waterValue.ToString()
					});
				}
				else if (this.energyValue != 0f)
				{
					result.description = this.energyKey.ToPlainText() + ": " + this.energyValue.ToString();
				}
				else
				{
					result.description = this.waterKey.ToPlainText() + ": " + this.waterValue.ToString();
				}
				return result;
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0006CC29 File Offset: 0x0006AE29
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0006CC3C File Offset: 0x0006AE3C
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			this.Eat(characterMainControl);
			if (this.UseDurability > 0f && item.UseDurability)
			{
				item.Durability -= this.UseDurability;
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0006CC88 File Offset: 0x0006AE88
		private void Eat(CharacterMainControl character)
		{
			if (this.energyValue != 0f)
			{
				character.AddEnergy(this.energyValue);
			}
			if (this.waterValue != 0f)
			{
				character.AddWater(this.waterValue);
			}
		}

		// Token: 0x040014F3 RID: 5363
		public float energyValue;

		// Token: 0x040014F4 RID: 5364
		public float waterValue;

		// Token: 0x040014F5 RID: 5365
		[LocalizationKey("Default")]
		public string energyKey = "Usage_Energy";

		// Token: 0x040014F6 RID: 5366
		[LocalizationKey("Default")]
		public string waterKey = "Usage_Water";

		// Token: 0x040014F7 RID: 5367
		public float UseDurability;
	}
}
