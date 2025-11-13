using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000408 RID: 1032
	public class CheckHeathLessThan : ConditionTask<AICharacterController>
	{
		// Token: 0x0600259B RID: 9627 RVA: 0x00082451 File Offset: 0x00080651
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00082454 File Offset: 0x00080654
		protected override void OnEnable()
		{
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00082456 File Offset: 0x00080656
		protected override void OnDisable()
		{
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00082458 File Offset: 0x00080658
		protected override bool OnCheck()
		{
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				return false;
			}
			this.checkTimeMarker = Time.time;
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				return false;
			}
			Health health = base.agent.CharacterMainControl.Health;
			return health && health.CurrentHealth / health.MaxHealth <= this.percent;
		}

		// Token: 0x04001996 RID: 6550
		public float percent;

		// Token: 0x04001997 RID: 6551
		private float checkTimeMarker = -1f;

		// Token: 0x04001998 RID: 6552
		public float checkTimeSpace = 1.5f;
	}
}
