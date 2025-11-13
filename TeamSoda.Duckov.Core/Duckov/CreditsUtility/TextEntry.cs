using System;
using TMPro;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000305 RID: 773
	public class TextEntry : MonoBehaviour
	{
		// Token: 0x0600193B RID: 6459 RVA: 0x0005C4D4 File Offset: 0x0005A6D4
		internal void Setup(string text, Color color, int size = -1, bool bold = false)
		{
			this.text.text = text;
			if (size < 0)
			{
				size = this.defaultSize;
			}
			this.text.color = color;
			this.text.fontSize = (float)size;
			this.text.fontStyle = ((this.text.fontStyle & ~FontStyles.Bold) | (bold ? FontStyles.Bold : FontStyles.Normal));
		}

		// Token: 0x04001250 RID: 4688
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001251 RID: 4689
		public int defaultSize = 26;
	}
}
