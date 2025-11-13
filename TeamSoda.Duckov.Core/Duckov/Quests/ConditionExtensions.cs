using System;
using System.Collections.Generic;

namespace Duckov.Quests
{
	// Token: 0x02000337 RID: 823
	public static class ConditionExtensions
	{
		// Token: 0x06001C0A RID: 7178 RVA: 0x0006611C File Offset: 0x0006431C
		public static bool Satisfied(this IEnumerable<Condition> conditions)
		{
			foreach (Condition condition in conditions)
			{
				if (!(condition == null) && !condition.Evaluate())
				{
					return false;
				}
			}
			return true;
		}
	}
}
