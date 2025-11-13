using System;
using TMPro;
using UnityEngine;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002FB RID: 763
	public class EndowmentDisplay : MonoBehaviour
	{
		// Token: 0x060018F4 RID: 6388 RVA: 0x0005B480 File Offset: 0x00059680
		private void Refresh()
		{
			EndowmentEntry endowmentEntry = EndowmentManager.Current;
			if (endowmentEntry == null)
			{
				this.displayNameText.text = "?";
				this.descriptionsText.text = "?";
				return;
			}
			this.displayNameText.text = endowmentEntry.DisplayName;
			this.descriptionsText.text = endowmentEntry.DescriptionAndEffects;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0005B4DF File Offset: 0x000596DF
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x04001220 RID: 4640
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001221 RID: 4641
		[SerializeField]
		private TextMeshProUGUI descriptionsText;
	}
}
