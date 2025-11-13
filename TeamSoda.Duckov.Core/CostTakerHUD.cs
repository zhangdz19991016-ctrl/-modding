using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class CostTakerHUD : MonoBehaviour
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001E150 File Offset: 0x0001C350
	private PrefabPool<CostTakerHUD_Entry> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<CostTakerHUD_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0001E189 File Offset: 0x0001C389
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
		this.ShowAll();
		CostTaker.OnCostTakerRegistered += this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered += this.OnCostTakerUnregistered;
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
	private void OnDestroy()
	{
		CostTaker.OnCostTakerRegistered -= this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered -= this.OnCostTakerUnregistered;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0001E1E8 File Offset: 0x0001C3E8
	private void OnCostTakerRegistered(CostTaker taker)
	{
		this.ShowHUD(taker);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0001E1F1 File Offset: 0x0001C3F1
	private void OnCostTakerUnregistered(CostTaker taker)
	{
		this.HideHUD(taker);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0001E1FA File Offset: 0x0001C3FA
	private void Start()
	{
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0001E1FC File Offset: 0x0001C3FC
	private void ShowAll()
	{
		this.EntryPool.ReleaseAll();
		foreach (CostTaker costTaker in CostTaker.ActiveCostTakers)
		{
			this.ShowHUD(costTaker);
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0001E254 File Offset: 0x0001C454
	private void ShowHUD(CostTaker costTaker)
	{
		this.EntryPool.Get(null).Setup(costTaker);
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0001E268 File Offset: 0x0001C468
	private void HideHUD(CostTaker costTaker)
	{
		CostTakerHUD_Entry costTakerHUD_Entry = this.EntryPool.Find((CostTakerHUD_Entry e) => e.gameObject.activeSelf && e.Target == costTaker);
		if (costTakerHUD_Entry == null)
		{
			return;
		}
		this.EntryPool.Release(costTakerHUD_Entry);
	}

	// Token: 0x04000670 RID: 1648
	[SerializeField]
	private CostTakerHUD_Entry entryTemplate;

	// Token: 0x04000671 RID: 1649
	private PrefabPool<CostTakerHUD_Entry> _entryPool;
}
