using System;
using System.Collections.Generic;
using DG.Tweening;
using NodeCanvas.DialogueTrees;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x0200021E RID: 542
	public class DialogueUIChoice : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0004017E File Offset: 0x0003E37E
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00040188 File Offset: 0x0003E388
		private void Awake()
		{
			MenuItem menuItem = this.menuItem;
			menuItem.onSelected = (Action<MenuItem>)Delegate.Combine(menuItem.onSelected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem2 = this.menuItem;
			menuItem2.onDeselected = (Action<MenuItem>)Delegate.Combine(menuItem2.onDeselected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem3 = this.menuItem;
			menuItem3.onFocusStatusChanged = (Action<MenuItem, bool>)Delegate.Combine(menuItem3.onFocusStatusChanged, new Action<MenuItem, bool>(this.Refresh));
			MenuItem menuItem4 = this.menuItem;
			menuItem4.onConfirmed = (Action<MenuItem>)Delegate.Combine(menuItem4.onConfirmed, new Action<MenuItem>(this.OnConfirm));
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00040231 File Offset: 0x0003E431
		private void OnConfirm(MenuItem item)
		{
			this.Confirm();
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0004023C File Offset: 0x0003E43C
		private void AnimateConfirm()
		{
			this.confirmIndicator.DOKill(false);
			this.confirmIndicator.DOGradientColor(this.confirmAnimationColor, this.confirmAnimationDuration).OnComplete(delegate
			{
				this.confirmIndicator.color = Color.clear;
			}).OnKill(delegate
			{
				this.confirmIndicator.color = Color.clear;
			});
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00040290 File Offset: 0x0003E490
		private void Refresh(MenuItem item, bool focus)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000402A8 File Offset: 0x0003E4A8
		private void Refresh(MenuItem item)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000402C0 File Offset: 0x0003E4C0
		private void Confirm()
		{
			this.master.NotifyChoiceConfirmed(this);
			this.AnimateConfirm();
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x000402D4 File Offset: 0x0003E4D4
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000402DC File Offset: 0x0003E4DC
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.menuItem.Select();
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000402EC File Offset: 0x0003E4EC
		internal void Setup(DialogueUI master, KeyValuePair<IStatement, int> cur)
		{
			this.master = master;
			this.index = cur.Value;
			this.text.text = cur.Key.text;
			this.confirmIndicator.color = Color.clear;
			this.Refresh(this.menuItem);
		}

		// Token: 0x04000D1D RID: 3357
		[SerializeField]
		private MenuItem menuItem;

		// Token: 0x04000D1E RID: 3358
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04000D1F RID: 3359
		[SerializeField]
		private Image confirmIndicator;

		// Token: 0x04000D20 RID: 3360
		[SerializeField]
		private Gradient confirmAnimationColor;

		// Token: 0x04000D21 RID: 3361
		[SerializeField]
		private float confirmAnimationDuration = 0.2f;

		// Token: 0x04000D22 RID: 3362
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000D23 RID: 3363
		private DialogueUI master;

		// Token: 0x04000D24 RID: 3364
		private int index;
	}
}
