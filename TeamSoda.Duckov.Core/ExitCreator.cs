using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000103 RID: 259
public class ExitCreator : MonoBehaviour
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000894 RID: 2196 RVA: 0x00026761 File Offset: 0x00024961
	private int minExitCount
	{
		get
		{
			return LevelConfig.MinExitCount;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000895 RID: 2197 RVA: 0x00026768 File Offset: 0x00024968
	private int maxExitCount
	{
		get
		{
			return LevelConfig.MaxExitCount;
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00026770 File Offset: 0x00024970
	public void Spawn()
	{
		int num = UnityEngine.Random.Range(this.minExitCount, this.maxExitCount + 1);
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		List<ValueTuple<string, SubSceneEntry.Location>> list = new List<ValueTuple<string, SubSceneEntry.Location>>();
		foreach (SubSceneEntry subSceneEntry in MultiSceneCore.Instance.SubScenes)
		{
			foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
			{
				if (this.IsPathCompitable(location))
				{
					list.Add(new ValueTuple<string, SubSceneEntry.Location>(subSceneEntry.sceneID, location));
				}
			}
		}
		list.Sort(new Comparison<ValueTuple<string, SubSceneEntry.Location>>(this.compareExit));
		if (num > list.Count)
		{
			num = list.Count;
		}
		Vector3 vector;
		MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out vector);
		int num2 = Mathf.RoundToInt((float)list.Count * 0.8f);
		if (num > num2)
		{
			num2 = num;
		}
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(0, num2);
			num2--;
			ValueTuple<string, SubSceneEntry.Location> valueTuple = list[index];
			list.RemoveAt(index);
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(valueTuple.Item1);
			this.CreateExit(valueTuple.Item2.position, sceneInfo.BuildIndex, i);
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000268F8 File Offset: 0x00024AF8
	private int compareExit([TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> a, [TupleElementNames(new string[]
	{
		"sceneID",
		"locationData"
	})] ValueTuple<string, SubSceneEntry.Location> b)
	{
		Vector3 a2;
		if (!MiniMapSettings.TryGetMinimapPosition(LevelManager.Instance.MainCharacter.transform.position, out a2))
		{
			return -1;
		}
		Vector3 b2;
		if (!MiniMapSettings.TryGetMinimapPosition(a.Item2.position, a.Item1, out b2))
		{
			return -1;
		}
		Vector3 b3;
		if (!MiniMapSettings.TryGetMinimapPosition(b.Item2.position, b.Item1, out b3))
		{
			return -1;
		}
		float num = Vector3.Distance(a2, b2);
		float num2 = Vector3.Distance(a2, b3);
		if (num > num2)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00026974 File Offset: 0x00024B74
	private bool IsPathCompitable(SubSceneEntry.Location location)
	{
		string path = location.path;
		int num = path.IndexOf('/');
		return num != -1 && path.Substring(0, num) == "Exits";
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000269AC File Offset: 0x00024BAC
	private void CreateExit(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.exitPrefab, position, Quaternion.identity);
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.MoveToActiveWithScene(go, sceneBuildIndex);
		}
		this.SpawnMapElement(position, sceneBuildIndex, debugIndex);
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000269E8 File Offset: 0x00024BE8
	private void SpawnMapElement(Vector3 position, int sceneBuildIndex, int debugIndex)
	{
		SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement").AddComponent<SimplePointOfInterest>();
		simplePointOfInterest.transform.position = position;
		if (MultiSceneCore.Instance != null)
		{
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			simplePointOfInterest.IsArea = false;
			simplePointOfInterest.ScaleFactor = 1f;
			string sceneID = SceneInfoCollection.GetSceneID(sceneBuildIndex);
			simplePointOfInterest.Setup(this.icon, this.exitNameKey, false, sceneID);
			SceneManager.MoveGameObjectToScene(simplePointOfInterest.gameObject, MultiSceneCore.MainScene.Value);
		}
	}

	// Token: 0x040007D2 RID: 2002
	public GameObject exitPrefab;

	// Token: 0x040007D3 RID: 2003
	[LocalizationKey("Default")]
	public string exitNameKey;

	// Token: 0x040007D4 RID: 2004
	[SerializeField]
	private Sprite icon;

	// Token: 0x040007D5 RID: 2005
	[SerializeField]
	private Color iconColor = Color.white;

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private Color shadowColor = Color.white;

	// Token: 0x040007D7 RID: 2007
	[SerializeField]
	private float shadowDistance;
}
