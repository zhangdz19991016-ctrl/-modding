using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem.Data;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B6 RID: 950
	public class StorageDockEntry : MonoBehaviour
	{
		// Token: 0x06002246 RID: 8774 RVA: 0x00077B12 File Offset: 0x00075D12
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00077B30 File Offset: 0x00075D30
		private void OnButtonClick()
		{
			if (!PlayerStorage.IsAccessableAndNotFull())
			{
				return;
			}
			this.TakeTask().Forget();
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00077B48 File Offset: 0x00075D48
		private UniTask TakeTask()
		{
			StorageDockEntry.<TakeTask>d__13 <TakeTask>d__;
			<TakeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TakeTask>d__.<>4__this = this;
			<TakeTask>d__.<>1__state = -1;
			<TakeTask>d__.<>t__builder.Start<StorageDockEntry.<TakeTask>d__13>(ref <TakeTask>d__);
			return <TakeTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00077B8C File Offset: 0x00075D8C
		public void Setup(int index, ItemTreeData item)
		{
			this.index = index;
			this.item = item;
			ItemTreeData.DataEntry rootData = item.RootData;
			this.itemDisplay.Setup(rootData.typeID);
			int stackCount = rootData.StackCount;
			if (stackCount > 1)
			{
				this.countText.text = stackCount.ToString();
				this.countDisplay.SetActive(true);
			}
			else
			{
				this.countDisplay.SetActive(false);
			}
			if (PlayerStorage.IsAccessableAndNotFull())
			{
				this.bgImage.color = this.colorNormal;
				this.text.text = this.textKeyNormal.ToPlainText();
			}
			else
			{
				this.bgImage.color = this.colorFull;
				this.text.text = this.textKeyInventoryFull.ToPlainText();
			}
			this.text.gameObject.SetActive(true);
			this.loadingIndicator.SetActive(false);
		}

		// Token: 0x04001719 RID: 5913
		[SerializeField]
		private ItemMetaDisplay itemDisplay;

		// Token: 0x0400171A RID: 5914
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400171B RID: 5915
		[SerializeField]
		private GameObject countDisplay;

		// Token: 0x0400171C RID: 5916
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x0400171D RID: 5917
		[SerializeField]
		private Image bgImage;

		// Token: 0x0400171E RID: 5918
		[SerializeField]
		private Button button;

		// Token: 0x0400171F RID: 5919
		[SerializeField]
		private GameObject loadingIndicator;

		// Token: 0x04001720 RID: 5920
		[SerializeField]
		private Color colorNormal;

		// Token: 0x04001721 RID: 5921
		[SerializeField]
		private Color colorFull;

		// Token: 0x04001722 RID: 5922
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyNormal;

		// Token: 0x04001723 RID: 5923
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyInventoryFull;

		// Token: 0x04001724 RID: 5924
		private int index;

		// Token: 0x04001725 RID: 5925
		private ItemTreeData item;
	}
}
