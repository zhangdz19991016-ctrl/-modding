using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Rules;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Token: 0x02000109 RID: 265
public class LevelManager : MonoBehaviour
{
	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x00028689 File Offset: 0x00026889
	public static LevelManager Instance
	{
		get
		{
			if (!LevelManager.instance)
			{
				LevelManager.SetInstance();
			}
			return LevelManager.instance;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x000286A4 File Offset: 0x000268A4
	public static Transform LootBoxInventoriesParent
	{
		get
		{
			if (LevelManager.Instance._lootBoxInventoriesParent == null)
			{
				GameObject gameObject = new GameObject("Loot Box Inventories");
				gameObject.transform.SetParent(LevelManager.Instance.transform);
				LevelManager.Instance._lootBoxInventoriesParent = gameObject.transform;
				LevelManager.LootBoxInventories.Clear();
			}
			return LevelManager.Instance._lootBoxInventoriesParent;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x00028707 File Offset: 0x00026907
	public static Dictionary<int, Inventory> LootBoxInventories
	{
		get
		{
			if (LevelManager.Instance._lootBoxInventories == null)
			{
				LevelManager.Instance._lootBoxInventories = new Dictionary<int, Inventory>();
			}
			return LevelManager.Instance._lootBoxInventories;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0002872E File Offset: 0x0002692E
	public bool IsRaidMap
	{
		get
		{
			return LevelConfig.IsRaidMap;
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00028735 File Offset: 0x00026935
	public bool IsBaseLevel
	{
		get
		{
			return LevelConfig.IsBaseLevel;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0002873C File Offset: 0x0002693C
	public InputManager InputManager
	{
		get
		{
			return this.inputManager;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00028744 File Offset: 0x00026944
	public CharacterCreator CharacterCreator
	{
		get
		{
			return this.characterCreator;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0002874C File Offset: 0x0002694C
	public ExitCreator ExitCreator
	{
		get
		{
			return this.exitCreator;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00028754 File Offset: 0x00026954
	public ExplosionManager ExplosionManager
	{
		get
		{
			return this.explosionManager;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0002875C File Offset: 0x0002695C
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00028768 File Offset: 0x00026968
	public CharacterMainControl MainCharacter
	{
		get
		{
			return this.mainCharacter;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00028770 File Offset: 0x00026970
	public CharacterMainControl PetCharacter
	{
		get
		{
			return this.petCharacter;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00028778 File Offset: 0x00026978
	public GameCamera GameCamera
	{
		get
		{
			return this.gameCamera;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x00028780 File Offset: 0x00026980
	public FogOfWarManager FogOfWarManager
	{
		get
		{
			return this.fowManager;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060008FB RID: 2299 RVA: 0x00028788 File Offset: 0x00026988
	public TimeOfDayController TimeOfDayController
	{
		get
		{
			return this.timeOfDayController;
		}
	}

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060008FC RID: 2300 RVA: 0x00028790 File Offset: 0x00026990
	// (remove) Token: 0x060008FD RID: 2301 RVA: 0x000287C4 File Offset: 0x000269C4
	public static event Action OnLevelBeginInitializing;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x060008FE RID: 2302 RVA: 0x000287F8 File Offset: 0x000269F8
	// (remove) Token: 0x060008FF RID: 2303 RVA: 0x0002882C File Offset: 0x00026A2C
	public static event Action OnLevelInitialized;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06000900 RID: 2304 RVA: 0x00028860 File Offset: 0x00026A60
	// (remove) Token: 0x06000901 RID: 2305 RVA: 0x00028894 File Offset: 0x00026A94
	public static event Action OnAfterLevelInitialized;

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x000288C7 File Offset: 0x00026AC7
	public AIMainBrain AIMainBrain
	{
		get
		{
			return this.aiMainBrain;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x000288CF File Offset: 0x00026ACF
	public static bool LevelInitializing
	{
		get
		{
			return !(LevelManager.Instance == null) && LevelManager.Instance.initingLevel;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x000288EA File Offset: 0x00026AEA
	public static bool AfterInit
	{
		get
		{
			return !(LevelManager.Instance == null) && LevelManager.Instance.afterInit;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x00028905 File Offset: 0x00026B05
	public PetProxy PetProxy
	{
		get
		{
			return this.petProxy;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x0002890D File Offset: 0x00026B0D
	public BulletPool BulletPool
	{
		get
		{
			return this.bulletPool;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00028915 File Offset: 0x00026B15
	public CustomFaceManager CustomFaceManager
	{
		get
		{
			return this.customFaceManager;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x0002891D File Offset: 0x00026B1D
	// (set) Token: 0x06000909 RID: 2313 RVA: 0x00028938 File Offset: 0x00026B38
	public static string LevelInitializingComment
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance._levelInitializingComment;
		}
		set
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			LevelManager.Instance._levelInitializingComment = value;
			Action<string> onLevelInitializingCommentChanged = LevelManager.OnLevelInitializingCommentChanged;
			if (onLevelInitializingCommentChanged != null)
			{
				onLevelInitializingCommentChanged(value);
			}
			Debug.Log("[Level Initialization] " + value);
		}
	}

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x0600090A RID: 2314 RVA: 0x00028974 File Offset: 0x00026B74
	// (remove) Token: 0x0600090B RID: 2315 RVA: 0x000289A8 File Offset: 0x00026BA8
	public static event Action<string> OnLevelInitializingCommentChanged;

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x0600090C RID: 2316 RVA: 0x000289DB File Offset: 0x00026BDB
	public static bool LevelInited
	{
		get
		{
			return !(LevelManager.instance == null) && LevelManager.instance.levelInited;
		}
	}

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x0600090D RID: 2317 RVA: 0x000289F8 File Offset: 0x00026BF8
	// (remove) Token: 0x0600090E RID: 2318 RVA: 0x00028A2C File Offset: 0x00026C2C
	public static event Action<EvacuationInfo> OnEvacuated;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x0600090F RID: 2319 RVA: 0x00028A60 File Offset: 0x00026C60
	// (remove) Token: 0x06000910 RID: 2320 RVA: 0x00028A94 File Offset: 0x00026C94
	public static event Action<DamageInfo> OnMainCharacterDead;

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x00028AC7 File Offset: 0x00026CC7
	public float LevelTime
	{
		get
		{
			return Time.time - this.levelStartTime;
		}
	}

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06000912 RID: 2322 RVA: 0x00028AD8 File Offset: 0x00026CD8
	// (remove) Token: 0x06000913 RID: 2323 RVA: 0x00028B0C File Offset: 0x00026D0C
	public static event Action OnNewGameReport;

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000914 RID: 2324 RVA: 0x00028B3F File Offset: 0x00026D3F
	public static Ruleset Rule
	{
		get
		{
			return LevelManager.rule;
		}
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00028B46 File Offset: 0x00026D46
	public static void RegisterWaitForInitialization<T>(T toWait) where T : class, IInitializedQueryHandler
	{
		if (toWait == null)
		{
			return;
		}
		if (toWait == null)
		{
			return;
		}
		LevelManager.waitForInitializationList.Add(toWait);
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00028B6A File Offset: 0x00026D6A
	public static bool UnregisterWaitForInitialization<T>(T obj) where T : class
	{
		return LevelManager.waitForInitializationList.Remove(obj);
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00028B7C File Offset: 0x00026D7C
	private void Start()
	{
		if (!SceneLoader.IsSceneLoading)
		{
			this.StartInit(default(SceneLoadingContext));
		}
		else
		{
			SceneLoader.onFinishedLoadingScene += this.StartInit;
		}
		if (!SavesSystem.Load<bool>("NewGameReported"))
		{
			SavesSystem.Save<bool>("NewGameReported", true);
			Action onNewGameReport = LevelManager.OnNewGameReport;
			if (onNewGameReport != null)
			{
				onNewGameReport();
			}
		}
		if (GameManager.newBoot)
		{
			this.OnNewBoot();
			GameManager.newBoot = false;
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00028BEC File Offset: 0x00026DEC
	private void OnDestroy()
	{
		SceneLoader.onFinishedLoadingScene -= this.StartInit;
		CharacterMainControl characterMainControl = this.mainCharacter;
		if (characterMainControl == null)
		{
			return;
		}
		Health health = characterMainControl.Health;
		if (health == null)
		{
			return;
		}
		health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnMainCharacterDie));
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00028C2A File Offset: 0x00026E2A
	private void OnNewBoot()
	{
		Debug.Log("New boot");
		GameClock.Instance.StepTimeTil(new TimeSpan(7, 0, 0));
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00028C48 File Offset: 0x00026E48
	private void StartInit(SceneLoadingContext context)
	{
		this.InitLevel(context).Forget();
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00028C64 File Offset: 0x00026E64
	private UniTaskVoid InitLevel(SceneLoadingContext context)
	{
		LevelManager.<InitLevel>d__116 <InitLevel>d__;
		<InitLevel>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<InitLevel>d__.<>4__this = this;
		<InitLevel>d__.context = context;
		<InitLevel>d__.<>1__state = -1;
		<InitLevel>d__.<>t__builder.Start<LevelManager.<InitLevel>d__116>(ref <InitLevel>d__);
		return <InitLevel>d__.<>t__builder.Task;
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00028CB0 File Offset: 0x00026EB0
	private UniTask CreateMate()
	{
		LevelManager.<CreateMate>d__117 <CreateMate>d__;
		<CreateMate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateMate>d__.<>4__this = this;
		<CreateMate>d__.<>1__state = -1;
		<CreateMate>d__.<>t__builder.Start<LevelManager.<CreateMate>d__117>(ref <CreateMate>d__);
		return <CreateMate>d__.<>t__builder.Task;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00028CF4 File Offset: 0x00026EF4
	private UniTask WaitForOtherInitialization()
	{
		LevelManager.<WaitForOtherInitialization>d__118 <WaitForOtherInitialization>d__;
		<WaitForOtherInitialization>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<WaitForOtherInitialization>d__.<>1__state = -1;
		<WaitForOtherInitialization>d__.<>t__builder.Start<LevelManager.<WaitForOtherInitialization>d__118>(ref <WaitForOtherInitialization>d__);
		return <WaitForOtherInitialization>d__.<>t__builder.Task;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00028D30 File Offset: 0x00026F30
	private void HandleRaidInitialization()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		if (this.IsRaidMap)
		{
			if (currentRaid.ended)
			{
				RaidUtilities.NewRaid();
				this.isNewRaidLevel = true;
				return;
			}
		}
		else if (this.IsBaseLevel && !currentRaid.ended)
		{
			RaidUtilities.NotifyEnd();
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00028D78 File Offset: 0x00026F78
	public void RefreshMainCharacterFace()
	{
		if (this.mainCharacter.characterModel.CustomFace)
		{
			CustomFaceSettingData saveData = this.customFaceManager.LoadMainCharacterSetting();
			this.mainCharacter.characterModel.CustomFace.LoadFromData(saveData);
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00028DC0 File Offset: 0x00026FC0
	private UniTask CreateMainCharacterAsync(Vector3 position, Quaternion rotation)
	{
		LevelManager.<CreateMainCharacterAsync>d__121 <CreateMainCharacterAsync>d__;
		<CreateMainCharacterAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateMainCharacterAsync>d__.<>4__this = this;
		<CreateMainCharacterAsync>d__.position = position;
		<CreateMainCharacterAsync>d__.rotation = rotation;
		<CreateMainCharacterAsync>d__.<>1__state = -1;
		<CreateMainCharacterAsync>d__.<>t__builder.Start<LevelManager.<CreateMainCharacterAsync>d__121>(ref <CreateMainCharacterAsync>d__);
		return <CreateMainCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00028E14 File Offset: 0x00027014
	private void SetCharacterItemsInspected()
	{
		foreach (Slot slot in this.mainCharacter.CharacterItem.Slots)
		{
			if (slot.Content != null)
			{
				slot.Content.Inspected = true;
			}
		}
		foreach (Item item in this.mainCharacter.CharacterItem.Inventory)
		{
			if (item != null)
			{
				item.Inspected = true;
			}
		}
		foreach (Item item2 in this.petProxy.Inventory)
		{
			if (item2 != null)
			{
				item2.Inspected = true;
			}
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00028F1C File Offset: 0x0002711C
	private static void SetInstance()
	{
		if (LevelManager.instance)
		{
			return;
		}
		LevelManager.instance = UnityEngine.Object.FindFirstObjectByType<LevelManager>();
		LevelManager.instance;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00028F40 File Offset: 0x00027140
	private UniTask<Item> LoadOrCreateCharacterItemInstance()
	{
		LevelManager.<LoadOrCreateCharacterItemInstance>d__124 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.<>4__this = this;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<LevelManager.<LoadOrCreateCharacterItemInstance>d__124>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00028F83 File Offset: 0x00027183
	public void NotifyEvacuated(EvacuationInfo info)
	{
		this.mainCharacter.Health.SetInvincible(true);
		Action<EvacuationInfo> onEvacuated = LevelManager.OnEvacuated;
		if (onEvacuated != null)
		{
			onEvacuated(info);
		}
		this.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		SavesSystem.SaveFile(true);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00028FB8 File Offset: 0x000271B8
	public void NotifySaveBeforeLoadScene(bool saveToFile)
	{
		this.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		if (saveToFile)
		{
			SavesSystem.SaveFile(true);
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00028FD0 File Offset: 0x000271D0
	private void OnMainCharacterDie(DamageInfo dmgInfo)
	{
		if (this.dieTask)
		{
			return;
		}
		this.dieTask = true;
		this.CharacterDieTask(dmgInfo).Forget();
		Action<DamageInfo> onMainCharacterDead = LevelManager.OnMainCharacterDead;
		if (onMainCharacterDead == null)
		{
			return;
		}
		onMainCharacterDead(dmgInfo);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0002900C File Offset: 0x0002720C
	private UniTaskVoid CharacterDieTask(DamageInfo dmgInfo)
	{
		LevelManager.<CharacterDieTask>d__129 <CharacterDieTask>d__;
		<CharacterDieTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CharacterDieTask>d__.<>4__this = this;
		<CharacterDieTask>d__.dmgInfo = dmgInfo;
		<CharacterDieTask>d__.<>1__state = -1;
		<CharacterDieTask>d__.<>t__builder.Start<LevelManager.<CharacterDieTask>d__129>(ref <CharacterDieTask>d__);
		return <CharacterDieTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00029057 File Offset: 0x00027257
	internal void SaveMainCharacter()
	{
		this.mainCharacter.CharacterItem.Save("MainCharacterItemData");
		SavesSystem.Save<float>("MainCharacterHealth", this.MainCharacter.Health.CurrentHealth);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00029088 File Offset: 0x00027288
	[return: TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})]
	private ValueTuple<string, SubSceneEntry.Location> GetPlayerStartLocation()
	{
		List<ValueTuple<string, SubSceneEntry.Location>> list = new List<ValueTuple<string, SubSceneEntry.Location>>();
		string text = "StartPoints";
		if (LevelManager.loadLevelBeaconIndex > 0)
		{
			text = text + "_" + LevelManager.loadLevelBeaconIndex.ToString();
			LevelManager.loadLevelBeaconIndex = 0;
		}
		foreach (SubSceneEntry subSceneEntry in MultiSceneCore.Instance.SubScenes)
		{
			foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
			{
				if (this.IsPathCompatible(location, text))
				{
					list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry.sceneID, location));
				}
			}
		}
		if (list.Count == 0)
		{
			text = "StartPoints";
			foreach (SubSceneEntry subSceneEntry2 in MultiSceneCore.Instance.SubScenes)
			{
				foreach (SubSceneEntry.Location location2 in subSceneEntry2.cachedLocations)
				{
					if (this.IsPathCompatible(location2, text))
					{
						list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry2.sceneID, location2));
					}
				}
			}
		}
		return list.GetRandom<ValueTuple<string, SubSceneEntry.Location>>();
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00029218 File Offset: 0x00027418
	private void CreateMainCharacterMapElement()
	{
		if (MultiSceneCore.Instance != null)
		{
			SimplePointOfInterest simplePointOfInterest = this.mainCharacter.gameObject.AddComponent<SimplePointOfInterest>();
			simplePointOfInterest.Color = this.characterMapIconColor;
			simplePointOfInterest.ShadowColor = this.characterMapShadowColor;
			simplePointOfInterest.ShadowDistance = 0f;
			simplePointOfInterest.Setup(this.characterMapIcon, "You", true, null);
		}
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00029277 File Offset: 0x00027477
	private void OnSubSceneLoaded()
	{
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0002927C File Offset: 0x0002747C
	private bool IsPathCompatible(SubSceneEntry.Location location, string keyWord)
	{
		string path = location.path;
		int num = path.IndexOf('/');
		return num != -1 && path.Substring(0, num) == keyWord;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x000292B0 File Offset: 0x000274B0
	public void TestTeleport()
	{
		MultiSceneCore.Instance.LoadAndTeleport(this.testTeleportTarget).Forget<bool>();
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x000292C8 File Offset: 0x000274C8
	private LevelManager.LevelInfo mGetInfo()
	{
		Scene? activeSubScene = MultiSceneCore.ActiveSubScene;
		string activeSubSceneID = (activeSubScene != null) ? activeSubScene.Value.name : "";
		return new LevelManager.LevelInfo
		{
			isBaseLevel = this.IsBaseLevel,
			sceneName = base.gameObject.scene.name,
			activeSubSceneID = activeSubSceneID
		};
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00029334 File Offset: 0x00027534
	public static LevelManager.LevelInfo GetCurrentLevelInfo()
	{
		if (LevelManager.Instance == null)
		{
			return default(LevelManager.LevelInfo);
		}
		return LevelManager.Instance.mGetInfo();
	}

	// Token: 0x04000824 RID: 2084
	private Transform _lootBoxInventoriesParent;

	// Token: 0x04000825 RID: 2085
	private Dictionary<int, Inventory> _lootBoxInventories;

	// Token: 0x04000826 RID: 2086
	[SerializeField]
	private Transform defaultStartPos;

	// Token: 0x04000827 RID: 2087
	private static LevelManager instance;

	// Token: 0x04000828 RID: 2088
	[SerializeField]
	private InputManager inputManager;

	// Token: 0x04000829 RID: 2089
	[SerializeField]
	private CharacterCreator characterCreator;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	private ExitCreator exitCreator;

	// Token: 0x0400082B RID: 2091
	[SerializeField]
	private ExplosionManager explosionManager;

	// Token: 0x0400082C RID: 2092
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x0400082D RID: 2093
	private CharacterMainControl mainCharacter;

	// Token: 0x0400082E RID: 2094
	private CharacterMainControl petCharacter;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	private GameCamera gameCamera;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private FogOfWarManager fowManager;

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	private TimeOfDayController timeOfDayController;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private AIMainBrain aiMainBrain;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private CharacterRandomPreset matePreset;

	// Token: 0x04000837 RID: 2103
	private bool initingLevel;

	// Token: 0x04000838 RID: 2104
	private bool isNewRaidLevel;

	// Token: 0x04000839 RID: 2105
	private bool afterInit;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private CharacterRandomPreset petPreset;

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private Sprite characterMapIcon;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	private Color characterMapIconColor;

	// Token: 0x0400083D RID: 2109
	[SerializeField]
	private Color characterMapShadowColor;

	// Token: 0x0400083E RID: 2110
	[SerializeField]
	private MultiSceneLocation testTeleportTarget;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	public SkillBase defaultSkill;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private PetProxy petProxy;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private CustomFaceManager customFaceManager;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private BulletPool bulletPool;

	// Token: 0x04000843 RID: 2115
	private string _levelInitializingComment = "";

	// Token: 0x04000844 RID: 2116
	public static int loadLevelBeaconIndex = 0;

	// Token: 0x04000846 RID: 2118
	private bool levelInited;

	// Token: 0x04000847 RID: 2119
	public const string MainCharacterItemSaveKey = "MainCharacterItemData";

	// Token: 0x04000848 RID: 2120
	public const string MainCharacterHealthSaveKey = "MainCharacterHealth";

	// Token: 0x0400084B RID: 2123
	private float levelStartTime = -0.1f;

	// Token: 0x0400084D RID: 2125
	private static Ruleset rule;

	// Token: 0x0400084E RID: 2126
	private static List<object> waitForInitializationList = new List<object>();

	// Token: 0x0400084F RID: 2127
	public static float enemySpawnCountFactor = 1f;

	// Token: 0x04000850 RID: 2128
	public static bool forceBossSpawn = false;

	// Token: 0x04000851 RID: 2129
	private bool dieTask;

	// Token: 0x02000492 RID: 1170
	[Serializable]
	public struct LevelInfo
	{
		// Token: 0x04001BED RID: 7149
		public bool isBaseLevel;

		// Token: 0x04001BEE RID: 7150
		public string sceneName;

		// Token: 0x04001BEF RID: 7151
		public string activeSubSceneID;
	}
}
