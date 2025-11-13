using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039B RID: 923
	public class ItemModifierEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002099 RID: 8345 RVA: 0x000724F3 File Offset: 0x000706F3
		public void NotifyPooled()
		{
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000724F5 File Offset: 0x000706F5
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000724FD File Offset: 0x000706FD
		internal void Setup(ModifierDescription target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00072518 File Offset: 0x00070718
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.value.text = this.target.GetDisplayValueString(entry.DisplayFormat);
			Color color = this.color_Neutral;
			Polarity polarity = entry.polarity;
			if (this.target.Value != 0f)
			{
				switch (polarity)
				{
				case Polarity.Negative:
					color = ((this.target.Value < 0f) ? this.color_Positive : this.color_Negative);
					break;
				case Polarity.Positive:
					color = ((this.target.Value > 0f) ? this.color_Positive : this.color_Negative);
					break;
				}
			}
			this.value.color = color;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000725EF File Offset: 0x000707EF
		private void RegisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000725F8 File Offset: 0x000707F8
		private void UnregisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x04001634 RID: 5684
		private ModifierDescription target;

		// Token: 0x04001635 RID: 5685
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001636 RID: 5686
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x04001637 RID: 5687
		[SerializeField]
		private Color color_Neutral;

		// Token: 0x04001638 RID: 5688
		[SerializeField]
		private Color color_Positive;

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private Color color_Negative;
	}
}
