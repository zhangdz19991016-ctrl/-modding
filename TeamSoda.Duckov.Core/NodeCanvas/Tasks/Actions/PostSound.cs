using System;
using Duckov;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000417 RID: 1047
	public class PostSound : ActionTask<AICharacterController>
	{
		// Token: 0x060025EA RID: 9706 RVA: 0x0008301E File Offset: 0x0008121E
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x00083021 File Offset: 0x00081221
		protected override string info
		{
			get
			{
				return string.Format("Post Sound: {0} ", this.voiceSound.ToString());
			}
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x00083040 File Offset: 0x00081240
		protected override void OnExecute()
		{
			if (base.agent && base.agent.CharacterMainControl)
			{
				if (!base.agent.canTalk)
				{
					base.EndAction(true);
					return;
				}
				GameObject gameObject = base.agent.CharacterMainControl.gameObject;
				switch (this.voiceSound)
				{
				case PostSound.VoiceSounds.normal:
					AudioManager.PostQuak("normal", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.surprise:
					AudioManager.PostQuak("surprise", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.death:
					AudioManager.PostQuak("death", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			base.EndAction(true);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0008311A File Offset: 0x0008131A
		protected override void OnStop()
		{
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0008311C File Offset: 0x0008131C
		protected override void OnPause()
		{
		}

		// Token: 0x040019C5 RID: 6597
		public PostSound.VoiceSounds voiceSound;

		// Token: 0x0200067F RID: 1663
		public enum VoiceSounds
		{
			// Token: 0x04002387 RID: 9095
			normal,
			// Token: 0x04002388 RID: 9096
			surprise,
			// Token: 0x04002389 RID: 9097
			death
		}
	}
}
