using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000306 RID: 774
	public class VerticalEntry : MonoBehaviour
	{
		// Token: 0x0600193D RID: 6461 RVA: 0x0005C544 File Offset: 0x0005A744
		public void Setup(params string[] args)
		{
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0005C546 File Offset: 0x0005A746
		public void SetLayoutSpacing(float spacing)
		{
			this.layoutGroup.spacing = spacing;
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0005C554 File Offset: 0x0005A754
		public void SetPreferredWidth(float width)
		{
			this.layoutElement.preferredWidth = width;
		}

		// Token: 0x04001252 RID: 4690
		[SerializeField]
		private VerticalLayoutGroup layoutGroup;

		// Token: 0x04001253 RID: 4691
		[SerializeField]
		private LayoutElement layoutElement;
	}
}
