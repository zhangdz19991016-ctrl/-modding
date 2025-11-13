using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F1 RID: 753
	public class Garden : MonoBehaviour
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x00059216 File Offset: 0x00057416
		public string GardenID
		{
			get
			{
				return this.gardenID;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x0005921E File Offset: 0x0005741E
		public string SaveKey
		{
			get
			{
				return "Garden_" + this.gardenID;
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06001844 RID: 6212 RVA: 0x00059230 File Offset: 0x00057430
		// (remove) Token: 0x06001845 RID: 6213 RVA: 0x00059264 File Offset: 0x00057464
		public static event Action OnSizeAddersChanged;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06001846 RID: 6214 RVA: 0x00059298 File Offset: 0x00057498
		// (remove) Token: 0x06001847 RID: 6215 RVA: 0x000592CC File Offset: 0x000574CC
		public static event Action OnAutoWatersChanged;

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x000592FF File Offset: 0x000574FF
		// (set) Token: 0x06001849 RID: 6217 RVA: 0x00059307 File Offset: 0x00057507
		public bool AutoWater
		{
			get
			{
				return this.autoWater;
			}
			set
			{
				this.autoWater = value;
				if (value)
				{
					this.WaterAll();
				}
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0005931C File Offset: 0x0005751C
		private void WaterAll()
		{
			foreach (Crop crop in this.dictioanry.Values)
			{
				if (!(crop == null) && !crop.Watered)
				{
					crop.Water();
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600184B RID: 6219 RVA: 0x00059384 File Offset: 0x00057584
		// (set) Token: 0x0600184C RID: 6220 RVA: 0x0005938C File Offset: 0x0005758C
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.sizeDirty = true;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x0005939C File Offset: 0x0005759C
		public PrefabPool<CellDisplay> CellPool
		{
			get
			{
				if (this._cellPool == null)
				{
					this._cellPool = new PrefabPool<CellDisplay>(this.cellDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._cellPool;
			}
		}

		// Token: 0x1700045B RID: 1115
		public Crop this[Vector2Int coord]
		{
			get
			{
				Crop result;
				if (this.dictioanry.TryGetValue(coord, out result))
				{
					return result;
				}
				return null;
			}
			private set
			{
				this.dictioanry[coord] = value;
			}
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00059408 File Offset: 0x00057608
		private void Awake()
		{
			Garden.gardens[this.gardenID] = this;
			SavesSystem.OnCollectSaveData += this.Save;
			Garden.OnSizeAddersChanged += this.RefreshSize;
			Garden.OnAutoWatersChanged += this.RefreshAutowater;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00059459 File Offset: 0x00057659
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			Garden.OnSizeAddersChanged -= this.RefreshSize;
			Garden.OnAutoWatersChanged -= this.RefreshAutowater;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0005948E File Offset: 0x0005768E
		private void Start()
		{
			this.RegenerateCellDisplays();
			this.Load();
			this.RefreshSize();
			this.RefreshAutowater();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x000594A8 File Offset: 0x000576A8
		private void FixedUpdate()
		{
			if (this.sizeDirty)
			{
				this.RegenerateCellDisplays();
			}
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x000594B8 File Offset: 0x000576B8
		private void RefreshAutowater()
		{
			bool flag = false;
			using (List<IGardenAutoWaterProvider>.Enumerator enumerator = Garden.autoWaters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TakeEffect(this.gardenID))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != this.AutoWater)
			{
				this.AutoWater = flag;
			}
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00059528 File Offset: 0x00057728
		private void RefreshSize()
		{
			Vector2Int a = Vector2Int.zero;
			foreach (IGardenSizeAdder gardenSizeAdder in Garden.sizeAdders)
			{
				if (gardenSizeAdder != null)
				{
					a += gardenSizeAdder.GetValue(this.gardenID);
				}
			}
			this.Size = new Vector2Int(3 + a.x, 3 + a.y);
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x000595AC File Offset: 0x000577AC
		public void SetSize(int x, int y)
		{
			this.RegenerateCellDisplays();
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x000595B4 File Offset: 0x000577B4
		private void RegenerateCellDisplays()
		{
			this.sizeDirty = false;
			this.CellPool.ReleaseAll();
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i < vector2Int.y; i++)
			{
				for (int j = 0; j < vector2Int.x; j++)
				{
					Vector3 localPosition = this.CoordToLocalPosition(new Vector2Int(j, i));
					CellDisplay cellDisplay = this.CellPool.Get(null);
					cellDisplay.transform.localPosition = localPosition;
					cellDisplay.Setup(this, j, i);
				}
			}
			Vector3 vector = this.CoordToLocalPosition(new Vector2Int(0, 0)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			Vector3 vector2 = this.CoordToLocalPosition(new Vector2Int(vector2Int.x, vector2Int.y)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			float num = vector2.x - vector.x;
			float num2 = vector2.z - vector.z;
			Vector3 localPosition2 = vector;
			Vector3 localPosition3 = new Vector3(vector.x, 0f, vector2.z);
			Vector3 localPosition4 = vector2;
			Vector3 localPosition5 = new Vector3(vector2.x, 0f, vector.z);
			Vector3 localScale = new Vector3(1f, 1f, num2);
			Vector3 localScale2 = new Vector3(1f, 1f, num);
			Vector3 localScale3 = new Vector3(1f, 1f, num2);
			Vector3 localScale4 = new Vector3(1f, 1f, num);
			this.border00.localPosition = localPosition2;
			this.border01.localPosition = localPosition3;
			this.border11.localPosition = localPosition4;
			this.border10.localPosition = localPosition5;
			this.corner00.localPosition = localPosition2;
			this.corner01.localPosition = localPosition3;
			this.corner11.localPosition = localPosition4;
			this.corner10.localPosition = localPosition5;
			this.border00.localScale = localScale;
			this.border01.localScale = localScale2;
			this.border11.localScale = localScale3;
			this.border10.localScale = localScale4;
			this.border00.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.border01.localRotation = Quaternion.Euler(0f, 90f, 0f);
			this.border11.localRotation = Quaternion.Euler(0f, 180f, 0f);
			this.border10.localRotation = Quaternion.Euler(0f, 270f, 0f);
			Vector3 localPosition6 = (vector + vector2) / 2f;
			this.interactBox.transform.localPosition = localPosition6;
			this.interactBox.center = Vector3.zero;
			this.interactBox.size = new Vector3(num + 0.5f, 1f, num2 + 0.5f);
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x000598E2 File Offset: 0x00057AE2
		private Crop CreateCropInstance(string id)
		{
			return UnityEngine.Object.Instantiate<Crop>(this.cropTemplate, base.transform);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000598F8 File Offset: 0x00057AF8
		public void Save()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			Garden.SaveData value = new Garden.SaveData(this);
			SavesSystem.Save<Garden.SaveData>(this.SaveKey, value);
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00059920 File Offset: 0x00057B20
		public void Load()
		{
			this.Clear();
			this.dictioanry.Clear();
			Garden.SaveData saveData = SavesSystem.Load<Garden.SaveData>(this.SaveKey);
			if (saveData == null)
			{
				return;
			}
			foreach (CropData cropData in saveData.crops)
			{
				Crop crop = this.CreateCropInstance(cropData.cropID);
				crop.Initialize(this, cropData);
				this[cropData.coord] = crop;
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000599B0 File Offset: 0x00057BB0
		private void Clear()
		{
			foreach (Crop crop in this.dictioanry.Values.ToList<Crop>())
			{
				if (!(crop == null))
				{
					UnityEngine.Object.Destroy(crop.gameObject);
				}
			}
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00059A1C File Offset: 0x00057C1C
		public bool IsCoordValid(Vector2Int coord)
		{
			Vector2Int vector2Int = this.Size;
			return vector2Int.x <= 0 || vector2Int.y <= 0 || (coord.x < vector2Int.x && coord.y < vector2Int.y && coord.x >= 0 && coord.y >= 0);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00059A7F File Offset: 0x00057C7F
		public bool IsCoordOccupied(Vector2Int coord)
		{
			return this[coord] != null;
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00059A90 File Offset: 0x00057C90
		public bool Plant(Vector2Int coord, string cropID)
		{
			if (!this.IsCoordValid(coord))
			{
				return false;
			}
			if (this.IsCoordOccupied(coord))
			{
				return false;
			}
			if (!CropDatabase.IsIdValid(cropID))
			{
				Debug.Log("[Garden] Invalid crop id " + cropID, this);
				return false;
			}
			Crop crop = this.CreateCropInstance(cropID);
			crop.InitializeNew(this, cropID, coord);
			this[coord] = crop;
			if (this.autoWater)
			{
				crop.Water();
			}
			return true;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00059AF8 File Offset: 0x00057CF8
		public void Water(Vector2Int coord)
		{
			Crop crop = this[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00059B20 File Offset: 0x00057D20
		public Vector3 CoordToWorldPosition(Vector2Int coord)
		{
			Vector3 position = this.CoordToLocalPosition(coord);
			return base.transform.TransformPoint(position);
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00059B44 File Offset: 0x00057D44
		public Vector3 CoordToLocalPosition(Vector2Int coord)
		{
			Vector3 cellCenterLocal = this.grid.GetCellCenterLocal((Vector3Int)coord);
			float z = this.grid.cellSize.z;
			float y = cellCenterLocal.y - z / 2f;
			Vector3 result = cellCenterLocal;
			result.y = y;
			return result;
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00059B8C File Offset: 0x00057D8C
		public Vector2Int WorldPositionToCoord(Vector3 wPos)
		{
			Vector3 worldPosition = wPos + Vector3.up * 0.1f * this.grid.cellSize.z;
			return (Vector2Int)this.grid.WorldToCell(worldPosition);
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00059BD5 File Offset: 0x00057DD5
		internal void Release(Crop crop)
		{
			UnityEngine.Object.Destroy(crop.gameObject);
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00059BE4 File Offset: 0x00057DE4
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			float x = this.grid.cellSize.x;
			float y = this.grid.cellSize.y;
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i <= vector2Int.x; i++)
			{
				Vector3 vector = Vector3.right * (float)i * x;
				Vector3 to = vector + Vector3.forward * (float)vector2Int.y * y;
				Gizmos.DrawLine(vector, to);
			}
			for (int j = 0; j <= vector2Int.y; j++)
			{
				Vector3 vector2 = Vector3.forward * (float)j * y;
				Vector3 to2 = vector2 + Vector3.right * (float)vector2Int.x * x;
				Gizmos.DrawLine(vector2, to2);
			}
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00059CC5 File Offset: 0x00057EC5
		internal static void Register(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Add(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00059CE1 File Offset: 0x00057EE1
		internal static void Register(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Add(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00059CFD File Offset: 0x00057EFD
		internal static void Unregister(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Remove(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00059D1A File Offset: 0x00057F1A
		internal static void Unregister(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Remove(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x040011AF RID: 4527
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x040011B0 RID: 4528
		public static List<IGardenSizeAdder> sizeAdders = new List<IGardenSizeAdder>();

		// Token: 0x040011B1 RID: 4529
		public static List<IGardenAutoWaterProvider> autoWaters = new List<IGardenAutoWaterProvider>();

		// Token: 0x040011B4 RID: 4532
		public static Dictionary<string, Garden> gardens = new Dictionary<string, Garden>();

		// Token: 0x040011B5 RID: 4533
		[SerializeField]
		private Grid grid;

		// Token: 0x040011B6 RID: 4534
		[SerializeField]
		private Crop cropTemplate;

		// Token: 0x040011B7 RID: 4535
		[SerializeField]
		private Transform border00;

		// Token: 0x040011B8 RID: 4536
		[SerializeField]
		private Transform border01;

		// Token: 0x040011B9 RID: 4537
		[SerializeField]
		private Transform border11;

		// Token: 0x040011BA RID: 4538
		[SerializeField]
		private Transform border10;

		// Token: 0x040011BB RID: 4539
		[SerializeField]
		private Transform corner00;

		// Token: 0x040011BC RID: 4540
		[SerializeField]
		private Transform corner01;

		// Token: 0x040011BD RID: 4541
		[SerializeField]
		private Transform corner11;

		// Token: 0x040011BE RID: 4542
		[SerializeField]
		private Transform corner10;

		// Token: 0x040011BF RID: 4543
		[SerializeField]
		private BoxCollider interactBox;

		// Token: 0x040011C0 RID: 4544
		[SerializeField]
		private Vector2Int size;

		// Token: 0x040011C1 RID: 4545
		[SerializeField]
		private bool autoWater;

		// Token: 0x040011C2 RID: 4546
		public Vector3 cameraRigCenter = new Vector3(3f, 0f, 3f);

		// Token: 0x040011C3 RID: 4547
		private bool sizeDirty;

		// Token: 0x040011C4 RID: 4548
		[SerializeField]
		private CellDisplay cellDisplayTemplate;

		// Token: 0x040011C5 RID: 4549
		private PrefabPool<CellDisplay> _cellPool;

		// Token: 0x040011C6 RID: 4550
		private Dictionary<Vector2Int, Crop> dictioanry = new Dictionary<Vector2Int, Crop>();

		// Token: 0x0200058F RID: 1423
		[Serializable]
		private class SaveData
		{
			// Token: 0x060028DA RID: 10458 RVA: 0x000970C4 File Offset: 0x000952C4
			public SaveData(Garden garden)
			{
				this.crops = new List<CropData>();
				foreach (Crop crop in garden.dictioanry.Values)
				{
					if (!(crop == null))
					{
						this.crops.Add(crop.Data);
					}
				}
			}

			// Token: 0x04002002 RID: 8194
			[SerializeField]
			public List<CropData> crops;
		}
	}
}
