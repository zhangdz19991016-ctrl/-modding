using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000258 RID: 600
	public class PerkRelationNode : PerkRelationNodeBase
	{
		// Token: 0x060012E0 RID: 4832 RVA: 0x00047934 File Offset: 0x00045B34
		internal void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00047940 File Offset: 0x00045B40
		public override void OnDestroy()
		{
			if (this.relatedNode == null)
			{
				return;
			}
			IEnumerable<Node> enumerable = from e in base.graph.allNodes
			where (e as PerkRelationNode).relatedNode == this.relatedNode
			select e;
			if (enumerable.Count<Node>() <= 2)
			{
				foreach (Node node in enumerable)
				{
					PerkRelationNode perkRelationNode = node as PerkRelationNode;
					if (perkRelationNode != null)
					{
						perkRelationNode.isDuplicate = false;
						perkRelationNode.SetDirty();
					}
				}
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x000479CC File Offset: 0x00045BCC
		internal void NotifyIncomingStateChanged()
		{
			this.relatedNode.NotifyParentStateChanged();
		}

		// Token: 0x04000E47 RID: 3655
		public Perk relatedNode;

		// Token: 0x04000E48 RID: 3656
		public Vector2 cachedPosition;

		// Token: 0x04000E49 RID: 3657
		private bool dirty = true;

		// Token: 0x04000E4A RID: 3658
		internal bool isDuplicate;

		// Token: 0x04000E4B RID: 3659
		internal bool isInvalid;
	}
}
