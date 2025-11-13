using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000299 RID: 665
	public class HookHead : MonoBehaviour
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x00051F2A File Offset: 0x0005012A
		private void OnCollisionEnter2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionEnter;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00051F3E File Offset: 0x0005013E
		private void OnCollisionExit2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionExit;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00051F52 File Offset: 0x00050152
		private void OnCollisionStay2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionStay;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x04001039 RID: 4153
		public Action<HookHead, Collision2D> onCollisionEnter;

		// Token: 0x0400103A RID: 4154
		public Action<HookHead, Collision2D> onCollisionExit;

		// Token: 0x0400103B RID: 4155
		public Action<HookHead, Collision2D> onCollisionStay;
	}
}
