using System;
using ItemStatsSystem;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000424 RID: 1060
	public class UseDrug : ActionTask<AICharacterController>
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x000840A5 File Offset: 0x000822A5
		protected override string info
		{
			get
			{
				if (!this.stopMove)
				{
					return "打药";
				}
				return "原地打药";
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000840BA File Offset: 0x000822BA
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000840C0 File Offset: 0x000822C0
		protected override void OnExecute()
		{
			Item drugItem = base.agent.GetDrugItem();
			if (drugItem == null)
			{
				base.EndAction(false);
				return;
			}
			base.agent.CharacterMainControl.UseItem(drugItem);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000840FC File Offset: 0x000822FC
		protected override void OnUpdate()
		{
			if (this.stopMove && base.agent.IsMoving())
			{
				base.agent.StopMove();
			}
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(false);
				return;
			}
			if (!base.agent.CharacterMainControl.useItemAction.Running)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0008416E File Offset: 0x0008236E
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x040019FA RID: 6650
		public bool stopMove;
	}
}
