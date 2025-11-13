using System;
using System.Linq;
using Sirenix.Utilities;

// Token: 0x020001AB RID: 427
public class InteractCrafter : InteractableBase
{
	// Token: 0x06000CAF RID: 3247 RVA: 0x00035C11 File Offset: 0x00033E11
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00035C20 File Offset: 0x00033E20
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		CraftView.SetupAndOpenView(new Predicate<CraftingFormula>(this.FilterCraft));
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00035C39 File Offset: 0x00033E39
	private bool FilterCraft(CraftingFormula formula)
	{
		return this.requireTag.IsNullOrWhitespace() || formula.tags.Contains(this.requireTag);
	}

	// Token: 0x04000B08 RID: 2824
	public string requireTag;
}
