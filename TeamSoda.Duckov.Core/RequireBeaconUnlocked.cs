using System;
using Duckov.Quests;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class RequireBeaconUnlocked : Condition
{
	// Token: 0x06000994 RID: 2452 RVA: 0x0002A2E4 File Offset: 0x000284E4
	public override bool Evaluate()
	{
		return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
	}

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	[SceneID]
	private string beaconID;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private int beaconIndex;
}
