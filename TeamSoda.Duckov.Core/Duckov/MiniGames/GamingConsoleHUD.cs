using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000286 RID: 646
	public class GamingConsoleHUD : View
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0004D4CD File Offset: 0x0004B6CD
		private static GamingConsoleHUD Instance
		{
			get
			{
				if (GamingConsoleHUD._instance_cache == null)
				{
					GamingConsoleHUD._instance_cache = View.GetViewInstance<GamingConsoleHUD>();
				}
				return GamingConsoleHUD._instance_cache;
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0004D4EB File Offset: 0x0004B6EB
		public static void Show()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalShow();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004D505 File Offset: 0x0004B705
		public static void Hide()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalHide();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004D51F File Offset: 0x0004B71F
		private void LocalShow()
		{
			this.contentFadeGroup.Show();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004D52C File Offset: 0x0004B72C
		private void LocalHide()
		{
			this.contentFadeGroup.Hide();
		}

		// Token: 0x04000F31 RID: 3889
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000F32 RID: 3890
		private static GamingConsoleHUD _instance_cache;
	}
}
