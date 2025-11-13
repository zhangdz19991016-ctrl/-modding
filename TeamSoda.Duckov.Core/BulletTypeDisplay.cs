using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class BulletTypeDisplay : MonoBehaviour
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0003B842 File Offset: 0x00039A42
	[LocalizationKey("Default")]
	private string NotAssignedTextKey
	{
		get
		{
			return "UI_Bullet_NotAssigned";
		}
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0003B84C File Offset: 0x00039A4C
	internal void Setup(int targetBulletID)
	{
		if (targetBulletID < 0)
		{
			this.bulletDisplayName.text = this.NotAssignedTextKey.ToPlainText();
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(targetBulletID);
		this.bulletDisplayName.text = metaData.DisplayName;
	}

	// Token: 0x04000C41 RID: 3137
	[SerializeField]
	private TextMeshProUGUI bulletDisplayName;
}
