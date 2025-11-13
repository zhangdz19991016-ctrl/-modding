using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000231 RID: 561
	public struct Progress
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00044801 File Offset: 0x00042A01
		public float progress
		{
			get
			{
				if (this.total > 0f)
				{
					return Mathf.Clamp01(this.current / this.total);
				}
				return 1f;
			}
		}

		// Token: 0x04000DA1 RID: 3489
		public bool inProgress;

		// Token: 0x04000DA2 RID: 3490
		public float total;

		// Token: 0x04000DA3 RID: 3491
		public float current;

		// Token: 0x04000DA4 RID: 3492
		public string progressName;
	}
}
