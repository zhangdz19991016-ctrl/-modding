using System;
using System.Collections.Generic;
using Duckov.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x02000313 RID: 787
	public class Basket : MonoBehaviour
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x0005EADA File Offset: 0x0005CCDA
		private void Awake()
		{
			this.trigger.onGoal.AddListener(new UnityAction<BasketBall>(this.OnGoal));
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0005EAF8 File Offset: 0x0005CCF8
		private void OnGoal(BasketBall ball)
		{
			if (!this.conditions.Satisfied())
			{
				return;
			}
			this.onGoal.Invoke(ball);
			this.netAnimator.SetTrigger("Goal");
		}

		// Token: 0x040012D0 RID: 4816
		[SerializeField]
		private Animator netAnimator;

		// Token: 0x040012D1 RID: 4817
		[SerializeField]
		private List<Condition> conditions = new List<Condition>();

		// Token: 0x040012D2 RID: 4818
		[SerializeField]
		private BasketTrigger trigger;

		// Token: 0x040012D3 RID: 4819
		public UnityEvent<BasketBall> onGoal;
	}
}
