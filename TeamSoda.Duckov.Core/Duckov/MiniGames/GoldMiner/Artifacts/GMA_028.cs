using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CD RID: 717
	public class GMA_028 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016EF RID: 5871 RVA: 0x0005481C File Offset: 0x00052A1C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopTicket++;
		}
	}
}
