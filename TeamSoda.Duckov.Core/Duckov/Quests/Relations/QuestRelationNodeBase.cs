using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000365 RID: 869
	public class QuestRelationNodeBase : Node
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x0006C386 File Offset: 0x0006A586
		public override int maxInConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0006C38A File Offset: 0x0006A58A
		public override int maxOutConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x0006C38E File Offset: 0x0006A58E
		public override Type outConnectionType
		{
			get
			{
				return typeof(QuestRelationConnection);
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0006C39A File Offset: 0x0006A59A
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x0006C39D File Offset: 0x0006A59D
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x0006C3A0 File Offset: 0x0006A5A0
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x0006C3A3 File Offset: 0x0006A5A3
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}
	}
}
