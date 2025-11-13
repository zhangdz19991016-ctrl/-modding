using System;

namespace Duckov
{
	// Token: 0x0200023B RID: 571
	[Serializable]
	public struct VersionData
	{
		// Token: 0x060011EC RID: 4588 RVA: 0x000457A0 File Offset: 0x000439A0
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}{3}", new object[]
			{
				this.mainVersion,
				this.subVersion,
				this.buildVersion,
				this.suffix
			});
		}

		// Token: 0x04000DCC RID: 3532
		public int mainVersion;

		// Token: 0x04000DCD RID: 3533
		public int subVersion;

		// Token: 0x04000DCE RID: 3534
		public int buildVersion;

		// Token: 0x04000DCF RID: 3535
		public string suffix;
	}
}
