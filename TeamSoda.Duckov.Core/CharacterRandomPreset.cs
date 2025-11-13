using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200008F RID: 143
[CreateAssetMenu(fileName = "New Character Random Preset", menuName = "Character Random Preset", order = 51)]
public class CharacterRandomPreset : ScriptableObject
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00016697 File Offset: 0x00014897
	public string Name
	{
		get
		{
			return this.nameKey.ToPlainText();
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000166A4 File Offset: 0x000148A4
	public string DisplayName
	{
		get
		{
			return this.nameKey.ToPlainText();
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x000166B1 File Offset: 0x000148B1
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000166C0 File Offset: 0x000148C0
	public Sprite GetCharacterIcon()
	{
		switch (this.characterIconType)
		{
		case CharacterIconTypes.none:
			return null;
		case CharacterIconTypes.elete:
			return GameplayDataSettings.UIStyle.EleteCharacterIcon;
		case CharacterIconTypes.pmc:
			return GameplayDataSettings.UIStyle.PmcCharacterIcon;
		case CharacterIconTypes.boss:
			return GameplayDataSettings.UIStyle.BossCharacterIcon;
		case CharacterIconTypes.merchant:
			return GameplayDataSettings.UIStyle.MerchantCharacterIcon;
		case CharacterIconTypes.pet:
			return GameplayDataSettings.UIStyle.PetCharacterIcon;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00016734 File Offset: 0x00014934
	public UniTask<CharacterMainControl> CreateCharacterAsync(Vector3 pos, Vector3 dir, int relatedScene, CharacterSpawnerGroup group, bool isLeader)
	{
		CharacterRandomPreset.<CreateCharacterAsync>d__81 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.pos = pos;
		<CreateCharacterAsync>d__.dir = dir;
		<CreateCharacterAsync>d__.relatedScene = relatedScene;
		<CreateCharacterAsync>d__.group = group;
		<CreateCharacterAsync>d__.isLeader = isLeader;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<CharacterRandomPreset.<CreateCharacterAsync>d__81>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000167A4 File Offset: 0x000149A4
	private UniTask<List<Item>> GenerateItems()
	{
		CharacterRandomPreset.<GenerateItems>d__82 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<CharacterRandomPreset.<GenerateItems>d__82>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000167E8 File Offset: 0x000149E8
	private UniTask AddBullet(CharacterMainControl character)
	{
		CharacterRandomPreset.<AddBullet>d__83 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.character = character;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<CharacterRandomPreset.<AddBullet>d__83>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00016A58 File Offset: 0x00014C58
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__SetCharacterStat|81_0(string statName, float value, ref CharacterRandomPreset.<>c__DisplayClass81_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue = value;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00016A84 File Offset: 0x00014C84
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__MultiplyCharacterStat|81_1(string statName, float multiplier, ref CharacterRandomPreset.<>c__DisplayClass81_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue *= multiplier;
	}

	// Token: 0x04000431 RID: 1073
	[LocalizationKey("Characters")]
	public string nameKey;

	// Token: 0x04000432 RID: 1074
	public AudioManager.VoiceType voiceType;

	// Token: 0x04000433 RID: 1075
	public AudioManager.FootStepMaterialType footstepMaterialType;

	// Token: 0x04000434 RID: 1076
	public InteractableLootbox lootBoxPrefab;

	// Token: 0x04000435 RID: 1077
	public List<AISpecialAttachmentBase> specialAttachmentBases;

	// Token: 0x04000436 RID: 1078
	public Teams team = Teams.scav;

	// Token: 0x04000437 RID: 1079
	public bool showName;

	// Token: 0x04000438 RID: 1080
	[FormerlySerializedAs("iconType")]
	[SerializeField]
	private CharacterIconTypes characterIconType;

	// Token: 0x04000439 RID: 1081
	public float health;

	// Token: 0x0400043A RID: 1082
	public bool hasSoul = true;

	// Token: 0x0400043B RID: 1083
	public bool showHealthBar = true;

	// Token: 0x0400043C RID: 1084
	public int exp = 100;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	private bool usePlayerPreset;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private CustomFacePreset facePreset;

	// Token: 0x04000440 RID: 1088
	[SerializeField]
	private AICharacterController aiController;

	// Token: 0x04000441 RID: 1089
	public bool setActiveByPlayerDistance = true;

	// Token: 0x04000442 RID: 1090
	public float forceTracePlayerDistance;

	// Token: 0x04000443 RID: 1091
	public bool shootCanMove;

	// Token: 0x04000444 RID: 1092
	public float sightDistance = 17f;

	// Token: 0x04000445 RID: 1093
	public float sightAngle = 100f;

	// Token: 0x04000446 RID: 1094
	public float reactionTime = 0.2f;

	// Token: 0x04000447 RID: 1095
	public float nightReactionTimeFactor = 1.5f;

	// Token: 0x04000448 RID: 1096
	public float shootDelay = 0.2f;

	// Token: 0x04000449 RID: 1097
	public Vector2 shootTimeRange = new Vector2(0.4f, 1.5f);

	// Token: 0x0400044A RID: 1098
	public Vector2 shootTimeSpaceRange = new Vector2(2f, 3f);

	// Token: 0x0400044B RID: 1099
	public Vector2 combatMoveTimeRange = new Vector2(1f, 3f);

	// Token: 0x0400044C RID: 1100
	public float hearingAbility = 1f;

	// Token: 0x0400044D RID: 1101
	public float patrolRange = 8f;

	// Token: 0x0400044E RID: 1102
	[FormerlySerializedAs("combatRange")]
	public float combatMoveRange = 8f;

	// Token: 0x0400044F RID: 1103
	public bool canDash;

	// Token: 0x04000450 RID: 1104
	public Vector2 dashCoolTimeRange = new Vector2(2f, 4f);

	// Token: 0x04000451 RID: 1105
	[Range(0f, 1f)]
	public float minTraceTargetChance = 1f;

	// Token: 0x04000452 RID: 1106
	[Range(0f, 1f)]
	public float maxTraceTargetChance = 1f;

	// Token: 0x04000453 RID: 1107
	public float forgetTime = 8f;

	// Token: 0x04000454 RID: 1108
	public bool defaultWeaponOut = true;

	// Token: 0x04000455 RID: 1109
	public bool canTalk = true;

	// Token: 0x04000456 RID: 1110
	public float patrolTurnSpeed = 180f;

	// Token: 0x04000457 RID: 1111
	public float combatTurnSpeed = 1200f;

	// Token: 0x04000458 RID: 1112
	[ItemTypeID]
	public int wantItem = -1;

	// Token: 0x04000459 RID: 1113
	public float moveSpeedFactor = 1f;

	// Token: 0x0400045A RID: 1114
	public float bulletSpeedMultiplier = 1f;

	// Token: 0x0400045B RID: 1115
	[Range(1f, 2f)]
	public float gunDistanceMultiplier = 1f;

	// Token: 0x0400045C RID: 1116
	public float nightVisionAbility = 0.5f;

	// Token: 0x0400045D RID: 1117
	public float gunScatterMultiplier = 1f;

	// Token: 0x0400045E RID: 1118
	public float scatterMultiIfTargetRunning = 3f;

	// Token: 0x0400045F RID: 1119
	public float scatterMultiIfOffScreen = 4f;

	// Token: 0x04000460 RID: 1120
	[FormerlySerializedAs("gunDamageMultiplier")]
	public float damageMultiplier = 1f;

	// Token: 0x04000461 RID: 1121
	public float gunCritRateGain;

	// Token: 0x04000462 RID: 1122
	[Tooltip("用来决定双方造成伤害缩放")]
	public float aiCombatFactor = 1f;

	// Token: 0x04000463 RID: 1123
	public bool hasSkill;

	// Token: 0x04000464 RID: 1124
	public SkillBase skillPfb;

	// Token: 0x04000465 RID: 1125
	[Range(0.01f, 1f)]
	public float hasSkillChance = 1f;

	// Token: 0x04000466 RID: 1126
	public Vector2 skillCoolTimeRange = Vector2.one;

	// Token: 0x04000467 RID: 1127
	[Range(0.01f, 1f)]
	public float skillSuccessChance = 1f;

	// Token: 0x04000468 RID: 1128
	private float tryReleaseSkillTimeMarker = -1f;

	// Token: 0x04000469 RID: 1129
	[Range(0f, 1f)]
	public float itemSkillChance = 0.3f;

	// Token: 0x0400046A RID: 1130
	public float itemSkillCoolTime = 6f;

	// Token: 0x0400046B RID: 1131
	public List<Buff> buffs;

	// Token: 0x0400046C RID: 1132
	public List<Buff.BuffExclusiveTags> buffResist;

	// Token: 0x0400046D RID: 1133
	public float elementFactor_Physics = 1f;

	// Token: 0x0400046E RID: 1134
	public float elementFactor_Fire = 1f;

	// Token: 0x0400046F RID: 1135
	public float elementFactor_Poison = 1f;

	// Token: 0x04000470 RID: 1136
	public float elementFactor_Electricity = 1f;

	// Token: 0x04000471 RID: 1137
	public float elementFactor_Space = 1f;

	// Token: 0x04000472 RID: 1138
	public float elementFactor_Ghost = 1f;

	// Token: 0x04000473 RID: 1139
	[SerializeField]
	private List<CharacterRandomPreset.SetCharacterStatInfo> setStats;

	// Token: 0x04000474 RID: 1140
	[Range(0f, 1f)]
	public float hasCashChance;

	// Token: 0x04000475 RID: 1141
	public Vector2Int cashRange;

	// Token: 0x04000476 RID: 1142
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x04000477 RID: 1143
	[Space(12f)]
	[SerializeField]
	private RandomContainer<int> bulletQualityDistribution;

	// Token: 0x04000478 RID: 1144
	[SerializeField]
	private Tag[] bulletExclusiveTags;

	// Token: 0x04000479 RID: 1145
	[HideInInspector]
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x0400047A RID: 1146
	[SerializeField]
	private Vector2 bulletCountRange = Vector2.one;

	// Token: 0x02000449 RID: 1097
	[Serializable]
	private struct SetCharacterStatInfo
	{
		// Token: 0x04001ABC RID: 6844
		public string statName;

		// Token: 0x04001ABD RID: 6845
		public Vector2 statBaseValue;
	}
}
