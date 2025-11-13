using System;
using System.Collections;
using UnityEngine;
using WyvrnSDK;

namespace Duckov
{
	// Token: 0x02000239 RID: 569
	public class WyvrnManager : MonoBehaviour
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x0004560D File Offset: 0x0004380D
		// (set) Token: 0x060011DC RID: 4572 RVA: 0x00045614 File Offset: 0x00043814
		public static WyvrnManager Instance { get; private set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x0004561C File Offset: 0x0004381C
		public static bool Initialized
		{
			get
			{
				return !(WyvrnManager.Instance == null) && WyvrnManager.Instance.initialized;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00045637 File Offset: 0x00043837
		private void Awake()
		{
			if (WyvrnManager.Instance != null)
			{
				Debug.LogError("Multiple wyvrn managers detected");
			}
			WyvrnManager.Instance = this;
			HardwareSyncingManager.OnSetEvent += this.OnHardwareSyncingManagerSetEvent;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00045668 File Offset: 0x00043868
		private void OnDestroy()
		{
			HardwareSyncingManager.OnSetEvent -= this.OnHardwareSyncingManagerSetEvent;
			if (!WyvrnManager.Initialized)
			{
				return;
			}
			int num = WyvrnAPI.CoreUnInit();
			if (num != 0)
			{
				Debug.LogError(string.Format("[WYVRN] Failed uninitializing wyvrn api. code: {0}", num));
			}
			this.initialized = false;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000456B3 File Offset: 0x000438B3
		private void OnHardwareSyncingManagerSetEvent(string eventName)
		{
			this.SetEvent(eventName);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000456BC File Offset: 0x000438BC
		public IEnumerator Start()
		{
			if (!WyvrnAPI.IsWyvrnSDKAvailable())
			{
				this._mResult = 6023;
				yield break;
			}
			APPINFOTYPE appinfotype = default(APPINFOTYPE);
			appinfotype.Title = "Escape From Duckov";
			appinfotype.Description = "Escape From Duckov";
			appinfotype.Author_Name = "Team Soda";
			appinfotype.Author_Contact = "https://game.bilibili.com/duckov/";
			appinfotype.Category = 2U;
			this._mResult = WyvrnAPI.CoreInitSDK(ref appinfotype);
			if (this._mResult == 0)
			{
				this.initialized = true;
			}
			yield break;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000456CC File Offset: 0x000438CC
		public void SetEvent(string eventName)
		{
			if (!WyvrnManager.Initialized)
			{
				return;
			}
			int num = WyvrnAPI.CoreSetEventName(eventName);
			if (num != 0)
			{
				Debug.LogError(string.Format("[WYVRN] Failed setting event in wyvrn api. code: {0}", num));
			}
		}

		// Token: 0x04000DC4 RID: 3524
		private bool initialized;

		// Token: 0x04000DC5 RID: 3525
		private int _mResult;
	}
}
