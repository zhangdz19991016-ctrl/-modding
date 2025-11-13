using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001AE RID: 430
public class CraftView_ListEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0003623A File Offset: 0x0003443A
	// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00036242 File Offset: 0x00034442
	public CraftView Master { get; private set; }

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003624B File Offset: 0x0003444B
	// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00036253 File Offset: 0x00034453
	public CraftingFormula Formula { get; private set; }

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0003625C File Offset: 0x0003445C
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0003626F File Offset: 0x0003446F
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x00036284 File Offset: 0x00034484
	public void Setup(CraftView master, CraftingFormula formula)
	{
		this.Master = master;
		this.Formula = formula;
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.Formula.result.id);
		this.icon.sprite = metaData.icon;
		this.nameText.text = string.Format("{0} x{1}", metaData.DisplayName, formula.result.amount);
		this.Refresh();
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x000362F8 File Offset: 0x000344F8
	public void OnPointerClick(PointerEventData eventData)
	{
		CraftView master = this.Master;
		if (master == null)
		{
			return;
		}
		master.SetSelection(this);
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x0003630C File Offset: 0x0003450C
	internal void NotifyUnselected()
	{
		this.Refresh();
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00036314 File Offset: 0x00034514
	internal void NotifySelected()
	{
		this.Refresh();
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x0003631C File Offset: 0x0003451C
	private void Refresh()
	{
		if (this.Master == null)
		{
			return;
		}
		bool active = this.Master.GetSelection() == this;
		Color color = this.normalColor;
		if (this.selectedIndicator != null)
		{
			this.selectedIndicator.SetActive(active);
		}
		if (this.Formula.cost.Enough)
		{
			color = this.normalColor;
		}
		else
		{
			color = this.normalInsufficientColor;
		}
		this.background.color = color;
	}

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	private Color normalInsufficientColor;

	// Token: 0x04000B27 RID: 2855
	[SerializeField]
	private Image icon;

	// Token: 0x04000B28 RID: 2856
	[SerializeField]
	private Image background;

	// Token: 0x04000B29 RID: 2857
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B2A RID: 2858
	[SerializeField]
	private GameObject selectedIndicator;
}
