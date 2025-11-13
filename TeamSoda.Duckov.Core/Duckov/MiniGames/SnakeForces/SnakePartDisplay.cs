using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x0200028F RID: 655
	public class SnakePartDisplay : MiniGameBehaviour
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0004F6C1 File Offset: 0x0004D8C1
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x0004F6C9 File Offset: 0x0004D8C9
		public SnakeDisplay Master { get; private set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0004F6D2 File Offset: 0x0004D8D2
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x0004F6DA File Offset: 0x0004D8DA
		public SnakeForce.Part Target { get; private set; }

		// Token: 0x0600154E RID: 5454 RVA: 0x0004F6E4 File Offset: 0x0004D8E4
		internal void Setup(SnakeDisplay master, SnakeForce.Part part)
		{
			if (this.Target != null)
			{
				this.Target.OnMove -= this.OnTargetMove;
			}
			this.Master = master;
			this.Target = part;
			this.cachedCoord = this.Target.coord;
			base.transform.localPosition = this.Master.GetPosition(this.cachedCoord);
			this.Target.OnMove += this.OnTargetMove;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0004F764 File Offset: 0x0004D964
		private void OnTargetMove(SnakeForce.Part part)
		{
			if (!base.enabled)
			{
				return;
			}
			int sqrMagnitude = (this.Target.coord - this.cachedCoord).sqrMagnitude;
			this.cachedCoord = this.Target.coord;
			Vector3 position = this.Master.GetPosition(this.cachedCoord);
			this.DoMove(position);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0004F7C3 File Offset: 0x0004D9C3
		private void DoMove(Vector3 vector3)
		{
			base.transform.localPosition = vector3;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0004F7D4 File Offset: 0x0004D9D4
		internal void Punch()
		{
			base.transform.DOKill(true);
			base.transform.localScale = Vector3.one;
			base.transform.DOPunchScale(Vector3.one * 1.1f, 0.2f, 4, 1f);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0004F824 File Offset: 0x0004DA24
		internal void PunchColor(Color color)
		{
			this.image.DOKill(false);
			this.image.color = color;
			this.image.DOColor(Color.white, 0.4f);
		}

		// Token: 0x04000F9E RID: 3998
		[SerializeField]
		private Image image;

		// Token: 0x04000F9F RID: 3999
		private Vector2Int cachedCoord;
	}
}
