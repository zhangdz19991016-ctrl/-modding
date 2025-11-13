using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class SkillHud3D : MonoBehaviour
{
	// Token: 0x06000664 RID: 1636 RVA: 0x0001CF3A File Offset: 0x0001B13A
	private void Awake()
	{
		this.HideAll();
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0001CF42 File Offset: 0x0001B142
	private void HideAll()
	{
		this.skillRangeHUD.gameObject.SetActive(false);
		this.projectileLine.gameObject.SetActive(false);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0001CF68 File Offset: 0x0001B168
	private void LateUpdate()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			return;
		}
		this.currentSkill = null;
		this.currentSkill = this.character.skillAction.CurrentRunningSkill;
		if (this.aiming != (this.currentSkill != null))
		{
			this.aiming = !this.aiming;
			if (this.currentSkill != null)
			{
				this.currentSkill = this.character.skillAction.CurrentRunningSkill;
				this.skillRangeHUD.gameObject.SetActive(true);
				float range = 1f;
				if (this.currentSkill.SkillContext.effectRange > 1f)
				{
					range = this.currentSkill.SkillContext.effectRange;
				}
				this.skillRangeHUD.SetRange(range);
				if (this.currentSkill.SkillContext.isGrenade)
				{
					this.projectileLine.gameObject.SetActive(true);
				}
			}
			else
			{
				this.HideAll();
			}
		}
		Vector3 currentSkillAimPoint = this.character.GetCurrentSkillAimPoint();
		Vector3 one = Vector3.one;
		if (this.projectileLine.gameObject.activeSelf)
		{
			bool flag = this.projectileLine.UpdateLine(this.character.CurrentUsingAimSocket.position, currentSkillAimPoint, this.currentSkill.SkillContext.grenageVerticleSpeed, ref one);
		}
		this.skillRangeHUD.transform.position = currentSkillAimPoint;
		this.skillRangeHUD.SetProgress(this.character.skillAction.GetProgress().progress);
	}

	// Token: 0x0400062C RID: 1580
	private CharacterMainControl character;

	// Token: 0x0400062D RID: 1581
	private bool aiming;

	// Token: 0x0400062E RID: 1582
	public SkillRangeHUD skillRangeHUD;

	// Token: 0x0400062F RID: 1583
	public SkillProjectileLineHUD projectileLine;

	// Token: 0x04000630 RID: 1584
	private SkillBase currentSkill;
}
