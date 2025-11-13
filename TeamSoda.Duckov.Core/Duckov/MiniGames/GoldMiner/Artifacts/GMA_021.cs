using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C6 RID: 710
	public class GMA_021 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016DB RID: 5851 RVA: 0x00054611 File Offset: 0x00052811
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.eagleEyePotion += 3;
		}
	}
}
