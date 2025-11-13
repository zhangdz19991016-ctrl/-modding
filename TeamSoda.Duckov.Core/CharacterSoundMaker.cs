using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CharacterSoundMaker : MonoBehaviour
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000392 RID: 914 RVA: 0x0000F95D File Offset: 0x0000DB5D
	public float walkSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.WalkSoundRange;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000393 RID: 915 RVA: 0x0000F97D File Offset: 0x0000DB7D
	public float runSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.RunSoundRange;
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
	private void Update()
	{
		if (this.characterMainControl.movementControl.Velocity.magnitude < 0.5f)
		{
			this.moveSoundTimer = 0f;
			return;
		}
		this.moveSoundTimer += Time.deltaTime;
		bool running = this.characterMainControl.Running;
		float num = 1f / (running ? this.runSoundFrequence : this.walkSoundFrequence);
		if (this.moveSoundTimer >= num)
		{
			this.moveSoundTimer = 0f;
			if (this.characterMainControl.IsInAdsInput)
			{
				return;
			}
			if (!this.characterMainControl.CharacterItem)
			{
				return;
			}
			bool flag = this.characterMainControl.CharacterItem.TotalWeight / this.characterMainControl.MaxWeight >= 0.75f;
			AISound sound = default(AISound);
			sound.pos = base.transform.position;
			sound.fromTeam = this.characterMainControl.Team;
			sound.soundType = SoundTypes.unknowNoise;
			sound.fromObject = this.characterMainControl.gameObject;
			sound.fromCharacter = this.characterMainControl;
			if (this.characterMainControl.Running)
			{
				if (this.runSoundDistance > 0f)
				{
					sound.radius = this.runSoundDistance * (flag ? 1.5f : 1f);
					Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound = CharacterSoundMaker.OnFootStepSound;
					if (onFootStepSound != null)
					{
						onFootStepSound(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.runHeavy : CharacterSoundMaker.FootStepTypes.runLight, this.characterMainControl);
					}
				}
			}
			else if (this.walkSoundDistance > 0f)
			{
				sound.radius = this.walkSoundDistance * (flag ? 1.5f : 1f);
				Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound2 = CharacterSoundMaker.OnFootStepSound;
				if (onFootStepSound2 != null)
				{
					onFootStepSound2(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.walkHeavy : CharacterSoundMaker.FootStepTypes.walkLight, this.characterMainControl);
				}
			}
			AIMainBrain.MakeSound(sound);
		}
	}

	// Token: 0x040002A8 RID: 680
	public CharacterMainControl characterMainControl;

	// Token: 0x040002A9 RID: 681
	private float moveSoundTimer;

	// Token: 0x040002AA RID: 682
	public float walkSoundFrequence = 4f;

	// Token: 0x040002AB RID: 683
	public float runSoundFrequence = 7f;

	// Token: 0x040002AC RID: 684
	public static Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> OnFootStepSound;

	// Token: 0x0200043A RID: 1082
	public enum FootStepTypes
	{
		// Token: 0x04001A64 RID: 6756
		walkLight,
		// Token: 0x04001A65 RID: 6757
		walkHeavy,
		// Token: 0x04001A66 RID: 6758
		runLight,
		// Token: 0x04001A67 RID: 6759
		runHeavy
	}
}
