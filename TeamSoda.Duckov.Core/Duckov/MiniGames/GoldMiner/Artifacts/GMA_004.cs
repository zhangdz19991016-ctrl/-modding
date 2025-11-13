using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B5 RID: 693
	public class GMA_004 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x00053CEB File Offset: 0x00051EEB
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00053D23 File Offset: 0x00051F23
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00053D5B File Offset: 0x00051F5B
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity) && entity.size > GoldMinerEntity.Size.M)
			{
				base.Run.levelScoreFactor += 0.3f;
			}
		}
	}
}
