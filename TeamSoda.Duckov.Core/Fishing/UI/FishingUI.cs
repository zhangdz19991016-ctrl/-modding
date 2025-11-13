using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing.UI
{
	// Token: 0x0200021C RID: 540
	public class FishingUI : View
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x0003FA80 File Offset: 0x0003DC80
		protected override void Awake()
		{
			base.Awake();
			Action_Fishing.OnPlayerStartSelectBait += this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0003FABB File Offset: 0x0003DCBB
		protected override void OnDestroy()
		{
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
			Action_Fishing.OnPlayerStartSelectBait -= this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			base.OnDestroy();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003FAF6 File Offset: 0x0003DCF6
		internal override void TryQuit()
		{
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0003FAF8 File Offset: 0x0003DCF8
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			Debug.Log("Open Fishing Panel");
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0003FB15 File Offset: 0x0003DD15
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0003FB28 File Offset: 0x0003DD28
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.baitSelectPanel.NotifyStop();
			this.confirmPanel.NotifyStop();
			base.Close();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003FB46 File Offset: 0x0003DD46
		private void OnStartSelectBait(Action_Fishing fishing, ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			this.SelectBaitTask(availableBaits, baitSelectionResultCallback).Forget();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003FB58 File Offset: 0x0003DD58
		private UniTask SelectBaitTask(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			FishingUI.<SelectBaitTask>d__10 <SelectBaitTask>d__;
			<SelectBaitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectBaitTask>d__.<>4__this = this;
			<SelectBaitTask>d__.availableBaits = availableBaits;
			<SelectBaitTask>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<SelectBaitTask>d__.<>1__state = -1;
			<SelectBaitTask>d__.<>t__builder.Start<FishingUI.<SelectBaitTask>d__10>(ref <SelectBaitTask>d__);
			return <SelectBaitTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003FBAB File Offset: 0x0003DDAB
		private void OnStopCatching(Action_Fishing fishing, Item catchedItem, Action<bool> confirmCallback)
		{
			this.ConfirmTask(catchedItem, confirmCallback).Forget();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003FBBC File Offset: 0x0003DDBC
		private UniTask ConfirmTask(Item catchedItem, Action<bool> confirmCallback)
		{
			FishingUI.<ConfirmTask>d__12 <ConfirmTask>d__;
			<ConfirmTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ConfirmTask>d__.<>4__this = this;
			<ConfirmTask>d__.catchedItem = catchedItem;
			<ConfirmTask>d__.confirmCallback = confirmCallback;
			<ConfirmTask>d__.<>1__state = -1;
			<ConfirmTask>d__.<>t__builder.Start<FishingUI.<ConfirmTask>d__12>(ref <ConfirmTask>d__);
			return <ConfirmTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000D06 RID: 3334
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000D07 RID: 3335
		[SerializeField]
		private BaitSelectPanel baitSelectPanel;

		// Token: 0x04000D08 RID: 3336
		[SerializeField]
		private ConfirmPanel confirmPanel;
	}
}
