using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI.PlayerStats
{
	// Token: 0x020003D3 RID: 979
	public class MainCharacterStatValueDisplay : MonoBehaviour
	{
		// Token: 0x060023BA RID: 9146 RVA: 0x0007D720 File Offset: 0x0007B920
		private void OnEnable()
		{
			if (this.target == null)
			{
				CharacterMainControl main = CharacterMainControl.Main;
				Stat stat;
				if (main == null)
				{
					stat = null;
				}
				else
				{
					Item characterItem = main.CharacterItem;
					stat = ((characterItem != null) ? characterItem.GetStat(this.statKey.GetHashCode()) : null);
				}
				this.target = stat;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0007D76F File Offset: 0x0007B96F
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0007D777 File Offset: 0x0007B977
		private void AutoRename()
		{
			base.gameObject.name = "StatDisplay_" + this.statKey;
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0007D794 File Offset: 0x0007B994
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetDirty;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0007D7B6 File Offset: 0x0007B9B6
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetDirty;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0007D7D8 File Offset: 0x0007B9D8
		private void OnTargetDirty(Stat stat)
		{
			this.Refresh();
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0007D7E0 File Offset: 0x0007B9E0
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.displayNameText.text = this.target.DisplayName;
			float value = this.target.Value;
			this.valueText.text = string.Format(this.format, value);
		}

		// Token: 0x04001845 RID: 6213
		[SerializeField]
		private string statKey;

		// Token: 0x04001846 RID: 6214
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001847 RID: 6215
		[SerializeField]
		private TextMeshProUGUI valueText;

		// Token: 0x04001848 RID: 6216
		[SerializeField]
		private string format = "{0:0.0}";

		// Token: 0x04001849 RID: 6217
		private Stat target;
	}
}
