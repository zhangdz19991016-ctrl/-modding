using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000DC RID: 220
public class InteractableLootbox : InteractableBase
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001F863 File Offset: 0x0001DA63
	public bool ShowSortButton
	{
		get
		{
			return this.showSortButton;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001F86B File Offset: 0x0001DA6B
	public bool UsePages
	{
		get
		{
			return this.usePages;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001F873 File Offset: 0x0001DA73
	public static Transform LootBoxInventoriesParent
	{
		get
		{
			return LevelManager.LootBoxInventoriesParent;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001F87A File Offset: 0x0001DA7A
	public static Dictionary<int, Inventory> Inventories
	{
		get
		{
			return LevelManager.LootBoxInventories;
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0001F884 File Offset: 0x0001DA84
	public static Inventory GetOrCreateInventory(InteractableLootbox lootBox)
	{
		if (lootBox == null)
		{
			if (CharacterMainControl.Main != null)
			{
				CharacterMainControl.Main.PopText("ERROR:尝试创建Inventory, 但lootbox是null", -1f);
			}
			Debug.LogError("尝试创建Inventory, 但lootbox是null");
			return null;
		}
		int key = lootBox.GetKey();
		Inventory inventory;
		if (InteractableLootbox.Inventories.TryGetValue(key, out inventory))
		{
			if (!(inventory == null))
			{
				return inventory;
			}
			CharacterMainControl.Main.PopText(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key), -1f);
			Debug.LogError(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key));
		}
		GameObject gameObject = new GameObject(string.Format("Inventory_{0}", key));
		gameObject.transform.SetParent(InteractableLootbox.LootBoxInventoriesParent);
		gameObject.transform.position = lootBox.transform.position;
		inventory = gameObject.AddComponent<Inventory>();
		inventory.NeedInspection = lootBox.needInspect;
		InteractableLootbox.Inventories.Add(key, inventory);
		LootBoxLoader component = lootBox.GetComponent<LootBoxLoader>();
		if (component && component.autoSetup)
		{
			component.Setup().Forget();
		}
		return inventory;
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0001F99C File Offset: 0x0001DB9C
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return vector3Int.GetHashCode();
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001F9F8 File Offset: 0x0001DBF8
	public Inventory Inventory
	{
		get
		{
			Inventory orCreateInventory;
			if (this.inventoryReference)
			{
				orCreateInventory = this.inventoryReference;
			}
			else
			{
				orCreateInventory = InteractableLootbox.GetOrCreateInventory(this);
				if (orCreateInventory == null)
				{
					if (LevelManager.Instance == null)
					{
						Debug.Log("LevelManager.Instance 不存在，取消创建i nventory");
						return null;
					}
					LevelManager.Instance.MainCharacter.PopText("空的Inventory", -1f);
					Debug.LogError("未能成功创建Inventory," + base.gameObject.name, this);
				}
				this.inventoryReference = orCreateInventory;
			}
			if (this.inventoryReference && this.inventoryReference.hasBeenInspectedInLootBox)
			{
				base.SetMarkerUsed();
			}
			orCreateInventory.DisplayNameKey = this.displayNameKey;
			return orCreateInventory;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001FAAE File Offset: 0x0001DCAE
	public bool Looted
	{
		get
		{
			return LootView.HasInventoryEverBeenLooted(this.Inventory);
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000703 RID: 1795 RVA: 0x0001FABC File Offset: 0x0001DCBC
	// (remove) Token: 0x06000704 RID: 1796 RVA: 0x0001FAF0 File Offset: 0x0001DCF0
	public static event Action<InteractableLootbox> OnStartLoot;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000705 RID: 1797 RVA: 0x0001FB24 File Offset: 0x0001DD24
	// (remove) Token: 0x06000706 RID: 1798 RVA: 0x0001FB58 File Offset: 0x0001DD58
	public static event Action<InteractableLootbox> OnStopLoot;

	// Token: 0x06000707 RID: 1799 RVA: 0x0001FB8C File Offset: 0x0001DD8C
	protected override void Start()
	{
		base.Start();
		if (this.inventoryReference == null)
		{
			InteractableLootbox.GetOrCreateInventory(this);
		}
		if (this.Inventory && this.Inventory.hasBeenInspectedInLootBox)
		{
			base.SetMarkerUsed();
		}
		this.overrideInteractName = true;
		base.InteractName = this.displayNameKey;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0001FBE7 File Offset: 0x0001DDE7
	protected override bool IsInteractable()
	{
		if (this.Inventory == null)
		{
			if (CharacterMainControl.Main)
			{
				CharacterMainControl.Main.PopText("ERROR :( 存在不包含Inventory的Lootbox。", -1f);
			}
			return false;
		}
		return this.lootState == InteractableLootbox.LootBoxStates.closed;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0001FC24 File Offset: 0x0001DE24
	protected override void OnUpdate(CharacterMainControl interactCharacter, float deltaTime)
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			if (LootView.Instance && LootView.Instance.open)
			{
				LootView.Instance.Close();
			}
			return;
		}
		switch (this.lootState)
		{
		case InteractableLootbox.LootBoxStates.closed:
			base.StopInteract();
			return;
		case InteractableLootbox.LootBoxStates.openning:
			if (interactCharacter.CurrentAction.ActionTimer >= base.InteractTime && !this.Inventory.Loading)
			{
				if (this.StartLoot())
				{
					this.lootState = InteractableLootbox.LootBoxStates.looting;
					return;
				}
				CharacterMainControl.Main.PopText("ERROR :Start loot失败，终止交互。", -1f);
				base.StopInteract();
				this.lootState = InteractableLootbox.LootBoxStates.closed;
				return;
			}
			break;
		case InteractableLootbox.LootBoxStates.looting:
			if (!LootView.Instance || !LootView.Instance.open)
			{
				CharacterMainControl.Main.PopText("ERROR :打开Loot界面失败，终止交互。", -1f);
				base.StopInteract();
				return;
			}
			if (this.inspectingItem != null)
			{
				this.inspectTimer += deltaTime;
				if (this.inspectTimer >= this.inspectTime)
				{
					this.inspectingItem.Inspected = true;
					this.inspectingItem.Inspecting = false;
				}
				if (!this.inspectingItem.Inspecting)
				{
					this.inspectingItem = null;
					return;
				}
			}
			else
			{
				Item item = this.FindFistUninspectedItem();
				if (!item)
				{
					base.StopInteract();
					return;
				}
				this.StartInspectItem(item);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0001FD88 File Offset: 0x0001DF88
	private void StartInspectItem(Item item)
	{
		if (item == null)
		{
			return;
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.inspectingItem = item;
		this.inspectingItem.Inspecting = true;
		this.inspectTimer = 0f;
		this.inspectTime = GameplayDataSettings.LootingData.GetInspectingTime(item);
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0001FDE3 File Offset: 0x0001DFE3
	private void UpdateInspect()
	{
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0001FDE8 File Offset: 0x0001DFE8
	private Item FindFistUninspectedItem()
	{
		if (!this.Inventory)
		{
			return null;
		}
		if (!this.Inventory.NeedInspection)
		{
			return null;
		}
		return this.Inventory.FirstOrDefault((Item e) => !e.Inspected);
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0001FE3D File Offset: 0x0001E03D
	protected override void OnInteractStart(CharacterMainControl interactCharacter)
	{
		this.lootState = InteractableLootbox.LootBoxStates.openning;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0001FE48 File Offset: 0x0001E048
	protected override void OnInteractStop()
	{
		this.lootState = InteractableLootbox.LootBoxStates.closed;
		Action<InteractableLootbox> onStopLoot = InteractableLootbox.OnStopLoot;
		if (onStopLoot != null)
		{
			onStopLoot(this);
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		if (this.Inventory)
		{
			this.Inventory.hasBeenInspectedInLootBox = true;
		}
		base.SetMarkerUsed();
		this.CheckHideIfEmpty();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0001FEAC File Offset: 0x0001E0AC
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.CheckHideIfEmpty();
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0001FED4 File Offset: 0x0001E0D4
	public void CheckHideIfEmpty()
	{
		if (!this.hideIfEmpty)
		{
			return;
		}
		if (this.Inventory.IsEmpty())
		{
			this.hideIfEmpty.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0001FF02 File Offset: 0x0001E102
	private bool StartLoot()
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			Debug.LogError("开始loot失败，缺少inventory。");
			return false;
		}
		Action<InteractableLootbox> onStartLoot = InteractableLootbox.OnStartLoot;
		if (onStartLoot != null)
		{
			onStartLoot(this);
		}
		return true;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0001FF38 File Offset: 0x0001E138
	private void CreateLocalInventory()
	{
		Inventory inventory = base.gameObject.AddComponent<Inventory>();
		this.inventoryReference = inventory;
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001FF58 File Offset: 0x0001E158
	public static InteractableLootbox Prefab
	{
		get
		{
			GameplayDataSettings.PrefabsData prefabs = GameplayDataSettings.Prefabs;
			if (prefabs == null)
			{
				return null;
			}
			return prefabs.LootBoxPrefab;
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0001FF6C File Offset: 0x0001E16C
	public static InteractableLootbox CreateFromItem(Item item, Vector3 position, Quaternion rotation, bool moveToMainScene = true, InteractableLootbox prefab = null, bool filterDontDropOnDead = false)
	{
		if (item == null)
		{
			Debug.LogError("正在尝试给一个不存在的Item创建LootBox，已取消。");
			return null;
		}
		if (prefab == null)
		{
			prefab = InteractableLootbox.Prefab;
		}
		if (prefab == null)
		{
			Debug.LogError("未配置LootBox的Prefab");
			return null;
		}
		InteractableLootbox interactableLootbox = UnityEngine.Object.Instantiate<InteractableLootbox>(prefab, position, rotation);
		interactableLootbox.CreateLocalInventory();
		if (moveToMainScene)
		{
			MultiSceneCore.MoveToActiveWithScene(interactableLootbox.gameObject, SceneManager.GetActiveScene().buildIndex);
		}
		Inventory inventory = interactableLootbox.Inventory;
		if (inventory == null)
		{
			Debug.LogError("LootBox未配置Inventory");
			return interactableLootbox;
		}
		inventory.SetCapacity(512);
		List<Item> list = new List<Item>();
		if (item.Slots != null)
		{
			foreach (Slot slot in item.Slots)
			{
				Item content = slot.Content;
				if (!(content == null))
				{
					content.Inspected = true;
					if (content.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
					{
						content.DestroyTree();
					}
					if (!filterDontDropOnDead || (!content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot) && !content.Sticky))
					{
						list.Add(content);
					}
				}
			}
		}
		if (item.Inventory != null)
		{
			foreach (Item item2 in item.Inventory)
			{
				if (!(item2 == null) && !item2.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
				{
					list.Add(item2);
				}
			}
		}
		foreach (Item item3 in list)
		{
			item3.Detach();
			inventory.AddAndMerge(item3, 0);
		}
		int capacity = Mathf.Max(8, inventory.GetLastItemPosition() + 1);
		inventory.SetCapacity(capacity);
		inventory.NeedInspection = prefab.needInspect;
		return interactableLootbox;
	}

	// Token: 0x040006B1 RID: 1713
	public bool useDefaultInteractName;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private bool showSortButton;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	private bool usePages;

	// Token: 0x040006B4 RID: 1716
	public bool needInspect = true;

	// Token: 0x040006B5 RID: 1717
	public bool showPickAllButton = true;

	// Token: 0x040006B6 RID: 1718
	public Transform hideIfEmpty;

	// Token: 0x040006B7 RID: 1719
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayNameKey;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	private Inventory inventoryReference;

	// Token: 0x040006B9 RID: 1721
	private Item inspectingItem;

	// Token: 0x040006BA RID: 1722
	private float inspectTime = 1f;

	// Token: 0x040006BB RID: 1723
	private float inspectTimer;

	// Token: 0x040006BC RID: 1724
	private InteractableLootbox.LootBoxStates lootState;

	// Token: 0x0200046B RID: 1131
	public enum LootBoxStates
	{
		// Token: 0x04001B65 RID: 7013
		closed,
		// Token: 0x04001B66 RID: 7014
		openning,
		// Token: 0x04001B67 RID: 7015
		looting
	}
}
