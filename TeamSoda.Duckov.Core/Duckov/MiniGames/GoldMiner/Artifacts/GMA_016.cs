using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C1 RID: 705
	public class GMA_016 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x00054415 File Offset: 0x00052615
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x0005443C File Offset: 0x0005263C
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
