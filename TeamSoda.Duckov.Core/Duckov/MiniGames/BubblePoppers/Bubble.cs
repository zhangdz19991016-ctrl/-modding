using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002DE RID: 734
	public class Bubble : MiniGameBehaviour
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x000555E1 File Offset: 0x000537E1
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x000555E9 File Offset: 0x000537E9
		public BubblePopper Master { get; private set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x000555F2 File Offset: 0x000537F2
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x000555FA File Offset: 0x000537FA
		public int ColorIndex
		{
			get
			{
				return this.colorIndex;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00055602 File Offset: 0x00053802
		public Color DisplayColor
		{
			get
			{
				if (this.Master == null)
				{
					return Color.white;
				}
				return this.Master.GetDisplayColor(this.ColorIndex);
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00055629 File Offset: 0x00053829
		// (set) Token: 0x06001744 RID: 5956 RVA: 0x00055631 File Offset: 0x00053831
		public Vector2Int Coord { get; internal set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x0005563A File Offset: 0x0005383A
		// (set) Token: 0x06001746 RID: 5958 RVA: 0x00055642 File Offset: 0x00053842
		public Vector2 MoveDirection { get; internal set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x0005564B File Offset: 0x0005384B
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x00055653 File Offset: 0x00053853
		public Vector2 Velocity { get; internal set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x0005565C File Offset: 0x0005385C
		// (set) Token: 0x0600174A RID: 5962 RVA: 0x00055664 File Offset: 0x00053864
		public Bubble.Status status { get; private set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x0005566D File Offset: 0x0005386D
		// (set) Token: 0x0600174C RID: 5964 RVA: 0x0005567F File Offset: 0x0005387F
		private Vector2 gPos
		{
			get
			{
				return this.graphicsRoot.localPosition;
			}
			set
			{
				this.graphicsRoot.localPosition = value;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00055694 File Offset: 0x00053894
		private Vector2 gForce
		{
			get
			{
				return (new Vector2(Mathf.PerlinNoise(7.3f, Time.time * this.vibrationFrequency) * 2f - 1f, Mathf.PerlinNoise(0.3f, Time.time * this.vibrationFrequency) * 2f - 1f) * this.vibrationAmp - this.gPos) * this.gSpring;
			}
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0005570B File Offset: 0x0005390B
		internal void Setup(BubblePopper master, int colorIndex)
		{
			this.Master = master;
			this.colorIndex = colorIndex;
			this.RefreshColor();
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00055721 File Offset: 0x00053921
		public void RefreshColor()
		{
			this.image.color = this.DisplayColor;
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00055734 File Offset: 0x00053934
		internal void Launch(Vector2 direction)
		{
			this.MoveDirection = direction;
			this.status = Bubble.Status.Moving;
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00055744 File Offset: 0x00053944
		internal void NotifyExplode(Vector2Int origin)
		{
			this.status = Bubble.Status.Explode;
			Vector2Int v = this.Coord - origin;
			float magnitude = v.magnitude;
			this.explodeETA = magnitude * 0.025f;
			this.Impact(v.normalized * 5f);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00055798 File Offset: 0x00053998
		internal void NotifyAttached(Vector2Int coord)
		{
			Vector2 v = this.Master.Layout.CoordToLocalPosition(coord);
			base.transform.position = this.Master.Layout.transform.localToWorldMatrix.MultiplyPoint(v);
			this.status = Bubble.Status.Attached;
			this.Coord = coord;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000557F3 File Offset: 0x000539F3
		public void NotifyDetached()
		{
			this.status = Bubble.Status.Detached;
			this.Velocity = Vector2.zero;
			this.explodeCountDown = this.explodeAfterDetachedFor;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00055813 File Offset: 0x00053A13
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.UpdateGraphics(deltaTime);
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x00055823 File Offset: 0x00053A23
		private void UpdateLogic(float deltaTime)
		{
			if (this.Master == null)
			{
				return;
			}
			if (this.Master.Busy)
			{
				return;
			}
			if (this.status == Bubble.Status.Moving)
			{
				this.Master.MoveBubble(this, deltaTime);
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00055858 File Offset: 0x00053A58
		private void UpdateGraphics(float deltaTime)
		{
			if (this.status == Bubble.Status.Explode)
			{
				this.explodeETA -= deltaTime;
				if (this.explodeETA <= 0f)
				{
					FXPool.Play(this.explodeFXrefab, base.transform.position, base.transform.rotation, this.DisplayColor);
					this.Master.Release(this);
				}
			}
			if (this.status == Bubble.Status.Detached)
			{
				base.transform.localPosition += this.Velocity * deltaTime;
				this.Velocity += -Vector2.up * this.gravity;
				this.explodeCountDown -= deltaTime;
				if (this.explodeCountDown <= 0f)
				{
					this.NotifyExplode(this.Coord);
				}
			}
			this.UpdateElasticMovement(deltaTime);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00055944 File Offset: 0x00053B44
		private void UpdateElasticMovement(float deltaTime)
		{
			float num = (Vector2.Dot(this.gVelocity, this.gForce.normalized) < 0f) ? this.gDamping : 1f;
			this.gVelocity += this.gForce * deltaTime;
			this.gVelocity = Vector2.MoveTowards(this.gVelocity, Vector2.zero, num * this.gVelocity.magnitude * deltaTime);
			this.gPos += this.gVelocity;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x000559D8 File Offset: 0x00053BD8
		public void Impact(Vector2 velocity)
		{
			this.gVelocity = velocity;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000559E1 File Offset: 0x00053BE1
		internal void Rest()
		{
			this.gPos = Vector2.zero;
			this.gVelocity = Vector2.zero;
		}

		// Token: 0x040010F7 RID: 4343
		[SerializeField]
		private float radius;

		// Token: 0x040010F8 RID: 4344
		[SerializeField]
		private int colorIndex;

		// Token: 0x040010F9 RID: 4345
		[SerializeField]
		private float gravity;

		// Token: 0x040010FA RID: 4346
		[SerializeField]
		private float explodeAfterDetachedFor = 1f;

		// Token: 0x040010FB RID: 4347
		[SerializeField]
		private ParticleSystem explodeFXrefab;

		// Token: 0x040010FC RID: 4348
		[SerializeField]
		private Image image;

		// Token: 0x040010FD RID: 4349
		[SerializeField]
		private RectTransform graphicsRoot;

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		private float gSpring = 1f;

		// Token: 0x040010FF RID: 4351
		[SerializeField]
		private float gDamping = 10f;

		// Token: 0x04001100 RID: 4352
		[SerializeField]
		private float vibrationFrequency = 10f;

		// Token: 0x04001101 RID: 4353
		[SerializeField]
		private float vibrationAmp = 4f;

		// Token: 0x04001106 RID: 4358
		private float explodeETA;

		// Token: 0x04001107 RID: 4359
		private float explodeCountDown;

		// Token: 0x04001108 RID: 4360
		private Vector2 gVelocity;

		// Token: 0x0200057C RID: 1404
		public enum Status
		{
			// Token: 0x04001FC0 RID: 8128
			Idle,
			// Token: 0x04001FC1 RID: 8129
			Moving,
			// Token: 0x04001FC2 RID: 8130
			Attached,
			// Token: 0x04001FC3 RID: 8131
			Detached,
			// Token: 0x04001FC4 RID: 8132
			Explode
		}
	}
}
