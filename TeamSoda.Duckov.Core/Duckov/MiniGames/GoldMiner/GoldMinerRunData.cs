using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000293 RID: 659
	[Serializable]
	public class GoldMinerRunData
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x0004FB99 File Offset: 0x0004DD99
		// (set) Token: 0x06001570 RID: 5488 RVA: 0x0004FBA1 File Offset: 0x0004DDA1
		public int seed { get; private set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x0004FBAA File Offset: 0x0004DDAA
		// (set) Token: 0x06001572 RID: 5490 RVA: 0x0004FBB2 File Offset: 0x0004DDB2
		public System.Random shopRandom { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0004FBBB File Offset: 0x0004DDBB
		// (set) Token: 0x06001574 RID: 5492 RVA: 0x0004FBC3 File Offset: 0x0004DDC3
		public System.Random levelRandom { get; private set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x0004FBCC File Offset: 0x0004DDCC
		public float GameSpeedFactor
		{
			get
			{
				return this.gameSpeedFactor.Value;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x0004FBD9 File Offset: 0x0004DDD9
		// (set) Token: 0x06001577 RID: 5495 RVA: 0x0004FBE1 File Offset: 0x0004DDE1
		public float stamina { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0004FBEA File Offset: 0x0004DDEA
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x0004FBF2 File Offset: 0x0004DDF2
		public bool gameOver { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x0004FBFB File Offset: 0x0004DDFB
		// (set) Token: 0x0600157B RID: 5499 RVA: 0x0004FC03 File Offset: 0x0004DE03
		public int level { get; set; }

		// Token: 0x0600157C RID: 5500 RVA: 0x0004FC0C File Offset: 0x0004DE0C
		public GoldMinerArtifact AttachArtifactFromPrefab(GoldMinerArtifact prefab)
		{
			if (prefab == null)
			{
				return null;
			}
			GoldMinerArtifact goldMinerArtifact = UnityEngine.Object.Instantiate<GoldMinerArtifact>(prefab, this.master.transform);
			this.AttachArtifact(goldMinerArtifact);
			return goldMinerArtifact;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0004FC40 File Offset: 0x0004DE40
		private void AttachArtifact(GoldMinerArtifact artifact)
		{
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]++;
			}
			else
			{
				this.artifactCount[artifact.ID] = 1;
			}
			this.artifacts.Add(artifact);
			artifact.Attach(this.master);
			this.master.NotifyArtifactChange();
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0004FCB8 File Offset: 0x0004DEB8
		public bool DetachArtifact(GoldMinerArtifact artifact)
		{
			bool result = this.artifacts.Remove(artifact);
			artifact.Detatch(this.master);
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]--;
			}
			else
			{
				Debug.LogError("Artifact counter error.", this.master);
			}
			this.master.NotifyArtifactChange();
			return result;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0004FD2C File Offset: 0x0004DF2C
		public int GetArtifactCount(string id)
		{
			int result;
			if (this.artifactCount.TryGetValue(id, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0004FD4C File Offset: 0x0004DF4C
		public GoldMinerRunData(GoldMiner master, int seed = 0)
		{
			this.master = master;
			if (seed == 0)
			{
				seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			this.seed = seed;
			this.levelRandom = new System.Random(seed);
			this.strengthPotionModifier = new Modifier(ModifierType.Add, 100f, this);
			this.eagleEyeModifier = new Modifier(ModifierType.PercentageMultiply, -0.5f, this);
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x0004FF5B File Offset: 0x0004E15B
		// (set) Token: 0x06001582 RID: 5506 RVA: 0x0004FF63 File Offset: 0x0004E163
		public bool StrengthPotionActivated { get; private set; }

		// Token: 0x06001583 RID: 5507 RVA: 0x0004FF6C File Offset: 0x0004E16C
		public void ActivateStrengthPotion()
		{
			if (this.StrengthPotionActivated)
			{
				return;
			}
			this.strength.AddModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = true;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0004FF8F File Offset: 0x0004E18F
		public void DeactivateStrengthPotion()
		{
			this.strength.RemoveModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = false;
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x0004FFAA File Offset: 0x0004E1AA
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x0004FFB2 File Offset: 0x0004E1B2
		public bool EagleEyeActivated { get; private set; }

		// Token: 0x06001587 RID: 5511 RVA: 0x0004FFBB File Offset: 0x0004E1BB
		public void ActivateEagleEye()
		{
			if (this.EagleEyeActivated)
			{
				return;
			}
			this.gameSpeedFactor.AddModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = true;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0004FFDE File Offset: 0x0004E1DE
		public void DeactivateEagleEye()
		{
			this.gameSpeedFactor.RemoveModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = false;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0004FFFC File Offset: 0x0004E1FC
		internal void Cleanup()
		{
			foreach (GoldMinerArtifact goldMinerArtifact in this.artifacts)
			{
				if (!(goldMinerArtifact == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerArtifact.gameObject);
					}
					else
					{
						UnityEngine.Object.Destroy(goldMinerArtifact.gameObject);
					}
				}
			}
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00050070 File Offset: 0x0004E270
		public bool IsGold(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Gold);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000500E0 File Offset: 0x0004E2E0
		public bool IsRock(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00050150 File Offset: 0x0004E350
		internal bool IsPig(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Pig);
		}

		// Token: 0x04000FAE RID: 4014
		public readonly GoldMiner master;

		// Token: 0x04000FB2 RID: 4018
		public int money;

		// Token: 0x04000FB3 RID: 4019
		public int bomb;

		// Token: 0x04000FB4 RID: 4020
		public int strengthPotion;

		// Token: 0x04000FB5 RID: 4021
		public int eagleEyePotion;

		// Token: 0x04000FB6 RID: 4022
		public int shopTicket;

		// Token: 0x04000FB7 RID: 4023
		public const int shopDefaultItemAmount = 3;

		// Token: 0x04000FB8 RID: 4024
		public const int shopMaxItemAmount = 3;

		// Token: 0x04000FB9 RID: 4025
		public int shopCapacity = 3;

		// Token: 0x04000FBA RID: 4026
		public float levelScoreFactor;

		// Token: 0x04000FBB RID: 4027
		public Stat maxStamina = new Stat("maxStamina", 15f, false);

		// Token: 0x04000FBC RID: 4028
		public Stat extraStamina = new Stat("extraStamina", 2f, false);

		// Token: 0x04000FBD RID: 4029
		public Stat staminaDrain = new Stat("staminaDrain", 1f, false);

		// Token: 0x04000FBE RID: 4030
		public Stat gameSpeedFactor = new Stat("gameSpeedFactor", 1f, false);

		// Token: 0x04000FBF RID: 4031
		public Stat emptySpeed = new Stat("emptySpeed", 300f, false);

		// Token: 0x04000FC0 RID: 4032
		public Stat strength = new Stat("strength", 0f, false);

		// Token: 0x04000FC1 RID: 4033
		public Stat scoreFactorBase = new Stat("scoreFactor", 1f, false);

		// Token: 0x04000FC2 RID: 4034
		public Stat rockValueFactor = new Stat("rockValueFactor", 1f, false);

		// Token: 0x04000FC3 RID: 4035
		public Stat goldValueFactor = new Stat("goldValueFactor", 1f, false);

		// Token: 0x04000FC4 RID: 4036
		public Stat charm = new Stat("charm", 1f, false);

		// Token: 0x04000FC5 RID: 4037
		public Stat shopRefreshPrice = new Stat("shopRefreshPrice", 100f, false);

		// Token: 0x04000FC6 RID: 4038
		public Stat shopRefreshPriceIncrement = new Stat("shopRefreshPriceIncrement", 50f, false);

		// Token: 0x04000FC7 RID: 4039
		public Stat shopRefreshChances = new Stat("shopRefreshChances", 2f, false);

		// Token: 0x04000FC8 RID: 4040
		public Stat shopPriceCut = new Stat("shopPriceCut", 0.7f, false);

		// Token: 0x04000FC9 RID: 4041
		public Stat defuse = new Stat("defuse", 0f, false);

		// Token: 0x04000FCA RID: 4042
		public float extraRocks;

		// Token: 0x04000FCB RID: 4043
		public float extraGold;

		// Token: 0x04000FCC RID: 4044
		public float extraDiamond;

		// Token: 0x04000FCD RID: 4045
		public List<GoldMinerArtifact> artifacts = new List<GoldMinerArtifact>();

		// Token: 0x04000FD1 RID: 4049
		private Dictionary<string, int> artifactCount = new Dictionary<string, int>();

		// Token: 0x04000FD2 RID: 4050
		private Modifier strengthPotionModifier;

		// Token: 0x04000FD4 RID: 4052
		private Modifier eagleEyeModifier;

		// Token: 0x04000FD5 RID: 4053
		internal int targetScore = 100;

		// Token: 0x04000FD7 RID: 4055
		public List<Func<GoldMinerEntity, bool>> isGoldPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x04000FD8 RID: 4056
		public List<Func<GoldMinerEntity, bool>> isRockPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x04000FD9 RID: 4057
		public List<Func<float>> additionalFactorFuncs = new List<Func<float>>();

		// Token: 0x04000FDA RID: 4058
		public List<Func<int, int>> settleValueProcessor = new List<Func<int, int>>();

		// Token: 0x04000FDB RID: 4059
		public List<Func<bool>> forceLevelSuccessFuncs = new List<Func<bool>>();

		// Token: 0x04000FDC RID: 4060
		internal int minMoneySum;
	}
}
