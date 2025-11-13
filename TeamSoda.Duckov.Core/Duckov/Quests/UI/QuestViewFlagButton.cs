using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000353 RID: 851
	public class QuestViewFlagButton : MonoBehaviour
	{
		// Token: 0x06001DA2 RID: 7586 RVA: 0x0006A78B File Offset: 0x0006898B
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.master.onShowingContentChanged += this.OnMasterShowingContentChanged;
			this.Refresh();
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0006A7C6 File Offset: 0x000689C6
		private void OnButtonClicked()
		{
			this.master.SetShowingContent(this.content);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0006A7D9 File Offset: 0x000689D9
		private void OnMasterShowingContentChanged(QuestView view, QuestView.ShowContent content)
		{
			this.Refresh();
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0006A7E4 File Offset: 0x000689E4
		private void Refresh()
		{
			bool active = this.master.ShowingContentType == this.content;
			this.selectionIndicator.SetActive(active);
		}

		// Token: 0x04001487 RID: 5255
		[SerializeField]
		private QuestView master;

		// Token: 0x04001488 RID: 5256
		[SerializeField]
		private Button button;

		// Token: 0x04001489 RID: 5257
		[SerializeField]
		private QuestView.ShowContent content;

		// Token: 0x0400148A RID: 5258
		[SerializeField]
		private GameObject selectionIndicator;
	}
}
