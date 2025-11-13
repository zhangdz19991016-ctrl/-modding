using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class BoundaryGenerator : MonoBehaviour
{
	// Token: 0x06000A35 RID: 2613 RVA: 0x0002C070 File Offset: 0x0002A270
	public void UpdateColliderParameters()
	{
		if (this.colliderObjects == null || this.colliderObjects.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.colliderObjects.Count; i++)
		{
			if (i < this.points.Count - 1)
			{
				BoxCollider boxCollider = this.colliderObjects[i];
				if (!(boxCollider == null))
				{
					boxCollider.gameObject.layer = base.gameObject.layer;
					Vector3 vector = base.transform.TransformPoint(this.points[i]);
					Vector3 vector2 = base.transform.TransformPoint(this.points[i + 1]);
					float y = Mathf.Min(vector.y, vector2.y);
					vector.y = y;
					vector2.y = y;
					Vector3 normalized = (vector2 - vector).normalized;
					float z = Vector3.Distance(vector, vector2);
					boxCollider.size = new Vector3(this.colliderThickness, this.colliderHeight, z);
					boxCollider.transform.forward = normalized;
					boxCollider.transform.position = (vector + vector2) / 2f + Vector3.up * 0.5f * this.colliderHeight + Vector3.up * this.yOffset + boxCollider.transform.right * 0.5f * this.colliderThickness * (this.inverseFaceDirection ? -1f : 1f);
					if (this.provideContects)
					{
						boxCollider.providesContacts = true;
					}
				}
			}
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0002C228 File Offset: 0x0002A428
	private void DestroyAllChildren()
	{
		while (base.transform.childCount > 0)
		{
			Transform child = base.transform.GetChild(0);
			child.SetParent(null);
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0002C278 File Offset: 0x0002A478
	public void UpdateColliders()
	{
		this.DestroyAllChildren();
		if (this.colliderObjects == null)
		{
			this.colliderObjects = new List<BoxCollider>();
		}
		this.colliderObjects.Clear();
		for (int i = 0; i < this.points.Count - 1; i++)
		{
			BoxCollider item = new GameObject(string.Format("Collider_{0}", i))
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<BoxCollider>();
			this.colliderObjects.Add(item);
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0002C2FC File Offset: 0x0002A4FC
	public void SetYtoZero()
	{
		for (int i = 0; i < this.points.Count; i++)
		{
			this.points[i] = new Vector3(this.points[i].x, 0f, this.points[i].z);
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0002C357 File Offset: 0x0002A557
	public void OnPointsUpdated(bool OnValidate = false)
	{
		if (this.points == null)
		{
			this.points = new List<Vector3>();
		}
		if (base.transform.childCount != this.points.Count - 1 && !OnValidate)
		{
			this.UpdateColliders();
		}
		this.UpdateColliderParameters();
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0002C395 File Offset: 0x0002A595
	public void RemoveAllPoints()
	{
		this.points.Clear();
		this.OnPointsUpdated(false);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0002C3A9 File Offset: 0x0002A5A9
	public void RespawnColliders()
	{
		this.DestroyAllChildren();
		this.colliderObjects.Clear();
		this.OnPointsUpdated(false);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0002C3C3 File Offset: 0x0002A5C3
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.OnPointsUpdated(true);
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (this.colliderObjects == null)
		{
			return;
		}
		if (this.colliderObjects.Count > 0)
		{
			foreach (Vector3 position in this.points)
			{
				Gizmos.DrawCube(base.transform.TransformPoint(position), Vector3.one * 0.15f);
			}
		}
	}

	// Token: 0x040008F9 RID: 2297
	public List<Vector3> points;

	// Token: 0x040008FA RID: 2298
	[HideInInspector]
	public int lastSelectedPointIndex = -1;

	// Token: 0x040008FB RID: 2299
	public float colliderHeight = 1f;

	// Token: 0x040008FC RID: 2300
	public float yOffset;

	// Token: 0x040008FD RID: 2301
	public float colliderThickness = 0.1f;

	// Token: 0x040008FE RID: 2302
	public bool inverseFaceDirection;

	// Token: 0x040008FF RID: 2303
	public bool provideContects;

	// Token: 0x04000900 RID: 2304
	[SerializeField]
	[HideInInspector]
	private List<BoxCollider> colliderObjects;
}
