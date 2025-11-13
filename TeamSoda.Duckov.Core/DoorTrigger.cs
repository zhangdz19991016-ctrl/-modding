using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class DoorTrigger : MonoBehaviour
{
	// Token: 0x060006CB RID: 1739 RVA: 0x0001EA48 File Offset: 0x0001CC48
	private void OnTriggerEnter(Collider collision)
	{
		if (this.parent.IsOpen)
		{
			return;
		}
		if (!this.parent.NoRequireItem)
		{
			return;
		}
		if (this.parent.Interact && !this.parent.Interact.gameObject.activeInHierarchy)
		{
			return;
		}
		if (collision.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		CharacterMainControl component = collision.gameObject.GetComponent<CharacterMainControl>();
		if (!component || component.Team == Teams.player)
		{
			return;
		}
		this.parent.Open();
	}

	// Token: 0x0400068D RID: 1677
	public Door parent;
}
