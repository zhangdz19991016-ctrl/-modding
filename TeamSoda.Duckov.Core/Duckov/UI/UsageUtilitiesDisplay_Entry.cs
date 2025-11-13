using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A0 RID: 928
	public class UsageUtilitiesDisplay_Entry : MonoBehaviour
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x00072A24 File Offset: 0x00070C24
		// (set) Token: 0x060020BF RID: 8383 RVA: 0x00072A2C File Offset: 0x00070C2C
		public UsageBehavior Target { get; private set; }

		// Token: 0x060020C0 RID: 8384 RVA: 0x00072A38 File Offset: 0x00070C38
		internal void Setup(UsageBehavior cur)
		{
			this.text.text = cur.DisplaySettings.Description;
		}

		// Token: 0x04001646 RID: 5702
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
