using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003D1 RID: 977
	public class PagesControl : MonoBehaviour
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x0007D4E4 File Offset: 0x0007B6E4
		private PrefabPool<PagesControl_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PagesControl_Entry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0007D51D File Offset: 0x0007B71D
		private void Start()
		{
			if (this.target != null)
			{
				this.Setup(this.target);
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0007D539 File Offset: 0x0007B739
		public void Setup(InventoryDisplay target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0007D554 File Offset: 0x0007B754
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed += this.OnPageInfoRefreshed;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0007D582 File Offset: 0x0007B782
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed -= this.OnPageInfoRefreshed;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0007D5AA File Offset: 0x0007B7AA
		private void OnPageInfoRefreshed()
		{
			this.Refresh();
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0007D5B4 File Offset: 0x0007B7B4
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.inputIndicators)
			{
				GameObject gameObject = this.inputIndicators;
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.target == null)
			{
				return;
			}
			if (!this.target.UsePages)
			{
				return;
			}
			if (this.target.MaxPage <= 1)
			{
				return;
			}
			for (int i = 0; i < this.target.MaxPage; i++)
			{
				this.Pool.Get(null).Setup(this, i, this.target.SelectedPage == i);
			}
			if (this.inputIndicators)
			{
				GameObject gameObject2 = this.inputIndicators;
				if (gameObject2 == null)
				{
					return;
				}
				gameObject2.SetActive(true);
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0007D66C File Offset: 0x0007B86C
		internal void NotifySelect(int i)
		{
			if (this.target == null)
			{
				return;
			}
			this.target.SetPage(i);
		}

		// Token: 0x0400183B RID: 6203
		[SerializeField]
		private InventoryDisplay target;

		// Token: 0x0400183C RID: 6204
		[SerializeField]
		private PagesControl_Entry template;

		// Token: 0x0400183D RID: 6205
		[SerializeField]
		private GameObject inputIndicators;

		// Token: 0x0400183E RID: 6206
		private PrefabPool<PagesControl_Entry> _pool;
	}
}
