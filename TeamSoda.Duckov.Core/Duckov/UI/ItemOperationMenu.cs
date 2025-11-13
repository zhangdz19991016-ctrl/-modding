using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A2 RID: 930
	public class ItemOperationMenu : ManagedUIElement
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002107 RID: 8455 RVA: 0x000738D1 File Offset: 0x00071AD1
		// (set) Token: 0x06002108 RID: 8456 RVA: 0x000738D8 File Offset: 0x00071AD8
		public static ItemOperationMenu Instance { get; private set; }

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x000738E0 File Offset: 0x00071AE0
		private Item TargetItem
		{
			get
			{
				ItemDisplay targetDisplay = this.TargetDisplay;
				if (targetDisplay == null)
				{
					return null;
				}
				return targetDisplay.Target;
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000738F3 File Offset: 0x00071AF3
		protected override void Awake()
		{
			base.Awake();
			ItemOperationMenu.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.Initialize();
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00073921 File Offset: 0x00071B21
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0007392C File Offset: 0x00071B2C
		private void Update()
		{
			if (this.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			if (!this.fadeGroup.IsShown)
			{
				return;
			}
			if (!Mouse.current.leftButton.wasReleasedThisFrame && !(this.targetView == null) && this.targetView.open)
			{
				if (this.fadeGroup.IsShowingInProgress)
				{
					return;
				}
				if (!Mouse.current.rightButton.wasReleasedThisFrame)
				{
					return;
				}
			}
			base.Close();
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000739A8 File Offset: 0x00071BA8
		private void Initialize()
		{
			this.btn_Use.onClick.AddListener(new UnityAction(this.Use));
			this.btn_Split.onClick.AddListener(new UnityAction(this.Split));
			this.btn_Dump.onClick.AddListener(new UnityAction(this.Dump));
			this.btn_Equip.onClick.AddListener(new UnityAction(this.Equip));
			this.btn_Modify.onClick.AddListener(new UnityAction(this.Modify));
			this.btn_Unload.onClick.AddListener(new UnityAction(this.Unload));
			this.btn_Wishlist.onClick.AddListener(new UnityAction(this.Wishlist));
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00073A7C File Offset: 0x00071C7C
		private void Wishlist()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			int typeID = this.TargetItem.TypeID;
			if (ItemWishlist.GetWishlistInfo(typeID).isManuallyWishlisted)
			{
				ItemWishlist.RemoveFromWishlist(typeID);
				return;
			}
			ItemWishlist.AddToWishList(this.TargetItem.TypeID);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00073AC9 File Offset: 0x00071CC9
		private void Use()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					mainCharacter.UseItem(this.TargetItem);
				}
			}
			InventoryView.Hide();
			base.Close();
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00073AF7 File Offset: 0x00071CF7
		private void Split()
		{
			SplitDialogue.SetupAndShow(this.TargetItem);
			base.Close();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00073B0A File Offset: 0x00071D0A
		private void Dump()
		{
			LevelManager instance = LevelManager.Instance;
			if ((instance != null) ? instance.MainCharacter : null)
			{
				this.TargetItem.Drop(LevelManager.Instance.MainCharacter, true);
			}
			base.Close();
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00073B40 File Offset: 0x00071D40
		private void Modify()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			List<Inventory> list = new List<Inventory>();
			LevelManager instance2 = LevelManager.Instance;
			Inventory inventory;
			if (instance2 == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
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
			if (inventory2)
			{
				list.Add(inventory2);
			}
			instance.Setup(this.TargetItem, list);
			instance.Open(null);
			base.Close();
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00073BC5 File Offset: 0x00071DC5
		private void Equip()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					Item characterItem = mainCharacter.CharacterItem;
					if (characterItem != null)
					{
						characterItem.TryPlug(this.TargetItem, false, null, 0);
					}
				}
			}
			base.Close();
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00073C00 File Offset: 0x00071E00
		private void Unload()
		{
			Item targetItem = this.TargetItem;
			ItemSetting_Gun itemSetting_Gun = (targetItem != null) ? targetItem.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				return;
			}
			AudioManager.Post("SFX/Combat/Gun/unload");
			itemSetting_Gun.TakeOutAllBullets();
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00073C3B File Offset: 0x00071E3B
		protected override void OnOpen()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00073C48 File Offset: 0x00071E48
		protected override void OnClose()
		{
			this.fadeGroup.Hide();
			this.displayingItem = null;
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00073C5C File Offset: 0x00071E5C
		public static void Show(ItemDisplay id)
		{
			if (ItemOperationMenu.Instance == null)
			{
				return;
			}
			ItemOperationMenu.Instance.MShow(id);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00073C77 File Offset: 0x00071E77
		private void MShow(ItemDisplay targetDisplay)
		{
			if (targetDisplay == null)
			{
				return;
			}
			this.TargetDisplay = targetDisplay;
			this.targetView = targetDisplay.GetComponentInParent<View>();
			this.Setup();
			base.Open(null);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00073CA4 File Offset: 0x00071EA4
		private void Setup()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			this.displayingItem = this.TargetItem;
			this.icon.sprite = this.TargetItem.Icon;
			this.nameText.text = this.TargetItem.DisplayName;
			this.btn_Use.gameObject.SetActive(this.Usable);
			this.btn_Use.interactable = this.UseButtonInteractable;
			this.btn_Split.gameObject.SetActive(this.Splittable);
			this.btn_Dump.gameObject.SetActive(this.Dumpable);
			this.btn_Equip.gameObject.SetActive(this.Equipable);
			this.btn_Modify.gameObject.SetActive(this.Modifyable);
			this.btn_Unload.gameObject.SetActive(this.Unloadable);
			this.RefreshWeightText();
			this.RefreshPosition();
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00073D9C File Offset: 0x00071F9C
		private void RefreshPosition()
		{
			RectTransform rectTransform = this.TargetDisplay.transform as RectTransform;
			Rect rect = rectTransform.rect;
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			Vector3 point = rectTransform.localToWorldMatrix.MultiplyPoint(min);
			Vector3 point2 = rectTransform.localToWorldMatrix.MultiplyPoint(max);
			Vector3 vector = this.rectTransform.worldToLocalMatrix.MultiplyPoint(point);
			Vector3 vector2 = this.rectTransform.worldToLocalMatrix.MultiplyPoint(point2);
			Vector2[] array = new Vector2[]
			{
				new Vector2(vector.x, vector.y),
				new Vector2(vector.x, vector2.y),
				new Vector2(vector2.x, vector.y),
				new Vector2(vector2.x, vector2.y)
			};
			int num = 0;
			float num2 = float.MaxValue;
			Vector2 center = this.rectTransform.rect.center;
			for (int i = 0; i < array.Length; i++)
			{
				float sqrMagnitude = (array[i] - center).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num = i;
					num2 = sqrMagnitude;
				}
			}
			bool flag = (num & 2) > 0;
			bool flag2 = (num & 1) > 0;
			float x = flag ? vector2.x : vector.x;
			float y = flag2 ? vector.y : vector2.y;
			this.contentRectTransform.pivot = new Vector2((float)(flag ? 0 : 1), (float)(flag2 ? 0 : 1));
			this.contentRectTransform.localPosition = new Vector2(x, y);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00073F70 File Offset: 0x00072170
		private void RefreshWeightText()
		{
			if (this.displayingItem == null)
			{
				return;
			}
			this.weightText.text = string.Format(this.weightTextFormat, this.displayingItem.TotalWeight);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x00073FA7 File Offset: 0x000721A7
		public void OnPointerClick(PointerEventData eventData)
		{
			base.Close();
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x00073FAF File Offset: 0x000721AF
		private bool Usable
		{
			get
			{
				return this.TargetItem.UsageUtilities != null;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x00073FC2 File Offset: 0x000721C2
		private bool UseButtonInteractable
		{
			get
			{
				if (this.TargetItem)
				{
					Item targetItem = this.TargetItem;
					LevelManager instance = LevelManager.Instance;
					return targetItem.IsUsable((instance != null) ? instance.MainCharacter : null);
				}
				return false;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x00073FF0 File Offset: 0x000721F0
		private bool Splittable
		{
			get
			{
				CharacterMainControl main = CharacterMainControl.Main;
				return (main == null || main.CharacterItem.Inventory.GetFirstEmptyPosition(0) >= 0) && (this.TargetItem && this.TargetItem.Stackable) && this.TargetItem.StackCount > 1;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x0007404C File Offset: 0x0007224C
		private bool Dumpable
		{
			get
			{
				if (!this.TargetItem.CanDrop)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				Item item;
				if (instance == null)
				{
					item = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
				}
				Item y = item;
				return this.TargetItem.GetRoot() == y;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x0007409C File Offset: 0x0007229C
		private bool Equipable
		{
			get
			{
				if (this.TargetItem == null)
				{
					return false;
				}
				if (this.TargetItem.PluggedIntoSlot != null)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				bool? flag;
				if (instance == null)
				{
					flag = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag = ((characterItem != null) ? new bool?(characterItem.Slots.Any((Slot e) => e.CanPlug(this.TargetItem))) : null);
					}
				}
				bool? flag2 = flag;
				return flag2 != null && flag2.Value;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x00074132 File Offset: 0x00072332
		private bool Modifyable
		{
			get
			{
				return this.alwaysModifyable;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x0007413F File Offset: 0x0007233F
		private bool Unloadable
		{
			get
			{
				return !(this.TargetItem == null) && this.TargetItem.GetComponent<ItemSetting_Gun>();
			}
		}

		// Token: 0x04001675 RID: 5749
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001676 RID: 5750
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001677 RID: 5751
		[SerializeField]
		private RectTransform contentRectTransform;

		// Token: 0x04001678 RID: 5752
		[SerializeField]
		private Image icon;

		// Token: 0x04001679 RID: 5753
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400167A RID: 5754
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x0400167B RID: 5755
		[SerializeField]
		private string weightTextFormat = "{0:0.#}kg";

		// Token: 0x0400167C RID: 5756
		[SerializeField]
		private Button btn_Use;

		// Token: 0x0400167D RID: 5757
		[SerializeField]
		private Button btn_Split;

		// Token: 0x0400167E RID: 5758
		[SerializeField]
		private Button btn_Dump;

		// Token: 0x0400167F RID: 5759
		[SerializeField]
		private Button btn_Equip;

		// Token: 0x04001680 RID: 5760
		[SerializeField]
		private Button btn_Modify;

		// Token: 0x04001681 RID: 5761
		[SerializeField]
		private Button btn_Unload;

		// Token: 0x04001682 RID: 5762
		[SerializeField]
		private Button btn_Wishlist;

		// Token: 0x04001683 RID: 5763
		[SerializeField]
		private bool alwaysModifyable;

		// Token: 0x04001684 RID: 5764
		private View targetView;

		// Token: 0x04001685 RID: 5765
		private ItemDisplay TargetDisplay;

		// Token: 0x04001686 RID: 5766
		private Item displayingItem;
	}
}
