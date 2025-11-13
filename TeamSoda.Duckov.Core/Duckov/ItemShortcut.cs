using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000232 RID: 562
	public class ItemShortcut : MonoBehaviour
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00044828 File Offset: 0x00042A28
		private static CharacterMainControl Master
		{
			get
			{
				return CharacterMainControl.Main;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x0004482F File Offset: 0x00042A2F
		private static Inventory MainInventory
		{
			get
			{
				if (ItemShortcut.Master == null)
				{
					return null;
				}
				if (!ItemShortcut.Master.CharacterItem)
				{
					return null;
				}
				return ItemShortcut.Master.CharacterItem.Inventory;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00044862 File Offset: 0x00042A62
		public static int MaxIndex
		{
			get
			{
				if (ItemShortcut.Instance == null)
				{
					return 0;
				}
				return ItemShortcut.Instance.maxIndex;
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00044880 File Offset: 0x00042A80
		private void Awake()
		{
			if (ItemShortcut.Instance == null)
			{
				ItemShortcut.Instance = this;
			}
			else
			{
				Debug.LogError("检测到多个ItemShortcut");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x000448DF File Offset: 0x00042ADF
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00044914 File Offset: 0x00042B14
		private void Start()
		{
			this.Load();
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004491C File Offset: 0x00042B1C
		private void OnLevelInitialized()
		{
			this.Load();
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00044924 File Offset: 0x00042B24
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004492C File Offset: 0x00042B2C
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00044934 File Offset: 0x00042B34
		private void Load()
		{
			ItemShortcut.SaveData saveData = SavesSystem.Load<ItemShortcut.SaveData>("ItemShortcut_Data");
			if (saveData == null)
			{
				return;
			}
			saveData.ApplyTo(this);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004494C File Offset: 0x00042B4C
		private void Save()
		{
			ItemShortcut.SaveData saveData = new ItemShortcut.SaveData();
			saveData.Generate(this);
			SavesSystem.Save<ItemShortcut.SaveData>("ItemShortcut_Data", saveData);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00044974 File Offset: 0x00042B74
		public static bool IsItemValid(Item item)
		{
			return !(item == null) && !(ItemShortcut.MainInventory == null) && !(ItemShortcut.MainInventory != item.InInventory) && !item.Tags.Contains("Weapon");
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x000449C4 File Offset: 0x00042BC4
		private bool Set_Local(int index, Item item)
		{
			if (ItemShortcut.Master == null)
			{
				return false;
			}
			if (index < 0 || index > this.maxIndex)
			{
				return false;
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				return false;
			}
			while (this.items.Count <= index)
			{
				this.items.Add(null);
			}
			while (this.itemTypes.Count <= index)
			{
				this.itemTypes.Add(-1);
			}
			this.items[index] = item;
			this.itemTypes[index] = item.TypeID;
			Action<int> onSetItem = ItemShortcut.OnSetItem;
			if (onSetItem != null)
			{
				onSetItem(index);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (i != index)
				{
					bool flag = false;
					if (this.items[i] == item)
					{
						this.items[i] = null;
						flag = true;
					}
					if (this.itemTypes[i] == item.TypeID)
					{
						this.itemTypes[i] = -1;
						this.items[i] = null;
						flag = true;
					}
					if (flag)
					{
						ItemShortcut.OnSetItem(i);
					}
				}
			}
			return true;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00044AE0 File Offset: 0x00042CE0
		private Item Get_Local(int index)
		{
			if (index >= this.items.Count)
			{
				return null;
			}
			Item item = this.items[index];
			if (item == null)
			{
				item = ItemShortcut.MainInventory.Find(this.itemTypes[index]);
				if (item != null)
				{
					this.items[index] = item;
				}
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				this.SetDirty(index);
				return null;
			}
			return item;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00044B52 File Offset: 0x00042D52
		private void SetDirty(int index)
		{
			this.dirtyIndexes.Add(index);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00044B64 File Offset: 0x00042D64
		private void Update()
		{
			if (this.dirtyIndexes.Count > 0)
			{
				foreach (int num in this.dirtyIndexes.ToArray<int>())
				{
					if (num < this.items.Count && !ItemShortcut.IsItemValid(this.items[num]))
					{
						this.items[num] = null;
						Action<int> onSetItem = ItemShortcut.OnSetItem;
						if (onSetItem != null)
						{
							onSetItem(num);
						}
					}
				}
				this.dirtyIndexes.Clear();
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001199 RID: 4505 RVA: 0x00044BE8 File Offset: 0x00042DE8
		// (remove) Token: 0x0600119A RID: 4506 RVA: 0x00044C1C File Offset: 0x00042E1C
		public static event Action<int> OnSetItem;

		// Token: 0x0600119B RID: 4507 RVA: 0x00044C4F File Offset: 0x00042E4F
		public static Item Get(int index)
		{
			if (ItemShortcut.Instance == null)
			{
				return null;
			}
			return ItemShortcut.Instance.Get_Local(index);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00044C6B File Offset: 0x00042E6B
		public static bool Set(int index, Item item)
		{
			return !(ItemShortcut.Instance == null) && ItemShortcut.Instance.Set_Local(index, item);
		}

		// Token: 0x04000DA5 RID: 3493
		public static ItemShortcut Instance;

		// Token: 0x04000DA6 RID: 3494
		[SerializeField]
		private int maxIndex = 3;

		// Token: 0x04000DA7 RID: 3495
		[SerializeField]
		private List<Item> items = new List<Item>();

		// Token: 0x04000DA8 RID: 3496
		[SerializeField]
		private List<int> itemTypes = new List<int>();

		// Token: 0x04000DA9 RID: 3497
		private const string SaveKey = "ItemShortcut_Data";

		// Token: 0x04000DAA RID: 3498
		private HashSet<int> dirtyIndexes = new HashSet<int>();

		// Token: 0x02000531 RID: 1329
		[Serializable]
		private class SaveData
		{
			// Token: 0x17000763 RID: 1891
			// (get) Token: 0x06002814 RID: 10260 RVA: 0x00092BCF File Offset: 0x00090DCF
			public int Count
			{
				get
				{
					return this.inventoryIndexes.Count;
				}
			}

			// Token: 0x06002815 RID: 10261 RVA: 0x00092BDC File Offset: 0x00090DDC
			public void Generate(ItemShortcut shortcut)
			{
				this.inventoryIndexes.Clear();
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < shortcut.items.Count; i++)
				{
					Item item = shortcut.items[i];
					int index = mainInventory.GetIndex(item);
					this.inventoryIndexes.Add(index);
				}
			}

			// Token: 0x06002816 RID: 10262 RVA: 0x00092C3C File Offset: 0x00090E3C
			public void ApplyTo(ItemShortcut shortcut)
			{
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < this.inventoryIndexes.Count; i++)
				{
					int num = this.inventoryIndexes[i];
					if (num >= 0)
					{
						Item itemAt = mainInventory.GetItemAt(num);
						shortcut.Set_Local(i, itemAt);
					}
				}
			}

			// Token: 0x04001E9E RID: 7838
			[SerializeField]
			internal List<int> inventoryIndexes = new List<int>();
		}
	}
}
