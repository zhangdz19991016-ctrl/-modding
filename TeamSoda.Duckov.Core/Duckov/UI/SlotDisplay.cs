using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A4 RID: 932
	public class SlotDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x0600213A RID: 8506 RVA: 0x00074500 File Offset: 0x00072700
		// (remove) Token: 0x0600213B RID: 8507 RVA: 0x00074538 File Offset: 0x00072738
		internal event Action<SlotDisplay> onSlotDisplayClicked;

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x0600213C RID: 8508 RVA: 0x00074570 File Offset: 0x00072770
		// (remove) Token: 0x0600213D RID: 8509 RVA: 0x000745A8 File Offset: 0x000727A8
		internal event Action<SlotDisplay> onSlotDisplayDoubleClicked;

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600213E RID: 8510 RVA: 0x000745DD File Offset: 0x000727DD
		// (set) Token: 0x0600213F RID: 8511 RVA: 0x000745E5 File Offset: 0x000727E5
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002140 RID: 8512 RVA: 0x000745EE File Offset: 0x000727EE
		// (set) Token: 0x06002141 RID: 8513 RVA: 0x000745F6 File Offset: 0x000727F6
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			internal set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x000745FF File Offset: 0x000727FF
		// (set) Token: 0x06002143 RID: 8515 RVA: 0x00074607 File Offset: 0x00072807
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
			internal set
			{
				this.showOperationMenu = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x00074610 File Offset: 0x00072810
		// (set) Token: 0x06002145 RID: 8517 RVA: 0x00074618 File Offset: 0x00072818
		public Slot Target { get; private set; }

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06002146 RID: 8518 RVA: 0x00074624 File Offset: 0x00072824
		// (remove) Token: 0x06002147 RID: 8519 RVA: 0x00074658 File Offset: 0x00072858
		public static event Action<SlotDisplayOperationContext> onOperation;

		// Token: 0x06002148 RID: 8520 RVA: 0x0007468C File Offset: 0x0007288C
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged += this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked += this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem += this.OnStartDragItem;
			IItemDragSource.OnEndDragItem += this.OnEndDragItem;
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnFastDrop;
			UIInputManager.OnUseItem += this.OnFastUse;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0007475C File Offset: 0x0007295C
		private void UnregisterEvents()
		{
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged -= this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked -= this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
			IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnFastDrop;
			UIInputManager.OnUseItem -= this.OnFastUse;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0007481C File Offset: 0x00072A1C
		private void OnFastDrop(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.CanDrop)
			{
				return;
			}
			if (this.Editable)
			{
				this.Target.Content.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0007488C File Offset: 0x00072A8C
		private void OnFastUse(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			CharacterMainControl.Main.UseItem(this.Target.Content);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000748F5 File Offset: 0x00072AF5
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.OnItemDisplayDoubleClicked(this.itemDisplay, new PointerEventData(EventSystem.current));
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0007491F File Offset: 0x00072B1F
		private void OnEndDragItem(Item item)
		{
			this.pluggableIndicator.Hide();
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0007492C File Offset: 0x00072B2C
		private void OnStartDragItem(Item item)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (item != this.Target.Content && this.Target.CanPlug(item))
			{
				this.pluggableIndicator.Show();
				return;
			}
			this.pluggableIndicator.Hide();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00074983 File Offset: 0x00072B83
		private void OnItemDisplayDoubleClicked(ItemDisplay arg1, PointerEventData arg2)
		{
			Action<SlotDisplay> action = this.onSlotDisplayDoubleClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.ContentSelectable)
			{
				arg2.Use();
			}
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000749A8 File Offset: 0x00072BA8
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (data.button == PointerEventData.InputButton.Left)
			{
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					if (this.Editable && this.Target.Content != null)
					{
						Item content = this.Target.Content;
						content.Detach();
						if (!ItemUtilities.SendToPlayerCharacterInventory(content, false))
						{
							if (PlayerStorage.IsAccessableAndNotFull())
							{
								ItemUtilities.SendToPlayerStorage(content, false);
							}
							else
							{
								ItemUtilities.SendToPlayer(content, false, false);
							}
						}
						data.Use();
						return;
					}
				}
				else if (!this.ContentSelectable)
				{
					data.Use();
					return;
				}
			}
			else if (data.button == PointerEventData.InputButton.Right && this.Editable)
			{
				Slot target = this.Target;
				if (((target != null) ? target.Content : null) != null)
				{
					ItemOperationMenu.Show(this.itemDisplay);
				}
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00074A84 File Offset: 0x00072C84
		private void OnTargetContentChanged(Slot slot)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00074A92 File Offset: 0x00072C92
		private void OnItemSelectionChanged()
		{
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00074A94 File Offset: 0x00072C94
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.label.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
			this.pluggableIndicator.Hide();
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00074ACC File Offset: 0x00072CCC
		private void Refresh()
		{
			if (this.Target.Content == null)
			{
				this.slotIcon.gameObject.SetActive(true);
				if (this.Target.SlotIcon != null)
				{
					this.slotIcon.sprite = this.Target.SlotIcon;
				}
				else
				{
					this.slotIcon.sprite = this.defaultSlotIcon;
				}
			}
			else
			{
				this.slotIcon.gameObject.SetActive(false);
			}
			this.itemDisplay.ShowOperationButtons = this.showOperationMenu;
			this.itemDisplay.Setup(this.Target.Content);
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x00074B73 File Offset: 0x00072D73
		public static PrefabPool<SlotDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.SlotDisplayPool;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x00074B7F File Offset: 0x00072D7F
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x00074B8C File Offset: 0x00072D8C
		public bool Movable
		{
			get
			{
				return this.itemDisplay.Movable;
			}
			set
			{
				this.itemDisplay.Movable = value;
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00074B9A File Offset: 0x00072D9A
		public static SlotDisplay Get()
		{
			return SlotDisplay.Pool.Get(null);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00074BA7 File Offset: 0x00072DA7
		public static void Release(SlotDisplay item)
		{
			SlotDisplay.Pool.Release(item);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00074BB4 File Offset: 0x00072DB4
		public void NotifyPooled()
		{
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00074BB6 File Offset: 0x00072DB6
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00074BC5 File Offset: 0x00072DC5
		private void Awake()
		{
			this.itemDisplay.onReceiveDrop += this.OnDrop;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00074BDF File Offset: 0x00072DDF
		private void OnEnable()
		{
			this.RegisterEvents();
			this.iconInitialColor = this.slotIcon.color;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x00074C09 File Offset: 0x00072E09
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00074C14 File Offset: 0x00072E14
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.Editable)
			{
				this.Punch();
				eventData.Use();
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					this.Punch();
					return;
				}
				if (this.Target.Content != null)
				{
					Debug.Log("槽位 " + this.Target.DisplayName + " 中已经有物品。操作已取消。");
					this.DenialPunch();
					return;
				}
				if (!this.Target.CanPlug(selectedItem))
				{
					Debug.Log(string.Concat(new string[]
					{
						"物品 ",
						selectedItem.DisplayName,
						" 未通过槽位 ",
						this.Target.DisplayName,
						" 安装检测。操作已取消。"
					}));
					this.DenialPunch();
					return;
				}
				eventData.Use();
				selectedItem.Detach();
				Item item;
				this.Target.Plug(selectedItem, out item);
				ItemUIUtilities.NotifyPutItem(selectedItem, false);
				if (item != null)
				{
					ItemUIUtilities.RaiseOrphan(item);
				}
				this.Punch();
			}
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00074D30 File Offset: 0x00072F30
		public void Punch()
		{
			if (this.slotIcon != null)
			{
				this.slotIcon.transform.DOKill(false);
				this.slotIcon.color = this.iconInitialColor;
				this.slotIcon.transform.localScale = Vector3.one;
				this.slotIcon.transform.DOPunchScale(Vector3.one * this.slotIconPunchScale, this.punchDuration, 10, 1f);
			}
			if (this.itemDisplay != null)
			{
				this.itemDisplay.Punch();
			}
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00074DCC File Offset: 0x00072FCC
		public void DenialPunch()
		{
			if (this.slotIcon == null)
			{
				return;
			}
			this.slotIcon.transform.DOKill(false);
			this.slotIcon.color = this.iconInitialColor;
			this.slotIcon.DOColor(this.slotIconDenialColor, this.denialPunchDuration).From<TweenerCore<Color, Color, ColorOptions>>();
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Deny, false));
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00074E3F File Offset: 0x0007303F
		public bool IsEditable()
		{
			return this.Editable;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x00074E47 File Offset: 0x00073047
		public Item GetItem()
		{
			Slot target = this.Target;
			if (target == null)
			{
				return null;
			}
			return target.Content;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00074E5C File Offset: 0x0007305C
		public void OnDrop(PointerEventData eventData)
		{
			if (!this.Editable)
			{
				return;
			}
			if (eventData.used)
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
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
			if (item == null)
			{
				return;
			}
			if (this.SetAmmo(item))
			{
				return;
			}
			if (!this.Target.CanPlug(item))
			{
				Debug.Log(string.Concat(new string[]
				{
					"物品 ",
					item.DisplayName,
					" 未通过槽位 ",
					this.Target.DisplayName,
					" 安装检测。操作已取消。"
				}));
				this.DenialPunch();
				return;
			}
			Inventory inInventory = item.InInventory;
			Slot pluggedIntoSlot = item.PluggedIntoSlot;
			if (pluggedIntoSlot == this.Target)
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			Item item2;
			bool succeed = this.Target.Plug(item, out item2);
			if (item2 != null && (!(inInventory != null) || !inInventory.AddAndMerge(item2, 0)))
			{
				Item item3;
				if (pluggedIntoSlot != null && pluggedIntoSlot.CanPlug(item2) && pluggedIntoSlot.Plug(item2, out item3))
				{
					if (item3)
					{
						Debug.LogError("Source slot spit out an unplugged item! " + item3.DisplayName);
					}
				}
				else if (!ItemUtilities.SendToPlayerCharacter(item2, false))
				{
					LootView lootView = View.ActiveView as LootView;
					if (lootView == null || !(lootView.TargetInventory != null) || !lootView.TargetInventory.AddAndMerge(item2, 0))
					{
						if (PlayerStorage.IsAccessableAndNotFull())
						{
							ItemUtilities.SendToPlayerStorage(item2, false);
						}
						else
						{
							item2.Drop(CharacterMainControl.Main, true);
						}
					}
				}
			}
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Equip, succeed));
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00075011 File Offset: 0x00073211
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x00075013 File Offset: 0x00073213
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(this.Editable);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00075032 File Offset: 0x00073232
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0007504C File Offset: 0x0007324C
		private bool SetAmmo(Item incomming)
		{
			Slot target = this.Target;
			ItemSetting_Gun itemSetting_Gun;
			if (target == null)
			{
				itemSetting_Gun = null;
			}
			else
			{
				Item content = target.Content;
				itemSetting_Gun = ((content != null) ? content.GetComponent<ItemSetting_Gun>() : null);
			}
			ItemSetting_Gun itemSetting_Gun2 = itemSetting_Gun;
			if (itemSetting_Gun2 == null)
			{
				return false;
			}
			if (!itemSetting_Gun2.IsValidBullet(incomming))
			{
				return false;
			}
			if (View.ActiveView is InventoryView || View.ActiveView is LootView)
			{
				View.ActiveView.Close();
			}
			return itemSetting_Gun2.LoadSpecificBullet(incomming);
		}

		// Token: 0x04001695 RID: 5781
		[SerializeField]
		private Sprite defaultSlotIcon;

		// Token: 0x04001696 RID: 5782
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001697 RID: 5783
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001698 RID: 5784
		[SerializeField]
		private Image slotIcon;

		// Token: 0x04001699 RID: 5785
		[SerializeField]
		private FadeGroup pluggableIndicator;

		// Token: 0x0400169A RID: 5786
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x0400169B RID: 5787
		[SerializeField]
		private bool editable = true;

		// Token: 0x0400169C RID: 5788
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x0400169D RID: 5789
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x040016A0 RID: 5792
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.1f;

		// Token: 0x040016A1 RID: 5793
		[SerializeField]
		[Range(-1f, 1f)]
		private float slotIconPunchScale = -0.1f;

		// Token: 0x040016A2 RID: 5794
		[SerializeField]
		[Range(0f, 1f)]
		private float denialPunchDuration = 0.2f;

		// Token: 0x040016A3 RID: 5795
		[SerializeField]
		private Color slotIconDenialColor = Color.red;

		// Token: 0x040016A4 RID: 5796
		private Color iconInitialColor;

		// Token: 0x040016A7 RID: 5799
		private bool hovering;
	}
}
