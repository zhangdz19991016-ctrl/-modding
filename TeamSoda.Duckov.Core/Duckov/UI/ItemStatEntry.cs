using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039C RID: 924
	public class ItemStatEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x060020A0 RID: 8352 RVA: 0x00072609 File Offset: 0x00070809
		public void NotifyPooled()
		{
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x0007260B File Offset: 0x0007080B
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x0007261A File Offset: 0x0007081A
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00072622 File Offset: 0x00070822
		internal void Setup(Stat target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00072640 File Offset: 0x00070840
		private void Refresh()
		{
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.Value.ToString(entry.DisplayFormat);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00072699 File Offset: 0x00070899
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetSetDirty;
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000726BB File Offset: 0x000708BB
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetSetDirty;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000726DD File Offset: 0x000708DD
		private void OnTargetSetDirty(Stat stat)
		{
			if (stat != this.target)
			{
				Debug.LogError("ItemStatEntry.target与事件触发者不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x0400163A RID: 5690
		private Stat target;

		// Token: 0x0400163B RID: 5691
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x0400163C RID: 5692
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
