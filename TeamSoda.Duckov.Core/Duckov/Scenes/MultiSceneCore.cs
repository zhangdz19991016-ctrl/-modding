using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x02000330 RID: 816
	public class MultiSceneCore : MonoBehaviour
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x00064F8C File Offset: 0x0006318C
		// (set) Token: 0x06001BA9 RID: 7081 RVA: 0x00064F93 File Offset: 0x00063193
		public static MultiSceneCore Instance { get; private set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x00064F9B File Offset: 0x0006319B
		public List<SubSceneEntry> SubScenes
		{
			get
			{
				return this.subScenes;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00064FA4 File Offset: 0x000631A4
		public static Scene? MainScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.gameObject.scene);
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x00064FDC File Offset: 0x000631DC
		public static string ActiveSubSceneID
		{
			get
			{
				if (MultiSceneCore.ActiveSubScene == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				SubSceneEntry subSceneEntry = MultiSceneCore.Instance.SubScenes.Find((SubSceneEntry e) => e != null && MultiSceneCore.ActiveSubScene.Value.buildIndex == e.Info.BuildIndex);
				if (subSceneEntry == null)
				{
					return null;
				}
				return subSceneEntry.sceneID;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x00065044 File Offset: 0x00063244
		public static Scene? ActiveSubScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance.isLoading)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.activeSubScene);
			}
		}

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06001BAE RID: 7086 RVA: 0x00065090 File Offset: 0x00063290
		// (remove) Token: 0x06001BAF RID: 7087 RVA: 0x000650C4 File Offset: 0x000632C4
		public static event Action<MultiSceneCore, Scene> OnSubSceneWillBeUnloaded;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06001BB0 RID: 7088 RVA: 0x000650F8 File Offset: 0x000632F8
		// (remove) Token: 0x06001BB1 RID: 7089 RVA: 0x0006512C File Offset: 0x0006332C
		public static event Action<MultiSceneCore, Scene> OnSubSceneLoaded;

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x00065160 File Offset: 0x00063360
		public SceneInfoEntry SceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x00065188 File Offset: 0x00063388
		public string DisplayName
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayName;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000651C0 File Offset: 0x000633C0
		public string DisplaynameRaw
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayNameRaw;
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000651F8 File Offset: 0x000633F8
		public static void MoveToActiveWithScene(GameObject go, int sceneBuildIndex)
		{
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Transform setActiveWithSceneParent = MultiSceneCore.Instance.GetSetActiveWithSceneParent(sceneBuildIndex);
			go.transform.SetParent(setActiveWithSceneParent);
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0006522C File Offset: 0x0006342C
		public static void MoveToActiveWithScene(GameObject go)
		{
			int buildIndex = go.scene.buildIndex;
			MultiSceneCore.MoveToActiveWithScene(go, buildIndex);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00065250 File Offset: 0x00063450
		public Transform GetSetActiveWithSceneParent(int sceneBuildIndex)
		{
			GameObject gameObject;
			if (this.setActiveWithSceneObjects.TryGetValue(sceneBuildIndex, out gameObject))
			{
				return gameObject.transform;
			}
			SceneInfoEntry sceneInfoEntry = SceneInfoCollection.GetSceneInfo(sceneBuildIndex);
			if (sceneInfoEntry == null)
			{
				sceneInfoEntry = new SceneInfoEntry();
				Debug.LogWarning(string.Format("BuildIndex {0} 的sceneInfo不存在", sceneBuildIndex));
			}
			GameObject gameObject2 = new GameObject(sceneInfoEntry.ID);
			gameObject2.transform.SetParent(base.transform);
			this.setActiveWithSceneObjects.Add(sceneBuildIndex, gameObject2);
			gameObject2.SetActive(sceneInfoEntry.IsLoaded);
			return gameObject2.transform;
		}

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06001BB8 RID: 7096 RVA: 0x000652D8 File Offset: 0x000634D8
		// (remove) Token: 0x06001BB9 RID: 7097 RVA: 0x0006530C File Offset: 0x0006350C
		public static event Action<MultiSceneCore> OnInstanceAwake;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06001BBA RID: 7098 RVA: 0x00065340 File Offset: 0x00063540
		// (remove) Token: 0x06001BBB RID: 7099 RVA: 0x00065374 File Offset: 0x00063574
		public static event Action<MultiSceneCore> OnInstanceDestroy;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06001BBC RID: 7100 RVA: 0x000653A8 File Offset: 0x000635A8
		// (remove) Token: 0x06001BBD RID: 7101 RVA: 0x000653DC File Offset: 0x000635DC
		public static event Action<string> OnSetSceneVisited;

		// Token: 0x06001BBE RID: 7102 RVA: 0x00065410 File Offset: 0x00063610
		private void Awake()
		{
			if (MultiSceneCore.Instance == null)
			{
				MultiSceneCore.Instance = this;
			}
			else
			{
				Debug.LogError("Multiple Multi Scene Core detected!");
			}
			Action<MultiSceneCore> onInstanceAwake = MultiSceneCore.OnInstanceAwake;
			if (onInstanceAwake != null)
			{
				onInstanceAwake(this);
			}
			if (this.playAfterLevelInit)
			{
				if (LevelManager.AfterInit)
				{
					this.PlayStinger();
					return;
				}
				LevelManager.OnAfterLevelInitialized += this.OnAfterLevelInitialized;
			}
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x00065474 File Offset: 0x00063674
		private void OnDestroy()
		{
			Action<MultiSceneCore> onInstanceDestroy = MultiSceneCore.OnInstanceDestroy;
			if (onInstanceDestroy != null)
			{
				onInstanceDestroy(this);
			}
			LevelManager.OnAfterLevelInitialized -= this.OnAfterLevelInitialized;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x00065498 File Offset: 0x00063698
		private void OnAfterLevelInitialized()
		{
			if (this.playAfterLevelInit)
			{
				this.PlayStinger();
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000654A8 File Offset: 0x000636A8
		public void PlayStinger()
		{
			if (!string.IsNullOrWhiteSpace(this.playStinger))
			{
				AudioManager.PlayStringer(this.playStinger);
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000654C4 File Offset: 0x000636C4
		private void Start()
		{
			this.CreatePointsOfInterestsForLocations();
			AudioManager.StopBGM();
			AudioManager.SetState("Level", this.levelStateName);
			if (this.SceneInfo != null && !string.IsNullOrEmpty(this.SceneInfo.ID))
			{
				MultiSceneCore.SetVisited(this.SceneInfo.ID);
			}
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00065516 File Offset: 0x00063716
		public static void SetVisited(string sceneID)
		{
			SavesSystem.Save<bool>("MultiSceneCore_Visited_" + sceneID, true);
			Action<string> onSetSceneVisited = MultiSceneCore.OnSetSceneVisited;
			if (onSetSceneVisited == null)
			{
				return;
			}
			onSetSceneVisited(sceneID);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00065539 File Offset: 0x00063739
		public static bool GetVisited(string sceneID)
		{
			return SavesSystem.Load<bool>("MultiSceneCore_Visited_" + sceneID);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0006554C File Offset: 0x0006374C
		private void CreatePointsOfInterestsForLocations()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
				{
					if (location.showInMap)
					{
						SimplePointOfInterest.Create(location.position, subSceneEntry.sceneID, location.DisplayNameRaw, null, true);
					}
				}
			}
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000655F8 File Offset: 0x000637F8
		private void CreatePointsOfInterestsForTeleporters()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					SimplePointOfInterest.Create(teleporterInfo.position, subSceneEntry.sceneID, "", GameplayDataSettings.UIStyle.DefaultTeleporterIcon, false).ScaleFactor = GameplayDataSettings.UIStyle.TeleporterIconScale;
				}
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000656B0 File Offset: 0x000638B0
		public void BeginLoadSubScene(SceneReference reference)
		{
			this.LoadSubScene(reference, true).Forget<bool>();
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000656BF File Offset: 0x000638BF
		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000656C8 File Offset: 0x000638C8
		public static string MainSceneID
		{
			get
			{
				return SceneInfoCollection.GetSceneID(MultiSceneCore.MainScene.Value.buildIndex);
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000656F0 File Offset: 0x000638F0
		private SceneReference GetSubSceneReference(string sceneID)
		{
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e.sceneID == sceneID);
			if (subSceneEntry == null)
			{
				return null;
			}
			return subSceneEntry.SceneReference;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x00065730 File Offset: 0x00063930
		private UniTask<bool> LoadSubScene(SceneReference targetScene, bool withBlackScreen = true)
		{
			MultiSceneCore.<LoadSubScene>d__62 <LoadSubScene>d__;
			<LoadSubScene>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadSubScene>d__.<>4__this = this;
			<LoadSubScene>d__.targetScene = targetScene;
			<LoadSubScene>d__.withBlackScreen = withBlackScreen;
			<LoadSubScene>d__.<>1__state = -1;
			<LoadSubScene>d__.<>t__builder.Start<MultiSceneCore.<LoadSubScene>d__62>(ref <LoadSubScene>d__);
			return <LoadSubScene>d__.<>t__builder.Task;
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00065784 File Offset: 0x00063984
		private void LocalOnSubSceneWillBeUnloaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			Transform setActiveWithSceneParent = this.GetSetActiveWithSceneParent(scene.buildIndex);
			Debug.Log(string.Format("Setting Active False {0}  {1}", setActiveWithSceneParent.name, scene.buildIndex));
			setActiveWithSceneParent.gameObject.SetActive(false);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000657FC File Offset: 0x000639FC
		private void LocalOnSubSceneLoaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			this.GetSetActiveWithSceneParent(scene.buildIndex).gameObject.SetActive(true);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0006584C File Offset: 0x00063A4C
		public UniTask<bool> LoadAndTeleport(MultiSceneLocation location)
		{
			MultiSceneCore.<LoadAndTeleport>d__65 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.location = location;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__65>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00065898 File Offset: 0x00063A98
		public UniTask<bool> LoadAndTeleport(string sceneID, Vector3 position, bool subSceneLocation = false)
		{
			MultiSceneCore.<LoadAndTeleport>d__66 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.sceneID = sceneID;
			<LoadAndTeleport>d__.position = position;
			<LoadAndTeleport>d__.subSceneLocation = subSceneLocation;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__66>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000658F4 File Offset: 0x00063AF4
		public static void MoveToMainScene(GameObject gameObject)
		{
			if (MultiSceneCore.Instance == null)
			{
				Debug.LogError("移动到主场景失败，因为MultiSceneCore不存在");
				return;
			}
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0006592C File Offset: 0x00063B2C
		public void CacheLocations()
		{
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0006592E File Offset: 0x00063B2E
		public void CacheTeleporters()
		{
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x00065930 File Offset: 0x00063B30
		private Vector3 GetClosestTeleporterPosition(Vector3 pos)
		{
			float num = float.MaxValue;
			Vector3 result = pos;
			foreach (SubSceneEntry subSceneEntry in this.subScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					float magnitude = (teleporterInfo.position - pos).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						result = teleporterInfo.position;
					}
				}
			}
			return result;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000659E8 File Offset: 0x00063BE8
		internal bool TryGetCachedPosition(MultiSceneLocation location, out Vector3 result)
		{
			return this.TryGetCachedPosition(location.SceneID, location.LocationName, out result);
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00065A00 File Offset: 0x00063C00
		internal bool TryGetCachedPosition(string sceneID, string locationName, out Vector3 result)
		{
			result = default(Vector3);
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e != null && e.sceneID == sceneID);
			return subSceneEntry != null && subSceneEntry.TryGetCachedPosition(locationName, out result);
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00065A4C File Offset: 0x00063C4C
		internal SubSceneEntry GetSubSceneInfo(Scene scene)
		{
			return this.subScenes.Find((SubSceneEntry e) => e != null && e.Info != null && e.Info.BuildIndex == scene.buildIndex);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00065A7D File Offset: 0x00063C7D
		public SubSceneEntry GetSubSceneInfo()
		{
			return this.cachedSubsceneEntry;
		}

		// Token: 0x040013A9 RID: 5033
		[SerializeField]
		private string levelStateName = "None";

		// Token: 0x040013AA RID: 5034
		[SerializeField]
		private string playStinger = "";

		// Token: 0x040013AB RID: 5035
		[SerializeField]
		private bool playAfterLevelInit;

		// Token: 0x040013AC RID: 5036
		[SerializeField]
		private List<SubSceneEntry> subScenes;

		// Token: 0x040013AD RID: 5037
		private Scene activeSubScene;

		// Token: 0x040013AE RID: 5038
		[HideInInspector]
		public List<int> usedCreatorIds = new List<int>();

		// Token: 0x040013AF RID: 5039
		[HideInInspector]
		public Dictionary<int, object> inLevelData = new Dictionary<int, object>();

		// Token: 0x040013B2 RID: 5042
		[SerializeField]
		private bool teleportToRandomOnLevelInitialized;

		// Token: 0x040013B3 RID: 5043
		private Dictionary<int, GameObject> setActiveWithSceneObjects = new Dictionary<int, GameObject>();

		// Token: 0x040013B7 RID: 5047
		private bool isLoading;

		// Token: 0x040013B8 RID: 5048
		private SubSceneEntry cachedSubsceneEntry;
	}
}
