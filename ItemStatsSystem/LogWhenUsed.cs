using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000024 RID: 36
	public class LogWhenUsed : UsageBehavior
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00007E96 File Offset: 0x00006096
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007E99 File Offset: 0x00006099
		protected override void OnUse(Item item, object user)
		{
			Debug.Log(item.name);
		}
	}
}
