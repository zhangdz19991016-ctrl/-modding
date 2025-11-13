using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000277 RID: 631
	public class MiniMapCenter : MonoBehaviour
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0004A848 File Offset: 0x00048A48
		public float WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004A850 File Offset: 0x00048A50
		private void OnEnable()
		{
			MiniMapCenter.activeMiniMapCenters.Add(this);
			if (MiniMapCenter.activeMiniMapCenters.Count > 1)
			{
				if (MiniMapCenter.activeMiniMapCenters.Find((MiniMapCenter e) => e != null && e != this && e.gameObject.scene.buildIndex == base.gameObject.scene.buildIndex))
				{
					Debug.LogError("场景 " + base.gameObject.scene.name + " 似乎存在两个MiniMapCenter！");
				}
				return;
			}
			this.CacheThisCenter();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0004A8C0 File Offset: 0x00048AC0
		private void CacheThisCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return;
			}
			Vector3 position = base.transform.position;
			instance.Cache(this);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0004A8F0 File Offset: 0x00048AF0
		private void OnDisable()
		{
			MiniMapCenter.activeMiniMapCenters.Remove(this);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004A900 File Offset: 0x00048B00
		internal static Vector3 GetCenterOfObjectScene(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			return MiniMapCenter.GetCenter(sceneBuildIndex);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0004A944 File Offset: 0x00048B44
		internal static string GetSceneID(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return null;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.Find((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry == null)
			{
				return null;
			}
			return mapEntry.sceneID;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0004A9C4 File Offset: 0x00048BC4
		internal static Vector3 GetCenter(int sceneBuildIndex)
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry != null)
			{
				return mapEntry.mapWorldCenter;
			}
			return instance.combinedCenter;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0004AA1B File Offset: 0x00048C1B
		internal static Vector3 GetCenter(string sceneID)
		{
			return MiniMapCenter.GetCenter(SceneInfoCollection.GetBuildIndex(sceneID));
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0004AA28 File Offset: 0x00048C28
		internal static Vector3 GetCombinedCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			return instance.combinedCenter;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0004AA50 File Offset: 0x00048C50
		private void OnDrawGizmosSelected()
		{
			if (this.WorldSize < 0f)
			{
				return;
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.WorldSize, 1f, this.WorldSize));
		}

		// Token: 0x04000EC2 RID: 3778
		private static List<MiniMapCenter> activeMiniMapCenters = new List<MiniMapCenter>();

		// Token: 0x04000EC3 RID: 3779
		[SerializeField]
		private float worldSize = -1f;
	}
}
