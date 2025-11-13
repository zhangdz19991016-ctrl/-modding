using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class PlayerPositionBackupProxy : MonoBehaviour
{
	// Token: 0x06000949 RID: 2377 RVA: 0x00029932 File Offset: 0x00027B32
	public void StartRecoverInteract()
	{
		PauseMenu.Instance.Close();
		PlayerPositionBackupManager.StartRecover();
	}
}
