using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000355 RID: 853
	public class TaskEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0006AA62 File Offset: 0x00068C62
		// (set) Token: 0x06001DB4 RID: 7604 RVA: 0x0006AA6A File Offset: 0x00068C6A
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0006AA73 File Offset: 0x00068C73
		private void Awake()
		{
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0006AA91 File Offset: 0x00068C91
		private void OnInteractionButtonClicked()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.Interact();
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0006AAAD File Offset: 0x00068CAD
		public void NotifyPooled()
		{
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0006AAAF File Offset: 0x00068CAF
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0006AABE File Offset: 0x00068CBE
		internal void Setup(Task target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0006AADC File Offset: 0x00068CDC
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.description.text = this.target.Description;
			foreach (string str in this.target.ExtraDescriptsions)
			{
				TextMeshProUGUI textMeshProUGUI = this.description;
				textMeshProUGUI.text = textMeshProUGUI.text + "  \n- " + str;
			}
			Sprite icon = this.target.Icon;
			if (icon)
			{
				this.taskIcon.sprite = icon;
				this.taskIcon.gameObject.SetActive(true);
			}
			else
			{
				this.taskIcon.gameObject.SetActive(false);
			}
			bool flag = this.target.IsFinished();
			this.statusIcon.sprite = (flag ? this.satisfiedIcon : this.unsatisfiedIcon);
			if (this.Interactable && !flag && this.target.Interactable)
			{
				bool possibleValidInteraction = this.target.PossibleValidInteraction;
				this.interactionText.text = this.target.InteractText;
				this.interactionPlaceHolderText.text = this.target.InteractText;
				this.interactionButton.gameObject.SetActive(possibleValidInteraction);
				this.targetNotInteractablePlaceHolder.gameObject.SetActive(!possibleValidInteraction);
				return;
			}
			this.interactionButton.gameObject.SetActive(false);
			this.targetNotInteractablePlaceHolder.gameObject.SetActive(false);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0006AC54 File Offset: 0x00068E54
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0006AC7C File Offset: 0x00068E7C
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0006ACA4 File Offset: 0x00068EA4
		private void OnTargetStatusChanged(Task task)
		{
			if (task != this.target)
			{
				Debug.LogError("目标不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0006ACC5 File Offset: 0x00068EC5
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				this.target.ForceFinish();
				eventData.Use();
			}
		}

		// Token: 0x04001498 RID: 5272
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04001499 RID: 5273
		[SerializeField]
		private Image taskIcon;

		// Token: 0x0400149A RID: 5274
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x0400149B RID: 5275
		[SerializeField]
		private Button interactionButton;

		// Token: 0x0400149C RID: 5276
		[SerializeField]
		private GameObject targetNotInteractablePlaceHolder;

		// Token: 0x0400149D RID: 5277
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x0400149E RID: 5278
		[SerializeField]
		private TextMeshProUGUI interactionPlaceHolderText;

		// Token: 0x0400149F RID: 5279
		[SerializeField]
		private Sprite unsatisfiedIcon;

		// Token: 0x040014A0 RID: 5280
		[SerializeField]
		private Sprite satisfiedIcon;

		// Token: 0x040014A1 RID: 5281
		[SerializeField]
		private bool interactable;

		// Token: 0x040014A2 RID: 5282
		private Task target;
	}
}
