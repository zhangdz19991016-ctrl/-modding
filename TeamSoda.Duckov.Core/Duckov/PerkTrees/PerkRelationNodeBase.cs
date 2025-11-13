using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000259 RID: 601
	public class PerkRelationNodeBase : Node
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00047A00 File Offset: 0x00045C00
		public override int maxInConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00047A04 File Offset: 0x00045C04
		public override int maxOutConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00047A08 File Offset: 0x00045C08
		public override Type outConnectionType
		{
			get
			{
				return typeof(PerkRelationConnection);
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00047A14 File Offset: 0x00045C14
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00047A17 File Offset: 0x00045C17
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00047A1A File Offset: 0x00045C1A
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00047A1D File Offset: 0x00045C1D
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}
	}
}
