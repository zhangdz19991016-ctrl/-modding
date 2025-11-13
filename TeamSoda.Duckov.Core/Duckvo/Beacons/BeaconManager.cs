using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using UnityEngine;

namespace Duckvo.Beacons
{
	// Token: 0x02000224 RID: 548
	public class BeaconManager : MonoBehaviour
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x00040689 File Offset: 0x0003E889
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x00040690 File Offset: 0x0003E890
		public static BeaconManager Instance { get; private set; }

		// Token: 0x0600108A RID: 4234 RVA: 0x00040698 File Offset: 0x0003E898
		private void Awake()
		{
			BeaconManager.Instance = this;
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x000406B7 File Offset: 0x0003E8B7
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x000406CA File Offset: 0x0003E8CA
		public void Load()
		{
			if (SavesSystem.KeyExisits("BeaconManager"))
			{
				this.data = SavesSystem.Load<BeaconManager.Data>("BeaconManager");
			}
			if (this.data.entries == null)
			{
				this.data.entries = new List<BeaconManager.BeaconStatus>();
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00040705 File Offset: 0x0003E905
		public void Save()
		{
			SavesSystem.Save<BeaconManager.Data>("BeaconManager", this.data);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00040718 File Offset: 0x0003E918
		public static void UnlockBeacon(string id, int index)
		{
			if (BeaconManager.Instance == null)
			{
				return;
			}
			if (BeaconManager.GetBeaconUnlocked(id, index))
			{
				return;
			}
			BeaconManager.Instance.data.entries.Add(new BeaconManager.BeaconStatus
			{
				beaconID = id,
				beaconIndex = index
			});
			Action<string, int> onBeaconUnlocked = BeaconManager.OnBeaconUnlocked;
			if (onBeaconUnlocked == null)
			{
				return;
			}
			onBeaconUnlocked(id, index);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004077C File Offset: 0x0003E97C
		public static bool GetBeaconUnlocked(string id, int index)
		{
			return !(BeaconManager.Instance == null) && BeaconManager.Instance.data.entries.Any((BeaconManager.BeaconStatus e) => e.beaconID == id && e.beaconIndex == index);
		}

		// Token: 0x04000D35 RID: 3381
		private BeaconManager.Data data;

		// Token: 0x04000D36 RID: 3382
		public static Action<string, int> OnBeaconUnlocked;

		// Token: 0x04000D37 RID: 3383
		private const string SaveKey = "BeaconManager";

		// Token: 0x0200050C RID: 1292
		[Serializable]
		public struct BeaconStatus
		{
			// Token: 0x04001DFF RID: 7679
			public string beaconID;

			// Token: 0x04001E00 RID: 7680
			public int beaconIndex;
		}

		// Token: 0x0200050D RID: 1293
		[Serializable]
		public struct Data
		{
			// Token: 0x04001E01 RID: 7681
			public List<BeaconManager.BeaconStatus> entries;
		}
	}
}
