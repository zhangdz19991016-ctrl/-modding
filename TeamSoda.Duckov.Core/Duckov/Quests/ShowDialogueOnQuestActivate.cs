using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000343 RID: 835
	public class ShowDialogueOnQuestActivate : MonoBehaviour
	{
		// Token: 0x06001CE5 RID: 7397 RVA: 0x00068824 File Offset: 0x00066A24
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x00068857 File Offset: 0x00066A57
		private void OnQuestActivated(Quest quest)
		{
			this.ShowDIalogue().Forget();
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x00068864 File Offset: 0x00066A64
		private UniTask ShowDIalogue()
		{
			ShowDialogueOnQuestActivate.<ShowDIalogue>d__6 <ShowDIalogue>d__;
			<ShowDIalogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDIalogue>d__.<>4__this = this;
			<ShowDIalogue>d__.<>1__state = -1;
			<ShowDIalogue>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDIalogue>d__6>(ref <ShowDIalogue>d__);
			return <ShowDIalogue>d__.<>t__builder.Task;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000688A8 File Offset: 0x00066AA8
		private UniTask ShowDialogueEntry(ShowDialogueOnQuestActivate.DialogueEntry cur)
		{
			ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7 <ShowDialogueEntry>d__;
			<ShowDialogueEntry>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDialogueEntry>d__.<>4__this = this;
			<ShowDialogueEntry>d__.cur = cur;
			<ShowDialogueEntry>d__.<>1__state = -1;
			<ShowDialogueEntry>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7>(ref <ShowDialogueEntry>d__);
			return <ShowDialogueEntry>d__.<>t__builder.Task;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000688F4 File Offset: 0x00066AF4
		private Transform GetQuestGiverTransform(Quest quest)
		{
			QuestGiverID id = quest.QuestGiverID;
			QuestGiver questGiver = UnityEngine.Object.FindObjectsByType<QuestGiver>(FindObjectsSortMode.None).FirstOrDefault((QuestGiver e) => e != null && e.ID == id);
			if (questGiver == null)
			{
				return null;
			}
			return questGiver.transform;
		}

		// Token: 0x04001416 RID: 5142
		[SerializeField]
		private Quest quest;

		// Token: 0x04001417 RID: 5143
		[SerializeField]
		private List<ShowDialogueOnQuestActivate.DialogueEntry> dialogueEntries;

		// Token: 0x04001418 RID: 5144
		private Transform cachedQuestGiverTransform;

		// Token: 0x02000605 RID: 1541
		[Serializable]
		public class DialogueEntry
		{
			// Token: 0x04002172 RID: 8562
			[TextArea]
			public string content;
		}
	}
}
