using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class SceneLoaderProxy : MonoBehaviour
{
	// Token: 0x0600096D RID: 2413 RVA: 0x00029F51 File Offset: 0x00028151
	public void LoadScene()
	{
		if (SceneLoader.Instance == null)
		{
			Debug.LogWarning("没找到SceneLoader实例，已取消加载场景");
			return;
		}
		InputManager.DisableInput(base.gameObject);
		this.Task().Forget();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00029F84 File Offset: 0x00028184
	private UniTask Task()
	{
		SceneLoaderProxy.<Task>d__10 <Task>d__;
		<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Task>d__.<>4__this = this;
		<Task>d__.<>1__state = -1;
		<Task>d__.<>t__builder.Start<SceneLoaderProxy.<Task>d__10>(ref <Task>d__);
		return <Task>d__.<>t__builder.Task;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00029FC7 File Offset: 0x000281C7
	public void LoadMainMenu()
	{
		SceneLoader.LoadMainMenu(this.circleFade);
	}

	// Token: 0x0400086F RID: 2159
	[SceneID]
	[SerializeField]
	private string sceneID;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private bool useLocation;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	private MultiSceneLocation location;

	// Token: 0x04000872 RID: 2162
	[SerializeField]
	private bool showClosure = true;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private bool notifyEvacuation = true;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	private SceneReference overrideCurtainScene;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private bool hideTips;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private bool circleFade = true;

	// Token: 0x04000877 RID: 2167
	private bool saveToFile;
}
