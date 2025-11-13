using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000407 RID: 1031
	public class CheckCanReleaseSkill : ConditionTask<AICharacterController>
	{
		// Token: 0x06002599 RID: 9625 RVA: 0x0008234C File Offset: 0x0008054C
		protected override bool OnCheck()
		{
			if (base.agent == null)
			{
				return false;
			}
			if (!base.agent.hasSkill)
			{
				return false;
			}
			if (!base.agent.skillInstance)
			{
				return false;
			}
			if (Time.time < base.agent.nextReleaseSkillTimeMarker)
			{
				return false;
			}
			if (!base.agent.CharacterMainControl.skillAction.IsSkillHasEnoughStaminaAndCD(base.agent.skillInstance))
			{
				return false;
			}
			if (base.agent.CharacterMainControl.CurrentAction && base.agent.CharacterMainControl.CurrentAction.Running)
			{
				return false;
			}
			base.agent.nextReleaseSkillTimeMarker = Time.time + UnityEngine.Random.Range(base.agent.skillCoolTimeRange.x, base.agent.skillCoolTimeRange.y);
			return UnityEngine.Random.Range(0f, 1f) <= base.agent.skillSuccessChance;
		}
	}
}
