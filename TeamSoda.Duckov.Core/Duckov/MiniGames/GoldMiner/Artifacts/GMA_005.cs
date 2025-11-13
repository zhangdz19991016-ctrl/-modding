using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B6 RID: 694
	public class GMA_005 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A4 RID: 5796 RVA: 0x00053D9D File Offset: 0x00051F9D
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00053DD5 File Offset: 0x00051FD5
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00053E10 File Offset: 0x00052010
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.remaining < 1)
			{
				return;
			}
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity) && entity.size < GoldMinerEntity.Size.M)
			{
				entity.Value += 500;
				this.remaining--;
			}
		}

		// Token: 0x040010B9 RID: 4281
		private int remaining = 3;
	}
}
