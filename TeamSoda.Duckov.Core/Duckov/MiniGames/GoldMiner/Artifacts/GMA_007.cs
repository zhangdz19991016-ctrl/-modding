using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B8 RID: 696
	public class GMA_007 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x00053EEA File Offset: 0x000520EA
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Add(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00053F11 File Offset: 0x00052111
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Remove(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00053F3C File Offset: 0x0005213C
		private float AddFactorIfResolved3DifferentKindsOfGold()
		{
			if ((from e in base.GoldMiner.resolvedEntities
			where e != null && e.tags.Contains(GoldMinerEntity.Tag.Gold)
			group e by e.size).Count<IGrouping<GoldMinerEntity.Size, GoldMinerEntity>>() >= 3)
			{
				return 0.5f;
			}
			return 0f;
		}
	}
}
