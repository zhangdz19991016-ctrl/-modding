using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C2 RID: 706
	public class GMA_017 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016CF RID: 5839 RVA: 0x00054461 File Offset: 0x00052661
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.AddModifier(new Modifier(ModifierType.Add, 0.2f, this));
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00054488 File Offset: 0x00052688
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
