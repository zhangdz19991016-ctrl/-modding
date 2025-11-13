using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000412 RID: 1042
	public class Dash : ActionTask<AICharacterController>
	{
		// Token: 0x060025CB RID: 9675 RVA: 0x00082961 File Offset: 0x00080B61
		protected override string OnInit()
		{
			this.dashTimeSpace = UnityEngine.Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			return null;
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x0008298F File Offset: 0x00080B8F
		protected override string info
		{
			get
			{
				return string.Format("Dash", Array.Empty<object>());
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000829A0 File Offset: 0x00080BA0
		protected override void OnExecute()
		{
			if (Time.time - this.lastDashTime < this.dashTimeSpace)
			{
				base.EndAction();
				return;
			}
			this.lastDashTime = Time.time;
			this.dashTimeSpace = UnityEngine.Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			Vector3 vector = Vector3.forward;
			Dash.DashDirectionModes dashDirectionModes = this.directionMode;
			if (dashDirectionModes != Dash.DashDirectionModes.random)
			{
				if (dashDirectionModes == Dash.DashDirectionModes.targetTransform)
				{
					if (this.targetTransform.value == null)
					{
						base.EndAction();
						return;
					}
					vector = this.targetTransform.value.position - base.agent.transform.position;
					vector.y = 0f;
					vector.Normalize();
					if (this.verticle)
					{
						vector = Vector3.Cross(vector, Vector3.up) * ((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1f : -1f);
					}
				}
			}
			else
			{
				vector = UnityEngine.Random.insideUnitCircle;
				vector.z = vector.y;
				vector.y = 0f;
				vector.Normalize();
			}
			base.agent.CharacterMainControl.SetMoveInput(vector);
			base.agent.CharacterMainControl.Dash();
			base.EndAction(true);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00082AFD File Offset: 0x00080CFD
		protected override void OnStop()
		{
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00082AFF File Offset: 0x00080CFF
		protected override void OnPause()
		{
		}

		// Token: 0x040019AE RID: 6574
		public Dash.DashDirectionModes directionMode;

		// Token: 0x040019AF RID: 6575
		[ShowIf("directionMode", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x040019B0 RID: 6576
		[ShowIf("directionMode", 1)]
		public bool verticle;

		// Token: 0x040019B1 RID: 6577
		public BBParameter<Vector2> dashTimeSpaceRange;

		// Token: 0x040019B2 RID: 6578
		private float dashTimeSpace;

		// Token: 0x040019B3 RID: 6579
		private float lastDashTime = -999f;

		// Token: 0x0200067E RID: 1662
		public enum DashDirectionModes
		{
			// Token: 0x04002384 RID: 9092
			random,
			// Token: 0x04002385 RID: 9093
			targetTransform
		}
	}
}
