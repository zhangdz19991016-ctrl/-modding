using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200022B RID: 555
	[MenuPath("General/On Shoot&Attack")]
	public class OnShootAttackTrigger : EffectTrigger
	{
		// Token: 0x06001134 RID: 4404 RVA: 0x000431BB File Offset: 0x000413BB
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000431C3 File Offset: 0x000413C3
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000431D1 File Offset: 0x000413D1
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000431DC File Offset: 0x000413DC
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
			this.target = item.GetCharacterMainControl();
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent += this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent += this.OnShootAttack;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00043270 File Offset: 0x00041470
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent -= this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent -= this.OnShootAttack;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000432CA File Offset: 0x000414CA
		private void OnShootAttack(DuckovItemAgent agent)
		{
			base.Trigger(true);
		}

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private bool onShoot = true;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		private bool onAttack = true;

		// Token: 0x04000D7B RID: 3451
		private CharacterMainControl target;
	}
}
