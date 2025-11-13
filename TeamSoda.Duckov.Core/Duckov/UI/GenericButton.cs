using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000395 RID: 917
	public class GenericButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x06002009 RID: 8201 RVA: 0x000704F8 File Offset: 0x0006E6F8
		public void OnPointerClick(PointerEventData eventData)
		{
			UnityEvent unityEvent = this.onPointerClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0007050C File Offset: 0x0006E70C
		public void OnPointerDown(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(true);
			}
			UnityEvent unityEvent = this.onPointerDown;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00070570 File Offset: 0x0006E770
		public void OnPointerUp(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(false);
			}
			UnityEvent unityEvent = this.onPointerUp;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x040015DA RID: 5594
		public List<ToggleAnimation> toggleAnimations = new List<ToggleAnimation>();

		// Token: 0x040015DB RID: 5595
		public UnityEvent onPointerClick;

		// Token: 0x040015DC RID: 5596
		public UnityEvent onPointerDown;

		// Token: 0x040015DD RID: 5597
		public UnityEvent onPointerUp;
	}
}
