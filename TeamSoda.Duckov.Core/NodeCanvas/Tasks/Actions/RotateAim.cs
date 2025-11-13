using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041A RID: 1050
	public class RotateAim : ActionTask<AICharacterController>
	{
		// Token: 0x060025FC RID: 9724 RVA: 0x000834A2 File Offset: 0x000816A2
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000834A8 File Offset: 0x000816A8
		protected override void OnExecute()
		{
			this.time = UnityEngine.Random.Range(this.timeRange.value.x, this.timeRange.value.y);
			this.startDir = base.agent.CharacterMainControl.CurrentAimDirection;
			base.agent.SetTarget(null);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x00083520 File Offset: 0x00081720
		protected override void OnUpdate()
		{
			this.currentAngle = this.angle * base.elapsedTime / this.time;
			Vector3 a = Quaternion.Euler(0f, this.currentAngle, 0f) * this.startDir;
			base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + a * 100f);
			if (this.shoot)
			{
				base.agent.CharacterMainControl.Trigger(true, true, false);
			}
			if (base.elapsedTime > this.time)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000835CD File Offset: 0x000817CD
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000835E2 File Offset: 0x000817E2
		protected override void OnPause()
		{
			base.agent.CharacterMainControl.Trigger(false, false, false);
		}

		// Token: 0x040019CC RID: 6604
		private Vector3 startDir;

		// Token: 0x040019CD RID: 6605
		public float angle;

		// Token: 0x040019CE RID: 6606
		private float currentAngle;

		// Token: 0x040019CF RID: 6607
		public BBParameter<Vector2> timeRange;

		// Token: 0x040019D0 RID: 6608
		private float time;

		// Token: 0x040019D1 RID: 6609
		public bool shoot;
	}
}
