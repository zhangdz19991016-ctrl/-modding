using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000292 RID: 658
	public class Bomb : MiniGameBehaviour
	{
		// Token: 0x0600156A RID: 5482 RVA: 0x0004FA28 File Offset: 0x0004DC28
		protected override void OnUpdate(float deltaTime)
		{
			base.transform.position += base.transform.up * this.moveSpeed * deltaTime;
			this.hoveringTargets.RemoveAll((GoldMinerEntity e) => e == null);
			if (this.hoveringTargets.Count > 0)
			{
				this.Explode(this.hoveringTargets[0]);
			}
			this.lifeTime += deltaTime;
			if (this.lifeTime > this.maxLifeTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0004FAD9 File Offset: 0x0004DCD9
		private void Explode(GoldMinerEntity goldMinerTarget)
		{
			goldMinerTarget.Explode(base.transform.position);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004FB1C File Offset: 0x0004DD1C
		private void OnCollisionEnter2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Add(component);
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0004FB4C File Offset: 0x0004DD4C
		private void OnCollisionExit2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Remove(component);
			}
		}

		// Token: 0x04000FA9 RID: 4009
		[SerializeField]
		private float moveSpeed;

		// Token: 0x04000FAA RID: 4010
		[SerializeField]
		private float maxLifeTime = 10f;

		// Token: 0x04000FAB RID: 4011
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x04000FAC RID: 4012
		private float lifeTime;

		// Token: 0x04000FAD RID: 4013
		private List<GoldMinerEntity> hoveringTargets = new List<GoldMinerEntity>();
	}
}
