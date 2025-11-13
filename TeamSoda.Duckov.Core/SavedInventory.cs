using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class SavedInventory : MonoBehaviour
{
	// Token: 0x060007D2 RID: 2002 RVA: 0x000235E3 File Offset: 0x000217E3
	private void Awake()
	{
		if (this.inventory == null)
		{
			this.inventory = base.GetComponent<Inventory>();
		}
		this.Register();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00023605 File Offset: 0x00021805
	private void Start()
	{
		if (this.registered)
		{
			this.Load();
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00023615 File Offset: 0x00021815
	private void OnDestroy()
	{
		this.Unregsister();
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00023620 File Offset: 0x00021820
	private void Register()
	{
		SavedInventory savedInventory;
		if (SavedInventory.activeInventories.TryGetValue(this.key, out savedInventory))
		{
			Debug.LogError("存在多个带有相同Key的Saved Inventory: " + this.key, base.gameObject);
			return;
		}
		SavesSystem.OnCollectSaveData += this.Save;
		this.registered = true;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00023675 File Offset: 0x00021875
	private void Unregsister()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00023688 File Offset: 0x00021888
	private void Save()
	{
		this.inventory.Save(this.key);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002369B File Offset: 0x0002189B
	private void Load()
	{
		if (this.inventory.Loading)
		{
			Debug.LogError("Inventory is already loading.", base.gameObject);
			return;
		}
		ItemSavesUtilities.LoadInventory(this.key, this.inventory).Forget();
	}

	// Token: 0x04000763 RID: 1891
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000764 RID: 1892
	[SerializeField]
	private string key = "DefaultSavedInventory";

	// Token: 0x04000765 RID: 1893
	private static Dictionary<string, SavedInventory> activeInventories = new Dictionary<string, SavedInventory>();

	// Token: 0x04000766 RID: 1894
	private bool registered;
}
