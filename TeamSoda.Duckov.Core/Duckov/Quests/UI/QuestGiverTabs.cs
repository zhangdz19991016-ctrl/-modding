using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034E RID: 846
	public class QuestGiverTabs : MonoBehaviour, ISingleSelectionMenu<QuestGiverTabButton>
	{
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06001D5B RID: 7515 RVA: 0x00069C0C File Offset: 0x00067E0C
		// (remove) Token: 0x06001D5C RID: 7516 RVA: 0x00069C44 File Offset: 0x00067E44
		public event Action<QuestGiverTabs> onSelectionChanged;

		// Token: 0x06001D5D RID: 7517 RVA: 0x00069C79 File Offset: 0x00067E79
		public QuestGiverTabButton GetSelection()
		{
			return this.selectedButton;
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x00069C81 File Offset: 0x00067E81
		public QuestStatus GetStatus()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return this.selectedButton.Status;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x00069C9C File Offset: 0x00067E9C
		public bool SetSelection(QuestGiverTabButton selection)
		{
			this.selectedButton = selection;
			this.RefreshAllButtons();
			Action<QuestGiverTabs> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this);
			}
			return true;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00069CC0 File Offset: 0x00067EC0
		private void Initialize()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Setup(this);
			}
			if (this.buttons.Count > 0)
			{
				this.SetSelection(this.buttons[0]);
			}
			this.initialized = true;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x00069D3C File Offset: 0x00067F3C
		private void Awake()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00069D4C File Offset: 0x00067F4C
		private void RefreshAllButtons()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Refresh();
			}
		}

		// Token: 0x04001460 RID: 5216
		[SerializeField]
		private List<QuestGiverTabButton> buttons = new List<QuestGiverTabButton>();

		// Token: 0x04001461 RID: 5217
		private QuestGiverTabButton selectedButton;

		// Token: 0x04001463 RID: 5219
		private bool initialized;
	}
}
