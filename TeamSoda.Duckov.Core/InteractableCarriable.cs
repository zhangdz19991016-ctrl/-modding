using System;

// Token: 0x020000DB RID: 219
public class InteractableCarriable : InteractableBase
{
	// Token: 0x060006F6 RID: 1782 RVA: 0x0001F820 File Offset: 0x0001DA20
	protected override void Start()
	{
		base.Start();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0001F82F File Offset: 0x0001DA2F
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0001F832 File Offset: 0x0001DA32
	protected override void OnInteractStart(CharacterMainControl character)
	{
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0001F834 File Offset: 0x0001DA34
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		CharacterMainControl interactCharacter = this.interactCharacter;
		base.StopInteract();
		interactCharacter.Carry(this.carryTarget);
	}

	// Token: 0x040006B0 RID: 1712
	public Carriable carryTarget;
}
