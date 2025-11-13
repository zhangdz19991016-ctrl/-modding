using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.Quests.Relations;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Duckov.Quests
{
	// Token: 0x02000338 RID: 824
	public class Quest : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00066178 File Offset: 0x00064378
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x00066188 File Offset: 0x00064388
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000661A7 File Offset: 0x000643A7
		public List<Task> Tasks
		{
			get
			{
				return this.tasks;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x000661AF File Offset: 0x000643AF
		public ReadOnlyCollection<Reward> Rewards
		{
			get
			{
				if (this._readonly_rewards == null)
				{
					this._readonly_rewards = new ReadOnlyCollection<Reward>(this.rewards);
				}
				return this._readonly_rewards;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x000661D0 File Offset: 0x000643D0
		public ReadOnlyCollection<Condition> Prerequisits
		{
			get
			{
				if (this.prerequisits_ReadOnly == null)
				{
					this.prerequisits_ReadOnly = new ReadOnlyCollection<Condition>(this.prerequisit);
				}
				return this.prerequisits_ReadOnly;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000661F4 File Offset: 0x000643F4
		public bool SceneRequirementSatisfied
		{
			get
			{
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty || requireScene.UnsafeReason != SceneReferenceUnsafeReason.None || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001C11 RID: 7185 RVA: 0x00066230 File Offset: 0x00064430
		public int RequireLevel
		{
			get
			{
				return this.requireLevel;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x00066238 File Offset: 0x00064438
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x00066240 File Offset: 0x00064440
		// (set) Token: 0x06001C14 RID: 7188 RVA: 0x00066248 File Offset: 0x00064448
		public bool Complete
		{
			get
			{
				return this.complete;
			}
			internal set
			{
				this.complete = value;
				Action<Quest> action = this.onStatusChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestStatusChanged;
				if (action2 != null)
				{
					action2(this);
				}
				if (this.complete)
				{
					Action<Quest> action3 = this.onCompleted;
					if (action3 != null)
					{
						action3(this);
					}
					UnityEvent onCompletedUnityEvent = this.OnCompletedUnityEvent;
					if (onCompletedUnityEvent != null)
					{
						onCompletedUnityEvent.Invoke();
					}
					Action<Quest> action4 = Quest.onQuestCompleted;
					if (action4 == null)
					{
						return;
					}
					action4(this);
				}
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000662BC File Offset: 0x000644BC
		// (set) Token: 0x06001C16 RID: 7190 RVA: 0x0006630C File Offset: 0x0006450C
		public bool NeedInspection
		{
			get
			{
				return (!this.Active && !QuestManager.EverInspected(this.ID)) || (this.Active && ((this.Active && this.AreTasksFinished()) || this.AnyTaskNeedInspection() || this.needInspection));
			}
			set
			{
				this.needInspection = value;
				Action<Quest> action = this.onNeedInspectionChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestNeedInspectionChanged;
				if (action2 == null)
				{
					return;
				}
				action2(this);
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00066338 File Offset: 0x00064538
		private bool AnyTaskNeedInspection()
		{
			if (this.tasks != null)
			{
				foreach (Task task in this.tasks)
				{
					if (!(task == null) && task.NeedInspection)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06001C18 RID: 7192 RVA: 0x000663A4 File Offset: 0x000645A4
		// (remove) Token: 0x06001C19 RID: 7193 RVA: 0x000663DC File Offset: 0x000645DC
		public event Action<Quest> onNeedInspectionChanged;

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06001C1A RID: 7194 RVA: 0x00066414 File Offset: 0x00064614
		// (remove) Token: 0x06001C1B RID: 7195 RVA: 0x0006644C File Offset: 0x0006464C
		internal event Action<Quest> onStatusChanged;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06001C1C RID: 7196 RVA: 0x00066484 File Offset: 0x00064684
		// (remove) Token: 0x06001C1D RID: 7197 RVA: 0x000664BC File Offset: 0x000646BC
		internal event Action<Quest> onActivated;

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06001C1E RID: 7198 RVA: 0x000664F4 File Offset: 0x000646F4
		// (remove) Token: 0x06001C1F RID: 7199 RVA: 0x0006652C File Offset: 0x0006472C
		internal event Action<Quest> onCompleted;

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06001C20 RID: 7200 RVA: 0x00066564 File Offset: 0x00064764
		// (remove) Token: 0x06001C21 RID: 7201 RVA: 0x00066598 File Offset: 0x00064798
		public static event Action<Quest> onQuestStatusChanged;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06001C22 RID: 7202 RVA: 0x000665CC File Offset: 0x000647CC
		// (remove) Token: 0x06001C23 RID: 7203 RVA: 0x00066600 File Offset: 0x00064800
		public static event Action<Quest> onQuestNeedInspectionChanged;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06001C24 RID: 7204 RVA: 0x00066634 File Offset: 0x00064834
		// (remove) Token: 0x06001C25 RID: 7205 RVA: 0x00066668 File Offset: 0x00064868
		public static event Action<Quest> onQuestActivated;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06001C26 RID: 7206 RVA: 0x0006669C File Offset: 0x0006489C
		// (remove) Token: 0x06001C27 RID: 7207 RVA: 0x000666D0 File Offset: 0x000648D0
		public static event Action<Quest> onQuestCompleted;

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00066703 File Offset: 0x00064903
		// (set) Token: 0x06001C29 RID: 7209 RVA: 0x0006670B File Offset: 0x0006490B
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00066714 File Offset: 0x00064914
		public bool Active
		{
			get
			{
				return QuestManager.IsQuestActive(this);
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x0006671C File Offset: 0x0006491C
		public int RequiredItemID
		{
			get
			{
				return this.requiredItemID;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x00066724 File Offset: 0x00064924
		public int RequiredItemCount
		{
			get
			{
				return this.requiredItemCount;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001C2D RID: 7213 RVA: 0x0006672C File Offset: 0x0006492C
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x00066739 File Offset: 0x00064939
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001C2F RID: 7215 RVA: 0x00066746 File Offset: 0x00064946
		// (set) Token: 0x06001C30 RID: 7216 RVA: 0x0006674E File Offset: 0x0006494E
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001C31 RID: 7217 RVA: 0x00066757 File Offset: 0x00064957
		// (set) Token: 0x06001C32 RID: 7218 RVA: 0x0006675F File Offset: 0x0006495F
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x00066768 File Offset: 0x00064968
		// (set) Token: 0x06001C34 RID: 7220 RVA: 0x00066770 File Offset: 0x00064970
		public QuestGiverID QuestGiverID
		{
			get
			{
				return this.questGiverID;
			}
			internal set
			{
				this.questGiverID = value;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x00066779 File Offset: 0x00064979
		public object FinishedTaskCount
		{
			get
			{
				return this.tasks.Count((Task e) => e.IsFinished());
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000667AC File Offset: 0x000649AC
		public bool MeetsPrerequisit()
		{
			if (this.RequireLevel > EXPManager.Level)
			{
				return false;
			}
			if (this.LockInDemo && GameMetaData.Instance.IsDemo)
			{
				return false;
			}
			QuestRelationGraph questRelation = GameplayDataSettings.QuestRelation;
			if (questRelation.GetNode(this.id) == null)
			{
				return false;
			}
			if (!QuestManager.AreQuestFinished(questRelation.GetRequiredIDs(this.id)))
			{
				return false;
			}
			using (List<Condition>.Enumerator enumerator = this.prerequisit.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Evaluate())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00066858 File Offset: 0x00064A58
		public bool AreTasksFinished()
		{
			foreach (Task task in this.tasks)
			{
				if (task == null)
				{
					Debug.LogError(string.Format("存在空的Task，QuestID：{0}", this.id));
				}
				else if (!task.IsFinished())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000668D8 File Offset: 0x00064AD8
		public void Initialize()
		{
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000668DA File Offset: 0x00064ADA
		public void OnValidate()
		{
			this.displayName = string.Format("Quest_{0}", this.id);
			this.description = string.Format("Quest_{0}_Desc", this.id);
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00066914 File Offset: 0x00064B14
		public object GenerateSaveData()
		{
			Quest.SaveData saveData = default(Quest.SaveData);
			saveData.id = this.id;
			saveData.complete = this.complete;
			saveData.needInspection = this.needInspection;
			saveData.taskStatus = new List<ValueTuple<int, object>>();
			saveData.rewardStatus = new List<ValueTuple<int, object>>();
			foreach (Task task in this.tasks)
			{
				int item = task.ID;
				object item2 = task.GenerateSaveData();
				if (!(task == null))
				{
					saveData.taskStatus.Add(new ValueTuple<int, object>(item, item2));
				}
			}
			foreach (Reward reward in this.rewards)
			{
				if (reward == null)
				{
					Debug.LogError(string.Format("Null Reward detected in quest {0}", this.id));
				}
				else
				{
					int item3 = reward.ID;
					object item4 = reward.GenerateSaveData();
					saveData.rewardStatus.Add(new ValueTuple<int, object>(item3, item4));
				}
			}
			return saveData;
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00066A60 File Offset: 0x00064C60
		public void SetupSaveData(object obj)
		{
			Quest.SaveData saveData = (Quest.SaveData)obj;
			if (saveData.id != this.id)
			{
				Debug.LogError("任务ID不匹配，加载失败");
				return;
			}
			this.complete = saveData.complete;
			this.needInspection = saveData.needInspection;
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.taskStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Task task = this.tasks.Find((Task e) => e.ID == cur.Item1);
					if (task == null)
					{
						Debug.LogWarning(string.Format("未找到Task {0}", cur.Item1));
					}
					else
					{
						task.SetupSaveData(cur.Item2);
					}
				}
			}
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.rewardStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Reward reward = this.rewards.Find((Reward e) => e.ID == cur.Item1);
					if (reward == null)
					{
						Debug.LogWarning(string.Format("未找到Reward {0}", cur.Item1));
					}
					else
					{
						reward.SetupSaveData(cur.Item2);
						reward.NotifyReload(this);
					}
				}
			}
			this.InitTasks();
			if (this.complete)
			{
				foreach (Reward reward2 in this.rewards)
				{
					if (!(reward2 == null) && !reward2.Claimed && reward2.AutoClaim)
					{
						reward2.Claim();
					}
				}
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00066C5C File Offset: 0x00064E5C
		internal void NotifyTaskFinished(Task task)
		{
			if (task.Master != this)
			{
				Debug.LogError("Task.Master 与 Quest不匹配");
				return;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			QuestManager.NotifyTaskFinished(this, task);
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00066CAC File Offset: 0x00064EAC
		internal void NotifyRewardClaimed(Reward reward)
		{
			if (reward.Master != this)
			{
				Debug.LogError("Reward.Master 与Quest 不匹配");
			}
			if (this.AreRewardsClaimed())
			{
				this.needInspection = false;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestNeedInspectionChanged;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00066D14 File Offset: 0x00064F14
		internal bool AreRewardsClaimed()
		{
			using (List<Reward>.Enumerator enumerator = this.rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Claimed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00066D70 File Offset: 0x00064F70
		internal void NotifyActivated()
		{
			this.InitTasks();
			Action<Quest> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onActivated;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestActivated;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00066DAC File Offset: 0x00064FAC
		private void InitTasks()
		{
			foreach (Task task in this.tasks)
			{
				task.Init();
			}
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00066DFC File Offset: 0x00064FFC
		public bool TryComplete()
		{
			if (this.Complete)
			{
				return false;
			}
			if (!this.AreTasksFinished())
			{
				return false;
			}
			this.Complete = true;
			return true;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00066E1A File Offset: 0x0006501A
		internal Quest.QuestInfo GetInfo()
		{
			return new Quest.QuestInfo(this);
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00066E24 File Offset: 0x00065024
		public static int Compare(Quest x, Quest y, Quest.SortingMode sortingMode, bool invert = false)
		{
			int num = 0;
			switch (sortingMode)
			{
			case Quest.SortingMode.ID:
				num = x.ID - y.ID;
				break;
			case Quest.SortingMode.Giver:
				num = x.QuestGiverID - y.QuestGiverID;
				break;
			case Quest.SortingMode.Location:
			{
				SceneInfoEntry requireSceneInfo = x.RequireSceneInfo;
				string x2 = (requireSceneInfo == null) ? "" : requireSceneInfo.ID;
				SceneInfoEntry requireSceneInfo2 = y.RequireSceneInfo;
				string y2 = (requireSceneInfo2 == null) ? "" : requireSceneInfo2.ID;
				num = StringComparer.CurrentCulture.Compare(x2, y2);
				break;
			}
			}
			if (invert)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x040013C8 RID: 5064
		[SerializeField]
		private int id;

		// Token: 0x040013C9 RID: 5065
		[LocalizationKey("Quests")]
		[SerializeField]
		private string displayName;

		// Token: 0x040013CA RID: 5066
		[LocalizationKey("Quests")]
		[SerializeField]
		private string description;

		// Token: 0x040013CB RID: 5067
		[SerializeField]
		private int requireLevel;

		// Token: 0x040013CC RID: 5068
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x040013CD RID: 5069
		[FormerlySerializedAs("requiredItem")]
		[SerializeField]
		[ItemTypeID]
		private int requiredItemID;

		// Token: 0x040013CE RID: 5070
		[SerializeField]
		private int requiredItemCount = 1;

		// Token: 0x040013CF RID: 5071
		[SceneID]
		[SerializeField]
		private string requireSceneID;

		// Token: 0x040013D0 RID: 5072
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x040013D1 RID: 5073
		[SerializeField]
		internal List<Condition> prerequisit = new List<Condition>();

		// Token: 0x040013D2 RID: 5074
		[SerializeField]
		internal List<Task> tasks = new List<Task>();

		// Token: 0x040013D3 RID: 5075
		[SerializeField]
		internal List<Reward> rewards = new List<Reward>();

		// Token: 0x040013D4 RID: 5076
		private ReadOnlyCollection<Reward> _readonly_rewards;

		// Token: 0x040013D5 RID: 5077
		[SerializeField]
		[HideInInspector]
		private int nextTaskID;

		// Token: 0x040013D6 RID: 5078
		[SerializeField]
		[HideInInspector]
		private int nextRewardID;

		// Token: 0x040013D7 RID: 5079
		private ReadOnlyCollection<Condition> prerequisits_ReadOnly;

		// Token: 0x040013D8 RID: 5080
		[SerializeField]
		private bool complete;

		// Token: 0x040013D9 RID: 5081
		[SerializeField]
		private bool needInspection;

		// Token: 0x040013DE RID: 5086
		public UnityEvent OnCompletedUnityEvent;

		// Token: 0x020005EF RID: 1519
		[Serializable]
		public struct SaveData
		{
			// Token: 0x0400213C RID: 8508
			public int id;

			// Token: 0x0400213D RID: 8509
			public bool complete;

			// Token: 0x0400213E RID: 8510
			public bool needInspection;

			// Token: 0x0400213F RID: 8511
			public QuestGiverID questGiverID;

			// Token: 0x04002140 RID: 8512
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> taskStatus;

			// Token: 0x04002141 RID: 8513
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> rewardStatus;
		}

		// Token: 0x020005F0 RID: 1520
		public struct QuestInfo
		{
			// Token: 0x060029C0 RID: 10688 RVA: 0x0009B20F File Offset: 0x0009940F
			public QuestInfo(Quest quest)
			{
				this.questId = quest.id;
			}

			// Token: 0x04002142 RID: 8514
			public int questId;
		}

		// Token: 0x020005F1 RID: 1521
		public enum SortingMode
		{
			// Token: 0x04002144 RID: 8516
			Default,
			// Token: 0x04002145 RID: 8517
			ID,
			// Token: 0x04002146 RID: 8518
			Giver,
			// Token: 0x04002147 RID: 8519
			Location
		}
	}
}
