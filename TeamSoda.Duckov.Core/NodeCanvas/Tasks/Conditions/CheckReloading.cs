using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200040C RID: 1036
	public class CheckReloading : ConditionTask<AICharacterController>
	{
		// Token: 0x060025AF RID: 9647 RVA: 0x000825E6 File Offset: 0x000807E6
		protected override bool OnCheck()
		{
			return !(base.agent == null) && !(base.agent.CharacterMainControl == null) && base.agent.CharacterMainControl.reloadAction.Running;
		}
	}
}
