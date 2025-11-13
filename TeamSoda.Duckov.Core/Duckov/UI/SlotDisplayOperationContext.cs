using System;

namespace Duckov.UI
{
	// Token: 0x020003A5 RID: 933
	public struct SlotDisplayOperationContext
	{
		// Token: 0x0600216A RID: 8554 RVA: 0x0007510C File Offset: 0x0007330C
		public SlotDisplayOperationContext(SlotDisplay slotDisplay, SlotDisplayOperationContext.Operation operation, bool succeed)
		{
			this.slotDisplay = slotDisplay;
			this.operation = operation;
			this.succeed = succeed;
		}

		// Token: 0x040016A8 RID: 5800
		public SlotDisplay slotDisplay;

		// Token: 0x040016A9 RID: 5801
		public SlotDisplayOperationContext.Operation operation;

		// Token: 0x040016AA RID: 5802
		public bool succeed;

		// Token: 0x0200062A RID: 1578
		public enum Operation
		{
			// Token: 0x04002204 RID: 8708
			None,
			// Token: 0x04002205 RID: 8709
			Equip,
			// Token: 0x04002206 RID: 8710
			Unequip,
			// Token: 0x04002207 RID: 8711
			Deny
		}
	}
}
