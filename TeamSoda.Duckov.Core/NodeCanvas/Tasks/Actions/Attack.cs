using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000410 RID: 1040
	public class Attack : ActionTask<AICharacterController>
	{
		// Token: 0x060025BD RID: 9661 RVA: 0x00082766 File Offset: 0x00080966
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x00082769 File Offset: 0x00080969
		protected override string info
		{
			get
			{
				return string.Format("Attack", Array.Empty<object>());
			}
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x0008277A File Offset: 0x0008097A
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.Attack();
			base.EndAction(true);
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x00082794 File Offset: 0x00080994
		protected override void OnStop()
		{
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x00082796 File Offset: 0x00080996
		protected override void OnPause()
		{
		}
	}
}
