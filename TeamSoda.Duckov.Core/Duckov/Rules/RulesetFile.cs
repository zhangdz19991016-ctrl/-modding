using System;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003FA RID: 1018
	[CreateAssetMenu(menuName = "Duckov/Ruleset")]
	public class RulesetFile : ScriptableObject
	{
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x0008072A File Offset: 0x0007E92A
		public Ruleset Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04001915 RID: 6421
		[SerializeField]
		private Ruleset data;
	}
}
