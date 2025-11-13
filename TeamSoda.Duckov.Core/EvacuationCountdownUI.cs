using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C5 RID: 197
public class EvacuationCountdownUI : MonoBehaviour
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001C5F2 File Offset: 0x0001A7F2
	public static EvacuationCountdownUI Instance
	{
		get
		{
			return EvacuationCountdownUI._instance;
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0001C5F9 File Offset: 0x0001A7F9
	private void Awake()
	{
		if (EvacuationCountdownUI._instance == null)
		{
			EvacuationCountdownUI._instance = this;
		}
		if (EvacuationCountdownUI._instance != this)
		{
			Debug.LogWarning("Multiple Evacuation Countdown UI detected");
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0001C628 File Offset: 0x0001A828
	private string ToDigitString(float number)
	{
		int num = (int)number;
		int num2 = Mathf.Min(999, Mathf.RoundToInt((number - (float)num) * 1000f));
		int num3 = num / 60;
		num -= num3 * 60;
		return string.Format(this.digitFormat, num3, num, num2);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0001C67B File Offset: 0x0001A87B
	private void Update()
	{
		if (this.target == null && this.fadeGroup.IsShown)
		{
			this.Hide().Forget();
		}
		this.Refresh();
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001C6AC File Offset: 0x0001A8AC
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.progressFill.fillAmount = this.target.Progress;
		this.countdownDigit.text = this.ToDigitString(this.target.RemainingTime);
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0001C6FC File Offset: 0x0001A8FC
	private UniTask Hide()
	{
		EvacuationCountdownUI.<Hide>d__12 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<EvacuationCountdownUI.<Hide>d__12>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0001C740 File Offset: 0x0001A940
	private UniTask Show(CountDownArea target)
	{
		EvacuationCountdownUI.<Show>d__13 <Show>d__;
		<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Show>d__.<>4__this = this;
		<Show>d__.target = target;
		<Show>d__.<>1__state = -1;
		<Show>d__.<>t__builder.Start<EvacuationCountdownUI.<Show>d__13>(ref <Show>d__);
		return <Show>d__.<>t__builder.Task;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0001C78B File Offset: 0x0001A98B
	public static void Request(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		EvacuationCountdownUI.Instance.Show(target).Forget();
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0001C7AB File Offset: 0x0001A9AB
	public static void Release(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		if (EvacuationCountdownUI.Instance.target == target)
		{
			EvacuationCountdownUI.Instance.Hide().Forget();
		}
	}

	// Token: 0x04000601 RID: 1537
	private static EvacuationCountdownUI _instance;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private Image progressFill;

	// Token: 0x04000604 RID: 1540
	[SerializeField]
	private TextMeshProUGUI countdownDigit;

	// Token: 0x04000605 RID: 1541
	[SerializeField]
	private string digitFormat = "{0:00}:{1:00}<sub>.{2:000}</sub>";

	// Token: 0x04000606 RID: 1542
	private CountDownArea target;
}
