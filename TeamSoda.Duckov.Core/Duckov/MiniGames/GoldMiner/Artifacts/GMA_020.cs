using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C5 RID: 709
	public class GMA_020 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016D9 RID: 5849 RVA: 0x000545EB File Offset: 0x000527EB
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strengthPotion += 3;
		}
	}
}
