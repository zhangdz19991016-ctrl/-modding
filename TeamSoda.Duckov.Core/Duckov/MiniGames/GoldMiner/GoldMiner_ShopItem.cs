using System;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029A RID: 666
	public class GoldMiner_ShopItem : MonoBehaviour
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x00051F6E File Offset: 0x0005016E
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x00051F76 File Offset: 0x00050176
		public string DisplayNameKey
		{
			get
			{
				return this.displayNameKey;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00051F7E File Offset: 0x0005017E
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x00051F8B File Offset: 0x0005018B
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00051F93 File Offset: 0x00050193
		public void OnBought(GoldMiner target)
		{
			UnityEvent<GoldMiner> unityEvent = this.onBought;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(target);
		}

		// Token: 0x0400103C RID: 4156
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400103D RID: 4157
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayNameKey;

		// Token: 0x0400103E RID: 4158
		[SerializeField]
		private int basePrice;

		// Token: 0x0400103F RID: 4159
		public UnityEvent<GoldMiner> onBought;
	}
}
