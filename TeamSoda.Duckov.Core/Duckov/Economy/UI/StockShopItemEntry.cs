using System;
using DG.Tweening;
using Duckov.UI;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Economy.UI
{
	// Token: 0x0200032C RID: 812
	public class StockShopItemEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x000639B9 File Offset: 0x00061BB9
		private StockShop stockShop
		{
			get
			{
				StockShopView stockShopView = this.master;
				if (stockShopView == null)
				{
					return null;
				}
				return stockShopView.Target;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x000639CC File Offset: 0x00061BCC
		public StockShop.Entry Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000639D4 File Offset: 0x00061BD4
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClick;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000639ED File Offset: 0x00061BED
		private void OnItemDisplayPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000639F6 File Offset: 0x00061BF6
		public Item GetItem()
		{
			return this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00063A10 File Offset: 0x00061C10
		internal void Setup(StockShopView master, StockShop.Entry entry)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = entry;
			Item itemInstanceDirect = this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
			this.itemDisplay.Setup(itemInstanceDirect);
			this.itemDisplay.ShowOperationButtons = false;
			this.itemDisplay.IsStockshopSample = true;
			int stackCount = itemInstanceDirect.StackCount;
			int num = this.stockShop.ConvertPrice(itemInstanceDirect, false);
			this.priceText.text = num.ToString(this.moneyFormat);
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00063AA8 File Offset: 0x00061CA8
		private void RegisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Combine(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged += this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x00063B0C File Offset: 0x00061D0C
		private void UnregisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Remove(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged -= this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x00063B6D File Offset: 0x00061D6D
		private void OnMasterSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00063B75 File Offset: 0x00061D75
		private void OnTargetStockChanged(StockShop.Entry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x00063B7D File Offset: 0x00061D7D
		public bool IsUnlocked()
		{
			return this.target != null && (this.target.ForceUnlock || EconomyManager.IsUnlocked(this.target.ItemTypeID));
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00063BA8 File Offset: 0x00061DA8
		private void Refresh()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			bool active = this.master.GetSelection() == this;
			this.selectionIndicator.SetActive(active);
			bool flag = EconomyManager.IsUnlocked(this.target.ItemTypeID);
			bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID);
			if (this.target.ForceUnlock)
			{
				flag = true;
				flag2 = false;
			}
			this.lockedIndicator.SetActive(!flag && !flag2);
			this.waitingForUnlockIndicator.SetActive(!flag && flag2);
			base.gameObject.SetActive(flag || flag2);
			this.outOfStockIndicator.SetActive(this.Target.CurrentStock <= 0);
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x00063C64 File Offset: 0x00061E64
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (this.master == null)
			{
				return;
			}
			eventData.Use();
			if (EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID))
			{
				EconomyManager.ConfirmUnlock(this.target.ItemTypeID);
			}
			if (this.master.GetSelection() == this)
			{
				this.master.SetSelection(null);
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x00063CDC File Offset: 0x00061EDC
		public void Punch()
		{
			this.selectionIndicator.transform.DOKill(false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			this.selectionIndicator.transform.DOPunchScale(Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00063D3E File Offset: 0x00061F3E
		private void OnEnable()
		{
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00063D51 File Offset: 0x00061F51
		private void OnDisable()
		{
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00063D64 File Offset: 0x00061F64
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.target == null)
			{
				return;
			}
			if (itemTypeID == this.target.ItemTypeID)
			{
				this.Refresh();
			}
		}

		// Token: 0x0400136F RID: 4975
		[SerializeField]
		private string moneyFormat = "n0";

		// Token: 0x04001370 RID: 4976
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001371 RID: 4977
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001372 RID: 4978
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001373 RID: 4979
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x04001374 RID: 4980
		[SerializeField]
		private GameObject waitingForUnlockIndicator;

		// Token: 0x04001375 RID: 4981
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x04001376 RID: 4982
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x04001377 RID: 4983
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x04001378 RID: 4984
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x04001379 RID: 4985
		private StockShopView master;

		// Token: 0x0400137A RID: 4986
		private StockShop.Entry target;
	}
}
