using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Bilibili.BDS;
using Duckov;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Economy;
using Duckov.MasterKeys;
using Duckov.PerkTrees;
using Duckov.Quests;
using Duckov.Rules;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using Saves;
using Steamworks;
using UnityEngine;

namespace EventReports
{
	// Token: 0x0200022A RID: 554
	public class BDSManager : MonoBehaviour
	{
		// Token: 0x06001106 RID: 4358 RVA: 0x000424F4 File Offset: 0x000406F4
		private void Awake()
		{
			if (PlatformInfo.Platform == Platform.Steam)
			{
				if (SteamManager.Initialized && SteamUtils.IsSteamChinaLauncher())
				{
				}
			}
			else
			{
				string.Format("{0}", PlatformInfo.Platform);
			}
			Debug.Log("Player Info:\n" + BDSManager.PlayerInfo.GetCurrent().ToJson());
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0004254A File Offset: 0x0004074A
		private void Start()
		{
			this.OnGameStarted();
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00042552 File Offset: 0x00040752
		private void OnDestroy()
		{
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00042554 File Offset: 0x00040754
		private float TimeSinceLastHeartbeat
		{
			get
			{
				return Time.unscaledTime - this.lastTimeHeartbeat;
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00042562 File Offset: 0x00040762
		private void Update()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0004256A File Offset: 0x0004076A
		private void UpdateHeartbeat()
		{
			if (this.TimeSinceLastHeartbeat > 60f)
			{
				this.ReportCustomEvent(BDSManager.EventName.heartbeat, "");
				this.lastTimeHeartbeat = Time.unscaledTime;
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00042594 File Offset: 0x00040794
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			SavesSystem.OnSaveDeleted += this.OnSaveDeleted;
			RaidUtilities.OnNewRaid += this.OnNewRaid;
			RaidUtilities.OnRaidEnd += this.OnRaidEnd;
			SceneLoader.onStartedLoadingScene += this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene += this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			LevelManager.OnEvacuated += this.OnEvacuated;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			Quest.onQuestActivated += this.OnQuestActivated;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			EconomyManager.OnCostPaid += this.OnCostPaid;
			EconomyManager.OnMoneyPaid += this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory += this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage += this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased += this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Combine(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Combine(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead += this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Combine(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex += this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex += this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed += this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked += this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Combine(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer += this.OnItemSold;
			Reward.OnRewardClaimed += this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent += this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent += this.OnInteractStart;
			LevelManager.OnNewGameReport += this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent += this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent += this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged += this.OnCheatModeStatusChanged;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00042804 File Offset: 0x00040A04
		private void UnregisterEvents()
		{
			SavesSystem.OnSaveDeleted -= this.OnSaveDeleted;
			RaidUtilities.OnNewRaid -= this.OnNewRaid;
			RaidUtilities.OnRaidEnd -= this.OnRaidEnd;
			SceneLoader.onStartedLoadingScene -= this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene -= this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			Quest.onQuestActivated -= this.OnQuestActivated;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			EconomyManager.OnCostPaid -= this.OnCostPaid;
			EconomyManager.OnMoneyPaid -= this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory -= this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage -= this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased -= this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Remove(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Remove(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead -= this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Remove(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex -= this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex -= this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed -= this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked -= this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Remove(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer -= this.OnItemSold;
			Reward.OnRewardClaimed -= this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent -= this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent -= this.OnInteractStart;
			LevelManager.OnNewGameReport -= this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent -= this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent -= this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged -= this.OnCheatModeStatusChanged;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00042A70 File Offset: 0x00040C70
		private void OnCheatModeStatusChanged(bool value)
		{
			this.ReportCustomEvent<BDSManager.CheatModeStatusChangeContext>(BDSManager.EventName.cheat_mode_changed, new BDSManager.CheatModeStatusChangeContext
			{
				cheatModeActive = value
			});
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00042A96 File Offset: 0x00040C96
		private void OnCustomFaceFinish()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_finish, "");
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00042AA5 File Offset: 0x00040CA5
		private void OnCustomFaceStart()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_begin, "");
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00042AB4 File Offset: 0x00040CB4
		private void OnNewGameReport()
		{
			this.ReportCustomEvent(BDSManager.EventName.begin_new_game, "");
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00042AC4 File Offset: 0x00040CC4
		private void OnInteractStart(InteractableBase target)
		{
			if (target == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.InteractEventContext>(BDSManager.EventName.interact_start, new BDSManager.InteractEventContext
			{
				interactGameObjectName = target.name,
				typeName = target.GetType().Name
			});
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00042B0C File Offset: 0x00040D0C
		private void OnItemUsed(Item item)
		{
			this.ReportCustomEvent<BDSManager.ItemUseEventContext>(BDSManager.EventName.item_use, new BDSManager.ItemUseEventContext
			{
				itemTypeID = item.TypeID
			});
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00042B38 File Offset: 0x00040D38
		private void OnRewardClaimed(Reward reward)
		{
			int questID = (reward.Master != null) ? reward.Master.ID : -1;
			this.ReportCustomEvent<BDSManager.RewardClaimEventContext>(BDSManager.EventName.reward_claimed, new BDSManager.RewardClaimEventContext
			{
				questID = questID,
				rewardID = reward.ID
			});
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00042B88 File Offset: 0x00040D88
		private void OnItemSold(StockShop shop, Item item, int price)
		{
			if (item == null)
			{
				return;
			}
			string stockShopID = (shop != null) ? shop.MerchantID : null;
			this.ReportCustomEvent<BDSManager.ItemSoldEventContext>(BDSManager.EventName.item_sold, new BDSManager.ItemSoldEventContext
			{
				stockShopID = stockShopID,
				itemID = item.TypeID,
				price = price
			});
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00042BDC File Offset: 0x00040DDC
		private void OnMainCharacterSlotContentChanged(CharacterMainControl control, Slot slot)
		{
			if (control == null || slot == null)
			{
				return;
			}
			if (slot.Content == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.EquipEventContext>(BDSManager.EventName.role_equip, new BDSManager.EquipEventContext
			{
				slotKey = slot.Key,
				contentItemTypeID = slot.Content.TypeID
			});
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00042C38 File Offset: 0x00040E38
		private void OnMasterKeyUnlocked(int id)
		{
			this.ReportCustomEvent<BDSManager.MasterKeyUnlockContext>(BDSManager.EventName.masterkey_unlocked, new BDSManager.MasterKeyUnlockContext
			{
				keyID = id
			});
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00042C60 File Offset: 0x00040E60
		private void OnPerkUnlockConfirmed(Perk perk)
		{
			if (perk == null)
			{
				return;
			}
			BDSManager.EventName eventName = BDSManager.EventName.perk_unlocked;
			BDSManager.PerkInfo customParameters = default(BDSManager.PerkInfo);
			PerkTree master = perk.Master;
			customParameters.perkTreeID = ((master != null) ? master.ID : null);
			customParameters.perkName = perk.name;
			this.ReportCustomEvent<BDSManager.PerkInfo>(eventName, customParameters);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00042CB0 File Offset: 0x00040EB0
		private void OnBuildingBuilt(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_built, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00042CDC File Offset: 0x00040EDC
		private void OnBuildingDestroyed(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_destroyed, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00042D07 File Offset: 0x00040F07
		private void OnLevelChanged(int from, int to)
		{
			this.ReportCustomEvent<BDSManager.LevelChangedEventContext>(BDSManager.EventName.role_level_changed, new BDSManager.LevelChangedEventContext(from, to));
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00042D18 File Offset: 0x00040F18
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			Teams team = health.team;
			bool flag = false;
			if (info.fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (flag)
			{
				this.ReportCustomEvent<BDSManager.EnemyKillInfo>(BDSManager.EventName.enemy_kill, new BDSManager.EnemyKillInfo
				{
					enemyPresetName = BDSManager.<OnHealthDead>g__GetPresetName|36_0(health),
					damageInfo = info
				});
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00042D7E File Offset: 0x00040F7E
		private void OnFormulaUnlocked(string formulaID)
		{
			this.ReportCustomEvent(BDSManager.EventName.craft_formula_unlock, StrJson.Create(new string[]
			{
				"id",
				formulaID
			}));
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00042D9F File Offset: 0x00040F9F
		private void OnItemCrafted(CraftingFormula formula, Item item)
		{
			this.ReportCustomEvent<CraftingFormula>(BDSManager.EventName.craft_craft, formula);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00042DAC File Offset: 0x00040FAC
		private void OnItemPurchased(StockShop shop, Item item)
		{
			if (shop == null || item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.PurchaseInfo>(BDSManager.EventName.shop_purchased, new BDSManager.PurchaseInfo
			{
				shopID = shop.MerchantID,
				itemTypeID = item.TypeID,
				itemAmount = item.StackCount
			});
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00042E04 File Offset: 0x00041004
		private void OnItemSentToPlayerStorage(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_storage, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00042E48 File Offset: 0x00041048
		private void OnItemSentToPlayerInventory(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_inventory, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00042E8C File Offset: 0x0004108C
		private void OnMoneyPaid(long money)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_money, new Cost
			{
				money = money,
				items = new Cost.ItemEntry[0]
			});
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00042EBF File Offset: 0x000410BF
		private void OnCostPaid(Cost cost)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_cost, cost);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00042ECA File Offset: 0x000410CA
		private void OnQuestActivated(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_activate, quest.GetInfo());
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00042EE4 File Offset: 0x000410E4
		private void OnQuestCompleted(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_complete, quest.GetInfo());
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00042F00 File Offset: 0x00041100
		private void OnMainCharacterDead(DamageInfo info)
		{
			string fromCharacterPresetName = "None";
			string fromCharacterNameKey = "None";
			if (info.fromCharacter)
			{
				CharacterRandomPreset characterPreset = info.fromCharacter.characterPreset;
				if (characterPreset != null)
				{
					fromCharacterPresetName = characterPreset.name;
					fromCharacterNameKey = characterPreset.nameKey;
				}
			}
			this.ReportCustomEvent<BDSManager.CharacterDeathContext>(BDSManager.EventName.main_character_dead, new BDSManager.CharacterDeathContext
			{
				damageInfo = info,
				levelInfo = LevelManager.GetCurrentLevelInfo(),
				fromCharacterPresetName = fromCharacterPresetName,
				fromCharacterNameKey = fromCharacterNameKey
			});
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00042F80 File Offset: 0x00041180
		private void OnEvacuated(EvacuationInfo evacuationInfo)
		{
			LevelManager.LevelInfo currentLevelInfo = LevelManager.GetCurrentLevelInfo();
			RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
			BDSManager.PlayerStatus playerStatus = BDSManager.PlayerStatus.CreateFromCurrent();
			this.ReportCustomEvent<BDSManager.EvacuationEventData>(BDSManager.EventName.level_evacuated, new BDSManager.EvacuationEventData
			{
				evacuationInfo = evacuationInfo,
				mapID = currentLevelInfo.activeSubSceneID,
				raidInfo = currentRaid,
				playerStatus = playerStatus
			});
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00042FD5 File Offset: 0x000411D5
		private void OnLevelInitialized()
		{
			this.ReportCustomEvent<LevelManager.LevelInfo>(BDSManager.EventName.level_initialized, LevelManager.GetCurrentLevelInfo());
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00042FE3 File Offset: 0x000411E3
		private void OnSceneLoadingFinish(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_start, context);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00042FED File Offset: 0x000411ED
		private void OnSceneLoadingStart(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_finish, context);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00042FF7 File Offset: 0x000411F7
		private void OnRaidEnd(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_end, info);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00043001 File Offset: 0x00041201
		private void OnNewRaid(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_new, info);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004300B File Offset: 0x0004120B
		private void OnSaveDeleted()
		{
			this.ReportCustomEvent(BDSManager.EventName.delete_save_data, StrJson.Create(new string[]
			{
				"slot",
				string.Format("{0}", SavesSystem.CurrentSlot)
			}));
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00043040 File Offset: 0x00041240
		private void OnGameStarted()
		{
			int @int = PlayerPrefs.GetInt("AppStartCount", 0);
			this.sessionInfo = new BDSManager.SessionInfo
			{
				startCount = @int,
				isFirstTimeStart = (@int <= 0),
				session_id = DateTime.Now.ToBinary().GetHashCode()
			};
			this.sessionStartTime = DateTime.Now;
			this.ReportCustomEvent<BDSManager.SessionInfo>(BDSManager.EventName.app_start, this.sessionInfo);
			PlayerPrefs.SetInt("AppStartCount", @int + 1);
			PlayerPrefs.Save();
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000430C6 File Offset: 0x000412C6
		private void ReportCustomEvent(BDSManager.EventName eventName, StrJson customParameters)
		{
			this.ReportCustomEvent(eventName, customParameters.ToString());
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000430D8 File Offset: 0x000412D8
		private void ReportCustomEvent<T>(BDSManager.EventName eventName, T customParameters)
		{
			string customParameters2 = (customParameters != null) ? JsonUtility.ToJson(customParameters) : "";
			this.ReportCustomEvent(eventName, customParameters2);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00043108 File Offset: 0x00041308
		private void ReportCustomEvent(BDSManager.EventName eventName, string customParameters = "")
		{
			string strPlayerInfo = BDSManager.PlayerInfo.GetCurrent().ToJson();
			SDK.ReportCustomEvent(eventName.ToString(), strPlayerInfo, "", customParameters);
			try
			{
				Action<string, string> onReportCustomEvent = BDSManager.OnReportCustomEvent;
				if (onReportCustomEvent != null)
				{
					onReportCustomEvent(eventName.ToString(), customParameters);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0004317C File Offset: 0x0004137C
		[CompilerGenerated]
		internal static string <OnHealthDead>g__GetPresetName|36_0(Health health)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (characterMainControl == null)
			{
				return "None";
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (characterPreset == null)
			{
				return "None";
			}
			return characterPreset.Name;
		}

		// Token: 0x04000D74 RID: 3444
		private float lastTimeHeartbeat;

		// Token: 0x04000D75 RID: 3445
		private int sessionID;

		// Token: 0x04000D76 RID: 3446
		private DateTime sessionStartTime;

		// Token: 0x04000D77 RID: 3447
		private BDSManager.SessionInfo sessionInfo;

		// Token: 0x04000D78 RID: 3448
		public static Action<string, string> OnReportCustomEvent;

		// Token: 0x02000518 RID: 1304
		private enum EventName
		{
			// Token: 0x04001E29 RID: 7721
			none,
			// Token: 0x04001E2A RID: 7722
			app_start,
			// Token: 0x04001E2B RID: 7723
			begin_new_game,
			// Token: 0x04001E2C RID: 7724
			delete_save_data,
			// Token: 0x04001E2D RID: 7725
			raid_new,
			// Token: 0x04001E2E RID: 7726
			raid_end,
			// Token: 0x04001E2F RID: 7727
			scene_load_start,
			// Token: 0x04001E30 RID: 7728
			scene_load_finish,
			// Token: 0x04001E31 RID: 7729
			level_initialized,
			// Token: 0x04001E32 RID: 7730
			level_evacuated,
			// Token: 0x04001E33 RID: 7731
			main_character_dead,
			// Token: 0x04001E34 RID: 7732
			quest_activate,
			// Token: 0x04001E35 RID: 7733
			quest_complete,
			// Token: 0x04001E36 RID: 7734
			pay_money,
			// Token: 0x04001E37 RID: 7735
			pay_cost,
			// Token: 0x04001E38 RID: 7736
			item_to_inventory,
			// Token: 0x04001E39 RID: 7737
			item_to_storage,
			// Token: 0x04001E3A RID: 7738
			shop_purchased,
			// Token: 0x04001E3B RID: 7739
			craft_craft,
			// Token: 0x04001E3C RID: 7740
			craft_formula_unlock,
			// Token: 0x04001E3D RID: 7741
			enemy_kill,
			// Token: 0x04001E3E RID: 7742
			role_level_changed,
			// Token: 0x04001E3F RID: 7743
			building_built,
			// Token: 0x04001E40 RID: 7744
			building_destroyed,
			// Token: 0x04001E41 RID: 7745
			perk_unlocked,
			// Token: 0x04001E42 RID: 7746
			masterkey_unlocked,
			// Token: 0x04001E43 RID: 7747
			role_equip,
			// Token: 0x04001E44 RID: 7748
			item_sold,
			// Token: 0x04001E45 RID: 7749
			reward_claimed,
			// Token: 0x04001E46 RID: 7750
			item_use,
			// Token: 0x04001E47 RID: 7751
			interact_start,
			// Token: 0x04001E48 RID: 7752
			face_customize_begin,
			// Token: 0x04001E49 RID: 7753
			face_customize_finish,
			// Token: 0x04001E4A RID: 7754
			heartbeat,
			// Token: 0x04001E4B RID: 7755
			cheat_mode_changed,
			// Token: 0x04001E4C RID: 7756
			app_end
		}

		// Token: 0x02000519 RID: 1305
		private struct CheatModeStatusChangeContext
		{
			// Token: 0x04001E4D RID: 7757
			public bool cheatModeActive;
		}

		// Token: 0x0200051A RID: 1306
		private struct InteractEventContext
		{
			// Token: 0x04001E4E RID: 7758
			public string interactGameObjectName;

			// Token: 0x04001E4F RID: 7759
			public string typeName;
		}

		// Token: 0x0200051B RID: 1307
		private struct ItemUseEventContext
		{
			// Token: 0x04001E50 RID: 7760
			public int itemTypeID;
		}

		// Token: 0x0200051C RID: 1308
		private struct RewardClaimEventContext
		{
			// Token: 0x04001E51 RID: 7761
			public int questID;

			// Token: 0x04001E52 RID: 7762
			public int rewardID;
		}

		// Token: 0x0200051D RID: 1309
		private struct ItemSoldEventContext
		{
			// Token: 0x04001E53 RID: 7763
			public string stockShopID;

			// Token: 0x04001E54 RID: 7764
			public int itemID;

			// Token: 0x04001E55 RID: 7765
			public int price;
		}

		// Token: 0x0200051E RID: 1310
		private struct EquipEventContext
		{
			// Token: 0x04001E56 RID: 7766
			public string slotKey;

			// Token: 0x04001E57 RID: 7767
			public int contentItemTypeID;
		}

		// Token: 0x0200051F RID: 1311
		private struct MasterKeyUnlockContext
		{
			// Token: 0x04001E58 RID: 7768
			public int keyID;
		}

		// Token: 0x02000520 RID: 1312
		private struct PerkInfo
		{
			// Token: 0x04001E59 RID: 7769
			public string perkTreeID;

			// Token: 0x04001E5A RID: 7770
			public string perkName;
		}

		// Token: 0x02000521 RID: 1313
		private struct BuildingEventContext
		{
			// Token: 0x04001E5B RID: 7771
			public string buildingID;
		}

		// Token: 0x02000522 RID: 1314
		private struct LevelChangedEventContext
		{
			// Token: 0x060027F2 RID: 10226 RVA: 0x0009264E File Offset: 0x0009084E
			public LevelChangedEventContext(int from, int to)
			{
				this.from = from;
				this.to = to;
			}

			// Token: 0x04001E5C RID: 7772
			public int from;

			// Token: 0x04001E5D RID: 7773
			public int to;
		}

		// Token: 0x02000523 RID: 1315
		private struct EnemyKillInfo
		{
			// Token: 0x04001E5E RID: 7774
			public string enemyPresetName;

			// Token: 0x04001E5F RID: 7775
			public DamageInfo damageInfo;
		}

		// Token: 0x02000524 RID: 1316
		[Serializable]
		public struct PurchaseInfo
		{
			// Token: 0x04001E60 RID: 7776
			public string shopID;

			// Token: 0x04001E61 RID: 7777
			public int itemTypeID;

			// Token: 0x04001E62 RID: 7778
			public int itemAmount;
		}

		// Token: 0x02000525 RID: 1317
		private struct ItemInfo
		{
			// Token: 0x04001E63 RID: 7779
			public int itemId;

			// Token: 0x04001E64 RID: 7780
			public int amount;
		}

		// Token: 0x02000526 RID: 1318
		public struct CharacterDeathContext
		{
			// Token: 0x04001E65 RID: 7781
			public DamageInfo damageInfo;

			// Token: 0x04001E66 RID: 7782
			public string fromCharacterPresetName;

			// Token: 0x04001E67 RID: 7783
			public string fromCharacterNameKey;

			// Token: 0x04001E68 RID: 7784
			public LevelManager.LevelInfo levelInfo;
		}

		// Token: 0x02000527 RID: 1319
		[Serializable]
		private struct PlayerStatus
		{
			// Token: 0x060027F3 RID: 10227 RVA: 0x00092660 File Offset: 0x00090860
			public static BDSManager.PlayerStatus CreateFromCurrent()
			{
				CharacterMainControl main = CharacterMainControl.Main;
				if (main == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				Health health = main.Health;
				if (health == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				CharacterBuffManager buffManager = main.GetBuffManager();
				if (buffManager == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				if (main.CharacterItem == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				string[] array = new string[buffManager.Buffs.Count];
				for (int i = 0; i < buffManager.Buffs.Count; i++)
				{
					Buff buff = buffManager.Buffs[i];
					if (!(buff == null))
					{
						array[i] = string.Format("{0} {1}", buff.ID, buff.DisplayNameKey);
					}
				}
				int totalRawValue = main.CharacterItem.GetTotalRawValue();
				return new BDSManager.PlayerStatus
				{
					valid = true,
					healthMax = health.MaxHealth,
					health = main.CurrentEnergy,
					water = main.CurrentWater,
					food = main.CurrentEnergy,
					waterMax = main.MaxWater,
					foodMax = main.MaxEnergy,
					totalItemValue = totalRawValue
				};
			}

			// Token: 0x04001E69 RID: 7785
			public bool valid;

			// Token: 0x04001E6A RID: 7786
			public float healthMax;

			// Token: 0x04001E6B RID: 7787
			public float health;

			// Token: 0x04001E6C RID: 7788
			public float waterMax;

			// Token: 0x04001E6D RID: 7789
			public float foodMax;

			// Token: 0x04001E6E RID: 7790
			public float water;

			// Token: 0x04001E6F RID: 7791
			public float food;

			// Token: 0x04001E70 RID: 7792
			public string[] activeEffects;

			// Token: 0x04001E71 RID: 7793
			public int totalItemValue;
		}

		// Token: 0x02000528 RID: 1320
		private struct EvacuationEventData
		{
			// Token: 0x04001E72 RID: 7794
			public EvacuationInfo evacuationInfo;

			// Token: 0x04001E73 RID: 7795
			public string mapID;

			// Token: 0x04001E74 RID: 7796
			public RaidUtilities.RaidInfo raidInfo;

			// Token: 0x04001E75 RID: 7797
			public BDSManager.PlayerStatus playerStatus;
		}

		// Token: 0x02000529 RID: 1321
		[Serializable]
		private struct SessionInfo
		{
			// Token: 0x04001E76 RID: 7798
			public int startCount;

			// Token: 0x04001E77 RID: 7799
			public bool isFirstTimeStart;

			// Token: 0x04001E78 RID: 7800
			public int session_id;

			// Token: 0x04001E79 RID: 7801
			public int session_duration_seconds;
		}

		// Token: 0x0200052A RID: 1322
		public struct PlayerInfo
		{
			// Token: 0x060027F4 RID: 10228 RVA: 0x000927B4 File Offset: 0x000909B4
			public PlayerInfo(int level, string steamAccountID, int saveSlot, string location, string language, string displayName, string difficulty, string platform, string version, string system)
			{
				this.role_name = displayName;
				this.profession_type = language;
				this.gender = version;
				this.level = string.Format("{0}", level);
				this.b_account_id = steamAccountID;
				this.b_role_id = string.Format("{0}|{1}", saveSlot, difficulty);
				this.b_tour_indicator = "0";
				this.b_zone_id = location;
				this.b_sdk_uid = platform + "|" + system;
			}

			// Token: 0x060027F5 RID: 10229 RVA: 0x00092838 File Offset: 0x00090A38
			public static BDSManager.PlayerInfo GetCurrent()
			{
				string id = PlatformInfo.GetID();
				string displayName = PlatformInfo.GetDisplayName();
				return new BDSManager.PlayerInfo(EXPManager.Level, id, SavesSystem.CurrentSlot, RegionInfo.CurrentRegion.Name, Application.systemLanguage.ToString(), displayName, GameRulesManager.Current.displayNameKey, PlatformInfo.Platform.ToString(), GameMetaData.Instance.Version.ToString(), Environment.OSVersion.Platform.ToString())
				{
					gender = GameMetaData.Instance.Version.ToString()
				};
			}

			// Token: 0x060027F6 RID: 10230 RVA: 0x000928F4 File Offset: 0x00090AF4
			public static string GetCurrentJson()
			{
				return BDSManager.PlayerInfo.GetCurrent().ToJson();
			}

			// Token: 0x060027F7 RID: 10231 RVA: 0x0009290E File Offset: 0x00090B0E
			public string ToJson()
			{
				return JsonUtility.ToJson(this);
			}

			// Token: 0x04001E7A RID: 7802
			public string role_name;

			// Token: 0x04001E7B RID: 7803
			public string profession_type;

			// Token: 0x04001E7C RID: 7804
			public string gender;

			// Token: 0x04001E7D RID: 7805
			public string level;

			// Token: 0x04001E7E RID: 7806
			public string b_account_id;

			// Token: 0x04001E7F RID: 7807
			public string b_role_id;

			// Token: 0x04001E80 RID: 7808
			public string b_tour_indicator;

			// Token: 0x04001E81 RID: 7809
			public string b_zone_id;

			// Token: 0x04001E82 RID: 7810
			public string b_sdk_uid;
		}
	}
}
