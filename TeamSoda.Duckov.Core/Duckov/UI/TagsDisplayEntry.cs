using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A8 RID: 936
	public class TagsDisplayEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipsProvider
	{
		// Token: 0x0600217C RID: 8572 RVA: 0x00075315 File Offset: 0x00073515
		public string GetTooltipsText()
		{
			if (this.target == null)
			{
				return "";
			}
			return this.target.Description;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00075336 File Offset: 0x00073536
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.target == null)
			{
				return;
			}
			if (!this.target.ShowDescription)
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x0007535B File Offset: 0x0007355B
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00075363 File Offset: 0x00073563
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x0007536B File Offset: 0x0007356B
		public void Setup(Tag tag)
		{
			this.target = tag;
			this.background.color = tag.Color;
			this.text.text = tag.DisplayName;
		}

		// Token: 0x040016AF RID: 5807
		[SerializeField]
		private Image background;

		// Token: 0x040016B0 RID: 5808
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040016B1 RID: 5809
		private Tag target;
	}
}
