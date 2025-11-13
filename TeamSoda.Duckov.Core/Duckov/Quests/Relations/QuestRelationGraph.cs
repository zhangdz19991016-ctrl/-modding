using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000364 RID: 868
	[CreateAssetMenu(menuName = "Quests/Relations")]
	public class QuestRelationGraph : Graph
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x0006C26E File Offset: 0x0006A46E
		public override Type baseNodeType
		{
			get
			{
				return typeof(QuestRelationNodeBase);
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0006C27A File Offset: 0x0006A47A
		public override bool requiresAgent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x0006C27D File Offset: 0x0006A47D
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x0006C280 File Offset: 0x0006A480
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x0006C283 File Offset: 0x0006A483
		public override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0006C286 File Offset: 0x0006A486
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x0006C289 File Offset: 0x0006A489
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0006C28C File Offset: 0x0006A48C
		public QuestRelationNode GetNode(int questID)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				QuestRelationNode questRelationNode = node as QuestRelationNode;
				return questRelationNode != null && questRelationNode.questID == questID;
			}) as QuestRelationNode;
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0006C2C4 File Offset: 0x0006A4C4
		public List<int> GetRequiredIDs(int targetID)
		{
			List<int> list = new List<int>();
			QuestRelationNode node = this.GetNode(targetID);
			if (node == null)
			{
				return list;
			}
			foreach (Connection connection in node.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					int questID = questRelationNode.questID;
					list.Add(questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						int questID2 = questRelationProxyNode.questID;
						list.Add(questID2);
					}
				}
			}
			return list;
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0006C36C File Offset: 0x0006A56C
		protected override void OnGraphValidate()
		{
			this.CheckDuplicates();
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0006C374 File Offset: 0x0006A574
		internal void CheckDuplicates()
		{
		}

		// Token: 0x040014D6 RID: 5334
		public static int selectedQuestID = -1;
	}
}
