using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000255 RID: 597
	public class PerkLevelLineNode : PerkRelationNodeBase
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x000477DF File Offset: 0x000459DF
		public string DisplayName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x000477E7 File Offset: 0x000459E7
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x000477EA File Offset: 0x000459EA
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04000E46 RID: 3654
		public Vector2 cachedPosition;
	}
}
