using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DB RID: 987
	public class RectTransformFade : FadeElement
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x0007E0A0 File Offset: 0x0007C2A0
		private Vector2 TargetAnchoredPosition
		{
			get
			{
				return this.cachedAnchordPosition + this.offset;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x0007E0B3 File Offset: 0x0007C2B3
		private Vector3 TargetScale
		{
			get
			{
				return this.cachedScale + Vector3.one * this.uniformScale;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x0007E0D0 File Offset: 0x0007C2D0
		private Vector3 TargetRotation
		{
			get
			{
				return this.cachedRotation + Vector3.forward * this.rotateZ;
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0007E0ED File Offset: 0x0007C2ED
		private void Initialize()
		{
			if (this.initialized)
			{
				Debug.LogError("Object Initialized Twice, aborting");
				return;
			}
			this.CachePose();
			this.initialized = true;
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0007E110 File Offset: 0x0007C310
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchordPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0007E168 File Offset: 0x0007C368
		private void Awake()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0007E1B5 File Offset: 0x0007C3B5
		private void OnValidate()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0007E1EC File Offset: 0x0007C3EC
		protected override UniTask HideTask(int token)
		{
			RectTransformFade.<HideTask>d__22 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<RectTransformFade.<HideTask>d__22>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0007E230 File Offset: 0x0007C430
		protected override UniTask ShowTask(int token)
		{
			RectTransformFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<RectTransformFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0007E274 File Offset: 0x0007C474
		protected override void OnSkipHide()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipHide");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.TargetAnchoredPosition;
			this.rectTransform.localScale = this.TargetScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.TargetRotation);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0007E2D9 File Offset: 0x0007C4D9
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.DOKill(false);
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x0007E2F0 File Offset: 0x0007C4F0
		protected override void OnSkipShow()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipShow");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.cachedAnchordPosition;
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x04001862 RID: 6242
		[SerializeField]
		private bool debug;

		// Token: 0x04001863 RID: 6243
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001864 RID: 6244
		[SerializeField]
		private float duration = 0.4f;

		// Token: 0x04001865 RID: 6245
		[SerializeField]
		private Vector2 offset = Vector2.left * 10f;

		// Token: 0x04001866 RID: 6246
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x04001867 RID: 6247
		[SerializeField]
		[Range(-180f, 180f)]
		private float rotateZ;

		// Token: 0x04001868 RID: 6248
		[SerializeField]
		private AnimationCurve showingAnimationCurve;

		// Token: 0x04001869 RID: 6249
		[SerializeField]
		private AnimationCurve hidingAnimationCurve;

		// Token: 0x0400186A RID: 6250
		private Vector2 cachedAnchordPosition = Vector2.zero;

		// Token: 0x0400186B RID: 6251
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x0400186C RID: 6252
		private Vector3 cachedRotation = Vector3.zero;

		// Token: 0x0400186D RID: 6253
		private bool initialized;
	}
}
