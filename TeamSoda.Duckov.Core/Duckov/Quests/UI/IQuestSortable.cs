using System;

namespace Duckov.Quests.UI
{
	// Token: 0x02000351 RID: 849
	public interface IQuestSortable
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001D8D RID: 7565
		// (set) Token: 0x06001D8E RID: 7566
		Quest.SortingMode SortingMode { get; set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001D8F RID: 7567
		// (set) Token: 0x06001D90 RID: 7568
		bool SortRevert { get; set; }
	}
}
