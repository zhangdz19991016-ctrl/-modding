using System;
using UnityEngine;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D4 RID: 980
	public class BarDisplayController : MonoBehaviour
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x0007D847 File Offset: 0x0007BA47
		protected virtual float Current
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x0007D84E File Offset: 0x0007BA4E
		protected virtual float Max
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0007D858 File Offset: 0x0007BA58
		protected void Refresh()
		{
			float current = this.Current;
			float max = this.Max;
			this.bar.SetValue(current, max, "0.#", 0f);
		}

		// Token: 0x0400184A RID: 6218
		[SerializeField]
		private BarDisplay bar;
	}
}
