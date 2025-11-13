using System;
using ItemStatsSystem;

// Token: 0x020000F7 RID: 247
public class ItemSetting_Skill : ItemSettingBase
{
	// Token: 0x06000820 RID: 2080 RVA: 0x00024820 File Offset: 0x00022A20
	public override void OnInit()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Combine(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
			this.Skill.fromItem = base.Item;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00024874 File Offset: 0x00022A74
	private void OnSkillReleased()
	{
		ItemSetting_Skill.OnReleaseAction onReleaseAction = this.onRelease;
		if (onReleaseAction != ItemSetting_Skill.OnReleaseAction.none && onReleaseAction == ItemSetting_Skill.OnReleaseAction.reduceCount && (!LevelManager.Instance || !LevelManager.Instance.IsBaseLevel))
		{
			if (base.Item.Stackable)
			{
				base.Item.StackCount--;
				return;
			}
			base.Item.Detach();
			base.Item.DestroyTree();
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x000248DE File Offset: 0x00022ADE
	private void OnDestroy()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Remove(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
		}
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00024914 File Offset: 0x00022B14
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsSkill", true, true);
	}

	// Token: 0x04000791 RID: 1937
	public ItemSetting_Skill.OnReleaseAction onRelease;

	// Token: 0x04000792 RID: 1938
	public SkillBase Skill;

	// Token: 0x02000476 RID: 1142
	public enum OnReleaseAction
	{
		// Token: 0x04001B98 RID: 7064
		none,
		// Token: 0x04001B99 RID: 7065
		reduceCount
	}
}
