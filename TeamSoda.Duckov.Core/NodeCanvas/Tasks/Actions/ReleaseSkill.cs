using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000419 RID: 1049
	public class ReleaseSkill : ActionTask<AICharacterController>
	{
		// Token: 0x060025F7 RID: 9719 RVA: 0x00083351 File Offset: 0x00081551
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00083354 File Offset: 0x00081554
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetSkill(SkillTypes.characterSkill, base.agent.skillInstance, base.agent.skillInstance.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.characterSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = base.agent.skillInstance.SkillContext.skillReadyTime;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000833C4 File Offset: 0x000815C4
		protected override void OnUpdate()
		{
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime <= this.readyTime + 0.1f)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) < base.agent.skillSuccessChance)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.characterSkill);
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.CancleSkill();
			base.EndAction(false);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0008346B File Offset: 0x0008166B
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x040019CA RID: 6602
		private float readyTime;

		// Token: 0x040019CB RID: 6603
		private float tryReleaseSkillTimeMarker = -1f;
	}
}
