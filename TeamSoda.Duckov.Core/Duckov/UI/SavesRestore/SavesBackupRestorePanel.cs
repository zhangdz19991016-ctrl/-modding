using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI.SavesRestore
{
	// Token: 0x020003F0 RID: 1008
	public class SavesBackupRestorePanel : MonoBehaviour
	{
		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x0007FA38 File Offset: 0x0007DC38
		private PrefabPool<SavesBackupRestorePanelEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<SavesBackupRestorePanelEntry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0007FA71 File Offset: 0x0007DC71
		private void Awake()
		{
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0007FA73 File Offset: 0x0007DC73
		public void Open(int savesSlot)
		{
			this.slot = savesSlot;
			this.Refresh();
			this.fadeGroup.Show();
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x0007FA8D File Offset: 0x0007DC8D
		public void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0007FA9A File Offset: 0x0007DC9A
		public void Confirm()
		{
			this.confirm = true;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0007FAA3 File Offset: 0x0007DCA3
		public void Cancel()
		{
			this.cancel = true;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0007FAAC File Offset: 0x0007DCAC
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			List<SavesSystem.BackupInfo> list = SavesSystem.GetBackupList(this.slot).ToList<SavesSystem.BackupInfo>();
			list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
			{
				if (a.Time < b.Time)
				{
					return 1;
				}
				return -1;
			});
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				SavesSystem.BackupInfo backupInfo = list[i];
				if (backupInfo.exists)
				{
					this.Pool.Get(null).Setup(this, backupInfo);
					num++;
				}
			}
			this.noBackupIndicator.SetActive(num <= 0);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0007FB48 File Offset: 0x0007DD48
		internal void NotifyClicked(SavesBackupRestorePanelEntry button)
		{
			if (this.recovering)
			{
				return;
			}
			SavesSystem.BackupInfo info = button.Info;
			if (!info.exists)
			{
				return;
			}
			this.RecoverTask(info).Forget();
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0007FB7C File Offset: 0x0007DD7C
		private UniTask RecoverTask(SavesSystem.BackupInfo info)
		{
			SavesBackupRestorePanel.<RecoverTask>d__21 <RecoverTask>d__;
			<RecoverTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RecoverTask>d__.<>4__this = this;
			<RecoverTask>d__.info = info;
			<RecoverTask>d__.<>1__state = -1;
			<RecoverTask>d__.<>t__builder.Start<SavesBackupRestorePanel.<RecoverTask>d__21>(ref <RecoverTask>d__);
			return <RecoverTask>d__.<>t__builder.Task;
		}

		// Token: 0x040018CB RID: 6347
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private FadeGroup confirmFadeGroup;

		// Token: 0x040018CD RID: 6349
		[SerializeField]
		private FadeGroup resultFadeGroup;

		// Token: 0x040018CE RID: 6350
		[SerializeField]
		private TextMeshProUGUI[] slotIndexTexts;

		// Token: 0x040018CF RID: 6351
		[SerializeField]
		private TextMeshProUGUI[] backupTimeTexts;

		// Token: 0x040018D0 RID: 6352
		[SerializeField]
		private SavesBackupRestorePanelEntry template;

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		private GameObject noBackupIndicator;

		// Token: 0x040018D2 RID: 6354
		private PrefabPool<SavesBackupRestorePanelEntry> _pool;

		// Token: 0x040018D3 RID: 6355
		private int slot;

		// Token: 0x040018D4 RID: 6356
		private bool recovering;

		// Token: 0x040018D5 RID: 6357
		private bool confirm;

		// Token: 0x040018D6 RID: 6358
		private bool cancel;
	}
}
