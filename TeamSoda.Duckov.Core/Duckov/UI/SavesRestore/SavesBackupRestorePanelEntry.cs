using System;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x020003F1 RID: 1009
	public class SavesBackupRestorePanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x0007FBCF File Offset: 0x0007DDCF
		public SavesSystem.BackupInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0007FBD7 File Offset: 0x0007DDD7
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.NotifyClicked(this);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0007FBE8 File Offset: 0x0007DDE8
		internal void Setup(SavesBackupRestorePanel master, SavesSystem.BackupInfo info)
		{
			this.master = master;
			this.info = info;
			if (info.time_raw <= 0L)
			{
				this.timeText.text = "???";
				return;
			}
			this.timeText.text = info.Time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
		}

		// Token: 0x040018D7 RID: 6359
		[SerializeField]
		private TextMeshProUGUI timeText;

		// Token: 0x040018D8 RID: 6360
		private SavesBackupRestorePanel master;

		// Token: 0x040018D9 RID: 6361
		private SavesSystem.BackupInfo info;
	}
}
