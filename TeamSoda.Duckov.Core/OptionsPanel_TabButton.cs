using System;
using Duckov.Options.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001E2 RID: 482
public class OptionsPanel_TabButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000E6A RID: 3690 RVA: 0x0003A8DB File Offset: 0x00038ADB
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<OptionsPanel_TabButton, PointerEventData> action = this.onClicked;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0003A8F0 File Offset: 0x00038AF0
	internal void NotifySelectionChanged(OptionsPanel optionsPanel, OptionsPanel_TabButton selection)
	{
		bool active = selection == this;
		this.tab.SetActive(active);
		this.selectedIndicator.SetActive(active);
	}

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private GameObject tab;

	// Token: 0x04000BF7 RID: 3063
	public Action<OptionsPanel_TabButton, PointerEventData> onClicked;
}
