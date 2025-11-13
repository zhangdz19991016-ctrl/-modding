using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B4 RID: 692
	public class GMA_003 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600169C RID: 5788 RVA: 0x00053C0A File Offset: 0x00051E0A
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00053C42 File Offset: 0x00051E42
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00053C7C File Offset: 0x00051E7C
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				Debug.Log("Enity is Rock ", entity);
				this.streak++;
			}
			else
			{
				this.streak = 0;
			}
			if (this.streak > 1)
			{
				base.Run.levelScoreFactor += 0.1f;
			}
		}

		// Token: 0x040010B8 RID: 4280
		private int streak;
	}
}
