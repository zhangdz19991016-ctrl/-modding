using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000385 RID: 901
	public class CopyTextButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001F69 RID: 8041 RVA: 0x0006E926 File Offset: 0x0006CB26
		public void OnPointerClick(PointerEventData eventData)
		{
			GUIUtility.systemCopyBuffer = this.text;
		}

		// Token: 0x04001576 RID: 5494
		[SerializeField]
		private string text;
	}
}
