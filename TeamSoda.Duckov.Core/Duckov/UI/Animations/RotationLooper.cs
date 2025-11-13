using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E4 RID: 996
	public class RotationLooper : LooperElement
	{
		// Token: 0x06002451 RID: 9297 RVA: 0x0007EF30 File Offset: 0x0007D130
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector3 euler = Vector3.Lerp(this.eulerRotationA, this.eulerRotationB, this.curve.Evaluate(t));
			base.transform.localRotation = Quaternion.Euler(euler);
		}

		// Token: 0x04001898 RID: 6296
		[SerializeField]
		private Vector3 eulerRotationA;

		// Token: 0x04001899 RID: 6297
		[SerializeField]
		private Vector3 eulerRotationB;

		// Token: 0x0400189A RID: 6298
		[SerializeField]
		private AnimationCurve curve;
	}
}
