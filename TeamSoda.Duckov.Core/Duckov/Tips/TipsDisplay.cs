using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x0200024A RID: 586
	public class TipsDisplay : MonoBehaviour
	{
		// Token: 0x0600125F RID: 4703 RVA: 0x00046608 File Offset: 0x00044808
		public void DisplayRandom()
		{
			if (this.entries.Length == 0)
			{
				return;
			}
			TipEntry tipEntry = this.entries[UnityEngine.Random.Range(0, this.entries.Length)];
			this.text.text = tipEntry.Description;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0004664C File Offset: 0x0004484C
		public void Display(string tipID)
		{
			TipEntry tipEntry = this.entries.FirstOrDefault((TipEntry e) => e.TipID == tipID);
			if (tipEntry.TipID != tipID)
			{
				return;
			}
			this.text.text = tipEntry.Description;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000466A5 File Offset: 0x000448A5
		private void OnEnable()
		{
			this.canvasGroup.alpha = (SceneLoader.HideTips ? 0f : 1f);
			this.DisplayRandom();
		}

		// Token: 0x04000E16 RID: 3606
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000E17 RID: 3607
		[SerializeField]
		private TipEntry[] entries;

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private CanvasGroup canvasGroup;
	}
}
