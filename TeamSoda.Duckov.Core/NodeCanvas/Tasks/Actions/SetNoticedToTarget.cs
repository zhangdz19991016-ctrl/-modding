using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041D RID: 1053
	public class SetNoticedToTarget : ActionTask<AICharacterController>
	{
		// Token: 0x06002611 RID: 9745 RVA: 0x000839C1 File Offset: 0x00081BC1
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x000839C4 File Offset: 0x00081BC4
		protected override string info
		{
			get
			{
				return "set noticed to";
			}
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000839CB File Offset: 0x00081BCB
		protected override void OnExecute()
		{
			base.agent.SetNoticedToTarget(this.target.value);
			base.EndAction(true);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000839EA File Offset: 0x00081BEA
		protected override void OnStop()
		{
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000839EC File Offset: 0x00081BEC
		protected override void OnPause()
		{
		}

		// Token: 0x040019E4 RID: 6628
		public BBParameter<DamageReceiver> target;
	}
}
