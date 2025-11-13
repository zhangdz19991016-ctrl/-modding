using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034B RID: 843
	public class QuestGiverView : View, ISingleSelectionMenu<QuestEntry>, IQuestSortable
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x000693EF File Offset: 0x000675EF
		public static QuestGiverView Instance
		{
			get
			{
				return View.GetViewInstance<QuestGiverView>();
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x000693F6 File Offset: 0x000675F6
		public string BtnText_CompleteQuest
		{
			get
			{
				return this.btnText_CompleteQuest.ToPlainText();
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x00069403 File Offset: 0x00067603
		public string BtnText_AcceptQuest
		{
			get
			{
				return this.btnText_AcceptQuest.ToPlainText();
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x00069410 File Offset: 0x00067610
		// (set) Token: 0x06001D37 RID: 7479 RVA: 0x00069418 File Offset: 0x00067618
		public Quest.SortingMode SortingMode
		{
			get
			{
				return this._sortingMode;
			}
			set
			{
				this._sortingMode = value;
				this.RefreshList();
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00069427 File Offset: 0x00067627
		// (set) Token: 0x06001D39 RID: 7481 RVA: 0x0006942F File Offset: 0x0006762F
		public bool SortRevert
		{
			get
			{
				return this._sortRevert;
			}
			set
			{
				this._sortRevert = value;
				this.RefreshList();
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x00069440 File Offset: 0x00067640
		private PrefabPool<QuestEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<QuestEntry>(this.entryPrefab, this.questEntriesParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00069494 File Offset: 0x00067694
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.RefreshList();
			this.RefreshDetails();
			QuestManager.onQuestListsChanged += this.OnQuestListChanged;
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000694E0 File Offset: 0x000676E0
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00069515 File Offset: 0x00067715
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00069531 File Offset: 0x00067731
		private void OnQuestStatusChanged(Quest quest)
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			if (quest == ((questEntry != null) ? questEntry.Target : null))
			{
				this.RefreshDetails();
			}
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00069553 File Offset: 0x00067753
		protected override void Awake()
		{
			base.Awake();
			this.tabs.onSelectionChanged += this.OnTabChanged;
			this.btn_Interact.onClick.AddListener(new UnityAction(this.OnInteractButtonClicked));
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00069590 File Offset: 0x00067790
		private void OnInteractButtonClicked()
		{
			if (this.btnAcceptQuest)
			{
				Quest quest = this.details.Target;
				if (quest != null && QuestManager.IsQuestAvaliable(quest.ID))
				{
					QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.target.ID));
					AudioManager.Post(this.sfx_AcceptQuest);
					return;
				}
			}
			else if (this.btnCompleteQuest)
			{
				Quest quest2 = this.details.Target;
				if (quest2 == null)
				{
					return;
				}
				if (quest2.TryComplete())
				{
					this.ShowCompleteUI(quest2);
					AudioManager.Post(this.sfx_CompleteQuest);
				}
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0006962D File Offset: 0x0006782D
		private void ShowCompleteUI(Quest quest)
		{
			this.completeUITask = this.questCompletePanel.Show(quest);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00069641 File Offset: 0x00067841
		private void OnTabChanged(QuestGiverTabs tabs)
		{
			this.RefreshList();
			this.RefreshDetails();
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0006964F File Offset: 0x0006784F
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x00069668 File Offset: 0x00067868
		private void OnQuestListChanged(QuestManager manager)
		{
			this.RefreshList();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0006967E File Offset: 0x0006787E
		public void Setup(QuestGiver target)
		{
			this.target = target;
			this.RefreshList();
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x00069690 File Offset: 0x00067890
		private void RefreshList()
		{
			QuestGiverView.<>c__DisplayClass50_0 CS$<>8__locals1 = new QuestGiverView.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			QuestGiverView.<>c__DisplayClass50_0 CS$<>8__locals2 = CS$<>8__locals1;
			QuestEntry questEntry = this.selectedQuestEntry;
			CS$<>8__locals2.keepQuest = ((questEntry != null) ? questEntry.Target : null);
			this.selectedQuestEntry = null;
			this.EntryPool.ReleaseAll();
			List<Quest> questsToShow = this.GetQuestsToShow();
			questsToShow.Sort((Quest a, Quest b) => Quest.Compare(a, b, CS$<>8__locals1.<>4__this.SortingMode, CS$<>8__locals1.<>4__this.SortRevert));
			bool flag = questsToShow.Count > 0;
			this.entryPlaceHolder.SetActive(!flag);
			this.RefreshRedDots();
			if (!flag)
			{
				return;
			}
			foreach (Quest quest in questsToShow)
			{
				QuestEntry questEntry2 = this.EntryPool.Get(this.questEntriesParent);
				questEntry2.transform.SetAsLastSibling();
				questEntry2.SetMenu(this);
				questEntry2.Setup(quest);
			}
			QuestEntry questEntry3 = this.activeEntries.Find((QuestEntry e) => e.Target == CS$<>8__locals1.keepQuest);
			if (questEntry3 != null)
			{
				this.SetSelection(questEntry3);
				return;
			}
			this.SetSelection(null);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000697A8 File Offset: 0x000679A8
		private void RefreshRedDots()
		{
			this.uninspectedAvaliableRedDot.SetActive(this.AnyUninspectedAvaliableQuest());
			this.activeRedDot.SetActive(this.AnyUninspectedActiveQuest());
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000697CC File Offset: 0x000679CC
		private bool AnyUninspectedActiveQuest()
		{
			return !(this.target == null) && QuestManager.AnyActiveQuestNeedsInspection(this.target.ID);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000697F0 File Offset: 0x000679F0
		private bool AnyUninspectedAvaliableQuest()
		{
			if (this.target == null)
			{
				return false;
			}
			return this.target.GetAvaliableQuests().Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0006983C File Offset: 0x00067A3C
		private List<Quest> GetQuestsToShow()
		{
			List<Quest> list = new List<Quest>();
			if (this.target == null)
			{
				return list;
			}
			QuestStatus status = this.tabs.GetStatus();
			switch (status)
			{
			case QuestStatus.None:
				return list;
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				list.AddRange(this.target.GetAvaliableQuests());
				break;
			case QuestStatus.Active:
				list.AddRange(QuestManager.GetActiveQuestsFromGiver(this.target.ID));
				break;
			default:
				if (status == QuestStatus.Finished)
				{
					list.AddRange(QuestManager.GetHistoryQuestsFromGiver(this.target.ID));
				}
				break;
			}
			return list;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000698D0 File Offset: 0x00067AD0
		private void RefreshDetails()
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			this.details.Setup(quest);
			this.RefreshInteractButton();
			bool interactable = quest && (QuestManager.IsQuestActive(quest) || quest.Complete);
			this.details.Interactable = interactable;
			this.details.Refresh();
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00069938 File Offset: 0x00067B38
		private void RefreshInteractButton()
		{
			this.btnAcceptQuest = false;
			this.btnCompleteQuest = false;
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			if (quest == null)
			{
				this.btn_Interact.gameObject.SetActive(false);
				return;
			}
			QuestStatus status = this.tabs.GetStatus();
			bool active = false;
			switch (status)
			{
			case QuestStatus.None:
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				active = true;
				this.btn_Interact.interactable = true;
				this.btnImage.color = this.interactableBtnImageColor;
				this.btnText.text = this.BtnText_AcceptQuest;
				this.btnAcceptQuest = true;
				break;
			case QuestStatus.Active:
			{
				active = true;
				bool flag = quest.AreTasksFinished();
				this.btn_Interact.interactable = flag;
				this.btnImage.color = (flag ? this.interactableBtnImageColor : this.uninteractableBtnImageColor);
				this.btnText.text = this.BtnText_CompleteQuest;
				this.btnCompleteQuest = true;
				break;
			}
			default:
				if (status != QuestStatus.Finished)
				{
				}
				break;
			}
			this.btn_Interact.gameObject.SetActive(active);
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00069A48 File Offset: 0x00067C48
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00069A50 File Offset: 0x00067C50
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			if (selection != null)
			{
				QuestManager.SetEverInspected(selection.Target.ID);
			}
			this.RefreshDetails();
			this.RefreshEntries();
			this.RefreshRedDots();
			return true;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00069A88 File Offset: 0x00067C88
		private void RefreshEntries()
		{
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00069AD8 File Offset: 0x00067CD8
		internal override void TryQuit()
		{
			if (this.questCompletePanel.isActiveAndEnabled)
			{
				this.questCompletePanel.Skip();
				return;
			}
			base.Close();
		}

		// Token: 0x0400143C RID: 5180
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400143D RID: 5181
		[SerializeField]
		private RectTransform questEntriesParent;

		// Token: 0x0400143E RID: 5182
		[SerializeField]
		private QuestCompletePanel questCompletePanel;

		// Token: 0x0400143F RID: 5183
		[SerializeField]
		private QuestGiverTabs tabs;

		// Token: 0x04001440 RID: 5184
		[SerializeField]
		private QuestEntry entryPrefab;

		// Token: 0x04001441 RID: 5185
		[SerializeField]
		private GameObject entryPlaceHolder;

		// Token: 0x04001442 RID: 5186
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x04001443 RID: 5187
		[SerializeField]
		private Button btn_Interact;

		// Token: 0x04001444 RID: 5188
		[SerializeField]
		private TextMeshProUGUI btnText;

		// Token: 0x04001445 RID: 5189
		[SerializeField]
		private Image btnImage;

		// Token: 0x04001446 RID: 5190
		[SerializeField]
		private string btnText_AcceptQuest = "接受任务";

		// Token: 0x04001447 RID: 5191
		[SerializeField]
		private string btnText_CompleteQuest = "完成任务";

		// Token: 0x04001448 RID: 5192
		[SerializeField]
		private Color interactableBtnImageColor = Color.green;

		// Token: 0x04001449 RID: 5193
		[SerializeField]
		private Color uninteractableBtnImageColor = Color.gray;

		// Token: 0x0400144A RID: 5194
		[SerializeField]
		private GameObject uninspectedAvaliableRedDot;

		// Token: 0x0400144B RID: 5195
		[SerializeField]
		private GameObject activeRedDot;

		// Token: 0x0400144C RID: 5196
		private string sfx_AcceptQuest = "UI/mission_accept";

		// Token: 0x0400144D RID: 5197
		private string sfx_CompleteQuest = "UI/mission_large";

		// Token: 0x0400144E RID: 5198
		private Quest.SortingMode _sortingMode;

		// Token: 0x0400144F RID: 5199
		private bool _sortRevert;

		// Token: 0x04001450 RID: 5200
		private PrefabPool<QuestEntry> _entryPool;

		// Token: 0x04001451 RID: 5201
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x04001452 RID: 5202
		private QuestGiver target;

		// Token: 0x04001453 RID: 5203
		private QuestEntry selectedQuestEntry;

		// Token: 0x04001454 RID: 5204
		private UniTask completeUITask;

		// Token: 0x04001455 RID: 5205
		private bool btnAcceptQuest;

		// Token: 0x04001456 RID: 5206
		private bool btnCompleteQuest;
	}
}
