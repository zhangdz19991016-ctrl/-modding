using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B8 RID: 952
	public class ClosureView : View
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x00077D03 File Offset: 0x00075F03
		public static ClosureView Instance
		{
			get
			{
				return View.GetViewInstance<ClosureView>();
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x00077D0A File Offset: 0x00075F0A
		private string EvacuatedTitleText
		{
			get
			{
				return this.evacuatedTitleTextKey.ToPlainText();
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x00077D17 File Offset: 0x00075F17
		private string FailedTitleText
		{
			get
			{
				return this.failedTitleTextKey.ToPlainText();
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00077D24 File Offset: 0x00075F24
		protected override void Awake()
		{
			base.Awake();
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00077D48 File Offset: 0x00075F48
		private void OnContinueButtonClicked()
		{
			if (!this.canContinue)
			{
				return;
			}
			this.continueButtonClicked = true;
			this.contentFadeGroup.Hide();
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00077D65 File Offset: 0x00075F65
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.contentFadeGroup.Show();
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00077D83 File Offset: 0x00075F83
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00077D98 File Offset: 0x00075F98
		public static UniTask ShowAndReturnTask(float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__36 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__36>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00077DDC File Offset: 0x00075FDC
		public static UniTask ShowAndReturnTask(DamageInfo dmgInfo, float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__37 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.dmgInfo = dmgInfo;
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__37>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00077E27 File Offset: 0x00076027
		private void SetupDamageInfo(DamageInfo dmgInfo)
		{
			this.damageSourceText.text = dmgInfo.GenerateDescription();
			this.damageInfoContainer.gameObject.SetActive(true);
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00077E4C File Offset: 0x0007604C
		private UniTask ClosureTask()
		{
			ClosureView.<ClosureTask>d__39 <ClosureTask>d__;
			<ClosureTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ClosureTask>d__.<>4__this = this;
			<ClosureTask>d__.<>1__state = -1;
			<ClosureTask>d__.<>t__builder.Start<ClosureView.<ClosureTask>d__39>(ref <ClosureTask>d__);
			return <ClosureTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00077E90 File Offset: 0x00076090
		private void SetupBeginning()
		{
			long cachedExp = EXPManager.CachedExp;
			long exp = EXPManager.EXP;
			this.Refresh(0f, cachedExp, exp);
			this.continueButton.gameObject.SetActive(false);
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00077EC8 File Offset: 0x000760C8
		private void SetupTitle(bool dead)
		{
			if (dead)
			{
				this.titleText.color = this.failedTitleTextColor;
				this.titleText.text = this.FailedTitleText;
				return;
			}
			this.titleText.color = this.evacuatedTitleTextColor;
			this.titleText.text = this.EvacuatedTitleText;
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00077F20 File Offset: 0x00076120
		private UniTask AnimateExpBar(long fromExp, long toExp)
		{
			ClosureView.<AnimateExpBar>d__42 <AnimateExpBar>d__;
			<AnimateExpBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateExpBar>d__.<>4__this = this;
			<AnimateExpBar>d__.fromExp = fromExp;
			<AnimateExpBar>d__.toExp = toExp;
			<AnimateExpBar>d__.<>1__state = -1;
			<AnimateExpBar>d__.<>t__builder.Start<ClosureView.<AnimateExpBar>d__42>(ref <AnimateExpBar>d__);
			return <AnimateExpBar>d__.<>t__builder.Task;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00077F74 File Offset: 0x00076174
		private void SpitExpUpSfx(float expDelta)
		{
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime - this.lastTimeExpUpSfxPlayed < 0.05f)
			{
				return;
			}
			this.lastTimeExpUpSfxPlayed = unscaledTime;
			AudioManager.SetRTPC("ExpDelta", expDelta, null);
			AudioManager.Post(this.sfx_ExpUp);
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00077FB8 File Offset: 0x000761B8
		private long Refresh(float t, long fromExp, long toExp)
		{
			long num = this.LongLerp(fromExp, toExp, t);
			this.SetExpDisplay(num, fromExp);
			this.SetLevelDisplay(this.cachedLevel);
			return num;
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00077FE4 File Offset: 0x000761E4
		private long LongLerp(long from, long to, float t)
		{
			return (long)((float)(to - from) * t) + from;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00077FF0 File Offset: 0x000761F0
		private void CacheLevelInfo(int level)
		{
			if (level == this.cachedLevel)
			{
				return;
			}
			this.cachedLevel = level;
			this.cachedLevelRange = EXPManager.Instance.GetLevelExpRange(level);
			this.cachedLevelLength = this.cachedLevelRange.Item2 - this.cachedLevelRange.Item1;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x0007803C File Offset: 0x0007623C
		private void SetExpDisplay(long currentExp, long oldExp)
		{
			int level = EXPManager.Instance.LevelFromExp(currentExp);
			this.CacheLevelInfo(level);
			float fillAmount = 0f;
			if (oldExp >= this.cachedLevelRange.Item1 && oldExp <= this.cachedLevelRange.Item2)
			{
				fillAmount = (float)(oldExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			}
			float fillAmount2 = (float)(currentExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			this.expBar_OldFill.fillAmount = fillAmount;
			this.expBar_CurrentFill.fillAmount = fillAmount2;
			string arg = (this.cachedLevelRange.Item2 >= long.MaxValue) ? "∞" : this.cachedLevelRange.Item2.ToString();
			this.expDisplay.text = string.Format(this.expFormat, currentExp, arg);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x0007810F File Offset: 0x0007630F
		private void SetLevelDisplay(int level)
		{
			if (this.displayingLevel > 0 && level != this.displayingLevel)
			{
				this.LevelUpPunch();
			}
			this.displayingLevel = level;
			this.levelDisplay.text = string.Format(this.levelFormat, level);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x0007814C File Offset: 0x0007634C
		private void LevelUpPunch()
		{
			PunchReceiver punchReceiver = this.levelDisplayPunchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			PunchReceiver punchReceiver2 = this.barPunchReceiver;
			if (punchReceiver2 != null)
			{
				punchReceiver2.Punch();
			}
			AudioManager.Post(this.sfx_LvUp);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x0007817C File Offset: 0x0007637C
		internal override void TryQuit()
		{
		}

		// Token: 0x04001728 RID: 5928
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001729 RID: 5929
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x0400172A RID: 5930
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400172B RID: 5931
		[SerializeField]
		[LocalizationKey("Default")]
		private string evacuatedTitleTextKey = "UI_Closure_Escaped";

		// Token: 0x0400172C RID: 5932
		[SerializeField]
		private Color evacuatedTitleTextColor = Color.white;

		// Token: 0x0400172D RID: 5933
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTitleTextKey = "UI_Closure_Dead";

		// Token: 0x0400172E RID: 5934
		[SerializeField]
		private Color failedTitleTextColor = Color.red;

		// Token: 0x0400172F RID: 5935
		[SerializeField]
		private GameObject damageInfoContainer;

		// Token: 0x04001730 RID: 5936
		[SerializeField]
		private TextMeshProUGUI damageSourceText;

		// Token: 0x04001731 RID: 5937
		[SerializeField]
		private Image expBar_OldFill;

		// Token: 0x04001732 RID: 5938
		[SerializeField]
		private Image expBar_CurrentFill;

		// Token: 0x04001733 RID: 5939
		[SerializeField]
		private AnimationCurve expBarAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001734 RID: 5940
		[SerializeField]
		private float expBarAnimationTime = 3f;

		// Token: 0x04001735 RID: 5941
		[SerializeField]
		private TextMeshProUGUI expDisplay;

		// Token: 0x04001736 RID: 5942
		[SerializeField]
		private string expFormat = "{0}/<sub>{1}</sub>";

		// Token: 0x04001737 RID: 5943
		[SerializeField]
		private TextMeshProUGUI levelDisplay;

		// Token: 0x04001738 RID: 5944
		[SerializeField]
		private string levelFormat = "Lv.{0}";

		// Token: 0x04001739 RID: 5945
		[SerializeField]
		private PunchReceiver levelDisplayPunchReceiver;

		// Token: 0x0400173A RID: 5946
		[SerializeField]
		private PunchReceiver barPunchReceiver;

		// Token: 0x0400173B RID: 5947
		[SerializeField]
		private Button continueButton;

		// Token: 0x0400173C RID: 5948
		[SerializeField]
		private PunchReceiver continueButtonPunchReceiver;

		// Token: 0x0400173D RID: 5949
		private string sfx_Pop = "UI/pop";

		// Token: 0x0400173E RID: 5950
		private string sfx_ExpUp = "UI/exp_up";

		// Token: 0x0400173F RID: 5951
		private string sfx_LvUp = "UI/level_up";

		// Token: 0x04001740 RID: 5952
		private bool continueButtonClicked;

		// Token: 0x04001741 RID: 5953
		private bool canContinue;

		// Token: 0x04001742 RID: 5954
		private float lastTimeExpUpSfxPlayed = float.MinValue;

		// Token: 0x04001743 RID: 5955
		private const float minIntervalForExpUpSfx = 0.05f;

		// Token: 0x04001744 RID: 5956
		private int cachedLevel = -1;

		// Token: 0x04001745 RID: 5957
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		private ValueTuple<long, long> cachedLevelRange;

		// Token: 0x04001746 RID: 5958
		private long cachedLevelLength;

		// Token: 0x04001747 RID: 5959
		private int displayingLevel = -1;
	}
}
