using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000322 RID: 802
	public class BuildingSelectionPanel : MonoBehaviour
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00061578 File Offset: 0x0005F778
		private PrefabPool<BuildingBtnEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<BuildingBtnEntry>(this.buildingBtnTemplate, null, new Action<BuildingBtnEntry>(this.OnGetButtonEntry), new Action<BuildingBtnEntry>(this.OnReleaseButtonEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000615C7 File Offset: 0x0005F7C7
		private void OnGetButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked += this.OnButtonSelected;
			entry.onRecycleRequested += this.OnRecycleRequested;
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000615ED File Offset: 0x0005F7ED
		private void OnReleaseButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked -= this.OnButtonSelected;
			entry.onRecycleRequested -= this.OnRecycleRequested;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x00061613 File Offset: 0x0005F813
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x00061626 File Offset: 0x0005F826
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onButtonSelected;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06001AD1 RID: 6865 RVA: 0x0006163C File Offset: 0x0005F83C
		// (remove) Token: 0x06001AD2 RID: 6866 RVA: 0x00061674 File Offset: 0x0005F874
		public event Action<BuildingBtnEntry> onButtonSelected;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001AD3 RID: 6867 RVA: 0x000616AC File Offset: 0x0005F8AC
		// (remove) Token: 0x06001AD4 RID: 6868 RVA: 0x000616E4 File Offset: 0x0005F8E4
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00061719 File Offset: 0x0005F919
		public void Show()
		{
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0006171B File Offset: 0x0005F91B
		internal void Setup(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			this.Refresh();
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0006172C File Offset: 0x0005F92C
		public void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
			{
				BuildingBtnEntry buildingBtnEntry = this.Pool.Get(null);
				buildingBtnEntry.Setup(buildingInfo);
				buildingBtnEntry.transform.SetAsLastSibling();
			}
			foreach (BuildingBtnEntry buildingBtnEntry2 in this.Pool.ActiveEntries)
			{
				if (!buildingBtnEntry2.CostEnough)
				{
					buildingBtnEntry2.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000617D4 File Offset: 0x0005F9D4
		public static BuildingInfo[] GetBuildingsToDisplay()
		{
			BuildingDataCollection instance = BuildingDataCollection.Instance;
			if (instance == null)
			{
				return new BuildingInfo[0];
			}
			List<BuildingInfo> list = new List<BuildingInfo>();
			foreach (BuildingInfo item in instance.Infos)
			{
				if (item.CurrentAmount > 0 || item.RequirementsSatisfied())
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x04001331 RID: 4913
		[SerializeField]
		private BuildingBtnEntry buildingBtnTemplate;

		// Token: 0x04001332 RID: 4914
		private PrefabPool<BuildingBtnEntry> _pool;

		// Token: 0x04001333 RID: 4915
		private BuildingArea targetArea;
	}
}
