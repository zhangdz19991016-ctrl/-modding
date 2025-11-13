using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000204 RID: 516
public class ItemRepair_RepairAllPanel : MonoBehaviour
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
	private PrefabPool<ItemDisplay> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, null, null, null, null, true, 10, 10000, delegate(ItemDisplay e)
				{
					e.onPointerClick += this.OnPointerClickEntry;
				});
			}
			return this._pool;
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0003CA0C File Offset: 0x0003AC0C
	private void OnPointerClickEntry(ItemDisplay display, PointerEventData data)
	{
		data.Use();
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0003CA14 File Offset: 0x0003AC14
	private void Awake()
	{
		this.itemDisplayTemplate.gameObject.SetActive(false);
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0003CA44 File Offset: 0x0003AC44
	private void OnButtonClicked()
	{
		if (this.master == null)
		{
			return;
		}
		List<Item> allEquippedItems = this.master.GetAllEquippedItems();
		this.master.RepairItems(allEquippedItems);
		this.needsRefresh = true;
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x0003CA7F File Offset: 0x0003AC7F
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone += this.OnRepairOptionDone;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x0003CAA3 File Offset: 0x0003ACA3
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone -= this.OnRepairOptionDone;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0003CAC7 File Offset: 0x0003ACC7
	public void Setup(ItemRepairView master)
	{
		this.master = master;
		this.Refresh();
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0003CAD6 File Offset: 0x0003ACD6
	private void OnPlayerItemOperation()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x0003CADF File Offset: 0x0003ACDF
	private void OnRepairOptionDone()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0003CAE8 File Offset: 0x0003ACE8
	private void Refresh()
	{
		this.needsRefresh = false;
		this.Pool.ReleaseAll();
		List<Item> list = (from e in this.master.GetAllEquippedItems()
		where e.Durability < e.MaxDurabilityWithLoss
		select e).ToList<Item>();
		int num = 0;
		if (list != null && list.Count > 0)
		{
			foreach (Item target in list)
			{
				this.Pool.Get(null).Setup(target);
			}
			num = this.master.CalculateRepairPrice(list);
			this.placeholder.SetActive(false);
			Cost cost = new Cost((long)num);
			bool enough = cost.Enough;
			this.button.interactable = enough;
		}
		else
		{
			this.placeholder.SetActive(true);
			this.button.interactable = false;
		}
		this.priceDisplay.text = num.ToString();
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x0003CC00 File Offset: 0x0003AE00
	private void Update()
	{
		if (this.needsRefresh)
		{
			this.Refresh();
		}
	}

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private ItemRepairView master;

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private TextMeshProUGUI priceDisplay;

	// Token: 0x04000C83 RID: 3203
	[SerializeField]
	private ItemDisplay itemDisplayTemplate;

	// Token: 0x04000C84 RID: 3204
	[SerializeField]
	private Button button;

	// Token: 0x04000C85 RID: 3205
	[SerializeField]
	private GameObject placeholder;

	// Token: 0x04000C86 RID: 3206
	private PrefabPool<ItemDisplay> _pool;

	// Token: 0x04000C87 RID: 3207
	private bool needsRefresh;
}
