using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x0200030E RID: 782
	public class DemandPanel : MonoBehaviour
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x0005DCE5 File Offset: 0x0005BEE5
		// (set) Token: 0x060019BE RID: 6590 RVA: 0x0005DCED File Offset: 0x0005BEED
		public BlackMarket Target { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x0005DCF8 File Offset: 0x0005BEF8
		private PrefabPool<DemandPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DemandPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<DemandPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0005DD3C File Offset: 0x0005BF3C
		private void OnCreateEntry(DemandPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0005DD50 File Offset: 0x0005BF50
		private void OnEntryClicked(DemandPanel_Entry entry)
		{
			this.Target.Sell(entry.Target);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0005DD64 File Offset: 0x0005BF64
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 DemandPanel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0005DD95 File Offset: 0x0005BF95
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0005DDA3 File Offset: 0x0005BFA3
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0005DDAC File Offset: 0x0005BFAC
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Demands)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0005DE24 File Offset: 0x0005C024
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0005DE4C File Offset: 0x0005C04C
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0005DE7A File Offset: 0x0005C07A
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x040012A3 RID: 4771
		[SerializeField]
		private DemandPanel_Entry entryTemplate;

		// Token: 0x040012A4 RID: 4772
		private PrefabPool<DemandPanel_Entry> _entryPool;
	}
}
