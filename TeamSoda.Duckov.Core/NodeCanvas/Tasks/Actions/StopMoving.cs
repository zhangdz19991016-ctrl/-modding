using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000421 RID: 1057
	public class StopMoving : ActionTask<AICharacterController>
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x00083C97 File Offset: 0x00081E97
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x00083C9A File Offset: 0x00081E9A
		protected override void OnExecute()
		{
			base.agent.StopMove();
			base.EndAction(true);
		}
	}
}
