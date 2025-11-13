using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E4 RID: 740
	public class MasterKeysIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x00057C14 File Offset: 0x00055E14
		public int ItemID
		{
			get
			{
				return this.itemID;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x00057C1C File Offset: 0x00055E1C
		public string DisplayName
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.DisplayName;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00057C4A File Offset: 0x00055E4A
		public Sprite Icon
		{
			get
			{
				if (this.status == null)
				{
					return this.undiscoveredIcon;
				}
				if (!this.status.active)
				{
					return this.undiscoveredIcon;
				}
				return this.metaData.icon;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00057C7A File Offset: 0x00055E7A
		public string Description
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.Description;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00057CA8 File Offset: 0x00055EA8
		public bool Active
		{
			get
			{
				return this.status != null && this.status.active;
			}
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x060017D2 RID: 6098 RVA: 0x00057CC0 File Offset: 0x00055EC0
		// (remove) Token: 0x060017D3 RID: 6099 RVA: 0x00057CF8 File Offset: 0x00055EF8
		internal event Action<MasterKeysIndexEntry> onPointerClicked;

		// Token: 0x060017D4 RID: 6100 RVA: 0x00057D2D File Offset: 0x00055F2D
		public void Setup(int itemID, ISingleSelectionMenu<MasterKeysIndexEntry> menu)
		{
			this.itemID = itemID;
			this.metaData = ItemAssetsCollection.GetMetaData(itemID);
			this.menu = menu;
			this.Refresh();
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00057D50 File Offset: 0x00055F50
		private void SetupNotDiscovered()
		{
			this.icon.sprite = (this.undiscoveredIcon ? this.undiscoveredIcon : this.metaData.icon);
			this.notDiscoveredLook.ApplyTo(this.icon);
			this.nameText.text = "???";
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00057DA9 File Offset: 0x00055FA9
		private void SetupActive()
		{
			this.icon.sprite = this.metaData.icon;
			this.activeLook.ApplyTo(this.icon);
			this.nameText.text = this.metaData.DisplayName;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00057DE8 File Offset: 0x00055FE8
		private void Refresh()
		{
			this.status = MasterKeysManager.GetStatus(this.itemID);
			if (this.status != null)
			{
				if (this.status.active)
				{
					this.SetupActive();
					return;
				}
			}
			else
			{
				this.SetupNotDiscovered();
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00057E1D File Offset: 0x0005601D
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Refresh();
			ISingleSelectionMenu<MasterKeysIndexEntry> singleSelectionMenu = this.menu;
			if (singleSelectionMenu != null)
			{
				singleSelectionMenu.SetSelection(this);
			}
			Action<MasterKeysIndexEntry> action = this.onPointerClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x04001157 RID: 4439
		[SerializeField]
		private Image icon;

		// Token: 0x04001158 RID: 4440
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001159 RID: 4441
		[SerializeField]
		private MasterKeysIndexEntry.Look notDiscoveredLook;

		// Token: 0x0400115A RID: 4442
		[SerializeField]
		private MasterKeysIndexEntry.Look activeLook;

		// Token: 0x0400115B RID: 4443
		[SerializeField]
		private Sprite undiscoveredIcon;

		// Token: 0x0400115C RID: 4444
		[ItemTypeID]
		private int itemID;

		// Token: 0x0400115D RID: 4445
		private ItemMetaData metaData;

		// Token: 0x0400115E RID: 4446
		private MasterKeysManager.Status status;

		// Token: 0x04001160 RID: 4448
		private ISingleSelectionMenu<MasterKeysIndexEntry> menu;

		// Token: 0x02000587 RID: 1415
		[Serializable]
		public struct Look
		{
			// Token: 0x060028D0 RID: 10448 RVA: 0x00096F51 File Offset: 0x00095151
			public void ApplyTo(Graphic graphic)
			{
				graphic.material = this.material;
				graphic.color = this.color;
			}

			// Token: 0x04001FEF RID: 8175
			public Color color;

			// Token: 0x04001FF0 RID: 8176
			public Material material;
		}
	}
}
