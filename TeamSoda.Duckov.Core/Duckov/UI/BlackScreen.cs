using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000381 RID: 897
	public class BlackScreen : MonoBehaviour
	{
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x0006DC8D File Offset: 0x0006BE8D
		public static BlackScreen Instance
		{
			get
			{
				return GameManager.BlackScreen;
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0006DC94 File Offset: 0x0006BE94
		private void Awake()
		{
			if (BlackScreen.Instance != this)
			{
				Debug.LogError("检测到应当删除的BlackScreen实例", base.gameObject);
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0006DCB3 File Offset: 0x0006BEB3
		private void SetFadeCurve(AnimationCurve curve)
		{
			this.fadeElement.ShowCurve = curve;
			this.fadeElement.HideCurve = curve;
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0006DCCD File Offset: 0x0006BECD
		private void SetCircleFade(float circleFade)
		{
			this.fadeImage.material.SetFloat("_CircleFade", circleFade);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0006DCE8 File Offset: 0x0006BEE8
		private UniTask LShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			this.taskCounter++;
			if (this.taskCounter > 1)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultShowCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0006DD58 File Offset: 0x0006BF58
		private UniTask LHideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			int num = this.taskCounter - 1;
			this.taskCounter = num;
			if (num > 0)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultHideCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0006DDC5 File Offset: 0x0006BFC5
		public static UniTask ShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LShowAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0006DDE7 File Offset: 0x0006BFE7
		public static UniTask HideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LHideAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x04001544 RID: 5444
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001545 RID: 5445
		[SerializeField]
		private MaterialPropertyFade fadeElement;

		// Token: 0x04001546 RID: 5446
		[SerializeField]
		private Image fadeImage;

		// Token: 0x04001547 RID: 5447
		[SerializeField]
		private float defaultDuration = 0.5f;

		// Token: 0x04001548 RID: 5448
		[SerializeField]
		private AnimationCurve defaultShowCurve;

		// Token: 0x04001549 RID: 5449
		[SerializeField]
		private AnimationCurve defaultHideCurve;

		// Token: 0x0400154A RID: 5450
		private int taskCounter;
	}
}
