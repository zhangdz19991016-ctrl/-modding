using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001AD RID: 429
public class CraftViewFilterBtnEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000CCC RID: 3276 RVA: 0x000361BA File Offset: 0x000343BA
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.master == null)
		{
			return;
		}
		this.master.SetFilter(this.index);
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x000361DC File Offset: 0x000343DC
	public void Setup(CraftView master, CraftView.FilterInfo filterInfo, int index, bool selected)
	{
		this.master = master;
		this.info = filterInfo;
		this.index = index;
		this.icon.sprite = filterInfo.icon;
		this.displayNameText.text = filterInfo.displayNameKey.ToPlainText();
		this.selectedIndicator.SetActive(selected);
	}

	// Token: 0x04000B1F RID: 2847
	[SerializeField]
	private Image icon;

	// Token: 0x04000B20 RID: 2848
	[SerializeField]
	private TextMeshProUGUI displayNameText;

	// Token: 0x04000B21 RID: 2849
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000B22 RID: 2850
	private CraftView.FilterInfo info;

	// Token: 0x04000B23 RID: 2851
	private CraftView master;

	// Token: 0x04000B24 RID: 2852
	private int index;
}
