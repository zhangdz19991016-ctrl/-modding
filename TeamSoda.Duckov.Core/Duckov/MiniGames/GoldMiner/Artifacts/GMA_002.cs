using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B3 RID: 691
	public class GMA_002 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001696 RID: 5782 RVA: 0x00053A50 File Offset: 0x00051C50
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (this.master == null)
			{
				return;
			}
			if (base.GoldMiner == null)
			{
				return;
			}
			this.modifer = new Modifier(ModifierType.PercentageMultiply, -0.5f, this);
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00053B08 File Offset: 0x00051D08
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
			if (base.Run != null)
			{
				base.Run.staminaDrain.RemoveModifier(this.modifer);
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00053BB8 File Offset: 0x00051DB8
		private void OnBeginRetrieve(GoldMiner miner, Hook hook)
		{
			if (!this.effectActive)
			{
				return;
			}
			base.Run.staminaDrain.AddModifier(this.modifer);
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00053BD9 File Offset: 0x00051DD9
		private void OnEndRetrieve(GoldMiner miner, Hook hook)
		{
			base.Run.staminaDrain.RemoveModifier(this.modifer);
			this.effectActive = false;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00053BF9 File Offset: 0x00051DF9
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.effectActive = true;
		}

		// Token: 0x040010B6 RID: 4278
		private Modifier modifer;

		// Token: 0x040010B7 RID: 4279
		private bool effectActive;
	}
}
