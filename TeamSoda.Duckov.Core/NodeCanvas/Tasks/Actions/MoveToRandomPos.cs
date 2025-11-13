using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000414 RID: 1044
	public class MoveToRandomPos : ActionTask<AICharacterController>
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x00082BA1 File Offset: 0x00080DA1
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00082BA4 File Offset: 0x00080DA4
		protected override void OnExecute()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			this.targetPoint = this.RandomPoint();
			base.agent.MoveToPos(this.targetPoint);
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00082BDC File Offset: 0x00080DDC
		protected override void OnUpdate()
		{
			if (base.agent == null)
			{
				base.EndAction(false);
				return;
			}
			if (base.elapsedTime > this.overTime.value)
			{
				base.EndAction(this.overTimeReturnSuccess);
				return;
			}
			if (this.useTransform && this.centerTransform.value == null)
			{
				base.EndAction(false);
				return;
			}
			if (this.syncDirectionIfNoAimTarget && base.agent.aimTarget == null)
			{
				if (this.setAimToPos && this.aimPos.isDefined)
				{
					base.agent.CharacterMainControl.SetAimPoint(this.aimPos.value);
				}
				else
				{
					Vector3 currentMoveDirection = base.agent.CharacterMainControl.CurrentMoveDirection;
					if (currentMoveDirection.magnitude > 0f)
					{
						base.agent.CharacterMainControl.SetAimPoint(base.agent.CharacterMainControl.transform.position + currentMoveDirection * 1000f);
					}
				}
			}
			if (!base.agent.WaitingForPathResult())
			{
				if (base.agent.ReachedEndOfPath() || !base.agent.IsMoving())
				{
					base.EndAction(true);
					return;
				}
				if (!base.agent.HasPath())
				{
					if (!this.failIfNoPath && this.retryIfNotFound)
					{
						this.targetPoint = this.RandomPoint();
						base.agent.MoveToPos(this.targetPoint);
						return;
					}
					base.EndAction(!this.failIfNoPath);
					return;
				}
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00082D63 File Offset: 0x00080F63
		protected override void OnStop()
		{
			base.agent.StopMove();
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00082D70 File Offset: 0x00080F70
		protected override void OnPause()
		{
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00082D74 File Offset: 0x00080F74
		private Vector3 RandomPoint()
		{
			Vector3 a = base.agent.CharacterMainControl.transform.position;
			if (this.useTransform)
			{
				if (this.centerTransform.isDefined)
				{
					a = this.centerTransform.value.position;
				}
			}
			else
			{
				a = this.centerPos.value;
			}
			Vector3 a2 = a - base.agent.transform.position;
			a2.y = 0f;
			if (a2.magnitude < 0.1f)
			{
				a2 = UnityEngine.Random.insideUnitSphere;
				a2.y = 0f;
			}
			a2 = a2.normalized;
			float y = UnityEngine.Random.Range(-0.5f * this.randomAngle, 0.5f * this.randomAngle);
			float d = UnityEngine.Random.Range(this.avoidRadius.value, this.radius.value);
			a2 = Quaternion.Euler(0f, y, 0f) * -a2;
			return a + a2 * d;
		}

		// Token: 0x040019B5 RID: 6581
		public bool useTransform;

		// Token: 0x040019B6 RID: 6582
		public bool setAimToPos;

		// Token: 0x040019B7 RID: 6583
		[ShowIf("setAimToPos", 1)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x040019B8 RID: 6584
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> centerPos;

		// Token: 0x040019B9 RID: 6585
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> centerTransform;

		// Token: 0x040019BA RID: 6586
		public BBParameter<float> radius;

		// Token: 0x040019BB RID: 6587
		public BBParameter<float> avoidRadius;

		// Token: 0x040019BC RID: 6588
		public float randomAngle = 360f;

		// Token: 0x040019BD RID: 6589
		public BBParameter<float> overTime = 8f;

		// Token: 0x040019BE RID: 6590
		public bool overTimeReturnSuccess = true;

		// Token: 0x040019BF RID: 6591
		private Vector3 targetPoint;

		// Token: 0x040019C0 RID: 6592
		public bool failIfNoPath;

		// Token: 0x040019C1 RID: 6593
		[ShowIf("failIfNoPath", 0)]
		public bool retryIfNotFound;

		// Token: 0x040019C2 RID: 6594
		public bool syncDirectionIfNoAimTarget = true;
	}
}
