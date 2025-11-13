using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x02000335 RID: 821
	[ExecuteAlways]
	public class SceneLocationsProvider : MonoBehaviour
	{
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x00065DD9 File Offset: 0x00063FD9
		public static ReadOnlyCollection<SceneLocationsProvider> ActiveProviders
		{
			get
			{
				if (SceneLocationsProvider._activeProviders_ReadOnly == null)
				{
					SceneLocationsProvider._activeProviders_ReadOnly = new ReadOnlyCollection<SceneLocationsProvider>(SceneLocationsProvider.activeProviders);
				}
				return SceneLocationsProvider._activeProviders_ReadOnly;
			}
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x00065DF8 File Offset: 0x00063FF8
		public static SceneLocationsProvider GetProviderOfScene(SceneReference sceneReference)
		{
			if (sceneReference == null)
			{
				return null;
			}
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneReference.BuildIndex);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x00065E34 File Offset: 0x00064034
		public static SceneLocationsProvider GetProviderOfScene(Scene scene)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == scene.buildIndex);
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x00065E64 File Offset: 0x00064064
		internal static SceneLocationsProvider GetProviderOfScene(int sceneBuildIndex)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneBuildIndex);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x00065E94 File Offset: 0x00064094
		public static Transform GetLocation(SceneReference scene, string name)
		{
			if (scene.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(scene.BuildIndex, name);
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00065EAC File Offset: 0x000640AC
		public static Transform GetLocation(int sceneBuildIndex, string name)
		{
			SceneLocationsProvider providerOfScene = SceneLocationsProvider.GetProviderOfScene(sceneBuildIndex);
			if (providerOfScene == null)
			{
				return null;
			}
			return providerOfScene.GetLocation(name);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x00065ED4 File Offset: 0x000640D4
		public static Transform GetLocation(string sceneID, string name)
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(sceneID);
			if (sceneInfo == null)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(sceneInfo.BuildIndex, name);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00065EF9 File Offset: 0x000640F9
		private void Awake()
		{
			SceneLocationsProvider.activeProviders.Add(this);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00065F06 File Offset: 0x00064106
		private void OnDestroy()
		{
			SceneLocationsProvider.activeProviders.Remove(this);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00065F14 File Offset: 0x00064114
		public Transform GetLocation(string path)
		{
			string[] array = path.Split('/', StringSplitOptions.None);
			Transform transform = base.transform;
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					transform = transform.Find(text);
					if (transform == null)
					{
						return null;
					}
				}
			}
			return transform;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00065F60 File Offset: 0x00064160
		public bool TryGetPath(Transform value, out string path)
		{
			path = "";
			Transform transform = value;
			List<Transform> list = new List<Transform>();
			while (transform != null && transform != base.transform)
			{
				list.Insert(0, transform);
				transform = transform.parent;
			}
			if (transform != base.transform)
			{
				return false;
			}
			this.sb.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.sb.Append('/');
				}
				this.sb.Append(list[i].name);
			}
			path = this.sb.ToString();
			return true;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x0006600C File Offset: 0x0006420C
		[return: TupleElementNames(new string[]
		{
			"path",
			"worldPosition",
			"gameObject"
		})]
		public List<ValueTuple<string, Vector3, GameObject>> GetAllPathsAndItsPosition()
		{
			List<ValueTuple<string, Vector3, GameObject>> list = new List<ValueTuple<string, Vector3, GameObject>>();
			Stack<Transform> stack = new Stack<Transform>();
			stack.Push(base.transform);
			while (stack.Count > 0)
			{
				Transform transform = stack.Pop();
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(i);
					string item;
					if (this.TryGetPath(child, out item))
					{
						list.Add(new ValueTuple<string, Vector3, GameObject>(item, child.transform.position, child.gameObject));
						stack.Push(child);
					}
				}
			}
			return list;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0006609C File Offset: 0x0006429C
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>())
			{
				if (transform.childCount == 0)
				{
					Gizmos.DrawSphere(transform.position, 1.5f);
				}
			}
		}

		// Token: 0x040013C5 RID: 5061
		private static List<SceneLocationsProvider> activeProviders = new List<SceneLocationsProvider>();

		// Token: 0x040013C6 RID: 5062
		private static ReadOnlyCollection<SceneLocationsProvider> _activeProviders_ReadOnly;

		// Token: 0x040013C7 RID: 5063
		private StringBuilder sb = new StringBuilder();
	}
}
