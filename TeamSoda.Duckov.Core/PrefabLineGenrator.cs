using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
[RequireComponent(typeof(Points))]
[ExecuteInEditMode]
public class PrefabLineGenrator : MonoBehaviour, IOnPointsChanged
{
	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0002E89B File Offset: 0x0002CA9B
	private List<Vector3> originPoints
	{
		get
		{
			return this.points.points;
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0002E8A8 File Offset: 0x0002CAA8
	public void OnPointsChanged()
	{
	}

	// Token: 0x04000937 RID: 2359
	[SerializeField]
	private float prefabLength = 2f;

	// Token: 0x04000938 RID: 2360
	public PrefabLineGenrator.SapwnInfo spawnPrefab;

	// Token: 0x04000939 RID: 2361
	public PrefabLineGenrator.SapwnInfo startPrefab;

	// Token: 0x0400093A RID: 2362
	public PrefabLineGenrator.SapwnInfo endPrefab;

	// Token: 0x0400093B RID: 2363
	[SerializeField]
	private Points points;

	// Token: 0x0400093C RID: 2364
	[SerializeField]
	[HideInInspector]
	private List<BoxCollider> colliderObjects;

	// Token: 0x0400093D RID: 2365
	[SerializeField]
	private float updateTick = 0.5f;

	// Token: 0x0400093E RID: 2366
	private float lastModifyTime;

	// Token: 0x0400093F RID: 2367
	private bool dirty;

	// Token: 0x04000940 RID: 2368
	public List<Vector3> searchedPointsLocalSpace;

	// Token: 0x020004AF RID: 1199
	[Serializable]
	public struct SapwnInfo
	{
		// Token: 0x0600272B RID: 10027 RVA: 0x0008D334 File Offset: 0x0008B534
		public GameObject GetRandomPrefab()
		{
			if (this.prefabs.Count < 1)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < this.prefabs.Count; i++)
			{
				num += this.prefabs[i].weight;
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			for (int j = 0; j < this.prefabs.Count; j++)
			{
				if (num2 <= this.prefabs[j].weight)
				{
					return this.prefabs[j].prefab;
				}
				num2 -= this.prefabs[j].weight;
			}
			return this.prefabs[this.prefabs.Count - 1].prefab;
		}

		// Token: 0x04001C79 RID: 7289
		public List<PrefabLineGenrator.PrefabPair> prefabs;

		// Token: 0x04001C7A RID: 7290
		public float rotateOffset;

		// Token: 0x04001C7B RID: 7291
		[Range(0f, 1f)]
		public float flatten;

		// Token: 0x04001C7C RID: 7292
		public Vector3 posOffset;
	}

	// Token: 0x020004B0 RID: 1200
	[Serializable]
	public struct PrefabPair
	{
		// Token: 0x04001C7D RID: 7293
		public GameObject prefab;

		// Token: 0x04001C7E RID: 7294
		public float weight;
	}
}
