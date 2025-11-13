using System;
using Drawing;
using Duckov.Utilities;
using SodaCraft.Localizations;
using Unity.Mathematics;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000317 RID: 791
	public class Building : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0005F256 File Offset: 0x0005D456
		private int guid
		{
			get
			{
				return this.data.GUID;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0005F263 File Offset: 0x0005D463
		public int GUID
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0005F26B File Offset: 0x0005D46B
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0005F273 File Offset: 0x0005D473
		public Vector2Int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005F27C File Offset: 0x0005D47C
		public Vector3 GetOffset(BuildingRotation rotation = BuildingRotation.Zero)
		{
			bool flag = rotation % BuildingRotation.Half > BuildingRotation.Zero;
			float num = (float)((flag ? this.dimensions.y : this.dimensions.x) - 1);
			float num2 = (float)((flag ? this.dimensions.x : this.dimensions.y) - 1);
			return new Vector3(num / 2f, 0f, num2 / 2f);
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x0005F2E6 File Offset: 0x0005D4E6
		// (set) Token: 0x06001A3B RID: 6715 RVA: 0x0005F2E4 File Offset: 0x0005D4E4
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.ID;
			}
			set
			{
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x0005F2F8 File Offset: 0x0005D4F8
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0005F305 File Offset: 0x0005D505
		public static string GetDisplayName(string id)
		{
			return ("Building_" + id).ToPlainText();
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x0005F319 File Offset: 0x0005D519
		// (set) Token: 0x06001A3F RID: 6719 RVA: 0x0005F317 File Offset: 0x0005D517
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.ID + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x0005F330 File Offset: 0x0005D530
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0005F340 File Offset: 0x0005D540
		private void Awake()
		{
			if (this.graphicsContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Graphics Container");
				Transform transform = base.transform.Find("Graphics");
				this.graphicsContainer = ((transform != null) ? transform.gameObject : null);
			}
			if (this.functionContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Function Container");
				Transform transform2 = base.transform.Find("Function");
				this.functionContainer = ((transform2 != null) ? transform2.gameObject : null);
			}
			this.CreateAreaMesh();
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0005F3E8 File Offset: 0x0005D5E8
		private void CreateAreaMesh()
		{
			if (this.areaMesh == null)
			{
				this.areaMesh = UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.BuildingBlockAreaMesh, base.transform);
				this.areaMesh.transform.localPosition = Vector3.zero;
				this.areaMesh.transform.localRotation = quaternion.identity;
				this.areaMesh.transform.localScale = new Vector3((float)this.dimensions.x - 0.02f, 1f, (float)this.dimensions.y - 0.02f);
				this.areaMesh.transform.SetParent(this.functionContainer.transform, true);
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0005F4AA File Offset: 0x0005D6AA
		private void RegisterEvents()
		{
			BuildingManager.OnBuildingDestroyed += this.OnBuildingDestroyed;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0005F4BD File Offset: 0x0005D6BD
		private void OnBuildingDestroyed(int guid)
		{
			if (guid == this.GUID)
			{
				this.Release();
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0005F4CE File Offset: 0x0005D6CE
		private void Release()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0005F4DB File Offset: 0x0005D6DB
		private void UnregisterEvents()
		{
			BuildingManager.OnBuildingDestroyed -= this.OnBuildingDestroyed;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0005F4F0 File Offset: 0x0005D6F0
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			using (Draw.WithColor(new Color(1f, 1f, 1f, 0.5f)))
			{
				using (Draw.InLocalSpace(base.transform))
				{
					float3 rhs = this.GetOffset(BuildingRotation.Zero);
					float2 size = new float2(0.9f, 0.9f);
					for (int i = 0; i < this.Dimensions.y; i++)
					{
						for (int j = 0; j < this.Dimensions.x; j++)
						{
							Draw.SolidPlane(new float3((float)j, 0f, (float)i) - rhs, Vector3.up, size);
						}
					}
				}
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0005F5EC File Offset: 0x0005D7EC
		internal void Setup(BuildingManager.BuildingData data)
		{
			this.data = data;
			base.transform.localRotation = Quaternion.Euler(0f, (float)(data.Rotation * (BuildingRotation)90), 0f);
			this.RegisterEvents();
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0005F61F File Offset: 0x0005D81F
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0005F628 File Offset: 0x0005D828
		internal void SetupPreview()
		{
			this.functionContainer.SetActive(false);
			Collider[] componentsInChildren = this.graphicsContainer.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x040012DB RID: 4827
		[SerializeField]
		private string id;

		// Token: 0x040012DC RID: 4828
		[SerializeField]
		private Vector2Int dimensions;

		// Token: 0x040012DD RID: 4829
		[SerializeField]
		private GameObject graphicsContainer;

		// Token: 0x040012DE RID: 4830
		[SerializeField]
		private GameObject functionContainer;

		// Token: 0x040012DF RID: 4831
		private BuildingManager.BuildingData data;

		// Token: 0x040012E0 RID: 4832
		public bool unlockAchievement;

		// Token: 0x040012E1 RID: 4833
		private GameObject areaMesh;
	}
}
