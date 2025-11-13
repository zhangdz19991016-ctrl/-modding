using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000368 RID: 872
	public class RequireDemo : Condition
	{
		// Token: 0x06001EA3 RID: 7843 RVA: 0x0006C4FF File Offset: 0x0006A6FF
		public override bool Evaluate()
		{
			if (this.inverse)
			{
				return !GameMetaData.Instance.IsDemo;
			}
			return GameMetaData.Instance.IsDemo;
		}

		// Token: 0x040014DC RID: 5340
		[SerializeField]
		private bool inverse;
	}
}
