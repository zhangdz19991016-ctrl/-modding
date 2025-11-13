using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E0 RID: 992
	public class ImageColorLooper : LooperElement
	{
		// Token: 0x06002443 RID: 9283 RVA: 0x0007ED34 File Offset: 0x0007CF34
		protected override void OnTick(LooperClock clock, float t)
		{
			Color color = this.colorOverT.Evaluate(t);
			float num = this.alphaOverT.Evaluate(t);
			color.a *= num;
			this.image.color = color;
		}

		// Token: 0x0400188E RID: 6286
		[SerializeField]
		private Image image;

		// Token: 0x0400188F RID: 6287
		[GradientUsage(true)]
		[SerializeField]
		private Gradient colorOverT;

		// Token: 0x04001890 RID: 6288
		[SerializeField]
		private AnimationCurve alphaOverT;
	}
}
