using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000321 RID: 801
	public class BuildingContextMenuEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001AC7 RID: 6855 RVA: 0x000614D5 File Offset: 0x0005F6D5
		private void OnEnable()
		{
			this.text.text = this.textKey.ToPlainText();
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06001AC8 RID: 6856 RVA: 0x000614F0 File Offset: 0x0005F6F0
		// (remove) Token: 0x06001AC9 RID: 6857 RVA: 0x00061528 File Offset: 0x0005F728
		public event Action<BuildingContextMenuEntry> onPointerClick;

		// Token: 0x06001ACA RID: 6858 RVA: 0x0006155D File Offset: 0x0005F75D
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<BuildingContextMenuEntry> action = this.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0400132E RID: 4910
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400132F RID: 4911
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey;
	}
}
