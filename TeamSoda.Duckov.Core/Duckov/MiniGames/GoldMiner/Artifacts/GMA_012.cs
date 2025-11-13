using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BD RID: 701
	public class GMA_012 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016C0 RID: 5824 RVA: 0x00054261 File Offset: 0x00052461
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00054288 File Offset: 0x00052488
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.RemoveAllModifiersFromSource(this);
		}
	}
}
