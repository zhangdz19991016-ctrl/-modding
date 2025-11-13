using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class PipeHelperFunctions
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x0002E2E8 File Offset: 0x0002C4E8
	public static void RecalculateNormals(ref PipeRenderer.OrientedPoint[] points)
	{
		for (int i = 0; i < points.Length - 1; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = points[i];
			PipeRenderer.OrientedPoint orientedPoint2 = points[i + 1];
			Vector3 vector = orientedPoint2.position - orientedPoint.position;
			float d = Vector3.Dot(vector, vector);
			Vector3 vector2 = orientedPoint.rotationalAxisVector - vector * 2f / d * Vector3.Dot(vector, orientedPoint.rotationalAxisVector);
			Vector3 b = orientedPoint.tangent - vector * 2f / d * Vector3.Dot(vector, orientedPoint.tangent);
			Vector3 vector3 = orientedPoint2.tangent - b;
			float d2 = Vector3.Dot(vector3, vector3);
			orientedPoint2.rotationalAxisVector = vector2 - vector3 * 2f / d2 * Vector3.Dot(vector3, vector2);
			orientedPoint2.normal = Vector3.Cross(orientedPoint2.rotationalAxisVector, orientedPoint2.tangent);
			orientedPoint2.rotation = Quaternion.LookRotation(orientedPoint2.tangent, orientedPoint2.rotationalAxisVector);
			if (i == 0)
			{
				orientedPoint.rotation = Quaternion.LookRotation(orientedPoint.tangent, orientedPoint.rotationalAxisVector);
			}
			points[i] = orientedPoint;
			points[i + 1] = orientedPoint2;
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0002E444 File Offset: 0x0002C644
	public static void RecalculateUvs(ref PipeRenderer.OrientedPoint[] points, float factor = 1f, float offset = 0f)
	{
		float num = 0f;
		for (int i = 0; i < points.Length; i++)
		{
			if (i > 0)
			{
				num += (points[i].position - points[i - 1].position).magnitude;
			}
			points[i].uv = Vector3.one * (num * factor + offset);
		}
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0002E4B8 File Offset: 0x0002C6B8
	public static void RotatePoints(ref PipeRenderer.OrientedPoint[] points, float offset, float step)
	{
		for (int i = 0; i < points.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = points[i];
			Vector3 tangent = orientedPoint.tangent;
			Vector3 rotationalAxisVector = orientedPoint.rotationalAxisVector;
			points[i].rotationalAxisVector = Quaternion.AngleAxis(offset + step * (float)i, tangent) * rotationalAxisVector;
			points[i].rotation = Quaternion.LookRotation(tangent, points[i].rotationalAxisVector);
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0002E528 File Offset: 0x0002C728
	public static PipeRenderer.OrientedPoint[] RemoveDuplicates(PipeRenderer.OrientedPoint[] points, float threshold = 0.0001f)
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>(points);
		for (int i = 0; i < list.Count - 1; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = list[i];
			PipeRenderer.OrientedPoint orientedPoint2 = list[i + 1];
			if ((orientedPoint2.position - orientedPoint.position).magnitude < threshold)
			{
				list.RemoveAt(i);
				if (i == list.Count - 1)
				{
					PipeRenderer.OrientedPoint orientedPoint3 = list[i - 1];
					orientedPoint2.rotation = Quaternion.LookRotation(orientedPoint2.tangent, Vector3.up);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0002E5B8 File Offset: 0x0002C7B8
	public static Vector2[] GenerateUV2(PipeRenderer.OrientedPoint[] points)
	{
		float num = Mathf.Sqrt(PipeHelperFunctions.GetTotalLength(points));
		Vector2[] array = new Vector2[points.Length];
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = points[i];
			if (i > 0)
			{
				PipeRenderer.OrientedPoint orientedPoint2 = points[i - 1];
				num2 += (orientedPoint2.position - orientedPoint.position).magnitude;
			}
			if (num2 > num)
			{
				num2 -= num;
				num3++;
			}
			array[i] = new Vector2((float)num3, num2);
		}
		return array;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0002E64C File Offset: 0x0002C84C
	public static float GetTotalLength(PipeRenderer.OrientedPoint[] points)
	{
		float num = 0f;
		for (int i = 1; i < points.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = points[i - 1];
			num += (points[i].position - orientedPoint.position).magnitude;
		}
		return num;
	}
}
