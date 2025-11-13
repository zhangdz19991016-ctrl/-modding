using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class TimeScaleManager : MonoBehaviour
{
	// Token: 0x0600054C RID: 1356 RVA: 0x00017C79 File Offset: 0x00015E79
	private void Awake()
	{
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00017C7C File Offset: 0x00015E7C
	private void Update()
	{
		float timeScale = 1f;
		if (GameManager.Paused)
		{
			timeScale = 0f;
		}
		if (CameraMode.Active)
		{
			timeScale = 0f;
		}
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = Mathf.Max(0.0005f, Time.timeScale * 0.02f);
	}
}
