using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CB RID: 715
	public class GMA_026 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016E9 RID: 5865 RVA: 0x00054780 File Offset: 0x00052980
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.AddModifier(new Modifier(ModifierType.PercentageMultiply, -1f, this));
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000547AB File Offset: 0x000529AB
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.RemoveAllModifiersFromSource(this);
		}
	}
}
