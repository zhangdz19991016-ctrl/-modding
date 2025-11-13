using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DC RID: 988
	public class ScaleFade : FadeElement
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0007E44F File Offset: 0x0007C64F
		private Vector3 HiddenScale
		{
			get
			{
				return Vector3.one + Vector3.one * this.uniformScale + this.scale;
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x0007E476 File Offset: 0x0007C676
		private void CachePose()
		{
			this.cachedScale = base.transform.localScale;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x0007E489 File Offset: 0x0007C689
		private void RestorePose()
		{
			base.transform.localScale = this.cachedScale;
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x0007E49C File Offset: 0x0007C69C
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.CachePose();
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x0007E4B4 File Offset: 0x0007C6B4
		protected override UniTask HideTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!base.transform)
			{
				return UniTask.CompletedTask;
			}
			return base.transform.DOScale(this.HiddenScale, this.duration).SetEase(this.hideCurve).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0007E513 File Offset: 0x0007C713
		protected override void OnSkipHide()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			base.transform.localScale = this.HiddenScale;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0007E534 File Offset: 0x0007C734
		protected override void OnSkipShow()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.RestorePose();
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0007E54C File Offset: 0x0007C74C
		protected override UniTask ShowTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return base.transform.DOScale(this.cachedScale, this.duration).SetEase(this.showCurve).OnComplete(new TweenCallback(this.RestorePose)).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x0400186E RID: 6254
		[SerializeField]
		private float duration = 0.1f;

		// Token: 0x0400186F RID: 6255
		[SerializeField]
		private Vector3 scale = Vector3.zero;

		// Token: 0x04001870 RID: 6256
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x04001871 RID: 6257
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001872 RID: 6258
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001873 RID: 6259
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x04001874 RID: 6260
		private bool initialized;
	}
}
