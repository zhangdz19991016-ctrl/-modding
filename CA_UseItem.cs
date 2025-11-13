using System;
using Duckov;
using FMOD.Studio;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class CA_UseItem : CharacterActionBase, IProgress
{
	// Token: 0x0600024F RID: 591 RVA: 0x0000A8F0 File Offset: 0x00008AF0
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.usingItem;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000A8F3 File Offset: 0x00008AF3
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000A8F6 File Offset: 0x00008AF6
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000A8F9 File Offset: 0x00008AF9
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000A8FC File Offset: 0x00008AFC
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000A8FF File Offset: 0x00008AFF
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000A904 File Offset: 0x00008B04
	protected override bool OnStart()
	{
		this.agentUsable = null;
		bool flag = false;
		if (this.item.AgentUtilities.ActiveAgent == null)
		{
			if (this.characterController.ChangeHoldItem(this.item) && this.characterController.CurrentHoldItemAgent != null)
			{
				this.agentUsable = (this.characterController.CurrentHoldItemAgent as IAgentUsable);
				flag = true;
			}
		}
		else if (this.item.AgentUtilities.ActiveAgent == this.characterController.CurrentHoldItemAgent)
		{
			flag = true;
		}
		if (flag)
		{
			this.PostActionSound();
		}
		return flag;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000A9A0 File Offset: 0x00008BA0
	protected override void OnStop()
	{
		this.StopSound();
		this.characterController.SwitchToWeaponBeforeUse();
		if (this.item != null && !this.item.IsBeingDestroyed && this.item.GetRoot() != this.characterController.CharacterItem && !this.characterController.PickupItem(this.item))
		{
			this.item.Drop(this.characterController, true);
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000AA1C File Offset: 0x00008C1C
	public void SetUseItem(Item _item)
	{
		this.item = _item;
		this.hasSound = false;
		UsageUtilities component = this.item.GetComponent<UsageUtilities>();
		if (component)
		{
			this.hasSound = component.hasSound;
			this.actionSound = component.actionSound;
			this.useSound = component.useSound;
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000AA70 File Offset: 0x00008C70
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.item == null)
		{
			base.StopAction();
			return;
		}
		if (this.characterController.CurrentHoldItemAgent == null || this.characterController.CurrentHoldItemAgent.Item == null || this.characterController.CurrentHoldItemAgent.Item != this.item)
		{
			Debug.Log("拿的不统一");
			base.StopAction();
			return;
		}
		if (base.ActionTimer > this.characterController.CurrentHoldItemAgent.Item.UseTime)
		{
			this.OnFinish();
			Debug.Log("Use Finished");
			base.StopAction();
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000AB24 File Offset: 0x00008D24
	private void OnFinish()
	{
		this.item.Use(this.characterController);
		this.PostUseSound();
		if (this.item.Stackable)
		{
			this.item.StackCount = this.item.StackCount - 1;
			return;
		}
		if (this.item.UseDurability)
		{
			if (this.item.Durability <= 0f && !this.item.IsBeingDestroyed)
			{
				this.item.DestroyTree();
				return;
			}
		}
		else
		{
			this.item.DestroyTree();
		}
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000ABB4 File Offset: 0x00008DB4
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (this.item != null && base.Running)
		{
			result.inProgress = true;
			result.total = this.item.UseTime;
			result.current = this.actionTimer;
			return result;
		}
		result.inProgress = false;
		return result;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000AC11 File Offset: 0x00008E11
	private void OnDestroy()
	{
		this.StopSound();
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000AC19 File Offset: 0x00008E19
	private void OnDisable()
	{
		this.StopSound();
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000AC21 File Offset: 0x00008E21
	private void PostActionSound()
	{
		if (!this.hasSound)
		{
			return;
		}
		this.soundInstance = AudioManager.Post(this.actionSound, base.gameObject);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000AC43 File Offset: 0x00008E43
	private void PostUseSound()
	{
		if (!this.hasSound)
		{
			return;
		}
		AudioManager.Post(this.useSound, base.gameObject);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000AC60 File Offset: 0x00008E60
	private void StopSound()
	{
		if (this.soundInstance != null)
		{
			this.soundInstance.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x040001D8 RID: 472
	private Item item;

	// Token: 0x040001D9 RID: 473
	public IAgentUsable agentUsable;

	// Token: 0x040001DA RID: 474
	public bool hasSound;

	// Token: 0x040001DB RID: 475
	public string actionSound;

	// Token: 0x040001DC RID: 476
	public string useSound;

	// Token: 0x040001DD RID: 477
	private EventInstance? soundInstance;
}
