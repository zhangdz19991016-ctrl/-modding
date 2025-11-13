using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class ItemAgentUtilities
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002D6C File Offset: 0x00000F6C
		public Item Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002D74 File Offset: 0x00000F74
		public ItemAgent ActiveAgent
		{
			get
			{
				return this.activeAgent;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002D7C File Offset: 0x00000F7C
		private Dictionary<int, ItemAgentUtilities.AgentKeyPair> HashedAgents
		{
			get
			{
				if (this.hashedAgentsCache == null)
				{
					this.hashedAgentsCache = new Dictionary<int, ItemAgentUtilities.AgentKeyPair>();
					foreach (ItemAgentUtilities.AgentKeyPair agentKeyPair in this.agents)
					{
						this.hashedAgentsCache.Add(agentKeyPair.key.GetHashCode(), agentKeyPair);
					}
				}
				return this.hashedAgentsCache;
			}
		}

		// Token: 0x1700000E RID: 14
		public ItemAgent this[int hash]
		{
			get
			{
				return this.GetPrefab(hash);
			}
		}

		// Token: 0x1700000F RID: 15
		public ItemAgent this[string key]
		{
			get
			{
				return this.GetPrefab(key);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000036 RID: 54 RVA: 0x00002E0C File Offset: 0x0000100C
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x00002E44 File Offset: 0x00001044
		public event Action<Item, ItemAgent> onCreateAgent;

		// Token: 0x06000038 RID: 56 RVA: 0x00002E7C File Offset: 0x0000107C
		public ItemAgent GetPrefab(int hash)
		{
			ItemAgentUtilities.AgentKeyPair agentKeyPair;
			if (this.HashedAgents.TryGetValue(hash, out agentKeyPair))
			{
				return agentKeyPair.agentPrefab;
			}
			return null;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002EA1 File Offset: 0x000010A1
		public ItemAgent GetPrefab(string key)
		{
			return this.GetPrefab(key.GetHashCode());
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002EAF File Offset: 0x000010AF
		public void Initialize(Item master)
		{
			this.master = master;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002EB8 File Offset: 0x000010B8
		public ItemAgent CreateAgent(int hash, ItemAgent.AgentTypes agentType)
		{
			ItemAgent prefab = this.GetPrefab(hash);
			return this.CreateAgent(prefab, agentType);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002ED8 File Offset: 0x000010D8
		public ItemAgent CreateAgent(ItemAgent prefab, ItemAgent.AgentTypes agentType)
		{
			if (prefab == null)
			{
				return null;
			}
			if (this.Master == null)
			{
				Debug.Log("Create agent:" + prefab.name + " failed,master is null");
				return null;
			}
			if (this.ActiveAgent != null)
			{
				this.ReleaseActiveAgent();
				Debug.Log("Creating agent:" + prefab.name + ",destrory another agent");
			}
			ItemAgent itemAgent = UnityEngine.Object.Instantiate<ItemAgent>(prefab);
			this.activeAgent = itemAgent;
			itemAgent.Initialize(this.Master, agentType);
			Action<Item, ItemAgent> action = this.onCreateAgent;
			if (action != null)
			{
				action(this.Master, itemAgent);
			}
			return itemAgent;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002F7C File Offset: 0x0000117C
		public void ReleaseActiveAgent()
		{
			if (this.ActiveAgent == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(this.ActiveAgent.gameObject);
			this.activeAgent = null;
		}

		// Token: 0x04000022 RID: 34
		private Item master;

		// Token: 0x04000023 RID: 35
		private ItemAgent activeAgent;

		// Token: 0x04000024 RID: 36
		[SerializeField]
		private List<ItemAgentUtilities.AgentKeyPair> agents;

		// Token: 0x04000025 RID: 37
		private Dictionary<int, ItemAgentUtilities.AgentKeyPair> hashedAgentsCache;

		// Token: 0x0200003C RID: 60
		[Serializable]
		public class AgentKeyPair
		{
			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x0600025C RID: 604 RVA: 0x000091EA File Offset: 0x000073EA
			private StringList avaliableKeys
			{
				get
				{
					return StringLists.ItemAgentKeys;
				}
			}

			// Token: 0x040000FB RID: 251
			public string key;

			// Token: 0x040000FC RID: 252
			public ItemAgent agentPrefab;
		}
	}
}
