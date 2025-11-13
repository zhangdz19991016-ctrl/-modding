using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200015F RID: 351
public class DragHandler : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	// Token: 0x06000ACE RID: 2766 RVA: 0x0002F37A File Offset: 0x0002D57A
	public void OnDrag(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onDrag;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x04000975 RID: 2421
	public UnityEvent<PointerEventData> onDrag;
}
