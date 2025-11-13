using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002D1 RID: 721
	public class GMA_032 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016F7 RID: 5879 RVA: 0x000548E7 File Offset: 0x00052AE7
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraDiamond = Mathf.MoveTowards(base.Run.extraDiamond, 5f, 0.5f);
		}
	}
}
