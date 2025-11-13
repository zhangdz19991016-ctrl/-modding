using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinemachine.Utility;
using Duckov.Scenes;
using Duckov.Utilities;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200027D RID: 637
	public class MiniMapDisplay : MonoBehaviour, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x0004B238 File Offset: 0x00049438
		public bool NoSignal()
		{
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in this.MapEntryPool.ActiveEntries)
			{
				if (!(miniMapDisplayEntry == null) && !(miniMapDisplayEntry.SceneID != MultiSceneCore.ActiveSubSceneID) && miniMapDisplayEntry.NoSignal())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0004B2B0 File Offset: 0x000494B0
		private PrefabPool<MiniMapDisplayEntry> MapEntryPool
		{
			get
			{
				if (this._mapEntryPool == null)
				{
					this._mapEntryPool = new PrefabPool<MiniMapDisplayEntry>(this.mapDisplayEntryPrefab, base.transform, new Action<MiniMapDisplayEntry>(this.OnGetMapEntry), null, null, true, 10, 10000, null);
				}
				return this._mapEntryPool;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0004B2FC File Offset: 0x000494FC
		private PrefabPool<PointOfInterestEntry> PointOfInterestEntryPool
		{
			get
			{
				if (this._pointOfInterestEntryPool == null)
				{
					this._pointOfInterestEntryPool = new PrefabPool<PointOfInterestEntry>(this.pointOfInterestEntryPrefab, base.transform, new Action<PointOfInterestEntry>(this.OnGetPointOfInterestEntry), null, null, true, 10, 10000, null);
				}
				return this._pointOfInterestEntryPool;
			}
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0004B345 File Offset: 0x00049545
		private void OnGetPointOfInterestEntry(PointOfInterestEntry entry)
		{
			entry.gameObject.hideFlags |= HideFlags.DontSave;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004B35B File Offset: 0x0004955B
		private void OnGetMapEntry(MiniMapDisplayEntry entry)
		{
			entry.gameObject.hideFlags |= HideFlags.DontSave;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004B371 File Offset: 0x00049571
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<MiniMapView>();
			}
			this.mapDisplayEntryPrefab.gameObject.SetActive(false);
			this.pointOfInterestEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004B3AF File Offset: 0x000495AF
		private void OnEnable()
		{
			if (this.autoSetupOnEnable)
			{
				this.AutoSetup();
			}
			this.RegisterEvents();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004B3C5 File Offset: 0x000495C5
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0004B3CD File Offset: 0x000495CD
		private void RegisterEvents()
		{
			PointsOfInterests.OnPointRegistered += this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered += this.ReleasePointOfInterest;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0004B3F1 File Offset: 0x000495F1
		private void UnregisterEvents()
		{
			PointsOfInterests.OnPointRegistered -= this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered -= this.ReleasePointOfInterest;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004B418 File Offset: 0x00049618
		internal void AutoSetup()
		{
			MiniMapSettings miniMapSettings = UnityEngine.Object.FindAnyObjectByType<MiniMapSettings>();
			if (miniMapSettings)
			{
				this.Setup(miniMapSettings);
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0004B43C File Offset: 0x0004963C
		public void Setup(IMiniMapDataProvider dataProvider)
		{
			if (dataProvider == null)
			{
				return;
			}
			this.MapEntryPool.ReleaseAll();
			bool flag = dataProvider.CombinedSprite != null;
			foreach (IMiniMapEntry cur in dataProvider.Maps)
			{
				MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.Get(null);
				miniMapDisplayEntry.Setup(this, cur, !flag);
				miniMapDisplayEntry.gameObject.SetActive(true);
			}
			if (flag)
			{
				MiniMapDisplayEntry miniMapDisplayEntry2 = this.MapEntryPool.Get(null);
				miniMapDisplayEntry2.SetupCombined(this, dataProvider);
				miniMapDisplayEntry2.gameObject.SetActive(true);
				miniMapDisplayEntry2.transform.SetAsFirstSibling();
			}
			this.SetupRotation();
			this.FitContent();
			this.HandlePointsOfInterests();
			this.HandleTeleporters();
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004B50C File Offset: 0x0004970C
		private void SetupRotation()
		{
			Vector3 to = LevelManager.Instance.GameCamera.mainVCam.transform.up.ProjectOntoPlane(Vector3.up);
			float z = Vector3.SignedAngle(Vector3.forward, to, Vector3.up);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0004B56C File Offset: 0x0004976C
		private void HandlePointsOfInterests()
		{
			this.PointOfInterestEntryPool.ReleaseAll();
			foreach (MonoBehaviour monoBehaviour in PointsOfInterests.Points)
			{
				if (!(monoBehaviour == null))
				{
					this.HandlePointOfInterest(monoBehaviour);
				}
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0004B5CC File Offset: 0x000497CC
		private void HandlePointOfInterest(MonoBehaviour poi)
		{
			int targetSceneIndex = poi.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = poi as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				targetSceneIndex = pointOfInterest.OverrideScene;
			}
			if (MultiSceneCore.ActiveSubScene == null || targetSceneIndex != MultiSceneCore.ActiveSubScene.Value.buildIndex)
			{
				return;
			}
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e.SceneReference != null && e.SceneReference.BuildIndex == targetSceneIndex);
			if (miniMapDisplayEntry == null)
			{
				return;
			}
			if (miniMapDisplayEntry.Hide)
			{
				return;
			}
			this.PointOfInterestEntryPool.Get(null).Setup(this, poi, miniMapDisplayEntry);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004B68C File Offset: 0x0004988C
		private void ReleasePointOfInterest(MonoBehaviour poi)
		{
			PointOfInterestEntry pointOfInterestEntry = this.PointOfInterestEntryPool.ActiveEntries.FirstOrDefault((PointOfInterestEntry e) => e != null && e.Target == poi);
			if (!pointOfInterestEntry)
			{
				return;
			}
			this.PointOfInterestEntryPool.Release(pointOfInterestEntry);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0004B6D8 File Offset: 0x000498D8
		private void HandleTeleporters()
		{
			this.teleporterSplines.gameObject.SetActive(false);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004B6F8 File Offset: 0x000498F8
		private void FitContent()
		{
			ReadOnlyCollection<MiniMapDisplayEntry> activeEntries = this.MapEntryPool.ActiveEntries;
			Vector2 vector = new Vector2(float.MinValue, float.MinValue);
			Vector2 vector2 = new Vector2(float.MaxValue, float.MaxValue);
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in activeEntries)
			{
				RectTransform rectTransform = miniMapDisplayEntry.transform as RectTransform;
				Vector2 vector3 = rectTransform.anchoredPosition + rectTransform.rect.min;
				Vector2 vector4 = rectTransform.anchoredPosition + rectTransform.rect.max;
				vector.x = MathF.Max(vector4.x, vector.x);
				vector.y = MathF.Max(vector4.y, vector.y);
				vector2.x = MathF.Min(vector3.x, vector2.x);
				vector2.y = MathF.Min(vector3.y, vector2.y);
			}
			Vector2 v = (vector + vector2) / 2f;
			foreach (MiniMapDisplayEntry miniMapDisplayEntry2 in activeEntries)
			{
				miniMapDisplayEntry2.transform.localPosition -= v;
			}
			(base.transform as RectTransform).sizeDelta = new Vector2(vector.x - vector2.x + this.padding * 2f, vector.y - vector2.y + this.padding * 2f);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004B8C8 File Offset: 0x00049AC8
		public bool TryConvertWorldToMinimap(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == sceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 center = MiniMapCenter.GetCenter(sceneID);
			Vector3 vector = worldPosition - center;
			Vector3 point = new Vector3(vector.x, vector.z);
			Vector3 point2 = miniMapDisplayEntry.transform.localToWorldMatrix.MultiplyPoint(point);
			result = base.transform.worldToLocalMatrix.MultiplyPoint(point2);
			return true;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004B970 File Offset: 0x00049B70
		public bool TryConvertToWorldPosition(Vector3 displayPosition, out Vector3 result)
		{
			result = default(Vector3);
			string activeSubsceneID = MultiSceneCore.ActiveSubSceneID;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == activeSubsceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 vector = miniMapDisplayEntry.transform.worldToLocalMatrix.MultiplyPoint(displayPosition);
			Vector3 b = new Vector3(vector.x, 0f, vector.y);
			Vector3 center = MiniMapCenter.GetCenter(activeSubsceneID);
			result = center + b;
			return true;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004BA08 File Offset: 0x00049C08
		internal void Center(Vector3 minimapPos)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector3 b = rectTransform.localToWorldMatrix.MultiplyPoint(minimapPos);
			Vector3 b2 = (rectTransform.parent as RectTransform).position - b;
			rectTransform.position += b2;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004BA64 File Offset: 0x00049C64
		public void OnScroll(PointerEventData eventData)
		{
			this.master.OnScroll(eventData);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0004BA85 File Offset: 0x00049C85
		[CompilerGenerated]
		internal static void <HandleTeleporters>g__ClearSplines|26_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				splineContainer.RemoveSplineAt(0);
			}
		}

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		private MiniMapView master;

		// Token: 0x04000EDB RID: 3803
		[SerializeField]
		private MiniMapDisplayEntry mapDisplayEntryPrefab;

		// Token: 0x04000EDC RID: 3804
		[SerializeField]
		private PointOfInterestEntry pointOfInterestEntryPrefab;

		// Token: 0x04000EDD RID: 3805
		[SerializeField]
		private UISplineRenderer teleporterSplines;

		// Token: 0x04000EDE RID: 3806
		[SerializeField]
		private bool autoSetupOnEnable;

		// Token: 0x04000EDF RID: 3807
		[SerializeField]
		private float padding = 25f;

		// Token: 0x04000EE0 RID: 3808
		private PrefabPool<MiniMapDisplayEntry> _mapEntryPool;

		// Token: 0x04000EE1 RID: 3809
		private PrefabPool<PointOfInterestEntry> _pointOfInterestEntryPool;
	}
}
