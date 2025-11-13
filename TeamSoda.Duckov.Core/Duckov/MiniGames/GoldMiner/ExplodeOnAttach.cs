using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029E RID: 670
	public class ExplodeOnAttach : MiniGameBehaviour
	{
		// Token: 0x06001615 RID: 5653 RVA: 0x00052118 File Offset: 0x00050318
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			if (this.goldMiner == null)
			{
				this.goldMiner = base.GetComponentInParent<GoldMiner>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00052180 File Offset: 0x00050380
		private void OnAttached(GoldMinerEntity target, Hook hook)
		{
			if (this.goldMiner == null)
			{
				return;
			}
			if (this.goldMiner.run == null)
			{
				return;
			}
			if (this.goldMiner.run.defuse.Value > 0.1f)
			{
				return;
			}
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, this.explodeRange);
			for (int i = 0; i < array.Length; i++)
			{
				GoldMinerEntity component = array[i].GetComponent<GoldMinerEntity>();
				if (!(component == null))
				{
					component.Explode(base.transform.position);
				}
			}
			this.master.Explode(base.transform.position);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0005222A File Offset: 0x0005042A
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.explodeRange);
		}

		// Token: 0x04001046 RID: 4166
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x04001047 RID: 4167
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001048 RID: 4168
		[SerializeField]
		private float explodeRange;
	}
}
