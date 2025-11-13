using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000208 RID: 520
public class PointerDownUpEvents : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000F4D RID: 3917 RVA: 0x0003CD93 File Offset: 0x0003AF93
	public void OnPointerDown(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerDown;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x0003CDA6 File Offset: 0x0003AFA6
	public void OnPointerUp(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerUp;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x04000C8C RID: 3212
	public UnityEvent<PointerEventData> onPointerDown;

	// Token: 0x04000C8D RID: 3213
	public UnityEvent<PointerEventData> onPointerUp;
}
