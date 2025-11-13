using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E1 RID: 993
	public class LocalPositionLooper : LooperElement
	{
		// Token: 0x06002445 RID: 9285 RVA: 0x0007ED7C File Offset: 0x0007CF7C
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector2 v = Vector2.Lerp(this.localPositionA, this.localPositionB, this.curve.Evaluate(t));
			base.transform.localPosition = v;
		}

		// Token: 0x04001891 RID: 6289
		[SerializeField]
		private Vector3 localPositionA;

		// Token: 0x04001892 RID: 6290
		[SerializeField]
		private Vector3 localPositionB;

		// Token: 0x04001893 RID: 6291
		[SerializeField]
		private AnimationCurve curve;
	}
}
