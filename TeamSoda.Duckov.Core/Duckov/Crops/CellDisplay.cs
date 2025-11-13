using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E9 RID: 745
	public class CellDisplay : MonoBehaviour
	{
		// Token: 0x0600180A RID: 6154 RVA: 0x00058690 File Offset: 0x00056890
		internal void Setup(Garden garden, int coordx, int coordy)
		{
			this.garden = garden;
			this.coord = new Vector2Int(coordx, coordy);
			bool watered = false;
			Crop crop = garden[this.coord];
			if (crop != null)
			{
				watered = crop.Watered;
			}
			this.RefreshGraphics(watered);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000586D7 File Offset: 0x000568D7
		private void OnEnable()
		{
			Crop.onCropStatusChange += this.HandleCropEvent;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000586EA File Offset: 0x000568EA
		private void OnDisable()
		{
			Crop.onCropStatusChange -= this.HandleCropEvent;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00058700 File Offset: 0x00056900
		private void HandleCropEvent(Crop crop, Crop.CropEvent e)
		{
			if (crop == null)
			{
				return;
			}
			if (this.garden == null)
			{
				return;
			}
			CropData data = crop.Data;
			if (data.gardenID != this.garden.GardenID || data.coord != this.coord)
			{
				return;
			}
			this.RefreshGraphics(crop.Watered && e != Crop.CropEvent.BeforeDestroy && e != Crop.CropEvent.Harvest);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00058775 File Offset: 0x00056975
		private void RefreshGraphics(bool watered)
		{
			if (watered)
			{
				this.ApplyGraphicsStype(this.styleWatered);
				return;
			}
			this.ApplyGraphicsStype(this.styleDry);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00058794 File Offset: 0x00056994
		private void ApplyGraphicsStype(CellDisplay.GraphicsStyle style)
		{
			if (this.propertyBlock == null)
			{
				this.propertyBlock = new MaterialPropertyBlock();
			}
			this.propertyBlock.Clear();
			string name = "_TintColor";
			string name2 = "_Smoothness";
			this.propertyBlock.SetColor(name, style.color);
			this.propertyBlock.SetFloat(name2, style.smoothness);
			this.renderer.SetPropertyBlock(this.propertyBlock);
		}

		// Token: 0x0400117C RID: 4476
		[SerializeField]
		private Renderer renderer;

		// Token: 0x0400117D RID: 4477
		[SerializeField]
		private CellDisplay.GraphicsStyle styleDry;

		// Token: 0x0400117E RID: 4478
		[SerializeField]
		private CellDisplay.GraphicsStyle styleWatered;

		// Token: 0x0400117F RID: 4479
		private Garden garden;

		// Token: 0x04001180 RID: 4480
		private Vector2Int coord;

		// Token: 0x04001181 RID: 4481
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x02000589 RID: 1417
		[Serializable]
		private struct GraphicsStyle
		{
			// Token: 0x04001FF5 RID: 8181
			public Color color;

			// Token: 0x04001FF6 RID: 8182
			public float smoothness;
		}
	}
}
