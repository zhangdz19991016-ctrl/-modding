using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000370 RID: 880
	[MenuPath("概率死亡")]
	public class DeadByChance : UsageBehavior
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0006C7D0 File Offset: 0x0006A9D0
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return new UsageBehavior.DisplaySettingsData
				{
					display = true,
					description = string.Format("{0}:  {1:0}%", this.descriptionKey.ToPlainText(), this.chance * 100f)
				};
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0006C81B File Offset: 0x0006AA1B
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0006C830 File Offset: 0x0006AA30
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			this.KillSelf(characterMainControl, item.TypeID).Forget();
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0006C87C File Offset: 0x0006AA7C
		private UniTaskVoid KillSelf(CharacterMainControl character, int weaponID)
		{
			DeadByChance.<KillSelf>d__8 <KillSelf>d__;
			<KillSelf>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<KillSelf>d__.<>4__this = this;
			<KillSelf>d__.character = character;
			<KillSelf>d__.weaponID = weaponID;
			<KillSelf>d__.<>1__state = -1;
			<KillSelf>d__.<>t__builder.Start<DeadByChance.<KillSelf>d__8>(ref <KillSelf>d__);
			return <KillSelf>d__.<>t__builder.Task;
		}

		// Token: 0x040014E9 RID: 5353
		public int damageValue = 9999;

		// Token: 0x040014EA RID: 5354
		public float chance;

		// Token: 0x040014EB RID: 5355
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_DeadByChance";

		// Token: 0x040014EC RID: 5356
		[LocalizationKey("Default")]
		public string popTextKey = "Usage_DeadByChance_PopText";
	}
}
