using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AC RID: 684
	public class PassivePropDisplay : MonoBehaviour
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x0005305A File Offset: 0x0005125A
		// (set) Token: 0x06001668 RID: 5736 RVA: 0x00053062 File Offset: 0x00051262
		public RectTransform rectTransform { get; private set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x0005306B File Offset: 0x0005126B
		public NavEntry NavEntry
		{
			get
			{
				return this.navEntry;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00053073 File Offset: 0x00051273
		// (set) Token: 0x0600166B RID: 5739 RVA: 0x0005307B File Offset: 0x0005127B
		public GoldMinerArtifact Target { get; private set; }

		// Token: 0x0600166C RID: 5740 RVA: 0x00053084 File Offset: 0x00051284
		internal void Setup(GoldMinerArtifact target, int amount)
		{
			this.Target = target;
			this.icon.sprite = target.Icon;
			this.rectTransform = (base.transform as RectTransform);
			this.amounText.text = ((amount > 1) ? string.Format("{0}", amount) : "");
		}

		// Token: 0x04001093 RID: 4243
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x04001094 RID: 4244
		[SerializeField]
		private Image icon;

		// Token: 0x04001095 RID: 4245
		[SerializeField]
		private TextMeshProUGUI amounText;
	}
}
