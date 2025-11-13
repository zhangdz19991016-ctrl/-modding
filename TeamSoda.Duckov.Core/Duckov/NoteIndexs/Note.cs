using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x0200026A RID: 618
	[Serializable]
	public class Note
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x00048BA2 File Offset: 0x00046DA2
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x00048BB9 File Offset: 0x00046DB9
		[LocalizationKey("Default")]
		public string titleKey
		{
			get
			{
				return "Note_" + this.key + "_Title";
			}
			set
			{
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x00048BBB File Offset: 0x00046DBB
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x00048BD2 File Offset: 0x00046DD2
		[LocalizationKey("Default")]
		public string contentKey
		{
			get
			{
				return "Note_" + this.key + "_Content";
			}
			set
			{
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x00048BD4 File Offset: 0x00046DD4
		public string Title
		{
			get
			{
				return this.titleKey.ToPlainText();
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x00048BE1 File Offset: 0x00046DE1
		private Sprite previewSprite
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x00048BE9 File Offset: 0x00046DE9
		public string Content
		{
			get
			{
				return this.contentKey.ToPlainText();
			}
		}

		// Token: 0x04000E75 RID: 3701
		[SerializeField]
		public string key;

		// Token: 0x04000E76 RID: 3702
		[SerializeField]
		public Sprite image;
	}
}
