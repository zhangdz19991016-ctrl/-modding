using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000350 RID: 848
	public class QuestView : View, ISingleSelectionMenu<QuestEntry>, IQuestSortable
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00069EFC File Offset: 0x000680FC
		public static QuestView Instance
		{
			get
			{
				return View.GetViewInstance<QuestView>();
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00069F03 File Offset: 0x00068103
		public QuestView.ShowContent ShowingContentType
		{
			get
			{
				return this.showingContentType;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x00069F0B File Offset: 0x0006810B
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x00069F13 File Offset: 0x00068113
		public Quest.SortingMode SortingMode
		{
			get
			{
				return this._sortingMode;
			}
			set
			{
				this._sortingMode = value;
				this.RefreshEntryList();
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x00069F22 File Offset: 0x00068122
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x00069F2A File Offset: 0x0006812A
		public bool SortRevert
		{
			get
			{
				return this._sortRevert;
			}
			set
			{
				this._sortRevert = value;
				this.RefreshEntryList();
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x00069F3C File Offset: 0x0006813C
		public IList<Quest> ShowingContent
		{
			get
			{
				if (this.target == null)
				{
					return null;
				}
				QuestView.ShowContent showContent = this.showingContentType;
				if (showContent == QuestView.ShowContent.Active)
				{
					return this.target.ActiveQuests;
				}
				if (showContent != QuestView.ShowContent.History)
				{
					return null;
				}
				return this.target.HistoryQuests;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x00069F84 File Offset: 0x00068184
		private PrefabPool<QuestEntry> QuestEntryPool
		{
			get
			{
				if (this._questEntryPool == null)
				{
					this._questEntryPool = new PrefabPool<QuestEntry>(this.questEntry, this.questEntryParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
						e.SetMenu(this);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._questEntryPool;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x00069FD8 File Offset: 0x000681D8
		private QuestEntry SelectedQuestEntry
		{
			get
			{
				return this.selectedQuestEntry;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x00069FE0 File Offset: 0x000681E0
		public Quest SelectedQuest
		{
			get
			{
				QuestEntry questEntry = this.selectedQuestEntry;
				if (questEntry == null)
				{
					return null;
				}
				return questEntry.Target;
			}
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06001D74 RID: 7540 RVA: 0x00069FF4 File Offset: 0x000681F4
		// (remove) Token: 0x06001D75 RID: 7541 RVA: 0x0006A02C File Offset: 0x0006822C
		internal event Action<QuestView, QuestView.ShowContent> onShowingContentChanged;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06001D76 RID: 7542 RVA: 0x0006A064 File Offset: 0x00068264
		// (remove) Token: 0x06001D77 RID: 7543 RVA: 0x0006A09C File Offset: 0x0006829C
		internal event Action<QuestView, QuestEntry> onSelectedEntryChanged;

		// Token: 0x06001D78 RID: 7544 RVA: 0x0006A0D1 File Offset: 0x000682D1
		public void Setup()
		{
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0006A0E0 File Offset: 0x000682E0
		private void Setup(QuestManager target)
		{
			this.target = target;
			Quest oldSelection = this.SelectedQuest;
			this.RefreshEntryList();
			QuestEntry questEntry = this.activeEntries.Find((QuestEntry e) => e.Target == oldSelection);
			if (questEntry != null)
			{
				this.SetSelection(questEntry);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0006A145 File Offset: 0x00068345
		public static void Show()
		{
			QuestView instance = QuestView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0006A157 File Offset: 0x00068357
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Setup();
			this.fadeGroup.Show();
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0006A170 File Offset: 0x00068370
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0006A183 File Offset: 0x00068383
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0006A18B File Offset: 0x0006838B
		private void OnEnable()
		{
			this.RegisterStaticEvents();
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0006A19E File Offset: 0x0006839E
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
			this.UnregisterStaticEvents();
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0006A1C0 File Offset: 0x000683C0
		private void RegisterStaticEvents()
		{
			QuestManager.onQuestListsChanged += this.Setup;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0006A1D3 File Offset: 0x000683D3
		private void UnregisterStaticEvents()
		{
			QuestManager.onQuestListsChanged -= this.Setup;
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x0006A1E8 File Offset: 0x000683E8
		private void RefreshEntryList()
		{
			this.QuestEntryPool.ReleaseAll();
			bool flag = this.target != null && this.ShowingContent != null && this.ShowingContent.Count > 0;
			this.entryListPlaceHolder.SetActive(!flag);
			if (!flag)
			{
				return;
			}
			List<Quest> list = this.ShowingContent.ToList<Quest>();
			if (this.SortingMode != Quest.SortingMode.Default)
			{
				list.Sort((Quest a, Quest b) => Quest.Compare(a, b, this.SortingMode, this.SortRevert));
			}
			foreach (Quest quest in list)
			{
				QuestEntry questEntry = this.QuestEntryPool.Get(this.questEntryParent);
				questEntry.Setup(quest);
				questEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0006A2C0 File Offset: 0x000684C0
		private void RefreshDetails()
		{
			this.details.Setup(this.SelectedQuest);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0006A2D4 File Offset: 0x000684D4
		public void SetShowingContent(QuestView.ShowContent flags)
		{
			this.showingContentType = flags;
			this.RefreshEntryList();
			List<QuestEntry> list = this.activeEntries;
			if (list != null && list.Count > 0)
			{
				this.SetSelection(this.activeEntries[0]);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			Action<QuestView, QuestView.ShowContent> action = this.onShowingContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, flags);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0006A380 File Offset: 0x00068580
		public void ShowActiveQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.Active);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0006A389 File Offset: 0x00068589
		public void ShowHistoryQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.History);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0006A392 File Offset: 0x00068592
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0006A39C File Offset: 0x0006859C
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			Action<QuestView, QuestEntry> action = this.onSelectedEntryChanged;
			if (action != null)
			{
				action(this, this.selectedQuestEntry);
			}
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			this.RefreshDetails();
			return true;
		}

		// Token: 0x04001469 RID: 5225
		[SerializeField]
		private QuestEntry questEntry;

		// Token: 0x0400146A RID: 5226
		[SerializeField]
		private Transform questEntryParent;

		// Token: 0x0400146B RID: 5227
		[SerializeField]
		private GameObject entryListPlaceHolder;

		// Token: 0x0400146C RID: 5228
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x0400146D RID: 5229
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400146E RID: 5230
		private QuestManager target;

		// Token: 0x0400146F RID: 5231
		[SerializeField]
		private QuestView.ShowContent showingContentType;

		// Token: 0x04001470 RID: 5232
		private Quest.SortingMode _sortingMode;

		// Token: 0x04001471 RID: 5233
		private bool _sortRevert;

		// Token: 0x04001472 RID: 5234
		private PrefabPool<QuestEntry> _questEntryPool;

		// Token: 0x04001473 RID: 5235
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x04001474 RID: 5236
		private QuestEntry selectedQuestEntry;

		// Token: 0x0200060F RID: 1551
		[Flags]
		public enum ShowContent
		{
			// Token: 0x04002194 RID: 8596
			Active = 1,
			// Token: 0x04002195 RID: 8597
			History = 2
		}
	}
}
