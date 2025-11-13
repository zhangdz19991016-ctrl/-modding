using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200031B RID: 795
	public class BuildingManager : MonoBehaviour
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0005FA35 File Offset: 0x0005DC35
		// (set) Token: 0x06001A69 RID: 6761 RVA: 0x0005FA3C File Offset: 0x0005DC3C
		public static BuildingManager Instance { get; private set; }

		// Token: 0x06001A6A RID: 6762 RVA: 0x0005FA44 File Offset: 0x0005DC44
		private static int GenerateBuildingGUID(string buildingID)
		{
			BuildingManager.<>c__DisplayClass4_0 CS$<>8__locals1 = new BuildingManager.<>c__DisplayClass4_0();
			CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			while (BuildingManager.Any((BuildingManager.BuildingData e) => e != null && e.GUID == CS$<>8__locals1.result))
			{
				CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			}
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0005FA80 File Offset: 0x0005DC80
		public int GetTokenAmount(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				return buildingTokenAmountEntry.amount;
			}
			return 0;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0005FAC0 File Offset: 0x0005DCC0
		private void SetTokenAmount(string id, int amount)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				buildingTokenAmountEntry.amount = amount;
				return;
			}
			buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
			{
				id = id,
				amount = amount
			};
			this.tokens.Add(buildingTokenAmountEntry);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0005FB24 File Offset: 0x0005DD24
		private void AddToken(string id, int amount = 1)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
				{
					id = id,
					amount = 0
				};
				this.tokens.Add(buildingTokenAmountEntry);
			}
			buildingTokenAmountEntry.amount += amount;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0005FB8C File Offset: 0x0005DD8C
		private bool PayToken(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				return false;
			}
			if (buildingTokenAmountEntry.amount <= 0)
			{
				return false;
			}
			buildingTokenAmountEntry.amount--;
			return true;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0005FBE0 File Offset: 0x0005DDE0
		public static Vector2Int[] GetOccupyingCoords(Vector2Int dimensions, BuildingRotation rotations, Vector2Int coord)
		{
			if (rotations % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			Vector2Int[] array = new Vector2Int[dimensions.x * dimensions.y];
			for (int i = 0; i < dimensions.y; i++)
			{
				for (int j = 0; j < dimensions.x; j++)
				{
					int num = j + dimensions.x * i;
					array[num] = coord + new Vector2Int(j, i);
				}
			}
			return array;
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0005FC61 File Offset: 0x0005DE61
		public List<BuildingManager.BuildingAreaData> Areas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0005FC6C File Offset: 0x0005DE6C
		public BuildingManager.BuildingAreaData GetOrCreateArea(string id)
		{
			BuildingManager.BuildingAreaData buildingAreaData = this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
			if (buildingAreaData != null)
			{
				return buildingAreaData;
			}
			BuildingManager.BuildingAreaData buildingAreaData2 = new BuildingManager.BuildingAreaData(id);
			this.areas.Add(buildingAreaData2);
			return buildingAreaData2;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0005FCBC File Offset: 0x0005DEBC
		public BuildingManager.BuildingAreaData GetArea(string id)
		{
			return this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0005FCED File Offset: 0x0005DEED
		private void CleanupAndSort()
		{
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0005FCEF File Offset: 0x0005DEEF
		public static BuildingInfo GetBuildingInfo(string id)
		{
			return BuildingDataCollection.GetInfo(id);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0005FCF8 File Offset: 0x0005DEF8
		public static bool Any(string id, bool includeTokens = false)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (includeTokens && BuildingManager.Instance.GetTokenAmount(id) > 0)
			{
				return true;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(id))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0005FD78 File Offset: 0x0005DF78
		public static bool Any(Func<BuildingManager.BuildingData, bool> predicate)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(predicate))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0005FDE8 File Offset: 0x0005DFE8
		public static int GetBuildingAmount(string id)
		{
			if (BuildingManager.Instance == null)
			{
				return 0;
			}
			int num = 0;
			foreach (BuildingManager.BuildingAreaData buildingAreaData in BuildingManager.Instance.Areas)
			{
				using (List<BuildingManager.BuildingData>.Enumerator enumerator2 = buildingAreaData.Buildings.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.ID == id)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06001A78 RID: 6776 RVA: 0x0005FE94 File Offset: 0x0005E094
		// (remove) Token: 0x06001A79 RID: 6777 RVA: 0x0005FEC8 File Offset: 0x0005E0C8
		public static event Action OnBuildingListChanged;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06001A7A RID: 6778 RVA: 0x0005FEFC File Offset: 0x0005E0FC
		// (remove) Token: 0x06001A7B RID: 6779 RVA: 0x0005FF30 File Offset: 0x0005E130
		public static event Action<int> OnBuildingBuilt;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06001A7C RID: 6780 RVA: 0x0005FF64 File Offset: 0x0005E164
		// (remove) Token: 0x06001A7D RID: 6781 RVA: 0x0005FF98 File Offset: 0x0005E198
		public static event Action<int> OnBuildingDestroyed;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06001A7E RID: 6782 RVA: 0x0005FFCC File Offset: 0x0005E1CC
		// (remove) Token: 0x06001A7F RID: 6783 RVA: 0x00060000 File Offset: 0x0005E200
		public static event Action<int, BuildingInfo> OnBuildingBuiltComplex;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06001A80 RID: 6784 RVA: 0x00060034 File Offset: 0x0005E234
		// (remove) Token: 0x06001A81 RID: 6785 RVA: 0x00060068 File Offset: 0x0005E268
		public static event Action<int, BuildingInfo> OnBuildingDestroyedComplex;

		// Token: 0x06001A82 RID: 6786 RVA: 0x0006009B File Offset: 0x0005E29B
		private void Awake()
		{
			BuildingManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000600BA File Offset: 0x0005E2BA
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000600CD File Offset: 0x0005E2CD
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000600D8 File Offset: 0x0005E2D8
		private void Load()
		{
			BuildingManager.SaveData saveData = SavesSystem.Load<BuildingManager.SaveData>("BuildingData");
			this.areas.Clear();
			if (saveData.data != null)
			{
				this.areas.AddRange(saveData.data);
			}
			this.tokens.Clear();
			if (saveData.tokenAmounts != null)
			{
				this.tokens.AddRange(saveData.tokenAmounts);
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00060138 File Offset: 0x0005E338
		private void Save()
		{
			BuildingManager.SaveData value = new BuildingManager.SaveData
			{
				data = new List<BuildingManager.BuildingAreaData>(this.areas),
				tokenAmounts = new List<BuildingManager.BuildingTokenAmountEntry>(this.tokens)
			};
			SavesSystem.Save<BuildingManager.SaveData>("BuildingData", value);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00060180 File Offset: 0x0005E380
		internal static BuildingManager.BuildingAreaData GetAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.Areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == areaID);
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000601C4 File Offset: 0x0005E3C4
		internal static BuildingManager.BuildingAreaData GetOrCreateAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.GetOrCreateArea(areaID);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000601E0 File Offset: 0x0005E3E0
		internal static BuildingManager.BuildingData GetBuildingData(int guid, string areaID = null)
		{
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData = buildings.Find(match);
						if (buildingData != null)
						{
							return buildingData;
						}
					}
					goto IL_9B;
				}
				goto IL_74;
				IL_9B:
				return null;
			}
			IL_74:
			BuildingManager.BuildingAreaData areaData = BuildingManager.GetAreaData(areaID);
			if (areaData == null)
			{
				return null;
			}
			return areaData.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0006029C File Offset: 0x0005E49C
		internal static BuildingBuyAndPlaceResults BuyAndPlace(string areaID, string id, Vector2Int coord, BuildingRotation rotation)
		{
			if (BuildingManager.Instance == null)
			{
				return BuildingBuyAndPlaceResults.NoReferences;
			}
			BuildingInfo buildingInfo = BuildingManager.GetBuildingInfo(id);
			if (!buildingInfo.Valid)
			{
				return BuildingBuyAndPlaceResults.InvalidBuildingInfo;
			}
			BuildingManager.GetBuildingAmount(id);
			if (buildingInfo.ReachedAmountLimit)
			{
				return BuildingBuyAndPlaceResults.ReachedAmountLimit;
			}
			BuildingManager.Instance.GetTokenAmount(id);
			if (!BuildingManager.Instance.PayToken(id) && !buildingInfo.cost.Pay(true, true))
			{
				return BuildingBuyAndPlaceResults.PaymentFailure;
			}
			BuildingManager.BuildingAreaData orCreateArea = BuildingManager.Instance.GetOrCreateArea(areaID);
			int num = BuildingManager.GenerateBuildingGUID(id);
			orCreateArea.Add(id, rotation, coord, num);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingBuilt = BuildingManager.OnBuildingBuilt;
			if (onBuildingBuilt != null)
			{
				onBuildingBuilt(num);
			}
			Action<int, BuildingInfo> onBuildingBuiltComplex = BuildingManager.OnBuildingBuiltComplex;
			if (onBuildingBuiltComplex != null)
			{
				onBuildingBuiltComplex(num, buildingInfo);
			}
			AudioManager.Post("UI/building_up");
			return BuildingBuyAndPlaceResults.Succeed;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00060364 File Offset: 0x0005E564
		internal static bool DestroyBuilding(int guid, string areaID = null)
		{
			BuildingManager.BuildingData buildingData;
			BuildingManager.BuildingAreaData buildingAreaData;
			if (!BuildingManager.TryGetBuildingDataAndAreaData(guid, out buildingData, out buildingAreaData, areaID))
			{
				return false;
			}
			buildingAreaData.Remove(buildingData);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingDestroyed = BuildingManager.OnBuildingDestroyed;
			if (onBuildingDestroyed != null)
			{
				onBuildingDestroyed(guid);
			}
			Action<int, BuildingInfo> onBuildingDestroyedComplex = BuildingManager.OnBuildingDestroyedComplex;
			if (onBuildingDestroyedComplex != null)
			{
				onBuildingDestroyedComplex(guid, buildingData.Info);
			}
			return true;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000603C4 File Offset: 0x0005E5C4
		internal static bool TryGetBuildingDataAndAreaData(int guid, out BuildingManager.BuildingData buildingData, out BuildingManager.BuildingAreaData areaData, string areaID = null)
		{
			buildingData = null;
			areaData = null;
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData2 = buildings.Find(match);
						if (buildingData2 != null)
						{
							areaData = buildingAreaData;
							buildingData = buildingData2;
							return true;
						}
					}
					return false;
				}
			}
			BuildingManager.BuildingAreaData area = BuildingManager.Instance.GetArea(areaID);
			if (area == null)
			{
				return false;
			}
			BuildingManager.BuildingData buildingData3 = area.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
			if (buildingData3 != null)
			{
				areaData = area;
				buildingData = buildingData3;
			}
			return false;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000604B4 File Offset: 0x0005E6B4
		internal static UniTask<bool> ReturnBuilding(int guid, string areaID = null)
		{
			BuildingManager.<ReturnBuilding>d__53 <ReturnBuilding>d__;
			<ReturnBuilding>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<ReturnBuilding>d__.guid = guid;
			<ReturnBuilding>d__.areaID = areaID;
			<ReturnBuilding>d__.<>1__state = -1;
			<ReturnBuilding>d__.<>t__builder.Start<BuildingManager.<ReturnBuilding>d__53>(ref <ReturnBuilding>d__);
			return <ReturnBuilding>d__.<>t__builder.Task;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00060500 File Offset: 0x0005E700
		internal static UniTask<int> ReturnBuildings(string areaID = null, params int[] buildings)
		{
			BuildingManager.<ReturnBuildings>d__54 <ReturnBuildings>d__;
			<ReturnBuildings>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildings>d__.areaID = areaID;
			<ReturnBuildings>d__.buildings = buildings;
			<ReturnBuildings>d__.<>1__state = -1;
			<ReturnBuildings>d__.<>t__builder.Start<BuildingManager.<ReturnBuildings>d__54>(ref <ReturnBuildings>d__);
			return <ReturnBuildings>d__.<>t__builder.Task;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0006054C File Offset: 0x0005E74C
		internal static UniTask<int> ReturnBuildingsOfType(string buildingID, string areaID = null)
		{
			BuildingManager.<ReturnBuildingsOfType>d__55 <ReturnBuildingsOfType>d__;
			<ReturnBuildingsOfType>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildingsOfType>d__.buildingID = buildingID;
			<ReturnBuildingsOfType>d__.areaID = areaID;
			<ReturnBuildingsOfType>d__.<>1__state = -1;
			<ReturnBuildingsOfType>d__.<>t__builder.Start<BuildingManager.<ReturnBuildingsOfType>d__55>(ref <ReturnBuildingsOfType>d__);
			return <ReturnBuildingsOfType>d__.<>t__builder.Task;
		}

		// Token: 0x040012F1 RID: 4849
		private List<BuildingManager.BuildingTokenAmountEntry> tokens = new List<BuildingManager.BuildingTokenAmountEntry>();

		// Token: 0x040012F2 RID: 4850
		[SerializeField]
		private List<BuildingManager.BuildingAreaData> areas = new List<BuildingManager.BuildingAreaData>();

		// Token: 0x040012F8 RID: 4856
		private const string SaveKey = "BuildingData";

		// Token: 0x040012F9 RID: 4857
		private static bool returningBuilding;

		// Token: 0x020005B3 RID: 1459
		[Serializable]
		public class BuildingTokenAmountEntry
		{
			// Token: 0x04002087 RID: 8327
			public string id;

			// Token: 0x04002088 RID: 8328
			public int amount;
		}

		// Token: 0x020005B4 RID: 1460
		[Serializable]
		public class BuildingAreaData
		{
			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x06002927 RID: 10535 RVA: 0x00098BA7 File Offset: 0x00096DA7
			public string AreaID
			{
				get
				{
					return this.areaID;
				}
			}

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x06002928 RID: 10536 RVA: 0x00098BAF File Offset: 0x00096DAF
			public List<BuildingManager.BuildingData> Buildings
			{
				get
				{
					return this.buildings;
				}
			}

			// Token: 0x06002929 RID: 10537 RVA: 0x00098BB8 File Offset: 0x00096DB8
			public bool Any(string buildingID)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (buildingData != null)
					{
						if (buildingData.ID == buildingID)
						{
							return true;
						}
						if (buildingData.Info.alternativeFor.Contains(buildingID))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x0600292A RID: 10538 RVA: 0x00098C34 File Offset: 0x00096E34
			public bool Add(string buildingID, BuildingRotation rotation, Vector2Int coord, int guid = -1)
			{
				BuildingManager.GetBuildingInfo(buildingID);
				if (guid < 0)
				{
					guid = BuildingManager.GenerateBuildingGUID(buildingID);
				}
				this.buildings.Add(new BuildingManager.BuildingData(guid, buildingID, rotation, coord));
				return true;
			}

			// Token: 0x0600292B RID: 10539 RVA: 0x00098C60 File Offset: 0x00096E60
			public bool Remove(int buildingGUID)
			{
				BuildingManager.BuildingData buildingData = this.buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == buildingGUID);
				return buildingData != null && this.buildings.Remove(buildingData);
			}

			// Token: 0x0600292C RID: 10540 RVA: 0x00098CA3 File Offset: 0x00096EA3
			public bool Remove(BuildingManager.BuildingData building)
			{
				return this.buildings.Remove(building);
			}

			// Token: 0x0600292D RID: 10541 RVA: 0x00098CB4 File Offset: 0x00096EB4
			public BuildingManager.BuildingData GetBuildingAt(Vector2Int coord)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord).Contains(coord))
					{
						return buildingData;
					}
				}
				return null;
			}

			// Token: 0x0600292E RID: 10542 RVA: 0x00098D28 File Offset: 0x00096F28
			public HashSet<Vector2Int> GetAllOccupiedCoords()
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					Vector2Int[] occupyingCoords = BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord);
					hashSet.AddRange(occupyingCoords);
				}
				return hashSet;
			}

			// Token: 0x0600292F RID: 10543 RVA: 0x00098D9C File Offset: 0x00096F9C
			public bool Collide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
			{
				HashSet<Vector2Int> allOccupiedCoords = this.GetAllOccupiedCoords();
				foreach (Vector2Int item in BuildingManager.GetOccupyingCoords(dimensions, rotation, coord))
				{
					if (allOccupiedCoords.Contains(item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06002930 RID: 10544 RVA: 0x00098DDB File Offset: 0x00096FDB
			internal bool Any(Func<BuildingManager.BuildingData, bool> predicate)
			{
				return this.buildings.Any(predicate);
			}

			// Token: 0x06002931 RID: 10545 RVA: 0x00098DE9 File Offset: 0x00096FE9
			public BuildingAreaData()
			{
			}

			// Token: 0x06002932 RID: 10546 RVA: 0x00098DFC File Offset: 0x00096FFC
			public BuildingAreaData(string areaID)
			{
				this.areaID = areaID;
			}

			// Token: 0x04002089 RID: 8329
			[SerializeField]
			private string areaID;

			// Token: 0x0400208A RID: 8330
			[SerializeField]
			private List<BuildingManager.BuildingData> buildings = new List<BuildingManager.BuildingData>();
		}

		// Token: 0x020005B5 RID: 1461
		[Serializable]
		public class BuildingData
		{
			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x06002933 RID: 10547 RVA: 0x00098E16 File Offset: 0x00097016
			public int GUID
			{
				get
				{
					return this.guid;
				}
			}

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06002934 RID: 10548 RVA: 0x00098E1E File Offset: 0x0009701E
			public string ID
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x17000789 RID: 1929
			// (get) Token: 0x06002935 RID: 10549 RVA: 0x00098E28 File Offset: 0x00097028
			public Vector2Int Dimensions
			{
				get
				{
					return this.Info.Dimensions;
				}
			}

			// Token: 0x1700078A RID: 1930
			// (get) Token: 0x06002936 RID: 10550 RVA: 0x00098E43 File Offset: 0x00097043
			public Vector2Int Coord
			{
				get
				{
					return this.coord;
				}
			}

			// Token: 0x1700078B RID: 1931
			// (get) Token: 0x06002937 RID: 10551 RVA: 0x00098E4B File Offset: 0x0009704B
			public BuildingRotation Rotation
			{
				get
				{
					return this.rotation;
				}
			}

			// Token: 0x1700078C RID: 1932
			// (get) Token: 0x06002938 RID: 10552 RVA: 0x00098E53 File Offset: 0x00097053
			public BuildingInfo Info
			{
				get
				{
					return BuildingDataCollection.GetInfo(this.id);
				}
			}

			// Token: 0x06002939 RID: 10553 RVA: 0x00098E60 File Offset: 0x00097060
			public BuildingData(int guid, string id, BuildingRotation rotation, Vector2Int coord)
			{
				this.guid = guid;
				this.id = id;
				this.coord = coord;
				this.rotation = rotation;
			}

			// Token: 0x0600293A RID: 10554 RVA: 0x00098E88 File Offset: 0x00097088
			internal Vector3 GetTransformPosition()
			{
				Vector2Int dimensions = this.Dimensions;
				if (this.rotation % BuildingRotation.Half > BuildingRotation.Zero)
				{
					dimensions = new Vector2Int(dimensions.y, dimensions.x);
				}
				return new Vector3((float)this.coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)this.coord.y - 0.5f + (float)dimensions.y / 2f);
			}

			// Token: 0x0400208B RID: 8331
			[SerializeField]
			private int guid;

			// Token: 0x0400208C RID: 8332
			[SerializeField]
			private string id;

			// Token: 0x0400208D RID: 8333
			[SerializeField]
			private Vector2Int coord;

			// Token: 0x0400208E RID: 8334
			[SerializeField]
			private BuildingRotation rotation;
		}

		// Token: 0x020005B6 RID: 1462
		[Serializable]
		private struct SaveData
		{
			// Token: 0x0400208F RID: 8335
			[SerializeField]
			public List<BuildingManager.BuildingAreaData> data;

			// Token: 0x04002090 RID: 8336
			[SerializeField]
			public List<BuildingManager.BuildingTokenAmountEntry> tokenAmounts;
		}
	}
}
