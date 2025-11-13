using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class MainMenu : MonoBehaviour
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x0002F9F4 File Offset: 0x0002DBF4
	private void Awake()
	{
		Action onMainMenuAwake = MainMenu.OnMainMenuAwake;
		if (onMainMenuAwake == null)
		{
			return;
		}
		onMainMenuAwake();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0002FA05 File Offset: 0x0002DC05
	private void OnDestroy()
	{
		Action onMainMenuDestroy = MainMenu.OnMainMenuDestroy;
		if (onMainMenuDestroy == null)
		{
			return;
		}
		onMainMenuDestroy();
	}

	// Token: 0x0400098A RID: 2442
	public static Action OnMainMenuAwake;

	// Token: 0x0400098B RID: 2443
	public static Action OnMainMenuDestroy;
}
