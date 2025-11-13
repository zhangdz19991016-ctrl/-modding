using System;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200027F RID: 639
	public class MiniMapView : View
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0004BDC5 File Offset: 0x00049FC5
		public static MiniMapView Instance
		{
			get
			{
				return View.GetViewInstance<MiniMapView>();
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0004BDE4 File Offset: 0x00049FE4
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x0004BDCC File Offset: 0x00049FCC
		private float Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				value = Mathf.Clamp01(value);
				this._zoom = value;
				this.OnSetZoom(value);
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004BDEC File Offset: 0x00049FEC
		private void OnSetZoom(float scale)
		{
			this.RefreshZoom();
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004BDF4 File Offset: 0x00049FF4
		private void RefreshZoom()
		{
			if (this.display == null)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			Transform transform = this.display.transform;
			Vector3 vector = rectTransform.localToWorldMatrix.MultiplyPoint(rectTransform.rect.center);
			Vector3 point = transform.worldToLocalMatrix.MultiplyPoint(vector);
			this.display.transform.localScale = Vector3.one * Mathf.Lerp(this.zoomMin, this.zoomMax, this.zoomCurve.Evaluate(this.Zoom));
			Vector3 b = transform.localToWorldMatrix.MultiplyPoint(point) - vector;
			this.display.transform.position -= b;
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004BEDC File Offset: 0x0004A0DC
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.display.AutoSetup();
			MultiSceneCore instance = MultiSceneCore.Instance;
			SceneInfoEntry sceneInfoEntry = (instance != null) ? instance.SceneInfo : null;
			if (sceneInfoEntry != null)
			{
				this.mapNameText.text = sceneInfoEntry.DisplayName;
				this.mapInfoText.text = sceneInfoEntry.Description;
			}
			else
			{
				this.mapNameText.text = "";
				this.mapInfoText.text = "";
			}
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
			this.RefreshZoom();
			this.CeneterPlayer();
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004BF7B File Offset: 0x0004A17B
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004BF8E File Offset: 0x0004A18E
		protected override void Awake()
		{
			base.Awake();
			this.zoomSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnZoomSliderValueChanged));
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004BFB2 File Offset: 0x0004A1B2
		private void FixedUpdate()
		{
			this.RefreshNoSignalIndicator();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004BFBA File Offset: 0x0004A1BA
		private void RefreshNoSignalIndicator()
		{
			this.noSignalIndicator.SetActive(this.display.NoSignal());
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004BFD2 File Offset: 0x0004A1D2
		private void OnZoomSliderValueChanged(float value)
		{
			this.Zoom = value;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004BFDB File Offset: 0x0004A1DB
		public static void Show()
		{
			if (MiniMapView.Instance == null)
			{
				return;
			}
			if (MiniMapSettings.Instance == null)
			{
				return;
			}
			MiniMapView.Instance.Open(null);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004C004 File Offset: 0x0004A204
		public void CeneterPlayer()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			Vector3 minimapPos;
			if (!this.display.TryConvertWorldToMinimap(main.transform.position, SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex), out minimapPos))
			{
				return;
			}
			this.display.Center(minimapPos);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004C05A File Offset: 0x0004A25A
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = default(Vector3);
			return !(MiniMapView.Instance == null) && MiniMapView.Instance.display.TryConvertWorldToMinimap(worldPosition, sceneID, out result);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004C084 File Offset: 0x0004A284
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = default(Vector3);
			if (MiniMapView.Instance == null)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex);
			return MiniMapView.TryConvertWorldToMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0004C0C2 File Offset: 0x0004A2C2
		internal void OnScroll(PointerEventData eventData)
		{
			this.Zoom += eventData.scrollDelta.y * this.scrollSensitivity;
			eventData.Use();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0004C0E9 File Offset: 0x0004A2E9
		internal static void RequestMarkPOI(Vector3 worldPos)
		{
			MapMarkerManager.Request(worldPos);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0004C0F1 File Offset: 0x0004A2F1
		public void LoadData(PackedMapData mapData)
		{
			if (mapData == null)
			{
				return;
			}
			this.display.Setup(mapData);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0004C109 File Offset: 0x0004A309
		public void LoadCurrent()
		{
			this.display.AutoSetup();
		}

		// Token: 0x04000EE9 RID: 3817
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000EEA RID: 3818
		[SerializeField]
		private MiniMapDisplay display;

		// Token: 0x04000EEB RID: 3819
		[SerializeField]
		private TextMeshProUGUI mapNameText;

		// Token: 0x04000EEC RID: 3820
		[SerializeField]
		private TextMeshProUGUI mapInfoText;

		// Token: 0x04000EED RID: 3821
		[SerializeField]
		private Slider zoomSlider;

		// Token: 0x04000EEE RID: 3822
		[SerializeField]
		private float zoomMin = 5f;

		// Token: 0x04000EEF RID: 3823
		[SerializeField]
		private float zoomMax = 20f;

		// Token: 0x04000EF0 RID: 3824
		[SerializeField]
		[HideInInspector]
		private float _zoom = 5f;

		// Token: 0x04000EF1 RID: 3825
		[SerializeField]
		[Range(0f, 0.01f)]
		private float scrollSensitivity = 0.01f;

		// Token: 0x04000EF2 RID: 3826
		[SerializeField]
		private SimplePointOfInterest markPoiTemplate;

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		private AnimationCurve zoomCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000EF4 RID: 3828
		[SerializeField]
		private GameObject noSignalIndicator;
	}
}
