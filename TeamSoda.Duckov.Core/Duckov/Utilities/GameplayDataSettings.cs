using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Duckov.Achievements;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Crops;
using Duckov.Quests;
using Duckov.Quests.Relations;
using Duckov.UI;
using Eflatun.SceneReference;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Duckov.Utilities
{
	// Token: 0x020003FF RID: 1023
	[CreateAssetMenu(menuName = "Settings/Gameplay Data Settings")]
	public class GameplayDataSettings : ScriptableObject
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00080F77 File Offset: 0x0007F177
		private static GameplayDataSettings Default
		{
			get
			{
				if (GameplayDataSettings.cachedDefault == null)
				{
					GameplayDataSettings.cachedDefault = Resources.Load<GameplayDataSettings>("GameplayDataSettings");
				}
				return GameplayDataSettings.cachedDefault;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x00080F9A File Offset: 0x0007F19A
		public static InputActionAsset InputActions
		{
			get
			{
				return GameplayDataSettings.Default.inputActions;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x00080FA6 File Offset: 0x0007F1A6
		public static CustomFaceData CustomFaceData
		{
			get
			{
				return GameplayDataSettings.Default.customFaceData;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x00080FB2 File Offset: 0x0007F1B2
		public static GameplayDataSettings.TagsData Tags
		{
			get
			{
				return GameplayDataSettings.Default.tags;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00080FBE File Offset: 0x0007F1BE
		public static GameplayDataSettings.PrefabsData Prefabs
		{
			get
			{
				return GameplayDataSettings.Default.prefabs;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x00080FCA File Offset: 0x0007F1CA
		public static UIPrefabsReference UIPrefabs
		{
			get
			{
				return GameplayDataSettings.Default.uiPrefabs;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x00080FD6 File Offset: 0x0007F1D6
		public static GameplayDataSettings.ItemAssetsData ItemAssets
		{
			get
			{
				return GameplayDataSettings.Default.itemAssets;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x00080FE2 File Offset: 0x0007F1E2
		public static GameplayDataSettings.StringListsData StringLists
		{
			get
			{
				return GameplayDataSettings.Default.stringLists;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x00080FEE File Offset: 0x0007F1EE
		public static GameplayDataSettings.LayersData Layers
		{
			get
			{
				return GameplayDataSettings.Default.layers;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x00080FFA File Offset: 0x0007F1FA
		public static GameplayDataSettings.SceneManagementData SceneManagement
		{
			get
			{
				return GameplayDataSettings.Default.sceneManagement;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x00081006 File Offset: 0x0007F206
		public static GameplayDataSettings.BuffsData Buffs
		{
			get
			{
				return GameplayDataSettings.Default.buffs;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x00081012 File Offset: 0x0007F212
		public static GameplayDataSettings.QuestsData Quests
		{
			get
			{
				return GameplayDataSettings.Default.quests;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x0008101E File Offset: 0x0007F21E
		public static QuestCollection QuestCollection
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestCollection;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x0008102F File Offset: 0x0007F22F
		public static QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestRelation;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x00081040 File Offset: 0x0007F240
		public static GameplayDataSettings.EconomyData Economy
		{
			get
			{
				return GameplayDataSettings.Default.economyData;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600252C RID: 9516 RVA: 0x0008104C File Offset: 0x0007F24C
		public static GameplayDataSettings.UIStyleData UIStyle
		{
			get
			{
				return GameplayDataSettings.Default.uiStyleData;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x00081058 File Offset: 0x0007F258
		public static BuildingDataCollection BuildingDataCollection
		{
			get
			{
				return GameplayDataSettings.Default.buildingDataCollection;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x00081064 File Offset: 0x0007F264
		public static CraftingFormulaCollection CraftingFormulas
		{
			get
			{
				return GameplayDataSettings.Default.craftingFormulas;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x00081070 File Offset: 0x0007F270
		public static DecomposeDatabase DecomposeDatabase
		{
			get
			{
				return GameplayDataSettings.Default.decomposeDatabase;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x0008107C File Offset: 0x0007F27C
		public static StatInfoDatabase StatInfo
		{
			get
			{
				return GameplayDataSettings.Default.statInfo;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x00081088 File Offset: 0x0007F288
		public static StockShopDatabase StockshopDatabase
		{
			get
			{
				return GameplayDataSettings.Default.stockShopDatabase;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x00081094 File Offset: 0x0007F294
		public static GameplayDataSettings.LootingData Looting
		{
			get
			{
				return GameplayDataSettings.Default.looting;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x000810A0 File Offset: 0x0007F2A0
		public static AchievementDatabase AchievementDatabase
		{
			get
			{
				return GameplayDataSettings.Default.achivementDatabase;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x000810AC File Offset: 0x0007F2AC
		public static CropDatabase CropDatabase
		{
			get
			{
				return GameplayDataSettings.Default.cropDatabase;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x000810B8 File Offset: 0x0007F2B8
		public static GameplayDataSettings.CharacterRandomPresets CharacterRandomPresetData
		{
			get
			{
				return GameplayDataSettings.Default.characterRandomPresets;
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000810C4 File Offset: 0x0007F2C4
		internal static Sprite GetSprite(string key)
		{
			return GameplayDataSettings.Default.spriteData.GetSprite(key);
		}

		// Token: 0x04001935 RID: 6453
		private static GameplayDataSettings cachedDefault;

		// Token: 0x04001936 RID: 6454
		[SerializeField]
		private GameplayDataSettings.TagsData tags;

		// Token: 0x04001937 RID: 6455
		[SerializeField]
		private GameplayDataSettings.PrefabsData prefabs;

		// Token: 0x04001938 RID: 6456
		[SerializeField]
		private UIPrefabsReference uiPrefabs;

		// Token: 0x04001939 RID: 6457
		[SerializeField]
		private GameplayDataSettings.ItemAssetsData itemAssets;

		// Token: 0x0400193A RID: 6458
		[SerializeField]
		private GameplayDataSettings.StringListsData stringLists;

		// Token: 0x0400193B RID: 6459
		[SerializeField]
		private GameplayDataSettings.LayersData layers;

		// Token: 0x0400193C RID: 6460
		[SerializeField]
		private GameplayDataSettings.SceneManagementData sceneManagement;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private GameplayDataSettings.BuffsData buffs;

		// Token: 0x0400193E RID: 6462
		[SerializeField]
		private GameplayDataSettings.QuestsData quests;

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private GameplayDataSettings.EconomyData economyData;

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private GameplayDataSettings.UIStyleData uiStyleData;

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private InputActionAsset inputActions;

		// Token: 0x04001942 RID: 6466
		[SerializeField]
		private BuildingDataCollection buildingDataCollection;

		// Token: 0x04001943 RID: 6467
		[SerializeField]
		private CustomFaceData customFaceData;

		// Token: 0x04001944 RID: 6468
		[SerializeField]
		private CraftingFormulaCollection craftingFormulas;

		// Token: 0x04001945 RID: 6469
		[SerializeField]
		private DecomposeDatabase decomposeDatabase;

		// Token: 0x04001946 RID: 6470
		[SerializeField]
		private StatInfoDatabase statInfo;

		// Token: 0x04001947 RID: 6471
		[SerializeField]
		private StockShopDatabase stockShopDatabase;

		// Token: 0x04001948 RID: 6472
		[SerializeField]
		private GameplayDataSettings.LootingData looting;

		// Token: 0x04001949 RID: 6473
		[SerializeField]
		private AchievementDatabase achivementDatabase;

		// Token: 0x0400194A RID: 6474
		[SerializeField]
		private CropDatabase cropDatabase;

		// Token: 0x0400194B RID: 6475
		[SerializeField]
		private GameplayDataSettings.SpritesData spriteData;

		// Token: 0x0400194C RID: 6476
		[SerializeField]
		private GameplayDataSettings.CharacterRandomPresets characterRandomPresets;

		// Token: 0x02000660 RID: 1632
		[Serializable]
		public class LootingData
		{
			// Token: 0x06002AA9 RID: 10921 RVA: 0x000A227C File Offset: 0x000A047C
			public float MGetInspectingTime(Item item)
			{
				int num = item.Quality;
				if (num < 0)
				{
					num = 0;
				}
				if (num >= this.inspectingTimes.Length)
				{
					num = this.inspectingTimes.Length - 1;
				}
				return this.inspectingTimes[num];
			}

			// Token: 0x06002AAA RID: 10922 RVA: 0x000A22B4 File Offset: 0x000A04B4
			public static float GetInspectingTime(Item item)
			{
				GameplayDataSettings.LootingData looting = GameplayDataSettings.Looting;
				if (looting == null)
				{
					return 1f;
				}
				return looting.MGetInspectingTime(item);
			}

			// Token: 0x040022F1 RID: 8945
			public float[] inspectingTimes;
		}

		// Token: 0x02000661 RID: 1633
		[Serializable]
		public class TagsData
		{
			// Token: 0x170007A4 RID: 1956
			// (get) Token: 0x06002AAC RID: 10924 RVA: 0x000A22DF File Offset: 0x000A04DF
			public Tag Character
			{
				get
				{
					return this.character;
				}
			}

			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x06002AAD RID: 10925 RVA: 0x000A22E7 File Offset: 0x000A04E7
			public Tag LockInDemoTag
			{
				get
				{
					return this.lockInDemoTag;
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x06002AAE RID: 10926 RVA: 0x000A22EF File Offset: 0x000A04EF
			public Tag Helmat
			{
				get
				{
					return this.helmat;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x06002AAF RID: 10927 RVA: 0x000A22F7 File Offset: 0x000A04F7
			public Tag Armor
			{
				get
				{
					return this.armor;
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x000A22FF File Offset: 0x000A04FF
			public Tag Backpack
			{
				get
				{
					return this.backpack;
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x000A2307 File Offset: 0x000A0507
			public Tag Bullet
			{
				get
				{
					return this.bullet;
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000A230F File Offset: 0x000A050F
			public Tag Bait
			{
				get
				{
					return this.bait;
				}
			}

			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000A2317 File Offset: 0x000A0517
			public Tag AdvancedDebuffMode
			{
				get
				{
					return this.advancedDebuffMode;
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000A231F File Offset: 0x000A051F
			public Tag Special
			{
				get
				{
					return this.special;
				}
			}

			// Token: 0x170007AD RID: 1965
			// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000A2327 File Offset: 0x000A0527
			public Tag DestroyOnLootBox
			{
				get
				{
					return this.destroyOnLootBox;
				}
			}

			// Token: 0x170007AE RID: 1966
			// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000A232F File Offset: 0x000A052F
			public Tag DontDropOnDeadInSlot
			{
				get
				{
					return this.dontDropOnDeadInSlot;
				}
			}

			// Token: 0x170007AF RID: 1967
			// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000A2337 File Offset: 0x000A0537
			public ReadOnlyCollection<Tag> AllTags
			{
				get
				{
					if (this.tagsReadOnly == null)
					{
						this.tagsReadOnly = this.allTags.AsReadOnly();
					}
					return this.tagsReadOnly;
				}
			}

			// Token: 0x170007B0 RID: 1968
			// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x000A2358 File Offset: 0x000A0558
			public Tag Gun
			{
				get
				{
					if (this.gun == null)
					{
						this.gun = this.Get("Gun");
					}
					return this.gun;
				}
			}

			// Token: 0x06002AB9 RID: 10937 RVA: 0x000A2380 File Offset: 0x000A0580
			internal Tag Get(string name)
			{
				foreach (Tag tag in this.AllTags)
				{
					if (tag.name == name)
					{
						return tag;
					}
				}
				return null;
			}

			// Token: 0x040022F2 RID: 8946
			[SerializeField]
			private Tag character;

			// Token: 0x040022F3 RID: 8947
			[SerializeField]
			private Tag lockInDemoTag;

			// Token: 0x040022F4 RID: 8948
			[SerializeField]
			private Tag helmat;

			// Token: 0x040022F5 RID: 8949
			[SerializeField]
			private Tag armor;

			// Token: 0x040022F6 RID: 8950
			[SerializeField]
			private Tag backpack;

			// Token: 0x040022F7 RID: 8951
			[SerializeField]
			private Tag bullet;

			// Token: 0x040022F8 RID: 8952
			[SerializeField]
			private Tag bait;

			// Token: 0x040022F9 RID: 8953
			[SerializeField]
			private Tag advancedDebuffMode;

			// Token: 0x040022FA RID: 8954
			[SerializeField]
			private Tag special;

			// Token: 0x040022FB RID: 8955
			[SerializeField]
			private Tag destroyOnLootBox;

			// Token: 0x040022FC RID: 8956
			[FormerlySerializedAs("dontDropOnDead")]
			[SerializeField]
			private Tag dontDropOnDeadInSlot;

			// Token: 0x040022FD RID: 8957
			[SerializeField]
			private List<Tag> allTags = new List<Tag>();

			// Token: 0x040022FE RID: 8958
			private ReadOnlyCollection<Tag> tagsReadOnly;

			// Token: 0x040022FF RID: 8959
			private Tag gun;
		}

		// Token: 0x02000662 RID: 1634
		[Serializable]
		public class PrefabsData
		{
			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000A23EF File Offset: 0x000A05EF
			public LevelManager LevelManagerPrefab
			{
				get
				{
					return this.levelManagerPrefab;
				}
			}

			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x06002ABC RID: 10940 RVA: 0x000A23F7 File Offset: 0x000A05F7
			public CharacterMainControl CharacterPrefab
			{
				get
				{
					return this.characterPrefab;
				}
			}

			// Token: 0x170007B3 RID: 1971
			// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000A23FF File Offset: 0x000A05FF
			public GameObject BulletHitObsticleFx
			{
				get
				{
					return this.bulletHitObsticleFx;
				}
			}

			// Token: 0x170007B4 RID: 1972
			// (get) Token: 0x06002ABE RID: 10942 RVA: 0x000A2407 File Offset: 0x000A0607
			public GameObject QuestMarker
			{
				get
				{
					return this.questMarker;
				}
			}

			// Token: 0x170007B5 RID: 1973
			// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000A240F File Offset: 0x000A060F
			public DuckovItemAgent PickupAgentPrefab
			{
				get
				{
					return this.pickupAgentPrefab;
				}
			}

			// Token: 0x170007B6 RID: 1974
			// (get) Token: 0x06002AC0 RID: 10944 RVA: 0x000A2417 File Offset: 0x000A0617
			public DuckovItemAgent PickupAgentNoRendererPrefab
			{
				get
				{
					return this.pickupAgentNoRendererPrefab;
				}
			}

			// Token: 0x170007B7 RID: 1975
			// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000A241F File Offset: 0x000A061F
			public DuckovItemAgent HandheldAgentPrefab
			{
				get
				{
					return this.handheldAgentPrefab;
				}
			}

			// Token: 0x170007B8 RID: 1976
			// (get) Token: 0x06002AC2 RID: 10946 RVA: 0x000A2427 File Offset: 0x000A0627
			public InteractableLootbox LootBoxPrefab
			{
				get
				{
					return this.lootBoxPrefab;
				}
			}

			// Token: 0x170007B9 RID: 1977
			// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000A242F File Offset: 0x000A062F
			public InteractableLootbox LootBoxPrefab_Tomb
			{
				get
				{
					return this.lootBoxPrefab_Tomb;
				}
			}

			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x000A2437 File Offset: 0x000A0637
			public InteractMarker InteractMarker
			{
				get
				{
					return this.interactMarker;
				}
			}

			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000A243F File Offset: 0x000A063F
			public HeadCollider HeadCollider
			{
				get
				{
					return this.headCollider;
				}
			}

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x06002AC6 RID: 10950 RVA: 0x000A2447 File Offset: 0x000A0647
			public Projectile DefaultBullet
			{
				get
				{
					return this.defaultBullet;
				}
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000A244F File Offset: 0x000A064F
			public GameObject BuildingBlockAreaMesh
			{
				get
				{
					return this.buildingBlockAreaMesh;
				}
			}

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x000A2457 File Offset: 0x000A0657
			public GameObject AlertFxPrefab
			{
				get
				{
					return this.alertFxPrefab;
				}
			}

			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000A245F File Offset: 0x000A065F
			public GameObject KazooUi
			{
				get
				{
					return this.kazooUi;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06002ACA RID: 10954 RVA: 0x000A2467 File Offset: 0x000A0667
			public UIInputManager UIInputManagerPrefab
			{
				get
				{
					return this.uiInputManagerPrefab;
				}
			}

			// Token: 0x04002300 RID: 8960
			[SerializeField]
			private LevelManager levelManagerPrefab;

			// Token: 0x04002301 RID: 8961
			[SerializeField]
			private CharacterMainControl characterPrefab;

			// Token: 0x04002302 RID: 8962
			[SerializeField]
			private GameObject bulletHitObsticleFx;

			// Token: 0x04002303 RID: 8963
			[SerializeField]
			private GameObject questMarker;

			// Token: 0x04002304 RID: 8964
			[SerializeField]
			private DuckovItemAgent pickupAgentPrefab;

			// Token: 0x04002305 RID: 8965
			[SerializeField]
			private DuckovItemAgent pickupAgentNoRendererPrefab;

			// Token: 0x04002306 RID: 8966
			[SerializeField]
			private DuckovItemAgent handheldAgentPrefab;

			// Token: 0x04002307 RID: 8967
			[SerializeField]
			private InteractableLootbox lootBoxPrefab;

			// Token: 0x04002308 RID: 8968
			[SerializeField]
			private InteractableLootbox lootBoxPrefab_Tomb;

			// Token: 0x04002309 RID: 8969
			[SerializeField]
			private InteractMarker interactMarker;

			// Token: 0x0400230A RID: 8970
			[SerializeField]
			private HeadCollider headCollider;

			// Token: 0x0400230B RID: 8971
			[SerializeField]
			private Projectile defaultBullet;

			// Token: 0x0400230C RID: 8972
			[SerializeField]
			private UIInputManager uiInputManagerPrefab;

			// Token: 0x0400230D RID: 8973
			[SerializeField]
			private GameObject buildingBlockAreaMesh;

			// Token: 0x0400230E RID: 8974
			[SerializeField]
			private GameObject alertFxPrefab;

			// Token: 0x0400230F RID: 8975
			[SerializeField]
			private GameObject kazooUi;
		}

		// Token: 0x02000663 RID: 1635
		[Serializable]
		public class BuffsData
		{
			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x06002ACC RID: 10956 RVA: 0x000A2477 File Offset: 0x000A0677
			public Buff BleedSBuff
			{
				get
				{
					return this.bleedSBuff;
				}
			}

			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x06002ACD RID: 10957 RVA: 0x000A247F File Offset: 0x000A067F
			public Buff UnlimitBleedBuff
			{
				get
				{
					return this.unlimitBleedBuff;
				}
			}

			// Token: 0x170007C3 RID: 1987
			// (get) Token: 0x06002ACE RID: 10958 RVA: 0x000A2487 File Offset: 0x000A0687
			public Buff BoneCrackBuff
			{
				get
				{
					return this.boneCrackBuff;
				}
			}

			// Token: 0x170007C4 RID: 1988
			// (get) Token: 0x06002ACF RID: 10959 RVA: 0x000A248F File Offset: 0x000A068F
			public Buff WoundBuff
			{
				get
				{
					return this.woundBuff;
				}
			}

			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x000A2497 File Offset: 0x000A0697
			public Buff Weight_Light
			{
				get
				{
					return this.weight_Light;
				}
			}

			// Token: 0x170007C6 RID: 1990
			// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000A249F File Offset: 0x000A069F
			public Buff Weight_Heavy
			{
				get
				{
					return this.weight_Heavy;
				}
			}

			// Token: 0x170007C7 RID: 1991
			// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x000A24A7 File Offset: 0x000A06A7
			public Buff Weight_SuperHeavy
			{
				get
				{
					return this.weight_SuperHeavy;
				}
			}

			// Token: 0x170007C8 RID: 1992
			// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000A24AF File Offset: 0x000A06AF
			public Buff Weight_Overweight
			{
				get
				{
					return this.weight_Overweight;
				}
			}

			// Token: 0x170007C9 RID: 1993
			// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x000A24B7 File Offset: 0x000A06B7
			public Buff Pain
			{
				get
				{
					return this.pain;
				}
			}

			// Token: 0x170007CA RID: 1994
			// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000A24BF File Offset: 0x000A06BF
			public Buff BaseBuff
			{
				get
				{
					return this.baseBuff;
				}
			}

			// Token: 0x170007CB RID: 1995
			// (get) Token: 0x06002AD6 RID: 10966 RVA: 0x000A24C7 File Offset: 0x000A06C7
			public Buff Starve
			{
				get
				{
					return this.starve;
				}
			}

			// Token: 0x170007CC RID: 1996
			// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000A24CF File Offset: 0x000A06CF
			public Buff Thirsty
			{
				get
				{
					return this.thirsty;
				}
			}

			// Token: 0x170007CD RID: 1997
			// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000A24D7 File Offset: 0x000A06D7
			public Buff Burn
			{
				get
				{
					return this.burn;
				}
			}

			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000A24DF File Offset: 0x000A06DF
			public Buff Poison
			{
				get
				{
					return this.poison;
				}
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06002ADA RID: 10970 RVA: 0x000A24E7 File Offset: 0x000A06E7
			public Buff Electric
			{
				get
				{
					return this.electric;
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000A24EF File Offset: 0x000A06EF
			public Buff Space
			{
				get
				{
					return this.space;
				}
			}

			// Token: 0x06002ADC RID: 10972 RVA: 0x000A24F8 File Offset: 0x000A06F8
			public string GetBuffDisplayName(int id)
			{
				Buff buff = this.allBuffs.Find((Buff e) => e != null && e.ID == id);
				if (buff == null)
				{
					return "?";
				}
				return buff.DisplayName;
			}

			// Token: 0x04002310 RID: 8976
			[SerializeField]
			private Buff bleedSBuff;

			// Token: 0x04002311 RID: 8977
			[SerializeField]
			private Buff unlimitBleedBuff;

			// Token: 0x04002312 RID: 8978
			[SerializeField]
			private Buff boneCrackBuff;

			// Token: 0x04002313 RID: 8979
			[SerializeField]
			private Buff woundBuff;

			// Token: 0x04002314 RID: 8980
			[SerializeField]
			private Buff weight_Light;

			// Token: 0x04002315 RID: 8981
			[SerializeField]
			private Buff weight_Heavy;

			// Token: 0x04002316 RID: 8982
			[SerializeField]
			private Buff weight_SuperHeavy;

			// Token: 0x04002317 RID: 8983
			[SerializeField]
			private Buff weight_Overweight;

			// Token: 0x04002318 RID: 8984
			[SerializeField]
			private Buff pain;

			// Token: 0x04002319 RID: 8985
			[SerializeField]
			private Buff baseBuff;

			// Token: 0x0400231A RID: 8986
			[SerializeField]
			private Buff starve;

			// Token: 0x0400231B RID: 8987
			[SerializeField]
			private Buff thirsty;

			// Token: 0x0400231C RID: 8988
			[SerializeField]
			private Buff burn;

			// Token: 0x0400231D RID: 8989
			[SerializeField]
			private Buff poison;

			// Token: 0x0400231E RID: 8990
			[SerializeField]
			private Buff electric;

			// Token: 0x0400231F RID: 8991
			[SerializeField]
			private Buff space;

			// Token: 0x04002320 RID: 8992
			[SerializeField]
			private List<Buff> allBuffs;
		}

		// Token: 0x02000664 RID: 1636
		[Serializable]
		public class ItemAssetsData
		{
			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06002ADE RID: 10974 RVA: 0x000A2547 File Offset: 0x000A0747
			public int DefaultCharacterItemTypeID
			{
				get
				{
					return this.defaultCharacterItemTypeID;
				}
			}

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000A254F File Offset: 0x000A074F
			public int CashItemTypeID
			{
				get
				{
					return this.cashItemTypeID;
				}
			}

			// Token: 0x04002321 RID: 8993
			[SerializeField]
			[ItemTypeID]
			private int defaultCharacterItemTypeID;

			// Token: 0x04002322 RID: 8994
			[SerializeField]
			[ItemTypeID]
			private int cashItemTypeID;
		}

		// Token: 0x02000665 RID: 1637
		public class StringListsData
		{
			// Token: 0x04002323 RID: 8995
			public static StringList StatKeys;

			// Token: 0x04002324 RID: 8996
			public static StringList SlotTypes;

			// Token: 0x04002325 RID: 8997
			public static StringList ItemAgentKeys;
		}

		// Token: 0x02000666 RID: 1638
		[Serializable]
		public class LayersData
		{
			// Token: 0x06002AE2 RID: 10978 RVA: 0x000A2567 File Offset: 0x000A0767
			public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
			{
				return (1 << layer & layerMask) != 0;
			}

			// Token: 0x04002326 RID: 8998
			public LayerMask damageReceiverLayerMask;

			// Token: 0x04002327 RID: 8999
			public LayerMask wallLayerMask;

			// Token: 0x04002328 RID: 9000
			public LayerMask groundLayerMask;

			// Token: 0x04002329 RID: 9001
			public LayerMask halfObsticleLayer;

			// Token: 0x0400232A RID: 9002
			public LayerMask fowBlockLayers;

			// Token: 0x0400232B RID: 9003
			public LayerMask fowBlockLayersWithThermal;
		}

		// Token: 0x02000667 RID: 1639
		[Serializable]
		public class SceneManagementData
		{
			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000A2583 File Offset: 0x000A0783
			public SceneInfoCollection SceneInfoCollection
			{
				get
				{
					return this.sceneInfoCollection;
				}
			}

			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000A258B File Offset: 0x000A078B
			public SceneReference PrologueScene
			{
				get
				{
					return this.prologueScene;
				}
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000A2593 File Offset: 0x000A0793
			public SceneReference MainMenuScene
			{
				get
				{
					return this.mainMenuScene;
				}
			}

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000A259B File Offset: 0x000A079B
			public SceneReference BaseScene
			{
				get
				{
					return this.baseScene;
				}
			}

			// Token: 0x170007D7 RID: 2007
			// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000A25A3 File Offset: 0x000A07A3
			public SceneReference FailLoadingScreenScene
			{
				get
				{
					return this.failLoadingScreenScene;
				}
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x000A25AB File Offset: 0x000A07AB
			public SceneReference EvacuateScreenScene
			{
				get
				{
					return this.evacuateScreenScene;
				}
			}

			// Token: 0x0400232C RID: 9004
			[SerializeField]
			private SceneInfoCollection sceneInfoCollection;

			// Token: 0x0400232D RID: 9005
			[SerializeField]
			private SceneReference prologueScene;

			// Token: 0x0400232E RID: 9006
			[SerializeField]
			private SceneReference mainMenuScene;

			// Token: 0x0400232F RID: 9007
			[SerializeField]
			private SceneReference baseScene;

			// Token: 0x04002330 RID: 9008
			[SerializeField]
			private SceneReference failLoadingScreenScene;

			// Token: 0x04002331 RID: 9009
			[SerializeField]
			private SceneReference evacuateScreenScene;
		}

		// Token: 0x02000668 RID: 1640
		[Serializable]
		public class QuestsData
		{
			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000A25BB File Offset: 0x000A07BB
			private string DefaultQuestGiverDisplayName
			{
				get
				{
					return this.defaultQuestGiverDisplayName;
				}
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000A25C3 File Offset: 0x000A07C3
			public QuestCollection QuestCollection
			{
				get
				{
					return this.questCollection;
				}
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x06002AED RID: 10989 RVA: 0x000A25CB File Offset: 0x000A07CB
			public QuestRelationGraph QuestRelation
			{
				get
				{
					return this.questRelation;
				}
			}

			// Token: 0x06002AEE RID: 10990 RVA: 0x000A25D4 File Offset: 0x000A07D4
			public GameplayDataSettings.QuestsData.QuestGiverInfo GetInfo(QuestGiverID id)
			{
				return this.questGiverInfos.Find((GameplayDataSettings.QuestsData.QuestGiverInfo e) => e != null && e.id == id);
			}

			// Token: 0x06002AEF RID: 10991 RVA: 0x000A2608 File Offset: 0x000A0808
			public string GetDisplayName(QuestGiverID id)
			{
				return string.Format("Character_{0}", id).ToPlainText();
			}

			// Token: 0x04002332 RID: 9010
			[SerializeField]
			private QuestCollection questCollection;

			// Token: 0x04002333 RID: 9011
			[SerializeField]
			private QuestRelationGraph questRelation;

			// Token: 0x04002334 RID: 9012
			[SerializeField]
			private List<GameplayDataSettings.QuestsData.QuestGiverInfo> questGiverInfos;

			// Token: 0x04002335 RID: 9013
			[SerializeField]
			private string defaultQuestGiverDisplayName = "佚名";

			// Token: 0x02000686 RID: 1670
			[Serializable]
			public class QuestGiverInfo
			{
				// Token: 0x170007E8 RID: 2024
				// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000A3645 File Offset: 0x000A1845
				public string DisplayName
				{
					get
					{
						return this.displayName;
					}
				}

				// Token: 0x04002392 RID: 9106
				public QuestGiverID id;

				// Token: 0x04002393 RID: 9107
				[SerializeField]
				private string displayName;
			}
		}

		// Token: 0x02000669 RID: 1641
		[Serializable]
		public class EconomyData
		{
			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000A263D File Offset: 0x000A083D
			public ReadOnlyCollection<int> UnlockedItemByDefault
			{
				get
				{
					return this.unlockItemByDefault.AsReadOnly();
				}
			}

			// Token: 0x04002336 RID: 9014
			[SerializeField]
			[ItemTypeID]
			private List<int> unlockItemByDefault = new List<int>();
		}

		// Token: 0x0200066A RID: 1642
		[Serializable]
		public class UIStyleData
		{
			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x000A265D File Offset: 0x000A085D
			public Sprite CritPopSprite
			{
				get
				{
					return this.critPopSprite;
				}
			}

			// Token: 0x170007DE RID: 2014
			// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x000A2665 File Offset: 0x000A0865
			public Sprite DefaultTeleporterIcon
			{
				get
				{
					return this.defaultTeleporterIcon;
				}
			}

			// Token: 0x170007DF RID: 2015
			// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000A266D File Offset: 0x000A086D
			public Sprite EleteCharacterIcon
			{
				get
				{
					return this.eleteCharacterIcon;
				}
			}

			// Token: 0x170007E0 RID: 2016
			// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x000A2675 File Offset: 0x000A0875
			public Sprite BossCharacterIcon
			{
				get
				{
					return this.bossCharacterIcon;
				}
			}

			// Token: 0x170007E1 RID: 2017
			// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000A267D File Offset: 0x000A087D
			public Sprite PmcCharacterIcon
			{
				get
				{
					return this.pmcCharacterIcon;
				}
			}

			// Token: 0x170007E2 RID: 2018
			// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000A2685 File Offset: 0x000A0885
			public Sprite MerchantCharacterIcon
			{
				get
				{
					return this.merchantCharacterIcon;
				}
			}

			// Token: 0x170007E3 RID: 2019
			// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x000A268D File Offset: 0x000A088D
			public Sprite PetCharacterIcon
			{
				get
				{
					return this.petCharacterIcon;
				}
			}

			// Token: 0x170007E4 RID: 2020
			// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000A2695 File Offset: 0x000A0895
			public float TeleporterIconScale
			{
				get
				{
					return this.teleporterIconScale;
				}
			}

			// Token: 0x170007E5 RID: 2021
			// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000A269D File Offset: 0x000A089D
			public Sprite FallbackItemIcon
			{
				get
				{
					return this.fallbackItemIcon;
				}
			}

			// Token: 0x170007E6 RID: 2022
			// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000A26A5 File Offset: 0x000A08A5
			public TextMeshProUGUI TemplateTextUGUI
			{
				get
				{
					return this.templateTextUGUI;
				}
			}

			// Token: 0x170007E7 RID: 2023
			// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000A26AD File Offset: 0x000A08AD
			[SerializeField]
			private TMP_Asset DefaultFont
			{
				get
				{
					return this.defaultFont;
				}
			}

			// Token: 0x06002AFE RID: 11006 RVA: 0x000A26B8 File Offset: 0x000A08B8
			[return: TupleElementNames(new string[]
			{
				"shadowOffset",
				"color",
				"innerGlow"
			})]
			public ValueTuple<float, Color, bool> GetShadowOffsetAndColorOfQuality(DisplayQuality displayQuality)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == displayQuality);
				if (displayQualityLook == null)
				{
					return new ValueTuple<float, Color, bool>(this.defaultDisplayQualityShadowOffset, this.defaultDisplayQualityShadowColor, this.defaultDIsplayQualityShadowInnerGlow);
				}
				return new ValueTuple<float, Color, bool>(displayQualityLook.shadowOffset, displayQualityLook.shadowColor, displayQualityLook.innerGlow);
			}

			// Token: 0x06002AFF RID: 11007 RVA: 0x000A271C File Offset: 0x000A091C
			public void ApplyDisplayQualityShadow(DisplayQuality displayQuality, TrueShadow target)
			{
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = this.GetShadowOffsetAndColorOfQuality(displayQuality);
				target.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				target.Color = shadowOffsetAndColorOfQuality.Item2;
				target.Inset = shadowOffsetAndColorOfQuality.Item3;
			}

			// Token: 0x06002B00 RID: 11008 RVA: 0x000A2758 File Offset: 0x000A0958
			public GameplayDataSettings.UIStyleData.DisplayQualityLook GetDisplayQualityLook(DisplayQuality q)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == q);
				if (displayQualityLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayQualityLook
					{
						quality = q,
						shadowOffset = this.defaultDisplayQualityShadowOffset,
						shadowColor = this.defaultDisplayQualityShadowColor,
						innerGlow = this.defaultDIsplayQualityShadowInnerGlow
					};
				}
				return displayQualityLook;
			}

			// Token: 0x06002B01 RID: 11009 RVA: 0x000A27C4 File Offset: 0x000A09C4
			public GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook GetElementDamagePopTextLook(ElementTypes elementType)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook displayElementDamagePopTextLook = this.elementDamagePopTextLook.Find((GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook e) => e != null && e.elementType == elementType);
				if (displayElementDamagePopTextLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook
					{
						elementType = ElementTypes.physics,
						normalSize = 1f,
						critSize = 1.6f,
						color = Color.white
					};
				}
				return displayElementDamagePopTextLook;
			}

			// Token: 0x04002337 RID: 9015
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayQualityLook> displayQualityLooks = new List<GameplayDataSettings.UIStyleData.DisplayQualityLook>();

			// Token: 0x04002338 RID: 9016
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook> elementDamagePopTextLook = new List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook>();

			// Token: 0x04002339 RID: 9017
			[SerializeField]
			private float defaultDisplayQualityShadowOffset = 8f;

			// Token: 0x0400233A RID: 9018
			[SerializeField]
			private Color defaultDisplayQualityShadowColor = Color.black;

			// Token: 0x0400233B RID: 9019
			[SerializeField]
			private bool defaultDIsplayQualityShadowInnerGlow;

			// Token: 0x0400233C RID: 9020
			[SerializeField]
			private Sprite defaultTeleporterIcon;

			// Token: 0x0400233D RID: 9021
			[SerializeField]
			private float teleporterIconScale = 0.5f;

			// Token: 0x0400233E RID: 9022
			[SerializeField]
			private Sprite critPopSprite;

			// Token: 0x0400233F RID: 9023
			[SerializeField]
			private Sprite fallbackItemIcon;

			// Token: 0x04002340 RID: 9024
			[SerializeField]
			private Sprite eleteCharacterIcon;

			// Token: 0x04002341 RID: 9025
			[SerializeField]
			private Sprite bossCharacterIcon;

			// Token: 0x04002342 RID: 9026
			[SerializeField]
			private Sprite pmcCharacterIcon;

			// Token: 0x04002343 RID: 9027
			[SerializeField]
			private Sprite merchantCharacterIcon;

			// Token: 0x04002344 RID: 9028
			[SerializeField]
			private Sprite petCharacterIcon;

			// Token: 0x04002345 RID: 9029
			[SerializeField]
			private TMP_Asset defaultFont;

			// Token: 0x04002346 RID: 9030
			[SerializeField]
			private TextMeshProUGUI templateTextUGUI;

			// Token: 0x02000688 RID: 1672
			[Serializable]
			public class DisplayQualityLook
			{
				// Token: 0x06002B2C RID: 11052 RVA: 0x000A3672 File Offset: 0x000A1872
				public void Apply(TrueShadow trueShadow)
				{
					trueShadow.OffsetDistance = this.shadowOffset;
					trueShadow.Color = this.shadowColor;
					trueShadow.Inset = this.innerGlow;
				}

				// Token: 0x04002395 RID: 9109
				public DisplayQuality quality;

				// Token: 0x04002396 RID: 9110
				public float shadowOffset;

				// Token: 0x04002397 RID: 9111
				public Color shadowColor;

				// Token: 0x04002398 RID: 9112
				public bool innerGlow;
			}

			// Token: 0x02000689 RID: 1673
			[Serializable]
			public class DisplayElementDamagePopTextLook
			{
				// Token: 0x04002399 RID: 9113
				public ElementTypes elementType;

				// Token: 0x0400239A RID: 9114
				public float normalSize;

				// Token: 0x0400239B RID: 9115
				public float critSize;

				// Token: 0x0400239C RID: 9116
				public Color color;
			}
		}

		// Token: 0x0200066B RID: 1643
		[Serializable]
		public class SpritesData
		{
			// Token: 0x06002B03 RID: 11011 RVA: 0x000A2868 File Offset: 0x000A0A68
			public Sprite GetSprite(string key)
			{
				foreach (GameplayDataSettings.SpritesData.Entry entry in this.entries)
				{
					if (entry.key == key)
					{
						return entry.sprite;
					}
				}
				return null;
			}

			// Token: 0x04002347 RID: 9031
			public List<GameplayDataSettings.SpritesData.Entry> entries;

			// Token: 0x0200068D RID: 1677
			[Serializable]
			public struct Entry
			{
				// Token: 0x040023A0 RID: 9120
				public string key;

				// Token: 0x040023A1 RID: 9121
				public Sprite sprite;
			}
		}

		// Token: 0x0200066C RID: 1644
		[Serializable]
		public class CharacterRandomPresets
		{
			// Token: 0x04002348 RID: 9032
			public List<CharacterRandomPreset> presets;
		}
	}
}
