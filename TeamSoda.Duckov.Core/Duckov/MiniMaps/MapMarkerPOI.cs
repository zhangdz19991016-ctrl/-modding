using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x02000276 RID: 630
	public class MapMarkerPOI : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0004A759 File Offset: 0x00048959
		public MapMarkerPOI.RuntimeData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0004A761 File Offset: 0x00048961
		public Sprite Icon
		{
			get
			{
				return MapMarkerManager.GetIcon(this.data.iconName);
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0004A773 File Offset: 0x00048973
		public int OverrideScene
		{
			get
			{
				return SceneInfoCollection.GetBuildIndex(this.data.overrideSceneKey);
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0004A785 File Offset: 0x00048985
		public Color Color
		{
			get
			{
				return this.data.color;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0004A792 File Offset: 0x00048992
		public Color ShadowColor
		{
			get
			{
				return Color.black;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x0004A799 File Offset: 0x00048999
		public float ScaleFactor
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004A7A0 File Offset: 0x000489A0
		public void Setup(Vector3 worldPosition, string iconName = "", string overrideScene = "", Color? color = null)
		{
			this.data = new MapMarkerPOI.RuntimeData
			{
				worldPosition = worldPosition,
				iconName = iconName,
				overrideSceneKey = overrideScene,
				color = ((color == null) ? Color.white : color.Value)
			};
			base.transform.position = worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004A80A File Offset: 0x00048A0A
		public void Setup(MapMarkerPOI.RuntimeData data)
		{
			this.data = data;
			base.transform.position = data.worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004A830 File Offset: 0x00048A30
		public void NotifyClicked(PointerEventData eventData)
		{
			MapMarkerManager.Release(this);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004A838 File Offset: 0x00048A38
		private void OnDestroy()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x04000EC1 RID: 3777
		[SerializeField]
		private MapMarkerPOI.RuntimeData data;

		// Token: 0x0200054D RID: 1357
		[Serializable]
		public struct RuntimeData
		{
			// Token: 0x04001EF9 RID: 7929
			public Vector3 worldPosition;

			// Token: 0x04001EFA RID: 7930
			public string iconName;

			// Token: 0x04001EFB RID: 7931
			public string overrideSceneKey;

			// Token: 0x04001EFC RID: 7932
			public Color color;
		}
	}
}
