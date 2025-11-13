using System;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class TeleportBeacon : MonoBehaviour
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x0001AC90 File Offset: 0x00018E90
	private void Start()
	{
		bool beaconUnlocked = BeaconManager.GetBeaconUnlocked(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(beaconUnlocked);
		this.interactable.gameObject.SetActive(!beaconUnlocked);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001ACCF File Offset: 0x00018ECF
	public void ActivateBeacon()
	{
		BeaconManager.UnlockBeacon(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(true);
		this.interactable.gameObject.SetActive(false);
	}

	// Token: 0x04000582 RID: 1410
	[SceneID]
	public string beaconScene;

	// Token: 0x04000583 RID: 1411
	public int beaconIndex;

	// Token: 0x04000584 RID: 1412
	public GameObject activeByUnlocked;

	// Token: 0x04000585 RID: 1413
	public InteractableBase interactable;
}
