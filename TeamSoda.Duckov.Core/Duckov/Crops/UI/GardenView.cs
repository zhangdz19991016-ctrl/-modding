using System;
using System.Runtime.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F5 RID: 757
	public class GardenView : View, IPointerClickHandler, IEventSystemHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ICursorDataProvider
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0005A15D File Offset: 0x0005835D
		// (set) Token: 0x06001880 RID: 6272 RVA: 0x0005A164 File Offset: 0x00058364
		public static GardenView Instance { get; private set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0005A16C File Offset: 0x0005836C
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x0005A174 File Offset: 0x00058374
		public Garden Target { get; private set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x0005A17D File Offset: 0x0005837D
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x0005A185 File Offset: 0x00058385
		public bool SeedSelected { get; private set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0005A18E File Offset: 0x0005838E
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x0005A196 File Offset: 0x00058396
		public int PlantingSeedTypeID
		{
			get
			{
				return this._plantingSeedTypeID;
			}
			private set
			{
				this._plantingSeedTypeID = value;
				this.SeedMeta = ItemAssetsCollection.GetMetaData(value);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x0005A1AB File Offset: 0x000583AB
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x0005A1B3 File Offset: 0x000583B3
		public ItemMetaData SeedMeta { get; private set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x0005A1BC File Offset: 0x000583BC
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x0005A1C4 File Offset: 0x000583C4
		public GardenView.ToolType Tool { get; private set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x0005A1CD File Offset: 0x000583CD
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x0005A1D5 File Offset: 0x000583D5
		public bool Hovering { get; private set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x0005A1DE File Offset: 0x000583DE
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x0005A1E6 File Offset: 0x000583E6
		public Vector2Int HoveringCoord { get; private set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x0005A1EF File Offset: 0x000583EF
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x0005A1F7 File Offset: 0x000583F7
		public Crop HoveringCrop { get; private set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x0005A200 File Offset: 0x00058400
		public string ToolDisplayName
		{
			get
			{
				switch (this.Tool)
				{
				case GardenView.ToolType.None:
					return "...";
				case GardenView.ToolType.Plant:
					return this.textKey_Plant.ToPlainText();
				case GardenView.ToolType.Harvest:
					return this.textKey_Harvest.ToPlainText();
				case GardenView.ToolType.Water:
					return this.textKey_Water.ToPlainText();
				case GardenView.ToolType.Destroy:
					return this.textKey_Destroy.ToPlainText();
				default:
					return "?";
				}
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06001892 RID: 6290 RVA: 0x0005A26C File Offset: 0x0005846C
		// (remove) Token: 0x06001893 RID: 6291 RVA: 0x0005A2A4 File Offset: 0x000584A4
		public event Action onContextChanged;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06001894 RID: 6292 RVA: 0x0005A2DC File Offset: 0x000584DC
		// (remove) Token: 0x06001895 RID: 6293 RVA: 0x0005A314 File Offset: 0x00058514
		public event Action onToolChanged;

		// Token: 0x06001896 RID: 6294 RVA: 0x0005A349 File Offset: 0x00058549
		protected override void Awake()
		{
			base.Awake();
			this.btn_ChangePlant.onClick.AddListener(new UnityAction(this.OnBtnChangePlantClicked));
			ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
			GardenView.Instance = this;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0005A384 File Offset: 0x00058584
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0005A39D File Offset: 0x0005859D
		private void OnDisable()
		{
			if (this.cellHoveringGizmos)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0005A3BD File Offset: 0x000585BD
		private void OnPlayerItemOperation()
		{
			if (base.gameObject.activeSelf && this.SeedSelected)
			{
				this.RefreshSeedAmount();
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0005A3DA File Offset: 0x000585DA
		public static void Show(Garden target)
		{
			GardenView.Instance.Target = target;
			GardenView.Instance.Open(null);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0005A3F4 File Offset: 0x000585F4
		protected override void OnOpen()
		{
			base.OnOpen();
			if (this.Target == null)
			{
				this.Target = UnityEngine.Object.FindObjectOfType<Garden>();
			}
			if (this.Target == null)
			{
				Debug.Log("No Garden instance found. Aborting..");
				base.Close();
			}
			this.fadeGroup.Show();
			this.RefreshSeedInfoDisplay();
			this.EnableCursor();
			this.SetTool(this.Tool);
			this.CenterCamera();
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0005A467 File Offset: 0x00058667
		protected override void OnClose()
		{
			base.OnClose();
			this.cropSelector.Hide();
			this.fadeGroup.Hide();
			this.ReleaseCursor();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0005A48B File Offset: 0x0005868B
		private void EnableCursor()
		{
			CursorManager.Register(this);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0005A493 File Offset: 0x00058693
		private void ReleaseCursor()
		{
			CursorManager.Unregister(this);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0005A49C File Offset: 0x0005869C
		private void ChangeCursor()
		{
			CursorManager.NotifyRefresh();
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0005A4A3 File Offset: 0x000586A3
		private void Update()
		{
			this.UpdateContext();
			this.UpdateCursor3D();
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0005A4B1 File Offset: 0x000586B1
		private void OnBtnChangePlantClicked()
		{
			this.cropSelector.Show();
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0005A4C0 File Offset: 0x000586C0
		private void OnContextChanged()
		{
			Action action = this.onContextChanged;
			if (action != null)
			{
				action();
			}
			this.RefreshHoveringGizmos();
			this.RefreshCursor();
			if (this.dragging && this.Hovering)
			{
				this.ExecuteTool(this.HoveringCoord);
			}
			this.ChangeCursor();
			this.RefreshCursor3DActive();
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0005A514 File Offset: 0x00058714
		private void RefreshCursor()
		{
			this.cursorIcon.gameObject.SetActive(false);
			this.cursorAmountDisplay.gameObject.SetActive(false);
			this.cursorItemDisplay.gameObject.SetActive(false);
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.cursorAmountDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorItemDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorIcon.sprite = this.iconPlant;
				return;
			case GardenView.ToolType.Harvest:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconHarvest;
				return;
			case GardenView.ToolType.Water:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconWater;
				return;
			case GardenView.ToolType.Destroy:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconDestroy;
				break;
			default:
				return;
			}
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0005A61C File Offset: 0x0005881C
		private void RefreshHoveringGizmos()
		{
			if (!this.cellHoveringGizmos)
			{
				return;
			}
			if (!this.Hovering || !base.enabled)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
				return;
			}
			this.cellHoveringGizmos.gameObject.SetActive(true);
			this.cellHoveringGizmos.SetParent(null);
			this.cellHoveringGizmos.localScale = Vector3.one;
			this.cellHoveringGizmos.position = this.Target.CoordToWorldPosition(this.HoveringCoord);
			this.cellHoveringGizmos.rotation = Quaternion.LookRotation(-Vector3.up);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0005A6BC File Offset: 0x000588BC
		public void SetTool(GardenView.ToolType action)
		{
			this.Tool = action;
			this.OnContextChanged();
			this.plantModePanel.SetActive(action == GardenView.ToolType.Plant);
			Action action2 = this.onToolChanged;
			if (action2 != null)
			{
				action2();
			}
			this.RefreshSeedAmount();
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0005A6F1 File Offset: 0x000588F1
		private CursorData GetCursorByTool(GardenView.ToolType action)
		{
			return null;
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0005A6F4 File Offset: 0x000588F4
		private void UpdateContext()
		{
			bool hovering = this.Hovering;
			Crop hoveringCrop = this.HoveringCrop;
			Vector2Int hoveringCoord = this.HoveringCoord;
			Vector2Int? pointingCoord = this.GetPointingCoord();
			if (pointingCoord == null)
			{
				this.HoveringCrop = null;
				return;
			}
			this.HoveringCoord = pointingCoord.Value;
			this.HoveringCrop = this.Target[this.HoveringCoord];
			this.Hovering = this.hoveringBG;
			if (!this.HoveringCrop)
			{
				this.Hovering &= this.Target.IsCoordValid(this.HoveringCoord);
			}
			if (hovering != this.HoveringCrop || hoveringCrop != this.HoveringCrop || hoveringCoord != this.HoveringCoord)
			{
				this.OnContextChanged();
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0005A7BC File Offset: 0x000589BC
		private void UpdateCursor3D()
		{
			Vector3 a;
			bool flag = this.TryPointerOnPlanePoint(UIInputManager.Point, out a);
			this.show3DCursor = (flag && this.Hovering);
			this.cursor3DTransform.gameObject.SetActive(this.show3DCursor);
			if (!flag)
			{
				return;
			}
			Vector3 position = this.cursor3DTransform.position;
			Vector3 vector = a + this.cursor3DOffset;
			Vector3 position2;
			if (this.show3DCursor)
			{
				position2 = Vector3.Lerp(position, vector, 0.25f);
			}
			else
			{
				position2 = vector;
			}
			this.cursor3DTransform.position = position2;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0005A844 File Offset: 0x00058A44
		private void RefreshCursor3DActive()
		{
			this.cursor3D_Plant.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Plant));
			this.cursor3D_Water.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Water));
			this.cursor3D_Harvest.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Harvest));
			this.cursor3D_Destory.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Destroy));
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0005A899 File Offset: 0x00058A99
		public void SelectSeed(int seedTypeID)
		{
			this.PlantingSeedTypeID = seedTypeID;
			if (seedTypeID > 0)
			{
				this.SeedSelected = true;
			}
			this.RefreshSeedInfoDisplay();
			this.OnContextChanged();
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0005A8BC File Offset: 0x00058ABC
		private void RefreshSeedInfoDisplay()
		{
			if (this.SeedSelected)
			{
				this.seedItemDisplay.Setup(this.PlantingSeedTypeID);
				this.cursorItemDisplay.Setup(this.PlantingSeedTypeID);
			}
			this.seedItemDisplay.gameObject.SetActive(this.SeedSelected);
			this.seedItemPlaceHolder.gameObject.SetActive(!this.SeedSelected);
			this.RefreshSeedAmount();
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0005A928 File Offset: 0x00058B28
		private bool TryPointerOnPlanePoint(Vector2 pointerPos, out Vector3 planePoint)
		{
			planePoint = default(Vector3);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane = new Plane(this.Target.transform.up, this.Target.transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			planePoint = ray.GetPoint(distance);
			return true;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0005A99C File Offset: 0x00058B9C
		private bool TryPointerPosToCoord(Vector2 pointerPos, out Vector2Int result)
		{
			result = default(Vector2Int);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane = new Plane(this.Target.transform.up, this.Target.transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			Vector3 point = ray.GetPoint(distance);
			result = this.Target.WorldPositionToCoord(point);
			return true;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0005AA20 File Offset: 0x00058C20
		private Vector2Int? GetPointingCoord()
		{
			Vector2Int value;
			if (!this.TryPointerPosToCoord(UIInputManager.Point, out value))
			{
				return null;
			}
			return new Vector2Int?(value);
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0005AA4C File Offset: 0x00058C4C
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2Int coord;
			if (!this.TryPointerPosToCoord(eventData.position, out coord))
			{
				return;
			}
			this.ExecuteTool(coord);
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0005AA74 File Offset: 0x00058C74
		private void ExecuteTool(Vector2Int coord)
		{
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.CropActionPlant(coord);
				return;
			case GardenView.ToolType.Harvest:
				this.CropActionHarvest(coord);
				return;
			case GardenView.ToolType.Water:
				this.CropActionWater(coord);
				return;
			case GardenView.ToolType.Destroy:
				this.CropActionDestroy(coord);
				break;
			default:
				return;
			}
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0005AAC4 File Offset: 0x00058CC4
		private void CropActionDestroy(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			if (crop.Ripen)
			{
				crop.Harvest();
				return;
			}
			crop.DestroyCrop();
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0005AB00 File Offset: 0x00058D00
		private void CropActionWater(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0005AB2C File Offset: 0x00058D2C
		private void CropActionHarvest(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Harvest();
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0005AB58 File Offset: 0x00058D58
		private void CropActionPlant(Vector2Int coord)
		{
			if (!this.Target.IsCoordValid(coord))
			{
				return;
			}
			if (this.Target[coord] != null)
			{
				return;
			}
			CropInfo? cropInfoFromSeedType = this.GetCropInfoFromSeedType(this.PlantingSeedTypeID);
			if (cropInfoFromSeedType == null)
			{
				return;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.PlantingSeedTypeID, 1L)
			});
			if (!cost.Pay(true, true))
			{
				return;
			}
			this.Target.Plant(coord, cropInfoFromSeedType.Value.id);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0005ABE8 File Offset: 0x00058DE8
		private CropInfo? GetCropInfoFromSeedType(int plantingSeedTypeID)
		{
			SeedInfo seedInfo = CropDatabase.GetSeedInfo(plantingSeedTypeID);
			if (seedInfo.cropIDs == null)
			{
				return null;
			}
			if (seedInfo.cropIDs.Count <= 0)
			{
				return null;
			}
			return CropDatabase.GetCropInfo(seedInfo.GetRandomCropID());
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0005AC34 File Offset: 0x00058E34
		public void OnPointerMove(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == this.mainEventReceiver)
			{
				this.hoveringBG = true;
				return;
			}
			this.hoveringBG = false;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0005AC6C File Offset: 0x00058E6C
		private void RefreshSeedAmount()
		{
			if (this.SeedSelected)
			{
				int itemCount = ItemUtilities.GetItemCount(this.PlantingSeedTypeID);
				this.seedAmount = itemCount;
				string text = string.Format("x{0}", itemCount);
				this.seedAmountText.text = text;
				this.cursorAmountDisplay.text = text;
				return;
			}
			this.seedAmountText.text = "";
			this.cursorAmountDisplay.text = "";
			this.seedAmount = 0;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0005ACE5 File Offset: 0x00058EE5
		public void OnPointerDown(PointerEventData eventData)
		{
			this.dragging = true;
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0005ACEE File Offset: 0x00058EEE
		public void OnPointerUp(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0005ACF7 File Offset: 0x00058EF7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0005AD00 File Offset: 0x00058F00
		private void UpdateCamera()
		{
			this.cameraRig.transform.position = this.camFocusPos;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x0005AD18 File Offset: 0x00058F18
		private void CenterCamera()
		{
			if (this.Target == null)
			{
				return;
			}
			this.camFocusPos = this.Target.transform.TransformPoint(this.Target.cameraRigCenter);
			this.UpdateCamera();
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0005AD50 File Offset: 0x00058F50
		public CursorData GetCursorData()
		{
			return this.GetCursorByTool(this.Tool);
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0005ADB8 File Offset: 0x00058FB8
		[CompilerGenerated]
		private bool <RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType toolType)
		{
			if (this.Tool != toolType)
			{
				return false;
			}
			if (!this.Hovering)
			{
				return false;
			}
			switch (toolType)
			{
			case GardenView.ToolType.None:
				return false;
			case GardenView.ToolType.Plant:
				return this.SeedSelected && this.seedAmount > 0 && !this.HoveringCrop;
			case GardenView.ToolType.Harvest:
				return this.HoveringCrop && this.HoveringCrop.Ripen;
			case GardenView.ToolType.Water:
				return this.HoveringCrop;
			case GardenView.ToolType.Destroy:
				return this.HoveringCrop;
			default:
				return false;
			}
		}

		// Token: 0x040011D4 RID: 4564
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011D5 RID: 4565
		[SerializeField]
		private GameObject mainEventReceiver;

		// Token: 0x040011D6 RID: 4566
		[SerializeField]
		private Button btn_ChangePlant;

		// Token: 0x040011D7 RID: 4567
		[SerializeField]
		private GameObject plantModePanel;

		// Token: 0x040011D8 RID: 4568
		[SerializeField]
		private ItemMetaDisplay seedItemDisplay;

		// Token: 0x040011D9 RID: 4569
		[SerializeField]
		private GameObject seedItemPlaceHolder;

		// Token: 0x040011DA RID: 4570
		[SerializeField]
		private TextMeshProUGUI seedAmountText;

		// Token: 0x040011DB RID: 4571
		[SerializeField]
		private GardenViewCropSelector cropSelector;

		// Token: 0x040011DC RID: 4572
		[SerializeField]
		private Transform cellHoveringGizmos;

		// Token: 0x040011DD RID: 4573
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Plant = "Garden_Plant";

		// Token: 0x040011DE RID: 4574
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Harvest = "Garden_Harvest";

		// Token: 0x040011DF RID: 4575
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Destroy = "Garden_Destroy";

		// Token: 0x040011E0 RID: 4576
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Water = "Garden_Water";

		// Token: 0x040011E1 RID: 4577
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_TargetOccupied = "Garden_TargetOccupied";

		// Token: 0x040011E2 RID: 4578
		[SerializeField]
		private Transform cameraRig;

		// Token: 0x040011E3 RID: 4579
		[SerializeField]
		private Image cursorIcon;

		// Token: 0x040011E4 RID: 4580
		[SerializeField]
		private TextMeshProUGUI cursorAmountDisplay;

		// Token: 0x040011E5 RID: 4581
		[SerializeField]
		private ItemMetaDisplay cursorItemDisplay;

		// Token: 0x040011E6 RID: 4582
		[SerializeField]
		private Sprite iconPlant;

		// Token: 0x040011E7 RID: 4583
		[SerializeField]
		private Sprite iconHarvest;

		// Token: 0x040011E8 RID: 4584
		[SerializeField]
		private Sprite iconWater;

		// Token: 0x040011E9 RID: 4585
		[SerializeField]
		private Sprite iconDestroy;

		// Token: 0x040011EA RID: 4586
		[SerializeField]
		private CursorData cursorPlant;

		// Token: 0x040011EB RID: 4587
		[SerializeField]
		private CursorData cursorHarvest;

		// Token: 0x040011EC RID: 4588
		[SerializeField]
		private CursorData cursorWater;

		// Token: 0x040011ED RID: 4589
		[SerializeField]
		private CursorData cursorDestroy;

		// Token: 0x040011EE RID: 4590
		[SerializeField]
		private Transform cursor3DTransform;

		// Token: 0x040011EF RID: 4591
		[SerializeField]
		private Vector3 cursor3DOffset = Vector3.up;

		// Token: 0x040011F0 RID: 4592
		[SerializeField]
		private GameObject cursor3D_Plant;

		// Token: 0x040011F1 RID: 4593
		[SerializeField]
		private GameObject cursor3D_Harvest;

		// Token: 0x040011F2 RID: 4594
		[SerializeField]
		private GameObject cursor3D_Water;

		// Token: 0x040011F3 RID: 4595
		[SerializeField]
		private GameObject cursor3D_Destory;

		// Token: 0x040011F4 RID: 4596
		private Vector3 camFocusPos;

		// Token: 0x040011F7 RID: 4599
		private int _plantingSeedTypeID;

		// Token: 0x040011FF RID: 4607
		private bool enabledCursor;

		// Token: 0x04001200 RID: 4608
		private bool show3DCursor;

		// Token: 0x04001201 RID: 4609
		private bool hoveringBG;

		// Token: 0x04001202 RID: 4610
		private int seedAmount;

		// Token: 0x04001203 RID: 4611
		private bool dragging;

		// Token: 0x02000590 RID: 1424
		public enum ToolType
		{
			// Token: 0x04002004 RID: 8196
			None,
			// Token: 0x04002005 RID: 8197
			Plant,
			// Token: 0x04002006 RID: 8198
			Harvest,
			// Token: 0x04002007 RID: 8199
			Water,
			// Token: 0x04002008 RID: 8200
			Destroy
		}
	}
}
