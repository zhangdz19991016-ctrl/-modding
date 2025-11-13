using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class TimeOfDayAlert : MonoBehaviour
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000673 RID: 1651 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
	// (remove) Token: 0x06000674 RID: 1652 RVA: 0x0001D51C File Offset: 0x0001B71C
	public static event Action OnAlertTriggeredEvent;

	// Token: 0x06000675 RID: 1653 RVA: 0x0001D54F File Offset: 0x0001B74F
	private void Awake()
	{
		this.canvasGroup.alpha = 0f;
		TimeOfDayAlert.OnAlertTriggeredEvent += this.OnAlertTriggered;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0001D572 File Offset: 0x0001B772
	private void OnDestroy()
	{
		TimeOfDayAlert.OnAlertTriggeredEvent -= this.OnAlertTriggered;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0001D588 File Offset: 0x0001B788
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!LevelManager.Instance.IsBaseLevel)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		if (this.timer <= 0f && this.canvasGroup.alpha > 0f)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, 0f, 0.4f * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0001D620 File Offset: 0x0001B820
	private void OnAlertTriggered()
	{
		bool flag = false;
		float time = TimeOfDayController.Instance.Time;
		if (TimeOfDayController.Instance.AtNight)
		{
			flag = true;
			Debug.Log(string.Format("At Night,time:{0}", time));
			this.text.text = this.inNightKey.ToPlainText();
		}
		else if (TimeOfDayController.Instance.nightStart - time < 4f)
		{
			flag = true;
			Debug.Log(string.Format("Near Night,time:{0},night start:{1}", time, TimeOfDayController.Instance.nightStart));
			this.text.text = this.nearNightKey.ToPlainText();
		}
		if (!flag)
		{
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.timer = this.stayTime;
		this.blinkPunch.Punch();
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0001D6EF File Offset: 0x0001B8EF
	public static void EnterAlertTrigger()
	{
		Action onAlertTriggeredEvent = TimeOfDayAlert.OnAlertTriggeredEvent;
		if (onAlertTriggeredEvent == null)
		{
			return;
		}
		onAlertTriggeredEvent();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001D700 File Offset: 0x0001B900
	public static void LeaveAlertTrigger()
	{
	}

	// Token: 0x0400063A RID: 1594
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x0400063C RID: 1596
	[SerializeField]
	private ColorPunch blinkPunch;

	// Token: 0x0400063E RID: 1598
	[LocalizationKey("Default")]
	public string nearNightKey = "TODAlert_NearNight";

	// Token: 0x0400063F RID: 1599
	[LocalizationKey("Default")]
	public string inNightKey = "TODAlert_InNight";

	// Token: 0x04000640 RID: 1600
	private float stayTime = 5f;

	// Token: 0x04000641 RID: 1601
	private float timer;
}
