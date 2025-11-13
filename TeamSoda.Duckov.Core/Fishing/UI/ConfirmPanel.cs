using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x0200021B RID: 539
	public class ConfirmPanel : MonoBehaviour
	{
		// Token: 0x06001029 RID: 4137 RVA: 0x0003F90C File Offset: 0x0003DB0C
		private void Awake()
		{
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
			this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
			this.itemDisplay.onPointerClick += this.OnItemDisplayClick;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0003F968 File Offset: 0x0003DB68
		private void OnItemDisplayClick(ItemDisplay display, PointerEventData data)
		{
			data.Use();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0003F970 File Offset: 0x0003DB70
		private void OnContinueButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = true;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0003F980 File Offset: 0x0003DB80
		private void OnQuitButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = false;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0003F990 File Offset: 0x0003DB90
		internal UniTask DoConfirmDialogue(Item catchedItem, Action<bool> confirmCallback)
		{
			ConfirmPanel.<DoConfirmDialogue>d__13 <DoConfirmDialogue>d__;
			<DoConfirmDialogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoConfirmDialogue>d__.<>4__this = this;
			<DoConfirmDialogue>d__.catchedItem = catchedItem;
			<DoConfirmDialogue>d__.confirmCallback = confirmCallback;
			<DoConfirmDialogue>d__.<>1__state = -1;
			<DoConfirmDialogue>d__.<>t__builder.Start<ConfirmPanel.<DoConfirmDialogue>d__13>(ref <DoConfirmDialogue>d__);
			return <DoConfirmDialogue>d__.<>t__builder.Task;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0003F9E4 File Offset: 0x0003DBE4
		private void Setup(Item item)
		{
			if (item == null)
			{
				this.titleText.text = this.failedTextKey.ToPlainText();
				this.itemDisplay.gameObject.SetActive(false);
				return;
			}
			this.titleText.text = this.succeedTextKey.ToPlainText();
			this.itemDisplay.Setup(item);
			this.itemDisplay.gameObject.SetActive(true);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0003FA55 File Offset: 0x0003DC55
		internal void NotifyStop()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04000CFD RID: 3325
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CFE RID: 3326
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x04000CFF RID: 3327
		[SerializeField]
		[LocalizationKey("Default")]
		private string succeedTextKey = "Fishing_Succeed";

		// Token: 0x04000D00 RID: 3328
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTextKey = "Fishing_Failed";

		// Token: 0x04000D01 RID: 3329
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000D02 RID: 3330
		[SerializeField]
		private Button continueButton;

		// Token: 0x04000D03 RID: 3331
		[SerializeField]
		private Button quitButton;

		// Token: 0x04000D04 RID: 3332
		private bool confirmed;

		// Token: 0x04000D05 RID: 3333
		private bool continueFishing;
	}
}
