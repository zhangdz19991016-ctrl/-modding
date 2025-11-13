using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000310 RID: 784
	public class SupplyPanel : MonoBehaviour
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x0005E15B File Offset: 0x0005C35B
		// (set) Token: 0x060019DC RID: 6620 RVA: 0x0005E163 File Offset: 0x0005C363
		public BlackMarket Target { get; private set; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x0005E16C File Offset: 0x0005C36C
		private PrefabPool<SupplyPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<SupplyPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<SupplyPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0005E1B0 File Offset: 0x0005C3B0
		private void OnCreateEntry(SupplyPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		private void OnEntryClicked(SupplyPanel_Entry entry)
		{
			Debug.Log("Supply entry clicked");
			this.Target.Buy(entry.Target);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0005E1E2 File Offset: 0x0005C3E2
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 Supply Panel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0005E213 File Offset: 0x0005C413
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005E221 File Offset: 0x0005C421
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0005E22C File Offset: 0x0005C42C
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Supplies)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0005E2A4 File Offset: 0x0005C4A4
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0005E2CC File Offset: 0x0005C4CC
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0005E2FA File Offset: 0x0005C4FA
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x040012B2 RID: 4786
		[SerializeField]
		private SupplyPanel_Entry entryTemplate;

		// Token: 0x040012B3 RID: 4787
		private PrefabPool<SupplyPanel_Entry> _entryPool;
	}
}
