using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Effects
{
	// Token: 0x020003F6 RID: 1014
	public class DamageAction : EffectAction
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x00080244 File Offset: 0x0007E444
		private CharacterMainControl MainControl
		{
			get
			{
				Effect master = base.Master;
				if (master == null)
				{
					return null;
				}
				Item item = master.Item;
				if (item == null)
				{
					return null;
				}
				return item.GetCharacterMainControl();
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00080264 File Offset: 0x0007E464
		protected override void OnTriggeredPositive()
		{
			if (this.MainControl == null)
			{
				return;
			}
			if (this.MainControl.Health == null)
			{
				return;
			}
			this.damageInfo.isFromBuffOrEffect = true;
			if (this.buff != null)
			{
				this.damageInfo.fromCharacter = this.buff.fromWho;
				this.damageInfo.fromWeaponItemID = this.buff.fromWeaponID;
			}
			this.damageInfo.damagePoint = this.MainControl.transform.position + Vector3.up * 0.8f;
			this.damageInfo.damageNormal = Vector3.up;
			if (this.percentDamage && this.MainControl.Health != null)
			{
				this.damageInfo.damageValue = this.percentDamageValue * this.MainControl.Health.MaxHealth * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			else
			{
				this.damageInfo.damageValue = this.damageValue * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			this.MainControl.Health.Hurt(this.damageInfo);
			if (this.fx)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.fx, this.damageInfo.damagePoint, Quaternion.identity);
			}
		}

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		private Buff buff;

		// Token: 0x040018F7 RID: 6391
		[SerializeField]
		private bool percentDamage;

		// Token: 0x040018F8 RID: 6392
		[SerializeField]
		private float damageValue = 1f;

		// Token: 0x040018F9 RID: 6393
		[SerializeField]
		private float percentDamageValue;

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		private DamageInfo damageInfo = new DamageInfo(null);

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private GameObject fx;
	}
}
