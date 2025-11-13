using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B7 RID: 695
	public class GMA_006 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x00053E77 File Offset: 0x00052077
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Add(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00053E9E File Offset: 0x0005209E
		private bool SmallRockIsGold(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock) && entity.size < GoldMinerEntity.Size.M;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00053EBA File Offset: 0x000520BA
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Remove(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}
	}
}
