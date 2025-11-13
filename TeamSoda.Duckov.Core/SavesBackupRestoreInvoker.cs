using System;
using Duckov.UI.Animations;
using Duckov.UI.SavesRestore;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016A RID: 362
public class SavesBackupRestoreInvoker : MonoBehaviour
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x0002FA20 File Offset: 0x0002DC20
	private void Awake()
	{
		this.mainButton.onClick.AddListener(new UnityAction(this.OnMainButtonClicked));
		this.buttonSlot1.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(1);
		});
		this.buttonSlot2.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(2);
		});
		this.buttonSlot3.onClick.AddListener(delegate()
		{
			this.OnButtonClicked(3);
		});
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0002FA9D File Offset: 0x0002DC9D
	private void OnMainButtonClicked()
	{
		this.menuFadeGroup.Toggle();
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0002FAAA File Offset: 0x0002DCAA
	private void OnButtonClicked(int index)
	{
		this.menuFadeGroup.Hide();
		SavesSystem.SetFile(index);
		this.restorePanel.Open(index);
	}

	// Token: 0x0400098C RID: 2444
	[SerializeField]
	private Button mainButton;

	// Token: 0x0400098D RID: 2445
	[SerializeField]
	private FadeGroup menuFadeGroup;

	// Token: 0x0400098E RID: 2446
	[SerializeField]
	private Button buttonSlot1;

	// Token: 0x0400098F RID: 2447
	[SerializeField]
	private Button buttonSlot2;

	// Token: 0x04000990 RID: 2448
	[SerializeField]
	private Button buttonSlot3;

	// Token: 0x04000991 RID: 2449
	[SerializeField]
	private SavesBackupRestorePanel restorePanel;
}
