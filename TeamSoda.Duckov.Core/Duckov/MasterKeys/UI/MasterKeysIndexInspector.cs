using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E5 RID: 741
	public class MasterKeysIndexInspector : MonoBehaviour
	{
		// Token: 0x060017DA RID: 6106 RVA: 0x00057E51 File Offset: 0x00056051
		internal void Setup(MasterKeysIndexEntry target)
		{
			if (target == null)
			{
				this.SetupEmpty();
				return;
			}
			this.SetupNormal(target);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00057E6C File Offset: 0x0005606C
		private void SetupNormal(MasterKeysIndexEntry target)
		{
			this.targetItemID = target.ItemID;
			this.placeHolder.SetActive(false);
			this.content.SetActive(true);
			this.nameText.text = target.DisplayName;
			this.descriptionText.text = target.Description;
			this.icon.sprite = target.Icon;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00057ED0 File Offset: 0x000560D0
		private void SetupEmpty()
		{
			this.content.gameObject.SetActive(false);
			this.placeHolder.SetActive(true);
		}

		// Token: 0x04001161 RID: 4449
		[SerializeField]
		private int targetItemID;

		// Token: 0x04001162 RID: 4450
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001163 RID: 4451
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04001164 RID: 4452
		[SerializeField]
		private Image icon;

		// Token: 0x04001165 RID: 4453
		[SerializeField]
		private GameObject content;

		// Token: 0x04001166 RID: 4454
		[SerializeField]
		private GameObject placeHolder;
	}
}
