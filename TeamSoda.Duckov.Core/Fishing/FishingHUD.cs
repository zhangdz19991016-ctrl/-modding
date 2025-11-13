using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
	// Token: 0x02000218 RID: 536
	public class FishingHUD : MonoBehaviour
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x0003F3E5 File Offset: 0x0003D5E5
		private void Awake()
		{
			Action_Fishing.OnPlayerStartCatching += this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003F41A File Offset: 0x0003D61A
		private void OnDestroy()
		{
			Action_Fishing.OnPlayerStartCatching -= this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003F44F File Offset: 0x0003D64F
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0003F45C File Offset: 0x0003D65C
		private void OnStopCatching(Action_Fishing fishing, Item item, Action<bool> action)
		{
			this.StopCatchingTask(item, action).Forget();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0003F46B File Offset: 0x0003D66B
		private void OnStartCatching(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			this.CatchingTask(fishing, totalTime, currentTimeGetter).Forget();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0003F47C File Offset: 0x0003D67C
		private UniTask CatchingTask(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			FishingHUD.<CatchingTask>d__9 <CatchingTask>d__;
			<CatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CatchingTask>d__.<>4__this = this;
			<CatchingTask>d__.fishing = fishing;
			<CatchingTask>d__.totalTime = totalTime;
			<CatchingTask>d__.currentTimeGetter = currentTimeGetter;
			<CatchingTask>d__.<>1__state = -1;
			<CatchingTask>d__.<>t__builder.Start<FishingHUD.<CatchingTask>d__9>(ref <CatchingTask>d__);
			return <CatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		private void UpdateBar(float totalTime, float currentTime)
		{
			if (totalTime <= 0f)
			{
				return;
			}
			float fillAmount = 1f - currentTime / totalTime;
			this.countDownFill.fillAmount = fillAmount;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003F504 File Offset: 0x0003D704
		private UniTask StopCatchingTask(Item item, Action<bool> confirmCallback)
		{
			FishingHUD.<StopCatchingTask>d__11 <StopCatchingTask>d__;
			<StopCatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<StopCatchingTask>d__.<>4__this = this;
			<StopCatchingTask>d__.item = item;
			<StopCatchingTask>d__.<>1__state = -1;
			<StopCatchingTask>d__.<>t__builder.Start<FishingHUD.<StopCatchingTask>d__11>(ref <StopCatchingTask>d__);
			return <StopCatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000CEA RID: 3306
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CEB RID: 3307
		[SerializeField]
		private Image countDownFill;

		// Token: 0x04000CEC RID: 3308
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04000CED RID: 3309
		[SerializeField]
		private FadeGroup failIndicator;
	}
}
