using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001E0 RID: 480
public class UIButtonRevertBinding : MonoBehaviour
{
	// Token: 0x06000E5D RID: 3677 RVA: 0x0003A6EB File Offset: 0x000388EB
	private void Awake()
	{
		if (this.button == null)
		{
			this.button = base.GetComponent<Button>();
		}
		this.button.onClick.AddListener(new UnityAction(this.OnBtnClick));
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x0003A723 File Offset: 0x00038923
	public void OnBtnClick()
	{
		InputRebinder.Clear();
		InputRebinder.Save();
	}

	// Token: 0x04000BEC RID: 3052
	[SerializeField]
	private Button button;
}
