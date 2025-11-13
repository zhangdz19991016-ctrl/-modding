using System;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000297 RID: 663
	[Serializable]
	public class ShopEntity
	{
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00051378 File Offset: 0x0004F578
		public string ID
		{
			get
			{
				if (!this.artifact)
				{
					return null;
				}
				return this.artifact.ID;
			}
		}

		// Token: 0x04001016 RID: 4118
		public GoldMinerArtifact artifact;

		// Token: 0x04001017 RID: 4119
		public bool locked;

		// Token: 0x04001018 RID: 4120
		public bool sold;

		// Token: 0x04001019 RID: 4121
		public float priceFactor = 1f;
	}
}
