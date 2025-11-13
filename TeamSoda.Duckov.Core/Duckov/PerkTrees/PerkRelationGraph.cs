using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000257 RID: 599
	public class PerkRelationGraph : Graph
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x000477FD File Offset: 0x000459FD
		public override Type baseNodeType
		{
			get
			{
				return typeof(PerkRelationNodeBase);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x00047809 File Offset: 0x00045A09
		public override bool requiresAgent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0004780C File Offset: 0x00045A0C
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0004780F File Offset: 0x00045A0F
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x00047812 File Offset: 0x00045A12
		public override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x00047815 File Offset: 0x00045A15
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00047818 File Offset: 0x00045A18
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0004781C File Offset: 0x00045A1C
		public PerkRelationNode GetRelatedNode(Perk perk)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				if (node == null)
				{
					return false;
				}
				PerkRelationNode perkRelationNode = node as PerkRelationNode;
				return perkRelationNode != null && perkRelationNode.relatedNode == perk;
			}) as PerkRelationNode;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00047854 File Offset: 0x00045A54
		public List<PerkRelationNode> GetIncomingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.inConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.sourceNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x000478C0 File Offset: 0x00045AC0
		public List<PerkRelationNode> GetOutgoingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.outConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}
	}
}
