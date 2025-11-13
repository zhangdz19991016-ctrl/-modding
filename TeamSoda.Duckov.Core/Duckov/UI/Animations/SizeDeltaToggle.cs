using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003EA RID: 1002
	public class SizeDeltaToggle : ToggleAnimation
	{
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x0007F3B6 File Offset: 0x0007D5B6
		private RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0007F3D8 File Offset: 0x0007D5D8
		private void CachePose()
		{
			this.cachedSizeDelta = this.RectTransform.sizeDelta;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0007F3EB File Offset: 0x0007D5EB
		private void Awake()
		{
			this.CachePose();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0007F3F4 File Offset: 0x0007D5F4
		protected override void OnSetToggle(bool status)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 endValue = status ? this.activeSizeDelta : this.idleSizeDelta;
			this.RectTransform.DOKill(false);
			this.RectTransform.DOSizeDelta(endValue, this.duration, false).SetEase(this.animationCurve);
		}

		// Token: 0x040018AF RID: 6319
		public Vector2 idleSizeDelta = Vector2.zero;

		// Token: 0x040018B0 RID: 6320
		public Vector2 activeSizeDelta = Vector2.one * 12f;

		// Token: 0x040018B1 RID: 6321
		public float duration = 0.1f;

		// Token: 0x040018B2 RID: 6322
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018B3 RID: 6323
		private Vector2 cachedSizeDelta = Vector3.one;

		// Token: 0x040018B4 RID: 6324
		private RectTransform _rectTransform;
	}
}
