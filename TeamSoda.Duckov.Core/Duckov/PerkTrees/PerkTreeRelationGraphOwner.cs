using System;
using System.Collections.Generic;
using NodeCanvas.Framework;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025A RID: 602
	public class PerkTreeRelationGraphOwner : GraphOwner<PerkRelationGraph>
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00047A28 File Offset: 0x00045C28
		public PerkRelationGraph RelationGraph
		{
			get
			{
				if (this._relationGraph == null)
				{
					this._relationGraph = (this.graph as PerkRelationGraph);
				}
				return this._relationGraph;
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00047A50 File Offset: 0x00045C50
		public List<Perk> GetRequiredNodes(Perk node)
		{
			PerkRelationNode relatedNode = this.RelationGraph.GetRelatedNode(node);
			if (relatedNode == null)
			{
				return null;
			}
			List<PerkRelationNode> incomingNodes = this.RelationGraph.GetIncomingNodes(relatedNode);
			if (incomingNodes == null)
			{
				return null;
			}
			if (incomingNodes.Count < 1)
			{
				return null;
			}
			List<Perk> list = new List<Perk>();
			foreach (PerkRelationNode perkRelationNode in incomingNodes)
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null))
				{
					list.Add(relatedNode2);
				}
			}
			return list;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00047AE8 File Offset: 0x00045CE8
		internal PerkRelationNode GetRelatedNode(Perk perk)
		{
			if (this.RelationGraph == null)
			{
				return null;
			}
			return this.RelationGraph.GetRelatedNode(perk);
		}

		// Token: 0x04000E4C RID: 3660
		private PerkRelationGraph _relationGraph;
	}
}
