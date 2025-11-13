using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class MultiInteraction : MonoBehaviour
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0003C0C6 File Offset: 0x0003A2C6
	public ReadOnlyCollection<InteractableBase> Interactables
	{
		get
		{
			return this.interactables.AsReadOnly();
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0003C0D3 File Offset: 0x0003A2D3
	private void OnTriggerEnter(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetupAndShow(this).Forget();
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0003C101 File Offset: 0x0003A301
	private void OnTriggerExit(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Hide().Forget();
		}
	}

	// Token: 0x04000C66 RID: 3174
	[SerializeField]
	private List<InteractableBase> interactables;
}
