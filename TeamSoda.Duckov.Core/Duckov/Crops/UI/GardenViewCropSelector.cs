using System;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F6 RID: 758
	public class GardenViewCropSelector : MonoBehaviour
	{
		// Token: 0x060018C0 RID: 6336 RVA: 0x0005AE4E File Offset: 0x0005904E
		private void Awake()
		{
			this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirm));
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0005AE6C File Offset: 0x0005906C
		private void OnConfirm()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem != null)
			{
				this.master.SelectSeed(selectedItem.TypeID);
			}
			this.Hide();
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0005AEA0 File Offset: 0x000590A0
		public void Show()
		{
			this.fadeGroup.Show();
			if (LevelManager.Instance == null)
			{
				return;
			}
			ItemUIUtilities.Select(null);
			this.playerInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
			this.storageInventoryDisplay.Setup(PlayerStorage.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0005AF3A File Offset: 0x0005913A
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0005AF4D File Offset: 0x0005914D
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0005AF60 File Offset: 0x00059160
		private void OnSelectionChanged()
		{
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0005AF62 File Offset: 0x00059162
		public void Hide()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04001204 RID: 4612
		[SerializeField]
		private GardenView master;

		// Token: 0x04001205 RID: 4613
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001206 RID: 4614
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x04001207 RID: 4615
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x04001208 RID: 4616
		[SerializeField]
		private InventoryDisplay storageInventoryDisplay;
	}
}
