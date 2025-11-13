using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A3 RID: 931
	public class ItemSlotCollectionDisplay : MonoBehaviour
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002126 RID: 8486 RVA: 0x00074187 File Offset: 0x00072387
		// (set) Token: 0x06002127 RID: 8487 RVA: 0x0007418F File Offset: 0x0007238F
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

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x00074198 File Offset: 0x00072398
		// (set) Token: 0x06002129 RID: 8489 RVA: 0x000741A0 File Offset: 0x000723A0
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x000741A9 File Offset: 0x000723A9
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x000741B1 File Offset: 0x000723B1
		// (set) Token: 0x0600212C RID: 8492 RVA: 0x000741B9 File Offset: 0x000723B9
		public bool Movable { get; private set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x000741C2 File Offset: 0x000723C2
		// (set) Token: 0x0600212E RID: 8494 RVA: 0x000741CA File Offset: 0x000723CA
		public Item Target { get; private set; }

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x0600212F RID: 8495 RVA: 0x000741D4 File Offset: 0x000723D4
		// (remove) Token: 0x06002130 RID: 8496 RVA: 0x0007420C File Offset: 0x0007240C
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementClicked;

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06002131 RID: 8497 RVA: 0x00074244 File Offset: 0x00072444
		// (remove) Token: 0x06002132 RID: 8498 RVA: 0x0007427C File Offset: 0x0007247C
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementDoubleClicked;

		// Token: 0x06002133 RID: 8499 RVA: 0x000742B4 File Offset: 0x000724B4
		public void Setup(Item target, bool movable = false)
		{
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Slots == null)
			{
				return;
			}
			this.Movable = movable;
			for (int i = 0; i < this.Target.Slots.Count; i++)
			{
				Slot slot = this.Target.Slots[i];
				if (slot != null)
				{
					SlotDisplay slotDisplay = SlotDisplay.Get();
					slotDisplay.onSlotDisplayClicked += this.OnSlotDisplayClicked;
					slotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDisplayDoubleClicked;
					slotDisplay.ShowOperationMenu = this.ShowOperationMenu;
					slotDisplay.Setup(slot);
					slotDisplay.Editable = this.editable;
					slotDisplay.ContentSelectable = this.contentSelectable;
					slotDisplay.transform.SetParent(this.entriesParent, false);
					slotDisplay.Movable = this.Movable;
					this.slots.Add(slotDisplay);
				}
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000743AD File Offset: 0x000725AD
		private void OnSlotDisplayDoubleClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, display);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000743C4 File Offset: 0x000725C4
		private void Clear()
		{
			foreach (SlotDisplay slotDisplay in this.slots)
			{
				slotDisplay.onSlotDisplayClicked -= this.OnSlotDisplayClicked;
				SlotDisplay.Release(slotDisplay);
			}
			this.slots.Clear();
			this.entriesParent.DestroyAllChildren();
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x0007443C File Offset: 0x0007263C
		private void OnSlotDisplayClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementClicked;
			if (action != null)
			{
				action(this, display);
			}
			if (!this.editable && this.notifyNotEditable)
			{
				this.ShowNotEditableIndicator().Forget();
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0007446C File Offset: 0x0007266C
		private UniTask ShowNotEditableIndicator()
		{
			ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36 <ShowNotEditableIndicator>d__;
			<ShowNotEditableIndicator>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNotEditableIndicator>d__.<>4__this = this;
			<ShowNotEditableIndicator>d__.<>1__state = -1;
			<ShowNotEditableIndicator>d__.<>t__builder.Start<ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36>(ref <ShowNotEditableIndicator>d__);
			return <ShowNotEditableIndicator>d__.<>t__builder.Task;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000744ED File Offset: 0x000726ED
		[CompilerGenerated]
		private bool <ShowNotEditableIndicator>g__TokenChanged|36_0(ref ItemSlotCollectionDisplay.<>c__DisplayClass36_0 A_1)
		{
			return A_1.token != this.currentToken;
		}

		// Token: 0x04001687 RID: 5767
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x04001688 RID: 5768
		[SerializeField]
		private CanvasGroup notEditableIndicator;

		// Token: 0x04001689 RID: 5769
		[SerializeField]
		private bool editable = true;

		// Token: 0x0400168A RID: 5770
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x0400168B RID: 5771
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x0400168C RID: 5772
		[SerializeField]
		private bool notifyNotEditable;

		// Token: 0x0400168D RID: 5773
		[SerializeField]
		private float fadeDuration = 1f;

		// Token: 0x0400168E RID: 5774
		[SerializeField]
		private float sustainDuration = 1f;

		// Token: 0x04001691 RID: 5777
		private List<SlotDisplay> slots = new List<SlotDisplay>();

		// Token: 0x04001694 RID: 5780
		private int currentToken;
	}
}
