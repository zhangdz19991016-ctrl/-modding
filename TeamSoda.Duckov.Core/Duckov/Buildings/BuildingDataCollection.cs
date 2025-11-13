using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000318 RID: 792
	[CreateAssetMenu]
	public class BuildingDataCollection : ScriptableObject
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x0005F664 File Offset: 0x0005D864
		public static BuildingDataCollection Instance
		{
			get
			{
				return GameplayDataSettings.BuildingDataCollection;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0005F66B File Offset: 0x0005D86B
		public ReadOnlyCollection<BuildingInfo> Infos
		{
			get
			{
				if (this.readonlyInfos == null)
				{
					this.readonlyInfos = new ReadOnlyCollection<BuildingInfo>(this.infos);
				}
				return this.readonlyInfos;
			}
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0005F68C File Offset: 0x0005D88C
		internal static BuildingInfo GetInfo(string id)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return default(BuildingInfo);
			}
			return BuildingDataCollection.Instance.infos.FirstOrDefault((BuildingInfo e) => e.id == id);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0005F6D8 File Offset: 0x0005D8D8
		internal static Building GetPrefab(string prefabName)
		{
			if (BuildingDataCollection.Instance == null)
			{
				return null;
			}
			return BuildingDataCollection.Instance.prefabs.FirstOrDefault((Building e) => e != null && e.name == prefabName);
		}

		// Token: 0x040012E2 RID: 4834
		[SerializeField]
		private List<BuildingInfo> infos = new List<BuildingInfo>();

		// Token: 0x040012E3 RID: 4835
		[SerializeField]
		private List<Building> prefabs;

		// Token: 0x040012E4 RID: 4836
		public ReadOnlyCollection<BuildingInfo> readonlyInfos;
	}
}
