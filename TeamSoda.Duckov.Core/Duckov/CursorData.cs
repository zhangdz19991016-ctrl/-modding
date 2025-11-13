using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000234 RID: 564
	[Serializable]
	public class CursorData
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00044E74 File Offset: 0x00043074
		public Texture2D texture
		{
			get
			{
				if (this.textures.Length == 0)
				{
					return null;
				}
				return this.textures[0];
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00044E8C File Offset: 0x0004308C
		internal void Apply(int frame)
		{
			if (this.textures == null || this.textures.Length < 1)
			{
				Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
				return;
			}
			if (frame < 0)
			{
				int num = this.textures.Length;
				frame = (-frame / this.textures.Length + 1) * num + frame;
			}
			frame %= this.textures.Length;
			Cursor.SetCursor(this.textures[frame], this.hotspot, CursorMode.Auto);
		}

		// Token: 0x04000DB2 RID: 3506
		public Texture2D[] textures;

		// Token: 0x04000DB3 RID: 3507
		public Vector2 hotspot;

		// Token: 0x04000DB4 RID: 3508
		public float fps;
	}
}
