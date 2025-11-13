using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class MultiCirceExtrudeShape : ShapeProvider
{
	// Token: 0x06000A53 RID: 2643 RVA: 0x0002C990 File Offset: 0x0002AB90
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)(this.subdivision * this.circles.Length);
		for (int i = 0; i < this.circles.Length; i++)
		{
			MultiCirceExtrudeShape.Circle circle = this.circles[i];
			float radius = circle.radius;
			Vector3 origin = circle.origin;
			Vector3 vector = Vector3.up * radius;
			for (int j = 0; j < this.subdivision; j++)
			{
				Quaternion rotation = Quaternion.AngleAxis(num * (float)j, Vector3.forward);
				Vector3 position = origin + rotation * vector;
				list.Add(new PipeRenderer.OrientedPoint
				{
					position = position,
					rotation = rotation,
					uv = num2 * (float)(i * this.subdivision + j) * Vector2.one
				});
			}
			list.Add(new PipeRenderer.OrientedPoint
			{
				position = vector,
				rotation = Quaternion.AngleAxis(0f, Vector3.forward),
				uv = num2 * (float)((i + 1) * this.subdivision) * Vector2.one
			});
		}
		return list.ToArray();
	}

	// Token: 0x0400090F RID: 2319
	public MultiCirceExtrudeShape.Circle[] circles;

	// Token: 0x04000910 RID: 2320
	public int subdivision = 4;

	// Token: 0x020004AD RID: 1197
	[Serializable]
	public struct Circle
	{
		// Token: 0x04001C71 RID: 7281
		public Vector3 origin;

		// Token: 0x04001C72 RID: 7282
		public float radius;
	}
}
