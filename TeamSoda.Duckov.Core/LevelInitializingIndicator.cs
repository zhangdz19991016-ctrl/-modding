using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class LevelInitializingIndicator : MonoBehaviour
{
	// Token: 0x06000AE4 RID: 2788 RVA: 0x0002F67C File Offset: 0x0002D87C
	private void Awake()
	{
		SceneLoader.onBeforeSetSceneActive += this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize += this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged += this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment += this.OnSetLoadingComment;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0002F6D8 File Offset: 0x0002D8D8
	private void OnSetLoadingComment(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0002F6EA File Offset: 0x0002D8EA
	private void OnCommentChanged(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0002F6FC File Offset: 0x0002D8FC
	private void OnDestroy()
	{
		SceneLoader.onBeforeSetSceneActive -= this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize -= this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged -= this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment -= this.OnSetLoadingComment;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0002F74D File Offset: 0x0002D94D
	private void SceneLoader_onBeforeSetSceneActive(SceneLoadingContext obj)
	{
		this.fadeGroup.Show();
		this.levelInitializationCommentText.text = LevelManager.LevelInitializingComment;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0002F76A File Offset: 0x0002D96A
	private void SceneLoader_onAfterSceneInitialize(SceneLoadingContext obj)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x0400097C RID: 2428
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x0400097D RID: 2429
	[SerializeField]
	private TextMeshProUGUI levelInitializationCommentText;
}
