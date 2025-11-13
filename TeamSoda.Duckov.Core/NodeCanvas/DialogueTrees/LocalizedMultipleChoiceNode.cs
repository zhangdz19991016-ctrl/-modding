using System;
using System.Collections.Generic;
using Dialogues;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000406 RID: 1030
	[ParadoxNotion.Design.Icon("List", false, "")]
	[Name("Multiple Choice Localized", 0)]
	[Category("Branch")]
	[Description("Prompt a Dialogue Multiple Choice. A choice will be available if the choice condition(s) are true or there is no choice conditions. The Actor selected is used for the condition checks and will also Say the selection if the option is checked.")]
	[Color("b3ff7f")]
	public class LocalizedMultipleChoiceNode : DTNode
	{
		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x000821F2 File Offset: 0x000803F2
		public override int maxOutConnections
		{
			get
			{
				return this.availableChoices.Count;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x000821FF File Offset: 0x000803FF
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00082204 File Offset: 0x00080404
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (base.outConnections.Count == 0)
			{
				return base.Error("There are no connections to the Multiple Choice Node!");
			}
			Dictionary<IStatement, int> dictionary = new Dictionary<IStatement, int>();
			for (int i = 0; i < this.availableChoices.Count; i++)
			{
				ConditionTask condition = this.availableChoices[i].condition;
				if (condition == null || condition.CheckOnce(base.finalActor.transform, bb))
				{
					LocalizedStatement statement = this.availableChoices[i].statement;
					dictionary[statement] = i;
				}
			}
			if (dictionary.Count == 0)
			{
				base.DLGTree.Stop(false);
				return Status.Failure;
			}
			DialogueTree.RequestMultipleChoices(new MultipleChoiceRequestInfo(base.finalActor, dictionary, this.availableTime, new Action<int>(this.OnOptionSelected))
			{
				showLastStatement = true
			});
			return Status.Running;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000822CC File Offset: 0x000804CC
		private void OnOptionSelected(int index)
		{
			base.status = Status.Success;
			Action action = delegate()
			{
				this.DLGTree.Continue(index);
			};
			if (this.saySelection)
			{
				LocalizedStatement statement = this.availableChoices[index].statement;
				DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, action));
				return;
			}
			action();
		}

		// Token: 0x04001993 RID: 6547
		[SliderField(0f, 10f)]
		public float availableTime;

		// Token: 0x04001994 RID: 6548
		public bool saySelection;

		// Token: 0x04001995 RID: 6549
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<LocalizedMultipleChoiceNode.Choice> availableChoices = new List<LocalizedMultipleChoiceNode.Choice>();

		// Token: 0x0200067C RID: 1660
		[Serializable]
		public class Choice
		{
			// Token: 0x06002B1E RID: 11038 RVA: 0x000A3580 File Offset: 0x000A1780
			public Choice()
			{
			}

			// Token: 0x06002B1F RID: 11039 RVA: 0x000A358F File Offset: 0x000A178F
			public Choice(LocalizedStatement statement)
			{
				this.statement = statement;
			}

			// Token: 0x0400237E RID: 9086
			public bool isUnfolded = true;

			// Token: 0x0400237F RID: 9087
			public LocalizedStatement statement;

			// Token: 0x04002380 RID: 9088
			public ConditionTask condition;
		}
	}
}
