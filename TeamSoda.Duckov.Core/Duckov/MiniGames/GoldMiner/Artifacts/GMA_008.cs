using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B9 RID: 697
	public class GMA_008 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016B0 RID: 5808 RVA: 0x00053FB8 File Offset: 0x000521B8
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00054024 File Offset: 0x00052224
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0005408E File Offset: 0x0005228E
		private void OnLevelBegin(GoldMiner miner)
		{
			this.triggered = false;
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00054098 File Offset: 0x00052298
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.triggered)
			{
				return;
			}
			if (base.GoldMiner.activeEntities.Count <= 0)
			{
				this.triggered = true;
				base.Run.charm.BaseValue += 0.5f;
			}
		}

		// Token: 0x040010BA RID: 4282
		private bool triggered;
	}
}
