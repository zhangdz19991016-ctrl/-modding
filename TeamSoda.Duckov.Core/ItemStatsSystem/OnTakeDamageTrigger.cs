using System;
using UnityEngine;
using UnityEngine.Events;

namespace ItemStatsSystem
{
	// Token: 0x0200022C RID: 556
	[MenuPath("General/On Take Damage")]
	public class OnTakeDamageTrigger : EffectTrigger
	{
		// Token: 0x0600113B RID: 4411 RVA: 0x000432E9 File Offset: 0x000414E9
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000432F1 File Offset: 0x000414F1
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000432FF File Offset: 0x000414FF
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00043308 File Offset: 0x00041508
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (base.Master == null)
			{
				return;
			}
			Item item = base.Master.Item;
			if (item == null)
			{
				return;
			}
			CharacterMainControl characterMainControl = item.GetCharacterMainControl();
			if (characterMainControl == null)
			{
				return;
			}
			this.target = characterMainControl.Health;
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00043379 File Offset: 0x00041579
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x000433A6 File Offset: 0x000415A6
		private void OnTookDamage(DamageInfo info)
		{
			if (info.damageValue < (float)this.threshold)
			{
				return;
			}
			base.Trigger(true);
		}

		// Token: 0x04000D7C RID: 3452
		[SerializeField]
		public int threshold;

		// Token: 0x04000D7D RID: 3453
		private Health target;
	}
}
