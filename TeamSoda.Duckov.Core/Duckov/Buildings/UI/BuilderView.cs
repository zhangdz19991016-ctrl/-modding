using System;
using Cinemachine;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031E RID: 798
	public class BuilderView : View, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000605B5 File Offset: 0x0005E7B5
		public static BuilderView Instance
		{
			get
			{
				return View.GetViewInstance<BuilderView>();
			}
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000605BC File Offset: 0x0005E7BC
		public void SetupAndShow(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			base.Open(null);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000605CC File Offset: 0x0005E7CC
		protected override void Awake()
		{
			base.Awake();
			this.input_Rotate.action.actionMap.Enable();
			this.input_MoveCamera.action.actionMap.Enable();
			this.selectionPanel.onButtonSelected += this.OnButtonSelected;
			this.selectionPanel.onRecycleRequested += this.OnRecycleRequested;
			BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00060648 File Offset: 0x0005E848
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			BuildingManager.ReturnBuildingsOfType(entry.Info.id, null).Forget<int>();
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00060660 File Offset: 0x0005E860
		protected override void OnDestroy()
		{
			base.OnDestroy();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingListChanged;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00060679 File Offset: 0x0005E879
		private void OnBuildingListChanged()
		{
			this.selectionPanel.Refresh();
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00060688 File Offset: 0x0005E888
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			if (!entry.CostEnough)
			{
				this.NotifyCostNotEnough(entry);
				return;
			}
			if (entry.Info.ReachedAmountLimit)
			{
				return;
			}
			this.BeginPlacing(entry.Info);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000606C4 File Offset: 0x0005E8C4
		private void NotifyCostNotEnough(BuildingBtnEntry entry)
		{
			Debug.Log("Resource not enough " + entry.Info.DisplayName);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000606EE File Offset: 0x0005E8EE
		private void SetMode(BuilderView.Mode mode)
		{
			this.placingModeInputIndicator.SetActive(false);
			this.OnExitMode(this.mode);
			this.mode = mode;
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.placingModeInputIndicator.SetActive(true);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0006072E File Offset: 0x0005E92E
		private void OnExitMode(BuilderView.Mode mode)
		{
			this.contextMenu.Hide();
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.OnExitPlacing();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00060754 File Offset: 0x0005E954
		public void BeginPlacing(BuildingInfo info)
		{
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
			this.placingBuildingInfo = info;
			this.SetMode(BuilderView.Mode.Placing);
			if (info.Prefab == null)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 没有prefab");
			}
			this.previewBuilding = UnityEngine.Object.Instantiate<Building>(info.Prefab);
			if (this.previewBuilding.ID != info.id)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 的 prefab 上的 ID 设置错误");
			}
			this.SetupPreview(this.previewBuilding);
			this.UpdatePlacing();
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0006080E File Offset: 0x0005EA0E
		public void BeginDestroying()
		{
			this.SetMode(BuilderView.Mode.Destroying);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00060817 File Offset: 0x0005EA17
		private void SetupPreview(Building previewBuilding)
		{
			if (previewBuilding == null)
			{
				return;
			}
			previewBuilding.SetupPreview();
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00060829 File Offset: 0x0005EA29
		private void OnExitPlacing()
		{
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
			GridDisplay.HidePreview();
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00060850 File Offset: 0x0005EA50
		private void Update()
		{
			switch (this.mode)
			{
			case BuilderView.Mode.None:
				this.UpdateNone();
				break;
			case BuilderView.Mode.Placing:
				this.UpdatePlacing();
				break;
			case BuilderView.Mode.Destroying:
				this.UpdateDestroying();
				break;
			}
			this.UpdateCamera();
			this.UpdateContextMenuIndicator();
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0006089C File Offset: 0x0005EA9C
		private unsafe void UpdateContextMenuIndicator()
		{
			Vector2Int coord;
			this.TryGetPointingCoord(out coord, null);
			bool flag = this.targetArea.GetBuildingInstanceAt(coord);
			bool isActiveAndEnabled = this.contextMenu.isActiveAndEnabled;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.followCursorUI.parent as RectTransform, *Mouse.current.position.value, null, out v);
			this.followCursorUI.localPosition = v;
			bool flag2 = flag && !isActiveAndEnabled;
			if (flag2 && !this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Show();
			}
			if (!flag2 && this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Hide();
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x0006094C File Offset: 0x0005EB4C
		private void UpdateNone()
		{
			if (this.input_RequestContextMenu.action.WasPressedThisFrame())
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, null))
				{
					return;
				}
				Building buildingInstanceAt = this.targetArea.GetBuildingInstanceAt(coord);
				if (buildingInstanceAt == null)
				{
					this.contextMenu.Hide();
					return;
				}
				this.contextMenu.Setup(buildingInstanceAt);
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000609A8 File Offset: 0x0005EBA8
		private void UpdateDestroying()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				GridDisplay.HidePreview();
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				GridDisplay.HidePreview();
				return;
			}
			this.gridDisplay.SetBuildingPreviewCoord(buildingAt.Coord, buildingAt.Dimensions, buildingAt.Rotation, false);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00060A00 File Offset: 0x0005EC00
		private void ConfirmDestroy()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(buildingAt.GUID, null).Forget<bool>();
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00060A48 File Offset: 0x0005EC48
		private void ConfirmPlacement()
		{
			if (this.previewBuilding == null)
			{
				Debug.Log("No Previewing Building");
				return;
			}
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
			{
				this.previewBuilding.gameObject.SetActive(false);
				Debug.Log("Mouse Not in Plane!");
				return;
			}
			if (!this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord))
			{
				Debug.Log("Invalid Placement!");
				return;
			}
			BuildingManager.BuyAndPlace(this.targetArea.AreaID, this.previewBuilding.ID, coord, this.previewRotation);
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00060AEC File Offset: 0x0005ECEC
		private void UpdatePlacing()
		{
			if (this.previewBuilding)
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
				{
					this.previewBuilding.gameObject.SetActive(false);
					return;
				}
				bool validPlacement = this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord);
				this.gridDisplay.SetBuildingPreviewCoord(coord, this.previewBuilding.Dimensions, this.previewRotation, validPlacement);
				this.ShowPreview(coord);
				if (this.input_Rotate.action.WasPressedThisFrame())
				{
					float num = this.input_Rotate.action.ReadValue<float>();
					this.previewRotation = (BuildingRotation)(((float)this.previewRotation + num + 4f) % 4f);
				}
				if (this.input_RequestContextMenu.action.WasPressedThisFrame())
				{
					this.SetMode(BuilderView.Mode.None);
					return;
				}
			}
			else
			{
				this.SetMode(BuilderView.Mode.None);
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00060BCC File Offset: 0x0005EDCC
		private void ShowPreview(Vector2Int coord)
		{
			Vector3 position = this.targetArea.CoordToWorldPosition(coord, this.previewBuilding.Dimensions, this.previewRotation);
			this.previewBuilding.transform.position = position;
			this.previewBuilding.gameObject.SetActive(true);
			Quaternion rhs = Quaternion.Euler(new Vector3(0f, (float)((BuildingRotation)90 * this.previewRotation), 0f));
			this.previewBuilding.transform.rotation = this.targetArea.transform.rotation * rhs;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00060C60 File Offset: 0x0005EE60
		public bool TryGetPointingCoord(out Vector2Int coord, Building previewBuilding = null)
		{
			coord = default(Vector2Int);
			Ray pointRay = UIInputManager.GetPointRay();
			float distance;
			if (!this.targetArea.Plane.Raycast(pointRay, out distance))
			{
				return false;
			}
			Vector3 point = pointRay.GetPoint(distance);
			if (previewBuilding != null)
			{
				coord = this.targetArea.CursorToCoord(point, previewBuilding.Dimensions, this.previewRotation);
				return true;
			}
			coord = this.targetArea.CursorToCoord(point, Vector2Int.one, BuildingRotation.Zero);
			return true;
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00060CE4 File Offset: 0x0005EEE4
		private bool IsValidPlacement(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			return this.targetArea.IsPlacementWithinRange(dimensions, rotation, coord) && !this.targetArea.AreaData.Collide(dimensions, rotation, coord) && !this.targetArea.PhysicsCollide(dimensions, rotation, coord, 0f, 2f);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00060D38 File Offset: 0x0005EF38
		protected override void OnOpen()
		{
			base.OnOpen();
			this.SetMode(BuilderView.Mode.None);
			this.fadeGroup.Show();
			this.selectionPanel.Setup(this.targetArea);
			this.gridDisplay.Setup(this.targetArea);
			this.cameraCursor = this.targetArea.transform.position;
			this.UpdateCamera();
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00060D9B File Offset: 0x0005EF9B
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			GridDisplay.Close();
			if (this.previewBuilding != null)
			{
				UnityEngine.Object.Destroy(this.previewBuilding.gameObject);
			}
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00060DD4 File Offset: 0x0005EFD4
		private void UpdateCamera()
		{
			if (this.input_MoveCamera.action.IsPressed())
			{
				Vector2 vector = this.input_MoveCamera.action.ReadValue<Vector2>();
				Transform transform = this.vcam.transform;
				float num = Mathf.Abs(Vector3.Dot(transform.forward, Vector3.up));
				float num2 = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up));
				Vector3 a = ((num > num2) ? transform.up : transform.forward).ProjectOntoPlane(Vector3.up);
				Vector3 a2 = transform.right.ProjectOntoPlane(Vector3.up);
				this.cameraCursor += (a2 * vector.x + a * vector.y) * this.cameraSpeed * Time.unscaledDeltaTime;
				this.cameraCursor.x = Mathf.Clamp(this.cameraCursor.x, this.targetArea.transform.position.x - (float)this.targetArea.Size.x, this.targetArea.transform.position.x + (float)this.targetArea.Size.x);
				this.cameraCursor.z = Mathf.Clamp(this.cameraCursor.z, this.targetArea.transform.position.z - (float)this.targetArea.Size.y, this.targetArea.transform.position.z + (float)this.targetArea.Size.y);
			}
			this.vcam.transform.position = this.cameraCursor + Quaternion.Euler(0f, this.yaw, 0f) * Quaternion.Euler(this.pitch, 0f, 0f) * Vector3.forward * this.cameraDistance;
			this.vcam.transform.LookAt(this.cameraCursor, Vector3.up);
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0006100C File Offset: 0x0005F20C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.contextMenu.Hide();
				BuilderView.Mode mode = this.mode;
				if (mode == BuilderView.Mode.Placing)
				{
					this.ConfirmPlacement();
					return;
				}
				if (mode != BuilderView.Mode.Destroying)
				{
					return;
				}
				this.ConfirmDestroy();
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00061049 File Offset: 0x0005F249
		public static void Show(BuildingArea target)
		{
			BuilderView.Instance.SetupAndShow(target);
		}

		// Token: 0x04001305 RID: 4869
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001306 RID: 4870
		[SerializeField]
		private BuildingSelectionPanel selectionPanel;

		// Token: 0x04001307 RID: 4871
		[SerializeField]
		private BuildingContextMenu contextMenu;

		// Token: 0x04001308 RID: 4872
		[SerializeField]
		private GameObject placingModeInputIndicator;

		// Token: 0x04001309 RID: 4873
		[SerializeField]
		private RectTransform followCursorUI;

		// Token: 0x0400130A RID: 4874
		[SerializeField]
		private FadeGroup hoveringBuildingFadeGroup;

		// Token: 0x0400130B RID: 4875
		[SerializeField]
		private CinemachineVirtualCamera vcam;

		// Token: 0x0400130C RID: 4876
		[SerializeField]
		private float cameraSpeed = 10f;

		// Token: 0x0400130D RID: 4877
		[SerializeField]
		private float pitch = 45f;

		// Token: 0x0400130E RID: 4878
		[SerializeField]
		private float cameraDistance = 10f;

		// Token: 0x0400130F RID: 4879
		[SerializeField]
		private float yaw = -45f;

		// Token: 0x04001310 RID: 4880
		[SerializeField]
		private Vector3 cameraCursor;

		// Token: 0x04001311 RID: 4881
		[SerializeField]
		private BuildingInfo placingBuildingInfo;

		// Token: 0x04001312 RID: 4882
		[SerializeField]
		private InputActionReference input_Rotate;

		// Token: 0x04001313 RID: 4883
		[SerializeField]
		private InputActionReference input_RequestContextMenu;

		// Token: 0x04001314 RID: 4884
		[SerializeField]
		private InputActionReference input_MoveCamera;

		// Token: 0x04001315 RID: 4885
		[SerializeField]
		private GridDisplay gridDisplay;

		// Token: 0x04001316 RID: 4886
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x04001317 RID: 4887
		[SerializeField]
		private BuilderView.Mode mode;

		// Token: 0x04001318 RID: 4888
		private Building previewBuilding;

		// Token: 0x04001319 RID: 4889
		[SerializeField]
		private BuildingRotation previewRotation;

		// Token: 0x020005C5 RID: 1477
		private enum Mode
		{
			// Token: 0x040020B0 RID: 8368
			None,
			// Token: 0x040020B1 RID: 8369
			Placing,
			// Token: 0x040020B2 RID: 8370
			Destroying
		}
	}
}
