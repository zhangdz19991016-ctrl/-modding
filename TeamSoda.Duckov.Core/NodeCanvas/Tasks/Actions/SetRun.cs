using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041E RID: 1054
	public class SetRun : ActionTask<AICharacterController>
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000839F6 File Offset: 0x00081BF6
		protected override string info
		{
			get
			{
				return string.Format("SetRun:{0}", this.run.value);
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x00083A12 File Offset: 0x00081C12
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x00083A15 File Offset: 0x00081C15
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetRunInput(this.run.value);
			base.EndAction(true);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00083A39 File Offset: 0x00081C39
		protected override void OnStop()
		{
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x00083A3B File Offset: 0x00081C3B
		protected override void OnPause()
		{
		}

		// Token: 0x040019E5 RID: 6629
		public BBParameter<bool> run;
	}
}
