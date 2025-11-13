using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x02000344 RID: 836
	public class SpawnItemForTask : MonoBehaviour
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x00068944 File Offset: 0x00066B44
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

		// Token: 0x06001CEC RID: 7404 RVA: 0x00068966 File Offset: 0x00066B66
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0006898A File Offset: 0x00066B8A
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00068992 File Offset: 0x00066B92
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000689B6 File Offset: 0x00066BB6
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning item for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000689C8 File Offset: 0x00066BC8
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000689D0 File Offset: 0x00066BD0
		private void SpawnIfNeeded()
		{
			if (this.itemID < 0)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.Log("spawn item task is null");
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

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x00068A20 File Offset: 0x00066C20
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

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00068A80 File Offset: 0x00066C80
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00068AD4 File Offset: 0x00066CD4
		private void Spawn()
		{
			MultiSceneLocation random = this.locations.GetRandom<MultiSceneLocation>();
			Vector3 pos;
			if (!random.TryGetLocationPosition(out pos))
			{
				return;
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
			this.SpawnItem(pos, base.transform.gameObject.scene, random).Forget();
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00068B48 File Offset: 0x00066D48
		private UniTaskVoid SpawnItem(Vector3 pos, Scene scene, MultiSceneLocation location)
		{
			SpawnItemForTask.<SpawnItem>d__18 <SpawnItem>d__;
			<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SpawnItem>d__.<>4__this = this;
			<SpawnItem>d__.pos = pos;
			<SpawnItem>d__.location = location;
			<SpawnItem>d__.<>1__state = -1;
			<SpawnItem>d__.<>t__builder.Start<SpawnItemForTask.<SpawnItem>d__18>(ref <SpawnItem>d__);
			return <SpawnItem>d__.<>t__builder.Task;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00068B9B File Offset: 0x00066D9B
		private void OnItemTreeChanged(Item selfItem)
		{
			if (this.mapElement && selfItem.ParentItem)
			{
				this.mapElement.SetVisibility(false);
				selfItem.onItemTreeChanged -= this.OnItemTreeChanged;
			}
		}

		// Token: 0x04001419 RID: 5145
		[SerializeField]
		private string componentID = "SpawnItemForTask";

		// Token: 0x0400141A RID: 5146
		private Task _taskCache;

		// Token: 0x0400141B RID: 5147
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x0400141C RID: 5148
		[ItemTypeID]
		[SerializeField]
		private int itemID = -1;

		// Token: 0x0400141D RID: 5149
		[SerializeField]
		private MapElementForTask mapElement;

		// Token: 0x0400141E RID: 5150
		private bool spawned;
	}
}
