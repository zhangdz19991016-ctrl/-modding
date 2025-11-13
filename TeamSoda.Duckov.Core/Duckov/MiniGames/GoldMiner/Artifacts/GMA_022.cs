using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C7 RID: 711
	public class GMA_022 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x00054637 File Offset: 0x00052837
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.AddModifier(new Modifier(ModifierType.Add, this.amount, this));
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0005465F File Offset: 0x0005285F
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x040010BE RID: 4286
		[SerializeField]
		private float amount = 0.1f;
	}
}
