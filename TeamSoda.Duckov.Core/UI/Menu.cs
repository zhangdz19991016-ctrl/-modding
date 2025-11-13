using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x02000212 RID: 530
	public class Menu : MonoBehaviour
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0003EB6F File Offset: 0x0003CD6F
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0003EB77 File Offset: 0x0003CD77
		public bool Focused
		{
			get
			{
				return this.focused;
			}
			set
			{
				this.SetFocused(value);
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06000FDA RID: 4058 RVA: 0x0003EB80 File Offset: 0x0003CD80
		// (remove) Token: 0x06000FDB RID: 4059 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
		public event Action<Menu, MenuItem> onSelectionChanged;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06000FDC RID: 4060 RVA: 0x0003EBF0 File Offset: 0x0003CDF0
		// (remove) Token: 0x06000FDD RID: 4061 RVA: 0x0003EC28 File Offset: 0x0003CE28
		public event Action<Menu, MenuItem> onConfirmed;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06000FDE RID: 4062 RVA: 0x0003EC60 File Offset: 0x0003CE60
		// (remove) Token: 0x06000FDF RID: 4063 RVA: 0x0003EC98 File Offset: 0x0003CE98
		public event Action<Menu, MenuItem> onCanceled;

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003ECCD File Offset: 0x0003CECD
		private void SetFocused(bool value)
		{
			this.focused = value;
			if (this.focused && this.cursor == null)
			{
				this.SelectDefault();
			}
			MenuItem menuItem = this.cursor;
			if (menuItem == null)
			{
				return;
			}
			menuItem.NotifyMasterFocusStatusChanged();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003ED02 File Offset: 0x0003CF02
		public MenuItem GetSelected()
		{
			return this.cursor;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003ED0C File Offset: 0x0003CF0C
		public T GetSelected<T>() where T : Component
		{
			if (this.cursor == null)
			{
				return default(T);
			}
			return this.cursor.GetComponent<T>();
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003ED3C File Offset: 0x0003CF3C
		public void Select(MenuItem toSelect)
		{
			if (toSelect.transform.parent != base.transform)
			{
				Debug.LogError("正在尝试选中不属于此菜单的项目。已取消。");
				return;
			}
			if (!this.items.Contains(toSelect))
			{
				this.items.Add(toSelect);
			}
			if (!toSelect.Selectable)
			{
				return;
			}
			if (this.cursor != null)
			{
				this.DeselectCurrent();
			}
			this.cursor = toSelect;
			this.cursor.NotifySelected();
			this.OnSelectionChanged();
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003EDBC File Offset: 0x0003CFBC
		public void SelectDefault()
		{
			MenuItem[] componentsInChildren = base.GetComponentsInChildren<MenuItem>(false);
			if (componentsInChildren == null)
			{
				return;
			}
			foreach (MenuItem menuItem in componentsInChildren)
			{
				if (!(menuItem == null) && menuItem.Selectable)
				{
					this.Select(menuItem);
				}
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003EDFF File Offset: 0x0003CFFF
		public void Confirm()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyConfirmed();
			}
			Action<Menu, MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0003EE31 File Offset: 0x0003D031
		public void Cancel()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyCanceled();
			}
			Action<Menu, MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0003EE63 File Offset: 0x0003D063
		private void DeselectCurrent()
		{
			this.cursor.NotifyDeselected();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0003EE70 File Offset: 0x0003D070
		private void OnSelectionChanged()
		{
			Action<Menu, MenuItem> action = this.onSelectionChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0003EE8C File Offset: 0x0003D08C
		public void Navigate(Vector2 direction)
		{
			if (this.cursor == null)
			{
				this.SelectDefault();
			}
			if (this.cursor == null)
			{
				return;
			}
			if (Mathf.Approximately(direction.sqrMagnitude, 0f))
			{
				return;
			}
			MenuItem menuItem = this.FindClosestEntryInDirection(this.cursor, direction);
			if (menuItem == null)
			{
				return;
			}
			this.Select(menuItem);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003EEF0 File Offset: 0x0003D0F0
		private MenuItem FindClosestEntryInDirection(MenuItem cursor, Vector2 direction)
		{
			if (cursor == null)
			{
				return null;
			}
			direction = direction.normalized;
			float num = Mathf.Cos(0.7853982f);
			Menu.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.bestMatch = null;
			CS$<>8__locals1.bestSqrDist = float.MaxValue;
			CS$<>8__locals1.bestDot = num;
			foreach (MenuItem cur in this.items)
			{
				Menu.<>c__DisplayClass26_1 CS$<>8__locals2;
				CS$<>8__locals2.cur = cur;
				if (!(CS$<>8__locals2.cur == null) && !(CS$<>8__locals2.cur == cursor) && CS$<>8__locals2.cur.Selectable)
				{
					Vector3 vector = CS$<>8__locals2.cur.transform.localPosition - cursor.transform.localPosition;
					Vector3 normalized = vector.normalized;
					Menu.<>c__DisplayClass26_2 CS$<>8__locals3;
					CS$<>8__locals3.dot = Vector3.Dot(normalized, direction);
					if (CS$<>8__locals3.dot >= num)
					{
						CS$<>8__locals3.sqrDist = vector.magnitude;
						if (CS$<>8__locals3.sqrDist <= CS$<>8__locals1.bestSqrDist)
						{
							if (CS$<>8__locals3.sqrDist < CS$<>8__locals1.bestSqrDist)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
							else if (CS$<>8__locals3.sqrDist == CS$<>8__locals1.bestSqrDist && CS$<>8__locals3.dot > CS$<>8__locals1.bestDot)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
						}
					}
				}
			}
			return CS$<>8__locals1.bestMatch;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003F068 File Offset: 0x0003D268
		internal void Register(MenuItem menuItem)
		{
			this.items.Add(menuItem);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003F077 File Offset: 0x0003D277
		internal void Unegister(MenuItem menuItem)
		{
			this.items.Remove(menuItem);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003F099 File Offset: 0x0003D299
		[CompilerGenerated]
		internal static void <FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref Menu.<>c__DisplayClass26_0 A_0, ref Menu.<>c__DisplayClass26_1 A_1, ref Menu.<>c__DisplayClass26_2 A_2)
		{
			A_0.bestMatch = A_1.cur;
			A_0.bestSqrDist = A_2.sqrDist;
			A_0.bestDot = A_2.dot;
		}

		// Token: 0x04000CD3 RID: 3283
		[SerializeField]
		private bool focused;

		// Token: 0x04000CD4 RID: 3284
		[SerializeField]
		private MenuItem cursor;

		// Token: 0x04000CD5 RID: 3285
		[SerializeField]
		private LayoutGroup layout;

		// Token: 0x04000CD9 RID: 3289
		private HashSet<MenuItem> items = new HashSet<MenuItem>();
	}
}
