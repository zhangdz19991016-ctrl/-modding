using System;
using Duckov.Economy;
using ItemStatsSystem;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024F RID: 591
	[Serializable]
	public class PerkRequirement
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x00047020 File Offset: 0x00045220
		public TimeSpan RequireTime
		{
			get
			{
				return TimeSpan.FromTicks(this.requireTime);
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0004702D File Offset: 0x0004522D
		internal bool AreSatisfied()
		{
			return this.level <= EXPManager.Level && this.cost.Enough;
		}

		// Token: 0x04000E35 RID: 3637
		public int level;

		// Token: 0x04000E36 RID: 3638
		public Cost cost;

		// Token: 0x04000E37 RID: 3639
		[TimeSpan]
		public long requireTime;

		// Token: 0x0200053D RID: 1341
		[Serializable]
		public class RequireItemEntry
		{
			// Token: 0x04001EC5 RID: 7877
			[ItemTypeID]
			public int id = 1;

			// Token: 0x04001EC6 RID: 7878
			public int amount = 1;
		}
	}
}
