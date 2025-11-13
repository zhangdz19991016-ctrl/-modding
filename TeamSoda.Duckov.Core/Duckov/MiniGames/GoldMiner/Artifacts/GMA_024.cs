using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C9 RID: 713
	public class GMA_024 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016E3 RID: 5859 RVA: 0x000546DB File Offset: 0x000528DB
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.AddModifier(new Modifier(ModifierType.Add, 1.5f, this));
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00054702 File Offset: 0x00052902
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.RemoveAllModifiersFromSource(this);
		}
	}
}
