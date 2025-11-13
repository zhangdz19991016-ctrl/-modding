using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public abstract class CharacterActionBase : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000261 RID: 609 RVA: 0x0000AC97 File Offset: 0x00008E97
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000262 RID: 610 RVA: 0x0000AC9F File Offset: 0x00008E9F
	public float ActionTimer
	{
		get
		{
			return this.actionTimer;
		}
	}

	// Token: 0x06000263 RID: 611
	public abstract CharacterActionBase.ActionPriorities ActionPriority();

	// Token: 0x06000264 RID: 612
	public abstract bool CanMove();

	// Token: 0x06000265 RID: 613
	public abstract bool CanRun();

	// Token: 0x06000266 RID: 614
	public abstract bool CanUseHand();

	// Token: 0x06000267 RID: 615
	public abstract bool CanControlAim();

	// Token: 0x06000268 RID: 616 RVA: 0x0000ACA7 File Offset: 0x00008EA7
	public virtual bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000ACAA File Offset: 0x00008EAA
	public void UpdateAction(float deltaTime)
	{
		this.actionTimer += deltaTime;
		this.OnUpdateAction(deltaTime);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000ACC1 File Offset: 0x00008EC1
	protected virtual void OnUpdateAction(float deltaTime)
	{
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000ACC3 File Offset: 0x00008EC3
	protected virtual bool OnStart()
	{
		return true;
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000ACC6 File Offset: 0x00008EC6
	public virtual bool IsStopable()
	{
		return true;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000ACC9 File Offset: 0x00008EC9
	protected virtual void OnStop()
	{
	}

	// Token: 0x0600026E RID: 622
	public abstract bool IsReady();

	// Token: 0x0600026F RID: 623 RVA: 0x0000ACCB File Offset: 0x00008ECB
	public bool StartActionByCharacter(CharacterMainControl _character)
	{
		if (!this.IsReady())
		{
			return false;
		}
		this.characterController = _character;
		if (this.OnStart())
		{
			this.actionTimer = 0f;
			this.running = true;
			return true;
		}
		return false;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000ACFB File Offset: 0x00008EFB
	public bool StopAction()
	{
		if (!this.running)
		{
			return true;
		}
		if (this.IsStopable())
		{
			this.running = false;
			this.OnStop();
			return true;
		}
		return false;
	}

	// Token: 0x040001DE RID: 478
	private bool running;

	// Token: 0x040001DF RID: 479
	protected float actionTimer;

	// Token: 0x040001E0 RID: 480
	public bool progressHUD = true;

	// Token: 0x040001E1 RID: 481
	public CharacterMainControl characterController;

	// Token: 0x02000436 RID: 1078
	public enum ActionPriorities
	{
		// Token: 0x04001A52 RID: 6738
		Whatever,
		// Token: 0x04001A53 RID: 6739
		Reload,
		// Token: 0x04001A54 RID: 6740
		Attack,
		// Token: 0x04001A55 RID: 6741
		usingItem,
		// Token: 0x04001A56 RID: 6742
		Dash,
		// Token: 0x04001A57 RID: 6743
		Skills,
		// Token: 0x04001A58 RID: 6744
		Fishing,
		// Token: 0x04001A59 RID: 6745
		Interact
	}
}
