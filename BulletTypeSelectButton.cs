using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class BulletTypeSelectButton : MonoBehaviour
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001BA55 File Offset: 0x00019C55
	public int BulletTypeID
	{
		get
		{
			return this.bulletTypeID;
		}
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0001BA5D File Offset: 0x00019C5D
	public void SetSelection(bool selected)
	{
		this.selectShadow.enabled = selected;
		this.indicator.SetActive(selected);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0001BA77 File Offset: 0x00019C77
	public void Init(int id, int count)
	{
		this.bulletTypeID = id;
		this.bulletCount = count;
		this.SetSelection(false);
		this.RefreshContent();
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0001BA94 File Offset: 0x00019C94
	public void RefreshContent()
	{
		this.nameText.text = this.GetBulletName(this.bulletTypeID);
		this.countText.text = this.bulletCount.ToString();
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0001BAC4 File Offset: 0x00019CC4
	public string GetBulletName(int id)
	{
		if (id > 0)
		{
			return ItemAssetsCollection.GetMetaData(id).DisplayName;
		}
		return "UI_Bullet_NotAssigned".ToPlainText();
	}

	// Token: 0x040005B2 RID: 1458
	private int bulletTypeID;

	// Token: 0x040005B3 RID: 1459
	private int bulletCount;

	// Token: 0x040005B4 RID: 1460
	public BulletTypeHUD bulletTypeHUD;

	// Token: 0x040005B5 RID: 1461
	public TextMeshProUGUI nameText;

	// Token: 0x040005B6 RID: 1462
	public TextMeshProUGUI countText;

	// Token: 0x040005B7 RID: 1463
	public TrueShadow selectShadow;

	// Token: 0x040005B8 RID: 1464
	public GameObject indicator;
}
