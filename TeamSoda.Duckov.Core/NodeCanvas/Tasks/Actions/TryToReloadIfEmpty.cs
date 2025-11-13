using System;
using Duckov;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000423 RID: 1059
	public class TryToReloadIfEmpty : ActionTask<AICharacterController>
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x00083F42 File Offset: 0x00082142
		public string SoundKey
		{
			get
			{
				return "normal";
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002635 RID: 9781 RVA: 0x00083F49 File Offset: 0x00082149
		private string Key
		{
			get
			{
				return this.poptextWhileReloading;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x00083F51 File Offset: 0x00082151
		private string DisplayText
		{
			get
			{
				return this.poptextWhileReloading.ToPlainText();
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x00083F5E File Offset: 0x0008215E
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x00083F64 File Offset: 0x00082164
		protected override void OnExecute()
		{
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun == null)
			{
				base.EndAction(true);
				return;
			}
			if (gun.BulletCount <= 0)
			{
				base.agent.CharacterMainControl.TryToReload(null);
				if (!this.isFirstTime)
				{
					if (!base.agent.CharacterMainControl.Health.Hidden && this.poptextWhileReloading != string.Empty && base.agent.canTalk)
					{
						base.agent.CharacterMainControl.PopText(this.poptextWhileReloading.ToPlainText(), -1f);
					}
					if (this.postSound && this.SoundKey != string.Empty && base.agent && base.agent.CharacterMainControl)
					{
						AudioManager.PostQuak(this.SoundKey, base.agent.CharacterMainControl.AudioVoiceType, base.agent.CharacterMainControl.gameObject);
					}
				}
			}
			this.isFirstTime = false;
			base.EndAction(true);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x00084085 File Offset: 0x00082285
		protected override void OnUpdate()
		{
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x00084087 File Offset: 0x00082287
		protected override void OnStop()
		{
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x00084089 File Offset: 0x00082289
		protected override void OnPause()
		{
		}

		// Token: 0x040019F7 RID: 6647
		public string poptextWhileReloading = "PopText_Reloading";

		// Token: 0x040019F8 RID: 6648
		public bool postSound;

		// Token: 0x040019F9 RID: 6649
		private bool isFirstTime = true;
	}
}
