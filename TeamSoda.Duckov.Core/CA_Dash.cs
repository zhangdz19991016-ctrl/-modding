using System;
using Duckov;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class CA_Dash : CharacterActionBase, IProgress
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000211 RID: 529 RVA: 0x00009EAD File Offset: 0x000080AD
	private string sfx
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this.overrideSFX))
			{
				return "Char/Footstep/dash";
			}
			return this.overrideSFX;
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00009EC8 File Offset: 0x000080C8
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Dash;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00009ECB File Offset: 0x000080CB
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00009ECE File Offset: 0x000080CE
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00009ED1 File Offset: 0x000080D1
	public override bool CanUseHand()
	{
		return this.dashCanControl;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00009ED9 File Offset: 0x000080D9
	public override bool CanControlAim()
	{
		return this.dashCanControl;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00009EE4 File Offset: 0x000080E4
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (base.Running)
		{
			result.inProgress = true;
			result.total = this.dashTime;
			result.current = this.actionTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00009F2E File Offset: 0x0000812E
	public override bool IsReady()
	{
		return Time.time - this.lastEndTime >= this.coolTime && !base.Running;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x00009F50 File Offset: 0x00008150
	protected override bool OnStart()
	{
		if (this.characterController.CurrentStamina < this.staminaCost)
		{
			return false;
		}
		this.characterController.UseStamina(this.staminaCost);
		this.dashSpeed = this.characterController.DashSpeed;
		this.dashCanControl = this.characterController.DashCanControl;
		if (this.characterController.MoveInput.magnitude > 0f)
		{
			this.dashDirection = this.characterController.MoveInput.normalized;
		}
		else
		{
			this.dashDirection = this.characterController.CurrentAimDirection;
		}
		this.characterController.SetForceMoveVelocity(this.dashSpeed * this.speedCurve.Evaluate(0f) * this.dashDirection);
		if (!this.dashCanControl)
		{
			this.characterController.movementControl.ForceTurnTo(this.dashDirection);
		}
		HardwareSyncingManager.SetEvent("Dodge");
		AudioManager.Post(this.sfx, base.gameObject);
		return true;
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000A053 File Offset: 0x00008253
	protected override void OnStop()
	{
		this.characterController.SetForceMoveVelocity(this.characterController.CharacterRunSpeed * this.dashDirection);
		this.lastEndTime = Time.time;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000A084 File Offset: 0x00008284
	protected override void OnUpdateAction(float deltaTime)
	{
		if ((this.actionTimer > this.dashTime || !base.Running) && base.StopAction())
		{
			return;
		}
		this.characterController.SetForceMoveVelocity(this.dashSpeed * this.speedCurve.Evaluate(Mathf.Clamp01(this.actionTimer / this.dashTime)) * this.dashDirection);
	}

	// Token: 0x040001BF RID: 447
	private float dashSpeed;

	// Token: 0x040001C0 RID: 448
	private bool dashCanControl;

	// Token: 0x040001C1 RID: 449
	public AnimationCurve speedCurve;

	// Token: 0x040001C2 RID: 450
	public float dashTime;

	// Token: 0x040001C3 RID: 451
	public float coolTime = 0.5f;

	// Token: 0x040001C4 RID: 452
	private Vector3 dashDirection;

	// Token: 0x040001C5 RID: 453
	public float staminaCost = 10f;

	// Token: 0x040001C6 RID: 454
	private float lastEndTime = -999f;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private string overrideSFX;
}
