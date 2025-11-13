using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000124 RID: 292
public class QuestRequiredItem : MonoBehaviour
{
	// Token: 0x060009A7 RID: 2471 RVA: 0x0002A530 File Offset: 0x00028730
	public void Set(int itemTypeID, int count = 1)
	{
		if (itemTypeID <= 0 || count <= 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
		if (metaData.id == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.icon.sprite = metaData.icon;
		this.text.text = string.Format("{0} x{1}", metaData.DisplayName, count);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private Image icon;

	// Token: 0x04000890 RID: 2192
	[SerializeField]
	private TextMeshProUGUI text;
}
