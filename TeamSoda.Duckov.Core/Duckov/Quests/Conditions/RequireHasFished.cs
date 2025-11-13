using System;
using Saves;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200036B RID: 875
	public class RequireHasFished : Condition
	{
		// Token: 0x06001EA9 RID: 7849 RVA: 0x0006C582 File Offset: 0x0006A782
		public override bool Evaluate()
		{
			return RequireHasFished.GetHasFished();
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0006C589 File Offset: 0x0006A789
		public static void SetHasFished()
		{
			SavesSystem.Save<bool>("HasFished", true);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0006C596 File Offset: 0x0006A796
		public static bool GetHasFished()
		{
			return SavesSystem.Load<bool>("HasFished");
		}
	}
}
