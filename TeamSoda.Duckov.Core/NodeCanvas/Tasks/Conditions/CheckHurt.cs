using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200040A RID: 1034
	public class CheckHurt : ConditionTask<AICharacterController>
	{
		// Token: 0x060025A5 RID: 9637 RVA: 0x00082520 File Offset: 0x00080720
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00082523 File Offset: 0x00080723
		protected override void OnEnable()
		{
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x00082525 File Offset: 0x00080725
		protected override void OnDisable()
		{
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00082528 File Offset: 0x00080728
		protected override bool OnCheck()
		{
			if (base.agent == null || this.cacheFromCharacterDmgReceiver == null)
			{
				return false;
			}
			bool result = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(this.hurtTimeThreshold, this.damageThreshold, ref damageInfo))
			{
				this.cacheFromCharacterDmgReceiver.value = damageInfo.fromCharacter.mainDamageReceiver;
				result = true;
			}
			return result;
		}

		// Token: 0x04001999 RID: 6553
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x0400199A RID: 6554
		public int damageThreshold = 3;

		// Token: 0x0400199B RID: 6555
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
