using System;
using DG.Tweening;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A1 RID: 929
	public class ItemDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
	{
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x00072A66 File Offset: 0x00070C66
		private Sprite FallbackIcon
		{
			get
			{
				return GameplayDataSettings.UIStyle.FallbackItemIcon;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x00072A72 File Offset: 0x00070C72
		// (set) Token: 0x060020C4 RID: 8388 RVA: 0x00072A7A File Offset: 0x00070C7A
		public Item Target { get; private set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x00072A83 File Offset: 0x00070C83
		// (set) Token: 0x060020C6 RID: 8390 RVA: 0x00072A8B File Offset: 0x00070C8B
		internal Action releaseAction { get; set; }

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x060020C7 RID: 8391 RVA: 0x00072A94 File Offset: 0x00070C94
		// (remove) Token: 0x060020C8 RID: 8392 RVA: 0x00072ACC File Offset: 0x00070CCC
		internal event Action<ItemDisplay, PointerEventData> onDoubleClicked;

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x060020C9 RID: 8393 RVA: 0x00072B04 File Offset: 0x00070D04
		// (remove) Token: 0x060020CA RID: 8394 RVA: 0x00072B3C File Offset: 0x00070D3C
		public event Action<PointerEventData> onReceiveDrop;

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x00072B71 File Offset: 0x00070D71
		public bool Selected
		{
			get
			{
				return ItemUIUtilities.SelectedItemDisplay == this;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x00072B80 File Offset: 0x00070D80
		private PrefabPool<SlotIndicator> SlotIndicatorPool
		{
			get
			{
				if (this._slotIndicatorPool == null)
				{
					if (this.slotIndicatorTemplate == null)
					{
						Debug.LogError("SI is null", base.gameObject);
					}
					this._slotIndicatorPool = new PrefabPool<SlotIndicator>(this.slotIndicatorTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._slotIndicatorPool;
			}
		}

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x060020CD RID: 8397 RVA: 0x00072BD8 File Offset: 0x00070DD8
		// (remove) Token: 0x060020CE RID: 8398 RVA: 0x00072C0C File Offset: 0x00070E0C
		public static event Action<ItemDisplay> OnPointerEnterItemDisplay;

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x060020CF RID: 8399 RVA: 0x00072C40 File Offset: 0x00070E40
		// (remove) Token: 0x060020D0 RID: 8400 RVA: 0x00072C74 File Offset: 0x00070E74
		public static event Action<ItemDisplay> OnPointerExitItemDisplay;

		// Token: 0x060020D1 RID: 8401 RVA: 0x00072CA8 File Offset: 0x00070EA8
		public void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			this.slotIndicatorTemplate.gameObject.SetActive(false);
			if (target == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.icon.color = Color.white;
				this.icon.sprite = target.Icon;
				if (this.icon.sprite == null)
				{
					this.icon.sprite = this.FallbackIcon;
				}
				this.icon.gameObject.SetActive(true);
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
				this.displayQualityShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.displayQualityShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.displayQualityShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				bool stackable = this.Target.Stackable;
				this.countGameObject.SetActive(stackable);
				this.nameText.text = this.Target.DisplayName;
				if (target.Slots != null)
				{
					foreach (Slot target2 in target.Slots)
					{
						this.SlotIndicatorPool.Get(null).Setup(target2);
					}
				}
			}
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x060020D2 RID: 8402 RVA: 0x00072E24 File Offset: 0x00071024
		// (remove) Token: 0x060020D3 RID: 8403 RVA: 0x00072E5C File Offset: 0x0007105C
		public event Action<ItemDisplay, PointerEventData> onPointerClick;

		// Token: 0x060020D4 RID: 8404 RVA: 0x00072E94 File Offset: 0x00071094
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged += this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy += this.OnTargetDestroy;
			this.Target.onSetStackCount += this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged += this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00072F34 File Offset: 0x00071134
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged -= this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy -= this.OnTargetDestroy;
			this.Target.onSetStackCount -= this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged -= this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00072FCE File Offset: 0x000711CE
		private void OnWishlistChanged(int type)
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.TypeID == type)
			{
				this.RefreshWishlistInfo();
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00072FF3 File Offset: 0x000711F3
		private void OnTargetDurabilityChanged(Item item)
		{
			this.Refresh();
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00072FFB File Offset: 0x000711FB
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00072FFD File Offset: 0x000711FD
		private void OnTargetSetStackCount(Item item)
		{
			if (item != this.Target)
			{
				Debug.LogError("触发事件的Item不匹配!");
			}
			this.Refresh();
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0007301D File Offset: 0x0007121D
		private void OnItemUtilitiesSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00073025 File Offset: 0x00071225
		private void OnTargetInspectionStateChanged(Item item)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00073033 File Offset: 0x00071233
		private void Clear()
		{
			this.SlotIndicatorPool.ReleaseAll();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00073040 File Offset: 0x00071240
		private void SetupEmpty()
		{
			this.icon.sprite = EmptySprite.Get();
			this.icon.color = Color.clear;
			this.countText.text = string.Empty;
			this.nameText.text = string.Empty;
			this.durabilityFill.fillAmount = 0f;
			this.durabilityLoss.fillAmount = 0f;
			this.durabilityZeroIndicator.gameObject.SetActive(false);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000730C0 File Offset: 0x000712C0
		private void Refresh()
		{
			if (this == null)
			{
				Debug.Log("NULL");
				return;
			}
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.Target == null)
			{
				this.HideMainContentAndDisableControl();
				this.HideInspectionElements();
				if (ItemUIUtilities.SelectedItemDisplayRaw == this)
				{
					ItemUIUtilities.Select(null);
				}
			}
			else if (this.Target.NeedInspection)
			{
				this.HideMainContentAndDisableControl();
				this.ShowInspectionElements();
			}
			else
			{
				this.HideInspectionElements();
				this.ShowMainContentAndEnableControl();
			}
			this.selectionIndicator.gameObject.SetActive(this.Selected);
			this.RefreshWishlistInfo();
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x0007315C File Offset: 0x0007135C
		private void RefreshWishlistInfo()
		{
			if (this.Target == null || this.Target.NeedInspection)
			{
				this.wishlistedIndicator.SetActive(false);
				this.questRequiredIndicator.SetActive(false);
				this.buildingRequiredIndicator.SetActive(false);
				return;
			}
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(this.Target.TypeID);
			this.wishlistedIndicator.SetActive(wishlistInfo.isManuallyWishlisted);
			this.questRequiredIndicator.SetActive(wishlistInfo.isQuestRequired);
			this.buildingRequiredIndicator.SetActive(wishlistInfo.isBuildingRequired);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000731F0 File Offset: 0x000713F0
		private void HideMainContentAndDisableControl()
		{
			this.mainContentShown = false;
			if (this.mainContentShown && ItemUIUtilities.SelectedItemDisplay == this)
			{
				ItemUIUtilities.Select(null);
			}
			this.interactionEventReceiver.raycastTarget = false;
			this.icon.gameObject.SetActive(false);
			this.countGameObject.SetActive(false);
			this.durabilityGameObject.SetActive(false);
			this.durabilityZeroIndicator.gameObject.SetActive(false);
			this.nameContainer.SetActive(false);
			this.slotIndicatorContainer.SetActive(false);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00073280 File Offset: 0x00071480
		private void ShowMainContentAndEnableControl()
		{
			this.mainContentShown = true;
			this.interactionEventReceiver.raycastTarget = true;
			this.icon.gameObject.SetActive(true);
			this.nameContainer.SetActive(true);
			this.countText.text = (this.Target.Stackable ? this.Target.StackCount.ToString() : string.Empty);
			bool useDurability = this.Target.UseDurability;
			if (useDurability)
			{
				float num = this.Target.Durability / this.Target.MaxDurability;
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityFillColorOverT.Evaluate(num);
				this.durabilityZeroIndicator.SetActive(this.Target.Durability <= 0f);
				this.durabilityLoss.fillAmount = this.Target.DurabilityLoss;
			}
			else
			{
				this.durabilityZeroIndicator.gameObject.SetActive(false);
			}
			this.countGameObject.SetActive(this.Target.Stackable);
			this.durabilityGameObject.SetActive(useDurability);
			this.slotIndicatorContainer.SetActive(true);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000733B0 File Offset: 0x000715B0
		private void ShowInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(true);
			bool inspecting = this.Target.Inspecting;
			if (this.inspectingElement)
			{
				this.inspectingElement.SetActive(inspecting);
			}
			if (this.notInspectingElement)
			{
				this.notInspectingElement.SetActive(!inspecting);
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0007340F File Offset: 0x0007160F
		private void HideInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(false);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00073422 File Offset: 0x00071622
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x0007342A File Offset: 0x0007162A
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			if (this.Selected)
			{
				ItemUIUtilities.Select(null);
			}
			this.UnregisterEvents();
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00073451 File Offset: 0x00071651
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			this.isBeingDestroyed = true;
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x00073471 File Offset: 0x00071671
		public static PrefabPool<ItemDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.ItemDisplayPool;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x0007347D File Offset: 0x0007167D
		// (set) Token: 0x060020E9 RID: 8425 RVA: 0x00073485 File Offset: 0x00071685
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020EA RID: 8426 RVA: 0x0007348E File Offset: 0x0007168E
		// (set) Token: 0x060020EB RID: 8427 RVA: 0x00073496 File Offset: 0x00071696
		public bool Editable { get; set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020EC RID: 8428 RVA: 0x0007349F File Offset: 0x0007169F
		// (set) Token: 0x060020ED RID: 8429 RVA: 0x000734A7 File Offset: 0x000716A7
		public bool Movable { get; set; }

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x000734B0 File Offset: 0x000716B0
		// (set) Token: 0x060020EF RID: 8431 RVA: 0x000734B8 File Offset: 0x000716B8
		public bool CanDrop { get; set; }

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000734C1 File Offset: 0x000716C1
		// (set) Token: 0x060020F1 RID: 8433 RVA: 0x000734C9 File Offset: 0x000716C9
		public bool IsStockshopSample { get; set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000734D2 File Offset: 0x000716D2
		public bool CanUse
		{
			get
			{
				return !(this.Target == null) && this.Editable && this.Target.IsUsable(CharacterMainControl.Main);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060020F3 RID: 8435 RVA: 0x00073503 File Offset: 0x00071703
		public bool CanSplit
		{
			get
			{
				return !(this.Target == null) && this.Editable && (this.Movable && this.Target.StackCount > 1);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x00073538 File Offset: 0x00071738
		// (set) Token: 0x060020F5 RID: 8437 RVA: 0x00073540 File Offset: 0x00071740
		public bool CanLockSort { get; internal set; }

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00073549 File Offset: 0x00071749
		public bool CanSetShortcut
		{
			get
			{
				return !(this.Target == null) && this.showOperationButtons && ItemShortcut.IsItemValid(this.Target);
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00073575 File Offset: 0x00071775
		public static ItemDisplay Get()
		{
			return ItemDisplay.Pool.Get(null);
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00073582 File Offset: 0x00071782
		public static void Release(ItemDisplay item)
		{
			ItemDisplay.Pool.Release(item);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0007358F File Offset: 0x0007178F
		public void NotifyPooled()
		{
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00073591 File Offset: 0x00071791
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.SetupEmpty();
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000735A6 File Offset: 0x000717A6
		[ContextMenu("Select")]
		private void Select()
		{
			ItemUIUtilities.Select(this);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000735AE File Offset: 0x000717AE
		public void NotifySelected()
		{
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000735B0 File Offset: 0x000717B0
		public void NotifyUnselected()
		{
			KontextMenu.Hide(this);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000735B8 File Offset: 0x000717B8
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<ItemDisplay, PointerEventData> action = this.onPointerClick;
			if (action != null)
			{
				action(this, eventData);
			}
			if (!eventData.used && eventData.button == PointerEventData.InputButton.Left)
			{
				if (eventData.clickTime - this.lastClickTime <= 0.3f && !this.doubleClickInvoked)
				{
					this.doubleClickInvoked = true;
					Action<ItemDisplay, PointerEventData> action2 = this.onDoubleClicked;
					if (action2 != null)
					{
						action2(this, eventData);
					}
				}
				if (!eventData.used && (!this.Target || !this.Target.NeedInspection))
				{
					if (ItemUIUtilities.SelectedItemDisplay != this)
					{
						this.Select();
						eventData.Use();
					}
					else
					{
						ItemUIUtilities.Select(null);
						eventData.Use();
					}
				}
			}
			if (eventData.clickTime - this.lastClickTime > 0.3f)
			{
				this.doubleClickInvoked = false;
			}
			this.lastClickTime = eventData.clickTime;
			this.Punch();
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00073698 File Offset: 0x00071898
		public void Punch()
		{
			this.selectionIndicator.transform.DOKill(false);
			this.icon.transform.DOKill(false);
			this.backgroundRing.transform.DOKill(false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			this.icon.transform.localScale = Vector3.one;
			this.backgroundRing.transform.localScale = Vector3.one;
			this.selectionIndicator.transform.DOPunchScale(Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
			this.icon.transform.DOPunchScale(Vector3.one * this.iconPunchScale, this.punchDuration, 10, 1f);
			this.backgroundRing.transform.DOPunchScale(Vector3.one * this.backgroundRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000737A4 File Offset: 0x000719A4
		public void OnPointerDown(PointerEventData eventData)
		{
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000737A6 File Offset: 0x000719A6
		public void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000737A8 File Offset: 0x000719A8
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerExitItemDisplay = ItemDisplay.OnPointerExitItemDisplay;
			if (onPointerExitItemDisplay == null)
			{
				return;
			}
			onPointerExitItemDisplay(this);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000737C9 File Offset: 0x000719C9
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerEnterItemDisplay = ItemDisplay.OnPointerEnterItemDisplay;
			if (onPointerEnterItemDisplay == null)
			{
				return;
			}
			onPointerEnterItemDisplay(this);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000737EA File Offset: 0x000719EA
		public void OnDrop(PointerEventData eventData)
		{
			this.HandleDirectDrop(eventData);
			if (eventData.used)
			{
				return;
			}
			Action<PointerEventData> action = this.onReceiveDrop;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00073810 File Offset: 0x00071A10
		private void HandleDirectDrop(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.IsStockshopSample)
			{
				return;
			}
			IItemDragSource component = eventData.pointerDrag.gameObject.GetComponent<IItemDragSource>();
			if (component == null)
			{
				return;
			}
			if (!component.IsEditable())
			{
				return;
			}
			Item item = component.GetItem();
			if (!this.Target.TryPlug(item, false, null, 0))
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			eventData.Use();
		}

		// Token: 0x04001647 RID: 5703
		[SerializeField]
		private Image icon;

		// Token: 0x04001648 RID: 5704
		[SerializeField]
		private TrueShadow displayQualityShadow;

		// Token: 0x04001649 RID: 5705
		[SerializeField]
		private GameObject countGameObject;

		// Token: 0x0400164A RID: 5706
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x0400164B RID: 5707
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x0400164C RID: 5708
		[SerializeField]
		private Graphic interactionEventReceiver;

		// Token: 0x0400164D RID: 5709
		[SerializeField]
		private GameObject backgroundRing;

		// Token: 0x0400164E RID: 5710
		[SerializeField]
		private GameObject inspectionElementRoot;

		// Token: 0x0400164F RID: 5711
		[SerializeField]
		private GameObject inspectingElement;

		// Token: 0x04001650 RID: 5712
		[SerializeField]
		private GameObject notInspectingElement;

		// Token: 0x04001651 RID: 5713
		[SerializeField]
		private GameObject nameContainer;

		// Token: 0x04001652 RID: 5714
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001653 RID: 5715
		[SerializeField]
		private GameObject durabilityGameObject;

		// Token: 0x04001654 RID: 5716
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x04001655 RID: 5717
		[SerializeField]
		private Gradient durabilityFillColorOverT;

		// Token: 0x04001656 RID: 5718
		[SerializeField]
		private GameObject durabilityZeroIndicator;

		// Token: 0x04001657 RID: 5719
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x04001658 RID: 5720
		[SerializeField]
		private GameObject slotIndicatorContainer;

		// Token: 0x04001659 RID: 5721
		[SerializeField]
		private SlotIndicator slotIndicatorTemplate;

		// Token: 0x0400165A RID: 5722
		[SerializeField]
		private GameObject wishlistedIndicator;

		// Token: 0x0400165B RID: 5723
		[SerializeField]
		private GameObject questRequiredIndicator;

		// Token: 0x0400165C RID: 5724
		[SerializeField]
		private GameObject buildingRequiredIndicator;

		// Token: 0x0400165D RID: 5725
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x0400165E RID: 5726
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x0400165F RID: 5727
		[SerializeField]
		[Range(-1f, 1f)]
		private float backgroundRingPunchScale = 0.2f;

		// Token: 0x04001660 RID: 5728
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x04001665 RID: 5733
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x04001666 RID: 5734
		private PrefabPool<SlotIndicator> _slotIndicatorPool;

		// Token: 0x0400166A RID: 5738
		private bool mainContentShown = true;

		// Token: 0x0400166B RID: 5739
		private bool isBeingDestroyed;

		// Token: 0x0400166C RID: 5740
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x04001672 RID: 5746
		private float lastClickTime;

		// Token: 0x04001673 RID: 5747
		private bool doubleClickInvoked;
	}
}
