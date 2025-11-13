using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003CF RID: 975
	public class KontextMenuDataEntry
	{
		// Token: 0x060023A6 RID: 9126 RVA: 0x0007D355 File Offset: 0x0007B555
		public void Invoke()
		{
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04001832 RID: 6194
		public Sprite icon;

		// Token: 0x04001833 RID: 6195
		public string text;

		// Token: 0x04001834 RID: 6196
		public Action action;
	}
}
