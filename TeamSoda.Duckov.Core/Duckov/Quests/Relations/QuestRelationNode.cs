using System;
using System.Collections.Generic;
using Duckov.Utilities;
using NodeCanvas.Framework;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000366 RID: 870
	public class QuestRelationNode : QuestRelationNodeBase
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x0006C3AE File Offset: 0x0006A5AE
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationNode._questCollection == null)
				{
					QuestRelationNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationNode._questCollection;
			}
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0006C3CC File Offset: 0x0006A5CC
		private void SelectQuest()
		{
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0006C3D0 File Offset: 0x0006A5D0
		public List<int> GetParents()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						list.Add(questRelationProxyNode.questID);
					}
				}
			}
			return list;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0006C460 File Offset: 0x0006A660
		public List<int> GetChildren()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.outConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
			}
			return list;
		}

		// Token: 0x040014D7 RID: 5335
		public int questID;

		// Token: 0x040014D8 RID: 5336
		private static QuestCollection _questCollection;

		// Token: 0x040014D9 RID: 5337
		internal bool isDuplicate;
	}
}
