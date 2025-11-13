using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x02000278 RID: 632
	public class MiniMapSettings : MonoBehaviour, IMiniMapDataProvider
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x0004AAF9 File Offset: 0x00048CF9
		public Sprite CombinedSprite
		{
			get
			{
				return this.combinedSprite;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0004AB01 File Offset: 0x00048D01
		public Vector3 CombinedCenter
		{
			get
			{
				return this.combinedCenter;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x0004AB09 File Offset: 0x00048D09
		public List<IMiniMapEntry> Maps
		{
			get
			{
				return this.maps.ToList<IMiniMapEntry>();
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0004AB16 File Offset: 0x00048D16
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x0004AB1D File Offset: 0x00048D1D
		public static MiniMapSettings Instance { get; private set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x0004AB28 File Offset: 0x00048D28
		public float PixelSize
		{
			get
			{
				int width = this.combinedSprite.texture.width;
				if (width > 0 && this.combinedSize > 0f)
				{
					return this.combinedSize / (float)width;
				}
				return -1f;
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004AB68 File Offset: 0x00048D68
		private void Awake()
		{
			foreach (MiniMapSettings.MapEntry mapEntry in this.maps)
			{
				SpriteRenderer offsetReference = mapEntry.offsetReference;
				if (offsetReference != null)
				{
					offsetReference.gameObject.SetActive(false);
				}
			}
			if (MiniMapSettings.Instance == null)
			{
				MiniMapSettings.Instance = this;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004ABE4 File Offset: 0x00048DE4
		public static bool TryGetMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			Vector3 a = worldPosition - mapEntry.mapWorldCenter;
			Vector3 b = mapEntry.mapWorldCenter - MiniMapSettings.Instance.combinedCenter;
			a + b;
			return true;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004AC70 File Offset: 0x00048E70
		public static bool TryGetWorldPosition(Vector3 minimapPosition, string sceneID, out Vector3 result)
		{
			result = minimapPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			result = mapEntry.mapWorldCenter + minimapPosition;
			return true;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004ACE4 File Offset: 0x00048EE4
		public static bool TryGetMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = worldPosition;
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.isLoaded)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(activeScene.buildIndex);
			return MiniMapSettings.TryGetMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004AD20 File Offset: 0x00048F20
		internal void Cache(MiniMapCenter miniMapCenter)
		{
			int scene = miniMapCenter.gameObject.scene.buildIndex;
			MiniMapSettings.MapEntry mapEntry = this.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference != null && e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == scene);
			if (mapEntry == null)
			{
				return;
			}
			mapEntry.mapWorldCenter = miniMapCenter.transform.position;
		}

		// Token: 0x04000EC4 RID: 3780
		public List<MiniMapSettings.MapEntry> maps;

		// Token: 0x04000EC5 RID: 3781
		public Vector3 combinedCenter;

		// Token: 0x04000EC6 RID: 3782
		public float combinedSize;

		// Token: 0x04000EC7 RID: 3783
		public Sprite combinedSprite;

		// Token: 0x02000550 RID: 1360
		[Serializable]
		public class MapEntry : IMiniMapEntry
		{
			// Token: 0x1700076B RID: 1899
			// (get) Token: 0x0600285E RID: 10334 RVA: 0x000945C4 File Offset: 0x000927C4
			public SceneReference SceneReference
			{
				get
				{
					SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
					if (sceneInfo == null)
					{
						return null;
					}
					return sceneInfo.SceneReference;
				}
			}

			// Token: 0x1700076C RID: 1900
			// (get) Token: 0x0600285F RID: 10335 RVA: 0x000945E8 File Offset: 0x000927E8
			public string SceneID
			{
				get
				{
					return this.sceneID;
				}
			}

			// Token: 0x1700076D RID: 1901
			// (get) Token: 0x06002860 RID: 10336 RVA: 0x000945F0 File Offset: 0x000927F0
			public Sprite Sprite
			{
				get
				{
					return this.sprite;
				}
			}

			// Token: 0x1700076E RID: 1902
			// (get) Token: 0x06002861 RID: 10337 RVA: 0x000945F8 File Offset: 0x000927F8
			public bool Hide
			{
				get
				{
					return this.hide;
				}
			}

			// Token: 0x1700076F RID: 1903
			// (get) Token: 0x06002862 RID: 10338 RVA: 0x00094600 File Offset: 0x00092800
			public bool NoSignal
			{
				get
				{
					return this.noSignal;
				}
			}

			// Token: 0x17000770 RID: 1904
			// (get) Token: 0x06002863 RID: 10339 RVA: 0x00094608 File Offset: 0x00092808
			public float PixelSize
			{
				get
				{
					int width = this.sprite.texture.width;
					if (width > 0 && this.imageWorldSize > 0f)
					{
						return this.imageWorldSize / (float)width;
					}
					return -1f;
				}
			}

			// Token: 0x17000771 RID: 1905
			// (get) Token: 0x06002864 RID: 10340 RVA: 0x00094646 File Offset: 0x00092846
			public Vector2 Offset
			{
				get
				{
					if (this.offsetReference == null)
					{
						return Vector2.zero;
					}
					return this.offsetReference.transform.localPosition;
				}
			}

			// Token: 0x06002865 RID: 10341 RVA: 0x00094671 File Offset: 0x00092871
			public MapEntry()
			{
			}

			// Token: 0x06002866 RID: 10342 RVA: 0x00094679 File Offset: 0x00092879
			public MapEntry(MiniMapSettings.MapEntry copyFrom)
			{
				this.imageWorldSize = copyFrom.imageWorldSize;
				this.sceneID = copyFrom.sceneID;
				this.sprite = copyFrom.sprite;
			}

			// Token: 0x04001EFF RID: 7935
			public float imageWorldSize;

			// Token: 0x04001F00 RID: 7936
			[SceneID]
			public string sceneID;

			// Token: 0x04001F01 RID: 7937
			public Sprite sprite;

			// Token: 0x04001F02 RID: 7938
			public SpriteRenderer offsetReference;

			// Token: 0x04001F03 RID: 7939
			public Vector3 mapWorldCenter;

			// Token: 0x04001F04 RID: 7940
			public bool hide;

			// Token: 0x04001F05 RID: 7941
			public bool noSignal;
		}

		// Token: 0x02000551 RID: 1361
		public struct Data
		{
		}
	}
}
