using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200016C RID: 364
public class OnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B13 RID: 2835 RVA: 0x0002FBF4 File Offset: 0x0002DDF4
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerClick;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x04000996 RID: 2454
	public UnityEvent<PointerEventData> onPointerClick;
}
