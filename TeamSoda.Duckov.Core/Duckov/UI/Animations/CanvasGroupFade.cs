using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D9 RID: 985
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupFade : FadeElement
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0007DBD8 File Offset: 0x0007BDD8
		private float ShowingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x0007DBE0 File Offset: 0x0007BDE0
		private float HidingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0007DBE8 File Offset: 0x0007BDE8
		private void Awake()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			this.awaked = true;
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x0007DC23 File Offset: 0x0007BE23
		private void OnValidate()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0007DC58 File Offset: 0x0007BE58
		protected override UniTask ShowTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (!this.awaked)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 0f, 1f, this.showingCurve, this.ShowingDuration);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x0007DCDC File Offset: 0x0007BEDC
		protected override UniTask HideTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 1f, 0f, this.hidingCurve, this.HidingDuration);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x0007DD48 File Offset: 0x0007BF48
		private UniTask FadeTask(int token, float beginAlpha, float targetAlpha, AnimationCurve animationCurve, float duration)
		{
			CanvasGroupFade.<FadeTask>d__14 <FadeTask>d__;
			<FadeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<FadeTask>d__.<>4__this = this;
			<FadeTask>d__.token = token;
			<FadeTask>d__.beginAlpha = beginAlpha;
			<FadeTask>d__.targetAlpha = targetAlpha;
			<FadeTask>d__.animationCurve = animationCurve;
			<FadeTask>d__.duration = duration;
			<FadeTask>d__.<>1__state = -1;
			<FadeTask>d__.<>t__builder.Start<CanvasGroupFade.<FadeTask>d__14>(ref <FadeTask>d__);
			return <FadeTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x0007DDB5 File Offset: 0x0007BFB5
		protected override void OnSkipHide()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0007DDE9 File Offset: 0x0007BFE9
		protected override void OnSkipShow()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 1f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0007DE30 File Offset: 0x0007C030
		[CompilerGenerated]
		private bool <FadeTask>g__CheckTaskValid|14_0(ref CanvasGroupFade.<>c__DisplayClass14_0 A_1)
		{
			return this.canvasGroup != null && A_1.token == base.ActiveTaskToken;
		}

		// Token: 0x04001855 RID: 6229
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001856 RID: 6230
		[SerializeField]
		private AnimationCurve showingCurve;

		// Token: 0x04001857 RID: 6231
		[SerializeField]
		private AnimationCurve hidingCurve;

		// Token: 0x04001858 RID: 6232
		[SerializeField]
		private float fadeDuration = 0.2f;

		// Token: 0x04001859 RID: 6233
		[SerializeField]
		private bool manageBlockRaycast;

		// Token: 0x0400185A RID: 6234
		private bool awaked;
	}
}
