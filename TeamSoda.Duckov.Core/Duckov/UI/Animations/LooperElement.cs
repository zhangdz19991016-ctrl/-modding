using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E3 RID: 995
	public abstract class LooperElement : MonoBehaviour
	{
		// Token: 0x0600244D RID: 9293 RVA: 0x0007EECF File Offset: 0x0007D0CF
		protected virtual void OnEnable()
		{
			this.clock.onTick += this.OnTick;
			this.OnTick(this.clock, this.clock.t);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0007EF00 File Offset: 0x0007D100
		protected virtual void OnDisable()
		{
			if (this.clock != null)
			{
				this.clock.onTick -= this.OnTick;
			}
		}

		// Token: 0x0600244F RID: 9295
		protected abstract void OnTick(LooperClock clock, float t);

		// Token: 0x04001897 RID: 6295
		[SerializeField]
		private LooperClock clock;
	}
}
