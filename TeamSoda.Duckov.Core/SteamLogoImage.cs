using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EC RID: 492
public class SteamLogoImage : MonoBehaviour
{
	// Token: 0x06000E9A RID: 3738 RVA: 0x0003B287 File Offset: 0x00039487
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x0003B290 File Offset: 0x00039490
	private void Refresh()
	{
		if (!SteamManager.Initialized)
		{
			this.image.sprite = this.steamLogo;
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.image.sprite = this.steamChinaLogo;
			return;
		}
		this.image.sprite = this.steamLogo;
	}

	// Token: 0x04000C20 RID: 3104
	[SerializeField]
	private Image image;

	// Token: 0x04000C21 RID: 3105
	[SerializeField]
	private Sprite steamLogo;

	// Token: 0x04000C22 RID: 3106
	[SerializeField]
	private Sprite steamChinaLogo;
}
