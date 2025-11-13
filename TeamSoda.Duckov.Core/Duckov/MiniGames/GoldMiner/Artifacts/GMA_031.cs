using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D0 RID: 720
	public class GMA_031 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016F5 RID: 5877 RVA: 0x000548AF File Offset: 0x00052AAF
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraGold = Mathf.MoveTowards(base.Run.extraGold, 5f, 1f);
		}
	}
}
