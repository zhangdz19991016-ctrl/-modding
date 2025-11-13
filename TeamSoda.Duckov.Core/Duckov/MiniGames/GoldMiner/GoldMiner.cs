using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000294 RID: 660
	public class GoldMiner : MiniGameBehaviour
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0005015E File Offset: 0x0004E35E
		public Hook Hook
		{
			get
			{
				return this.hook;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00050166 File Offset: 0x0004E366
		public Bounds Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x0005016E File Offset: 0x0004E36E
		public int Money
		{
			get
			{
				if (this.run == null)
				{
					return 0;
				}
				return this.run.money;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x00050185 File Offset: 0x0004E385
		public ReadOnlyCollection<GoldMinerArtifact> ArtifactPrefabs
		{
			get
			{
				if (this.artifactPrefabs_ReadOnly == null)
				{
					this.artifactPrefabs_ReadOnly = new ReadOnlyCollection<GoldMinerArtifact>(this.artifactPrefabs);
				}
				return this.artifactPrefabs_ReadOnly;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x000501A6 File Offset: 0x0004E3A6
		// (set) Token: 0x06001592 RID: 5522 RVA: 0x000501B2 File Offset: 0x0004E3B2
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/GoldMiner/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/GoldMiner/HighLevel", value);
			}
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000501C0 File Offset: 0x0004E3C0
		private void Awake()
		{
			this.Hook.OnBeginRetrieve += this.OnHookBeginRetrieve;
			this.Hook.OnEndRetrieve += this.OnHookEndRetrieve;
			this.Hook.OnLaunch += this.OnHookLaunch;
			this.Hook.OnResolveTarget += this.OnHookResolveEntity;
			this.Hook.OnAttach += this.OnHookAttach;
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00050240 File Offset: 0x0004E440
		protected override void Start()
		{
			base.Start();
			this.hook.BeginSwing();
			this.Main().Forget();
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0005025E File Offset: 0x0004E45E
		internal bool PayMoney(int price)
		{
			if (this.run.money < price)
			{
				return false;
			}
			this.run.money -= price;
			return true;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00050284 File Offset: 0x0004E484
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x0005028C File Offset: 0x0004E48C
		public GoldMinerRunData run { get; private set; }

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06001598 RID: 5528 RVA: 0x00050298 File Offset: 0x0004E498
		// (remove) Token: 0x06001599 RID: 5529 RVA: 0x000502CC File Offset: 0x0004E4CC
		public static event Action<int> OnLevelClear;

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x000502FF File Offset: 0x0004E4FF
		private bool ShouldQuit
		{
			get
			{
				return this.isBeingDestroyed;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x0005030C File Offset: 0x0004E50C
		public float GlobalPriceFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00050314 File Offset: 0x0004E514
		private UniTask Main()
		{
			GoldMiner.<Main>d__51 <Main>d__;
			<Main>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Main>d__.<>4__this = this;
			<Main>d__.<>1__state = -1;
			<Main>d__.<>t__builder.Start<GoldMiner.<Main>d__51>(ref <Main>d__);
			return <Main>d__.<>t__builder.Task;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00050358 File Offset: 0x0004E558
		private UniTask DoTitleScreen()
		{
			GoldMiner.<DoTitleScreen>d__53 <DoTitleScreen>d__;
			<DoTitleScreen>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoTitleScreen>d__.<>4__this = this;
			<DoTitleScreen>d__.<>1__state = -1;
			<DoTitleScreen>d__.<>t__builder.Start<GoldMiner.<DoTitleScreen>d__53>(ref <DoTitleScreen>d__);
			return <DoTitleScreen>d__.<>t__builder.Task;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0005039C File Offset: 0x0004E59C
		private UniTask DoGameOver()
		{
			GoldMiner.<DoGameOver>d__55 <DoGameOver>d__;
			<DoGameOver>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoGameOver>d__.<>4__this = this;
			<DoGameOver>d__.<>1__state = -1;
			<DoGameOver>d__.<>t__builder.Start<GoldMiner.<DoGameOver>d__55>(ref <DoGameOver>d__);
			return <DoGameOver>d__.<>t__builder.Task;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x000503DF File Offset: 0x0004E5DF
		public void Cleanup()
		{
			if (this.run != null)
			{
				this.run.Cleanup();
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000503F4 File Offset: 0x0004E5F4
		private void GenerateLevel()
		{
			GoldMiner.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			for (int i = 0; i < this.activeEntities.Count; i++)
			{
				GoldMinerEntity goldMinerEntity = this.activeEntities[i];
				if (!(goldMinerEntity == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerEntity.gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(goldMinerEntity.gameObject);
					}
				}
			}
			this.activeEntities.Clear();
			for (int j = 0; j < this.resolvedEntities.Count; j++)
			{
				GoldMinerEntity goldMinerEntity2 = this.activeEntities[j];
				if (!(goldMinerEntity2 == null))
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(goldMinerEntity2.gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(goldMinerEntity2.gameObject);
					}
				}
			}
			this.resolvedEntities.Clear();
			int seed = this.run.levelRandom.Next();
			CS$<>8__locals1.levelGenRandom = new System.Random(seed);
			int minValue = 10;
			int maxValue = 20;
			int num = CS$<>8__locals1.levelGenRandom.Next(minValue, maxValue);
			for (int k = 0; k < num; k++)
			{
				GoldMinerEntity random = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, 0f);
				this.<GenerateLevel>g__Generate|58_0(random, ref CS$<>8__locals1);
			}
			for (float num2 = this.run.extraRocks; num2 > 0f; num2 -= 1f)
			{
				if (num2 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num2)
				{
					GoldMinerEntity random2 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Rock), 0f);
					this.<GenerateLevel>g__Generate|58_0(random2, ref CS$<>8__locals1);
				}
			}
			for (float num3 = this.run.extraGold; num3 > 0f; num3 -= 1f)
			{
				if (num3 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num3)
				{
					GoldMinerEntity random3 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Gold), 0f);
					this.<GenerateLevel>g__Generate|58_0(random3, ref CS$<>8__locals1);
				}
			}
			for (float num4 = this.run.extraDiamond; num4 > 0f; num4 -= 1f)
			{
				if (num4 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num4)
				{
					GoldMinerEntity random4 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Diamond), 0f);
					this.<GenerateLevel>g__Generate|58_0(random4, ref CS$<>8__locals1);
				}
			}
			this.run.shopRandom = new System.Random(this.run.seed + CS$<>8__locals1.levelGenRandom.Next());
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x000506C8 File Offset: 0x0004E8C8
		private Vector3 NormalizedPosToLocalPos(Vector2 posNormalized)
		{
			float x = Mathf.Lerp(this.bounds.min.x, this.bounds.max.x, posNormalized.x);
			float y = Mathf.Lerp(this.bounds.min.y, this.bounds.max.y, posNormalized.y);
			return new Vector3(x, y, 0f);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00050737 File Offset: 0x0004E937
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = this.levelLayout.localToWorldMatrix;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.extents * 2f);
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00050770 File Offset: 0x0004E970
		private UniTask Run(int seed = 0)
		{
			GoldMiner.<Run>d__61 <Run>d__;
			<Run>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.seed = seed;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<GoldMiner.<Run>d__61>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000507BC File Offset: 0x0004E9BC
		private UniTask<bool> SettleLevel()
		{
			GoldMiner.<SettleLevel>d__62 <SettleLevel>d__;
			<SettleLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SettleLevel>d__.<>4__this = this;
			<SettleLevel>d__.<>1__state = -1;
			<SettleLevel>d__.<>t__builder.Start<GoldMiner.<SettleLevel>d__62>(ref <SettleLevel>d__);
			return <SettleLevel>d__.<>t__builder.Task;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00050800 File Offset: 0x0004EA00
		private UniTask DoLevel()
		{
			GoldMiner.<DoLevel>d__65 <DoLevel>d__;
			<DoLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoLevel>d__.<>4__this = this;
			<DoLevel>d__.<>1__state = -1;
			<DoLevel>d__.<>t__builder.Start<GoldMiner.<DoLevel>d__65>(ref <DoLevel>d__);
			return <DoLevel>d__.<>t__builder.Task;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00050843 File Offset: 0x0004EA43
		protected override void OnUpdate(float deltaTime)
		{
			if (this.levelPlaying)
			{
				this.UpdateLevelPlaying(deltaTime);
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x00050854 File Offset: 0x0004EA54
		private void UpdateLevelPlaying(float deltaTime)
		{
			Action<GoldMiner> action = this.onEarlyLevelPlayTick;
			if (action != null)
			{
				action(this);
			}
			this.Hook.SetParameters(this.run.GameSpeedFactor, this.run.emptySpeed.Value, this.run.strength.Value);
			this.Hook.Tick(deltaTime);
			Hook.HookStatus status = this.Hook.Status;
			if (status != Hook.HookStatus.Swinging)
			{
				if (status == Hook.HookStatus.Retrieving)
				{
					this.run.stamina -= deltaTime * this.run.staminaDrain.Value;
				}
			}
			else if (this.launchHook)
			{
				this.Hook.Launch();
			}
			Action<GoldMiner> action2 = this.onLateLevelPlayTick;
			if (action2 != null)
			{
				action2(this);
			}
			this.launchHook = false;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0005091D File Offset: 0x0004EB1D
		public void LaunchHook()
		{
			this.launchHook = true;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00050928 File Offset: 0x0004EB28
		private bool IsLevelOver()
		{
			this.activeEntities.RemoveAll((GoldMinerEntity e) => e == null);
			return this.activeEntities.Count <= 0 || (this.hook.Status == Hook.HookStatus.Swinging && this.run.stamina <= 0f) || (this.Hook.Status == Hook.HookStatus.Retrieving && this.run.stamina < -this.run.extraStamina.Value);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x000509C4 File Offset: 0x0004EBC4
		private UniTask DoShop()
		{
			GoldMiner.<DoShop>d__71 <DoShop>d__;
			<DoShop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoShop>d__.<>4__this = this;
			<DoShop>d__.<>1__state = -1;
			<DoShop>d__.<>t__builder.Start<GoldMiner.<DoShop>d__71>(ref <DoShop>d__);
			return <DoShop>d__.<>t__builder.Task;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00050A08 File Offset: 0x0004EC08
		private void OnHookResolveEntity(Hook hook, GoldMinerEntity entity)
		{
			entity.NotifyResolved(this);
			entity.gameObject.SetActive(false);
			this.activeEntities.Remove(entity);
			this.resolvedEntities.Add(entity);
			if (this.run.IsRock(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.rockValueFactor.Value);
			}
			if (this.run.IsGold(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.goldValueFactor.Value);
			}
			this.popText.Pop(string.Format("${0}", entity.Value), hook.Axis.position);
			Action<GoldMiner, GoldMinerEntity> action = this.onResolveEntity;
			if (action != null)
			{
				action(this, entity);
			}
			Action<GoldMiner, GoldMinerEntity> action2 = this.onAfterResolveEntity;
			if (action2 == null)
			{
				return;
			}
			action2(this, entity);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00050AF3 File Offset: 0x0004ECF3
		private void OnHookBeginRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookBeginRetrieve;
			if (action == null)
			{
				return;
			}
			action(this, hook);
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00050B07 File Offset: 0x0004ED07
		private void OnHookEndRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookEndRetrieve;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.StrengthPotionActivated)
			{
				this.run.DeactivateStrengthPotion();
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00050B34 File Offset: 0x0004ED34
		private void OnHookLaunch(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookLaunch;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.EagleEyeActivated)
			{
				this.run.DeactivateEagleEye();
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00050B61 File Offset: 0x0004ED61
		private void OnHookAttach(Hook hook, GoldMinerEntity entity)
		{
			Action<GoldMiner, Hook, GoldMinerEntity> action = this.onHookAttach;
			if (action == null)
			{
				return;
			}
			action(this, hook, entity);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00050B76 File Offset: 0x0004ED76
		public bool UseStrengthPotion()
		{
			if (this.run.strengthPotion <= 0)
			{
				return false;
			}
			if (this.run.StrengthPotionActivated)
			{
				return false;
			}
			this.run.strengthPotion--;
			this.run.ActivateStrengthPotion();
			return true;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00050BB6 File Offset: 0x0004EDB6
		public bool UseEagleEyePotion()
		{
			if (this.run.eagleEyePotion <= 0)
			{
				return false;
			}
			if (this.run.EagleEyeActivated)
			{
				return false;
			}
			this.run.eagleEyePotion--;
			this.run.ActivateEagleEye();
			return true;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00050BF8 File Offset: 0x0004EDF8
		public GoldMinerArtifact GetArtifactPrefab(string id)
		{
			return this.artifactPrefabs.Find((GoldMinerArtifact e) => e != null && e.ID == id);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00050C2C File Offset: 0x0004EE2C
		internal bool UseBomb()
		{
			if (this.run.bomb <= 0)
			{
				return false;
			}
			this.run.bomb--;
			UnityEngine.Object.Instantiate<Bomb>(this.bombPrefab, this.hook.Axis.transform.position, Quaternion.FromToRotation(Vector3.up, -this.hook.Axis.transform.up), base.transform);
			return true;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00050CA8 File Offset: 0x0004EEA8
		internal void NotifyArtifactChange()
		{
			Action<GoldMiner> action = this.onArtifactChange;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x00050CBB File Offset: 0x0004EEBB
		// (set) Token: 0x060015B6 RID: 5558 RVA: 0x00050CC3 File Offset: 0x0004EEC3
		public bool isBeingDestroyed { get; private set; }

		// Token: 0x060015B7 RID: 5559 RVA: 0x00050CCC File Offset: 0x0004EECC
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00050D00 File Offset: 0x0004EF00
		[CompilerGenerated]
		private void <GenerateLevel>g__Generate|58_0(GoldMinerEntity entityPrefab, ref GoldMiner.<>c__DisplayClass58_0 A_2)
		{
			if (entityPrefab == null)
			{
				return;
			}
			Vector2 posNormalized = new Vector2((float)A_2.levelGenRandom.NextDouble(), (float)A_2.levelGenRandom.NextDouble());
			GoldMinerEntity goldMinerEntity = UnityEngine.Object.Instantiate<GoldMinerEntity>(entityPrefab, this.levelLayout);
			Vector3 localPosition = this.NormalizedPosToLocalPos(posNormalized);
			Quaternion localRotation = Quaternion.AngleAxis((float)A_2.levelGenRandom.NextDouble() * 360f, Vector3.forward);
			goldMinerEntity.transform.localPosition = localPosition;
			goldMinerEntity.transform.localRotation = localRotation;
			goldMinerEntity.SetMaster(this);
			this.activeEntities.Add(goldMinerEntity);
		}

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		private Hook hook;

		// Token: 0x04000FDE RID: 4062
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x04000FDF RID: 4063
		[SerializeField]
		private LevelSettlementUI settlementUI;

		// Token: 0x04000FE0 RID: 4064
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04000FE1 RID: 4065
		[SerializeField]
		private GameObject gameoverScreen;

		// Token: 0x04000FE2 RID: 4066
		[SerializeField]
		private GoldMiner_PopText popText;

		// Token: 0x04000FE3 RID: 4067
		[SerializeField]
		private Transform levelLayout;

		// Token: 0x04000FE4 RID: 4068
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04000FE5 RID: 4069
		[SerializeField]
		private Bomb bombPrefab;

		// Token: 0x04000FE6 RID: 4070
		[SerializeField]
		private RandomContainer<GoldMinerEntity> entities;

		// Token: 0x04000FE7 RID: 4071
		[SerializeField]
		private List<GoldMinerArtifact> artifactPrefabs = new List<GoldMinerArtifact>();

		// Token: 0x04000FE8 RID: 4072
		private ReadOnlyCollection<GoldMinerArtifact> artifactPrefabs_ReadOnly;

		// Token: 0x04000FE9 RID: 4073
		public Action<GoldMiner> onLevelBegin;

		// Token: 0x04000FEA RID: 4074
		public Action<GoldMiner> onLevelEnd;

		// Token: 0x04000FEB RID: 4075
		public Action<GoldMiner> onShopBegin;

		// Token: 0x04000FEC RID: 4076
		public Action<GoldMiner> onShopEnd;

		// Token: 0x04000FED RID: 4077
		public Action<GoldMiner> onEarlyLevelPlayTick;

		// Token: 0x04000FEE RID: 4078
		public Action<GoldMiner> onLateLevelPlayTick;

		// Token: 0x04000FEF RID: 4079
		public Action<GoldMiner, Hook> onHookLaunch;

		// Token: 0x04000FF0 RID: 4080
		public Action<GoldMiner, Hook> onHookBeginRetrieve;

		// Token: 0x04000FF1 RID: 4081
		public Action<GoldMiner, Hook> onHookEndRetrieve;

		// Token: 0x04000FF2 RID: 4082
		public Action<GoldMiner, Hook, GoldMinerEntity> onHookAttach;

		// Token: 0x04000FF3 RID: 4083
		public Action<GoldMiner, GoldMinerEntity> onResolveEntity;

		// Token: 0x04000FF4 RID: 4084
		public Action<GoldMiner, GoldMinerEntity> onAfterResolveEntity;

		// Token: 0x04000FF5 RID: 4085
		public Action<GoldMiner> onArtifactChange;

		// Token: 0x04000FF6 RID: 4086
		private const string HighLevelSaveKey = "MiniGame/GoldMiner/HighLevel";

		// Token: 0x04000FF9 RID: 4089
		private bool titleConfirmed;

		// Token: 0x04000FFA RID: 4090
		private bool gameOverConfirmed;

		// Token: 0x04000FFB RID: 4091
		public List<GoldMinerEntity> activeEntities = new List<GoldMinerEntity>();

		// Token: 0x04000FFC RID: 4092
		private bool levelPlaying;

		// Token: 0x04000FFD RID: 4093
		public List<GoldMinerEntity> resolvedEntities = new List<GoldMinerEntity>();

		// Token: 0x04000FFE RID: 4094
		private bool launchHook;
	}
}
