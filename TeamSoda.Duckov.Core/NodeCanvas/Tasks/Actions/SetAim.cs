using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041C RID: 1052
	public class SetAim : ActionTask<AICharacterController>
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600260A RID: 9738 RVA: 0x000838B4 File Offset: 0x00081AB4
		protected override string info
		{
			get
			{
				if (this.useTransfom && string.IsNullOrEmpty(this.aimTarget.name))
				{
					return "Set aim to null";
				}
				if (!this.useTransfom)
				{
					return "Set aim to " + this.aimPos.name;
				}
				return "Set aim to " + this.aimTarget.name;
			}
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00083914 File Offset: 0x00081B14
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00083918 File Offset: 0x00081B18
		protected override void OnExecute()
		{
			base.agent.SetTarget(this.aimTarget.value);
			if (!this.useTransfom || !(this.aimTarget.value != null))
			{
				if (!this.useTransfom)
				{
					base.agent.SetAimInput((this.aimPos.value - base.agent.transform.position).normalized, AimTypes.normalAim);
				}
				else
				{
					base.agent.SetAimInput(Vector3.zero, AimTypes.normalAim);
				}
			}
			base.EndAction(true);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000839AC File Offset: 0x00081BAC
		protected override void OnUpdate()
		{
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000839AE File Offset: 0x00081BAE
		protected override void OnStop()
		{
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000839B0 File Offset: 0x00081BB0
		protected override void OnPause()
		{
		}

		// Token: 0x040019E0 RID: 6624
		public bool useTransfom = true;

		// Token: 0x040019E1 RID: 6625
		[ShowIf("useTransfom", 1)]
		public BBParameter<Transform> aimTarget;

		// Token: 0x040019E2 RID: 6626
		[ShowIf("useTransfom", 0)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x040019E3 RID: 6627
		private bool waitingSearchResult;
	}
}
