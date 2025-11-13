using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E7 RID: 999
	public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06002458 RID: 9304 RVA: 0x0007F154 File Offset: 0x0007D354
		private void Awake()
		{
			this.SetAll(false);
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0007F176 File Offset: 0x0007D376
		private void OnEnable()
		{
			this.SetAll(false);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0007F17F File Offset: 0x0007D37F
		private void OnDisable()
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0007F19C File Offset: 0x0007D39C
		private void SetAll(bool value)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggles)
			{
				if (!(toggleAnimation == null))
				{
					toggleAnimation.SetToggle(value);
				}
			}
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0007F1F8 File Offset: 0x0007D3F8
		public void OnPointerDown(PointerEventData eventData)
		{
			this.SetAll(true);
			if (!this.mute)
			{
				AudioManager.Post("UI/click");
			}
			HardwareSyncingManager.SetEvent("Interact_UI");
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0007F21E File Offset: 0x0007D41E
		public void OnPointerUp(PointerEventData eventData)
		{
			this.SetAll(false);
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0007F227 File Offset: 0x0007D427
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(true);
			}
			if (!this.mute)
			{
				AudioManager.Post("UI/hover");
			}
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0007F255 File Offset: 0x0007D455
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x040018A2 RID: 6306
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x040018A3 RID: 6307
		[SerializeField]
		private List<ToggleAnimation> toggles = new List<ToggleAnimation>();

		// Token: 0x040018A4 RID: 6308
		[SerializeField]
		private bool mute;
	}
}
