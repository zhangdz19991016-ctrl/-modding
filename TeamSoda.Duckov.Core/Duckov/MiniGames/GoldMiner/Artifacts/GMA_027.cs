using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CC RID: 716
	public class GMA_027 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016EC RID: 5868 RVA: 0x000547D0 File Offset: 0x000529D0
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000547F7 File Offset: 0x000529F7
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.RemoveAllModifiersFromSource(this);
		}
	}
}
