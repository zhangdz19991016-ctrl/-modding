using System;
using Duckvo.Beacons;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000340 RID: 832
	public class QuestTask_UnlockBeacon : Task
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x00068427 File Offset: 0x00066627
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00068439 File Offset: 0x00066639
		[LocalizationKey("Default")]
		private string DescriptionKey
		{
			get
			{
				return "Task_Beacon_" + this.beaconID;
			}
			set
			{
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0006843B File Offset: 0x0006663B
		public override string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x00068448 File Offset: 0x00066648
		public override object GenerateSaveData()
		{
			return 0;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x00068450 File Offset: 0x00066650
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x00068452 File Offset: 0x00066652
		protected override bool CheckFinished()
		{
			return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
		}

		// Token: 0x04001409 RID: 5129
		[SerializeField]
		private string beaconID;

		// Token: 0x0400140A RID: 5130
		[SerializeField]
		private int beaconIndex;
	}
}
