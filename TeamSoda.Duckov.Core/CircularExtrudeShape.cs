using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
[RequireComponent(typeof(PipeRenderer))]
public class CircularExtrudeShape : ShapeProvider
{
	// Token: 0x06000A50 RID: 2640 RVA: 0x0002C808 File Offset: 0x0002AA08
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		Vector3 vector = Vector3.up * this.radius;
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)this.subdivision;
		PipeRenderer.OrientedPoint[] array = new PipeRenderer.OrientedPoint[this.subdivision + 1];
		for (int i = 0; i < this.subdivision; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(num * (float)i, Vector3.forward);
			Vector3 position = rotation * vector + this.offset;
			array[i] = new PipeRenderer.OrientedPoint
			{
				position = position,
				rotation = rotation,
				uv = num2 * (float)i * Vector2.one
			};
		}
		array[this.subdivision] = new PipeRenderer.OrientedPoint
		{
			position = vector + this.offset,
			rotation = Quaternion.AngleAxis(0f, Vector3.forward),
			uv = Vector2.one
		};
		return array;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0002C914 File Offset: 0x0002AB14
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		if (this.pipeRenderer != null && this.pipeRenderer.extrudeShapeProvider == null)
		{
			this.pipeRenderer.extrudeShapeProvider = this;
		}
	}

	// Token: 0x0400090B RID: 2315
	public PipeRenderer pipeRenderer;

	// Token: 0x0400090C RID: 2316
	public float radius = 1f;

	// Token: 0x0400090D RID: 2317
	public int subdivision = 12;

	// Token: 0x0400090E RID: 2318
	public Vector3 offset = Vector3.zero;
}
