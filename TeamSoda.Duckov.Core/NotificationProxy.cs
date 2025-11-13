using System;
using Duckov.UI;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class NotificationProxy : MonoBehaviour
{
	// Token: 0x0600064F RID: 1615 RVA: 0x0001C7EF File Offset: 0x0001A9EF
	public void Notify()
	{
		NotificationText.Push(this.notification.ToPlainText());
	}

	// Token: 0x04000607 RID: 1543
	[LocalizationKey("Default")]
	public string notification;
}
