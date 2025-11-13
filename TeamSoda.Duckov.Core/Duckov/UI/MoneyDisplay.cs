using System;
using Duckov.Economy;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000386 RID: 902
	public class MoneyDisplay : MonoBehaviour
	{
		// Token: 0x06001F6B RID: 8043 RVA: 0x0006E93B File Offset: 0x0006CB3B
		private void Awake()
		{
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			SavesSystem.OnSetFile += this.OnSaveFileChanged;
			this.Refresh();
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0006E965 File Offset: 0x0006CB65
		private void OnDestroy()
		{
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			SavesSystem.OnSetFile -= this.OnSaveFileChanged;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0006E989 File Offset: 0x0006CB89
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0006E994 File Offset: 0x0006CB94
		private void Refresh()
		{
			this.text.text = EconomyManager.Money.ToString(this.format);
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0006E9BF File Offset: 0x0006CBBF
		private void OnMoneyChanged(long arg1, long arg2)
		{
			this.Refresh();
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0006E9C7 File Offset: 0x0006CBC7
		private void OnSaveFileChanged()
		{
			this.Refresh();
		}

		// Token: 0x04001577 RID: 5495
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001578 RID: 5496
		[SerializeField]
		private string format = "n0";
	}
}
