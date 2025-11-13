using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000025 RID: 37
	public abstract class UsageBehavior : MonoBehaviour
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007EB0 File Offset: 0x000060B0
		public virtual UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return default(UsageBehavior.DisplaySettingsData);
			}
		}

		// Token: 0x060001FB RID: 507
		public abstract bool CanBeUsed(Item item, object user);

		// Token: 0x060001FC RID: 508
		protected abstract void OnUse(Item item, object user);

		// Token: 0x060001FD RID: 509 RVA: 0x00007EC6 File Offset: 0x000060C6
		public void Use(Item item, object user)
		{
			this.OnUse(item, user);
		}

		// Token: 0x0200004A RID: 74
		public struct DisplaySettingsData
		{
			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x0600028C RID: 652 RVA: 0x000097CF File Offset: 0x000079CF
			public string Description
			{
				get
				{
					return this.description;
				}
			}

			// Token: 0x04000126 RID: 294
			public bool display;

			// Token: 0x04000127 RID: 295
			public string description;
		}
	}
}
