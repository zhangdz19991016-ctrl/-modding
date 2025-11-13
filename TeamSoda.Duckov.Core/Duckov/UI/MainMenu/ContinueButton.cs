using System;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003F2 RID: 1010
	public class ContinueButton : MonoBehaviour
	{
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x0007FC4D File Offset: 0x0007DE4D
		[SerializeField]
		private string Text_NewGame
		{
			get
			{
				return this.text_NewGame.ToPlainText();
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x0007FC5A File Offset: 0x0007DE5A
		[SerializeField]
		private string Text_Continue
		{
			get
			{
				return this.text_Continue.ToPlainText();
			}
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0007FC68 File Offset: 0x0007DE68
		private void Awake()
		{
			SavesSystem.OnSetFile += this.Refresh;
			SavesSystem.OnSaveDeleted += this.Refresh;
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0007FCC4 File Offset: 0x0007DEC4
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			SavesSystem.OnSaveDeleted -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0007FCF9 File Offset: 0x0007DEF9
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0007FD04 File Offset: 0x0007DF04
		private void OnButtonClicked()
		{
			GameManager.newBoot = true;
			if (MultiSceneCore.GetVisited("Base"))
			{
				SceneLoader.Instance.LoadBaseScene(null, true).Forget();
				return;
			}
			SavesSystem.Save<VersionData>("CreatedWithVersion", GameMetaData.Instance.Version);
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.PrologueScene, this.overrideCurtainScene, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x0007FD77 File Offset: 0x0007DF77
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0007FD80 File Offset: 0x0007DF80
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			this.text.text = (flag ? this.Text_Continue : this.Text_NewGame);
		}

		// Token: 0x040018DA RID: 6362
		[SerializeField]
		private Button button;

		// Token: 0x040018DB RID: 6363
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018DC RID: 6364
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_NewGame = "新游戏";

		// Token: 0x040018DD RID: 6365
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_Continue = "继续";

		// Token: 0x040018DE RID: 6366
		[SerializeField]
		private SceneReference overrideCurtainScene;
	}
}
