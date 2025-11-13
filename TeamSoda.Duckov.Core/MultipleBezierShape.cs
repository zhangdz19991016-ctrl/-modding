using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class MultipleBezierShape : ShapeProvider
{
	// Token: 0x06000A6A RID: 2666 RVA: 0x0002E204 File Offset: 0x0002C404
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		for (int i = 0; i < this.points.Length / 4; i++)
		{
			Vector3 p = this.points[i * 4];
			Vector3 p2 = this.points[i * 4 + 1];
			Vector3 p3 = this.points[i * 4 + 2];
			Vector3 p4 = this.points[i * 4 + 3];
			PipeRenderer.OrientedPoint[] collection = BezierSpline.GenerateShape(p, p2, p3, p4, this.subdivisions);
			if (list.Count > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.AddRange(collection);
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		PipeHelperFunctions.RecalculateUvs(ref result, 1f, 0f);
		PipeHelperFunctions.RotatePoints(ref result, this.rotationOffset, this.twist);
		return result;
	}

	// Token: 0x04000929 RID: 2345
	public Vector3[] points;

	// Token: 0x0400092A RID: 2346
	public int subdivisions = 16;

	// Token: 0x0400092B RID: 2347
	public bool lockedHandles;

	// Token: 0x0400092C RID: 2348
	public float rotationOffset;

	// Token: 0x0400092D RID: 2349
	public float twist;

	// Token: 0x0400092E RID: 2350
	public bool edit;
}
