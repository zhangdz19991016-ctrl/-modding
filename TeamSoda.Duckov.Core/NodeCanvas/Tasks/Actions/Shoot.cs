using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041F RID: 1055
	public class Shoot : ActionTask<AICharacterController>
	{
		// Token: 0x0600261D RID: 9757 RVA: 0x00083A45 File Offset: 0x00081C45
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x00083A48 File Offset: 0x00081C48
		protected override string info
		{
			get
			{
				return string.Format("Shoot {0}to{1} sec.", this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			}
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x00083A80 File Offset: 0x00081C80
		protected override void OnExecute()
		{
			this.semiTimer = this.semiTimeSpace;
			base.agent.CharacterMainControl.Trigger(true, true, false);
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			this.shootTime = UnityEngine.Random.Range(this.shootTimeRange.value.x, this.shootTimeRange.value.y);
			if (this.shootTime <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x00083B04 File Offset: 0x00081D04
		protected override void OnUpdate()
		{
			bool triggerThisFrame = false;
			this.semiTimer += Time.deltaTime;
			if (!base.agent.shootCanMove)
			{
				base.agent.StopMove();
			}
			if (this.semiTimer >= this.semiTimeSpace)
			{
				this.semiTimer = 0f;
				triggerThisFrame = true;
			}
			base.agent.CharacterMainControl.Trigger(true, triggerThisFrame, false);
			if (base.elapsedTime >= this.shootTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x00083B80 File Offset: 0x00081D80
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x00083B95 File Offset: 0x00081D95
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x040019E6 RID: 6630
		public BBParameter<Vector2> shootTimeRange;

		// Token: 0x040019E7 RID: 6631
		private float shootTime;

		// Token: 0x040019E8 RID: 6632
		public float semiTimeSpace = 0.35f;

		// Token: 0x040019E9 RID: 6633
		private float semiTimer;
	}
}
