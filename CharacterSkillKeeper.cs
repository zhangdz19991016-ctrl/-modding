using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
[Serializable]
public class CharacterSkillKeeper
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0002B06E File Offset: 0x0002926E
	public SkillBase Skill
	{
		get
		{
			return this.skill;
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0002B076 File Offset: 0x00029276
	public void SetSkill(SkillBase _skill, GameObject _bindingObject)
	{
		this.skill = null;
		this.skillBindingObject = null;
		if (_skill != null && _bindingObject != null)
		{
			this.skill = _skill;
			this.skillBindingObject = _bindingObject;
		}
		Action onSkillChanged = this.OnSkillChanged;
		if (onSkillChanged == null)
		{
			return;
		}
		onSkillChanged();
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002B0B6 File Offset: 0x000292B6
	public bool CheckSkillAndBinding()
	{
		if (this.skill != null && this.skillBindingObject != null)
		{
			return true;
		}
		this.skill = null;
		this.skillBindingObject = null;
		return false;
	}

	// Token: 0x040008B7 RID: 2231
	private SkillBase skill;

	// Token: 0x040008B8 RID: 2232
	private GameObject skillBindingObject;

	// Token: 0x040008B9 RID: 2233
	public Action OnSkillChanged;
}
