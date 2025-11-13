using System;
using UnityEngine;

namespace Debugging
{
	// Token: 0x02000222 RID: 546
	public class InstantiateTiming : MonoBehaviour
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x000405CF File Offset: 0x0003E7CF
		public void InstantiatePrefab()
		{
			Debug.Log("Start Instantiate");
			UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			Debug.Log("Instantiated");
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000405F1 File Offset: 0x0003E7F1
		private void Awake()
		{
			Debug.Log("Awake");
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000405FD File Offset: 0x0003E7FD
		private void Start()
		{
			Debug.Log("Start");
		}

		// Token: 0x04000D30 RID: 3376
		public GameObject prefab;
	}
}
