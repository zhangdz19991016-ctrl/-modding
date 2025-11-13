using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039A RID: 922
	public class ItemEffectEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002091 RID: 8337 RVA: 0x000724A1 File Offset: 0x000706A1
		public void NotifyPooled()
		{
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000724A3 File Offset: 0x000706A3
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000724B2 File Offset: 0x000706B2
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000724BA File Offset: 0x000706BA
		public void Setup(Effect target)
		{
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000724CF File Offset: 0x000706CF
		private void Refresh()
		{
			this.text.text = this.target.GetDisplayString();
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000724E7 File Offset: 0x000706E7
		private void RegisterEvents()
		{
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000724E9 File Offset: 0x000706E9
		private void UnregisterEvents()
		{
		}

		// Token: 0x04001632 RID: 5682
		private Effect target;

		// Token: 0x04001633 RID: 5683
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
