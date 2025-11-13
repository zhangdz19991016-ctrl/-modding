using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029F RID: 671
	public class PigBehaviour : MiniGameBehaviour
	{
		// Token: 0x06001619 RID: 5657 RVA: 0x00052254 File Offset: 0x00050454
		private void Awake()
		{
			if (this.entity == null)
			{
				this.entity = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.entity;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000522A4 File Offset: 0x000504A4
		protected override void OnUpdate(float deltaTime)
		{
			Quaternion localRotation = Quaternion.AngleAxis((float)(this.movingRight ? 0 : 180), Vector3.up);
			base.transform.localRotation = localRotation;
			base.transform.localPosition += (this.movingRight ? Vector3.right : Vector3.left) * this.moveSpeed * this.entity.master.run.GameSpeedFactor * deltaTime;
			if (base.transform.localPosition.x > this.entity.master.Bounds.max.x)
			{
				this.movingRight = false;
				return;
			}
			if (base.transform.localPosition.x < this.entity.master.Bounds.min.x)
			{
				this.movingRight = true;
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0005239B File Offset: 0x0005059B
		private void OnAttached(GoldMinerEntity entity, Hook hook)
		{
		}

		// Token: 0x04001049 RID: 4169
		[SerializeField]
		private GoldMinerEntity entity;

		// Token: 0x0400104A RID: 4170
		[SerializeField]
		private float moveSpeed = 50f;

		// Token: 0x0400104B RID: 4171
		private bool attached;

		// Token: 0x0400104C RID: 4172
		private bool movingRight;
	}
}
