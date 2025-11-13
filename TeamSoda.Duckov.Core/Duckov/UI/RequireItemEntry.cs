using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C4 RID: 964
	public class RequireItemEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002321 RID: 8993 RVA: 0x0007B30B File Offset: 0x0007950B
		public void NotifyPooled()
		{
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0007B30D File Offset: 0x0007950D
		public void NotifyReleased()
		{
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0007B310 File Offset: 0x00079510
		public void Setup(PerkRequirement.RequireItemEntry target)
		{
			int id = target.id;
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(id);
			this.icon.sprite = metaData.icon;
			string displayName = metaData.DisplayName;
			int itemCount = ItemUtilities.GetItemCount(id);
			this.text.text = string.Format(this.textFormat, displayName, target.amount, itemCount);
		}

		// Token: 0x040017E4 RID: 6116
		[SerializeField]
		private Image icon;

		// Token: 0x040017E5 RID: 6117
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040017E6 RID: 6118
		[SerializeField]
		private string textFormat = "{0} x{1}";
	}
}
