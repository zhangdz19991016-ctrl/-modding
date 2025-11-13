using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class TimeOfDayAlertTriggerProxy : MonoBehaviour
{
	// Token: 0x0600067C RID: 1660 RVA: 0x0001D72B File Offset: 0x0001B92B
	public void OnEnter()
	{
		TimeOfDayAlert.EnterAlertTrigger();
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0001D732 File Offset: 0x0001B932
	public void OnLeave()
	{
		TimeOfDayAlert.LeaveAlertTrigger();
	}
}
