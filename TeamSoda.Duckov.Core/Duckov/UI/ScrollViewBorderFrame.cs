using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CC RID: 972
	public class ScrollViewBorderFrame : MonoBehaviour
	{
		// Token: 0x06002380 RID: 9088 RVA: 0x0007C932 File Offset: 0x0007AB32
		private void OnEnable()
		{
			this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.Refresh));
			UniTask.Void(delegate()
			{
				ScrollViewBorderFrame.<<OnEnable>b__8_0>d <<OnEnable>b__8_0>d;
				<<OnEnable>b__8_0>d.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<<OnEnable>b__8_0>d.<>4__this = this;
				<<OnEnable>b__8_0>d.<>1__state = -1;
				<<OnEnable>b__8_0>d.<>t__builder.Start<ScrollViewBorderFrame.<<OnEnable>b__8_0>d>(ref <<OnEnable>b__8_0>d);
				return <<OnEnable>b__8_0>d.<>t__builder.Task;
			});
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0007C961 File Offset: 0x0007AB61
		private void OnDisable()
		{
			this.scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.Refresh));
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0007C97F File Offset: 0x0007AB7F
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0007C988 File Offset: 0x0007AB88
		private void Refresh(Vector2 scrollPos)
		{
			RectTransform viewport = this.scrollRect.viewport;
			RectTransform content = this.scrollRect.content;
			Rect rect = viewport.rect;
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, content);
			float num = bounds.max.y - rect.max.y + this.extendOffset;
			float num2 = rect.min.y - bounds.min.y + this.extendOffset;
			float num3 = rect.min.x - bounds.min.x + this.extendOffset;
			float num4 = bounds.max.x - rect.max.x + this.extendOffset;
			float alpha = Mathf.Lerp(0f, this.maxAlpha, num / this.extendThreshold);
			float alpha2 = Mathf.Lerp(0f, this.maxAlpha, num2 / this.extendThreshold);
			float alpha3 = Mathf.Lerp(0f, this.maxAlpha, num3 / this.extendThreshold);
			float alpha4 = Mathf.Lerp(0f, this.maxAlpha, num4 / this.extendThreshold);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.upGraphic, alpha);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.downGraphic, alpha2);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.leftGraphic, alpha3);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.rightGraphic, alpha4);
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0007CAE0 File Offset: 0x0007ACE0
		private void Refresh()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			this.Refresh(this.scrollRect.normalizedPosition);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0007CB64 File Offset: 0x0007AD64
		[CompilerGenerated]
		internal static void <Refresh>g__SetAlpha|11_0(Graphic graphic, float alpha)
		{
			if (graphic == null)
			{
				return;
			}
			Color color = graphic.color;
			color.a = alpha;
			graphic.color = color;
		}

		// Token: 0x04001817 RID: 6167
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04001818 RID: 6168
		[Range(0f, 1f)]
		[SerializeField]
		private float maxAlpha = 1f;

		// Token: 0x04001819 RID: 6169
		[SerializeField]
		private float extendThreshold = 10f;

		// Token: 0x0400181A RID: 6170
		[SerializeField]
		private float extendOffset;

		// Token: 0x0400181B RID: 6171
		[SerializeField]
		private Graphic upGraphic;

		// Token: 0x0400181C RID: 6172
		[SerializeField]
		private Graphic downGraphic;

		// Token: 0x0400181D RID: 6173
		[SerializeField]
		private Graphic leftGraphic;

		// Token: 0x0400181E RID: 6174
		[SerializeField]
		private Graphic rightGraphic;
	}
}
