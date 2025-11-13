using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.UI.Animations;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003F3 RID: 1011
	public class SavesButton : MonoBehaviour
	{
		// Token: 0x060024A9 RID: 9385 RVA: 0x0007FDD0 File Offset: 0x0007DFD0
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
			SavesSystem.OnSetFile += this.Refresh;
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			SavesSystem.OnSaveDeleted += this.Refresh;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x0007FE2C File Offset: 0x0007E02C
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
			SavesSystem.OnSaveDeleted -= this.Refresh;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x0007FE61 File Offset: 0x0007E061
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0007FE69 File Offset: 0x0007E069
		private void OnButtonClick()
		{
			if (!this.executing)
			{
				this.SavesSelectionTask().Forget();
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0007FE80 File Offset: 0x0007E080
		private UniTask SavesSelectionTask()
		{
			SavesButton.<SavesSelectionTask>d__12 <SavesSelectionTask>d__;
			<SavesSelectionTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SavesSelectionTask>d__.<>4__this = this;
			<SavesSelectionTask>d__.<>1__state = -1;
			<SavesSelectionTask>d__.<>t__builder.Start<SavesButton.<SavesSelectionTask>d__12>(ref <SavesSelectionTask>d__);
			return <SavesSelectionTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x0007FEC3 File Offset: 0x0007E0C3
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0007FECC File Offset: 0x0007E0CC
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			string difficulty = flag ? GameRulesManager.Current.DisplayName : "";
			this.text.text = this.textFormat.Format(new
			{
				text = this.textKey.ToPlainText(),
				slotNumber = SavesSystem.CurrentSlot,
				difficulty = difficulty
			});
			bool active = flag && SavesSystem.IsOldSave(SavesSystem.CurrentSlot);
			this.oldSaveIndicator.SetActive(active);
		}

		// Token: 0x040018DF RID: 6367
		[SerializeField]
		private FadeGroup currentMenuFadeGroup;

		// Token: 0x040018E0 RID: 6368
		[SerializeField]
		private SaveSlotSelectionMenu selectionMenu;

		// Token: 0x040018E1 RID: 6369
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		private Button button;

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018E4 RID: 6372
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey = "MainMenu_SaveSlot";

		// Token: 0x040018E5 RID: 6373
		[SerializeField]
		private string textFormat = "{text}: {slotNumber}";

		// Token: 0x040018E6 RID: 6374
		private bool executing;
	}
}
