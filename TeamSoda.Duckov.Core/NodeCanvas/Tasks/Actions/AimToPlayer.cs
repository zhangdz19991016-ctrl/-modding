using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040F RID: 1039
	public class AimToPlayer : ActionTask<AICharacterController>
	{
		// Token: 0x060025B9 RID: 9657 RVA: 0x0008271F File Offset: 0x0008091F
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x00082722 File Offset: 0x00080922
		protected override void OnExecute()
		{
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00082724 File Offset: 0x00080924
		protected override void OnUpdate()
		{
			if (!this.target)
			{
				this.target = CharacterMainControl.Main;
			}
			base.agent.CharacterMainControl.SetAimPoint(this.target.transform.position);
		}

		// Token: 0x040019A7 RID: 6567
		private CharacterMainControl target;
	}
}
