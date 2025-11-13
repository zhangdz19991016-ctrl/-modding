using System;
using Duckov;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class CA_Carry : CharacterActionBase, IProgress
{
	// Token: 0x06000205 RID: 517 RVA: 0x00009D97 File Offset: 0x00007F97
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Whatever;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00009D9A File Offset: 0x00007F9A
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00009D9D File Offset: 0x00007F9D
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00009DA0 File Offset: 0x00007FA0
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00009DA3 File Offset: 0x00007FA3
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00009DA6 File Offset: 0x00007FA6
	public override bool IsReady()
	{
		return this.carryTarget != null;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00009DB4 File Offset: 0x00007FB4
	public float GetWeight()
	{
		if (!base.Running)
		{
			return 0f;
		}
		if (!this.carringTarget)
		{
			return 0f;
		}
		return this.carringTarget.GetWeight();
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00009DE4 File Offset: 0x00007FE4
	public Progress GetProgress()
	{
		return new Progress
		{
			inProgress = false,
			total = 1f,
			current = 1f
		};
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00009E1A File Offset: 0x0000801A
	protected override bool OnStart()
	{
		this.characterController.ChangeHoldItem(null);
		this.carryTarget.Take(this);
		this.carringTarget = this.carryTarget;
		return true;
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00009E42 File Offset: 0x00008042
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.characterController.CurrentHoldItemAgent != null)
		{
			base.StopAction();
		}
		if (this.carryTarget)
		{
			this.carryTarget.OnCarriableUpdate(deltaTime);
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00009E77 File Offset: 0x00008077
	protected override void OnStop()
	{
		this.carryTarget.Drop();
		this.carringTarget = null;
	}

	// Token: 0x040001BC RID: 444
	[HideInInspector]
	public Carriable carryTarget;

	// Token: 0x040001BD RID: 445
	private Carriable carringTarget;

	// Token: 0x040001BE RID: 446
	public Vector3 carryPoint = new Vector3(0f, 1f, 0.8f);
}
