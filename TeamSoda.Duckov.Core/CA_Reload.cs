using System;
using Duckov;
using ItemStatsSystem;

// Token: 0x02000053 RID: 83
public class CA_Reload : CharacterActionBase, IProgress
{
	// Token: 0x06000230 RID: 560 RVA: 0x0000A494 File Offset: 0x00008694
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Reload;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000A497 File Offset: 0x00008697
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000A49A File Offset: 0x0000869A
	public override bool CanRun()
	{
		return true;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000A49D File Offset: 0x0000869D
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000A4A0 File Offset: 0x000086A0
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000A4A3 File Offset: 0x000086A3
	public override bool IsReady()
	{
		this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
		return this.currentGun && !this.currentGun.IsReloading();
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000A4DC File Offset: 0x000086DC
	protected override bool OnStart()
	{
		this.currentGun = null;
		if (!this.characterController || !this.characterController.CurrentHoldItemAgent)
		{
			return false;
		}
		this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
		this.currentGun.GunItemSetting.PreferdBulletsToLoad = this.preferedBulletToReload;
		this.preferedBulletToReload = null;
		return this.currentGun != null && this.currentGun.BeginReload();
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000A562 File Offset: 0x00008762
	protected override void OnStop()
	{
		if (this.currentGun != null)
		{
			this.currentGun.CancleReload();
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000A580 File Offset: 0x00008780
	public bool GetGunReloadable()
	{
		if (this.currentGun == null)
		{
			this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
			return false;
		}
		return !base.Running && !this.currentGun.IsFull();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000A5CF File Offset: 0x000087CF
	public override bool CanEditInventory()
	{
		return true;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000A5D2 File Offset: 0x000087D2
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.currentGun == null)
		{
			base.StopAction();
			return;
		}
		if (!this.currentGun.IsReloading())
		{
			base.StopAction();
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000A600 File Offset: 0x00008800
	public Progress GetProgress()
	{
		if (this.currentGun != null)
		{
			return this.currentGun.GetReloadProgress();
		}
		return new Progress
		{
			inProgress = false
		};
	}

	// Token: 0x040001CF RID: 463
	public ItemAgent_Gun currentGun;

	// Token: 0x040001D0 RID: 464
	public Item preferedBulletToReload;
}
