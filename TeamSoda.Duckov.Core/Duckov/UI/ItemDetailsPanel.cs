using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000399 RID: 921
	public class ItemDetailsPanel : ManagedUIElement
	{
		// Token: 0x06002087 RID: 8327 RVA: 0x000723B6 File Offset: 0x000705B6
		protected override void Awake()
		{
			base.Awake();
			if (ItemDetailsPanel.instance == null)
			{
				ItemDetailsPanel.instance = this;
			}
			this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000723ED File Offset: 0x000705ED
		private void OnCloseButtonClicked()
		{
			base.Close();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000723F5 File Offset: 0x000705F5
		public static void Show(Item target, ManagedUIElement source = null)
		{
			if (ItemDetailsPanel.instance == null)
			{
				return;
			}
			ItemDetailsPanel.instance.Open(target, source);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00072411 File Offset: 0x00070611
		public void Open(Item target, ManagedUIElement source)
		{
			this.target = target;
			this.source = source;
			base.Open(source);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00072428 File Offset: 0x00070628
		protected override void OnOpen()
		{
			if (this.target == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.Setup(this.target);
			this.fadeGroup.Show();
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0007245C File Offset: 0x0007065C
		protected override void OnClose()
		{
			this.UnregisterEvents();
			this.target = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00072476 File Offset: 0x00070676
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0007247E File Offset: 0x0007067E
		internal void Setup(Item target)
		{
			this.display.Setup(target);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x0007248C File Offset: 0x0007068C
		private void UnregisterEvents()
		{
			this.display.UnregisterEvents();
		}

		// Token: 0x0400162C RID: 5676
		private static ItemDetailsPanel instance;

		// Token: 0x0400162D RID: 5677
		private Item target;

		// Token: 0x0400162E RID: 5678
		[SerializeField]
		private ItemDetailsDisplay display;

		// Token: 0x0400162F RID: 5679
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001630 RID: 5680
		[SerializeField]
		private Button closeButton;

		// Token: 0x04001631 RID: 5681
		private ManagedUIElement source;
	}
}
