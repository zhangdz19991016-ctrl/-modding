using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x02000315 RID: 789
	public class BasketTrigger : MonoBehaviour
	{
		// Token: 0x06001A1D RID: 6685 RVA: 0x0005EB40 File Offset: 0x0005CD40
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("ONTRIGGERENTER:" + other.name);
			BasketBall component = other.GetComponent<BasketBall>();
			if (component == null)
			{
				return;
			}
			this.onGoal.Invoke(component);
		}

		// Token: 0x040012D4 RID: 4820
		public UnityEvent<BasketBall> onGoal;
	}
}
