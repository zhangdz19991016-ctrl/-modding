using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039F RID: 927
	public class UsageUtilitiesDisplay : MonoBehaviour
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000728DF File Offset: 0x00070ADF
		// (set) Token: 0x060020B9 RID: 8377 RVA: 0x000728E7 File Offset: 0x00070AE7
		public UsageUtilities Target { get; private set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x000728F0 File Offset: 0x00070AF0
		private PrefabPool<UsageUtilitiesDisplay_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<UsageUtilitiesDisplay_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x0007292C File Offset: 0x00070B2C
		public void Setup(Item item)
		{
			if (!(item == null))
			{
				UsageUtilities component = item.GetComponent<UsageUtilities>();
				if (!(component == null))
				{
					this.Target = component;
					base.gameObject.SetActive(true);
					this.Refresh();
					return;
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00072978 File Offset: 0x00070B78
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			foreach (UsageBehavior usageBehavior in this.Target.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.DisplaySettings.display)
				{
					this.EntryPool.Get(null).Setup(usageBehavior);
				}
			}
			if (this.EntryPool.ActiveEntries.Count <= 0)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private UsageUtilitiesDisplay_Entry entryTemplate;

		// Token: 0x04001644 RID: 5700
		private PrefabPool<UsageUtilitiesDisplay_Entry> _entryPool;
	}
}
