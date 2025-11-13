using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038E RID: 910
	public class SleepView : View
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x0006F651 File Offset: 0x0006D851
		public static SleepView Instance
		{
			get
			{
				return View.GetViewInstance<SleepView>();
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0006F658 File Offset: 0x0006D858
		private TimeSpan SleepTimeSpan
		{
			get
			{
				return TimeSpan.FromMinutes((double)this.sleepForMinuts);
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x0006F666 File Offset: 0x0006D866
		private TimeSpan WillWakeUpAt
		{
			get
			{
				return GameClock.TimeOfDay + this.SleepTimeSpan;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x0006F678 File Offset: 0x0006D878
		private bool WillWakeUpNextDay
		{
			get
			{
				return this.WillWakeUpAt.Days > 0;
			}
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0006F696 File Offset: 0x0006D896
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0006F6A9 File Offset: 0x0006D8A9
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0006F6BC File Offset: 0x0006D8BC
		protected override void Awake()
		{
			base.Awake();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0006F6FC File Offset: 0x0006D8FC
		private void OnConfirmButtonClicked()
		{
			this.Sleep((float)this.sleepForMinuts).Forget();
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0006F710 File Offset: 0x0006D910
		private UniTask Sleep(float minuts)
		{
			SleepView.<Sleep>d__21 <Sleep>d__;
			<Sleep>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sleep>d__.<>4__this = this;
			<Sleep>d__.minuts = minuts;
			<Sleep>d__.<>1__state = -1;
			<Sleep>d__.<>t__builder.Start<SleepView.<Sleep>d__21>(ref <Sleep>d__);
			return <Sleep>d__.<>t__builder.Task;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0006F75B File Offset: 0x0006D95B
		private void OnGameClockStep()
		{
			this.Refresh();
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0006F763 File Offset: 0x0006D963
		private void OnEnable()
		{
			this.InitializeUI();
			GameClock.OnGameClockStep += this.OnGameClockStep;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0006F77C File Offset: 0x0006D97C
		private void OnDisable()
		{
			GameClock.OnGameClockStep -= this.OnGameClockStep;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0006F78F File Offset: 0x0006D98F
		private void OnSliderValueChanged(float newValue)
		{
			this.sleepForMinuts = Mathf.RoundToInt(newValue);
			this.Refresh();
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0006F7A3 File Offset: 0x0006D9A3
		private void InitializeUI()
		{
			this.slider.SetValueWithoutNotify((float)this.sleepForMinuts);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0006F7B7 File Offset: 0x0006D9B7
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0006F7C0 File Offset: 0x0006D9C0
		private void Refresh()
		{
			TimeSpan willWakeUpAt = this.WillWakeUpAt;
			this.willWakeUpAtText.text = string.Format("{0:00}:{1:00}", willWakeUpAt.Hours, willWakeUpAt.Minutes);
			TimeSpan sleepTimeSpan = this.SleepTimeSpan;
			this.sleepTimeSpanText.text = string.Format("{0:00} h {1:00} min", (int)sleepTimeSpan.TotalHours, sleepTimeSpan.Minutes);
			this.nextDayIndicator.gameObject.SetActive(willWakeUpAt.Days > 0);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0006F850 File Offset: 0x0006DA50
		public static void Show()
		{
			if (SleepView.Instance == null)
			{
				return;
			}
			SleepView.Instance.Open(null);
		}

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400159D RID: 5533
		[SerializeField]
		private Slider slider;

		// Token: 0x0400159E RID: 5534
		[SerializeField]
		private TextMeshProUGUI willWakeUpAtText;

		// Token: 0x0400159F RID: 5535
		[SerializeField]
		private TextMeshProUGUI sleepTimeSpanText;

		// Token: 0x040015A0 RID: 5536
		[SerializeField]
		private GameObject nextDayIndicator;

		// Token: 0x040015A1 RID: 5537
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040015A2 RID: 5538
		private int sleepForMinuts;

		// Token: 0x040015A3 RID: 5539
		public static Action OnAfterSleep;

		// Token: 0x040015A4 RID: 5540
		private bool sleeping;
	}
}
