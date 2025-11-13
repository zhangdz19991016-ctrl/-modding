using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class PetProxy : MonoBehaviour
{
	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x000293BA File Offset: 0x000275BA
	public static PetProxy Instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.PetProxy;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x000293D5 File Offset: 0x000275D5
	public static Inventory PetInventory
	{
		get
		{
			if (PetProxy.Instance == null)
			{
				return null;
			}
			return PetProxy.Instance.Inventory;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x000293F0 File Offset: 0x000275F0
	public Inventory Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x000293F8 File Offset: 0x000275F8
	private void Start()
	{
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		ItemSavesUtilities.LoadInventory("Inventory_Safe", this.inventory).Forget();
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00029420 File Offset: 0x00027620
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00029433 File Offset: 0x00027633
	private void OnCollectSaveData()
	{
		this.inventory.Save("Inventory_Safe");
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00029448 File Offset: 0x00027648
	public void DestroyItemInBase()
	{
		if (!this.Inventory)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in this.Inventory)
		{
			list.Add(item);
		}
		foreach (Item item2 in list)
		{
			if (item2.Tags.Contains("DestroyInBase"))
			{
				item2.DestroyTree();
			}
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x000294FC File Offset: 0x000276FC
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (LevelManager.Instance.PetCharacter == null)
		{
			return;
		}
		base.transform.position = LevelManager.Instance.PetCharacter.transform.position;
		if (this.checkTimer > 0f)
		{
			this.checkTimer -= Time.unscaledDeltaTime;
			return;
		}
		if (CharacterMainControl.Main.PetCapcity != this.inventory.Capacity)
		{
			this.inventory.SetCapacity(CharacterMainControl.Main.PetCapcity);
		}
		this.checkTimer = 1f;
	}

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000855 RID: 2133
	private float checkTimer = 0.02f;
}
