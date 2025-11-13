using System;
using DG.Tweening;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B1 RID: 945
	public class ItemShortcutButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x00076E90 File Offset: 0x00075090
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x00076E98 File Offset: 0x00075098
		public int Index { get; private set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x00076EA1 File Offset: 0x000750A1
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x00076EA9 File Offset: 0x000750A9
		public ItemShortcutPanel Master { get; private set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x00076EB2 File Offset: 0x000750B2
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x00076EBA File Offset: 0x000750BA
		public Inventory Inventory { get; private set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x00076EC3 File Offset: 0x000750C3
		// (set) Token: 0x060021F9 RID: 8697 RVA: 0x00076ECB File Offset: 0x000750CB
		public CharacterMainControl Character { get; private set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x00076ED4 File Offset: 0x000750D4
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x00076EDC File Offset: 0x000750DC
		public Item TargetItem { get; private set; }

		// Token: 0x060021FC RID: 8700 RVA: 0x00076EE5 File Offset: 0x000750E5
		private Item GetTargetItem()
		{
			return ItemShortcut.Get(this.Index);
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x00076EF4 File Offset: 0x000750F4
		private bool Interactable
		{
			get
			{
				Item targetItem = this.TargetItem;
				return ((targetItem != null) ? targetItem.UsageUtilities : null) || (this.TargetItem && this.TargetItem.HasHandHeldAgent) || (this.TargetItem && this.TargetItem.GetBool("IsSkill", false));
			}
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00076F5C File Offset: 0x0007515C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.Interactable)
			{
				this.denialIndicator.color = this.denialColor;
				this.denialIndicator.DOColor(Color.clear, 0.1f);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.UsageUtilities && this.TargetItem.UsageUtilities.IsUsable(this.TargetItem, this.Character))
			{
				this.Character.UseItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.GetBool("IsSkill", false))
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.HasHandHeldAgent)
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			this.AnimateDenial();
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00077075 File Offset: 0x00075275
		public void AnimateDenial()
		{
			this.denialIndicator.DOKill(false);
			this.denialIndicator.color = this.denialColor;
			this.denialIndicator.DOColor(Color.clear, 0.1f);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000770AB File Offset: 0x000752AB
		private void Awake()
		{
			ItemShortcutButton.OnRequireAnimateDenial += this.OnStaticAnimateDenial;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000770BE File Offset: 0x000752BE
		private void OnDestroy()
		{
			ItemShortcutButton.OnRequireAnimateDenial -= this.OnStaticAnimateDenial;
			this.isBeingDestroyed = true;
			this.UnregisterEvents();
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000770DE File Offset: 0x000752DE
		private void OnStaticAnimateDenial(int index)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (index == this.Index)
			{
				this.AnimateDenial();
			}
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06002203 RID: 8707 RVA: 0x000770F8 File Offset: 0x000752F8
		// (remove) Token: 0x06002204 RID: 8708 RVA: 0x0007712C File Offset: 0x0007532C
		private static event Action<int> OnRequireAnimateDenial;

		// Token: 0x06002205 RID: 8709 RVA: 0x0007715F File Offset: 0x0007535F
		public static void AnimateDenial(int index)
		{
			Action<int> onRequireAnimateDenial = ItemShortcutButton.OnRequireAnimateDenial;
			if (onRequireAnimateDenial == null)
			{
				return;
			}
			onRequireAnimateDenial(index);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00077174 File Offset: 0x00075374
		internal void Initialize(ItemShortcutPanel itemShortcutPanel, int index)
		{
			this.UnregisterEvents();
			this.Master = itemShortcutPanel;
			this.Inventory = this.Master.Target;
			this.Index = index;
			this.Character = this.Master.Character;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000771C4 File Offset: 0x000753C4
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.UnregisterEvents();
			this.TargetItem = this.GetTargetItem();
			if (this.TargetItem == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.SetupItem(this.TargetItem);
			}
			this.RegisterEvents();
			this.requireRefresh = false;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0007721C File Offset: 0x0007541C
		private void SetupItem(Item targetItem)
		{
			if (this.notInteractableIndicator)
			{
				this.notInteractableIndicator.gameObject.SetActive(false);
			}
			this.itemDisplay.Setup(targetItem);
			this.itemDisplay.gameObject.SetActive(true);
			this.notInteractableIndicator.gameObject.SetActive(!this.Interactable);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0007727D File Offset: 0x0007547D
		private void SetupEmpty()
		{
			this.itemDisplay.gameObject.SetActive(false);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00077290 File Offset: 0x00075490
		private void RegisterEvents()
		{
			ItemShortcut.OnSetItem += this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged += this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount += this.OnItemStackCountChanged;
			}
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000772F8 File Offset: 0x000754F8
		private void UnregisterEvents()
		{
			ItemShortcut.OnSetItem -= this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged -= this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount -= this.OnItemStackCountChanged;
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00077360 File Offset: 0x00075560
		private void OnItemShortcutSetItem(int obj)
		{
			this.Refresh();
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00077368 File Offset: 0x00075568
		private void OnItemStackCountChanged(Item item)
		{
			if (item != this.TargetItem)
			{
				return;
			}
			this.requireRefresh = true;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00077380 File Offset: 0x00075580
		private void OnContentChanged(Inventory inventory, int index)
		{
			this.requireRefresh = true;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0007738C File Offset: 0x0007558C
		private void Update()
		{
			if (this.requireRefresh)
			{
				this.Refresh();
			}
			bool flag = this.TargetItem != null && this.Character.CurrentHoldItemAgent != null && this.TargetItem == this.Character.CurrentHoldItemAgent.Item;
			if (flag && !this.lastFrameUsing)
			{
				this.OnStartedUsing();
			}
			else if (!flag && this.lastFrameUsing)
			{
				this.OnStoppedUsing();
			}
			this.usingIndicator.gameObject.SetActive(flag);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0007741C File Offset: 0x0007561C
		private void OnStartedUsing()
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0007741E File Offset: 0x0007561E
		private void OnStoppedUsing()
		{
		}

		// Token: 0x040016F6 RID: 5878
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040016F7 RID: 5879
		[SerializeField]
		private GameObject usingIndicator;

		// Token: 0x040016F8 RID: 5880
		[SerializeField]
		private GameObject notInteractableIndicator;

		// Token: 0x040016F9 RID: 5881
		[SerializeField]
		private Image denialIndicator;

		// Token: 0x040016FA RID: 5882
		[SerializeField]
		private Color denialColor;

		// Token: 0x04001701 RID: 5889
		private bool isBeingDestroyed;

		// Token: 0x04001702 RID: 5890
		private bool requireRefresh;

		// Token: 0x04001703 RID: 5891
		private bool lastFrameUsing;
	}
}
