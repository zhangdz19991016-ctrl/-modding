using System;

namespace Duckov.Tasks
{
	// Token: 0x02000375 RID: 885
	public interface ITaskBehaviour
	{
		// Token: 0x06001ED1 RID: 7889
		void Begin();

		// Token: 0x06001ED2 RID: 7890
		bool IsPending();

		// Token: 0x06001ED3 RID: 7891
		bool IsComplete();

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0006CEB4 File Offset: 0x0006B0B4
		void Skip()
		{
		}
	}
}
