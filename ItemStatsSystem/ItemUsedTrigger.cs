using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000016 RID: 22
	public class ItemUsedTrigger : EffectTrigger
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003E65 File Offset: 0x00002065
		public override string DisplayName
		{
			get
			{
				return "当物品被使用";
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003E6C File Offset: 0x0000206C
		private void OnEnable()
		{
			if (base.Master != null && base.Master.Item != null)
			{
				base.Master.Item.onUse += this.OnItemUsed;
				return;
			}
			Debug.LogError("因为找不到对象，未能注册物品使用事件。");
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003EC4 File Offset: 0x000020C4
		private new void OnDisable()
		{
			if (base.Master == null)
			{
				return;
			}
			if (base.Master.Item == null)
			{
				return;
			}
			base.Master.Item.onUse -= this.OnItemUsed;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003F10 File Offset: 0x00002110
		private void OnItemUsed(Item item, object user)
		{
			base.Trigger(true);
		}
	}
}
