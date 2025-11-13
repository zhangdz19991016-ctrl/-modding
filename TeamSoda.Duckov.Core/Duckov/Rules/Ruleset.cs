using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003F9 RID: 1017
	[Serializable]
	public class Ruleset
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x00080641 File Offset: 0x0007E841
		// (set) Token: 0x060024D4 RID: 9428 RVA: 0x00080653 File Offset: 0x0007E853
		[LocalizationKey("UIText")]
		internal string descriptionKey
		{
			get
			{
				return this.displayNameKey + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060024D5 RID: 9429 RVA: 0x00080655 File Offset: 0x0007E855
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x00080662 File Offset: 0x0007E862
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0008066F File Offset: 0x0007E86F
		public bool SpawnDeadBody
		{
			get
			{
				return this.spawnDeadBody;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x00080677 File Offset: 0x0007E877
		public int SaveDeadbodyCount
		{
			get
			{
				return this.saveDeadbodyCount;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x0008067F File Offset: 0x0007E87F
		public bool FogOfWar
		{
			get
			{
				return this.fogOfWar;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x00080687 File Offset: 0x0007E887
		public bool AdvancedDebuffMode
		{
			get
			{
				return this.advancedDebuffMode;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x0008068F File Offset: 0x0007E88F
		public float RecoilMultiplier
		{
			get
			{
				return this.recoilMultiplier;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x00080697 File Offset: 0x0007E897
		public float DamageFactor_ToPlayer
		{
			get
			{
				return this.damageFactor_ToPlayer;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0008069F File Offset: 0x0007E89F
		public float EnemyHealthFactor
		{
			get
			{
				return this.enemyHealthFactor;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000806A7 File Offset: 0x0007E8A7
		public float EnemyReactionTimeFactor
		{
			get
			{
				return this.enemyReactionTimeFactor;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x000806AF File Offset: 0x0007E8AF
		public float EnemyAttackTimeSpaceFactor
		{
			get
			{
				return this.enemyAttackTimeSpaceFactor;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x000806B7 File Offset: 0x0007E8B7
		public float EnemyAttackTimeFactor
		{
			get
			{
				return this.enemyAttackTimeFactor;
			}
		}

		// Token: 0x0400190A RID: 6410
		[LocalizationKey("UIText")]
		[SerializeField]
		internal string displayNameKey;

		// Token: 0x0400190B RID: 6411
		[SerializeField]
		private float damageFactor_ToPlayer = 1f;

		// Token: 0x0400190C RID: 6412
		[SerializeField]
		private float enemyHealthFactor = 1f;

		// Token: 0x0400190D RID: 6413
		[SerializeField]
		private bool spawnDeadBody = true;

		// Token: 0x0400190E RID: 6414
		[SerializeField]
		private bool fogOfWar = true;

		// Token: 0x0400190F RID: 6415
		[SerializeField]
		private bool advancedDebuffMode;

		// Token: 0x04001910 RID: 6416
		[SerializeField]
		private int saveDeadbodyCount = 1;

		// Token: 0x04001911 RID: 6417
		[Range(0f, 1f)]
		[SerializeField]
		private float recoilMultiplier = 1f;

		// Token: 0x04001912 RID: 6418
		[SerializeField]
		internal float enemyReactionTimeFactor = 1f;

		// Token: 0x04001913 RID: 6419
		[SerializeField]
		internal float enemyAttackTimeSpaceFactor = 1f;

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		internal float enemyAttackTimeFactor = 1f;
	}
}
