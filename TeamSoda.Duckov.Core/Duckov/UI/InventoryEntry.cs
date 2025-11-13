using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x02000397 RID: 919
	public class InventoryEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IDropHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x0007103E File Offset: 0x0006F23E
		// (set) Token: 0x06002042 RID: 8258 RVA: 0x00071046 File Offset: 0x0006F246
		public InventoryDisplay Master { get; private set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x0007104F File Offset: 0x0006F24F
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x00071057 File Offset: 0x0006F257
		// (set) Token: 0x06002045 RID: 8261 RVA: 0x0007105F File Offset: 0x0006F25F
		public bool Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
				this.Refresh();
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x00071070 File Offset: 0x0006F270
		public Item Content
		{
			get
			{
				InventoryDisplay master = this.Master;
				Inventory inventory = (master != null) ? master.Target : null;
				if (inventory == null)
				{
					return null;
				}
				if (this.index >= inventory.Capacity)
				{
					return null;
				}
				InventoryDisplay master2 = this.Master;
				if (master2 == null)
				{
					return null;
				}
				Inventory target = master2.Target;
				if (target == null)
				{
					return null;
				}
				return target.GetItemAt(this.index);
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x000710D0 File Offset: 0x0006F2D0
		public bool ShouldHighlight
		{
			get
			{
				return !(this.Master == null) && !(this.Content == null) && (this.Master.EvaluateShouldHighlight(this.Content) || (this.Editable && ItemUIUtilities.IsGunSelected && !this.cacheContentIsGun && this.IsCaliberMatchItemSelected()));
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00071131 File Offset: 0x0006F331
		private bool IsCaliberMatchItemSelected()
		{
			return !(this.Content == null) && ItemUIUtilities.SelectedItemCaliber == this.cachedMeta.caliber;
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x00071158 File Offset: 0x0006F358
		public bool CanOperate
		{
			get
			{
				return !(this.Master == null) && this.Master.Func_CanOperate(this.Content);
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x00071180 File Offset: 0x0006F380
		public bool Editable
		{
			get
			{
				return !(this.Master == null) && this.Master.Editable && this.CanOperate;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x000711A7 File Offset: 0x0006F3A7
		public bool Movable
		{
			get
			{
				return !(this.Master == null) && this.Master.Movable;
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x0600204C RID: 8268 RVA: 0x000711C4 File Offset: 0x0006F3C4
		// (remove) Token: 0x0600204D RID: 8269 RVA: 0x000711FC File Offset: 0x0006F3FC
		public event Action<InventoryEntry> onRefresh;

		// Token: 0x0600204E RID: 8270 RVA: 0x00071234 File Offset: 0x0006F434
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClicked;
			this.itemDisplay.onDoubleClicked += this.OnDisplayDoubleClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnDropItemButton;
			UIInputManager.OnUseItem += this.OnUseItemButton;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000712CC File Offset: 0x0006F4CC
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex += this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput += this.OnShortcutInput;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00071304 File Offset: 0x0006F504
		private void OnDisable()
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex -= this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput -= this.OnShortcutInput;
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0007135D File Offset: 0x0006F55D
		private void OnShortcutInput(UIInputEventData data, int shortcutIndex)
		{
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			ItemShortcut.Set(shortcutIndex, this.Item);
			ItemUIUtilities.NotifyPutItem(this.Item, false);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00071390 File Offset: 0x0006F590
		private void OnInputLockInventoryIndex(UIInputEventData data)
		{
			if (!this.hovering)
			{
				return;
			}
			this.ToggleLock();
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000713A1 File Offset: 0x0006F5A1
		private void OnSelectionChanged()
		{
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			if (ItemUIUtilities.SelectedItemDisplay == this.itemDisplay)
			{
				this.Refresh();
			}
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000713CC File Offset: 0x0006F5CC
		private void OnDestroy()
		{
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnDropItemButton;
			UIInputManager.OnUseItem -= this.OnUseItemButton;
			if (this.itemDisplay != null)
			{
				this.itemDisplay.onPointerClick -= this.OnItemDisplayPointerClicked;
				this.itemDisplay.onDoubleClicked -= this.OnDisplayDoubleClicked;
				this.itemDisplay.onReceiveDrop -= this.OnDrop;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00071460 File Offset: 0x0006F660
		private void OnFastPick(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.Master.NotifyItemDoubleClicked(this, new PointerEventData(EventSystem.current));
			data.Use();
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0007149C File Offset: 0x0006F69C
		private void OnDropItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.CanDrop)
			{
				return;
			}
			if (this.CanOperate)
			{
				this.Item.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000714F4 File Offset: 0x0006F6F4
		private void OnUseItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			if (this.CanOperate)
			{
				CharacterMainControl.Main.UseItem(this.Item);
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00071550 File Offset: 0x0006F750
		private void OnItemDisplayPointerClicked(ItemDisplay display, PointerEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.disabled || !this.CanOperate)
			{
				data.Use();
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (data.button == PointerEventData.InputButton.Left)
			{
				if (this.Content == null)
				{
					return;
				}
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					data.Use();
					if (ItemUIUtilities.SelectedItem != null)
					{
						ItemUIUtilities.SelectedItem.TryPlug(this.Content, false, null, 0);
					}
					CharacterMainControl.Main.CharacterItem.TryPlug(this.Content, false, null, 0);
					return;
				}
				if (ItemUIUtilities.SelectedItem == null)
				{
					return;
				}
				if (this.Content.Stackable && ItemUIUtilities.SelectedItem != this.Content && ItemUIUtilities.SelectedItem.TypeID == this.Content.TypeID)
				{
					ItemUIUtilities.SelectedItem.CombineInto(this.Content);
					return;
				}
			}
			else if (data.button == PointerEventData.InputButton.Right && this.Editable && this.Content != null)
			{
				ItemOperationMenu.Show(this.itemDisplay);
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00071678 File Offset: 0x0006F878
		private void OnDisplayDoubleClicked(ItemDisplay display, PointerEventData data)
		{
			this.Master.NotifyItemDoubleClicked(this, data);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00071687 File Offset: 0x0006F887
		public void Setup(InventoryDisplay master, int index, bool disabled = false)
		{
			this.Master = master;
			this.index = index;
			this.disabled = disabled;
			this.Refresh();
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000716A4 File Offset: 0x0006F8A4
		internal void Refresh()
		{
			Item content = this.Content;
			if (content != null)
			{
				this.cachedMeta = ItemAssetsCollection.GetMetaData(content.TypeID);
				this.cacheContentIsGun = content.Tags.Contains("Gun");
			}
			else
			{
				this.cacheContentIsGun = false;
				this.cachedMeta = default(ItemMetaData);
			}
			this.itemDisplay.Setup(content);
			this.itemDisplay.CanDrop = this.CanOperate;
			this.itemDisplay.Movable = this.Movable;
			this.itemDisplay.Editable = (this.Editable && this.CanOperate);
			this.itemDisplay.CanLockSort = true;
			if (!this.Master.Target.NeedInspection && content != null)
			{
				content.Inspected = true;
			}
			this.itemDisplay.ShowOperationButtons = this.Master.ShowOperationButtons;
			this.shortcutIndicator.gameObject.SetActive(this.Master.IsShortcut(this.index));
			this.disabledIndicator.SetActive(this.disabled || !this.CanOperate);
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			bool active = this.Master.Target.IsIndexLocked(this.Index);
			this.lockIndicator.SetActive(active);
			Action<InventoryEntry> action = this.onRefresh;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x00071810 File Offset: 0x0006FA10
		public static PrefabPool<InventoryEntry> Pool
		{
			get
			{
				return GameplayUIManager.Instance.InventoryEntryPool;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600205D RID: 8285 RVA: 0x0007181C File Offset: 0x0006FA1C
		public Item Item
		{
			get
			{
				if (this.itemDisplay != null && this.itemDisplay.isActiveAndEnabled)
				{
					return this.itemDisplay.Target;
				}
				return null;
			}
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00071846 File Offset: 0x0006FA46
		public static InventoryEntry Get()
		{
			return InventoryEntry.Pool.Get(null);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00071853 File Offset: 0x0006FA53
		public static void Release(InventoryEntry item)
		{
			InventoryEntry.Pool.Release(item);
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00071860 File Offset: 0x0006FA60
		public void NotifyPooled()
		{
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00071862 File Offset: 0x0006FA62
		public void NotifyReleased()
		{
			this.Master = null;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0007186C File Offset: 0x0006FA6C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.lastClickTime = eventData.clickTime;
				if (this.Editable)
				{
					Item selectedItem = ItemUIUtilities.SelectedItem;
					if (!(selectedItem == null))
					{
						if (this.Content != null)
						{
							Debug.Log(string.Format("{0}(Inventory) 的 {1} 已经有物品。操作已取消。", this.Master.Target.name, this.index));
						}
						else
						{
							eventData.Use();
							selectedItem.Detach();
							this.Master.Target.AddAt(selectedItem, this.index);
							ItemUIUtilities.NotifyPutItem(selectedItem, false);
						}
					}
				}
				this.lastClickTime = eventData.clickTime;
			}
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0007191E File Offset: 0x0006FB1E
		internal void Punch()
		{
			this.itemDisplay.Punch();
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0007192B File Offset: 0x0006FB2B
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00071930 File Offset: 0x0006FB30
		public void OnDrop(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (!this.Editable)
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
			if (item.Sticky && !this.Master.Target.AcceptSticky)
			{
				return;
			}
			if (Keyboard.current != null && Keyboard.current.ctrlKey.isPressed)
			{
				if (this.Content != null)
				{
					NotificationText.Push("UI_Inventory_TargetOccupiedCannotSplit".ToPlainText());
					return;
				}
				Debug.Log("SPLIT");
				SplitDialogue.SetupAndShow(item, this.Master.Target, this.index);
				return;
			}
			else
			{
				ItemUIUtilities.NotifyPutItem(item, false);
				if (this.Content == null)
				{
					item.Detach();
					this.Master.Target.AddAt(item, this.index);
					return;
				}
				if (this.Content.TypeID == item.TypeID && this.Content.Stackable)
				{
					this.Content.Combine(item);
					return;
				}
				Inventory inInventory = item.InInventory;
				Inventory target = this.Master.Target;
				if (inInventory != null)
				{
					int atPosition = inInventory.GetIndex(item);
					int atPosition2 = this.index;
					Item content = this.Content;
					if (content != item)
					{
						item.Detach();
						content.Detach();
						inInventory.AddAt(content, atPosition);
						target.AddAt(item, atPosition2);
					}
				}
				return;
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00071ABF File Offset: 0x0006FCBF
		public bool IsEditable()
		{
			return !(this.Content == null) && !this.Content.NeedInspection && this.Editable;
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00071AE6 File Offset: 0x0006FCE6
		public Item GetItem()
		{
			return this.Content;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00071AEE File Offset: 0x0006FCEE
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

		// Token: 0x06002069 RID: 8297 RVA: 0x00071B0D File Offset: 0x0006FD0D
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

		// Token: 0x0600206A RID: 8298 RVA: 0x00071B27 File Offset: 0x0006FD27
		public void ToggleLock()
		{
			this.Master.Target.ToggleLockIndex(this.Index);
		}

		// Token: 0x040015FD RID: 5629
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040015FE RID: 5630
		[SerializeField]
		private GameObject shortcutIndicator;

		// Token: 0x040015FF RID: 5631
		[SerializeField]
		private GameObject disabledIndicator;

		// Token: 0x04001600 RID: 5632
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x04001601 RID: 5633
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x04001602 RID: 5634
		[SerializeField]
		private GameObject lockIndicator;

		// Token: 0x04001604 RID: 5636
		[SerializeField]
		private int index;

		// Token: 0x04001605 RID: 5637
		[SerializeField]
		private bool disabled;

		// Token: 0x04001607 RID: 5639
		private bool cacheContentIsGun;

		// Token: 0x04001608 RID: 5640
		private ItemMetaData cachedMeta;

		// Token: 0x04001609 RID: 5641
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x0400160A RID: 5642
		private float lastClickTime;

		// Token: 0x0400160B RID: 5643
		private bool hovering;
	}
}
