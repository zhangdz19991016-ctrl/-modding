using System;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C6 RID: 966
	public class PlayerStatsView : View
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x0007BF17 File Offset: 0x0007A117
		public static PlayerStatsView Instance
		{
			get
			{
				return View.GetViewInstance<PlayerStatsView>();
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0007BF1E File Offset: 0x0007A11E
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0007BF26 File Offset: 0x0007A126
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0007BF39 File Offset: 0x0007A139
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0007BF4C File Offset: 0x0007A14C
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0007BF54 File Offset: 0x0007A154
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0007BF5C File Offset: 0x0007A15C
		private void RegisterEvents()
		{
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0007BF5E File Offset: 0x0007A15E
		private void UnregisterEvents()
		{
		}

		// Token: 0x040017F7 RID: 6135
		[SerializeField]
		private FadeGroup fadeGroup;
	}
}
