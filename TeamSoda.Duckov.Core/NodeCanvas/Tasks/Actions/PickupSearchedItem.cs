using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000415 RID: 1045
	public class PickupSearchedItem : ActionTask<AICharacterController>
	{
		// Token: 0x060025DE RID: 9694 RVA: 0x00082EAA File Offset: 0x000810AA
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00082EB0 File Offset: 0x000810B0
		protected override void OnExecute()
		{
			if (base.agent == null || base.agent.CharacterMainControl == null || base.agent.searchedPickup == null)
			{
				base.EndAction(false);
				return;
			}
			if (Vector3.Distance(base.agent.transform.position, base.agent.searchedPickup.transform.position) > 1.5f)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedPickup.ItemAgent != null)
			{
				base.agent.CharacterMainControl.PickupItem(base.agent.searchedPickup.ItemAgent.Item);
			}
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00082F70 File Offset: 0x00081170
		protected override void OnUpdate()
		{
		}
	}
}
