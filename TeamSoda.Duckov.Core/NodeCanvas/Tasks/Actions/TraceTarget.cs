using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000422 RID: 1058
	public class TraceTarget : ActionTask<AICharacterController>
	{
		// Token: 0x0600262E RID: 9774 RVA: 0x00083CB6 File Offset: 0x00081EB6
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00083CBC File Offset: 0x00081EBC
		protected override void OnExecute()
		{
			if (base.agent == null || (this.traceTargetTransform && this.centerTransform.value == null))
			{
				base.EndAction(false);
				return;
			}
			Vector3 pos = this.traceTargetTransform ? this.centerTransform.value.position : this.centerPosition.value;
			base.agent.MoveToPos(pos);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00083D2C File Offset: 0x00081F2C
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			Vector3 vector = (this.traceTargetTransform && this.centerTransform.value != null) ? this.centerTransform.value.position : this.centerPosition.value;
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (Vector3.Distance(vector, base.agent.transform.position) < this.stopDistance.value)
			{
				base.EndAction(true);
				return;
			}
			this.recalculatePathTimer -= Time.deltaTime;
			if (this.recalculatePathTimer <= 0f)
			{
				this.recalculatePathTimer = this.recalculatePathTimeSpace;
				base.agent.MoveToPos(vector);
			}
			else if (!base.agent.WaitingForPathResult())
			{
				if (!base.agent.IsMoving() || base.agent.ReachedEndOfPath())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						base.agent.MoveToPos(vector);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
				if (currentMoveDirection.magnitude > 0f)
				{
					base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
				}
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00083EE4 File Offset: 0x000820E4
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x00083EF1 File Offset: 0x000820F1
		protected override void OnPause()
		{
		}

		// Token: 0x040019EB RID: 6635
		public bool traceTargetTransform = true;

		// Token: 0x040019EC RID: 6636
		[ShowIf("traceTargetTransform", 0)]
		public BBParameter<Vector3> centerPosition;

		// Token: 0x040019ED RID: 6637
		[ShowIf("traceTargetTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x040019EE RID: 6638
		public BBParameter<float> stopDistance;

		// Token: 0x040019EF RID: 6639
		public BBParameter<float> overTime = 8f;

		// Token: 0x040019F0 RID: 6640
		public bool overTimeReturnSuccess = true;

		// Token: 0x040019F1 RID: 6641
		private Vector3 targetPoint;

		// Token: 0x040019F2 RID: 6642
		public bool failIfNoPath;

		// Token: 0x040019F3 RID: 6643
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x040019F4 RID: 6644
		private float recalculatePathTimeSpace = 0.15f;

		// Token: 0x040019F5 RID: 6645
		private float recalculatePathTimer = 0.15f;

		// Token: 0x040019F6 RID: 6646
		public bool syncDirectionIfNoAimTarget = true;
	}
}
