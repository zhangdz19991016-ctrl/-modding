using System;
using System.Collections.Generic;
using Drawing;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000316 RID: 790
	public class BuildingArea : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0005EBB2 File Offset: 0x0005CDB2
		public string AreaID
		{
			get
			{
				return this.areaID;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x0005EBBA File Offset: 0x0005CDBA
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0005EBC2 File Offset: 0x0005CDC2
		public Vector2Int LowerLeftCorner
		{
			get
			{
				return this.CenterCoord - (this.size - Vector2Int.one);
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0005EBDF File Offset: 0x0005CDDF
		private Vector2Int CenterCoord
		{
			get
			{
				return new Vector2Int(Mathf.RoundToInt(base.transform.position.x), Mathf.RoundToInt(base.transform.position.z));
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0005EC10 File Offset: 0x0005CE10
		private int Width
		{
			get
			{
				return this.size.x;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0005EC1D File Offset: 0x0005CE1D
		private int Height
		{
			get
			{
				return this.size.y;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x0005EC2A File Offset: 0x0005CE2A
		public BuildingManager.BuildingAreaData AreaData
		{
			get
			{
				return BuildingManager.GetOrCreateAreaData(this.AreaID);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0005EC37 File Offset: 0x0005CE37
		public Plane Plane
		{
			get
			{
				return new Plane(base.transform.up, base.transform.position);
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0005EC54 File Offset: 0x0005CE54
		private void Awake()
		{
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005EC67 File Offset: 0x0005CE67
		private void OnDestroy()
		{
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x0005EC7C File Offset: 0x0005CE7C
		private void OnBuildingBuilt(int guid)
		{
			BuildingManager.BuildingData buildingData = BuildingManager.GetBuildingData(guid, null);
			if (buildingData == null)
			{
				return;
			}
			this.Display(buildingData);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0005EC9C File Offset: 0x0005CE9C
		private void Start()
		{
			this.RepaintAll();
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0005ECA4 File Offset: 0x0005CEA4
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			int num = this.CenterCoord.x - (this.size.x - 1);
			int num2 = this.CenterCoord.x + (this.size.x - 1) + 1;
			int num3 = this.CenterCoord.y - (this.size.y - 1);
			int num4 = this.CenterCoord.y + (this.size.y - 1) + 1;
			Vector3 b = new Vector3(-0.5f, 0f, -0.5f);
			for (int i = num; i <= num2; i++)
			{
				Draw.Line(new Vector3((float)i, 0f, (float)num3) + b, new Vector3((float)i, 0f, (float)num4) + b);
			}
			for (int j = num3; j <= num4; j++)
			{
				Draw.Line(new Vector3((float)num, 0f, (float)j) + b, new Vector3((float)num2, 0f, (float)j) + b);
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0005EDD0 File Offset: 0x0005CFD0
		public bool IsPlacementWithinRange(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			coord -= this.CenterCoord;
			return coord.x > -this.size.x && coord.y > -this.size.y && coord.x + dimensions.x <= this.size.x && coord.y + dimensions.y <= this.size.y;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0005EE70 File Offset: 0x0005D070
		public Vector2Int CursorToCoord(Vector3 point, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			int x = Mathf.RoundToInt(point.x) - dimensions.x / 2;
			int y = Mathf.RoundToInt(point.z) - dimensions.y / 2;
			return new Vector2Int(x, y);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0005EECC File Offset: 0x0005D0CC
		private void ReleaseAllBuildings()
		{
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building = this.activeBuildings[i];
				if (!(building == null))
				{
					UnityEngine.Object.Destroy(building.gameObject);
				}
			}
			this.activeBuildings.Clear();
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0005EF20 File Offset: 0x0005D120
		public void RepaintAll()
		{
			this.ReleaseAllBuildings();
			BuildingManager.BuildingAreaData areaData = this.AreaData;
			if (areaData == null)
			{
				return;
			}
			foreach (BuildingManager.BuildingData building in areaData.Buildings)
			{
				this.Display(building);
			}
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005EF84 File Offset: 0x0005D184
		private void Display(BuildingManager.BuildingData building)
		{
			if (building == null)
			{
				return;
			}
			Building prefab = building.Info.Prefab;
			if (prefab == null)
			{
				Debug.LogError("No prefab for building " + building.ID);
				return;
			}
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building2 = this.activeBuildings[i];
				if (building2 == null)
				{
					this.activeBuildings.RemoveAt(i);
				}
				else if (building2.GUID == building.GUID)
				{
					Debug.LogError(string.Format("重复显示建筑{0}({1})", building.Info.DisplayName, building.GUID));
					return;
				}
			}
			Building building3 = UnityEngine.Object.Instantiate<Building>(prefab, base.transform);
			building3.Setup(building);
			building3.transform.position = building.GetTransformPosition();
			this.activeBuildings.Add(building3);
			if (building3.unlockAchievement && AchievementManager.Instance)
			{
				AchievementManager.Instance.Unlock("Building_" + building3.ID.Trim());
			}
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0005F0A0 File Offset: 0x0005D2A0
		internal Vector3 CoordToWorldPosition(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			return new Vector3((float)coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)coord.y - 0.5f + (float)dimensions.y / 2f);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0005F10C File Offset: 0x0005D30C
		internal bool PhysicsCollide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord, float castBeginHeight = 0f, float castHeight = 2f)
		{
			if (rotation % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			this.raycastHitCount = 0;
			for (int i = coord.y; i < coord.y + dimensions.y; i++)
			{
				for (int j = coord.x; j < coord.x + dimensions.x; j++)
				{
					Vector3 vector = new Vector3((float)j, castBeginHeight, (float)i);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector, Vector3.up, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector + Vector3.up * castHeight, Vector3.down, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					if (this.raycastHitCount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x0005F208 File Offset: 0x0005D408
		internal Building GetBuildingInstanceAt(Vector2Int coord)
		{
			BuildingManager.BuildingData buildingData = this.AreaData.GetBuildingAt(coord);
			if (buildingData == null)
			{
				return null;
			}
			return this.activeBuildings.Find((Building e) => e != null && e.GUID == buildingData.GUID);
		}

		// Token: 0x040012D5 RID: 4821
		[SerializeField]
		private string areaID;

		// Token: 0x040012D6 RID: 4822
		[SerializeField]
		private Vector2Int size;

		// Token: 0x040012D7 RID: 4823
		[SerializeField]
		private LayerMask physicsCollisionLayers = -1;

		// Token: 0x040012D8 RID: 4824
		private List<Building> activeBuildings = new List<Building>();

		// Token: 0x040012D9 RID: 4825
		private int raycastHitCount;

		// Token: 0x040012DA RID: 4826
		private RaycastHit[] raycastHitBuffer = new RaycastHit[5];
	}
}
