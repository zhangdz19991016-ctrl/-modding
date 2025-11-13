using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200040B RID: 1035
	public class CheckNoticed : ConditionTask<AICharacterController>
	{
		// Token: 0x060025AA RID: 9642 RVA: 0x000825A5 File Offset: 0x000807A5
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000825A8 File Offset: 0x000807A8
		protected override void OnEnable()
		{
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000825AA File Offset: 0x000807AA
		protected override void OnDisable()
		{
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000825AC File Offset: 0x000807AC
		protected override bool OnCheck()
		{
			bool result = base.agent.isNoticing(this.noticedTimeThreshold);
			if (this.resetNotice)
			{
				base.agent.noticed = false;
			}
			return result;
		}

		// Token: 0x0400199C RID: 6556
		public float noticedTimeThreshold = 0.2f;

		// Token: 0x0400199D RID: 6557
		public bool resetNotice;
	}
}
