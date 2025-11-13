using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000298 RID: 664
	public class Hook : MiniGameBehaviour
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x000513A7 File Offset: 0x0004F5A7
		public Transform Axis
		{
			get
			{
				return this.hookAxis;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x000513AF File Offset: 0x0004F5AF
		public Hook.HookStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x000513B7 File Offset: 0x0004F5B7
		private float RopeDistance
		{
			get
			{
				return Mathf.Lerp(this.minDist, this.maxDist, this.ropeControl);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x000513D0 File Offset: 0x0004F5D0
		private float AxisAngle
		{
			get
			{
				return Mathf.Lerp(-this.maxAngle, this.maxAngle, (this.axisControl + 1f) / 2f);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x000513F8 File Offset: 0x0004F5F8
		private bool RopeOutOfBound
		{
			get
			{
				Vector3 point = Quaternion.Euler(0f, 0f, this.AxisAngle) * Vector2.down * this.RopeDistance;
				return !this.bounds.Contains(point);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x00051444 File Offset: 0x0004F644
		// (set) Token: 0x060015DE RID: 5598 RVA: 0x0005144C File Offset: 0x0004F64C
		public GoldMinerEntity GrabbingTarget
		{
			get
			{
				return this._grabbingTarget;
			}
			private set
			{
				this._grabbingTarget = value;
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060015DF RID: 5599 RVA: 0x00051458 File Offset: 0x0004F658
		// (remove) Token: 0x060015E0 RID: 5600 RVA: 0x00051490 File Offset: 0x0004F690
		public event Action<Hook, GoldMinerEntity> OnResolveTarget;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x060015E1 RID: 5601 RVA: 0x000514C8 File Offset: 0x0004F6C8
		// (remove) Token: 0x060015E2 RID: 5602 RVA: 0x00051500 File Offset: 0x0004F700
		public event Action<Hook> OnLaunch;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060015E3 RID: 5603 RVA: 0x00051538 File Offset: 0x0004F738
		// (remove) Token: 0x060015E4 RID: 5604 RVA: 0x00051570 File Offset: 0x0004F770
		public event Action<Hook> OnBeginRetrieve;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x060015E5 RID: 5605 RVA: 0x000515A8 File Offset: 0x0004F7A8
		// (remove) Token: 0x060015E6 RID: 5606 RVA: 0x000515E0 File Offset: 0x0004F7E0
		public event Action<Hook, GoldMinerEntity> OnAttach;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060015E7 RID: 5607 RVA: 0x00051618 File Offset: 0x0004F818
		// (remove) Token: 0x060015E8 RID: 5608 RVA: 0x00051650 File Offset: 0x0004F850
		public event Action<Hook> OnEndRetrieve;

		// Token: 0x060015E9 RID: 5609 RVA: 0x00051685 File Offset: 0x0004F885
		public void SetParameters(float swingFreqFactor, float emptySpeed, float strength)
		{
			this.swingFreqFactor = swingFreqFactor;
			this.emptySpeed = emptySpeed;
			this.strength = strength;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0005169C File Offset: 0x0004F89C
		public void Tick(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
			this.ropeLineRenderer.SetPositions(new Vector3[]
			{
				this.hookAxis.transform.position,
				this.hookHead.transform.position
			});
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x000516FB File Offset: 0x0004F8FB
		private void UpdateHookHeadPosition()
		{
			this.hookHead.transform.localPosition = this.GetHookHeadPosition(this.RopeDistance);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00051719 File Offset: 0x0004F919
		private Vector3 GetHookHeadPosition(float ropeDistance)
		{
			return -Vector3.up * this.RopeDistance;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00051730 File Offset: 0x0004F930
		private void UpdateAxis()
		{
			this.hookAxis.transform.localRotation = Quaternion.Euler(0f, 0f, this.AxisAngle);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00051757 File Offset: 0x0004F957
		private void OnValidate()
		{
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00051768 File Offset: 0x0004F968
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case Hook.HookStatus.Idle:
				break;
			case Hook.HookStatus.Swinging:
				this.UpdateSwinging(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Launching:
				this.UpdateClaw();
				this.UpdateLaunching(deltaTime);
				return;
			case Hook.HookStatus.Attaching:
				this.UpdateAttaching(deltaTime);
				return;
			case Hook.HookStatus.Retrieving:
				this.UpdateRetreving(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Retrieved:
				this.UpdateRetrieved();
				break;
			default:
				return;
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x000517D3 File Offset: 0x0004F9D3
		public void Launch()
		{
			if (this.status != Hook.HookStatus.Swinging)
			{
				return;
			}
			this.EnterStatus(Hook.HookStatus.Launching);
			Action<Hook> onLaunch = this.OnLaunch;
			if (onLaunch == null)
			{
				return;
			}
			onLaunch(this);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000517F7 File Offset: 0x0004F9F7
		public void Reset()
		{
			this.ropeControl = 0f;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00051804 File Offset: 0x0004FA04
		private void UpdateClaw()
		{
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			if (!this.GrabbingTarget)
			{
				this.claw.localRotation = Quaternion.Euler(0f, 0f, -180f);
				this.claw.localPosition = Vector3.zero;
				return;
			}
			Vector2 to = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			this.claw.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, to));
			this.claw.position = this.hookHead.transform.position + to.normalized * this.clawOffset;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x000518F0 File Offset: 0x0004FAF0
		private void UpdateSwinging(float deltaTime)
		{
			this.t += deltaTime * 90f * this.swingFreqFactor * 0.017453292f;
			this.axisControl = Mathf.Sin(this.t);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00051924 File Offset: 0x0004FB24
		private void UpdateLaunching(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float maxDelta = num * deltaTime / (this.maxDist - this.minDist);
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 1f, maxDelta);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.RopeOutOfBound || this.ropeControl >= 1f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
			this.CheckGrab(oldWorldPos, newWorldPos);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00051A00 File Offset: 0x0004FC00
		private void CheckGrab(Vector3 oldWorldPos, Vector3 newWorldPos)
		{
			if (this.GrabbingTarget)
			{
				return;
			}
			Vector3 vector = newWorldPos - oldWorldPos;
			foreach (RaycastHit2D raycastHit2D in Physics2D.CircleCastAll(oldWorldPos, 8f, vector.normalized, vector.magnitude))
			{
				if (!(raycastHit2D.collider == null))
				{
					GoldMinerEntity component = raycastHit2D.collider.gameObject.GetComponent<GoldMinerEntity>();
					if (!(component == null))
					{
						this.Grab(component);
						return;
					}
				}
			}
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x00051A94 File Offset: 0x0004FC94
		private void Grab(GoldMinerEntity target)
		{
			this.GrabbingTarget = target;
			this.EnterStatus(Hook.HookStatus.Attaching);
			this.relativePos = target.transform.position - this.hookHead.transform.position;
			this.targetDist = this.relativePos.magnitude;
			this.targetRelativeRotation = Quaternion.FromToRotation(this.relativePos, this.GrabbingTarget.transform.up);
			this.retrieveETA = this.grabAnimationTime;
			Vector2 to = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			Vector3 endValue = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, to));
			Vector3 endValue2 = this.hookHead.transform.position + to.normalized * this.clawOffset;
			this.claw.DORotate(endValue, this.retrieveETA, RotateMode.Fast).SetEase(this.grabAnimationEase);
			this.claw.DOMove(endValue2, this.retrieveETA, false).SetEase(this.grabAnimationEase);
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			this.GrabbingTarget.NotifyAttached(this);
			Action<Hook, GoldMinerEntity> onAttach = this.OnAttach;
			if (onAttach == null)
			{
				return;
			}
			onAttach(this, target);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00051C08 File Offset: 0x0004FE08
		private void UpdateAttaching(float deltaTime)
		{
			if (this.GrabbingTarget == null)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
				return;
			}
			this.retrieveETA -= deltaTime;
			if (this.retrieveETA <= 0f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00051C44 File Offset: 0x0004FE44
		private void UpdateRetreving(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float maxDelta = num * deltaTime / (this.maxDist - this.minDist);
			this.maxDeltaWatch = maxDelta;
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 0f, maxDelta);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.ropeControl <= 0f)
			{
				this.ropeControl = 0f;
				this.EnterStatus(Hook.HookStatus.Retrieved);
			}
			if (this.GrabbingTarget)
			{
				Vector3 point = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
				if (point.magnitude > this.targetDist)
				{
					this.GrabbingTarget.transform.position = this.hookHead.transform.position + point.normalized * this.targetDist;
					Vector3 toDirection = this.targetRelativeRotation * point;
					this.GrabbingTarget.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
					return;
				}
			}
			else
			{
				this.CheckGrab(oldWorldPos, newWorldPos);
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00051DD3 File Offset: 0x0004FFD3
		private void UpdateRetrieved()
		{
			if (this.GrabbingTarget)
			{
				this.ResolveRetrievedObject(this.GrabbingTarget);
				this.GrabbingTarget = null;
			}
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00051DFC File Offset: 0x0004FFFC
		private void ResolveRetrievedObject(GoldMinerEntity grabingTarget)
		{
			Action<Hook, GoldMinerEntity> onResolveTarget = this.OnResolveTarget;
			if (onResolveTarget == null)
			{
				return;
			}
			onResolveTarget(this, grabingTarget);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00051E10 File Offset: 0x00050010
		private void OnExitStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Swinging:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Retrieving:
			{
				Action<Hook> onEndRetrieve = this.OnEndRetrieve;
				if (onEndRetrieve == null)
				{
					return;
				}
				onEndRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00051E42 File Offset: 0x00050042
		private void EnterStatus(Hook.HookStatus status)
		{
			this.OnExitStatus(this.status);
			this.status = status;
			this.OnEnterStatus(this.status);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00051E64 File Offset: 0x00050064
		private void OnEnterStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Swinging:
				this.ropeControl = 0f;
				return;
			case Hook.HookStatus.Retrieving:
			{
				if (this.GrabbingTarget)
				{
					this.GrabbingTarget.NotifyBeginRetrieving();
				}
				Action<Hook> onBeginRetrieve = this.OnBeginRetrieve;
				if (onBeginRetrieve == null)
				{
					return;
				}
				onBeginRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00051EC5 File Offset: 0x000500C5
		internal Vector3 Direction
		{
			get
			{
				return -this.hookAxis.transform.up;
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00051EDC File Offset: 0x000500DC
		internal void ReleaseClaw()
		{
			this.GrabbingTarget = null;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00051EE5 File Offset: 0x000500E5
		internal void BeginSwing()
		{
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x0400101A RID: 4122
		public float emptySpeed = 1000f;

		// Token: 0x0400101B RID: 4123
		public float strength;

		// Token: 0x0400101C RID: 4124
		public float swingFreqFactor = 1f;

		// Token: 0x0400101D RID: 4125
		[SerializeField]
		private Transform hookAxis;

		// Token: 0x0400101E RID: 4126
		[SerializeField]
		private HookHead hookHead;

		// Token: 0x0400101F RID: 4127
		[SerializeField]
		private Transform claw;

		// Token: 0x04001020 RID: 4128
		[SerializeField]
		private float clawOffset = 4f;

		// Token: 0x04001021 RID: 4129
		[SerializeField]
		private Animator clawAnimator;

		// Token: 0x04001022 RID: 4130
		[SerializeField]
		private LineRenderer ropeLineRenderer;

		// Token: 0x04001023 RID: 4131
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04001024 RID: 4132
		[SerializeField]
		private float grabAnimationTime = 0.5f;

		// Token: 0x04001025 RID: 4133
		[SerializeField]
		private Ease grabAnimationEase = Ease.OutBounce;

		// Token: 0x04001026 RID: 4134
		[SerializeField]
		private float maxAngle;

		// Token: 0x04001027 RID: 4135
		[SerializeField]
		private float minDist;

		// Token: 0x04001028 RID: 4136
		[SerializeField]
		private float maxDist;

		// Token: 0x04001029 RID: 4137
		[Range(0f, 1f)]
		private float ropeControl;

		// Token: 0x0400102A RID: 4138
		[Range(-1f, 1f)]
		private float axisControl;

		// Token: 0x0400102B RID: 4139
		private Hook.HookStatus status;

		// Token: 0x0400102C RID: 4140
		private float t;

		// Token: 0x0400102D RID: 4141
		private GoldMinerEntity _grabbingTarget;

		// Token: 0x0400102E RID: 4142
		private Vector2 relativePos;

		// Token: 0x0400102F RID: 4143
		private Quaternion targetRelativeRotation;

		// Token: 0x04001030 RID: 4144
		private float targetDist;

		// Token: 0x04001031 RID: 4145
		private float retrieveETA;

		// Token: 0x04001037 RID: 4151
		public float forceModification;

		// Token: 0x04001038 RID: 4152
		private float maxDeltaWatch;

		// Token: 0x02000578 RID: 1400
		public enum HookStatus
		{
			// Token: 0x04001FB1 RID: 8113
			Idle,
			// Token: 0x04001FB2 RID: 8114
			Swinging,
			// Token: 0x04001FB3 RID: 8115
			Launching,
			// Token: 0x04001FB4 RID: 8116
			Attaching,
			// Token: 0x04001FB5 RID: 8117
			Retrieving,
			// Token: 0x04001FB6 RID: 8118
			Retrieved
		}
	}
}
