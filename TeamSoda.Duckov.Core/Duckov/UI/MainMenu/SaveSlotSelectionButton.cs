using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003F4 RID: 1012
	public class SaveSlotSelectionButton : MonoBehaviour
	{
		// Token: 0x060024B1 RID: 9393 RVA: 0x0007FF59 File Offset: 0x0007E159
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0007FF77 File Offset: 0x0007E177
		private void OnDestroy()
		{
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0007FF79 File Offset: 0x0007E179
		private void OnEnable()
		{
			SavesSystem.OnSetFile += this.Refresh;
			this.Refresh();
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0007FF92 File Offset: 0x0007E192
		private void OnDisable()
		{
			SavesSystem.OnSetFile -= this.Refresh;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0007FFA5 File Offset: 0x0007E1A5
		private void OnButtonClick()
		{
			SavesSystem.SetFile(this.index);
			this.menu.Finish();
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0007FFBD File Offset: 0x0007E1BD
		private void OnValidate()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
			if (this.text == null)
			{
				this.text = base.GetComponentInChildren<TextMeshProUGUI>();
			}
			this.Refresh();
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0007FFFC File Offset: 0x0007E1FC
		private void Refresh()
		{
			new ES3Settings(SavesSystem.GetFilePath(this.index), null).location = ES3.Location.File;
			this.text.text = this.format.Format(new
			{
				slotText = this.slotTextKey.ToPlainText(),
				index = this.index
			});
			bool active = SavesSystem.CurrentSlot == this.index;
			GameObject gameObject = this.activeIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
			if (SavesSystem.IsOldGame(this.index))
			{
				this.difficultyText.text = (GameRulesManager.GetRuleIndexDisplayNameOfSlot(this.index) ?? "");
				this.playTimeText.gameObject.SetActive(true);
				TimeSpan realTimePlayedOfSaveSlot = GameClock.GetRealTimePlayedOfSaveSlot(this.index);
				this.playTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt((float)realTimePlayedOfSaveSlot.TotalHours), realTimePlayedOfSaveSlot.Minutes);
				bool active2 = SavesSystem.IsOldSave(this.index);
				this.oldSlotIndicator.SetActive(active2);
				long num = SavesSystem.Load<long>("SaveTime", this.index);
				string text = (num > 0L) ? DateTime.FromBinary(num).ToLocalTime().ToString("yyyy/MM/dd HH:mm") : "???";
				this.saveTimeText.text = text;
				return;
			}
			this.difficultyText.text = this.newGameTextKey.ToPlainText();
			this.playTimeText.gameObject.SetActive(false);
			this.oldSlotIndicator.SetActive(false);
			this.saveTimeText.text = "----/--/-- --:--";
		}

		// Token: 0x040018E7 RID: 6375
		[SerializeField]
		private SaveSlotSelectionMenu menu;

		// Token: 0x040018E8 RID: 6376
		[SerializeField]
		private Button button;

		// Token: 0x040018E9 RID: 6377
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018EA RID: 6378
		[SerializeField]
		private TextMeshProUGUI difficultyText;

		// Token: 0x040018EB RID: 6379
		[SerializeField]
		private TextMeshProUGUI playTimeText;

		// Token: 0x040018EC RID: 6380
		[SerializeField]
		private TextMeshProUGUI saveTimeText;

		// Token: 0x040018ED RID: 6381
		[SerializeField]
		private string slotTextKey = "MainMenu_SaveSelection_Slot";

		// Token: 0x040018EE RID: 6382
		[SerializeField]
		private string format = "{slotText} {index}";

		// Token: 0x040018EF RID: 6383
		[LocalizationKey("Default")]
		[SerializeField]
		private string newGameTextKey = "NewGame";

		// Token: 0x040018F0 RID: 6384
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		private GameObject oldSlotIndicator;

		// Token: 0x040018F2 RID: 6386
		[Min(1f)]
		[SerializeField]
		private int index;
	}
}
