using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200007C RID: 124
public class SkillHUD : MonoBehaviour
{
	// Token: 0x060004B5 RID: 1205 RVA: 0x00015908 File Offset: 0x00013B08
	private void Awake()
	{
		this.SyncHud();
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00015910 File Offset: 0x00013B10
	private void SyncHud()
	{
		if (this.rangeCache < 0f)
		{
			this.rangeCache = this.skillJoystick.joystickRangePercent;
		}
		this.activeParent.SetActive(this.skillHudActive);
		if (this.skillHudActive)
		{
			this.skillIcon.sprite = this.skillKeeper.Skill.icon;
			if (this.skillKeeper.Skill.SkillContext.castRange > 0f)
			{
				this.skillJoystick.canCancle = true;
				this.skillJoystick.joystickRangePercent = this.rangeCache;
				return;
			}
			this.skillJoystick.canCancle = false;
			this.skillJoystick.joystickRangePercent = 0f;
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000159C8 File Offset: 0x00013BC8
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
			this.OnInit();
		}
		if (this.skillHudActive && (this.skillKeeper == null || !this.skillKeeper.CheckSkillAndBinding()))
		{
			this.skillHudActive = false;
			this.SyncHud();
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00015A30 File Offset: 0x00013C30
	private void OnInit()
	{
		SkillTypes skillTypes = this.skillType;
		if (skillTypes != SkillTypes.itemSkill)
		{
			if (skillTypes == SkillTypes.characterSkill)
			{
				this.skillKeeper = this.characterMainControl.skillAction.characterSkillKeeper;
				this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetCharacterSkillAimInput));
				this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartCharacterSkillAim));
				this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.CharacterSkillRelease));
			}
		}
		else
		{
			this.skillKeeper = this.characterMainControl.skillAction.holdItemSkillKeeper;
			this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetItemSkillAimInput));
			this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartItemSkillAim));
			this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.ItemSkillRelease));
		}
		CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
		characterSkillKeeper.OnSkillChanged = (Action)Delegate.Combine(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		if (this.skillKeeper.CheckSkillAndBinding())
		{
			this.OnSkillChanged();
		}
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00015B81 File Offset: 0x00013D81
	private void OnSkillChanged()
	{
		this.skillHudActive = this.skillKeeper.CheckSkillAndBinding();
		if (this.skillJoystick.Holding)
		{
			this.skillJoystick.CancleTouch();
		}
		this.SyncHud();
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00015BB2 File Offset: 0x00013DB2
	private void OnDestroy()
	{
		if (this.skillKeeper != null)
		{
			CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
			characterSkillKeeper.OnSkillChanged = (Action)Delegate.Remove(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		}
	}

	// Token: 0x040003F8 RID: 1016
	private CharacterMainControl characterMainControl;

	// Token: 0x040003F9 RID: 1017
	public CharacterTouchInputControl touchInputController;

	// Token: 0x040003FA RID: 1018
	public Image skillIcon;

	// Token: 0x040003FB RID: 1019
	private bool skillHudActive;

	// Token: 0x040003FC RID: 1020
	public Soda_Joysticks skillJoystick;

	// Token: 0x040003FD RID: 1021
	public GameObject skillButton;

	// Token: 0x040003FE RID: 1022
	public GameObject activeParent;

	// Token: 0x040003FF RID: 1023
	[SerializeField]
	private SkillTypes skillType;

	// Token: 0x04000400 RID: 1024
	private CharacterSkillKeeper skillKeeper;

	// Token: 0x04000401 RID: 1025
	private float rangeCache = -1f;
}
