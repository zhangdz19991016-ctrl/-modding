using System;
using System.Runtime.CompilerServices;

namespace DuckovSkills
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public class DisplaySkillValueConfig
	{
		// Token: 0x04000003 RID: 3
		public string hotKey = "<Keyboard>/alt";

		// Token: 0x04000004 RID: 4
		public int skillCoolDown = 90;

		// Token: 0x04000005 RID: 5
		public int skillDuration = 30;

		// Token: 0x04000006 RID: 6
		public string defaultName = ModBehaviour.IS_CHINESE ? "默认角色技能" : "Default Role Skills";

		// Token: 0x04000007 RID: 7
		public bool defaultRemoveBleeding = true;

		// Token: 0x04000008 RID: 8
		public float defaultAddHealth = 15f;

		// Token: 0x04000009 RID: 9
		public string surviverName = ModBehaviour.IS_CHINESE ? "幸存者技能" : "Surviver Skills";

		// Token: 0x0400000A RID: 10
		public bool surviverImmuneBleeding = true;

		// Token: 0x0400000B RID: 11
		public bool surviverImmunePoison = true;

		// Token: 0x0400000C RID: 12
		public bool surviverImmuneElectric = true;

		// Token: 0x0400000D RID: 13
		public bool surviverImmuneBurning = true;

		// Token: 0x0400000E RID: 14
		public bool surviverImmuneNauseous = true;

		// Token: 0x0400000F RID: 15
		public bool surviverImmuneStun = true;

		// Token: 0x04000010 RID: 16
		public float surviverAddHealthTickPercent = 10f;

		// Token: 0x04000011 RID: 17
		public string porterName = ModBehaviour.IS_CHINESE ? "搬运者技能" : "Porter Skills";

		// Token: 0x04000012 RID: 18
		public float porterAddWalkSpeed = 3.5f;

		// Token: 0x04000013 RID: 19
		public float porterAddRunSpeed = 3.5f;

		// Token: 0x04000014 RID: 20
		public float porterAddMaxWeight = 20f;

		// Token: 0x04000015 RID: 21
		public string berserkerName = ModBehaviour.IS_CHINESE ? "狂战士技能" : "Berserker Skills";

		// Token: 0x04000016 RID: 22
		public float berserkerAddLifeStealPercent = 30f;

		// Token: 0x04000017 RID: 23
		public float berserkerAddStaminaDrainRate = -1.2f;

		// Token: 0x04000018 RID: 24
		public float berserkerAddBodyArmor = 3f;

		// Token: 0x04000019 RID: 25
		public float berserkerAddHeadArmor = 3f;

		// Token: 0x0400001A RID: 26
		public float berserkerAddGunDamageMultiplier = -0.75f;

		// Token: 0x0400001B RID: 27
		public float berserkerAddMeleeDamageMultiplier = 0.25f;

		// Token: 0x0400001C RID: 28
		public float berserkerAddMeleeCritRateGain = 1f;

		// Token: 0x0400001D RID: 29
		public float berserkerAddMeleeCritDamageGain = 0.5f;

		// Token: 0x0400001E RID: 30
		public float berserkerAddWalkSoundRange = -0.8f;

		// Token: 0x0400001F RID: 31
		public float berserkerAddRunSoundRange = -0.8f;

		// Token: 0x04000020 RID: 32
		public string marksmanName = ModBehaviour.IS_CHINESE ? "枪手" : "Marksman";

		// Token: 0x04000021 RID: 33
		public float marksmanAddGunDamageMultiplier = 0.15f;

		// Token: 0x04000022 RID: 34
		public float marksmanAddGunCritRateGain = 0.3f;

		// Token: 0x04000023 RID: 35
		public float marksmanAddGunCritDamageGain = 0.5f;

		// Token: 0x04000024 RID: 36
		public float marksmanAddGunDistanceMultiplier = 0.25f;

		// Token: 0x04000025 RID: 37
		public float marksmanAddGunScatterMultiplier = -0.4f;

		// Token: 0x04000026 RID: 38
		public float marksmanAddRecoilControl = 0.3f;
	}
}
