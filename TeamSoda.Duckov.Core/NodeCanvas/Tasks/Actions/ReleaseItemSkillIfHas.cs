using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000418 RID: 1048
	public class ReleaseItemSkillIfHas : ActionTask<AICharacterController>
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00083126 File Offset: 0x00081326
		private float chance
		{
			get
			{
				if (!base.agent)
				{
					return 0f;
				}
				return base.agent.itemSkillChance;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x00083146 File Offset: 0x00081346
		public float checkTimeSpace
		{
			get
			{
				if (!base.agent)
				{
					return 999f;
				}
				return base.agent.itemSkillCoolTime;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00083166 File Offset: 0x00081366
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0008316C File Offset: 0x0008136C
		protected override void OnExecute()
		{
			this.skillRefrence = null;
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				base.EndAction(false);
				return;
			}
			this.checkTimeMarker = Time.time;
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				base.EndAction(false);
				return;
			}
			ItemSetting_Skill itemSkill = base.agent.GetItemSkill(this.random);
			if (!itemSkill)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.CharacterMainControl.CurrentAction && base.agent.CharacterMainControl.CurrentAction.Running)
			{
				base.EndAction(false);
				return;
			}
			this.skillRefrence = itemSkill;
			base.agent.CharacterMainControl.ChangeHoldItem(itemSkill.Item);
			base.agent.CharacterMainControl.SetSkill(SkillTypes.itemSkill, itemSkill.Skill, itemSkill.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.itemSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = itemSkill.Skill.SkillContext.skillReadyTime;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0008328C File Offset: 0x0008148C
		protected override void OnUpdate()
		{
			if (!this.skillRefrence)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime > this.readyTime + 0.1f)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.itemSkill);
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00083313 File Offset: 0x00081513
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x040019C6 RID: 6598
		public bool random = true;

		// Token: 0x040019C7 RID: 6599
		private float checkTimeMarker = -1f;

		// Token: 0x040019C8 RID: 6600
		private float readyTime;

		// Token: 0x040019C9 RID: 6601
		private ItemSetting_Skill skillRefrence;
	}
}
