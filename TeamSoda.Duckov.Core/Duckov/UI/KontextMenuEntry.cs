using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D0 RID: 976
	public class KontextMenuEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060023A8 RID: 9128 RVA: 0x0007D36F File Offset: 0x0007B56F
		public void NotifyPooled()
		{
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0007D371 File Offset: 0x0007B571
		public void NotifyReleased()
		{
			this.target = null;
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0007D37A File Offset: 0x0007B57A
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.menu != null)
			{
				this.menu.InstanceHide();
			}
			if (this.target != null)
			{
				Action action = this.target.action;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x0007D3B4 File Offset: 0x0007B5B4
		public void Setup(KontextMenu menu, int index, KontextMenuDataEntry data)
		{
			this.menu = menu;
			this.target = data;
			if (this.icon)
			{
				if (data.icon)
				{
					this.icon.sprite = data.icon;
					this.icon.gameObject.SetActive(true);
				}
				else
				{
					this.icon.gameObject.SetActive(false);
				}
			}
			if (this.text)
			{
				if (!string.IsNullOrEmpty(this.target.text))
				{
					this.text.text = this.target.text;
					this.text.gameObject.SetActive(true);
				}
				else
				{
					this.text.gameObject.SetActive(false);
				}
			}
			foreach (FadeElement fadeElement in this.fadeInElements)
			{
				fadeElement.SkipHide();
				fadeElement.Show(this.delayByIndex * (float)index).Forget();
			}
		}

		// Token: 0x04001835 RID: 6197
		[SerializeField]
		private Image icon;

		// Token: 0x04001836 RID: 6198
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001837 RID: 6199
		[SerializeField]
		private float delayByIndex = 0.1f;

		// Token: 0x04001838 RID: 6200
		[SerializeField]
		private List<FadeElement> fadeInElements;

		// Token: 0x04001839 RID: 6201
		private KontextMenu menu;

		// Token: 0x0400183A RID: 6202
		private KontextMenuDataEntry target;
	}
}
