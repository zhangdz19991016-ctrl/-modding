using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.Sounds
{
	// Token: 0x0200024C RID: 588
	public class SoundDisplay : MonoBehaviour
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x000466FC File Offset: 0x000448FC
		public float Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00046704 File Offset: 0x00044904
		public AISound CurrentSount
		{
			get
			{
				return this.sound;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0004670C File Offset: 0x0004490C
		internal void Trigger(AISound sound)
		{
			this.sound = sound;
			base.gameObject.SetActive(true);
			this.velocity = this.triggerVelocity;
			this.value += this.velocity * Time.deltaTime;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00046748 File Offset: 0x00044948
		private void Update()
		{
			this.velocity -= this.gravity * Time.deltaTime;
			this.value += this.velocity * Time.deltaTime;
			if (this.value > 1f || this.value < 0f)
			{
				this.velocity = 0f;
			}
			this.value = Mathf.Clamp01(this.value);
			this.image.color = new Color(1f, 1f, 1f, this.value);
		}

		// Token: 0x04000E1A RID: 3610
		[SerializeField]
		private Image image;

		// Token: 0x04000E1B RID: 3611
		[SerializeField]
		private float removeRecordAfterTime = 1f;

		// Token: 0x04000E1C RID: 3612
		[SerializeField]
		private float triggerVelocity = 10f;

		// Token: 0x04000E1D RID: 3613
		[SerializeField]
		private float gravity = 1f;

		// Token: 0x04000E1E RID: 3614
		[SerializeField]
		private float untriggerVelocity = 100f;

		// Token: 0x04000E1F RID: 3615
		private float value;

		// Token: 0x04000E20 RID: 3616
		private float velocity;

		// Token: 0x04000E21 RID: 3617
		private AISound sound;
	}
}
