using System;
using System.IO;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x02000270 RID: 624
	[Serializable]
	public struct ModInfo
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x0004998B File Offset: 0x00047B8B
		public string dllPath
		{
			get
			{
				return Path.Combine(this.path, this.name + ".dll");
			}
		}

		// Token: 0x04000E87 RID: 3719
		public string path;

		// Token: 0x04000E88 RID: 3720
		public string name;

		// Token: 0x04000E89 RID: 3721
		public string displayName;

		// Token: 0x04000E8A RID: 3722
		public string description;

		// Token: 0x04000E8B RID: 3723
		public Texture2D preview;

		// Token: 0x04000E8C RID: 3724
		public bool dllFound;

		// Token: 0x04000E8D RID: 3725
		public bool isSteamItem;

		// Token: 0x04000E8E RID: 3726
		public ulong publishedFileId;
	}
}
