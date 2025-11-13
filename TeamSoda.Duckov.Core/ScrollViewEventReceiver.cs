using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200020A RID: 522
public class ScrollViewEventReceiver : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06000F56 RID: 3926 RVA: 0x0003CE98 File Offset: 0x0003B098
	private void Awake()
	{
		if (this.scrollRect == null)
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
		}
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0003CEB4 File Offset: 0x0003B0B4
	public void OnScroll(PointerEventData eventData)
	{
	}

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private ScrollRect scrollRect;
}
