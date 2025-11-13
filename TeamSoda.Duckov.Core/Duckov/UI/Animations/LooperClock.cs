using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E2 RID: 994
	public class LooperClock : MonoBehaviour
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x0007EDD9 File Offset: 0x0007CFD9
		public float t
		{
			get
			{
				if (this.duration > 0f)
				{
					return this.time / this.duration;
				}
				return 1f;
			}
		}

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x06002448 RID: 9288 RVA: 0x0007EDFC File Offset: 0x0007CFFC
		// (remove) Token: 0x06002449 RID: 9289 RVA: 0x0007EE34 File Offset: 0x0007D034
		public event Action<LooperClock, float> onTick;

		// Token: 0x0600244A RID: 9290 RVA: 0x0007EE69 File Offset: 0x0007D069
		private void Update()
		{
			if (this.duration > 0f)
			{
				this.time += Time.unscaledDeltaTime;
				this.time %= this.duration;
				this.Tick();
			}
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0007EEA3 File Offset: 0x0007D0A3
		private void Tick()
		{
			Action<LooperClock, float> action = this.onTick;
			if (action == null)
			{
				return;
			}
			action(this, this.t);
		}

		// Token: 0x04001894 RID: 6292
		[SerializeField]
		private float duration = 1f;

		// Token: 0x04001895 RID: 6293
		private float time;
	}
}
