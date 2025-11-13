using System;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000304 RID: 772
	public class ImageEntry : MonoBehaviour
	{
		// Token: 0x06001939 RID: 6457 RVA: 0x0005C434 File Offset: 0x0005A634
		internal void Setup(string[] elements)
		{
			if (elements.Length < 2)
			{
				return;
			}
			for (int i = 0; i < elements.Length; i++)
			{
				float preferredWidth;
				if (i == 1)
				{
					string text = elements[1];
					Sprite sprite = GameplayDataSettings.GetSprite(text);
					if (sprite == null)
					{
						Debug.LogError("Cannot find sprite:" + text);
					}
					else
					{
						this.image.sprite = sprite;
					}
				}
				else if (i == 2)
				{
					float preferredHeight;
					if (float.TryParse(elements[2], out preferredHeight))
					{
						this.layoutElement.preferredHeight = preferredHeight;
					}
				}
				else if (i == 3 && float.TryParse(elements[2], out preferredWidth))
				{
					this.layoutElement.preferredWidth = preferredWidth;
				}
			}
		}

		// Token: 0x0400124E RID: 4686
		[SerializeField]
		private Image image;

		// Token: 0x0400124F RID: 4687
		[SerializeField]
		private LayoutElement layoutElement;
	}
}
