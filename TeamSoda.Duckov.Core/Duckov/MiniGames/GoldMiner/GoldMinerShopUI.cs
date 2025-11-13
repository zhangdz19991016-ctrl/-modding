using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A3 RID: 675
	public class GoldMinerShopUI : MiniGameBehaviour
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x00052584 File Offset: 0x00050784
		// (set) Token: 0x06001628 RID: 5672 RVA: 0x0005258C File Offset: 0x0005078C
		public GoldMinerShop target { get; private set; }

		// Token: 0x06001629 RID: 5673 RVA: 0x00052595 File Offset: 0x00050795
		private void UnregisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Remove(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000525CD File Offset: 0x000507CD
		private void RegisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Combine(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00052605 File Offset: 0x00050805
		private void OnAfterOperation()
		{
			this.RefreshEntries();
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00052610 File Offset: 0x00050810
		private void RefreshEntries()
		{
			for (int i = 0; i < this.entries.Length; i++)
			{
				GoldMinerShopUIEntry goldMinerShopUIEntry = this.entries[i];
				if (i >= this.target.stock.Count)
				{
					goldMinerShopUIEntry.gameObject.SetActive(false);
				}
				else
				{
					goldMinerShopUIEntry.gameObject.SetActive(true);
					ShopEntity target = this.target.stock[i];
					goldMinerShopUIEntry.Setup(this, target);
				}
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00052680 File Offset: 0x00050880
		public void Setup(GoldMinerShop shop)
		{
			this.UnregisterEvent();
			this.target = shop;
			this.RegisterEvent();
			this.RefreshEntries();
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0005269B File Offset: 0x0005089B
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.RefreshDescriptionText();
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000526AC File Offset: 0x000508AC
		private void RefreshDescriptionText()
		{
			string text = "";
			if (this.hoveringEntry != null && this.hoveringEntry.target != null && this.hoveringEntry.target.artifact != null)
			{
				text = this.hoveringEntry.target.artifact.Description;
			}
			this.descriptionText.text = text;
		}

		// Token: 0x04001056 RID: 4182
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001057 RID: 4183
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04001058 RID: 4184
		[SerializeField]
		private GoldMinerShopUIEntry[] entries;

		// Token: 0x04001059 RID: 4185
		public int navIndex;

		// Token: 0x0400105B RID: 4187
		public bool enableInput;

		// Token: 0x0400105C RID: 4188
		public GoldMinerShopUIEntry hoveringEntry;
	}
}
