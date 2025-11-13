using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000393 RID: 915
	public class PunchReceiver : MonoBehaviour
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00070033 File Offset: 0x0006E233
		private float PunchAnchorPositionDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x0007003B File Offset: 0x0006E23B
		private float PunchScaleDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x00070043 File Offset: 0x0006E243
		private float PunchRotationDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0007004B File Offset: 0x0006E24B
		private bool ShouldPunchPosition
		{
			get
			{
				return this.randomAnchorPosition.magnitude > 0.001f && this.punchAnchorPosition.magnitude > 0.001f;
			}
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00070073 File Offset: 0x0006E273
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.CachePose();
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00070095 File Offset: 0x0006E295
		private void Start()
		{
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00070098 File Offset: 0x0006E298
		[ContextMenu("Punch")]
		public void Punch()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.particle != null)
			{
				this.particle.Play();
			}
			this.rectTransform.DOKill(false);
			if (this.cacheWhenPunched)
			{
				this.CachePose();
			}
			Vector2 punch = this.punchAnchorPosition + new Vector2(UnityEngine.Random.Range(-this.randomAnchorPosition.x, this.randomAnchorPosition.x), UnityEngine.Random.Range(-this.randomAnchorPosition.y, this.randomAnchorPosition.y));
			float d = this.punchScaleUniform;
			float d2 = this.punchRotationZ + UnityEngine.Random.Range(-this.randomRotationZ, this.randomRotationZ);
			if (this.ShouldPunchPosition)
			{
				this.rectTransform.DOPunchAnchorPos(punch, this.PunchAnchorPositionDuration, this.vibrato, this.elasticity, false).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			}
			this.rectTransform.DOPunchScale(Vector3.one * d, this.PunchScaleDuration, this.vibrato, this.elasticity).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			this.rectTransform.DOPunchRotation(Vector3.forward * d2, this.PunchRotationDuration, this.vibrato, this.elasticity).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			if (!string.IsNullOrWhiteSpace(this.sfx))
			{
				AudioManager.Post(this.sfx);
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00070244 File Offset: 0x0006E444
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchorPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000702A8 File Offset: 0x0006E4A8
		private void RestorePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.ShouldPunchPosition)
			{
				this.rectTransform.anchoredPosition = this.cachedAnchorPosition;
			}
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0007030E File Offset: 0x0006E50E
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.DOKill(false);
		}

		// Token: 0x040015C3 RID: 5571
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040015C4 RID: 5572
		[SerializeField]
		private ParticleSystem particle;

		// Token: 0x040015C5 RID: 5573
		[Min(0.0001f)]
		[SerializeField]
		private float duration = 0.01f;

		// Token: 0x040015C6 RID: 5574
		public int vibrato = 10;

		// Token: 0x040015C7 RID: 5575
		public float elasticity = 1f;

		// Token: 0x040015C8 RID: 5576
		[SerializeField]
		private Vector2 punchAnchorPosition;

		// Token: 0x040015C9 RID: 5577
		[SerializeField]
		[Range(-1f, 1f)]
		private float punchScaleUniform;

		// Token: 0x040015CA RID: 5578
		[SerializeField]
		[Range(-180f, 180f)]
		private float punchRotationZ;

		// Token: 0x040015CB RID: 5579
		[SerializeField]
		private Vector2 randomAnchorPosition;

		// Token: 0x040015CC RID: 5580
		[SerializeField]
		[Range(0f, 180f)]
		private float randomRotationZ;

		// Token: 0x040015CD RID: 5581
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040015CE RID: 5582
		[SerializeField]
		private bool cacheWhenPunched;

		// Token: 0x040015CF RID: 5583
		[SerializeField]
		private string sfx;

		// Token: 0x040015D0 RID: 5584
		private Vector2 cachedAnchorPosition;

		// Token: 0x040015D1 RID: 5585
		private Vector2 cachedScale;

		// Token: 0x040015D2 RID: 5586
		private Vector2 cachedRotation;
	}
}
