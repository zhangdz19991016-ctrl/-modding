using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000ED RID: 237
public abstract class ItemSettingBase : MonoBehaviour
{
	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00023C7C File Offset: 0x00021E7C
	public Item Item
	{
		get
		{
			if (this._item == null)
			{
				this._item = base.GetComponent<Item>();
			}
			return this._item;
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00023C9E File Offset: 0x00021E9E
	public void Awake()
	{
		if (this.Item)
		{
			this.SetMarkerParam(this.Item);
			this.OnInit();
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00023CBF File Offset: 0x00021EBF
	public virtual void OnInit()
	{
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00023CC1 File Offset: 0x00021EC1
	public virtual void Start()
	{
	}

	// Token: 0x060007E9 RID: 2025
	public abstract void SetMarkerParam(Item selfItem);

	// Token: 0x04000769 RID: 1897
	protected Item _item;
}
