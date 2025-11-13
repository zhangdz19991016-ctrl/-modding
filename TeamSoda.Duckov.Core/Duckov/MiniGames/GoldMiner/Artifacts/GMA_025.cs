using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CA RID: 714
	public class GMA_025 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x00054727 File Offset: 0x00052927
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.AddModifier(new Modifier(ModifierType.PercentageAdd, this.addAmount, this));
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00054750 File Offset: 0x00052950
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x040010BF RID: 4287
		[SerializeField]
		private float addAmount = 1f;
	}
}
