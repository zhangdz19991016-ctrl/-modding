using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class PlayerStorage : MonoBehaviour, IInitializedQueryHandler
{
	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x0002563A File Offset: 0x0002383A
	// (set) Token: 0x06000843 RID: 2115 RVA: 0x00025641 File Offset: 0x00023841
	public static PlayerStorage Instance { get; private set; }

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06000844 RID: 2116 RVA: 0x0002564C File Offset: 0x0002384C
	// (remove) Token: 0x06000845 RID: 2117 RVA: 0x00025680 File Offset: 0x00023880
	public static event Action<PlayerStorage, Inventory, int> OnPlayerStorageChange;

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x000256B3 File Offset: 0x000238B3
	public static Inventory Inventory
	{
		get
		{
			if (PlayerStorage.Instance == null)
			{
				return null;
			}
			return PlayerStorage.Instance.inventory;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x000256CE File Offset: 0x000238CE
	public static List<ItemTreeData> IncomingItemBuffer
	{
		get
		{
			return PlayerStorageBuffer.Buffer;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x000256D5 File Offset: 0x000238D5
	public InteractableLootbox InteractableLootBox
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000849 RID: 2121 RVA: 0x000256E0 File Offset: 0x000238E0
	// (remove) Token: 0x0600084A RID: 2122 RVA: 0x00025714 File Offset: 0x00023914
	public static event Action<PlayerStorage.StorageCapacityCalculationHolder> OnRecalculateStorageCapacity;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600084B RID: 2123 RVA: 0x00025748 File Offset: 0x00023948
	// (remove) Token: 0x0600084C RID: 2124 RVA: 0x0002577C File Offset: 0x0002397C
	public static event Action OnTakeBufferItem;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600084D RID: 2125 RVA: 0x000257B0 File Offset: 0x000239B0
	// (remove) Token: 0x0600084E RID: 2126 RVA: 0x000257E4 File Offset: 0x000239E4
	public static event Action<Item> OnItemAddedToBuffer;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x0600084F RID: 2127 RVA: 0x00025818 File Offset: 0x00023A18
	// (remove) Token: 0x06000850 RID: 2128 RVA: 0x0002584C File Offset: 0x00023A4C
	public static event Action OnLoadingFinished;

	// Token: 0x06000851 RID: 2129 RVA: 0x0002587F File Offset: 0x00023A7F
	public static bool IsAccessableAndNotFull()
	{
		return !(PlayerStorage.Instance == null) && !(PlayerStorage.Inventory == null) && PlayerStorage.Inventory.GetFirstEmptyPosition(0) >= 0;
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x000258B0 File Offset: 0x00023AB0
	public int DefaultCapacity
	{
		get
		{
			return this.defaultCapacity;
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x000258B8 File Offset: 0x00023AB8
	public static void NotifyCapacityDirty()
	{
		PlayerStorage.needRecalculateCapacity = true;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000258C0 File Offset: 0x00023AC0
	private void Awake()
	{
		if (PlayerStorage.Instance == null)
		{
			PlayerStorage.Instance = this;
		}
		if (PlayerStorage.Instance != this)
		{
			Debug.LogError("发现了多个Player Storage!");
			return;
		}
		if (this.interactable == null)
		{
			this.interactable = base.GetComponent<InteractableLootbox>();
		}
		this.inventory.onContentChanged += this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData += this.SavesSystem_OnCollectSaveData;
		LevelManager.RegisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00025940 File Offset: 0x00023B40
	private void Start()
	{
		this.Load().Forget();
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002594D File Offset: 0x00023B4D
	private void OnDestroy()
	{
		this.inventory.onContentChanged -= this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData -= this.SavesSystem_OnCollectSaveData;
		LevelManager.UnregisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002597E File Offset: 0x00023B7E
	private void SavesSystem_OnSetFile()
	{
		this.Load().Forget();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002598B File Offset: 0x00023B8B
	private void SavesSystem_OnCollectSaveData()
	{
		this.Save();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00025993 File Offset: 0x00023B93
	private void OnInventoryContentChanged(Inventory inventory, int index)
	{
		Action<PlayerStorage, Inventory, int> onPlayerStorageChange = PlayerStorage.OnPlayerStorageChange;
		if (onPlayerStorageChange == null)
		{
			return;
		}
		onPlayerStorageChange(this, inventory, index);
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x000259A8 File Offset: 0x00023BA8
	public static void Push(Item item, bool toBufferDirectly = false)
	{
		if (item == null)
		{
			return;
		}
		if (!toBufferDirectly && PlayerStorage.Inventory != null)
		{
			if (item.Stackable)
			{
				Func<Item, bool> <>9__0;
				while (item.StackCount > 0)
				{
					IEnumerable<Item> source = PlayerStorage.Inventory;
					Func<Item, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Item e) => e.TypeID == item.TypeID && e.MaxStackCount > e.StackCount));
					}
					Item item2 = source.FirstOrDefault(predicate);
					if (item2 == null)
					{
						break;
					}
					item2.Combine(item);
				}
			}
			if (item != null && item.StackCount > 0)
			{
				int firstEmptyPosition = PlayerStorage.Inventory.GetFirstEmptyPosition(0);
				if (firstEmptyPosition >= 0)
				{
					PlayerStorage.Inventory.AddAt(item, firstEmptyPosition);
					return;
				}
			}
		}
		NotificationText.Push("PlayerStorage_Notification_ItemAddedToBuffer".ToPlainText().Format(new
		{
			displayName = item.DisplayName
		}));
		PlayerStorage.IncomingItemBuffer.Add(ItemTreeData.FromItem(item));
		item.Detach();
		item.DestroyTree();
		Action<Item> onItemAddedToBuffer = PlayerStorage.OnItemAddedToBuffer;
		if (onItemAddedToBuffer == null)
		{
			return;
		}
		onItemAddedToBuffer(item);
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00025AE6 File Offset: 0x00023CE6
	private void Save()
	{
		if (PlayerStorage.Loading)
		{
			return;
		}
		this.inventory.Save("PlayerStorage");
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600085C RID: 2140 RVA: 0x00025B00 File Offset: 0x00023D00
	// (set) Token: 0x0600085D RID: 2141 RVA: 0x00025B07 File Offset: 0x00023D07
	public static bool Loading { get; private set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x00025B0F File Offset: 0x00023D0F
	// (set) Token: 0x0600085F RID: 2143 RVA: 0x00025B16 File Offset: 0x00023D16
	public static bool TakingItem { get; private set; }

	// Token: 0x06000860 RID: 2144 RVA: 0x00025B20 File Offset: 0x00023D20
	private UniTask Load()
	{
		PlayerStorage.<Load>d__52 <Load>d__;
		<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Load>d__.<>4__this = this;
		<Load>d__.<>1__state = -1;
		<Load>d__.<>t__builder.Start<PlayerStorage.<Load>d__52>(ref <Load>d__);
		return <Load>d__.<>t__builder.Task;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00025B63 File Offset: 0x00023D63
	private void Update()
	{
		if (PlayerStorage.needRecalculateCapacity)
		{
			PlayerStorage.RecalculateStorageCapacity();
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00025B74 File Offset: 0x00023D74
	public static int RecalculateStorageCapacity()
	{
		if (PlayerStorage.Instance == null)
		{
			return 0;
		}
		PlayerStorage.StorageCapacityCalculationHolder storageCapacityCalculationHolder = new PlayerStorage.StorageCapacityCalculationHolder();
		storageCapacityCalculationHolder.capacity = PlayerStorage.Instance.DefaultCapacity;
		Action<PlayerStorage.StorageCapacityCalculationHolder> onRecalculateStorageCapacity = PlayerStorage.OnRecalculateStorageCapacity;
		if (onRecalculateStorageCapacity != null)
		{
			onRecalculateStorageCapacity(storageCapacityCalculationHolder);
		}
		int capacity = storageCapacityCalculationHolder.capacity;
		PlayerStorage.Instance.SetCapacity(capacity);
		PlayerStorage.needRecalculateCapacity = false;
		return capacity;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00025BD0 File Offset: 0x00023DD0
	private void SetCapacity(int capacity)
	{
		this.inventory.SetCapacity(capacity);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00025BE0 File Offset: 0x00023DE0
	public static UniTask TakeBufferItem(int index)
	{
		PlayerStorage.<TakeBufferItem>d__56 <TakeBufferItem>d__;
		<TakeBufferItem>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<TakeBufferItem>d__.index = index;
		<TakeBufferItem>d__.<>1__state = -1;
		<TakeBufferItem>d__.<>t__builder.Start<PlayerStorage.<TakeBufferItem>d__56>(ref <TakeBufferItem>d__);
		return <TakeBufferItem>d__.<>t__builder.Task;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00025C23 File Offset: 0x00023E23
	public bool HasInitialized()
	{
		return this.initialized;
	}

	// Token: 0x04000798 RID: 1944
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000799 RID: 1945
	[SerializeField]
	private InteractableLootbox interactable;

	// Token: 0x0400079E RID: 1950
	[SerializeField]
	private int defaultCapacity = 32;

	// Token: 0x0400079F RID: 1951
	private static bool needRecalculateCapacity;

	// Token: 0x040007A0 RID: 1952
	private const string inventorySaveKey = "PlayerStorage";

	// Token: 0x040007A3 RID: 1955
	private bool initialized;

	// Token: 0x02000484 RID: 1156
	public class StorageCapacityCalculationHolder
	{
		// Token: 0x04001BB3 RID: 7091
		public int capacity;
	}
}
