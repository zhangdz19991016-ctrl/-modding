using System;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000237 RID: 567
	public class RichPresenceManager : MonoBehaviour
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00045229 File Offset: 0x00043429
		public bool isPlaying
		{
			get
			{
				return !this.isMainMenu;
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00045234 File Offset: 0x00043434
		private void InvokeChangeEvent()
		{
			Action<RichPresenceManager> onInstanceChanged = RichPresenceManager.OnInstanceChanged;
			if (onInstanceChanged == null)
			{
				return;
			}
			onInstanceChanged(this);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00045248 File Offset: 0x00043448
		private void Awake()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Combine(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Combine(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake += this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy += this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000452B8 File Offset: 0x000434B8
		private void OnDestroy()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Remove(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Remove(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake -= this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy -= this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00045327 File Offset: 0x00043527
		private void OnMainMenuAwake()
		{
			this.isMainMenu = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00045336 File Offset: 0x00043536
		private void OnMainMenuDestroy()
		{
			this.isMainMenu = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00045345 File Offset: 0x00043545
		private void OnMultiSceneCoreInstanceAwake(MultiSceneCore core)
		{
			this.levelDisplayNameRaw = core.DisplaynameRaw;
			this.isInLevel = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00045360 File Offset: 0x00043560
		private void OnMultiSceneCoreInstanceDestroy(MultiSceneCore core)
		{
			this.isInLevel = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004536F File Offset: 0x0004356F
		internal string GetSteamDisplay()
		{
			if (Application.isEditor)
			{
				return "#Status_UnityEditor";
			}
			if (!this.isMainMenu)
			{
				return "#Status_Playing";
			}
			return "#Status_MainMenu";
		}

		// Token: 0x04000DBE RID: 3518
		public bool isMainMenu = true;

		// Token: 0x04000DBF RID: 3519
		public bool isInLevel;

		// Token: 0x04000DC0 RID: 3520
		public string levelDisplayNameRaw;

		// Token: 0x04000DC1 RID: 3521
		public static Action<RichPresenceManager> OnInstanceChanged;
	}
}
