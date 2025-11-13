using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B2 RID: 690
	public class GMA_001 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x0005399C File Offset: 0x00051B9C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			this.cachedRun = base.Run;
			this.staminaModifier = new Modifier(ModifierType.Add, 1f, this);
			this.scoreFactorModifier = new Modifier(ModifierType.Add, 1f, this);
			this.cachedRun.staminaDrain.AddModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.AddModifier(this.scoreFactorModifier);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00053A0E File Offset: 0x00051C0E
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (this.cachedRun == null)
			{
				return;
			}
			this.cachedRun.staminaDrain.RemoveModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.RemoveModifier(this.scoreFactorModifier);
		}

		// Token: 0x040010B3 RID: 4275
		private Modifier staminaModifier;

		// Token: 0x040010B4 RID: 4276
		private Modifier scoreFactorModifier;

		// Token: 0x040010B5 RID: 4277
		private GoldMinerRunData cachedRun;
	}
}
