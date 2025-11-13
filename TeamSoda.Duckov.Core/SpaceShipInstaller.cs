using System;
using Duckov;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SpaceShipInstaller : MonoBehaviour
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001AADD File Offset: 0x00018CDD
	// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0001AAEA File Offset: 0x00018CEA
	private bool Installed
	{
		get
		{
			return SavesSystem.Load<bool>(this.saveDataKey);
		}
		set
		{
			SavesSystem.Save<bool>(this.saveDataKey, value);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001AAF8 File Offset: 0x00018CF8
	private void Awake()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(false);
		}
		this.interactable.overrideInteractName = true;
		this.interactable._overrideInteractNameKey = this.interactKey;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001AB30 File Offset: 0x00018D30
	public void Install()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(true);
		}
		AudioManager.Post("Archived/Building/Default/Constructed", base.gameObject);
		this.Installed = true;
		this.SyncGraphic(true);
		this.interactable.gameObject.SetActive(false);
		NotificationText.Push(this.notificationKey.ToPlainText());
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0001AB96 File Offset: 0x00018D96
	private void SyncGraphic(bool _installed)
	{
		if (this.builtGraphic)
		{
			this.builtGraphic.SetActive(_installed);
		}
		if (this.unbuiltGraphic)
		{
			this.unbuiltGraphic.SetActive(!_installed);
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0001ABD0 File Offset: 0x00018DD0
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.inited)
		{
			bool flag = this.Installed;
			if (flag)
			{
				TaskEvent.EmitTaskEvent(this.saveDataKey);
			}
			else if (QuestManager.IsQuestFinished(this.questID))
			{
				flag = true;
				this.Installed = true;
			}
			this.interactable.gameObject.SetActive(!flag && QuestManager.IsQuestActive(this.questID));
			this.SyncGraphic(flag);
			this.inited = true;
		}
		if (!this.Installed && !this.interactable.gameObject.activeSelf && QuestManager.IsQuestActive(this.questID))
		{
			this.interactable.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000579 RID: 1401
	[SerializeField]
	private string saveDataKey;

	// Token: 0x0400057A RID: 1402
	[SerializeField]
	private int questID;

	// Token: 0x0400057B RID: 1403
	[SerializeField]
	private InteractableBase interactable;

	// Token: 0x0400057C RID: 1404
	[SerializeField]
	[LocalizationKey("Default")]
	private string notificationKey;

	// Token: 0x0400057D RID: 1405
	[SerializeField]
	[LocalizationKey("Default")]
	private string interactKey;

	// Token: 0x0400057E RID: 1406
	private bool inited;

	// Token: 0x0400057F RID: 1407
	public GameObject builtGraphic;

	// Token: 0x04000580 RID: 1408
	public GameObject unbuiltGraphic;

	// Token: 0x04000581 RID: 1409
	public GameObject buildFx;
}
