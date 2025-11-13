using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000128 RID: 296
public class LoadUnitySceneOnStart : MonoBehaviour
{
	// Token: 0x060009B7 RID: 2487 RVA: 0x0002A77F File Offset: 0x0002897F
	private void Start()
	{
		SceneManager.LoadScene(this.sceneIndex);
	}

	// Token: 0x04000895 RID: 2197
	public int sceneIndex;
}
