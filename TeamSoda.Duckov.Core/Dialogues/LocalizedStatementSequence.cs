using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x02000221 RID: 545
	public class LocalizedStatementSequence : DTNode
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x000404FB File Offset: 0x0003E6FB
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x000404FE File Offset: 0x0003E6FE
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			this.Begin();
			return Status.Running;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00040507 File Offset: 0x0003E707
		private void Begin()
		{
			this.index = this.beginIndex.value - 1;
			this.Next();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00040524 File Offset: 0x0003E724
		private void Next()
		{
			this.index++;
			if (this.index > this.endIndex.value)
			{
				base.status = Status.Success;
				base.DLGTree.Continue(0);
				return;
			}
			LocalizedStatement statement = new LocalizedStatement(this.format.value.Format(new
			{
				keyPrefix = this.keyPrefix.value,
				index = this.index
			}));
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, new Action(this.OnStatementFinish)));
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x000405AF File Offset: 0x0003E7AF
		private void OnStatementFinish()
		{
			this.Next();
		}

		// Token: 0x04000D2B RID: 3371
		public BBParameter<string> keyPrefix;

		// Token: 0x04000D2C RID: 3372
		public BBParameter<int> beginIndex;

		// Token: 0x04000D2D RID: 3373
		public BBParameter<int> endIndex;

		// Token: 0x04000D2E RID: 3374
		public BBParameter<string> format = new BBParameter<string>("{keyPrefix}_{index}");

		// Token: 0x04000D2F RID: 3375
		private int index;
	}
}
