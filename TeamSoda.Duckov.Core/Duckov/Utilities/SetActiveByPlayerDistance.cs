using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Utilities
{
	// Token: 0x02000402 RID: 1026
	public class SetActiveByPlayerDistance : MonoBehaviour
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x000814D6 File Offset: 0x0007F6D6
		// (set) Token: 0x0600254A RID: 9546 RVA: 0x000814DD File Offset: 0x0007F6DD
		public static SetActiveByPlayerDistance Instance { get; private set; }

		// Token: 0x0600254B RID: 9547 RVA: 0x000814E8 File Offset: 0x0007F6E8
		private static List<GameObject> GetListByScene(int sceneBuildIndex, bool createIfNotExist = true)
		{
			List<GameObject> result;
			if (SetActiveByPlayerDistance.listsOfScenes.TryGetValue(sceneBuildIndex, out result))
			{
				return result;
			}
			if (createIfNotExist)
			{
				List<GameObject> list = new List<GameObject>();
				SetActiveByPlayerDistance.listsOfScenes[sceneBuildIndex] = list;
				return list;
			}
			return null;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x0008151E File Offset: 0x0007F71E
		private static List<GameObject> GetListByScene(Scene scene, bool createIfNotExist = true)
		{
			return SetActiveByPlayerDistance.GetListByScene(scene.buildIndex, createIfNotExist);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x0008152D File Offset: 0x0007F72D
		public static void Register(GameObject gameObject, int sceneBuildIndex)
		{
			SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, true).Add(gameObject);
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x0008153C File Offset: 0x0007F73C
		public static bool Unregister(GameObject gameObject, int sceneBuildIndex)
		{
			List<GameObject> listByScene = SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, false);
			return listByScene != null && listByScene.Remove(gameObject);
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x0008155D File Offset: 0x0007F75D
		public static void Register(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Register(gameObject, scene.buildIndex);
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x0008156C File Offset: 0x0007F76C
		public static void Unregister(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Unregister(gameObject, scene.buildIndex);
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x0008157C File Offset: 0x0007F77C
		public float Distance
		{
			get
			{
				return this.distance;
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00081584 File Offset: 0x0007F784
		private void Awake()
		{
			if (SetActiveByPlayerDistance.Instance == null)
			{
				SetActiveByPlayerDistance.Instance = this;
			}
			this.CleanUp();
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.RefreshCache();
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000815C4 File Offset: 0x0007F7C4
		private void CleanUp()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, List<GameObject>> keyValuePair in SetActiveByPlayerDistance.listsOfScenes)
			{
				List<GameObject> value = keyValuePair.Value;
				value.RemoveAll((GameObject e) => e == null);
				if (value == null || value.Count < 1)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (int key in list)
			{
				SetActiveByPlayerDistance.listsOfScenes.Remove(key);
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000816A4 File Offset: 0x0007F8A4
		private void OnActiveSceneChanged(Scene prev, Scene cur)
		{
			this.RefreshCache();
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000816AC File Offset: 0x0007F8AC
		private void RefreshCache()
		{
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.cachedListRef = SetActiveByPlayerDistance.GetListByScene(this.cachedActiveScene, true);
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x000816CB File Offset: 0x0007F8CB
		private Transform PlayerTransform
		{
			get
			{
				if (!this.cachedPlayerTransform)
				{
					CharacterMainControl main = CharacterMainControl.Main;
					this.cachedPlayerTransform = ((main != null) ? main.transform : null);
				}
				return this.cachedPlayerTransform;
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000816F8 File Offset: 0x0007F8F8
		private void FixedUpdate()
		{
			if (this.PlayerTransform == null)
			{
				return;
			}
			if (this.cachedListRef == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.cachedListRef)
			{
				if (!(gameObject == null))
				{
					bool active = (this.PlayerTransform.position - gameObject.transform.position).sqrMagnitude < this.distance * this.distance;
					gameObject.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000817A4 File Offset: 0x0007F9A4
		private void DebugRegister(GameObject go)
		{
			SetActiveByPlayerDistance.Register(go, go.gameObject.scene);
		}

		// Token: 0x04001964 RID: 6500
		private static Dictionary<int, List<GameObject>> listsOfScenes = new Dictionary<int, List<GameObject>>();

		// Token: 0x04001965 RID: 6501
		[SerializeField]
		private float distance = 100f;

		// Token: 0x04001966 RID: 6502
		private Scene cachedActiveScene;

		// Token: 0x04001967 RID: 6503
		private List<GameObject> cachedListRef;

		// Token: 0x04001968 RID: 6504
		private Transform cachedPlayerTransform;
	}
}
