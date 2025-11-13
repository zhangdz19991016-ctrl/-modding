using System;
using DG.Tweening;
using Duckov;
using Duckov.Achievements;
using Duckov.Modding;
using Duckov.NoteIndexs;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

// Token: 0x0200009C RID: 156
public class GameManager : MonoBehaviour
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00017A88 File Offset: 0x00015C88
	public static GameManager Instance
	{
		get
		{
			if (!Application.isPlaying)
			{
				return null;
			}
			if (GameManager._instance == null)
			{
				GameManager._instance = UnityEngine.Object.FindObjectOfType<GameManager>();
				if (GameManager._instance)
				{
					UnityEngine.Object.DontDestroyOnLoad(GameManager._instance.gameObject);
				}
			}
			if (GameManager._instance == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("GameManager");
				if (gameObject == null)
				{
					Debug.LogError("Resources中找不到GameManager的Prefab");
				}
				GameManager component = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<GameManager>();
				if (component == null)
				{
					Debug.LogError("GameManager的prefab上没有GameManager组件");
					return null;
				}
				GameManager._instance = component;
				if (GameManager._instance)
				{
					UnityEngine.Object.DontDestroyOnLoad(GameManager._instance.gameObject);
				}
			}
			return GameManager._instance;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x00017B40 File Offset: 0x00015D40
	public static bool Paused
	{
		get
		{
			return !(GameManager.Instance == null) && GameManager.Instance.pauseMenu.Shown;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x00017B65 File Offset: 0x00015D65
	public static AudioManager AudioManager
	{
		get
		{
			return GameManager.Instance.audioManager;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x00017B71 File Offset: 0x00015D71
	public static UIInputManager UiInputManager
	{
		get
		{
			return GameManager.Instance.uiInputManager;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x00017B7D File Offset: 0x00015D7D
	public static PauseMenu PauseMenu
	{
		get
		{
			return GameManager.Instance.pauseMenu;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x00017B89 File Offset: 0x00015D89
	public static GameRulesManager DifficultyManager
	{
		get
		{
			return GameManager.Instance.difficultyManager;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x00017B95 File Offset: 0x00015D95
	public static SceneLoader SceneLoader
	{
		get
		{
			return GameManager.Instance.sceneLoader;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000540 RID: 1344 RVA: 0x00017BA1 File Offset: 0x00015DA1
	public static BlackScreen BlackScreen
	{
		get
		{
			return GameManager.Instance.blackScreen;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x00017BAD File Offset: 0x00015DAD
	public static EventSystem EventSystem
	{
		get
		{
			return GameManager.Instance.eventSystem;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x00017BB9 File Offset: 0x00015DB9
	public static NightVisionVisual NightVision
	{
		get
		{
			return GameManager.Instance.nightVision;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x00017BC5 File Offset: 0x00015DC5
	public static bool BloodFxOn
	{
		get
		{
			return GameMetaData.BloodFxOn;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00017BCC File Offset: 0x00015DCC
	public static PlayerInput MainPlayerInput
	{
		get
		{
			return GameManager.Instance.mainPlayerInput;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00017BD8 File Offset: 0x00015DD8
	public static ModManager ModManager
	{
		get
		{
			return GameManager.Instance.modManager;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00017BE4 File Offset: 0x00015DE4
	public static NoteIndex NoteIndex
	{
		get
		{
			return GameManager.Instance.noteIndex;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000547 RID: 1351 RVA: 0x00017BF0 File Offset: 0x00015DF0
	public static AchievementManager AchievementManager
	{
		get
		{
			return GameManager.Instance.achievementManager;
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00017BFC File Offset: 0x00015DFC
	private void Awake()
	{
		if (GameManager._instance == null)
		{
			GameManager._instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (GameManager._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		DOTween.defaultTimeScaleIndependent = true;
		DebugManager.instance.enableRuntimeUI = false;
		DebugManager.instance.displayRuntimeUI = false;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00017C5D File Offset: 0x00015E5D
	private void Update()
	{
		bool isEditor = Application.isEditor;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00017C65 File Offset: 0x00015E65
	public static void TimeTravelDetected()
	{
		Debug.Log("检测到穿越者");
	}

	// Token: 0x040004B2 RID: 1202
	private static GameManager _instance;

	// Token: 0x040004B3 RID: 1203
	[SerializeField]
	private AudioManager audioManager;

	// Token: 0x040004B4 RID: 1204
	[SerializeField]
	private UIInputManager uiInputManager;

	// Token: 0x040004B5 RID: 1205
	[SerializeField]
	private GameRulesManager difficultyManager;

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private PauseMenu pauseMenu;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	private SceneLoader sceneLoader;

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	private BlackScreen blackScreen;

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	private EventSystem eventSystem;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	private PlayerInput mainPlayerInput;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private NightVisionVisual nightVision;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private ModManager modManager;

	// Token: 0x040004BD RID: 1213
	[SerializeField]
	private NoteIndex noteIndex;

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private AchievementManager achievementManager;

	// Token: 0x040004BF RID: 1215
	public static bool newBoot;
}
