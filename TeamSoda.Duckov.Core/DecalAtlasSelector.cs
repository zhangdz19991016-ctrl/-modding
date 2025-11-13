using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x0200013F RID: 319
[DisallowMultipleComponent]
public class DecalAtlasSelector : MonoBehaviour
{
	// Token: 0x06000A3F RID: 2623 RVA: 0x0002C48C File Offset: 0x0002A68C
	private void OnValidate()
	{
		if (this.projector == null)
		{
			this.projector = base.GetComponent<DecalProjector>();
		}
		if (this.projector == null || this.rows <= 0 || this.columns <= 0)
		{
			return;
		}
		int num = this.rows * this.columns;
		int num2 = Mathf.Clamp(this.index, 0, num - 1);
		Vector2 vector = new Vector2(1f / (float)this.columns, 1f / (float)this.rows);
		Vector2 uvBias = new Vector2((float)(num2 % this.columns) * vector.x, 1f - vector.y - (float)(num2 / this.columns) * vector.y);
		this.projector.uvScale = vector;
		this.projector.uvBias = uvBias;
	}

	// Token: 0x04000901 RID: 2305
	[Header("Atlas 设置")]
	[Min(1f)]
	public int rows = 1;

	// Token: 0x04000902 RID: 2306
	[Min(1f)]
	public int columns = 1;

	// Token: 0x04000903 RID: 2307
	[Min(0f)]
	public int index;

	// Token: 0x04000904 RID: 2308
	private DecalProjector projector;
}
