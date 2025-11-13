using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A2 RID: 674
	public class GoldMinerMoneyDisplay : MonoBehaviour
	{
		// Token: 0x06001625 RID: 5669 RVA: 0x00052540 File Offset: 0x00050740
		private void Update()
		{
			this.text.text = this.master.Money.ToString(this.format);
		}

		// Token: 0x04001053 RID: 4179
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001054 RID: 4180
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001055 RID: 4181
		[SerializeField]
		private string format = "$0";
	}
}
