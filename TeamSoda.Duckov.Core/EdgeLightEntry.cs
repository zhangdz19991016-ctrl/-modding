using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class EdgeLightEntry : MonoBehaviour
{
	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06000DDE RID: 3550 RVA: 0x00039404 File Offset: 0x00037604
	// (remove) Token: 0x06000DDF RID: 3551 RVA: 0x00039438 File Offset: 0x00037638
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0003946B File Offset: 0x0003766B
	public static void SetEnabled(bool enabled)
	{
		EdgeLightEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = EdgeLightEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x00039482 File Offset: 0x00037682
	private void Awake()
	{
		EdgeLightEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x000394A5 File Offset: 0x000376A5
	private void OnDestroy()
	{
		EdgeLightEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000394B8 File Offset: 0x000376B8
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x04000BC2 RID: 3010
	private static bool settingEnabled = true;
}
