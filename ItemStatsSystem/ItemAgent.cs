using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000008 RID: 8
	[AddComponentMenu("ItemAgent(不要用，用DuckovItemAgent)")]
	public class ItemAgent : MonoBehaviour
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002C7B File Offset: 0x00000E7B
		public ItemAgent.AgentTypes AgentType
		{
			get
			{
				return this.agentType;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002C83 File Offset: 0x00000E83
		public Item Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C8B File Offset: 0x00000E8B
		public void Initialize(Item item, ItemAgent.AgentTypes _agentType)
		{
			this.item = item;
			this.agentType = _agentType;
			item.onUnpluggedFromSlot += this.OnUnplugedFromSlot;
			this.OnInitialize();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002CB3 File Offset: 0x00000EB3
		protected virtual void OnDestroy()
		{
			if (this.item != null)
			{
				this.item.onUnpluggedFromSlot -= this.OnUnplugedFromSlot;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002CDC File Offset: 0x00000EDC
		private void OnUnplugedFromSlot(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (item.AgentUtilities == null)
			{
				return;
			}
			if (item.AgentUtilities.ActiveAgent == null)
			{
				return;
			}
			if (item.AgentUtilities.ActiveAgent != this)
			{
				Debug.LogError("release的对象是" + item.AgentUtilities.ActiveAgent.gameObject.name + ",而调用者是" + base.gameObject.name);
			}
			item.AgentUtilities.ReleaseActiveAgent();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002D62 File Offset: 0x00000F62
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x04000020 RID: 32
		private Item item;

		// Token: 0x04000021 RID: 33
		protected ItemAgent.AgentTypes agentType;

		// Token: 0x0200003B RID: 59
		public enum AgentTypes
		{
			// Token: 0x040000F7 RID: 247
			normal,
			// Token: 0x040000F8 RID: 248
			pickUp,
			// Token: 0x040000F9 RID: 249
			handheld,
			// Token: 0x040000FA RID: 250
			equipment
		}
	}
}
