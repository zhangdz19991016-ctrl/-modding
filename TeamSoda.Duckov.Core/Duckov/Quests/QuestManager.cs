using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Quests.Relations;
using Duckov.Quests.Tasks;
using Duckov.UI;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033C RID: 828
	public class QuestManager : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x000674E6 File Offset: 0x000656E6
		public string TaskFinishNotificationFormat
		{
			get
			{
				return this.taskFinishNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x000674F3 File Offset: 0x000656F3
		public static QuestManager Instance
		{
			get
			{
				return QuestManager.instance;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x000674FA File Offset: 0x000656FA
		public static bool AnyQuestNeedsInspection
		{
			get
			{
				return !(QuestManager.Instance == null) && QuestManager.Instance.NeedInspection;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x00067515 File Offset: 0x00065715
		public bool NeedInspection
		{
			get
			{
				if (this.activeQuests == null)
				{
					return false;
				}
				return this.activeQuests.Any((Quest e) => e != null && e.NeedInspection);
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0006754B File Offset: 0x0006574B
		public static IEnumerable<int> GetAllRequiredItems()
		{
			if (QuestManager.Instance == null)
			{
				yield break;
			}
			List<Quest> list = QuestManager.Instance.ActiveQuests;
			foreach (Quest quest in list)
			{
				if (quest.tasks != null)
				{
					foreach (Task task in quest.tasks)
					{
						SubmitItems submitItems = task as SubmitItems;
						if (submitItems != null && !submitItems.IsFinished())
						{
							yield return submitItems.ItemTypeID;
						}
					}
					List<Task>.Enumerator enumerator2 = default(List<Task>.Enumerator);
				}
			}
			List<Quest>.Enumerator enumerator = default(List<Quest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00067554 File Offset: 0x00065754
		public static bool AnyActiveQuestNeedsInspection(QuestGiverID giverID)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.activeQuests != null && QuestManager.Instance.activeQuests.Any((Quest e) => e != null && e.QuestGiverID == giverID && e.NeedInspection);
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x000675A6 File Offset: 0x000657A6
		private ICollection<Quest> QuestPrefabCollection
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x000675AD File Offset: 0x000657AD
		private QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.QuestRelation;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000675B4 File Offset: 0x000657B4
		public List<Quest> ActiveQuests
		{
			get
			{
				this.activeQuests.Sort(delegate(Quest a, Quest b)
				{
					int num = a.AreTasksFinished() ? 1 : 0;
					return (b.AreTasksFinished() ? 1 : 0) - num;
				});
				return this.activeQuests;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001C73 RID: 7283 RVA: 0x000675E6 File Offset: 0x000657E6
		public List<Quest> HistoryQuests
		{
			get
			{
				return this.historyQuests;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x000675EE File Offset: 0x000657EE
		public List<int> EverInspectedQuest
		{
			get
			{
				return this.everInspectedQuest;
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06001C75 RID: 7285 RVA: 0x000675F8 File Offset: 0x000657F8
		// (remove) Token: 0x06001C76 RID: 7286 RVA: 0x0006762C File Offset: 0x0006582C
		public static event Action<QuestManager> onQuestListsChanged;

		// Token: 0x06001C77 RID: 7287 RVA: 0x00067660 File Offset: 0x00065860
		public object GenerateSaveData()
		{
			QuestManager.SaveData saveData = default(QuestManager.SaveData);
			saveData.activeQuestsData = new List<object>();
			saveData.historyQuestsData = new List<object>();
			saveData.everInspectedQuest = new List<int>();
			foreach (Quest quest in this.ActiveQuests)
			{
				saveData.activeQuestsData.Add(quest.GenerateSaveData());
			}
			foreach (Quest quest2 in this.HistoryQuests)
			{
				saveData.historyQuestsData.Add(quest2.GenerateSaveData());
			}
			saveData.everInspectedQuest.AddRange(this.EverInspectedQuest);
			return saveData;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0006774C File Offset: 0x0006594C
		public void SetupSaveData(object dataObj)
		{
			if (dataObj is QuestManager.SaveData)
			{
				QuestManager.SaveData saveData = (QuestManager.SaveData)dataObj;
				if (saveData.activeQuestsData != null)
				{
					foreach (object obj in saveData.activeQuestsData)
					{
						int id = ((Quest.SaveData)obj).id;
						Quest questPrefab = this.GetQuestPrefab(id);
						if (questPrefab == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id));
						}
						else
						{
							Quest quest = UnityEngine.Object.Instantiate<Quest>(questPrefab, base.transform);
							quest.SetupSaveData(obj);
							this.activeQuests.Add(quest);
						}
					}
				}
				if (saveData.historyQuestsData != null)
				{
					foreach (object obj2 in saveData.historyQuestsData)
					{
						int id2 = ((Quest.SaveData)obj2).id;
						Quest questPrefab2 = this.GetQuestPrefab(id2);
						if (questPrefab2 == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id2));
						}
						else
						{
							Quest quest2 = UnityEngine.Object.Instantiate<Quest>(questPrefab2, base.transform);
							quest2.SetupSaveData(obj2);
							this.historyQuests.Add(quest2);
						}
					}
				}
				if (saveData.everInspectedQuest != null)
				{
					this.EverInspectedQuest.Clear();
					this.EverInspectedQuest.AddRange(saveData.everInspectedQuest);
				}
				return;
			}
			Debug.LogError("错误的数据类型");
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000678E4 File Offset: 0x00065AE4
		private void Save()
		{
			SavesSystem.Save<object>("Quest", "Data", this.GenerateSaveData());
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000678FC File Offset: 0x00065AFC
		private void Load()
		{
			try
			{
				QuestManager.SaveData saveData = SavesSystem.Load<QuestManager.SaveData>("Quest", "Data");
				this.SetupSaveData(saveData);
			}
			catch
			{
				Debug.LogError("在加载Quest存档时出现了错误");
			}
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00067944 File Offset: 0x00065B44
		public IEnumerable<Quest> GetAllQuestsByQuestGiverID(QuestGiverID questGiverID)
		{
			return from e in this.QuestPrefabCollection
			where e != null && e.QuestGiverID == questGiverID
			select e;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00067978 File Offset: 0x00065B78
		private Quest GetQuestPrefab(int id)
		{
			return this.QuestPrefabCollection.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000679A9 File Offset: 0x00065BA9
		private void Awake()
		{
			if (QuestManager.instance == null)
			{
				QuestManager.instance = this;
			}
			if (QuestManager.instance != this)
			{
				Debug.LogError("侦测到多个QuestManager！");
				return;
			}
			this.RegisterEvents();
			this.Load();
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000679E2 File Offset: 0x00065BE2
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000679EC File Offset: 0x00065BEC
		private void RegisterEvents()
		{
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00067A40 File Offset: 0x00065C40
		private void UnregisterEvents()
		{
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00067A94 File Offset: 0x00065C94
		private void OnQuestCompleted(Quest quest)
		{
			if (!this.activeQuests.Remove(quest))
			{
				Debug.LogWarning(quest.DisplayName + " 并不存在于活跃任务表中。已终止操作。");
				return;
			}
			this.historyQuests.Add(quest);
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00067AE1 File Offset: 0x00065CE1
		private void OnQuestStatusChanged(Quest quest)
		{
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00067AE4 File Offset: 0x00065CE4
		public void ActivateQuest(int id, QuestGiverID? overrideQuestGiverID = null)
		{
			Quest quest = UnityEngine.Object.Instantiate<Quest>(this.GetQuestPrefab(id), base.transform);
			if (overrideQuestGiverID != null)
			{
				quest.QuestGiverID = overrideQuestGiverID.Value;
			}
			this.activeQuests.Add(quest);
			quest.NotifyActivated();
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00067B3C File Offset: 0x00065D3C
		internal static bool IsQuestAvaliable(int id)
		{
			return !(QuestManager.Instance == null) && !QuestManager.IsQuestFinished(id) && !QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id) && QuestManager.Instance.GetQuestPrefab(id).MeetsPrerequisit();
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00067BA8 File Offset: 0x00065DA8
		internal static bool IsQuestFinished(int id)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.historyQuests.Any((Quest e) => e.ID == id);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00067BEC File Offset: 0x00065DEC
		internal static bool AreQuestFinished(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.AddRange(requiredQuestIDs);
			foreach (Quest quest in QuestManager.instance.historyQuests)
			{
				hashSet.Remove(quest.ID);
			}
			return hashSet.Count <= 0;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00067C74 File Offset: 0x00065E74
		internal static List<Quest> GetActiveQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.ActiveQuests
			where e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00067CC8 File Offset: 0x00065EC8
		internal static List<Quest> GetHistoryQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.historyQuests
			where e != null && e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00067D17 File Offset: 0x00065F17
		internal static bool IsQuestActive(Quest quest)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Contains(quest);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00067D38 File Offset: 0x00065F38
		internal static bool IsQuestActive(int questID)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Any((Quest e) => e.ID == questID);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00067D84 File Offset: 0x00065F84
		internal static bool AreQuestsActive(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			using (IEnumerator<int> enumerator = requiredQuestIDs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int id = enumerator.Current;
					if (!QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00067E04 File Offset: 0x00066004
		private void OnTaskFinished(Quest quest, Task task)
		{
			NotificationText.Push(this.TaskFinishNotificationFormat.Format(new
			{
				questDisplayName = quest.DisplayName,
				finishedTasks = quest.FinishedTaskCount,
				totalTasks = quest.tasks.Count
			}));
			Action<Quest, Task> onTaskFinishedEvent = QuestManager.OnTaskFinishedEvent;
			if (onTaskFinishedEvent != null)
			{
				onTaskFinishedEvent(quest, task);
			}
			AudioManager.Post("UI/mission_small");
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00067E5A File Offset: 0x0006605A
		internal static void NotifyTaskFinished(Quest quest, Task task)
		{
			QuestManager questManager = QuestManager.instance;
			if (questManager == null)
			{
				return;
			}
			questManager.OnTaskFinished(quest, task);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00067E6D File Offset: 0x0006606D
		internal static bool EverInspected(int id)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.EverInspectedQuest.Contains(id);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00067E8E File Offset: 0x0006608E
		internal static void SetEverInspected(int id)
		{
			if (QuestManager.EverInspected(id))
			{
				return;
			}
			if (QuestManager.Instance == null)
			{
				return;
			}
			QuestManager.Instance.EverInspectedQuest.Add(id);
		}

		// Token: 0x040013F2 RID: 5106
		[SerializeField]
		private string taskFinishNotificationFormatKey = "UI_Quest_TaskFinishedNotification";

		// Token: 0x040013F3 RID: 5107
		private static QuestManager instance;

		// Token: 0x040013F4 RID: 5108
		public static Action<Quest, Task> OnTaskFinishedEvent;

		// Token: 0x040013F5 RID: 5109
		private List<Quest> activeQuests = new List<Quest>();

		// Token: 0x040013F6 RID: 5110
		private List<Quest> historyQuests = new List<Quest>();

		// Token: 0x040013F7 RID: 5111
		private List<int> everInspectedQuest = new List<int>();

		// Token: 0x040013F9 RID: 5113
		private const string savePrefix = "Quest";

		// Token: 0x040013FA RID: 5114
		private const string saveKey = "Data";

		// Token: 0x020005F8 RID: 1528
		[Serializable]
		public struct SaveData
		{
			// Token: 0x04002156 RID: 8534
			public List<object> activeQuestsData;

			// Token: 0x04002157 RID: 8535
			public List<object> historyQuestsData;

			// Token: 0x04002158 RID: 8536
			public List<int> everInspectedQuest;
		}
	}
}
