using System;
using UnityEngine.Events;

namespace Duckov.MiniGames
{
	// Token: 0x02000283 RID: 643
	public class VirtualCursorTarget : MiniGameBehaviour
	{
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0004CB4D File Offset: 0x0004AD4D
		public bool IsHovering
		{
			get
			{
				return VirtualCursor.IsHovering(this);
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0004CB55 File Offset: 0x0004AD55
		public void OnCursorEnter()
		{
			UnityEvent unityEvent = this.onEnter;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004CB67 File Offset: 0x0004AD67
		public void OnCursorExit()
		{
			UnityEvent unityEvent = this.onExit;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0004CB79 File Offset: 0x0004AD79
		public void OnClick()
		{
			UnityEvent unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000F11 RID: 3857
		public UnityEvent onEnter;

		// Token: 0x04000F12 RID: 3858
		public UnityEvent onExit;

		// Token: 0x04000F13 RID: 3859
		public UnityEvent onClick;
	}
}
