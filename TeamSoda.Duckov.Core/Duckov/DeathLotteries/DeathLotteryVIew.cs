using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x0200030B RID: 779
	public class DeathLotteryVIew : View
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x0005CF85 File Offset: 0x0005B185
		private string RemainingTextFormat
		{
			get
			{
				return this.remainingTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x0005CF92 File Offset: 0x0005B192
		public DeathLottery Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001979 RID: 6521 RVA: 0x0005CF9A File Offset: 0x0005B19A
		public int RemainingChances
		{
			get
			{
				if (this.Target == null)
				{
					return 0;
				}
				return this.Target.RemainingChances;
			}
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0005CFB7 File Offset: 0x0005B1B7
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.selectionBusyIndicator.SkipHide();
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0005CFD5 File Offset: 0x0005B1D5
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0005CFE8 File Offset: 0x0005B1E8
		protected override void Awake()
		{
			base.Awake();
			DeathLottery.OnRequestUI += this.Show;
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0005D001 File Offset: 0x0005B201
		protected override void OnDestroy()
		{
			base.OnDestroy();
			DeathLottery.OnRequestUI -= this.Show;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0005D01A File Offset: 0x0005B21A
		private void Show(DeathLottery target)
		{
			this.target = target;
			this.Setup();
			base.Open(null);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0005D030 File Offset: 0x0005B230
		private void RefreshTexts()
		{
			this.remainingCountText.text = ((this.RemainingChances > 0) ? this.RemainingTextFormat.Format(new
			{
				amount = this.RemainingChances
			}) : this.noRemainingChances.ToPlainText());
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0005D06C File Offset: 0x0005B26C
		private void Setup()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.Loading)
			{
				return;
			}
			DeathLottery.Status currentStatus = this.target.CurrentStatus;
			if (!currentStatus.valid)
			{
				return;
			}
			for (int i = 0; i < currentStatus.candidates.Count; i++)
			{
				this.cards[i].Setup(this, i);
			}
			this.RefreshTexts();
			this.HandleRemaining();
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0005D0DC File Offset: 0x0005B2DC
		internal void NotifyEntryClicked(DeathLotteryCard deathLotteryCard, Cost cost)
		{
			if (deathLotteryCard == null)
			{
				return;
			}
			if (this.ProcessingSelection)
			{
				return;
			}
			if (this.RemainingChances <= 0)
			{
				return;
			}
			int index = deathLotteryCard.Index;
			if (this.target.CurrentStatus.selectedItems.Contains(index))
			{
				return;
			}
			this.selectTask = this.SelectTask(index, cost);
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0005D134 File Offset: 0x0005B334
		private bool ProcessingSelection
		{
			get
			{
				return this.selectTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0005D144 File Offset: 0x0005B344
		private UniTask SelectTask(int index, Cost cost)
		{
			DeathLotteryVIew.<SelectTask>d__24 <SelectTask>d__;
			<SelectTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectTask>d__.<>4__this = this;
			<SelectTask>d__.index = index;
			<SelectTask>d__.cost = cost;
			<SelectTask>d__.<>1__state = -1;
			<SelectTask>d__.<>t__builder.Start<DeathLotteryVIew.<SelectTask>d__24>(ref <SelectTask>d__);
			return <SelectTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0005D198 File Offset: 0x0005B398
		private void HandleRemaining()
		{
			if (this.RemainingChances > 0)
			{
				return;
			}
			DeathLotteryCard[] array = this.cards;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].NotifyFacing(true);
			}
		}

		// Token: 0x04001275 RID: 4725
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001276 RID: 4726
		[LocalizationKey("Default")]
		[SerializeField]
		private string remainingTextFormatKey = "DeathLottery_Remaining";

		// Token: 0x04001277 RID: 4727
		[LocalizationKey("Default")]
		[SerializeField]
		private string noRemainingChances = "DeathLottery_NoRemainingChances";

		// Token: 0x04001278 RID: 4728
		[SerializeField]
		private TextMeshProUGUI remainingCountText;

		// Token: 0x04001279 RID: 4729
		[SerializeField]
		private DeathLotteryCard[] cards;

		// Token: 0x0400127A RID: 4730
		[SerializeField]
		private FadeGroup selectionBusyIndicator;

		// Token: 0x0400127B RID: 4731
		private DeathLottery target;

		// Token: 0x0400127C RID: 4732
		private UniTask selectTask;
	}
}
