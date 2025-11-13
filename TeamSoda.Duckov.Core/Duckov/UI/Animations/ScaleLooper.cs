using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E5 RID: 997
	public class ScaleLooper : LooperElement
	{
		// Token: 0x06002453 RID: 9299 RVA: 0x0007EF84 File Offset: 0x0007D184
		protected override void OnTick(LooperClock clock, float t)
		{
			float num = this.xOverT.Evaluate(t);
			float num2 = this.yOverT.Evaluate(t);
			float num3 = this.zOverT.Evaluate(t);
			float num4 = this.uniformScaleOverT.Evaluate(t);
			num *= num4;
			num2 *= num4;
			num3 *= num4;
			base.transform.localScale = new Vector3(num, num2, num3);
		}

		// Token: 0x0400189B RID: 6299
		[SerializeField]
		private AnimationCurve uniformScaleOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400189C RID: 6300
		[SerializeField]
		private AnimationCurve xOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400189D RID: 6301
		[SerializeField]
		private AnimationCurve yOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400189E RID: 6302
		[SerializeField]
		private AnimationCurve zOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	}
}
