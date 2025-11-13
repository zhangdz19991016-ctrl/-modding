using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.MasterKeys;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public static class ItemUtilities
{
	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x0002492B File Offset: 0x00022B2B
	private static Item CharacterItem
	{
		get
		{
			LevelManager instance = LevelManager.Instance;
			if (instance == null)
			{
				return null;
			}
			CharacterMainControl mainCharacter = instance.MainCharacter;
			if (mainCharacter == null)
			{
				return null;
			}
			return mainCharacter.CharacterItem;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000826 RID: 2086 RVA: 0x00024948 File Offset: 0x00022B48
	private static Inventory CharacterInventory
	{
		get
		{
			Item characterItem = ItemUtilities.CharacterItem;
			if (characterItem == null)
			{
				return null;
			}
			return characterItem.Inventory;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002495A File Offset: 0x00022B5A
	private static Inventory PlayerStorageInventory
	{
		get
		{
			return PlayerStorage.Inventory;
		}
	}

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000828 RID: 2088 RVA: 0x00024964 File Offset: 0x00022B64
	// (remove) Token: 0x06000829 RID: 2089 RVA: 0x00024998 File Offset: 0x00022B98
	public static event Action OnPlayerItemOperation;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x0600082A RID: 2090 RVA: 0x000249CC File Offset: 0x00022BCC
	// (remove) Token: 0x0600082B RID: 2091 RVA: 0x00024A00 File Offset: 0x00022C00
	public static event Action<Item> OnItemSentToPlayerInventory;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x0600082C RID: 2092 RVA: 0x00024A34 File Offset: 0x00022C34
	// (remove) Token: 0x0600082D RID: 2093 RVA: 0x00024A68 File Offset: 0x00022C68
	public static event Action<Item> OnItemSentToPlayerStorage;

	// Token: 0x0600082E RID: 2094 RVA: 0x00024A9C File Offset: 0x00022C9C
	public static UniTask<bool> Decompose(Item item, int count)
	{
		ItemUtilities.<Decompose>d__15 <Decompose>d__;
		<Decompose>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Decompose>d__.item = item;
		<Decompose>d__.count = count;
		<Decompose>d__.<>1__state = -1;
		<Decompose>d__.<>t__builder.Start<ItemUtilities.<Decompose>d__15>(ref <Decompose>d__);
		return <Decompose>d__.<>t__builder.Task;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00024AE8 File Offset: 0x00022CE8
	public static UniTask<Item> GenerateBullet(Item gunItem)
	{
		ItemUtilities.<GenerateBullet>d__16 <GenerateBullet>d__;
		<GenerateBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<GenerateBullet>d__.gunItem = gunItem;
		<GenerateBullet>d__.<>1__state = -1;
		<GenerateBullet>d__.<>t__builder.Start<ItemUtilities.<GenerateBullet>d__16>(ref <GenerateBullet>d__);
		return <GenerateBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00024B2C File Offset: 0x00022D2C
	public static List<Item> FindAllBelongsToPlayer(Predicate<Item> predicate)
	{
		List<Item> list = new List<Item>();
		Inventory playerStorageInventory = ItemUtilities.PlayerStorageInventory;
		if (playerStorageInventory != null)
		{
			List<Item> collection = playerStorageInventory.FindAll(predicate);
			list.AddRange(collection);
		}
		Inventory characterInventory = ItemUtilities.CharacterInventory;
		if (characterInventory != null)
		{
			List<Item> collection2 = characterInventory.FindAll(predicate);
			list.AddRange(collection2);
		}
		LevelManager instance = LevelManager.Instance;
		Inventory inventory;
		if (instance == null)
		{
			inventory = null;
		}
		else
		{
			PetProxy petProxy = instance.PetProxy;
			inventory = ((petProxy != null) ? petProxy.Inventory : null);
		}
		Inventory inventory2 = inventory;
		if (inventory2 != null)
		{
			List<Item> collection3 = inventory2.FindAll(predicate);
			list.AddRange(collection3);
		}
		return list;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00024BB8 File Offset: 0x00022DB8
	public static int GetItemCount(int typeID)
	{
		List<Item> list = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == typeID);
		int num = 0;
		foreach (Item item in list)
		{
			num += item.StackCount;
		}
		return num;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00024C28 File Offset: 0x00022E28
	public static bool AddAndMerge(this Inventory inventory, Item item, int preferedFirstPosition = 0)
	{
		if (inventory == null)
		{
			return false;
		}
		if (item.Stackable)
		{
			Func<Item, bool> <>9__0;
			while (item.StackCount > 0)
			{
				Func<Item, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((Item e) => e.TypeID == item.TypeID && e.MaxStackCount > e.StackCount));
				}
				Item item2 = inventory.FirstOrDefault(predicate);
				if (item2 == null)
				{
					break;
				}
				item2.Combine(item);
			}
			if (item.StackCount <= 0)
			{
				return true;
			}
		}
		int firstEmptyPosition = inventory.GetFirstEmptyPosition(preferedFirstPosition);
		if (firstEmptyPosition < 0)
		{
			return false;
		}
		item.Detach();
		inventory.AddAt(item, firstEmptyPosition);
		return true;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00024CE0 File Offset: 0x00022EE0
	public static bool SendToPlayerCharacter(Item item, bool dontMerge = false)
	{
		if (item == null)
		{
			return false;
		}
		LevelManager instance = LevelManager.Instance;
		Item item2;
		if (instance == null)
		{
			item2 = null;
		}
		else
		{
			CharacterMainControl mainCharacter = instance.MainCharacter;
			item2 = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
		}
		Item item3 = item2;
		if (item3 == null)
		{
			return false;
		}
		if (item3.TryPlug(item, true, null, 0))
		{
			Action onPlayerItemOperation = ItemUtilities.OnPlayerItemOperation;
			if (onPlayerItemOperation != null)
			{
				onPlayerItemOperation();
			}
			return true;
		}
		return ItemUtilities.SendToPlayerCharacterInventory(item, dontMerge);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00024D46 File Offset: 0x00022F46
	public static void SendToPlayer(Item item, bool dontMerge = false, bool sendToStorage = true)
	{
		if (ItemUtilities.SendToPlayerCharacter(item, dontMerge))
		{
			return;
		}
		if (sendToStorage)
		{
			ItemUtilities.SendToPlayerStorage(item, false);
			return;
		}
		item.Drop(CharacterMainControl.Main, true);
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00024D6C File Offset: 0x00022F6C
	public static bool SendToPlayerCharacterInventory(Item item, bool dontMerge = false)
	{
		if (item == null)
		{
			return false;
		}
		LevelManager instance = LevelManager.Instance;
		Inventory inventory;
		if (instance == null)
		{
			inventory = null;
		}
		else
		{
			CharacterMainControl mainCharacter = instance.MainCharacter;
			if (mainCharacter == null)
			{
				inventory = null;
			}
			else
			{
				Item characterItem = mainCharacter.CharacterItem;
				inventory = ((characterItem != null) ? characterItem.Inventory : null);
			}
		}
		Inventory inventory2 = inventory;
		if (inventory2 == null)
		{
			return false;
		}
		int preferedFirstPosition = 0;
		bool flag;
		if (dontMerge)
		{
			flag = inventory2.AddItem(item);
		}
		else
		{
			flag = inventory2.AddAndMerge(item, preferedFirstPosition);
		}
		if (!flag)
		{
			return false;
		}
		Action onPlayerItemOperation = ItemUtilities.OnPlayerItemOperation;
		if (onPlayerItemOperation != null)
		{
			onPlayerItemOperation();
		}
		Action<Item> onItemSentToPlayerInventory = ItemUtilities.OnItemSentToPlayerInventory;
		if (onItemSentToPlayerInventory != null)
		{
			onItemSentToPlayerInventory(item);
		}
		return true;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00024DFA File Offset: 0x00022FFA
	public static void SendToPlayerStorage(Item item, bool directToBuffer = false)
	{
		item.Detach();
		PlayerStorage.Push(item, directToBuffer);
		Action<Item> onItemSentToPlayerStorage = ItemUtilities.OnItemSentToPlayerStorage;
		if (onItemSentToPlayerStorage == null)
		{
			return;
		}
		onItemSentToPlayerStorage(item);
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00024E1C File Offset: 0x0002301C
	public static bool IsInPlayerCharacter(this Item item)
	{
		ItemUtilities.<>c__DisplayClass25_0 CS$<>8__locals1 = new ItemUtilities.<>c__DisplayClass25_0();
		ItemUtilities.<>c__DisplayClass25_0 CS$<>8__locals2 = CS$<>8__locals1;
		LevelManager instance = LevelManager.Instance;
		Item characterItem;
		if (instance == null)
		{
			characterItem = null;
		}
		else
		{
			CharacterMainControl mainCharacter = instance.MainCharacter;
			characterItem = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
		}
		CS$<>8__locals2.characterItem = characterItem;
		return !(CS$<>8__locals1.characterItem == null) && item.GetAllParents(false).Any((Item e) => e == CS$<>8__locals1.characterItem);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00024E7C File Offset: 0x0002307C
	public static bool IsInPlayerStorage(this Item item)
	{
		Inventory playerStorageInventory = PlayerStorage.Inventory;
		return !(playerStorageInventory == null) && item.GetAllParents(false).Any((Item e) => e.InInventory == playerStorageInventory);
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00024EC2 File Offset: 0x000230C2
	public static bool IsRegistered(this Item item)
	{
		return !(item == null) && (MasterKeysManager.IsActive(item.TypeID) || CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(item)));
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00024EF0 File Offset: 0x000230F0
	public static bool TryPlug(this Item main, Item part, bool emptyOnly = false, Inventory backupInventory = null, int preferredFirstIndex = 0)
	{
		if (main == null)
		{
			return false;
		}
		if (part == null)
		{
			return false;
		}
		if (main.Slots == null)
		{
			return false;
		}
		bool flag = main.IsInPlayerCharacter();
		bool flag2 = part.IsInPlayerCharacter();
		bool flag3 = main.IsInPlayerStorage();
		bool flag4 = part.IsInPlayerStorage();
		bool flag5 = flag || flag2 || flag3 || flag4;
		Slot slot = null;
		Slot pluggedIntoSlot = part.PluggedIntoSlot;
		if (backupInventory == null)
		{
			if (part.InInventory)
			{
				backupInventory = part.InInventory;
			}
			else if (main.InInventory)
			{
				backupInventory = main.InInventory;
			}
			else if (part.PluggedIntoSlot != null)
			{
				Item characterItem = part.GetCharacterItem();
				if (characterItem != null)
				{
					backupInventory = characterItem.Inventory;
				}
			}
			if (backupInventory == null)
			{
				Item characterItem2 = main.GetCharacterItem();
				if (characterItem2 != null)
				{
					backupInventory = characterItem2.Inventory;
				}
			}
		}
		IEnumerable<Slot> enumerable = from e in main.Slots
		where e != null && e.CanPlug(part)
		select e;
		if (part.PluggedIntoSlot != null)
		{
			foreach (Slot slot2 in enumerable)
			{
				if (part.PluggedIntoSlot == slot2)
				{
					Debug.Log("什么也没做，因为已经在这个物体上了。");
					return false;
				}
			}
		}
		if (part.Stackable)
		{
			foreach (Slot slot3 in enumerable)
			{
				Item content = slot3.Content;
				if (!(content == null) && content.TypeID == part.TypeID)
				{
					content.Combine(part);
					if (part.StackCount <= 0)
					{
						return true;
					}
				}
			}
		}
		Slot slot4 = enumerable.FirstOrDefault((Slot e) => e.Content == null);
		if (slot4 != null)
		{
			slot = slot4;
		}
		else if (!emptyOnly)
		{
			slot = enumerable.FirstOrDefault<Slot>();
		}
		if (slot == null)
		{
			return false;
		}
		Item item;
		slot.Plug(part, out item);
		if (item != null)
		{
			bool flag6 = false;
			if (pluggedIntoSlot != null && pluggedIntoSlot.Content == null)
			{
				Item item2;
				flag6 = pluggedIntoSlot.Plug(item, out item2);
			}
			if (!flag6 && backupInventory != null)
			{
				flag6 = backupInventory.AddAndMerge(item, preferredFirstIndex);
			}
			if (!flag6)
			{
				if (flag5)
				{
					item.Drop(CharacterMainControl.Main, true);
				}
				else
				{
					item.Drop(Vector3.down * 1000f, false, Vector3.up, 0f);
				}
			}
		}
		return true;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000251EC File Offset: 0x000233EC
	public static CharacterMainControl GetCharacterMainControl(this Item item)
	{
		Item root = item.GetRoot();
		if (root == null)
		{
			return null;
		}
		return root.GetComponentInParent<CharacterMainControl>();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00025200 File Offset: 0x00023400
	internal static IEnumerable<Inventory> GetPlayerInventories()
	{
		HashSet<Inventory> hashSet = new HashSet<Inventory>();
		LevelManager instance = LevelManager.Instance;
		Inventory inventory;
		if (instance == null)
		{
			inventory = null;
		}
		else
		{
			CharacterMainControl mainCharacter = instance.MainCharacter;
			if (mainCharacter == null)
			{
				inventory = null;
			}
			else
			{
				Item characterItem = mainCharacter.CharacterItem;
				inventory = ((characterItem != null) ? characterItem.Inventory : null);
			}
		}
		Inventory inventory2 = inventory;
		if (inventory2)
		{
			hashSet.Add(inventory2);
		}
		if (PlayerStorage.Inventory != null)
		{
			hashSet.Add(PlayerStorage.Inventory);
		}
		return hashSet;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00025268 File Offset: 0x00023468
	internal static bool ConsumeItems(Cost cost)
	{
		ItemUtilities.<>c__DisplayClass31_0 CS$<>8__locals1 = new ItemUtilities.<>c__DisplayClass31_0();
		List<Action> list = new List<Action>();
		CS$<>8__locals1.detachedItems = new List<Item>();
		if (cost.items != null)
		{
			Cost.ItemEntry[] items2 = cost.items;
			for (int i = 0; i < items2.Length; i++)
			{
				ItemUtilities.<>c__DisplayClass31_1 CS$<>8__locals2 = new ItemUtilities.<>c__DisplayClass31_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.cur = items2[i];
				List<Item> items = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == CS$<>8__locals2.cur.id);
				items.Sort(ItemUtilities.ItemDurabilityComparer.Instance);
				int count = ItemUtilities.Count(items);
				if ((long)count < CS$<>8__locals2.cur.amount)
				{
					return false;
				}
				list.Add(delegate
				{
					long num = CS$<>8__locals2.cur.amount;
					for (int j = 0; j < count; j++)
					{
						Item item2 = items[j];
						if (!(item2 == null))
						{
							if (item2.Slots != null)
							{
								foreach (Slot slot in item2.Slots)
								{
									if (slot != null)
									{
										Item content = slot.Content;
										if (!(content == null))
										{
											content.Detach();
											CS$<>8__locals2.CS$<>8__locals1.detachedItems.Add(content);
										}
									}
								}
							}
							if ((long)item2.StackCount <= num)
							{
								num -= (long)item2.StackCount;
								item2.Detach();
								item2.DestroyTree();
							}
							else
							{
								item2.StackCount -= (int)num;
								num = 0L;
							}
							if (num <= 0L)
							{
								break;
							}
						}
					}
				});
			}
		}
		foreach (Action action in list)
		{
			action();
		}
		foreach (Item item in CS$<>8__locals1.detachedItems)
		{
			if (!(item == null))
			{
				ItemUtilities.SendToPlayer(item, false, PlayerStorage.Inventory != null);
			}
		}
		Action onPlayerItemOperation = ItemUtilities.OnPlayerItemOperation;
		if (onPlayerItemOperation != null)
		{
			onPlayerItemOperation();
		}
		return true;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x000253FC File Offset: 0x000235FC
	internal static bool ConsumeItems(int itemTypeID, long amount)
	{
		List<Item> list = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == itemTypeID);
		if ((long)ItemUtilities.Count(list) < amount)
		{
			return false;
		}
		List<Item> list2 = new List<Item>();
		long num = amount;
		for (int i = 0; i < list.Count; i++)
		{
			Item item = list[i];
			if (!(item == null))
			{
				if (item.Slots != null)
				{
					foreach (Slot slot in item.Slots)
					{
						if (slot != null)
						{
							Item content = slot.Content;
							if (!(content == null))
							{
								content.Detach();
								list2.Add(content);
							}
						}
					}
				}
				if ((long)item.StackCount <= num)
				{
					num -= (long)item.StackCount;
					item.Detach();
					item.DestroyTree();
				}
				else
				{
					item.StackCount -= (int)num;
					num = 0L;
				}
				if (num <= 0L)
				{
					break;
				}
			}
		}
		foreach (Item item2 in list2)
		{
			if (!(item2 == null))
			{
				ItemUtilities.SendToPlayer(item2, false, PlayerStorage.Inventory != null);
			}
		}
		return true;
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00025570 File Offset: 0x00023770
	internal static int Count(IEnumerable<Item> items)
	{
		int num = 0;
		foreach (Item item in items)
		{
			if (item.Stackable)
			{
				num += item.StackCount;
			}
			else
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000255CC File Offset: 0x000237CC
	public static float GetRepairLossRatio(this Item item)
	{
		if (item == null)
		{
			return 0f;
		}
		float defaultResult = 0.14f;
		float num = item.Constants.GetFloat("RepairLossRatio", defaultResult);
		if (item.Tags.Contains("Weapon"))
		{
			float num2 = CharacterMainControl.WeaponRepairLossFactor();
			num *= num2;
		}
		else
		{
			float num3 = CharacterMainControl.EquipmentRepairLossFactor();
			num *= num3;
		}
		return num;
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00025629 File Offset: 0x00023829
	internal static void NotifyPlayerItemOperation()
	{
		Action onPlayerItemOperation = ItemUtilities.OnPlayerItemOperation;
		if (onPlayerItemOperation == null)
		{
			return;
		}
		onPlayerItemOperation();
	}

	// Token: 0x02000477 RID: 1143
	private class ItemDurabilityComparer : IComparer<Item>
	{
		// Token: 0x060026CF RID: 9935 RVA: 0x0008987C File Offset: 0x00087A7C
		public int Compare(Item x, Item y)
		{
			int result = 0;
			if (!x.UseDurability)
			{
				return 0;
			}
			if (!y.UseDurability)
			{
				return 0;
			}
			float num = x.Durability - y.Durability;
			if (num > 0f)
			{
				result = 1;
			}
			if (num < 0f)
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x04001B9A RID: 7066
		public static readonly ItemUtilities.ItemDurabilityComparer Instance = new ItemUtilities.ItemDurabilityComparer();
	}
}
