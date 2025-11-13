using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E9 RID: 1001
	public class ScaleToggle : ToggleAnimation
	{
		// Token: 0x06002463 RID: 9315 RVA: 0x0007F2C8 File Offset: 0x0007D4C8
		private void CachePose()
		{
			this.cachedScale = this.rectTransform.localScale;
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0007F2DB File Offset: 0x0007D4DB
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.CachePose();
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0007F2F4 File Offset: 0x0007D4F4
		protected override void OnSetToggle(bool status)
		{
			float d = status ? this.activeScale : this.idleScale;
			d * this.cachedScale;
			this.rectTransform.DOKill(false);
			this.rectTransform.DOScale(this.cachedScale * d, this.duration).SetEase(this.animationCurve);
		}

		// Token: 0x040018A9 RID: 6313
		public float idleScale = 1f;

		// Token: 0x040018AA RID: 6314
		public float activeScale = 0.9f;

		// Token: 0x040018AB RID: 6315
		public float duration = 0.1f;

		// Token: 0x040018AC RID: 6316
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018AD RID: 6317
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x040018AE RID: 6318
		private RectTransform rectTransform;
	}
}
