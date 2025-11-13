using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Economy.UI
{
	// Token: 0x0200032D RID: 813
	public class StockShopView : View, ISingleSelectionMenu<StockShopItemEntry>
	{
		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x00063DB7 File Offset: 0x00061FB7
		public static StockShopView Instance
		{
			get
			{
				return View.GetViewInstance<StockShopView>();
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x00063DBE File Offset: 0x00061FBE
		private string TextBuy
		{
			get
			{
				return this.textBuy.ToPlainText();
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00063DCB File Offset: 0x00061FCB
		private string TextSoldOut
		{
			get
			{
				return this.textSoldOut.ToPlainText();
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00063DD8 File Offset: 0x00061FD8
		private string TextSell
		{
			get
			{
				return this.textSell.ToPlainText();
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00063DE5 File Offset: 0x00061FE5
		private string TextUnlock
		{
			get
			{
				return this.textUnlock.ToPlainText();
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00063DF2 File Offset: 0x00061FF2
		private string TextLocked
		{
			get
			{
				return this.textLocked.ToPlainText();
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x00063E00 File Offset: 0x00062000
		private PrefabPool<StockShopItemEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StockShopItemEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x00063E59 File Offset: 0x00062059
		private UnityEngine.Object Selection
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay != null)
				{
					return ItemUIUtilities.SelectedItemDisplay;
				}
				if (this.selectedItem != null)
				{
					return this.selectedItem;
				}
				return null;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x00063E84 File Offset: 0x00062084
		public StockShop Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x00063E8C File Offset: 0x0006208C
		protected override void Awake()
		{
			base.Awake();
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x00063EC1 File Offset: 0x000620C1
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x00063EDA File Offset: 0x000620DA
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.OnInteractionButtonClicked();
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00063EEB File Offset: 0x000620EB
		private void FixedUpdate()
		{
			this.RefreshCountDown();
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00063EF4 File Offset: 0x000620F4
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				this.refreshCountDown.text = "-";
			}
			TimeSpan nextRefreshETA = this.target.NextRefreshETA;
			int days = nextRefreshETA.Days;
			int hours = nextRefreshETA.Hours;
			int minutes = nextRefreshETA.Minutes;
			int seconds = nextRefreshETA.Seconds;
			this.refreshCountDown.text = string.Format("{0}{1:00}:{2:00}:{3:00}", new object[]
			{
				(days > 0) ? (days.ToString() + " - ") : "",
				hours,
				minutes,
				seconds
			});
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x00063FA4 File Offset: 0x000621A4
		private void OnInteractionButtonClicked()
		{
			if (this.Selection == null)
			{
				return;
			}
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				this.Target.Sell(itemDisplay.Target).Forget();
				AudioManager.Post(this.sfx_Sell);
				ItemUIUtilities.Select(null);
				this.OnSelectionChanged();
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				int itemTypeID = stockShopItemEntry.Target.ItemTypeID;
				if (stockShopItemEntry.IsUnlocked())
				{
					this.BuyTask(itemTypeID).Forget();
					return;
				}
				if (EconomyManager.IsWaitingForUnlockConfirm(itemTypeID))
				{
					EconomyManager.ConfirmUnlock(itemTypeID);
				}
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0006403C File Offset: 0x0006223C
		private UniTask BuyTask(int itemTypeID)
		{
			StockShopView.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShopView.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00064088 File Offset: 0x00062288
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold += this.OnAfterItemSold;
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000640EC File Offset: 0x000622EC
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold -= this.OnAfterItemSold;
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0006414E File Offset: 0x0006234E
		private void OnNextPage(UIInputEventData data)
		{
			this.playerStorageDisplay.NextPage();
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0006415B File Offset: 0x0006235B
		private void OnPreviousPage(UIInputEventData data)
		{
			this.playerStorageDisplay.PreviousPage();
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00064168 File Offset: 0x00062368
		private void OnAfterItemSold(StockShop shop)
		{
			this.RefreshInteractionButton();
			this.RefreshStockText();
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00064176 File Offset: 0x00062376
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.details.Target == null)
			{
				return;
			}
			if (itemTypeID == this.details.Target.TypeID)
			{
				this.RefreshInteractionButton();
				this.RefreshStockText();
			}
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000641AB File Offset: 0x000623AB
		private void OnItemUIUtilitiesSelectionChanged()
		{
			if (this.selectedItem != null && ItemUIUtilities.SelectedItemDisplay != null)
			{
				this.selectedItem = null;
			}
			this.OnSelectionChanged();
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000641D8 File Offset: 0x000623D8
		private void OnSelectionChanged()
		{
			Action action = this.onSelectionChanged;
			if (action != null)
			{
				action();
			}
			if (this.Selection == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			Item x = null;
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				x = stockShopItemEntry.GetItem();
			}
			else
			{
				ItemDisplay itemDisplay = this.Selection as ItemDisplay;
				if (itemDisplay != null)
				{
					x = itemDisplay.Target;
				}
			}
			if (x == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			this.details.Setup(x);
			this.RefreshStockText();
			this.RefreshInteractionButton();
			this.RefreshCountDown();
			this.detailsFadeGroup.Show();
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x00064280 File Offset: 0x00062480
		private void RefreshStockText()
		{
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				this.stockText.gameObject.SetActive(true);
				this.stockText.text = this.stockTextFormat.Format(new
				{
					text = this.stockTextKey.ToPlainText(),
					current = stockShopItemEntry.Target.CurrentStock,
					max = stockShopItemEntry.Target.MaxStock
				});
				return;
			}
			if (this.Selection is ItemDisplay)
			{
				this.stockText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x00064308 File Offset: 0x00062508
		public StockShopItemEntry GetSelection()
		{
			return this.Selection as StockShopItemEntry;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00064315 File Offset: 0x00062515
		public bool SetSelection(StockShopItemEntry selection)
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				ItemUIUtilities.Select(null);
			}
			this.selectedItem = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00064338 File Offset: 0x00062538
		internal void Setup(StockShop target)
		{
			this.target = target;
			this.detailsFadeGroup.SkipHide();
			this.merchantNameText.text = target.DisplayName;
			LevelManager instance = LevelManager.Instance;
			Inventory inventory;
			if (instance == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			this.playerInventoryDisplay.Setup(inventory2, null, (Item e) => e == null || e.CanBeSold, false, null);
			if (PetProxy.PetInventory != null)
			{
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, (Item e) => e == null || e.CanBeSold, false, null);
				this.petInventoryDisplay.gameObject.SetActive(true);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.playerStorageDisplay.gameObject.SetActive(true);
				this.playerStorageDisplay.Setup(inventory3, null, (Item e) => e == null || e.CanBeSold, false, null);
			}
			else
			{
				this.playerStorageDisplay.gameObject.SetActive(false);
			}
			this.EntryPool.ReleaseAll();
			Transform parent = this.entryTemplate.transform.parent;
			foreach (StockShop.Entry entry in target.entries)
			{
				if (entry.Show)
				{
					StockShopItemEntry stockShopItemEntry = this.EntryPool.Get(parent);
					stockShopItemEntry.Setup(this, entry);
					stockShopItemEntry.transform.SetAsLastSibling();
				}
			}
			TradingUIUtilities.ActiveMerchant = target;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00064510 File Offset: 0x00062710
		private void RefreshInteractionButton()
		{
			this.cannotSellIndicator.SetActive(false);
			this.cashOnlyIndicator.SetActive(!this.Target.AccountAvaliable);
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				bool canBeSold = itemDisplay.Target.CanBeSold;
				this.interactionButton.interactable = canBeSold;
				this.priceDisplay.gameObject.SetActive(true);
				this.lockDisplay.gameObject.SetActive(false);
				this.interactionText.text = this.TextSell;
				this.interactionButtonImage.color = this.buttonColor_Interactable;
				this.priceText.text = this.<RefreshInteractionButton>g__GetPriceText|71_1(itemDisplay.Target, true);
				this.cannotSellIndicator.SetActive(!itemDisplay.Target.CanBeSold);
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				bool flag = stockShopItemEntry.IsUnlocked();
				bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(stockShopItemEntry.Target.ItemTypeID);
				this.interactionButton.interactable = (flag || flag2);
				this.priceDisplay.gameObject.SetActive(flag);
				this.lockDisplay.gameObject.SetActive(!flag);
				this.cannotSellIndicator.SetActive(false);
				if (flag)
				{
					Item item = stockShopItemEntry.GetItem();
					int num = this.<RefreshInteractionButton>g__GetPrice|71_0(item, false);
					bool enough = new Cost((long)num).Enough;
					this.priceText.text = num.ToString("n0");
					if (stockShopItemEntry.Target.CurrentStock > 0)
					{
						this.interactionText.text = this.TextBuy;
						this.interactionButtonImage.color = (enough ? this.buttonColor_Interactable : this.buttonColor_NotInteractable);
						return;
					}
					this.interactionButton.interactable = false;
					this.interactionText.text = this.TextSoldOut;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
					return;
				}
				else
				{
					if (flag2)
					{
						this.interactionText.text = this.TextUnlock;
						this.interactionButtonImage.color = this.buttonColor_Interactable;
						return;
					}
					this.interactionText.text = this.TextLocked;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
				}
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00064745 File Offset: 0x00062945
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00064758 File Offset: 0x00062958
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0006476B File Offset: 0x0006296B
		internal void SetupAndShow(StockShop stockShop)
		{
			ItemUIUtilities.Select(null);
			this.SetSelection(null);
			this.Setup(stockShop);
			base.Open(null);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00064802 File Offset: 0x00062A02
		[CompilerGenerated]
		private int <RefreshInteractionButton>g__GetPrice|71_0(Item item, bool selling)
		{
			return this.Target.ConvertPrice(item, selling);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00064814 File Offset: 0x00062A14
		[CompilerGenerated]
		private string <RefreshInteractionButton>g__GetPriceText|71_1(Item item, bool selling)
		{
			return this.<RefreshInteractionButton>g__GetPrice|71_0(item, selling).ToString("n0");
		}

		// Token: 0x0400137B RID: 4987
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400137C RID: 4988
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x0400137D RID: 4989
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x0400137E RID: 4990
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x04001380 RID: 4992
		[SerializeField]
		private InventoryDisplay playerStorageDisplay;

		// Token: 0x04001381 RID: 4993
		[SerializeField]
		private StockShopItemEntry entryTemplate;

		// Token: 0x04001382 RID: 4994
		[SerializeField]
		private TextMeshProUGUI stockText;

		// Token: 0x04001383 RID: 4995
		[SerializeField]
		[LocalizationKey("Default")]
		private string stockTextKey = "UI_Stock";

		// Token: 0x04001384 RID: 4996
		[SerializeField]
		private string stockTextFormat = "{text} {current}/{max}";

		// Token: 0x04001385 RID: 4997
		[SerializeField]
		private TextMeshProUGUI merchantNameText;

		// Token: 0x04001386 RID: 4998
		[SerializeField]
		private Button interactionButton;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		private Image interactionButtonImage;

		// Token: 0x04001388 RID: 5000
		[SerializeField]
		private Color buttonColor_Interactable;

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		private Color buttonColor_NotInteractable;

		// Token: 0x0400138A RID: 5002
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		private GameObject cashOnlyIndicator;

		// Token: 0x0400138C RID: 5004
		[SerializeField]
		private GameObject cannotSellIndicator;

		// Token: 0x0400138D RID: 5005
		[LocalizationKey("Default")]
		[SerializeField]
		private string textBuy = "购买";

		// Token: 0x0400138E RID: 5006
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSoldOut = "已售罄";

		// Token: 0x0400138F RID: 5007
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSell = "出售";

		// Token: 0x04001390 RID: 5008
		[LocalizationKey("Default")]
		[SerializeField]
		private string textUnlock = "解锁";

		// Token: 0x04001391 RID: 5009
		[LocalizationKey("Default")]
		[SerializeField]
		private string textLocked = "已锁定";

		// Token: 0x04001392 RID: 5010
		[SerializeField]
		private GameObject priceDisplay;

		// Token: 0x04001393 RID: 5011
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001394 RID: 5012
		[SerializeField]
		private GameObject lockDisplay;

		// Token: 0x04001395 RID: 5013
		[SerializeField]
		private FadeGroup clickBlockerFadeGroup;

		// Token: 0x04001396 RID: 5014
		[SerializeField]
		private TextMeshProUGUI refreshCountDown;

		// Token: 0x04001397 RID: 5015
		private string sfx_Buy = "UI/buy";

		// Token: 0x04001398 RID: 5016
		private string sfx_Sell = "UI/sell";

		// Token: 0x04001399 RID: 5017
		private PrefabPool<StockShopItemEntry> _entryPool;

		// Token: 0x0400139A RID: 5018
		private StockShop target;

		// Token: 0x0400139B RID: 5019
		private StockShopItemEntry selectedItem;

		// Token: 0x0400139C RID: 5020
		public Action onSelectionChanged;
	}
}
