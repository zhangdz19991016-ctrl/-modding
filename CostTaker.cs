using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Economy;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D5 RID: 213
public class CostTaker : InteractableBase
{
	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001DF13 File Offset: 0x0001C113
	public Cost Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000697 RID: 1687 RVA: 0x0001DF1C File Offset: 0x0001C11C
	// (remove) Token: 0x06000698 RID: 1688 RVA: 0x0001DF54 File Offset: 0x0001C154
	public event Action<CostTaker> onPayed;

	// Token: 0x06000699 RID: 1689 RVA: 0x0001DF89 File Offset: 0x0001C189
	protected override bool IsInteractable()
	{
		return this.cost.Enough;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0001DF98 File Offset: 0x0001C198
	protected override void OnInteractFinished()
	{
		if (!this.cost.Enough)
		{
			return;
		}
		if (this.cost.Pay(true, true))
		{
			Action<CostTaker> action = this.onPayed;
			if (action != null)
			{
				action(this);
			}
			UnityEvent<CostTaker> unityEvent = this.onPayedUnityEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001DFE5 File Offset: 0x0001C1E5
	private void OnEnable()
	{
		CostTaker.Register(this);
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0001DFED File Offset: 0x0001C1ED
	private void OnDisable()
	{
		CostTaker.Unregister(this);
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001DFF5 File Offset: 0x0001C1F5
	public static ReadOnlyCollection<CostTaker> ActiveCostTakers
	{
		get
		{
			if (CostTaker._activeCostTakers_ReadOnly == null)
			{
				CostTaker._activeCostTakers_ReadOnly = new ReadOnlyCollection<CostTaker>(CostTaker.activeCostTakers);
			}
			return CostTaker._activeCostTakers_ReadOnly;
		}
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600069E RID: 1694 RVA: 0x0001E014 File Offset: 0x0001C214
	// (remove) Token: 0x0600069F RID: 1695 RVA: 0x0001E048 File Offset: 0x0001C248
	public static event Action<CostTaker> OnCostTakerRegistered;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x060006A0 RID: 1696 RVA: 0x0001E07C File Offset: 0x0001C27C
	// (remove) Token: 0x060006A1 RID: 1697 RVA: 0x0001E0B0 File Offset: 0x0001C2B0
	public static event Action<CostTaker> OnCostTakerUnregistered;

	// Token: 0x060006A2 RID: 1698 RVA: 0x0001E0E3 File Offset: 0x0001C2E3
	public static void Register(CostTaker costTaker)
	{
		CostTaker.activeCostTakers.Add(costTaker);
		Action<CostTaker> onCostTakerRegistered = CostTaker.OnCostTakerRegistered;
		if (onCostTakerRegistered == null)
		{
			return;
		}
		onCostTakerRegistered(costTaker);
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001E100 File Offset: 0x0001C300
	public static void Unregister(CostTaker costTaker)
	{
		if (CostTaker.activeCostTakers.Remove(costTaker))
		{
			Action<CostTaker> onCostTakerUnregistered = CostTaker.OnCostTakerUnregistered;
			if (onCostTakerUnregistered == null)
			{
				return;
			}
			onCostTakerUnregistered(costTaker);
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0001E11F File Offset: 0x0001C31F
	public void SetCost(Cost cost)
	{
		CostTaker.Unregister(this);
		this.cost = cost;
		if (base.isActiveAndEnabled)
		{
			CostTaker.Register(this);
		}
	}

	// Token: 0x04000669 RID: 1641
	[SerializeField]
	private Cost cost;

	// Token: 0x0400066B RID: 1643
	public UnityEvent<CostTaker> onPayedUnityEvent;

	// Token: 0x0400066C RID: 1644
	private static List<CostTaker> activeCostTakers = new List<CostTaker>();

	// Token: 0x0400066D RID: 1645
	private static ReadOnlyCollection<CostTaker> _activeCostTakers_ReadOnly;
}
