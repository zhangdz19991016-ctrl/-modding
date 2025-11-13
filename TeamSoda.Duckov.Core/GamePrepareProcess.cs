using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class GamePrepareProcess : MonoBehaviour
{
	// Token: 0x06000E82 RID: 3714 RVA: 0x0003AB1C File Offset: 0x00038D1C
	private UniTask Execute()
	{
		GamePrepareProcess.<Execute>d__6 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<GamePrepareProcess.<Execute>d__6>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0003AB5F File Offset: 0x00038D5F
	private void Start()
	{
		this.Execute().Forget();
	}

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private DifficultySelection difficultySelection;

	// Token: 0x04000BFE RID: 3070
	[SerializeField]
	[SceneID]
	private string introScene;

	// Token: 0x04000BFF RID: 3071
	[SerializeField]
	[SceneID]
	private string guideScene;

	// Token: 0x04000C00 RID: 3072
	public bool goToBaseSceneIfVisted;

	// Token: 0x04000C01 RID: 3073
	[SerializeField]
	[SceneID]
	private string baseScene;

	// Token: 0x04000C02 RID: 3074
	public SceneReference overrideCurtainScene;
}
