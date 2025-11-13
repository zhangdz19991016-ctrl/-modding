using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000352 RID: 850
	public class QuestViewDetails : MonoBehaviour
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0006A460 File Offset: 0x00068660
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x0006A468 File Offset: 0x00068668
		// (set) Token: 0x06001D93 RID: 7571 RVA: 0x0006A470 File Offset: 0x00068670
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x0006A47C File Offset: 0x0006867C
		private PrefabPool<TaskEntry> TaskEntryPool
		{
			get
			{
				if (this._taskEntryPool == null)
				{
					this._taskEntryPool = new PrefabPool<TaskEntry>(this.taskEntryPrefab, this.tasksParent, null, null, null, true, 10, 10000, null);
				}
				return this._taskEntryPool;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x0006A4BC File Offset: 0x000686BC
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntry, this.rewardsParent, null, null, null, true, 10, 10000, null);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x0006A4FA File Offset: 0x000686FA
		private void Awake()
		{
			this.rewardEntry.gameObject.SetActive(false);
			this.taskEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0006A51E File Offset: 0x0006871E
		internal void Refresh()
		{
			this.RefreshAsync().Forget();
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0006A52C File Offset: 0x0006872C
		private int GetNewToken()
		{
			int num;
			for (num = this.activeTaskToken; num == this.activeTaskToken; num = UnityEngine.Random.Range(1, int.MaxValue))
			{
			}
			this.activeTaskToken = num;
			return this.activeTaskToken;
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0006A564 File Offset: 0x00068764
		private UniTask RefreshAsync()
		{
			QuestViewDetails.<RefreshAsync>d__28 <RefreshAsync>d__;
			<RefreshAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RefreshAsync>d__.<>4__this = this;
			<RefreshAsync>d__.<>1__state = -1;
			<RefreshAsync>d__.<>t__builder.Start<QuestViewDetails.<RefreshAsync>d__28>(ref <RefreshAsync>d__);
			return <RefreshAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x0006A5A8 File Offset: 0x000687A8
		private void SetupTasks()
		{
			this.TaskEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Task task in this.target.tasks)
			{
				TaskEntry taskEntry = this.TaskEntryPool.Get(this.tasksParent);
				taskEntry.Interactable = this.Interactable;
				taskEntry.Setup(task);
				taskEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x0006A644 File Offset: 0x00068844
		private void SetupRewards()
		{
			this.RewardEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Reward x in this.target.rewards)
			{
				if (x == null)
				{
					Debug.LogError(string.Format("任务 {0} - {1} 中包含值为 null 的奖励。", this.target.ID, this.target.DisplayName));
				}
				else
				{
					RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardsParent);
					rewardEntry.Interactable = this.Interactable;
					rewardEntry.Setup(x);
					rewardEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0006A714 File Offset: 0x00068914
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x0006A73C File Offset: 0x0006893C
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0006A764 File Offset: 0x00068964
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0006A76C File Offset: 0x0006896C
		internal void Setup(Quest quest)
		{
			this.target = quest;
			this.Refresh();
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0006A77B File Offset: 0x0006897B
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x04001477 RID: 5239
		private Quest target;

		// Token: 0x04001478 RID: 5240
		[SerializeField]
		private TaskEntry taskEntryPrefab;

		// Token: 0x04001479 RID: 5241
		[SerializeField]
		private RewardEntry rewardEntry;

		// Token: 0x0400147A RID: 5242
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x0400147B RID: 5243
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x0400147C RID: 5244
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x0400147D RID: 5245
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x0400147E RID: 5246
		[SerializeField]
		private TextMeshProUGUI questGiverDisplayName;

		// Token: 0x0400147F RID: 5247
		[SerializeField]
		private Transform tasksParent;

		// Token: 0x04001480 RID: 5248
		[SerializeField]
		private Transform rewardsParent;

		// Token: 0x04001481 RID: 5249
		[SerializeField]
		private QuestRequiredItem requiredItem;

		// Token: 0x04001482 RID: 5250
		[SerializeField]
		private bool interactable;

		// Token: 0x04001483 RID: 5251
		private PrefabPool<TaskEntry> _taskEntryPool;

		// Token: 0x04001484 RID: 5252
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x04001485 RID: 5253
		private Quest showingQuest;

		// Token: 0x04001486 RID: 5254
		private int activeTaskToken;
	}
}
