using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200040D RID: 1037
	public class CheckTargetDistance : ConditionTask<AICharacterController>
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x0008262A File Offset: 0x0008082A
		protected override string info
		{
			get
			{
				return "is target in range";
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00082634 File Offset: 0x00080834
		protected override bool OnCheck()
		{
			if (this.useTransform && this.targetTransform.value == null)
			{
				return false;
			}
			Vector3 b = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			float num;
			if (this.useShootRange)
			{
				num = base.agent.CharacterMainControl.GetAimRange() * this.shootRangeMultiplier.value;
			}
			else
			{
				num = this.distance.value;
			}
			return Vector3.Distance(base.agent.transform.position, b) <= num;
		}

		// Token: 0x0400199E RID: 6558
		public bool useTransform;

		// Token: 0x0400199F RID: 6559
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x040019A0 RID: 6560
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x040019A1 RID: 6561
		public bool useShootRange;

		// Token: 0x040019A2 RID: 6562
		[ShowIf("useShootRange", 1)]
		public BBParameter<float> shootRangeMultiplier = 1f;

		// Token: 0x040019A3 RID: 6563
		[ShowIf("useShootRange", 0)]
		public BBParameter<float> distance;
	}
}
