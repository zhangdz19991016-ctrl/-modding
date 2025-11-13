using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class Points : MonoBehaviour
{
	// Token: 0x06000A75 RID: 2677 RVA: 0x0002E724 File Offset: 0x0002C924
	public void SetYtoZero()
	{
		for (int i = 0; i < this.points.Count; i++)
		{
			this.points[i] = new Vector3(this.points[i].x, 0f, this.points[i].z);
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0002E77F File Offset: 0x0002C97F
	public void RemoveAllPoints()
	{
		this.points = new List<Vector3>();
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0002E78C File Offset: 0x0002C98C
	public List<Vector3> GetRandomPoints(int count)
	{
		List<Vector3> list = new List<Vector3>();
		list.AddRange(this.points);
		List<Vector3> list2 = new List<Vector3>();
		while (list2.Count < count && list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			Vector3 item = this.PointToWorld(list[index]);
			list2.Add(item);
			list.RemoveAt(index);
		}
		return list2;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0002E7F0 File Offset: 0x0002C9F0
	public Vector3 GetRandomPoint()
	{
		int index = UnityEngine.Random.Range(0, this.points.Count);
		return this.GetPoint(index);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0002E818 File Offset: 0x0002CA18
	public Vector3 GetPoint(int index)
	{
		if (index >= this.points.Count)
		{
			return Vector3.zero;
		}
		Vector3 point = this.points[index];
		return this.PointToWorld(point);
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0002E84D File Offset: 0x0002CA4D
	private Vector3 PointToWorld(Vector3 point)
	{
		if (!this.worldSpace)
		{
			point = base.transform.TransformPoint(point);
		}
		return point;
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0002E868 File Offset: 0x0002CA68
	public void SendPointsChangedMessage()
	{
		IOnPointsChanged component = base.GetComponent<IOnPointsChanged>();
		if (component != null)
		{
			component.OnPointsChanged();
		}
	}

	// Token: 0x04000933 RID: 2355
	public List<Vector3> points;

	// Token: 0x04000934 RID: 2356
	[HideInInspector]
	public int lastSelectedPointIndex = -1;

	// Token: 0x04000935 RID: 2357
	public bool worldSpace = true;

	// Token: 0x04000936 RID: 2358
	public bool syncToSelectedPoint;
}
