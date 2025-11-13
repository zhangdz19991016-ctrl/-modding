using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B7 RID: 951
	public static class TradingUIUtilities
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x00077C72 File Offset: 0x00075E72
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x00077C7E File Offset: 0x00075E7E
		public static IMerchant ActiveMerchant
		{
			get
			{
				return TradingUIUtilities.activeMerchant as IMerchant;
			}
			set
			{
				TradingUIUtilities.activeMerchant = (value as UnityEngine.Object);
				Action<IMerchant> onActiveMerchantChanged = TradingUIUtilities.OnActiveMerchantChanged;
				if (onActiveMerchantChanged == null)
				{
					return;
				}
				onActiveMerchantChanged(value);
			}
		}

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x0600224D RID: 8781 RVA: 0x00077C9C File Offset: 0x00075E9C
		// (remove) Token: 0x0600224E RID: 8782 RVA: 0x00077CD0 File Offset: 0x00075ED0
		public static event Action<IMerchant> OnActiveMerchantChanged;

		// Token: 0x04001726 RID: 5926
		private static UnityEngine.Object activeMerchant;
	}
}
