using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CF RID: 719
	public class GMA_030 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016F3 RID: 5875 RVA: 0x00054877 File Offset: 0x00052A77
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraRocks = Mathf.MoveTowards(base.Run.extraRocks, 5f, 1f);
		}
	}
}
