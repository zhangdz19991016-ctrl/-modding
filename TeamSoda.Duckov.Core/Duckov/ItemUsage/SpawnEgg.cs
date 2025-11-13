using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000374 RID: 884
	public class SpawnEgg : UsageBehavior
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0006CDC4 File Offset: 0x0006AFC4
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return new UsageBehavior.DisplaySettingsData
				{
					display = true,
					description = (this.descriptionKey.ToPlainText() ?? "")
				};
			}
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0006CDFD File Offset: 0x0006AFFD
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0006CE00 File Offset: 0x0006B000
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			Egg egg = UnityEngine.Object.Instantiate<Egg>(this.eggPrefab, characterMainControl.transform.position, Quaternion.identity);
			Collider component = egg.GetComponent<Collider>();
			Collider component2 = characterMainControl.GetComponent<Collider>();
			if (component && component2)
			{
				Debug.Log("关掉角色和蛋的碰撞");
				Physics.IgnoreCollision(component, component2, true);
			}
			egg.Init(characterMainControl.transform.position, characterMainControl.CurrentAimDirection * 1f, characterMainControl, this.spawnCharacter, this.eggSpawnDelay);
		}

		// Token: 0x040014FD RID: 5373
		public Egg eggPrefab;

		// Token: 0x040014FE RID: 5374
		public CharacterRandomPreset spawnCharacter;

		// Token: 0x040014FF RID: 5375
		public float eggSpawnDelay = 2f;

		// Token: 0x04001500 RID: 5376
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_SpawnEgg";
	}
}
