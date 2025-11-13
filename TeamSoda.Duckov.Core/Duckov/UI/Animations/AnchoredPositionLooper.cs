using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DF RID: 991
	public class AnchoredPositionLooper : LooperElement
	{
		// Token: 0x06002440 RID: 9280 RVA: 0x0007ECD1 File Offset: 0x0007CED1
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0007ECE4 File Offset: 0x0007CEE4
		protected override void OnTick(LooperClock clock, float t)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 anchoredPosition = Vector2.Lerp(this.anchoredPositionA, this.anchoredPositionB, this.curve.Evaluate(t));
			this.rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0400188A RID: 6282
		[SerializeField]
		private Vector2 anchoredPositionA;

		// Token: 0x0400188B RID: 6283
		[SerializeField]
		private Vector2 anchoredPositionB;

		// Token: 0x0400188C RID: 6284
		[SerializeField]
		private AnimationCurve curve;

		// Token: 0x0400188D RID: 6285
		private RectTransform rectTransform;
	}
}
