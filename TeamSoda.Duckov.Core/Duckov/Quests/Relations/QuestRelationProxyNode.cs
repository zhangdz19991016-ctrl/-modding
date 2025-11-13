using System;
using Duckov.Utilities;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000367 RID: 871
	public class QuestRelationProxyNode : QuestRelationNodeBase
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x0006C4D4 File Offset: 0x0006A6D4
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x0006C4D7 File Offset: 0x0006A6D7
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationProxyNode._questCollection == null)
				{
					QuestRelationProxyNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationProxyNode._questCollection;
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0006C4F5 File Offset: 0x0006A6F5
		private void SelectQuest()
		{
		}

		// Token: 0x040014DA RID: 5338
		private static QuestCollection _questCollection;

		// Token: 0x040014DB RID: 5339
		public int questID;
	}
}
