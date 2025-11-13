using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x0200012C RID: 300
public class SceneLoader : MonoBehaviour
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x0002AA11 File Offset: 0x00028C11
	public static SceneLoader Instance
	{
		get
		{
			return GameManager.SceneLoader;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x0002AA18 File Offset: 0x00028C18
	// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0002AA1F File Offset: 0x00028C1F
	public static bool IsSceneLoading { get; private set; }

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060009D1 RID: 2513 RVA: 0x0002AA28 File Offset: 0x00028C28
	// (remove) Token: 0x060009D2 RID: 2514 RVA: 0x0002AA5C File Offset: 0x00028C5C
	public static event Action<SceneLoadingContext> onStartedLoadingScene;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060009D3 RID: 2515 RVA: 0x0002AA90 File Offset: 0x00028C90
	// (remove) Token: 0x060009D4 RID: 2516 RVA: 0x0002AAC4 File Offset: 0x00028CC4
	public static event Action<SceneLoadingContext> onFinishedLoadingScene;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060009D5 RID: 2517 RVA: 0x0002AAF8 File Offset: 0x00028CF8
	// (remove) Token: 0x060009D6 RID: 2518 RVA: 0x0002AB2C File Offset: 0x00028D2C
	public static event Action<SceneLoadingContext> onBeforeSetSceneActive;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060009D7 RID: 2519 RVA: 0x0002AB60 File Offset: 0x00028D60
	// (remove) Token: 0x060009D8 RID: 2520 RVA: 0x0002AB94 File Offset: 0x00028D94
	public static event Action<SceneLoadingContext> onAfterSceneInitialize;

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002ABC7 File Offset: 0x00028DC7
	// (set) Token: 0x060009DA RID: 2522 RVA: 0x0002ABEF File Offset: 0x00028DEF
	public static string LoadingComment
	{
		get
		{
			if (LevelManager.LevelInitializing)
			{
				return LevelManager.LevelInitializingComment;
			}
			if (SceneLoader.Instance != null)
			{
				return SceneLoader.Instance._loadingComment;
			}
			return null;
		}
		set
		{
			if (SceneLoader.Instance == null)
			{
				return;
			}
			SceneLoader.Instance._loadingComment = value;
			Action<string> onSetLoadingComment = SceneLoader.OnSetLoadingComment;
			if (onSetLoadingComment == null)
			{
				return;
			}
			onSetLoadingComment(value);
		}
	}

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060009DB RID: 2523 RVA: 0x0002AC1C File Offset: 0x00028E1C
	// (remove) Token: 0x060009DC RID: 2524 RVA: 0x0002AC50 File Offset: 0x00028E50
	public static event Action<string> OnSetLoadingComment;

	// Token: 0x060009DD RID: 2525 RVA: 0x0002AC84 File Offset: 0x00028E84
	private void Awake()
	{
		if (SceneLoader.Instance != this)
		{
			Debug.LogError(base.gameObject.scene.name + " 场景中出现了应当删除的Scene Loader");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.pointerClickEventRecevier.onPointerClick.AddListener(new UnityAction<PointerEventData>(this.NotifyPointerClick));
		this.pointerClickEventRecevier.gameObject.SetActive(false);
		this.content.Hide();
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0002AD04 File Offset: 0x00028F04
	public UniTask LoadScene(string sceneID, MultiSceneLocation location, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__39 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.location = location;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__39>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0002AD8C File Offset: 0x00028F8C
	public UniTask LoadScene(string sceneID, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__40 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__40>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0002AE1D File Offset: 0x0002901D
	// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0002AE24 File Offset: 0x00029024
	public static bool HideTips { get; private set; }

	// Token: 0x060009E2 RID: 2530 RVA: 0x0002AE2C File Offset: 0x0002902C
	public UniTask LoadScene(SceneReference sceneReference, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__45 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneReference = sceneReference;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__45>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0002AEC0 File Offset: 0x000290C0
	public void LoadTarget()
	{
		this.LoadScene(this.target, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0002AEF0 File Offset: 0x000290F0
	public UniTask LoadBaseScene(SceneReference overrideCurtainScene = null, bool doCircleFade = true)
	{
		SceneLoader.<LoadBaseScene>d__47 <LoadBaseScene>d__;
		<LoadBaseScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadBaseScene>d__.<>4__this = this;
		<LoadBaseScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadBaseScene>d__.doCircleFade = doCircleFade;
		<LoadBaseScene>d__.<>1__state = -1;
		<LoadBaseScene>d__.<>t__builder.Start<SceneLoader.<LoadBaseScene>d__47>(ref <LoadBaseScene>d__);
		return <LoadBaseScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x0002AF43 File Offset: 0x00029143
	public void NotifyPointerClick(PointerEventData eventData)
	{
		this.clicked = true;
		AudioManager.Post("UI/sceneloader_click");
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0002AF57 File Offset: 0x00029157
	internal static void StaticLoadSingle(SceneReference sceneReference)
	{
		SceneManager.LoadScene(sceneReference.Name, LoadSceneMode.Single);
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0002AF65 File Offset: 0x00029165
	internal static void StaticLoadSingle(string sceneID)
	{
		SceneManager.LoadScene(SceneInfoCollection.GetBuildIndex(sceneID), LoadSceneMode.Single);
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0002AF74 File Offset: 0x00029174
	public static void LoadMainMenu(bool circleFade = true)
	{
		if (SceneLoader.Instance)
		{
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.MainMenuScene, null, false, false, circleFade, false, default(MultiSceneLocation), true, false).Forget();
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0002AFD4 File Offset: 0x000291D4
	[CompilerGenerated]
	internal static float <LoadScene>g__TimeSinceLoadingStarted|45_0(ref SceneLoader.<>c__DisplayClass45_0 A_0)
	{
		return Time.unscaledTime - A_0.timeWhenLoadingStarted;
	}

	// Token: 0x0400089F RID: 2207
	public SceneReference defaultCurtainScene;

	// Token: 0x040008A0 RID: 2208
	[SerializeField]
	private OnPointerClick pointerClickEventRecevier;

	// Token: 0x040008A1 RID: 2209
	[SerializeField]
	private float minimumLoadingTime = 1f;

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private float waitAfterSceneLoaded = 1f;

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private FadeGroup content;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private FadeGroup clickIndicator;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private AnimationCurve fadeCurve1;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private AnimationCurve fadeCurve2;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private AnimationCurve fadeCurve3;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private AnimationCurve fadeCurve4;

	// Token: 0x040008AF RID: 2223
	private string _loadingComment;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private SceneReference target;

	// Token: 0x040008B2 RID: 2226
	private bool clicked;
}
