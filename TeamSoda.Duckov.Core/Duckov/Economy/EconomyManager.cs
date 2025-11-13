using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x02000329 RID: 809
	public class EconomyManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x00062698 File Offset: 0x00060898
		public static string ItemUnlockNotificationTextMainFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextMainFormat;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x000626AA File Offset: 0x000608AA
		public static string ItemUnlockNotificationTextSubFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextSubFormat;
			}
		}

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001B1F RID: 6943 RVA: 0x000626BC File Offset: 0x000608BC
		// (remove) Token: 0x06001B20 RID: 6944 RVA: 0x000626F0 File Offset: 0x000608F0
		public static event Action OnEconomyManagerLoaded;

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00062723 File Offset: 0x00060923
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x0006272A File Offset: 0x0006092A
		public static EconomyManager Instance { get; private set; }

		// Token: 0x06001B23 RID: 6947 RVA: 0x00062732 File Offset: 0x00060932
		private void Awake()
		{
			if (EconomyManager.Instance == null)
			{
				EconomyManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			this.Load();
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0006276F File Offset: 0x0006096F
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00062777 File Offset: 0x00060977
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00062780 File Offset: 0x00060980
		private void Load()
		{
			if (SavesSystem.KeyExisits("EconomyData"))
			{
				this.SetupSaveData(SavesSystem.Load<EconomyManager.SaveData>("EconomyData"));
			}
			try
			{
				Action onEconomyManagerLoaded = EconomyManager.OnEconomyManagerLoaded;
				if (onEconomyManagerLoaded != null)
				{
					onEconomyManagerLoaded();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x000627D8 File Offset: 0x000609D8
		private void Save()
		{
			SavesSystem.Save<EconomyManager.SaveData>("EconomyData", (EconomyManager.SaveData)this.GenerateSaveData());
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000627EF File Offset: 0x000609EF
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x00062813 File Offset: 0x00060A13
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x00062830 File Offset: 0x00060A30
		public static long Money
		{
			get
			{
				if (EconomyManager.Instance == null)
				{
					return 0L;
				}
				return EconomyManager.Instance.money;
			}
			private set
			{
				long arg = EconomyManager.Money;
				if (EconomyManager.Instance == null)
				{
					return;
				}
				EconomyManager.Instance.money = value;
				Action<long, long> onMoneyChanged = EconomyManager.OnMoneyChanged;
				if (onMoneyChanged == null)
				{
					return;
				}
				onMoneyChanged(arg, value);
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0006286D File Offset: 0x00060A6D
		public static long Cash
		{
			get
			{
				return (long)ItemUtilities.GetItemCount(451);
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x0006287A File Offset: 0x00060A7A
		public ReadOnlyCollection<int> UnlockedItemIds
		{
			get
			{
				return this.unlockedItemIds.AsReadOnly();
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001B2D RID: 6957 RVA: 0x00062888 File Offset: 0x00060A88
		// (remove) Token: 0x06001B2E RID: 6958 RVA: 0x000628BC File Offset: 0x00060ABC
		public static event Action<long, long> OnMoneyChanged;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06001B2F RID: 6959 RVA: 0x000628F0 File Offset: 0x00060AF0
		// (remove) Token: 0x06001B30 RID: 6960 RVA: 0x00062924 File Offset: 0x00060B24
		public static event Action<int> OnItemUnlockStateChanged;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06001B31 RID: 6961 RVA: 0x00062958 File Offset: 0x00060B58
		// (remove) Token: 0x06001B32 RID: 6962 RVA: 0x0006298C File Offset: 0x00060B8C
		public static event Action<long> OnMoneyPaid;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06001B33 RID: 6963 RVA: 0x000629C0 File Offset: 0x00060BC0
		// (remove) Token: 0x06001B34 RID: 6964 RVA: 0x000629F4 File Offset: 0x00060BF4
		public static event Action<Cost> OnCostPaid;

		// Token: 0x06001B35 RID: 6965 RVA: 0x00062A28 File Offset: 0x00060C28
		private static bool Pay(long amount, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < amount)
			{
				return false;
			}
			long num3 = amount;
			if (accountAvaliable)
			{
				if (num > amount)
				{
					num3 = 0L;
					EconomyManager.Money -= amount;
				}
				else
				{
					num3 -= num;
					EconomyManager.Money = 0L;
				}
			}
			if (cashAvaliale && num3 > 0L)
			{
				ItemUtilities.ConsumeItems(451, num3);
			}
			if (amount > 0L)
			{
				Action<long> onMoneyPaid = EconomyManager.OnMoneyPaid;
				if (onMoneyPaid != null)
				{
					onMoneyPaid(amount);
				}
			}
			return true;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00062AAA File Offset: 0x00060CAA
		public static bool Pay(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			if (!EconomyManager.IsEnough(cost, accountAvaliable, true))
			{
				return false;
			}
			if (!EconomyManager.Pay(cost.money, accountAvaliable, cashAvaliale))
			{
				return false;
			}
			if (!ItemUtilities.ConsumeItems(cost))
			{
				return false;
			}
			Action<Cost> onCostPaid = EconomyManager.OnCostPaid;
			if (onCostPaid != null)
			{
				onCostPaid(cost);
			}
			return true;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00062AE8 File Offset: 0x00060CE8
		public static bool IsEnough(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < cost.money)
			{
				return false;
			}
			if (cost.items != null)
			{
				foreach (Cost.ItemEntry itemEntry in cost.items)
				{
					if ((long)ItemUtilities.GetItemCount(itemEntry.id) < itemEntry.amount)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00062B56 File Offset: 0x00060D56
		public static bool Add(long amount)
		{
			if (EconomyManager.Instance == null)
			{
				return false;
			}
			EconomyManager.Money += amount;
			return true;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00062B74 File Offset: 0x00060D74
		public static bool IsWaitingForUnlockConfirm(int itemTypeID)
		{
			return !GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) && !(EconomyManager.Instance == null) && EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00062BA9 File Offset: 0x00060DA9
		public static bool IsUnlocked(int itemTypeID)
		{
			return GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) || (!(EconomyManager.Instance == null) && EconomyManager.Instance.UnlockedItemIds.Contains(itemTypeID));
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00062BE0 File Offset: 0x00060DE0
		public static void Unlock(int itemTypeID, bool needConfirm = true, bool showUI = true)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			if (EconomyManager.Instance.unlockedItemIds.Contains(itemTypeID))
			{
				return;
			}
			if (EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID))
			{
				return;
			}
			if (needConfirm)
			{
				EconomyManager.Instance.unlockesWaitingForConfirm.Add(itemTypeID);
			}
			else
			{
				EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			}
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged != null)
			{
				onItemUnlockStateChanged(itemTypeID);
			}
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			Debug.Log(EconomyManager.ItemUnlockNotificationTextMainFormat);
			Debug.Log(metaData.DisplayName);
			if (showUI)
			{
				NotificationText.Push("Notification_StockShoopItemUnlockFormat".ToPlainText().Format(new
				{
					displayName = metaData.DisplayName
				}));
			}
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00062C98 File Offset: 0x00060E98
		public static void ConfirmUnlock(int itemTypeID)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			EconomyManager.Instance.unlockesWaitingForConfirm.Remove(itemTypeID);
			EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged == null)
			{
				return;
			}
			onItemUnlockStateChanged(itemTypeID);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00062CE4 File Offset: 0x00060EE4
		public object GenerateSaveData()
		{
			return new EconomyManager.SaveData
			{
				money = EconomyManager.Money,
				unlockedItems = this.unlockedItemIds.ToArray(),
				unlockesWaitingForConfirm = this.unlockesWaitingForConfirm.ToArray()
			};
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00062D30 File Offset: 0x00060F30
		public void SetupSaveData(object rawData)
		{
			if (rawData is EconomyManager.SaveData)
			{
				EconomyManager.SaveData saveData = (EconomyManager.SaveData)rawData;
				this.money = saveData.money;
				this.unlockedItemIds.Clear();
				if (saveData.unlockedItems != null)
				{
					this.unlockedItemIds.AddRange(saveData.unlockedItems);
				}
				this.unlockesWaitingForConfirm.Clear();
				if (saveData.unlockesWaitingForConfirm != null)
				{
					this.unlockesWaitingForConfirm.AddRange(saveData.unlockesWaitingForConfirm);
				}
				return;
			}
		}

		// Token: 0x0400134E RID: 4942
		[SerializeField]
		private string itemUnlockNotificationTextMainFormat = "物品 {itemDisplayName} 已解锁";

		// Token: 0x0400134F RID: 4943
		[SerializeField]
		private string itemUnlockNotificationTextSubFormat = "请在对应商店中查看";

		// Token: 0x04001352 RID: 4946
		private const string saveKey = "EconomyData";

		// Token: 0x04001353 RID: 4947
		private long money;

		// Token: 0x04001354 RID: 4948
		[SerializeField]
		private List<int> unlockedItemIds;

		// Token: 0x04001355 RID: 4949
		[SerializeField]
		private List<int> unlockesWaitingForConfirm;

		// Token: 0x0400135A RID: 4954
		public const int CashItemID = 451;

		// Token: 0x020005CD RID: 1485
		[Serializable]
		public struct SaveData
		{
			// Token: 0x040020CA RID: 8394
			public long money;

			// Token: 0x040020CB RID: 8395
			public int[] unlockedItems;

			// Token: 0x040020CC RID: 8396
			public int[] unlockesWaitingForConfirm;
		}
	}
}
