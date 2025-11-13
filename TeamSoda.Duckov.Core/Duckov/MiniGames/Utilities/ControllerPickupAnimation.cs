using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x0200028B RID: 651
	public class ControllerPickupAnimation : MonoBehaviour
	{
		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x0004E3E0 File Offset: 0x0004C5E0
		private AnimationCurve pickupRotCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0004E3E8 File Offset: 0x0004C5E8
		private AnimationCurve pickupPosCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0004E3F0 File Offset: 0x0004C5F0
		private AnimationCurve putDownCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0004E3F8 File Offset: 0x0004C5F8
		public UniTask PickUp(Transform endTransform)
		{
			ControllerPickupAnimation.<PickUp>d__11 <PickUp>d__;
			<PickUp>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PickUp>d__.<>4__this = this;
			<PickUp>d__.endTransform = endTransform;
			<PickUp>d__.<>1__state = -1;
			<PickUp>d__.<>t__builder.Start<ControllerPickupAnimation.<PickUp>d__11>(ref <PickUp>d__);
			return <PickUp>d__.<>t__builder.Task;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0004E444 File Offset: 0x0004C644
		public UniTask PutDown()
		{
			ControllerPickupAnimation.<PutDown>d__12 <PutDown>d__;
			<PutDown>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PutDown>d__.<>4__this = this;
			<PutDown>d__.<>1__state = -1;
			<PutDown>d__.<>t__builder.Start<ControllerPickupAnimation.<PutDown>d__12>(ref <PutDown>d__);
			return <PutDown>d__.<>t__builder.Task;
		}

		// Token: 0x04000F5B RID: 3931
		[SerializeField]
		private Transform restTransform;

		// Token: 0x04000F5C RID: 3932
		[SerializeField]
		private Transform controllerTransform;

		// Token: 0x04000F5D RID: 3933
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000F5E RID: 3934
		[SerializeField]
		private AnimationCurve pickupCurve;

		// Token: 0x04000F5F RID: 3935
		private int activeToken;
	}
}
