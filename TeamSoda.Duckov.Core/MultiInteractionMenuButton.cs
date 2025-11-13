using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001FE RID: 510
public class MultiInteractionMenuButton : MonoBehaviour
{
	// Token: 0x06000F04 RID: 3844 RVA: 0x0003C32A File Offset: 0x0003A52A
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0003C348 File Offset: 0x0003A548
	private void OnButtonClicked()
	{
		if (this.target == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.Interact(this.target);
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0003C36E File Offset: 0x0003A56E
	internal void Setup(InteractableBase target)
	{
		base.gameObject.SetActive(true);
		this.target = target;
		this.text.text = target.InteractName;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0003C39F File Offset: 0x0003A59F
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0003C3AC File Offset: 0x0003A5AC
	internal void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x04000C6D RID: 3181
	[SerializeField]
	private Button button;

	// Token: 0x04000C6E RID: 3182
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000C6F RID: 3183
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C70 RID: 3184
	private InteractableBase target;
}
