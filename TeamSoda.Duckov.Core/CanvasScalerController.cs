using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000155 RID: 341
public class CanvasScalerController : MonoBehaviour
{
	// Token: 0x06000A89 RID: 2697 RVA: 0x0002E995 File Offset: 0x0002CB95
	private void OnValidate()
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0002E9B1 File Offset: 0x0002CBB1
	private void Awake()
	{
		this.OnValidate();
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0002E9B9 File Offset: 0x0002CBB9
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0002E9C4 File Offset: 0x0002CBC4
	private void Refresh()
	{
		if (this.canvasScaler == null)
		{
			return;
		}
		Vector2Int currentResolution = this.GetCurrentResolution();
		float num = (float)currentResolution.x / (float)currentResolution.y;
		Vector2 referenceResolution = this.canvasScaler.referenceResolution;
		float num2 = referenceResolution.x / referenceResolution.y;
		if (num > num2)
		{
			this.canvasScaler.matchWidthOrHeight = 1f;
		}
		else
		{
			this.canvasScaler.matchWidthOrHeight = 0f;
		}
		this.cachedResolution = currentResolution;
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0002EA3F File Offset: 0x0002CC3F
	private void FixedUpdate()
	{
		if (!this.ResolutionMatch())
		{
			this.Refresh();
		}
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0002EA50 File Offset: 0x0002CC50
	private bool ResolutionMatch()
	{
		Vector2Int currentResolution = this.GetCurrentResolution();
		return this.cachedResolution.x == currentResolution.x && this.cachedResolution.y == currentResolution.y;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0002EA8E File Offset: 0x0002CC8E
	private Vector2Int GetCurrentResolution()
	{
		return new Vector2Int(Display.main.renderingWidth, Display.main.renderingHeight);
	}

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	private CanvasScaler canvasScaler;

	// Token: 0x04000945 RID: 2373
	private Vector2Int cachedResolution;
}
