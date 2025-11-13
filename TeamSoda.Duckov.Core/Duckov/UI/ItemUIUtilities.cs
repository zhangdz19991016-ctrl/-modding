using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AD RID: 941
	public static class ItemUIUtilities
	{
		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060021C3 RID: 8643 RVA: 0x000763A8 File Offset: 0x000745A8
		// (remove) Token: 0x060021C4 RID: 8644 RVA: 0x000763DC File Offset: 0x000745DC
		public static event Action OnSelectionChanged;

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060021C5 RID: 8645 RVA: 0x00076410 File Offset: 0x00074610
		// (remove) Token: 0x060021C6 RID: 8646 RVA: 0x00076444 File Offset: 0x00074644
		public static event Action<Item> OnOrphanRaised;

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x00076477 File Offset: 0x00074677
		public static ItemDisplay SelectedItemDisplayRaw
		{
			get
			{
				return ItemUIUtilities.selectedItemDisplay;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x0007647E File Offset: 0x0007467E
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x000764A8 File Offset: 0x000746A8
		public static ItemDisplay SelectedItemDisplay
		{
			get
			{
				if (ItemUIUtilities.selectedItemDisplay == null)
				{
					return null;
				}
				if (ItemUIUtilities.selectedItemDisplay.Target == null)
				{
					return null;
				}
				return ItemUIUtilities.selectedItemDisplay;
			}
			private set
			{
				ItemDisplay itemDisplay = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay != null)
				{
					itemDisplay.NotifyUnselected();
				}
				ItemUIUtilities.selectedItemDisplay = value;
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					ItemUIUtilities.selectedItemTypeID = -1;
				}
				else
				{
					ItemUIUtilities.selectedItemTypeID = selectedItem.TypeID;
					ItemUIUtilities.cachedSelectedItemMeta = ItemAssetsCollection.GetMetaData(ItemUIUtilities.selectedItemTypeID);
					ItemUIUtilities.cacheGunSelected = selectedItem.Tags.Contains("Gun");
				}
				ItemDisplay itemDisplay2 = ItemUIUtilities.selectedItemDisplay;
				if (itemDisplay2 != null)
				{
					itemDisplay2.NotifySelected();
				}
				Action onSelectionChanged = ItemUIUtilities.OnSelectionChanged;
				if (onSelectionChanged == null)
				{
					return;
				}
				onSelectionChanged();
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00076530 File Offset: 0x00074730
		public static Item SelectedItem
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay == null)
				{
					return null;
				}
				return ItemUIUtilities.SelectedItemDisplay.Target;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x0007654B File Offset: 0x0007474B
		public static bool IsGunSelected
		{
			get
			{
				return !(ItemUIUtilities.SelectedItem == null) && ItemUIUtilities.cacheGunSelected;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x00076561 File Offset: 0x00074761
		public static string SelectedItemCaliber
		{
			get
			{
				return ItemUIUtilities.cachedSelectedItemMeta.caliber;
			}
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060021CD RID: 8653 RVA: 0x00076570 File Offset: 0x00074770
		// (remove) Token: 0x060021CE RID: 8654 RVA: 0x000765A4 File Offset: 0x000747A4
		public static event Action<Item, bool> OnPutItem;

		// Token: 0x060021CF RID: 8655 RVA: 0x000765D7 File Offset: 0x000747D7
		public static void Select(ItemDisplay itemDisplay)
		{
			ItemUIUtilities.SelectedItemDisplay = itemDisplay;
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000765DF File Offset: 0x000747DF
		public static void RaiseOrphan(Item orphan)
		{
			if (orphan == null)
			{
				return;
			}
			Action<Item> onOrphanRaised = ItemUIUtilities.OnOrphanRaised;
			if (onOrphanRaised != null)
			{
				onOrphanRaised(orphan);
			}
			Debug.LogWarning(string.Format("游戏中出现了孤儿Item {0}。", orphan));
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x0007660C File Offset: 0x0007480C
		public static void NotifyPutItem(Item item, bool pickup = false)
		{
			Action<Item, bool> onPutItem = ItemUIUtilities.OnPutItem;
			if (onPutItem == null)
			{
				return;
			}
			onPutItem(item, pickup);
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00076620 File Offset: 0x00074820
		public static string GetPropertiesDisplayText(this Item item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						stringBuilder.AppendLine(customData.DisplayName + "\t" + customData.GetValueDisplayString(""));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						stringBuilder.AppendLine(customData2.DisplayName + "\t" + customData2.GetValueDisplayString(""));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						stringBuilder.AppendLine(string.Format("{0}\t{1}", stat.DisplayName, stat.Value));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						stringBuilder.AppendLine(modifierDescription.DisplayName + "\t" + modifierDescription.GetDisplayValueString("0.##"));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000767F8 File Offset: 0x000749F8
		[return: TupleElementNames(new string[]
		{
			"name",
			"value",
			"polarity"
		})]
		public static List<ValueTuple<string, string, Polarity>> GetPropertyValueTextPair(this Item item)
		{
			List<ValueTuple<string, string, Polarity>> list = new List<ValueTuple<string, string, Polarity>>();
			if (item.Variables != null)
			{
				foreach (CustomData customData in item.Variables)
				{
					if (customData.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData.DisplayName, customData.GetValueDisplayString(""), Polarity.Neutral));
					}
				}
			}
			if (item.Constants != null)
			{
				foreach (CustomData customData2 in item.Constants)
				{
					if (customData2.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(customData2.DisplayName, customData2.GetValueDisplayString(""), Polarity.Neutral));
					}
				}
			}
			if (item.Stats != null)
			{
				foreach (Stat stat in item.Stats)
				{
					if (stat.Display)
					{
						list.Add(new ValueTuple<string, string, Polarity>(stat.DisplayName, stat.Value.ToString(), Polarity.Neutral));
					}
				}
			}
			if (item.Modifiers != null)
			{
				foreach (ModifierDescription modifierDescription in item.Modifiers)
				{
					if (modifierDescription.Display)
					{
						Polarity polarity = StatInfoDatabase.GetPolarity(modifierDescription.Key);
						if (modifierDescription.Value < 0f)
						{
							polarity = -polarity;
						}
						list.Add(new ValueTuple<string, string, Polarity>(modifierDescription.DisplayName, modifierDescription.GetDisplayValueString("0.##"), polarity));
					}
				}
			}
			return list;
		}

		// Token: 0x040016E3 RID: 5859
		private static ItemDisplay selectedItemDisplay;

		// Token: 0x040016E4 RID: 5860
		private static bool cacheGunSelected;

		// Token: 0x040016E5 RID: 5861
		private static int selectedItemTypeID;

		// Token: 0x040016E6 RID: 5862
		private static ItemMetaData cachedSelectedItemMeta;
	}
}
