using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Quests.UI;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033A RID: 826
	public class QuestGiver : InteractableBase
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x00067191 File Offset: 0x00065391
		private IEnumerable<Quest> PossibleQuests
		{
			get
			{
				if (this._possibleQuests == null && QuestManager.Instance != null)
				{
					this._possibleQuests = QuestManager.Instance.GetAllQuestsByQuestGiverID(this.questGiverID);
				}
				return this._possibleQuests;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x000671C4 File Offset: 0x000653C4
		public QuestGiverID ID
		{
			get
			{
				return this.questGiverID;
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000671CC File Offset: 0x000653CC
		protected override void Awake()
		{
			base.Awake();
			QuestManager.onQuestListsChanged += this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Combine(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
			this.inspectionIndicator = UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.QuestMarker);
			this.inspectionIndicator.transform.SetParent(base.transform);
			this.inspectionIndicator.transform.position = base.transform.TransformPoint(this.interactMarkerOffset + Vector3.up * 0.5f);
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00067281 File Offset: 0x00065481
		protected override void Start()
		{
			base.Start();
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00067290 File Offset: 0x00065490
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Remove(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000672E5 File Offset: 0x000654E5
		private void OnTaskFinished(Quest quest, Task task)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000672ED File Offset: 0x000654ED
		private void OnBuildingBuilt(int buildingID)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000672F5 File Offset: 0x000654F5
		private bool AnyQuestNeedsInspection()
		{
			return QuestManager.GetActiveQuestsFromGiver(this.questGiverID).Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00067328 File Offset: 0x00065528
		private bool AnyQuestAvaliable()
		{
			using (IEnumerator<Quest> enumerator = this.PossibleQuests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (QuestManager.IsQuestAvaliable(enumerator.Current.ID))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00067380 File Offset: 0x00065580
		private void OnQuestListsChanged(QuestManager manager)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00067388 File Offset: 0x00065588
		private void RefreshInspectionIndicator()
		{
			if (this.inspectionIndicator)
			{
				bool flag = this.AnyQuestNeedsInspection();
				bool flag2 = this.AnyQuestAvaliable();
				bool active = flag || flag2;
				this.inspectionIndicator.gameObject.SetActive(active);
			}
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000673C3 File Offset: 0x000655C3
		public void ActivateQuest(Quest quest)
		{
			QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.questGiverID));
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000673E0 File Offset: 0x000655E0
		internal List<Quest> GetAvaliableQuests()
		{
			List<Quest> list = new List<Quest>();
			foreach (Quest quest in this.PossibleQuests)
			{
				if (QuestManager.IsQuestAvaliable(quest.ID))
				{
					list.Add(quest);
				}
			}
			return list;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00067444 File Offset: 0x00065644
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			QuestGiverView instance = QuestGiverView.Instance;
			if (instance == null)
			{
				base.StopInteract();
				return;
			}
			instance.Setup(this);
			instance.Open(null);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0006747C File Offset: 0x0006567C
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			if (QuestGiverView.Instance && QuestGiverView.Instance.open)
			{
				QuestGiverView instance = QuestGiverView.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Close();
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000674AB File Offset: 0x000656AB
		protected override void OnUpdate(CharacterMainControl _interactCharacter, float deltaTime)
		{
			base.OnUpdate(_interactCharacter, deltaTime);
			if (!QuestGiverView.Instance || !QuestGiverView.Instance.open)
			{
				base.StopInteract();
			}
		}

		// Token: 0x040013E4 RID: 5092
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x040013E5 RID: 5093
		private GameObject inspectionIndicator;

		// Token: 0x040013E6 RID: 5094
		private IEnumerable<Quest> _possibleQuests;

		// Token: 0x040013E7 RID: 5095
		private List<Quest> avaliableQuests = new List<Quest>();
	}
}
