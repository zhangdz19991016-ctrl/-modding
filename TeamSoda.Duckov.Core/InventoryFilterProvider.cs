using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class InventoryFilterProvider : MonoBehaviour
{
	// Token: 0x04000C40 RID: 3136
	public InventoryFilterProvider.FilterEntry[] entries;

	// Token: 0x020004E6 RID: 1254
	[Serializable]
	public struct FilterEntry
	{
		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000903D6 File Offset: 0x0008E5D6
		public string DisplayName
		{
			get
			{
				return this.name.ToPlainText();
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000903E4 File Offset: 0x0008E5E4
		private bool FilterFunction(Item item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.requireTags.Length == 0)
			{
				return true;
			}
			foreach (Tag tag in this.requireTags)
			{
				if (!(tag == null) && item.Tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x0009043A File Offset: 0x0008E63A
		public Func<Item, bool> GetFunction()
		{
			if (this.requireTags.Length == 0)
			{
				return null;
			}
			return new Func<Item, bool>(this.FilterFunction);
		}

		// Token: 0x04001D56 RID: 7510
		[LocalizationKey("Default")]
		public string name;

		// Token: 0x04001D57 RID: 7511
		public Sprite icon;

		// Token: 0x04001D58 RID: 7512
		public Tag[] requireTags;
	}
}
