using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001F2 RID: 498
public class InventoryFilterDisplay : MonoBehaviour, ISingleSelectionMenu<InventoryFilterDisplayEntry>
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0003B5E0 File Offset: 0x000397E0
	private PrefabPool<InventoryFilterDisplayEntry> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<InventoryFilterDisplayEntry>(this.template, null, null, null, null, true, 10, 10000, null);
			}
			return this._pool;
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0003B619 File Offset: 0x00039819
	private void Awake()
	{
		this.template.gameObject.SetActive(false);
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0003B62C File Offset: 0x0003982C
	public void Setup(InventoryDisplay target)
	{
		this.Pool.ReleaseAll();
		this.entries.Clear();
		if (target == null)
		{
			return;
		}
		this.targetDisplay = target;
		this.provider = target.Target.GetComponent<InventoryFilterProvider>();
		if (this.provider == null)
		{
			return;
		}
		foreach (InventoryFilterProvider.FilterEntry filter in this.provider.entries)
		{
			InventoryFilterDisplayEntry inventoryFilterDisplayEntry = this.Pool.Get(null);
			inventoryFilterDisplayEntry.Setup(new Action<InventoryFilterDisplayEntry, PointerEventData>(this.OnEntryClicked), filter);
			this.entries.Add(inventoryFilterDisplayEntry);
		}
		this.selection = null;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0003B6D5 File Offset: 0x000398D5
	private void OnEntryClicked(InventoryFilterDisplayEntry entry, PointerEventData data)
	{
		this.SetSelection(entry);
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0003B6DF File Offset: 0x000398DF
	internal void Select(int i)
	{
		if (i < 0 || i >= this.entries.Count)
		{
			return;
		}
		this.SetSelection(this.entries[i]);
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0003B707 File Offset: 0x00039907
	public InventoryFilterDisplayEntry GetSelection()
	{
		return this.selection;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0003B710 File Offset: 0x00039910
	public bool SetSelection(InventoryFilterDisplayEntry selection)
	{
		if (selection == null)
		{
			return false;
		}
		this.selection = selection;
		InventoryFilterProvider.FilterEntry filter = selection.Filter;
		this.targetDisplay.SetFilter(filter.GetFunction());
		foreach (InventoryFilterDisplayEntry inventoryFilterDisplayEntry in this.entries)
		{
			inventoryFilterDisplayEntry.NotifySelectionChanged(inventoryFilterDisplayEntry == selection);
		}
		return true;
	}

	// Token: 0x04000C35 RID: 3125
	[SerializeField]
	private InventoryFilterDisplayEntry template;

	// Token: 0x04000C36 RID: 3126
	private PrefabPool<InventoryFilterDisplayEntry> _pool;

	// Token: 0x04000C37 RID: 3127
	private InventoryDisplay targetDisplay;

	// Token: 0x04000C38 RID: 3128
	private InventoryFilterProvider provider;

	// Token: 0x04000C39 RID: 3129
	private List<InventoryFilterDisplayEntry> entries = new List<InventoryFilterDisplayEntry>();

	// Token: 0x04000C3A RID: 3130
	private InventoryFilterDisplayEntry selection;
}
