using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C8 RID: 712
	public class GMA_023 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016E0 RID: 5856 RVA: 0x0005468F File Offset: 0x0005288F
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.AddModifier(new Modifier(ModifierType.Add, 10f, this));
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x000546B6 File Offset: 0x000528B6
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.RemoveAllModifiersFromSource(this);
		}
	}
}
