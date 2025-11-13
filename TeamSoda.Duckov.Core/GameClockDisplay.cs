using System;
using TMPro;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class GameClockDisplay : MonoBehaviour
{
	// Token: 0x06000D45 RID: 3397 RVA: 0x00037A51 File Offset: 0x00035C51
	private void Awake()
	{
		this.Refresh();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00037A59 File Offset: 0x00035C59
	private void OnEnable()
	{
		GameClock.OnGameClockStep += this.Refresh;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00037A6C File Offset: 0x00035C6C
	private void OnDisable()
	{
		GameClock.OnGameClockStep -= this.Refresh;
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00037A80 File Offset: 0x00035C80
	private void Refresh()
	{
		string text;
		if (GameClock.Instance == null)
		{
			text = "--:--";
		}
		else
		{
			text = string.Format("{0:00}:{1:00}", GameClock.Hour, GameClock.Minut);
		}
		this.text.text = text;
	}

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private TextMeshProUGUI text;
}
