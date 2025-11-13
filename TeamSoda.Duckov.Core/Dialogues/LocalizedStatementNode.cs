using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x02000220 RID: 544
	public class LocalizedStatementNode : DTNode
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x00040459 File Offset: 0x0003E659
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0004045C File Offset: 0x0003E65C
		private string Key
		{
			get
			{
				if (this.useSequence.value)
				{
					return string.Format("{0}_{1}", this.key.value, this.sequenceIndex.value);
				}
				return this.key.value;
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0004049C File Offset: 0x0003E69C
		private LocalizedStatement CreateStatement()
		{
			return new LocalizedStatement(this.Key);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x000404AC File Offset: 0x0003E6AC
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			LocalizedStatement statement = this.CreateStatement();
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, new Action(this.OnStatementFinish)));
			return Status.Running;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000404DE File Offset: 0x0003E6DE
		private void OnStatementFinish()
		{
			base.status = Status.Success;
			base.DLGTree.Continue(0);
		}

		// Token: 0x04000D28 RID: 3368
		public BBParameter<string> key;

		// Token: 0x04000D29 RID: 3369
		public BBParameter<bool> useSequence;

		// Token: 0x04000D2A RID: 3370
		public BBParameter<int> sequenceIndex;
	}
}
