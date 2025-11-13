using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C7 RID: 967
	public class CloseViewOnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002349 RID: 9033 RVA: 0x0007BF68 File Offset: 0x0007A168
		private void OnValidate()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0007BFA0 File Offset: 0x0007A1A0
		private void Awake()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
			ManagedUIElement.onOpen += this.OnViewOpen;
			ManagedUIElement.onClose += this.OnViewClose;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0007C003 File Offset: 0x0007A203
		private void OnDestroy()
		{
			ManagedUIElement.onOpen -= this.OnViewOpen;
			ManagedUIElement.onClose -= this.OnViewClose;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0007C027 File Offset: 0x0007A227
		private void OnViewClose(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = false;
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0007C053 File Offset: 0x0007A253
		private void OnViewOpen(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = true;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0007C080 File Offset: 0x0007A280
		public void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x040017F8 RID: 6136
		private const bool FunctionEnabled = false;

		// Token: 0x040017F9 RID: 6137
		[SerializeField]
		private View view;

		// Token: 0x040017FA RID: 6138
		[SerializeField]
		private Graphic graphic;
	}
}
