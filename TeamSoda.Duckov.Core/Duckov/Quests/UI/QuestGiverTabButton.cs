using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034C RID: 844
	public class QuestGiverTabButton : MonoBehaviour
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001D54 RID: 7508 RVA: 0x00069B79 File Offset: 0x00067D79
		public QuestStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00069B81 File Offset: 0x00067D81
		internal void Setup(QuestGiverTabs questGiverTabs)
		{
			this.master = questGiverTabs;
			this.Refresh();
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00069B90 File Offset: 0x00067D90
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x00069BAE File Offset: 0x00067DAE
		private void OnClick()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x00069BCC File Offset: 0x00067DCC
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00069BEF File Offset: 0x00067DEF
		internal void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x04001457 RID: 5207
		[SerializeField]
		private Button button;

		// Token: 0x04001458 RID: 5208
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001459 RID: 5209
		[SerializeField]
		private QuestStatus status;

		// Token: 0x0400145A RID: 5210
		private QuestGiverTabs master;
	}
}
