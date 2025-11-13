using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BA RID: 698
	public class GMA_009 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x000540EC File Offset: 0x000522EC
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00054124 File Offset: 0x00052324
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0005415C File Offset: 0x0005235C
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				this.effectActive = true;
			}
			if (this.effectActive && base.Run.IsGold(entity))
			{
				this.effectActive = false;
				entity.Value *= 2;
			}
		}

		// Token: 0x040010BB RID: 4283
		private bool effectActive;
	}
}
