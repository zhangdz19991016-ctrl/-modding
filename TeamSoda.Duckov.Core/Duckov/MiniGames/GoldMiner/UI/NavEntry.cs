using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner.UI
{
	// Token: 0x020002B1 RID: 689
	public class NavEntry : MonoBehaviour
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x000538B6 File Offset: 0x00051AB6
		// (set) Token: 0x0600168C RID: 5772 RVA: 0x000538BE File Offset: 0x00051ABE
		public bool selectionState { get; private set; }

		// Token: 0x0600168D RID: 5773 RVA: 0x000538C8 File Offset: 0x00051AC8
		private void Awake()
		{
			if (this.masterGroup == null)
			{
				this.masterGroup = base.GetComponentInParent<NavGroup>();
			}
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.TrySelectThis));
				this.VCT.onClick.AddListener(new UnityAction(this.Interact));
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00053940 File Offset: 0x00051B40
		private void Interact()
		{
			this.NotifyInteract();
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00053948 File Offset: 0x00051B48
		public void NotifySelectionState(bool value)
		{
			this.selectionState = value;
			this.selectedIndicator.SetActive(this.selectionState);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00053962 File Offset: 0x00051B62
		internal void NotifyInteract()
		{
			Action<NavEntry> action = this.onInteract;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00053975 File Offset: 0x00051B75
		public void TrySelectThis()
		{
			if (this.masterGroup == null)
			{
				return;
			}
			this.masterGroup.TrySelect(this);
		}

		// Token: 0x040010AD RID: 4269
		public GameObject selectedIndicator;

		// Token: 0x040010AE RID: 4270
		public Action<NavEntry> onInteract;

		// Token: 0x040010AF RID: 4271
		public Action<NavEntry> onTrySelectThis;

		// Token: 0x040010B0 RID: 4272
		public NavGroup masterGroup;

		// Token: 0x040010B1 RID: 4273
		public VirtualCursorTarget VCT;
	}
}
