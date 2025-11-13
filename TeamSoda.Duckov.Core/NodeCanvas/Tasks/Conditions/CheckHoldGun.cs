using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000409 RID: 1033
	public class CheckHoldGun : ConditionTask<AICharacterController>
	{
		// Token: 0x060025A0 RID: 9632 RVA: 0x000824F9 File Offset: 0x000806F9
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000824FC File Offset: 0x000806FC
		protected override void OnEnable()
		{
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000824FE File Offset: 0x000806FE
		protected override void OnDisable()
		{
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00082500 File Offset: 0x00080700
		protected override bool OnCheck()
		{
			return base.agent.CharacterMainControl.GetGun() != null;
		}
	}
}
