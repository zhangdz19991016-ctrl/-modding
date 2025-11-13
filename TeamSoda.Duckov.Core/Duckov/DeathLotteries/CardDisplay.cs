using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000309 RID: 777
	public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerMoveHandler
	{
		// Token: 0x0600195F RID: 6495 RVA: 0x0005CA1E File Offset: 0x0005AC1E
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.RefreshFadeGroups();
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0005CA38 File Offset: 0x0005AC38
		private void CacheRadius()
		{
			this.cachedRect = this.rectTransform.rect;
			Rect rect = this.cachedRect;
			this.cachedRadius = Mathf.Sqrt(rect.width * rect.width + rect.height * rect.height) / 2f;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0005CA8D File Offset: 0x0005AC8D
		private void Update()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			this.HandleAnimation();
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0005CAB4 File Offset: 0x0005ACB4
		private void HandleAnimation()
		{
			Quaternion quaternion = this.cardTransform.rotation;
			if ((this.facingFront && !this.frontFadeGroup.IsShown) || (!this.facingFront && !this.backFadeGroup.IsShown))
			{
				quaternion = Quaternion.RotateTowards(quaternion, Quaternion.Euler(0f, 90f, 0f), this.flipSpeed * Time.deltaTime);
				if (Mathf.Approximately(Quaternion.Angle(quaternion, Quaternion.Euler(0f, 90f, 0f)), 0f))
				{
					quaternion = Quaternion.Euler(0f, -90f, 0f);
					this.RefreshFadeGroups();
				}
			}
			else
			{
				quaternion = Quaternion.RotateTowards(quaternion, this.GetIdealRotation(), this.rotateSpeed * Time.deltaTime);
			}
			this.cardTransform.rotation = quaternion;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0005CB8F File Offset: 0x0005AD8F
		private void OnEnable()
		{
			this.CacheRadius();
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0005CB98 File Offset: 0x0005AD98
		private Quaternion GetIdealRotation()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			if (this.hovering && !Mathf.Approximately(this.cachedRadius, 0f))
			{
				Vector2 a;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, this.pointerPosition, null, out a);
				Vector2 center = this.rectTransform.rect.center;
				Vector2 a2 = a - center;
				float d = Mathf.Max(10f, this.cachedRadius);
				Vector2 vector = Vector2.ClampMagnitude(a2 / d, 1f);
				return Quaternion.Euler(-vector.y * this.idleAmp, -vector.x * this.idleAmp, 0f);
			}
			return Quaternion.Euler(Mathf.Sin(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, Mathf.Cos(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, 0f);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0005CCAC File Offset: 0x0005AEAC
		private void SkipAnimation()
		{
			this.RefreshFadeGroups();
			this.cardTransform.rotation = this.GetIdealRotation();
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0005CCC5 File Offset: 0x0005AEC5
		public void SetFacing(bool facingFront, bool skipAnimation = false)
		{
			this.facingFront = facingFront;
			if (skipAnimation)
			{
				this.SkipAnimation();
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0005CCD7 File Offset: 0x0005AED7
		public void Flip()
		{
			this.SetFacing(!this.facingFront, false);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0005CCE9 File Offset: 0x0005AEE9
		private void RefreshFadeGroups()
		{
			if (this.facingFront)
			{
				this.frontFadeGroup.Show();
				this.backFadeGroup.Hide();
				return;
			}
			this.frontFadeGroup.Hide();
			this.backFadeGroup.Show();
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0005CD20 File Offset: 0x0005AF20
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0005CD29 File Offset: 0x0005AF29
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0005CD32 File Offset: 0x0005AF32
		public void OnPointerMove(PointerEventData eventData)
		{
			this.pointerPosition = eventData.position;
		}

		// Token: 0x0400125F RID: 4703
		private RectTransform rectTransform;

		// Token: 0x04001260 RID: 4704
		[SerializeField]
		private RectTransform cardTransform;

		// Token: 0x04001261 RID: 4705
		[SerializeField]
		private FadeGroup frontFadeGroup;

		// Token: 0x04001262 RID: 4706
		[SerializeField]
		private FadeGroup backFadeGroup;

		// Token: 0x04001263 RID: 4707
		[SerializeField]
		private float idleAmp = 10f;

		// Token: 0x04001264 RID: 4708
		[SerializeField]
		private float idleFrequency = 0.5f;

		// Token: 0x04001265 RID: 4709
		[SerializeField]
		private float rotateSpeed = 300f;

		// Token: 0x04001266 RID: 4710
		[SerializeField]
		private float flipSpeed = 300f;

		// Token: 0x04001267 RID: 4711
		private bool facingFront;

		// Token: 0x04001268 RID: 4712
		private bool hovering;

		// Token: 0x04001269 RID: 4713
		private Vector2 pointerPosition;

		// Token: 0x0400126A RID: 4714
		private Rect cachedRect;

		// Token: 0x0400126B RID: 4715
		private float cachedRadius;
	}
}
