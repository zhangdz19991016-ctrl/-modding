using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034F RID: 847
	public class QuestSortButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x00069DB0 File Offset: 0x00067FB0
		public Quest.SortingMode SortingMode
		{
			get
			{
				if (this.entries.Length == 0)
				{
					Debug.LogError("Error: Entries not configured for sorting mode button of quest ui.");
					return Quest.SortingMode.Default;
				}
				if (this.index < 0)
				{
					this.index = 0;
				}
				else if (this.index >= this.entries.Length)
				{
					this.index = 0;
				}
				return this.entries[this.index].mode;
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x00069E14 File Offset: 0x00068014
		private void Start()
		{
			this.Refresh();
			if (this.targetBehaviour == null)
			{
				return;
			}
			IQuestSortable questSortable = this.targetBehaviour as IQuestSortable;
			if (questSortable == null)
			{
				return;
			}
			this.target = questSortable;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00069E4D File Offset: 0x0006804D
		public void OnPointerClick(PointerEventData eventData)
		{
			eventData.Use();
			this.index++;
			if (this.index >= this.entries.Length)
			{
				this.index = 0;
			}
			this.Refresh();
			this.Apply();
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00069E88 File Offset: 0x00068088
		private void Refresh()
		{
			if (this.entries.Length == 0)
			{
				return;
			}
			if (this.index < 0 || this.index >= this.entries.Length)
			{
				return;
			}
			QuestSortButton.Entry entry = this.entries[this.index];
			this.text.text = entry.displayNameKey.ToPlainText();
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00069EE1 File Offset: 0x000680E1
		private void Apply()
		{
			this.target.SortingMode = this.SortingMode;
		}

		// Token: 0x04001464 RID: 5220
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001465 RID: 5221
		[SerializeField]
		private MonoBehaviour targetBehaviour;

		// Token: 0x04001466 RID: 5222
		[SerializeField]
		private QuestSortButton.Entry[] entries;

		// Token: 0x04001467 RID: 5223
		private int index;

		// Token: 0x04001468 RID: 5224
		private IQuestSortable target;

		// Token: 0x0200060E RID: 1550
		[Serializable]
		private struct Entry
		{
			// Token: 0x04002191 RID: 8593
			[LocalizationKey("Default")]
			public string displayNameKey;

			// Token: 0x04002192 RID: 8594
			public Quest.SortingMode mode;
		}
	}
}
