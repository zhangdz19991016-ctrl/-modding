using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000302 RID: 770
	public class EmptyEntry : MonoBehaviour
	{
		// Token: 0x06001933 RID: 6451 RVA: 0x0005C37C File Offset: 0x0005A57C
		public void Setup(params string[] args)
		{
			this.layoutElement.preferredWidth = this.defaultWidth;
			this.layoutElement.preferredHeight = this.defaultHeight;
			if (args == null)
			{
				return;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (i == 1)
				{
					this.TrySetWidth(args[i]);
				}
				if (i == 2)
				{
					this.TrySetHeight(args[i]);
				}
			}
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0005C3D8 File Offset: 0x0005A5D8
		private void TrySetWidth(string v)
		{
			float preferredWidth;
			if (!float.TryParse(v, out preferredWidth))
			{
				return;
			}
			this.layoutElement.preferredWidth = preferredWidth;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0005C3FC File Offset: 0x0005A5FC
		private void TrySetHeight(string v)
		{
			float preferredHeight;
			if (!float.TryParse(v, out preferredHeight))
			{
				return;
			}
			this.layoutElement.preferredHeight = preferredHeight;
		}

		// Token: 0x0400124B RID: 4683
		[SerializeField]
		private LayoutElement layoutElement;

		// Token: 0x0400124C RID: 4684
		[SerializeField]
		private float defaultWidth;

		// Token: 0x0400124D RID: 4685
		[SerializeField]
		private float defaultHeight;
	}
}
