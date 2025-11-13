using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200036A RID: 874
	public class RequireGameobjectsActived : Condition
	{
		// Token: 0x06001EA7 RID: 7847 RVA: 0x0006C540 File Offset: 0x0006A740
		public override bool Evaluate()
		{
			foreach (GameObject gameObject in this.targets)
			{
				if (gameObject == null || !gameObject.activeInHierarchy)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040014E0 RID: 5344
		[SerializeField]
		private GameObject[] targets;
	}
}
