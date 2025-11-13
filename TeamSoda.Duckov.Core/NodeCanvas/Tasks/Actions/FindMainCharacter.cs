using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000413 RID: 1043
	public class FindMainCharacter : ActionTask<AICharacterController>
	{
		// Token: 0x060025D1 RID: 9681 RVA: 0x00082B14 File Offset: 0x00080D14
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00082B17 File Offset: 0x00080D17
		protected override void OnExecute()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00082B56 File Offset: 0x00080D56
		protected override void OnUpdate()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00082B95 File Offset: 0x00080D95
		protected override void OnStop()
		{
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00082B97 File Offset: 0x00080D97
		protected override void OnPause()
		{
		}

		// Token: 0x040019B4 RID: 6580
		public BBParameter<CharacterMainControl> mainCharacter;
	}
}
