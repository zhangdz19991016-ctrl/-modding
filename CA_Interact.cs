using System;
using Duckov;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CA_Interact : CharacterActionBase, IProgress
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600021D RID: 541 RVA: 0x0000A113 File Offset: 0x00008313
	public InteractableBase MasterInteractableAround
	{
		get
		{
			return this.masterInteractableAround;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600021E RID: 542 RVA: 0x0000A11B File Offset: 0x0000831B
	public InteractableBase InteractTarget
	{
		get
		{
			if (this.masterInteractableAround)
			{
				return this.masterInteractableAround.GetInteractableInGroup(this.interactIndexInGroup);
			}
			return null;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600021F RID: 543 RVA: 0x0000A13D File Offset: 0x0000833D
	public int InteractIndexInGroup
	{
		get
		{
			return this.interactIndexInGroup;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A145 File Offset: 0x00008345
	public InteractableBase InteractingTarget
	{
		get
		{
			return this.interactingTarget;
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000A14D File Offset: 0x0000834D
	private void Awake()
	{
		this.interactLayers = 1 << LayerMask.NameToLayer("Interactable");
		this.colliders = new Collider[5];
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000A178 File Offset: 0x00008378
	public void SearchInteractableAround()
	{
		if (!this.characterController.IsMainCharacter)
		{
			return;
		}
		InteractableBase x = this.masterInteractableAround;
		int num = Physics.OverlapSphereNonAlloc(base.transform.position + Vector3.up * 0.5f + this.characterController.CurrentAimDirection * 0.2f, 0.3f, this.colliders, this.interactLayers);
		if (num <= 0)
		{
			this.masterInteractableAround = null;
			return;
		}
		float num2 = 999f;
		this.minDistanceInteractable = null;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.colliders[i];
			float num3 = Vector3.Distance(base.transform.position, collider.transform.position);
			if (num3 < num2)
			{
				InteractableBase interactableBase = null;
				if (this.masterInteractableAround == null || this.masterInteractableAround.gameObject != collider.gameObject)
				{
					interactableBase = collider.GetComponent<InteractableBase>();
				}
				else if (this.masterInteractableAround != null && this.masterInteractableAround.gameObject == collider.gameObject)
				{
					interactableBase = this.masterInteractableAround;
				}
				if (!(interactableBase == null) && interactableBase.CheckInteractable())
				{
					this.minDistanceInteractable = interactableBase;
					num2 = num3;
				}
			}
		}
		this.masterInteractableAround = this.minDistanceInteractable;
		if (x != this.masterInteractableAround || x == null)
		{
			this.interactIndexInGroup = 0;
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000A300 File Offset: 0x00008500
	public void SwitchInteractable(int dir)
	{
		if (this.MasterInteractableAround == null || !this.MasterInteractableAround.interactableGroup)
		{
			this.interactIndexInGroup = 0;
			return;
		}
		this.interactIndexInGroup += dir;
		int num = 1;
		if (this.MasterInteractableAround.interactableGroup)
		{
			num = this.MasterInteractableAround.GetInteractableList().Count;
		}
		if (this.interactIndexInGroup >= num)
		{
			this.interactIndexInGroup = 0;
			return;
		}
		if (this.interactIndexInGroup < 0)
		{
			this.interactIndexInGroup = num - 1;
		}
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000A381 File Offset: 0x00008581
	public void SetInteractableTarget(InteractableBase interactable)
	{
		this.masterInteractableAround = interactable;
		this.interactIndexInGroup = 0;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000A394 File Offset: 0x00008594
	protected override bool OnStart()
	{
		InteractableBase interactTarget = this.InteractTarget;
		if (!interactTarget)
		{
			return false;
		}
		if (interactTarget.StartInteract(this.characterController))
		{
			this.interactingTarget = interactTarget;
			return true;
		}
		return false;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000A3CA File Offset: 0x000085CA
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.interactingTarget == null)
		{
			base.StopAction();
			return;
		}
		if (!this.interactingTarget.Interacting)
		{
			base.StopAction();
			return;
		}
		this.interactingTarget.UpdateInteract(this.characterController, deltaTime);
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000A409 File Offset: 0x00008609
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Interact;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000A40C File Offset: 0x0000860C
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000A40F File Offset: 0x0000860F
	protected override void OnStop()
	{
		if (this.interactingTarget && this.interactingTarget.Interacting)
		{
			this.interactingTarget.InternalStopInteract();
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000A436 File Offset: 0x00008636
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000A439 File Offset: 0x00008639
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000A43C File Offset: 0x0000863C
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000A43F File Offset: 0x0000863F
	public override bool IsReady()
	{
		return !base.Running && this.InteractTarget != null;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000A457 File Offset: 0x00008657
	public Progress GetProgress()
	{
		if (this.interactingTarget != null)
		{
			this.progress = this.interactingTarget.GetProgress();
		}
		else
		{
			this.progress.inProgress = false;
		}
		return this.progress;
	}

	// Token: 0x040001C8 RID: 456
	private InteractableBase masterInteractableAround;

	// Token: 0x040001C9 RID: 457
	private int interactIndexInGroup;

	// Token: 0x040001CA RID: 458
	private InteractableBase interactingTarget;

	// Token: 0x040001CB RID: 459
	private LayerMask interactLayers;

	// Token: 0x040001CC RID: 460
	private InteractableBase minDistanceInteractable;

	// Token: 0x040001CD RID: 461
	private Collider[] colliders;

	// Token: 0x040001CE RID: 462
	private Progress progress;
}
