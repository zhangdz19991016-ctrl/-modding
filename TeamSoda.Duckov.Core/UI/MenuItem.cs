using System;
using UnityEngine;

namespace UI
{
	// Token: 0x02000213 RID: 531
	public class MenuItem : MonoBehaviour
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0003F0BF File Offset: 0x0003D2BF
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0003F0F2 File Offset: 0x0003D2F2
		public Menu Master
		{
			get
			{
				if (this._master == null)
				{
					Transform parent = base.transform.parent;
					this._master = ((parent != null) ? parent.GetComponent<Menu>() : null);
				}
				return this._master;
			}
			set
			{
				this._master = value;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0003F0FB File Offset: 0x0003D2FB
		// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0003F112 File Offset: 0x0003D312
		public bool Selectable
		{
			get
			{
				return base.gameObject.activeSelf && this.selectable;
			}
			set
			{
				this.selectable = value;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0003F11B File Offset: 0x0003D31B
		public bool IsSelected
		{
			get
			{
				return this.cacheSelected;
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003F123 File Offset: 0x0003D323
		private void OnTransformParentChanged()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003F140 File Offset: 0x0003D340
		private void OnEnable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003F15D File Offset: 0x0003D35D
		private void OnDisable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Unegister(this);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0003F17A File Offset: 0x0003D37A
		public void Select()
		{
			if (this.Master == null)
			{
				Debug.LogError("Menu Item " + base.name + " 没有Master。");
				return;
			}
			this.Master.Select(this);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003F1B1 File Offset: 0x0003D3B1
		internal void NotifySelected()
		{
			this.cacheSelected = true;
			Action<MenuItem> action = this.onSelected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0003F1CB File Offset: 0x0003D3CB
		internal void NotifyDeselected()
		{
			this.cacheSelected = false;
			Action<MenuItem> action = this.onDeselected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0003F1E5 File Offset: 0x0003D3E5
		internal void NotifyConfirmed()
		{
			Action<MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0003F1F8 File Offset: 0x0003D3F8
		internal void NotifyCanceled()
		{
			Action<MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0003F20B File Offset: 0x0003D40B
		internal void NotifyMasterFocusStatusChanged()
		{
			Action<MenuItem, bool> action = this.onFocusStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Master.Focused);
		}

		// Token: 0x04000CDA RID: 3290
		private Menu _master;

		// Token: 0x04000CDB RID: 3291
		[SerializeField]
		private bool selectable = true;

		// Token: 0x04000CDC RID: 3292
		private bool cacheSelected;

		// Token: 0x04000CDD RID: 3293
		public Action<MenuItem> onSelected;

		// Token: 0x04000CDE RID: 3294
		public Action<MenuItem> onDeselected;

		// Token: 0x04000CDF RID: 3295
		public Action<MenuItem> onConfirmed;

		// Token: 0x04000CE0 RID: 3296
		public Action<MenuItem> onCanceled;

		// Token: 0x04000CE1 RID: 3297
		public Action<MenuItem, bool> onFocusStatusChanged;
	}
}
