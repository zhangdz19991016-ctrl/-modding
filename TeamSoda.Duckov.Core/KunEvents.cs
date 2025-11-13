using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000A8 RID: 168
public class KunEvents : MonoBehaviour
{
	// Token: 0x060005BA RID: 1466 RVA: 0x00019AC6 File Offset: 0x00017CC6
	private void Awake()
	{
		this.setActiveObject.SetActive(false);
		if (!this.dialogueBubbleProxy)
		{
			this.dialogueBubbleProxy.GetComponent<DialogueBubbleProxy>();
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00019AF0 File Offset: 0x00017CF0
	public void Check()
	{
		bool flag = false;
		if (CharacterMainControl.Main == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return;
		}
		bool flag2 = customFace.ConvertToSaveData().hairID == this.hairID;
		Item armorItem = main.GetArmorItem();
		if (armorItem != null && armorItem.TypeID == this.armorID)
		{
			flag = true;
		}
		if (!flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.notRight;
		}
		else if (flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightFace;
		}
		else if (!flag2 && flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightCloth;
		}
		else
		{
			this.dialogueBubbleProxy.textKey = this.allRight;
			this.setActiveObject.SetActive(true);
		}
		this.dialogueBubbleProxy.Pop();
	}

	// Token: 0x04000533 RID: 1331
	[SerializeField]
	private int hairID = 6;

	// Token: 0x04000534 RID: 1332
	[ItemTypeID]
	[SerializeField]
	private int armorID;

	// Token: 0x04000535 RID: 1333
	public DialogueBubbleProxy dialogueBubbleProxy;

	// Token: 0x04000536 RID: 1334
	[LocalizationKey("Dialogues")]
	public string notRight;

	// Token: 0x04000537 RID: 1335
	[LocalizationKey("Dialogues")]
	public string onlyRightFace;

	// Token: 0x04000538 RID: 1336
	[LocalizationKey("Dialogues")]
	public string onlyRightCloth;

	// Token: 0x04000539 RID: 1337
	[LocalizationKey("Dialogues")]
	public string allRight;

	// Token: 0x0400053A RID: 1338
	[FormerlySerializedAs("SetActiveObject")]
	public GameObject setActiveObject;
}
