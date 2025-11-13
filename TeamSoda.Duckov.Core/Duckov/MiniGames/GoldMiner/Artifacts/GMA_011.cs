using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BC RID: 700
	public class GMA_011 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016BC RID: 5820 RVA: 0x000541F5 File Offset: 0x000523F5
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Add(new Func<bool>(this.ForceAndDetach));
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0005421C File Offset: 0x0005241C
		private bool ForceAndDetach()
		{
			base.Run.DetachArtifact(this.master);
			return true;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00054231 File Offset: 0x00052431
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Remove(new Func<bool>(this.ForceAndDetach));
		}
	}
}
