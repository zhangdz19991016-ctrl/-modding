using System;
using Duckov.UI;

namespace Duckov.PerkTrees.Interactable
{
	// Token: 0x02000260 RID: 608
	public class PerkTreeUIInvoker : InteractableBase
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x00047F30 File Offset: 0x00046130
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00047F33 File Offset: 0x00046133
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			PerkTreeView.Show(PerkTreeManager.GetPerkTree(this.perkTreeID));
			base.StopInteract();
		}

		// Token: 0x04000E53 RID: 3667
		public string perkTreeID;
	}
}
