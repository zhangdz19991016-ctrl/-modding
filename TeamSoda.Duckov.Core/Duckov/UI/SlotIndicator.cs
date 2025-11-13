using System;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A6 RID: 934
	public class SlotIndicator : MonoBehaviour, IPoolable
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x00075123 File Offset: 0x00073323
		// (set) Token: 0x0600216C RID: 8556 RVA: 0x0007512B File Offset: 0x0007332B
		public Slot Target { get; private set; }

		// Token: 0x0600216D RID: 8557 RVA: 0x00075134 File Offset: 0x00073334
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x0007514F File Offset: 0x0007334F
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00075177 File Offset: 0x00073377
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00075199 File Offset: 0x00073399
		private void OnSlotContentChanged(Slot slot)
		{
			if (slot != this.Target)
			{
				Debug.LogError("Slot内容改变事件触发了，但它来自别的Slot。这说明Slot Indicator注册的事件发生了泄露，请检查代码。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000751B5 File Offset: 0x000733B5
		private void Refresh()
		{
			if (this.contentIndicator == null)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			this.contentIndicator.SetActive(this.Target.Content);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000751EA File Offset: 0x000733EA
		public void NotifyPooled()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000751F8 File Offset: 0x000733F8
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.contentIndicator.SetActive(false);
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00075213 File Offset: 0x00073413
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00075221 File Offset: 0x00073421
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x040016AC RID: 5804
		[SerializeField]
		private GameObject contentIndicator;
	}
}
