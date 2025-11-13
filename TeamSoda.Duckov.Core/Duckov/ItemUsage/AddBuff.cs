using System;
using Duckov.Buffs;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x0200036F RID: 879
	public class AddBuff : UsageBehavior
	{
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0006C6A4 File Offset: 0x0006A8A4
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = "";
				result.description = (this.buffPrefab.DisplayName ?? "");
				if (this.buffPrefab.LimitedLifeTime)
				{
					result.description += string.Format(" : {0}s ", this.buffPrefab.TotalLifeTime);
				}
				if (this.chance < 1f)
				{
					result.description += string.Format(" ({0} : {1}%)", this.chanceKey.ToPlainText(), Mathf.RoundToInt(this.chance * 100f));
				}
				return result;
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0006C766 File Offset: 0x0006A966
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0006C76C File Offset: 0x0006A96C
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			characterMainControl.AddBuff(this.buffPrefab, characterMainControl, 0);
		}

		// Token: 0x040014E6 RID: 5350
		public Buff buffPrefab;

		// Token: 0x040014E7 RID: 5351
		[Range(0.01f, 1f)]
		public float chance = 1f;

		// Token: 0x040014E8 RID: 5352
		[LocalizationKey("Default")]
		private string chanceKey = "UI_AddBuffChance";
	}
}
