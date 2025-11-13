using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class ItemSetting_Accessory : ItemSettingBase
{
	// Token: 0x060007EB RID: 2027 RVA: 0x00023CCB File Offset: 0x00021ECB
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsBullet", true, true);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00023CDA File Offset: 0x00021EDA
	public override void OnInit()
	{
		base.Item.onPluggedIntoSlot += this.OnPluggedIntoSlot;
		base.Item.onUnpluggedFromSlot += this.OnUnpluggedIntoSlot;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00023D0C File Offset: 0x00021F0C
	private void OnPluggedIntoSlot(Item selfItem)
	{
		Slot pluggedIntoSlot = selfItem.PluggedIntoSlot;
		if (pluggedIntoSlot == null)
		{
			return;
		}
		this.masterItem = pluggedIntoSlot.Master;
		if (!this.masterItem)
		{
			return;
		}
		this.masterItem.AgentUtilities.onCreateAgent += this.OnMasterCreateAgent;
		this.CreateAccessory(this.masterItem.AgentUtilities.ActiveAgent as DuckovItemAgent);
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00023D75 File Offset: 0x00021F75
	private void OnUnpluggedIntoSlot(Item selfItem)
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00023DA6 File Offset: 0x00021FA6
	private void OnDestroy()
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00023DD7 File Offset: 0x00021FD7
	private void OnMasterCreateAgent(Item _masterItem, ItemAgent newAgnet)
	{
		if (this.masterItem != _masterItem)
		{
			Debug.LogError("缓存了错误的Item");
		}
		if (newAgnet.AgentType != ItemAgent.AgentTypes.handheld)
		{
			return;
		}
		this.CreateAccessory(newAgnet as DuckovItemAgent);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00023E08 File Offset: 0x00022008
	private void CreateAccessory(DuckovItemAgent parentAgent)
	{
		this.DestroyAccessory();
		if (this.accessoryPfb == null || parentAgent == null || parentAgent.AgentType != ItemAgent.AgentTypes.handheld)
		{
			return;
		}
		this.accessoryInstance = UnityEngine.Object.Instantiate<AccessoryBase>(this.accessoryPfb);
		this.accessoryInstance.Init(parentAgent, base.Item);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00023E5F File Offset: 0x0002205F
	private void DestroyAccessory()
	{
		if (this.accessoryInstance)
		{
			UnityEngine.Object.Destroy(this.accessoryInstance.gameObject);
		}
	}

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private AccessoryBase accessoryPfb;

	// Token: 0x0400076B RID: 1899
	public ADSAimMarker overrideAdsAimMarker;

	// Token: 0x0400076C RID: 1900
	private AccessoryBase accessoryInstance;

	// Token: 0x0400076D RID: 1901
	private bool created;

	// Token: 0x0400076E RID: 1902
	private Item masterItem;
}
