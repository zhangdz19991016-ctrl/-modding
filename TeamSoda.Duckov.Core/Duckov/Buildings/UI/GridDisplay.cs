using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000323 RID: 803
	public class GridDisplay : MonoBehaviour
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00061860 File Offset: 0x0005FA60
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x00061867 File Offset: 0x0005FA67
		public static GridDisplay Instance { get; private set; }

		// Token: 0x06001ADC RID: 6876 RVA: 0x0006186F File Offset: 0x0005FA6F
		private void Awake()
		{
			GridDisplay.Instance = this;
			GridDisplay.Close();
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0006187C File Offset: 0x0005FA7C
		public void Setup(BuildingArea buildingArea)
		{
			Vector2Int lowerLeftCorner = buildingArea.LowerLeftCorner;
			Vector4 value = new Vector4((float)lowerLeftCorner.x, (float)lowerLeftCorner.y, (float)(buildingArea.Size.x * 2 - 1), (float)(buildingArea.Size.y * 2 - 1));
			Shader.SetGlobalVector("BuildingGrid_AreaPosAndSize", value);
			GridDisplay.ShowGrid();
			GridDisplay.HidePreview();
			GridDisplay.ShowGrid();
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000618E7 File Offset: 0x0005FAE7
		public static void Close()
		{
			GridDisplay.HidePreview();
			GridDisplay.HideGrid();
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000618F4 File Offset: 0x0005FAF4
		public static UniTask SetGridShowHide(bool show, AnimationCurve curve, float duration)
		{
			GridDisplay.<SetGridShowHide>d__12 <SetGridShowHide>d__;
			<SetGridShowHide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SetGridShowHide>d__.show = show;
			<SetGridShowHide>d__.curve = curve;
			<SetGridShowHide>d__.duration = duration;
			<SetGridShowHide>d__.<>1__state = -1;
			<SetGridShowHide>d__.<>t__builder.Start<GridDisplay.<SetGridShowHide>d__12>(ref <SetGridShowHide>d__);
			return <SetGridShowHide>d__.<>t__builder.Task;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00061947 File Offset: 0x0005FB47
		public static void HideGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(false, GridDisplay.Instance.hideCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 0f);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00061984 File Offset: 0x0005FB84
		public static void ShowGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(true, GridDisplay.Instance.showCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 1f);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000619C1 File Offset: 0x0005FBC1
		public static void HidePreview()
		{
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", Vector4.zero);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000619D4 File Offset: 0x0005FBD4
		internal void SetBuildingPreviewCoord(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation, bool validPlacement)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			Vector4 value = new Vector4((float)coord.x, (float)coord.y, (float)dimensions.x, (float)dimensions.y);
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", value);
			Shader.SetGlobalFloat("BuildingGrid_CanBuild", (float)(validPlacement ? 1 : 0));
		}

		// Token: 0x04001337 RID: 4919
		[HideInInspector]
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x04001338 RID: 4920
		[SerializeField]
		private float animationDuration;

		// Token: 0x04001339 RID: 4921
		[SerializeField]
		private AnimationCurve showCurve;

		// Token: 0x0400133A RID: 4922
		[SerializeField]
		private AnimationCurve hideCurve;

		// Token: 0x0400133B RID: 4923
		private static int gridShowHideTaskToken;
	}
}
