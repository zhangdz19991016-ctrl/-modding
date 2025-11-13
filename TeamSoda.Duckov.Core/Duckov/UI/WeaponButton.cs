using System;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AA RID: 938
	public class WeaponButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x0600218C RID: 8588 RVA: 0x000754EC File Offset: 0x000736EC
		// (remove) Token: 0x0600218D RID: 8589 RVA: 0x00075520 File Offset: 0x00073720
		public static event Action<WeaponButton> OnWeaponButtonSelected;

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x00075553 File Offset: 0x00073753
		private CharacterMainControl Character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x0007555B File Offset: 0x0007375B
		private Slot TargetSlot
		{
			get
			{
				return this._targetSlot;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x00075563 File Offset: 0x00073763
		private Item TargetItem
		{
			get
			{
				Slot targetSlot = this.TargetSlot;
				if (targetSlot == null)
				{
					return null;
				}
				return targetSlot.Content;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x00075578 File Offset: 0x00073778
		private bool IsSelected
		{
			get
			{
				Item targetItem = this.TargetItem;
				if (((targetItem != null) ? targetItem.ActiveAgent : null) != null)
				{
					UnityEngine.Object activeAgent = this.TargetItem.ActiveAgent;
					ItemAgentHolder agentHolder = this._character.agentHolder;
					return activeAgent == ((agentHolder != null) ? agentHolder.CurrentHoldItemAgent : null);
				}
				return false;
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000755C8 File Offset: 0x000737C8
		private void Awake()
		{
			this.RegisterStaticEvents();
			LevelManager instance = LevelManager.Instance;
			if (((instance != null) ? instance.MainCharacter : null) != null)
			{
				this.Initialize(LevelManager.Instance.MainCharacter);
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000755F9 File Offset: 0x000737F9
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
			this.isBeingDestroyed = true;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00075608 File Offset: 0x00073808
		private void RegisterStaticEvents()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Combine(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0007563B File Offset: 0x0007383B
		private void UnregisterStaticEvents()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Remove(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0007566E File Offset: 0x0007386E
		private void OnMainCharacterChangeHoldItemAgent(CharacterMainControl control, DuckovItemAgent agent)
		{
			if (this._character && control == this._character)
			{
				this.Refresh();
			}
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00075691 File Offset: 0x00073891
		private void OnLevelInitialized()
		{
			LevelManager instance = LevelManager.Instance;
			this.Initialize((instance != null) ? instance.MainCharacter : null);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x000756AC File Offset: 0x000738AC
		private void Initialize(CharacterMainControl character)
		{
			this.UnregisterEvents();
			this._character = character;
			if (character == null)
			{
				Debug.LogError("Character 不存在，初始化失败");
			}
			if (character.CharacterItem == null)
			{
				Debug.LogError("Character item 不存在，初始化失败");
			}
			this._targetSlot = character.CharacterItem.Slots.GetSlot(this.targetSlotKey);
			if (this._targetSlot == null)
			{
				Debug.LogError("Slot " + this.targetSlotKey + " 不存在，初始化失败");
			}
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0007573B File Offset: 0x0007393B
		private void RegisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0007575D File Offset: 0x0007395D
		private void UnregisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0007577F File Offset: 0x0007397F
		private void OnSlotContentChanged(Slot slot)
		{
			this.Refresh();
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00075788 File Offset: 0x00073988
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.displayParent.SetActive(this.TargetItem);
			bool isSelected = this.IsSelected;
			if (this.TargetItem)
			{
				this.icon.sprite = this.TargetItem.Icon;
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.TargetItem.DisplayQuality);
				this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.selectionFrame.SetActive(isSelected);
			}
			UnityEvent<WeaponButton> unityEvent = this.onRefresh;
			if (unityEvent != null)
			{
				unityEvent.Invoke(this);
			}
			if (isSelected)
			{
				UnityEvent<WeaponButton> unityEvent2 = this.onSelected;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke(this);
				}
				Action<WeaponButton> onWeaponButtonSelected = WeaponButton.OnWeaponButtonSelected;
				if (onWeaponButtonSelected == null)
				{
					return;
				}
				onWeaponButtonSelected(this);
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0007586A File Offset: 0x00073A6A
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Character == null)
			{
				return;
			}
			UnityEvent<WeaponButton> unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x040016B7 RID: 5815
		[SerializeField]
		private string targetSlotKey = "PrimaryWeapon";

		// Token: 0x040016B8 RID: 5816
		[SerializeField]
		private GameObject displayParent;

		// Token: 0x040016B9 RID: 5817
		[SerializeField]
		private Image icon;

		// Token: 0x040016BA RID: 5818
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x040016BB RID: 5819
		[SerializeField]
		private GameObject selectionFrame;

		// Token: 0x040016BC RID: 5820
		public UnityEvent<WeaponButton> onClick;

		// Token: 0x040016BD RID: 5821
		public UnityEvent<WeaponButton> onRefresh;

		// Token: 0x040016BE RID: 5822
		public UnityEvent<WeaponButton> onSelected;

		// Token: 0x040016C0 RID: 5824
		private CharacterMainControl _character;

		// Token: 0x040016C1 RID: 5825
		private Slot _targetSlot;

		// Token: 0x040016C2 RID: 5826
		private bool isBeingDestroyed;
	}
}
