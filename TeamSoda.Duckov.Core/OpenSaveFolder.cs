using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001C7 RID: 455
public class OpenSaveFolder : MonoBehaviour
{
	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00038AA5 File Offset: 0x00036CA5
	private string filePath
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00038AAC File Offset: 0x00036CAC
	private void Update()
	{
		if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.lKey.isPressed)
		{
			this.OpenFolder();
		}
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00038AD6 File Offset: 0x00036CD6
	public void OpenFolder()
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = this.filePath,
			UseShellExecute = true
		});
	}
}
