using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BB RID: 699
	public class GMA_010 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016B9 RID: 5817 RVA: 0x000541BB File Offset: 0x000523BB
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 1000;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000541D6 File Offset: 0x000523D6
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 0;
		}
	}
}
