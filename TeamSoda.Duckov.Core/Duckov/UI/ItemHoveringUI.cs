using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x02000382 RID: 898
	public class ItemHoveringUI : MonoBehaviour
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x0006DE1C File Offset: 0x0006C01C
		// (set) Token: 0x06001F36 RID: 7990 RVA: 0x0006DE23 File Offset: 0x0006C023
		public static ItemHoveringUI Instance { get; private set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0006DE2B File Offset: 0x0006C02B
		public RectTransform LayoutParent
		{
			get
			{
				return this.layoutParent;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06001F38 RID: 7992 RVA: 0x0006DE34 File Offset: 0x0006C034
		// (remove) Token: 0x06001F39 RID: 7993 RVA: 0x0006DE68 File Offset: 0x0006C068
		public static event Action<ItemHoveringUI, ItemMetaData> onSetupMeta;

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06001F3A RID: 7994 RVA: 0x0006DE9C File Offset: 0x0006C09C
		// (remove) Token: 0x06001F3B RID: 7995 RVA: 0x0006DED0 File Offset: 0x0006C0D0
		public static event Action<ItemHoveringUI, Item> onSetupItem;

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x0006DF03 File Offset: 0x0006C103
		// (set) Token: 0x06001F3D RID: 7997 RVA: 0x0006DF0A File Offset: 0x0006C10A
		public static int DisplayingItemID { get; private set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0006DF12 File Offset: 0x0006C112
		public static bool Shown
		{
			get
			{
				return !(ItemHoveringUI.Instance == null) && ItemHoveringUI.Instance.fadeGroup.IsShown;
			}
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006DF34 File Offset: 0x0006C134
		private void Awake()
		{
			ItemHoveringUI.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			ItemDisplay.OnPointerEnterItemDisplay += this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay += this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter += this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit += this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter += this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit += this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0006DFC8 File Offset: 0x0006C1C8
		private void OnDestroy()
		{
			ItemDisplay.OnPointerEnterItemDisplay -= this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay -= this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter -= this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit -= this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter -= this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit -= this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0006E03B File Offset: 0x0006C23B
		private void OnMouseExitMetaDisplay(ItemMetaDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0006E051 File Offset: 0x0006C251
		private void OnMouseEnterMetaDisplay(ItemMetaDisplay display)
		{
			this.SetupAndShowMeta<ItemMetaDisplay>(display);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0006E05A File Offset: 0x0006C25A
		private void OnMouseExitItemAmountDisplay(ItemAmountDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0006E070 File Offset: 0x0006C270
		private void OnMouseEnterItemAmountDisplay(ItemAmountDisplay display)
		{
			this.SetupAndShowMeta<ItemAmountDisplay>(display);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0006E079 File Offset: 0x0006C279
		private void OnPointerExitItemDisplay(ItemDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0006E08F File Offset: 0x0006C28F
		private void OnPointerEnterItemDisplay(ItemDisplay display)
		{
			this.SetupAndShow(display);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0006E098 File Offset: 0x0006C298
		private void SetupAndShow(ItemDisplay display)
		{
			if (display == null)
			{
				return;
			}
			Item item = display.Target;
			if (item == null)
			{
				return;
			}
			if (item.NeedInspection)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = display;
			this.itemName.text = (item.DisplayName ?? "");
			this.itemDescription.text = (item.Description ?? "");
			this.weightDisplay.gameObject.SetActive(true);
			this.weightDisplay.text = string.Format("{0:0.#} kg", item.TotalWeight);
			this.itemID.text = string.Format("#{0}", item.TypeID);
			ItemHoveringUI.DisplayingItemID = item.TypeID;
			this.itemProperties.gameObject.SetActive(true);
			this.itemProperties.Setup(item);
			this.interactionIndicatorsContainer.SetActive(true);
			this.interactionIndicator_Menu.SetActive(display.ShowOperationButtons);
			this.interactionIndicator_Move.SetActive(display.Movable);
			this.interactionIndicator_Drop.SetActive(display.CanDrop);
			this.interactionIndicator_Use.SetActive(display.CanUse);
			this.interactionIndicator_Split.SetActive(display.CanSplit);
			this.interactionIndicator_LockSort.SetActive(display.CanLockSort);
			this.interactionIndicator_Shortcut.SetActive(display.CanSetShortcut);
			this.usageUtilitiesDisplay.Setup(item);
			this.SetupWishlistInfos(item.TypeID);
			this.SetupBulletDisplay();
			try
			{
				Action<ItemHoveringUI, Item> action = ItemHoveringUI.onSetupItem;
				if (action != null)
				{
					action(this, item);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.RefreshPosition();
			this.SetupRegisteredInfo(item);
			this.fadeGroup.Show();
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0006E270 File Offset: 0x0006C470
		private void SetupRegisteredInfo(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (item.IsRegistered())
			{
				this.registeredIndicator.SetActive(true);
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0006E290 File Offset: 0x0006C490
		private void SetupAndShowMeta<T>(T dataProvider) where T : MonoBehaviour, IItemMetaDataProvider
		{
			if (dataProvider == null)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = dataProvider;
			ItemMetaData metaData = dataProvider.GetMetaData();
			this.itemName.text = metaData.DisplayName;
			this.itemID.text = string.Format("{0}", metaData.id);
			ItemHoveringUI.DisplayingItemID = metaData.id;
			this.itemDescription.text = metaData.Description;
			this.interactionIndicatorsContainer.SetActive(true);
			this.weightDisplay.gameObject.SetActive(false);
			this.bulletTypeDisplay.gameObject.SetActive(false);
			this.itemProperties.gameObject.SetActive(false);
			this.interactionIndicator_Menu.gameObject.SetActive(false);
			this.interactionIndicator_Move.gameObject.SetActive(false);
			this.interactionIndicator_Drop.gameObject.SetActive(false);
			this.interactionIndicator_Use.gameObject.SetActive(false);
			this.usageUtilitiesDisplay.gameObject.SetActive(false);
			this.interactionIndicator_Split.SetActive(false);
			this.interactionIndicator_Shortcut.SetActive(false);
			this.SetupWishlistInfos(metaData.id);
			try
			{
				Action<ItemHoveringUI, ItemMetaData> action = ItemHoveringUI.onSetupMeta;
				if (action != null)
				{
					action(this, metaData);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.RefreshPosition();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0006E414 File Offset: 0x0006C614
		private void SetupBulletDisplay()
		{
			ItemDisplay itemDisplay = this.target as ItemDisplay;
			if (itemDisplay == null)
			{
				return;
			}
			ItemSetting_Gun component = itemDisplay.Target.GetComponent<ItemSetting_Gun>();
			if (component == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(component.TargetBulletID);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0006E480 File Offset: 0x0006C680
		private unsafe void RefreshPosition()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, null, out vector);
			float xMax = this.contents.rect.xMax;
			float yMin = this.contents.rect.yMin;
			float b = this.rectTransform.rect.xMax - xMax;
			float b2 = this.rectTransform.rect.yMin - yMin;
			vector.x = Mathf.Min(vector.x, b);
			vector.y = Mathf.Max(vector.y, b2);
			this.contents.anchoredPosition = vector;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0006E540 File Offset: 0x0006C740
		private void Hide()
		{
			this.fadeGroup.Hide();
			ItemHoveringUI.DisplayingItemID = -1;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0006E554 File Offset: 0x0006C754
		private void Update()
		{
			if (this.fadeGroup.IsShown)
			{
				if (this.target == null || !this.target.isActiveAndEnabled)
				{
					this.Hide();
				}
				ItemDisplay itemDisplay = this.target as ItemDisplay;
				if (itemDisplay != null && itemDisplay.Target == null)
				{
					this.Hide();
				}
			}
			this.RefreshPosition();
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0006E5B8 File Offset: 0x0006C7B8
		private void SetupWishlistInfos(int itemTypeID)
		{
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(itemTypeID);
			bool isManuallyWishlisted = wishlistInfo.isManuallyWishlisted;
			bool isBuildingRequired = wishlistInfo.isBuildingRequired;
			bool isQuestRequired = wishlistInfo.isQuestRequired;
			bool active = isManuallyWishlisted || isBuildingRequired || isQuestRequired;
			this.wishlistIndicator.SetActive(isManuallyWishlisted);
			this.buildingIndicator.SetActive(isBuildingRequired);
			this.questIndicator.SetActive(isQuestRequired);
			this.wishlistInfoParent.SetActive(active);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0006E615 File Offset: 0x0006C815
		internal static void NotifyRefreshWishlistInfo()
		{
			if (ItemHoveringUI.Instance == null)
			{
				return;
			}
			ItemHoveringUI.Instance.SetupWishlistInfos(ItemHoveringUI.DisplayingItemID);
		}

		// Token: 0x0400154C RID: 5452
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400154D RID: 5453
		[SerializeField]
		private RectTransform layoutParent;

		// Token: 0x0400154E RID: 5454
		[SerializeField]
		private RectTransform contents;

		// Token: 0x0400154F RID: 5455
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001550 RID: 5456
		[SerializeField]
		private TextMeshProUGUI itemName;

		// Token: 0x04001551 RID: 5457
		[SerializeField]
		private TextMeshProUGUI weightDisplay;

		// Token: 0x04001552 RID: 5458
		[SerializeField]
		private TextMeshProUGUI itemDescription;

		// Token: 0x04001553 RID: 5459
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x04001554 RID: 5460
		[SerializeField]
		private ItemPropertiesDisplay itemProperties;

		// Token: 0x04001555 RID: 5461
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x04001556 RID: 5462
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x04001557 RID: 5463
		[SerializeField]
		private GameObject interactionIndicatorsContainer;

		// Token: 0x04001558 RID: 5464
		[SerializeField]
		private GameObject interactionIndicator_Move;

		// Token: 0x04001559 RID: 5465
		[SerializeField]
		private GameObject interactionIndicator_Menu;

		// Token: 0x0400155A RID: 5466
		[SerializeField]
		private GameObject interactionIndicator_Drop;

		// Token: 0x0400155B RID: 5467
		[SerializeField]
		private GameObject interactionIndicator_Use;

		// Token: 0x0400155C RID: 5468
		[SerializeField]
		private GameObject interactionIndicator_Split;

		// Token: 0x0400155D RID: 5469
		[SerializeField]
		private GameObject interactionIndicator_LockSort;

		// Token: 0x0400155E RID: 5470
		[SerializeField]
		private GameObject interactionIndicator_Shortcut;

		// Token: 0x0400155F RID: 5471
		[SerializeField]
		private GameObject wishlistInfoParent;

		// Token: 0x04001560 RID: 5472
		[SerializeField]
		private GameObject wishlistIndicator;

		// Token: 0x04001561 RID: 5473
		[SerializeField]
		private GameObject buildingIndicator;

		// Token: 0x04001562 RID: 5474
		[SerializeField]
		private GameObject questIndicator;

		// Token: 0x04001563 RID: 5475
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x04001567 RID: 5479
		private MonoBehaviour target;
	}
}
