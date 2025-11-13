using System;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000416 RID: 1046
	public class PopText : ActionTask<AICharacterController>
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x00082F7A File Offset: 0x0008117A
		private string Key
		{
			get
			{
				return this.content.value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060025E3 RID: 9699 RVA: 0x00082F87 File Offset: 0x00081187
		private string DisplayText
		{
			get
			{
				return this.Key.ToPlainText();
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00082F94 File Offset: 0x00081194
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x00082F97 File Offset: 0x00081197
		protected override string info
		{
			get
			{
				return string.Format("Pop:'{0}'", this.DisplayText);
			}
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00082FAC File Offset: 0x000811AC
		protected override void OnExecute()
		{
			if (this.checkHide && base.agent.CharacterMainControl.Hidden)
			{
				base.EndAction(true);
				return;
			}
			if (!base.agent.canTalk)
			{
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.PopText(this.DisplayText, -1f);
			base.EndAction(true);
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00083012 File Offset: 0x00081212
		protected override void OnStop()
		{
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00083014 File Offset: 0x00081214
		protected override void OnPause()
		{
		}

		// Token: 0x040019C3 RID: 6595
		public BBParameter<string> content;

		// Token: 0x040019C4 RID: 6596
		public bool checkHide;
	}
}
