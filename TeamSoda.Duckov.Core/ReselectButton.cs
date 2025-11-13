using System;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200015D RID: 349
public class ReselectButton : MonoBehaviour
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x0002F26B File Offset: 0x0002D46B
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0002F289 File Offset: 0x0002D489
	private void OnEnable()
	{
		this.setActiveGroup.SetActive(LevelManager.Instance && LevelManager.Instance.IsBaseLevel);
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0002F2AF File Offset: 0x0002D4AF
	private void OnDisable()
	{
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0002F2B4 File Offset: 0x0002D4B4
	private void OnButtonClicked()
	{
		SceneLoader.Instance.LoadScene(this.prepareSceneID, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		if (PauseMenu.Instance && PauseMenu.Instance.Shown)
		{
			PauseMenu.Hide();
		}
	}

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private GameObject setActiveGroup;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private Button button;

	// Token: 0x04000973 RID: 2419
	[SerializeField]
	[SceneID]
	private string prepareSceneID = "Prepare";
}
