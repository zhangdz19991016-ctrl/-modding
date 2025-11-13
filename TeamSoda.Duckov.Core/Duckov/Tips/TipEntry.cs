using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x0200024B RID: 587
	[Serializable]
	internal struct TipEntry
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x000466D3 File Offset: 0x000448D3
		public string TipID
		{
			get
			{
				return this.tipID;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x000466DB File Offset: 0x000448DB
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x000466ED File Offset: 0x000448ED
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Tips_" + this.tipID;
			}
			set
			{
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x000466EF File Offset: 0x000448EF
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x04000E19 RID: 3609
		[SerializeField]
		private string tipID;
	}
}
