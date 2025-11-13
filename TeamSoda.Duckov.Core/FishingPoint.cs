using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A4 RID: 164
public class FishingPoint : MonoBehaviour
{
	// Token: 0x060005A7 RID: 1447 RVA: 0x00019763 File Offset: 0x00017963
	private void Awake()
	{
		this.OnPlayerTakeFishingRod(null);
		this.Interactable.OnInteractFinishedEvent.AddListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00019788 File Offset: 0x00017988
	private void OnDestroy()
	{
		if (this.Interactable)
		{
			this.Interactable.OnInteractFinishedEvent.RemoveListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x000197B3 File Offset: 0x000179B3
	private void OnPlayerTakeFishingRod(FishingRod rod)
	{
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x000197B8 File Offset: 0x000179B8
	private void OnInteractFinished(CharacterMainControl character, InteractableBase interact)
	{
		if (!character)
		{
			return;
		}
		character.SetPosition(this.playerPoint.position);
		character.SetAimPoint(this.playerPoint.position + this.playerPoint.forward * 10f);
		character.movementControl.SetAimDirection(this.playerPoint.forward);
		character.StartAction(this.action);
	}

	// Token: 0x0400051F RID: 1311
	public InteractableBase Interactable;

	// Token: 0x04000520 RID: 1312
	public Action_Fishing action;

	// Token: 0x04000521 RID: 1313
	public Transform playerPoint;
}
