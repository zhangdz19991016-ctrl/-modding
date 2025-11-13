using System;
using TMPro;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023F RID: 575
	public class GameVersionDisplay : MonoBehaviour
	{
		// Token: 0x060011F6 RID: 4598 RVA: 0x0004587C File Offset: 0x00043A7C
		private void Start()
		{
			this.text.text = string.Format("v{0}", GameMetaData.Instance.Version);
		}

		// Token: 0x04000DDB RID: 3547
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
