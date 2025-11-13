using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;

namespace Saves
{
	// Token: 0x02000227 RID: 551
	public static class ItemSavesUtilities
	{
		// Token: 0x0600109F RID: 4255 RVA: 0x00040D94 File Offset: 0x0003EF94
		public static void SaveAsLastDeadCharacter(Item item)
		{
			uint num = SavesSystem.Load<uint>("DeadCharacterToken");
			uint num2 = num;
			do
			{
				num2 += 1U;
			}
			while (num2 == num);
			SavesSystem.Save<uint>("DeadCharacterToken", num2);
			item.Save("LastDeadCharacter");
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00040DCC File Offset: 0x0003EFCC
		public static UniTask<Item> LoadLastDeadCharacterItem()
		{
			ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3 <LoadLastDeadCharacterItem>d__;
			<LoadLastDeadCharacterItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadLastDeadCharacterItem>d__.<>1__state = -1;
			<LoadLastDeadCharacterItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadLastDeadCharacterItem>d__3>(ref <LoadLastDeadCharacterItem>d__);
			return <LoadLastDeadCharacterItem>d__.<>t__builder.Task;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00040E08 File Offset: 0x0003F008
		public static void Save(this Item item, string key)
		{
			ItemTreeData value = ItemTreeData.FromItem(item);
			SavesSystem.Save<ItemTreeData>("Item/", key, value);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00040E28 File Offset: 0x0003F028
		public static void Save(this Inventory inventory, string key)
		{
			InventoryData value = InventoryData.FromInventory(inventory);
			SavesSystem.Save<InventoryData>("Inventory/", key, value);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00040E48 File Offset: 0x0003F048
		public static UniTask<Item> LoadItem(string key)
		{
			ItemSavesUtilities.<LoadItem>d__6 <LoadItem>d__;
			<LoadItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<LoadItem>d__.key = key;
			<LoadItem>d__.<>1__state = -1;
			<LoadItem>d__.<>t__builder.Start<ItemSavesUtilities.<LoadItem>d__6>(ref <LoadItem>d__);
			return <LoadItem>d__.<>t__builder.Task;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00040E8C File Offset: 0x0003F08C
		public static UniTask LoadInventory(string key, Inventory inventoryInstance)
		{
			ItemSavesUtilities.<LoadInventory>d__7 <LoadInventory>d__;
			<LoadInventory>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadInventory>d__.key = key;
			<LoadInventory>d__.inventoryInstance = inventoryInstance;
			<LoadInventory>d__.<>1__state = -1;
			<LoadInventory>d__.<>t__builder.Start<ItemSavesUtilities.<LoadInventory>d__7>(ref <LoadInventory>d__);
			return <LoadInventory>d__.<>t__builder.Task;
		}

		// Token: 0x04000D44 RID: 3396
		private const string InventoryPrefix = "Inventory/";

		// Token: 0x04000D45 RID: 3397
		private const string ItemPrefix = "Item/";
	}
}
