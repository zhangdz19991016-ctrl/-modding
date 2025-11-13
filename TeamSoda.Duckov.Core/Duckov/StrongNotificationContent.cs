using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000245 RID: 581
	public class StrongNotificationContent
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x000460D8 File Offset: 0x000442D8
		public StrongNotificationContent(string mainText, string subText = "", Sprite image = null)
		{
			this.mainText = mainText;
			this.subText = subText;
			this.image = image;
		}

		// Token: 0x04000DF9 RID: 3577
		public string mainText;

		// Token: 0x04000DFA RID: 3578
		public string subText;

		// Token: 0x04000DFB RID: 3579
		public Sprite image;
	}
}
