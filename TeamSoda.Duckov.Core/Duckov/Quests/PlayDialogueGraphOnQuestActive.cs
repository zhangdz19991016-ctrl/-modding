using System;
using Duckov.UI;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000342 RID: 834
	public class PlayDialogueGraphOnQuestActive : MonoBehaviour
	{
		// Token: 0x06001CE0 RID: 7392 RVA: 0x000686FE File Offset: 0x000668FE
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00068731 File Offset: 0x00066931
		private void OnQuestActivated(Quest quest)
		{
			if (View.ActiveView != null)
			{
				View.ActiveView.Close();
			}
			this.SetupActors();
			this.PlayDialogue();
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00068756 File Offset: 0x00066956
		private void PlayDialogue()
		{
			this.dialogueTreeController.StartDialogue();
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x00068764 File Offset: 0x00066964
		private void SetupActors()
		{
			if (this.dialogueTreeController.behaviour == null)
			{
				Debug.LogError("Dialoguetree没有配置", this.dialogueTreeController);
				return;
			}
			foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeController.behaviour.actorParameters)
			{
				string name = actorParameter.name;
				if (!string.IsNullOrEmpty(name))
				{
					DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
					if (duckovDialogueActor == null)
					{
						Debug.LogError("未找到actor ID:" + name);
					}
					else
					{
						this.dialogueTreeController.SetActorReference(name, duckovDialogueActor);
					}
				}
			}
		}

		// Token: 0x04001414 RID: 5140
		[SerializeField]
		private Quest quest;

		// Token: 0x04001415 RID: 5141
		[SerializeField]
		private DialogueTreeController dialogueTreeController;
	}
}
