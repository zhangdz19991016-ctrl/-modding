using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BF RID: 703
	public class GMA_014 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016C4 RID: 5828 RVA: 0x000542B5 File Offset: 0x000524B5
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000542ED File Offset: 0x000524ED
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00054325 File Offset: 0x00052525
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run == null)
			{
				return;
			}
			if (!base.Run.IsPig(entity))
			{
				return;
			}
			base.Run.stamina += 2f;
		}
	}
}
