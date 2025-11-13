using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C3 RID: 707
	public class GMA_018 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016D2 RID: 5842 RVA: 0x000544B0 File Offset: 0x000526B0
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x0005451C File Offset: 0x0005271C
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00054586 File Offset: 0x00052786
		private void OnLevelBegin(GoldMiner miner)
		{
			this.remaining = 5;
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x0005458F File Offset: 0x0005278F
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (!entity)
			{
				return;
			}
			if (this.remaining < 1)
			{
				return;
			}
			this.remaining--;
			entity.Value = 200;
		}

		// Token: 0x040010BD RID: 4285
		private int remaining;
	}
}
