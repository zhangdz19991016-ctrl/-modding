using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class SetActiveByInputDevice : MonoBehaviour
{
	// Token: 0x06000B2C RID: 2860 RVA: 0x0002FEA5 File Offset: 0x0002E0A5
	private void Awake()
	{
		this.OnInputDeviceChanged();
		InputManager.OnInputDeviceChanged += this.OnInputDeviceChanged;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0002FEBE File Offset: 0x0002E0BE
	private void OnDestroy()
	{
		InputManager.OnInputDeviceChanged -= this.OnInputDeviceChanged;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0002FED1 File Offset: 0x0002E0D1
	private void OnInputDeviceChanged()
	{
		if (InputManager.InputDevice == this.activeIfDeviceIs)
		{
			base.gameObject.SetActive(true);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040009A3 RID: 2467
	public InputManager.InputDevices activeIfDeviceIs;
}
