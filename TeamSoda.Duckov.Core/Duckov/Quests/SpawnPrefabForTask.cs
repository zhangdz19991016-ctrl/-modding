using System;
using System.Collections.Generic;
using Duckov.Quests.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x02000345 RID: 837
	public class SpawnPrefabForTask : MonoBehaviour
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00068BEF File Offset: 0x00066DEF
		private Task task
		{
			get
			{
				if (this._taskCache == null)
				{
					this._taskCache = base.GetComponent<Task>();
				}
				return this._taskCache;
			}
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00068C11 File Offset: 0x00066E11
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x00068C35 File Offset: 0x00066E35
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x00068C3D File Offset: 0x00066E3D
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x00068C61 File Offset: 0x00066E61
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning prefabs for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x00068C73 File Offset: 0x00066E73
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00068C7C File Offset: 0x00066E7C
		private void SpawnIfNeeded()
		{
			if (this.prefab == null)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.LogWarning("未配置Task");
				return;
			}
			if (this.task.IsFinished())
			{
				return;
			}
			if (this.IsSpawned())
			{
				return;
			}
			this.Spawn();
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x00068CD0 File Offset: 0x00066ED0
		private int SpawnKey
		{
			get
			{
				return string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					"SpawnPrefabForTask",
					this.task.Master.ID,
					this.task.ID,
					this.componentID
				}).GetHashCode();
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x00068D30 File Offset: 0x00066F30
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x00068D84 File Offset: 0x00066F84
		private void Spawn()
		{
			Vector3 position;
			if (!this.locations.GetRandom<MultiSceneLocation>().TryGetLocationPosition(out position))
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, position, Quaternion.identity);
			QuestTask_TaskEvent questTask_TaskEvent = this.task as QuestTask_TaskEvent;
			if (questTask_TaskEvent)
			{
				TaskEventEmitter component = gameObject.GetComponent<TaskEventEmitter>();
				if (component)
				{
					component.SetKey(questTask_TaskEvent.EventKey);
				}
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.MoveToActiveWithScene(gameObject, base.transform.gameObject.scene.buildIndex);
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
		}

		// Token: 0x0400141F RID: 5151
		[SerializeField]
		private string componentID = "SpawnPrefabForTask";

		// Token: 0x04001420 RID: 5152
		private Task _taskCache;

		// Token: 0x04001421 RID: 5153
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x04001422 RID: 5154
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04001423 RID: 5155
		private bool spawned;
	}
}
