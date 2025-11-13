using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000275 RID: 629
	public class MapMarkerManager : MonoBehaviour
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x0004A37D File Offset: 0x0004857D
		// (set) Token: 0x060013D5 RID: 5077 RVA: 0x0004A384 File Offset: 0x00048584
		public static MapMarkerManager Instance { get; private set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0004A38C File Offset: 0x0004858C
		public static int SelectedIconIndex
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return 0;
				}
				return MapMarkerManager.Instance.selectedIconIndex;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0004A3A7 File Offset: 0x000485A7
		public static Color SelectedColor
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return Color.white;
				}
				return MapMarkerManager.Instance.selectedColor;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0004A3C6 File Offset: 0x000485C6
		public static Sprite SelectedIcon
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				if (MapMarkerManager.Instance.icons.Count <= MapMarkerManager.SelectedIconIndex)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons[MapMarkerManager.SelectedIconIndex];
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0004A404 File Offset: 0x00048604
		public static string SelectedIconName
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				Sprite selectedIcon = MapMarkerManager.SelectedIcon;
				if (selectedIcon == null)
				{
					return null;
				}
				return selectedIcon.name;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0004A437 File Offset: 0x00048637
		public static List<Sprite> Icons
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons;
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0004A452 File Offset: 0x00048652
		private void Awake()
		{
			MapMarkerManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004A46B File Offset: 0x0004866B
		private void Start()
		{
			this.Load();
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004A473 File Offset: 0x00048673
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004A486 File Offset: 0x00048686
		private string SaveKey
		{
			get
			{
				return "MapMarkerManager_" + MultiSceneCore.MainSceneID;
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004A498 File Offset: 0x00048698
		private void Load()
		{
			this.loaded = true;
			MapMarkerManager.SaveData saveData = SavesSystem.Load<MapMarkerManager.SaveData>(this.SaveKey);
			if (saveData.pois != null)
			{
				foreach (MapMarkerPOI.RuntimeData data in saveData.pois)
				{
					MapMarkerManager.Request(data);
				}
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004A504 File Offset: 0x00048704
		private void OnCollectSaveData()
		{
			if (!this.loaded)
			{
				return;
			}
			MapMarkerManager.SaveData saveData = new MapMarkerManager.SaveData
			{
				pois = new List<MapMarkerPOI.RuntimeData>()
			};
			foreach (MapMarkerPOI mapMarkerPOI in this.pois)
			{
				if (!(mapMarkerPOI == null))
				{
					saveData.pois.Add(mapMarkerPOI.Data);
				}
			}
			SavesSystem.Save<MapMarkerManager.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0004A598 File Offset: 0x00048798
		public static void Request(MapMarkerPOI.RuntimeData data)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = UnityEngine.Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(data);
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0004A5E8 File Offset: 0x000487E8
		public static void Request(Vector3 worldPos)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = UnityEngine.Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(worldPos, MapMarkerManager.SelectedIconName, MultiSceneCore.ActiveSubSceneID, new Color?(MapMarkerManager.SelectedColor));
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0004A649 File Offset: 0x00048849
		public static void Release(MapMarkerPOI entry)
		{
			if (entry == null)
			{
				return;
			}
			if (MapMarkerManager.Instance != null)
			{
				MapMarkerManager.Instance.pois.Remove(entry);
			}
			if (entry != null)
			{
				UnityEngine.Object.Destroy(entry.gameObject);
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0004A688 File Offset: 0x00048888
		internal static Sprite GetIcon(string iconName)
		{
			if (MapMarkerManager.Instance == null)
			{
				return null;
			}
			if (MapMarkerManager.Instance.icons == null)
			{
				return null;
			}
			return MapMarkerManager.Instance.icons.Find((Sprite e) => e != null && e.name == iconName);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004A6DA File Offset: 0x000488DA
		internal static void SelectColor(Color color)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedColor = color;
			Action<Color> onColorChanged = MapMarkerManager.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(color);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0004A705 File Offset: 0x00048905
		internal static void SelectIcon(int index)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedIconIndex = index;
			Action<int> onIconChanged = MapMarkerManager.OnIconChanged;
			if (onIconChanged == null)
			{
				return;
			}
			onIconChanged(index);
		}

		// Token: 0x04000EB9 RID: 3769
		[SerializeField]
		private List<Sprite> icons = new List<Sprite>();

		// Token: 0x04000EBA RID: 3770
		[SerializeField]
		private MapMarkerPOI markerPrefab;

		// Token: 0x04000EBB RID: 3771
		[SerializeField]
		private int selectedIconIndex;

		// Token: 0x04000EBC RID: 3772
		[SerializeField]
		private Color selectedColor = Color.white;

		// Token: 0x04000EBD RID: 3773
		public static Action<int> OnIconChanged;

		// Token: 0x04000EBE RID: 3774
		public static Action<Color> OnColorChanged;

		// Token: 0x04000EBF RID: 3775
		private bool loaded;

		// Token: 0x04000EC0 RID: 3776
		private List<MapMarkerPOI> pois = new List<MapMarkerPOI>();

		// Token: 0x0200054B RID: 1355
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04001EF6 RID: 7926
			public string mainSceneName;

			// Token: 0x04001EF7 RID: 7927
			public List<MapMarkerPOI.RuntimeData> pois;
		}
	}
}
