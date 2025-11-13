using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200040E RID: 1038
	public class HasObsticleToTarget : ConditionTask<AICharacterController>
	{
		// Token: 0x060025B4 RID: 9652 RVA: 0x000826F1 File Offset: 0x000808F1
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000826F4 File Offset: 0x000808F4
		protected override void OnEnable()
		{
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000826F6 File Offset: 0x000808F6
		protected override void OnDisable()
		{
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000826F8 File Offset: 0x000808F8
		protected override bool OnCheck()
		{
			return base.agent.hasObsticleToTarget;
		}

		// Token: 0x040019A4 RID: 6564
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x040019A5 RID: 6565
		public int damageThreshold = 3;

		// Token: 0x040019A6 RID: 6566
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
