using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000389 RID: 905
	public class TooltipsProvider : MonoBehaviour, ITooltipsProvider, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001F7E RID: 8062 RVA: 0x0006EB73 File Offset: 0x0006CD73
		public string GetTooltipsText()
		{
			return this.text;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0006EB7B File Offset: 0x0006CD7B
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0006EB91 File Offset: 0x0006CD91
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0006EB99 File Offset: 0x0006CD99
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x04001580 RID: 5504
		public string text;
	}
}
