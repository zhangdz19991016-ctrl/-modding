using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B1 RID: 177
[RequireComponent(typeof(Points))]
public class SimpleTeleporterSpawner : MonoBehaviour
{
	// Token: 0x060005DD RID: 1501 RVA: 0x0001A4C4 File Offset: 0x000186C4
	private void Start()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
			if (this.points == null)
			{
				return;
			}
		}
		this.scene = SceneManager.GetActiveScene().buildIndex;
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0001A52C File Offset: 0x0001872C
	private void OnValidate()
	{
		if (this.points == null)
		{
			this.points = base.GetComponent<Points>();
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0001A548 File Offset: 0x00018748
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0001A55C File Offset: 0x0001875C
	public void StartCreate()
	{
		this.scene = SceneManager.GetActiveScene().buildIndex;
		int key = this.GetKey();
		object obj;
		if (!MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
		{
			MultiSceneCore.Instance.inLevelData.Add(key, true);
			this.Create();
			return;
		}
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001A5B4 File Offset: 0x000187B4
	private void Create()
	{
		List<Vector3> randomPoints = this.points.GetRandomPoints(this.pairCount * 2);
		for (int i = 0; i < this.pairCount; i++)
		{
			this.CreateAPair(randomPoints[i * 2], randomPoints[i * 2 + 1]);
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0001A600 File Offset: 0x00018800
	private void CreateAPair(Vector3 point1, Vector3 point2)
	{
		SimpleTeleporter simpleTeleporter = this.CreateATeleporter(point1);
		SimpleTeleporter simpleTeleporter2 = this.CreateATeleporter(point2);
		simpleTeleporter.target = simpleTeleporter2.TeleportPoint;
		simpleTeleporter2.target = simpleTeleporter.TeleportPoint;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001A635 File Offset: 0x00018835
	private SimpleTeleporter CreateATeleporter(Vector3 point)
	{
		SimpleTeleporter simpleTeleporter = UnityEngine.Object.Instantiate<SimpleTeleporter>(this.simpleTeleporterPfb);
		MultiSceneCore.MoveToActiveWithScene(simpleTeleporter.gameObject, this.scene);
		simpleTeleporter.transform.position = point;
		return simpleTeleporter;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001A660 File Offset: 0x00018860
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("SimpTeles_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x04000561 RID: 1377
	private int scene = -1;

	// Token: 0x04000562 RID: 1378
	[SerializeField]
	private int pairCount = 3;

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	private SimpleTeleporter simpleTeleporterPfb;

	// Token: 0x04000564 RID: 1380
	[SerializeField]
	private Points points;
}
