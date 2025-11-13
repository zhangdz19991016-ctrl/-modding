using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class CharacterTouchInputControl : MonoBehaviour
{
	// Token: 0x06000498 RID: 1176 RVA: 0x000151F9 File Offset: 0x000133F9
	public void SetMoveInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetMoveInput(axisInput);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00015207 File Offset: 0x00013407
	public void SetRunInput(bool holding)
	{
		this.characterInputManager.SetRunInput(holding);
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00015215 File Offset: 0x00013415
	public void SetAdsInput(bool holding)
	{
		this.characterInputManager.SetAdsInput(holding);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00015223 File Offset: 0x00013423
	public void SetGunAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.normalAim);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0001523D File Offset: 0x0001343D
	public void SetCharacterSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.characterSkill);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00015257 File Offset: 0x00013457
	public void StartCharacterSkillAim()
	{
		this.characterInputManager.StartCharacterSkillAim();
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00015264 File Offset: 0x00013464
	public void CharacterSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseCharacterSkill();
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00015281 File Offset: 0x00013481
	public void SetItemSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.handheldSkill);
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001529B File Offset: 0x0001349B
	public void StartItemSkillAim()
	{
		this.characterInputManager.StartItemSkillAim();
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000152A8 File Offset: 0x000134A8
	public void ItemSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseItemSkill();
	}

	// Token: 0x040003E0 RID: 992
	public InputManager characterInputManager;
}
