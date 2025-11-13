using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C0 RID: 704
	public class GMA_015 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016C8 RID: 5832 RVA: 0x00054368 File Offset: 0x00052568
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x000543A0 File Offset: 0x000525A0
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000543D8 File Offset: 0x000525D8
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (base.Run == null)
			{
				return;
			}
			if (!base.Run.IsPig(entity))
			{
				return;
			}
			entity.Value += this.amount;
		}

		// Token: 0x040010BC RID: 4284
		[SerializeField]
		private int amount = 20;
	}
}
