using System;
using Duckov.Economy;
using TMPro;
using UnityEngine;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F4 RID: 756
	public class CellContextDisplay : MonoBehaviour
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x00059D8F File Offset: 0x00057F8F
		private Garden Garden
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Target;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600186E RID: 6254 RVA: 0x00059DAC File Offset: 0x00057FAC
		private Vector2Int HoveringCoord
		{
			get
			{
				if (this.master == null)
				{
					return default(Vector2Int);
				}
				return this.master.HoveringCoord;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x00059DDC File Offset: 0x00057FDC
		private Crop HoveringCrop
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.HoveringCrop;
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00059DF9 File Offset: 0x00057FF9
		private void Show()
		{
			this.canvasGroup.alpha = 1f;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00059E0B File Offset: 0x0005800B
		private void Hide()
		{
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00059E1D File Offset: 0x0005801D
		private void Awake()
		{
			this.master.onContextChanged += this.OnContextChanged;
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00059E36 File Offset: 0x00058036
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00059E3E File Offset: 0x0005803E
		private bool AnyContent
		{
			get
			{
				return this.plantInfo.activeSelf || this.currentCropInfo.activeSelf || this.operationInfo.activeSelf;
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00059E67 File Offset: 0x00058067
		private void Update()
		{
			if (this.master.Hovering && this.AnyContent)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
			if (this.HoveringCrop)
			{
				this.UpdateCurrentCropInfo();
			}
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00059EA0 File Offset: 0x000580A0
		private void LateUpdate()
		{
			Vector3 worldPoint = this.Garden.CoordToWorldPosition(this.HoveringCoord) + Vector3.up * 2f;
			Vector2 v = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPoint);
			base.transform.position = v;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00059EF0 File Offset: 0x000580F0
		private void OnContextChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00059EF8 File Offset: 0x000580F8
		private void Refresh()
		{
			this.HideAll();
			switch (this.master.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				if (this.HoveringCrop)
				{
					this.SetupCurrentCropInfo();
					return;
				}
				this.SetupPlantInfo();
				if (this.master.PlantingSeedTypeID > 0)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Harvest:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				if (this.HoveringCrop.Ripen)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Water:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				return;
			case GardenView.ToolType.Destroy:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00059FC1 File Offset: 0x000581C1
		private void SetupCurrentCropInfo()
		{
			this.currentCropInfo.SetActive(true);
			this.cropNameText.text = this.HoveringCrop.DisplayName;
			this.UpdateCurrentCropInfo();
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00059FEC File Offset: 0x000581EC
		private void UpdateCurrentCropInfo()
		{
			if (this.HoveringCrop == null)
			{
				return;
			}
			this.cropCountdownText.text = this.HoveringCrop.RemainingTime.ToString("hh\\:mm\\:ss");
			this.cropCountdownText.gameObject.SetActive(!this.HoveringCrop.Ripen && this.HoveringCrop.Data.watered);
			this.noWaterIndicator.SetActive(!this.HoveringCrop.Data.watered);
			this.ripenIndicator.SetActive(this.HoveringCrop.Ripen);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0005A08F File Offset: 0x0005828F
		private void SetupOperationInfo()
		{
			this.operationInfo.SetActive(true);
			this.operationNameText.text = this.master.ToolDisplayName;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0005A0B4 File Offset: 0x000582B4
		private void SetupPlantInfo()
		{
			if (!this.master.SeedSelected)
			{
				return;
			}
			this.plantInfo.SetActive(true);
			this.plantingCropNameText.text = this.master.SeedMeta.DisplayName;
			this.plantCostDisplay.Setup(new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.master.PlantingSeedTypeID, 1L)
			}), 1);
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0005A129 File Offset: 0x00058329
		private void HideAll()
		{
			this.plantInfo.SetActive(false);
			this.currentCropInfo.SetActive(false);
			this.operationInfo.SetActive(false);
			this.Hide();
		}

		// Token: 0x040011C7 RID: 4551
		[SerializeField]
		private GardenView master;

		// Token: 0x040011C8 RID: 4552
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040011C9 RID: 4553
		[SerializeField]
		private GameObject plantInfo;

		// Token: 0x040011CA RID: 4554
		[SerializeField]
		private TextMeshProUGUI plantingCropNameText;

		// Token: 0x040011CB RID: 4555
		[SerializeField]
		private CostDisplay plantCostDisplay;

		// Token: 0x040011CC RID: 4556
		[SerializeField]
		private GameObject currentCropInfo;

		// Token: 0x040011CD RID: 4557
		[SerializeField]
		private TextMeshProUGUI cropNameText;

		// Token: 0x040011CE RID: 4558
		[SerializeField]
		private TextMeshProUGUI cropCountdownText;

		// Token: 0x040011CF RID: 4559
		[SerializeField]
		private GameObject noWaterIndicator;

		// Token: 0x040011D0 RID: 4560
		[SerializeField]
		private GameObject ripenIndicator;

		// Token: 0x040011D1 RID: 4561
		[SerializeField]
		private GameObject operationInfo;

		// Token: 0x040011D2 RID: 4562
		[SerializeField]
		private TextMeshProUGUI operationNameText;
	}
}
