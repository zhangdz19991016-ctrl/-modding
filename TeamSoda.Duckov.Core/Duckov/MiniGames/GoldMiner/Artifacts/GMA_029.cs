using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CE RID: 718
	public class GMA_029 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016F1 RID: 5873 RVA: 0x00054842 File Offset: 0x00052A42
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			if (base.Run.shopCapacity >= 6)
			{
				return;
			}
			base.Run.shopCapacity++;
		}
	}
}
