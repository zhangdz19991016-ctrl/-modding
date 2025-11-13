using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003D2 RID: 978
	public class PagesControl_Entry : MonoBehaviour
	{
		// Token: 0x060023B6 RID: 9142 RVA: 0x0007D691 File Offset: 0x0007B891
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0007D6AF File Offset: 0x0007B8AF
		private void OnButtonClicked()
		{
			this.master.NotifySelect(this.index);
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0007D6C4 File Offset: 0x0007B8C4
		internal void Setup(PagesControl master, int i, bool selected)
		{
			this.master = master;
			this.index = i;
			this.selected = selected;
			this.text.text = string.Format("{0}", this.index);
			this.selectedIndicator.SetActive(this.selected);
		}

		// Token: 0x0400183F RID: 6207
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001840 RID: 6208
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001841 RID: 6209
		[SerializeField]
		private Button button;

		// Token: 0x04001842 RID: 6210
		private PagesControl master;

		// Token: 0x04001843 RID: 6211
		private int index;

		// Token: 0x04001844 RID: 6212
		private bool selected;
	}
}
