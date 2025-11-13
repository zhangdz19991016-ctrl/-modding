using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class SunFogEntry : MonoBehaviour
{
	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06000E3A RID: 3642 RVA: 0x0003A350 File Offset: 0x00038550
	// (remove) Token: 0x06000E3B RID: 3643 RVA: 0x0003A384 File Offset: 0x00038584
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000E3C RID: 3644 RVA: 0x0003A3B7 File Offset: 0x000385B7
	public static void SetEnabled(bool enabled)
	{
		SunFogEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = SunFogEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0003A3CE File Offset: 0x000385CE
	private void Awake()
	{
		SunFogEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0003A3F1 File Offset: 0x000385F1
	private void OnDestroy()
	{
		SunFogEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0003A404 File Offset: 0x00038604
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x04000BE4 RID: 3044
	private static bool settingEnabled = true;
}
