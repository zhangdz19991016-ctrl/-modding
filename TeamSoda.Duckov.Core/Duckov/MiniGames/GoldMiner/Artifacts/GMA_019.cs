using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C4 RID: 708
	public class GMA_019 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016D7 RID: 5847 RVA: 0x000545C5 File Offset: 0x000527C5
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.bomb += 3;
		}
	}
}
