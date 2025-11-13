using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000331 RID: 817
	[Serializable]
	public class SubSceneEntry
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x00065AC4 File Offset: 0x00063CC4
		public string AmbientSound
		{
			get
			{
				return this.overrideAmbientSound;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x00065ACC File Offset: 0x00063CCC
		public bool IsInDoor
		{
			get
			{
				return this.isInDoor;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x00065AD4 File Offset: 0x00063CD4
		public SceneInfoEntry Info
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.sceneID);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x00065AE4 File Offset: 0x00063CE4
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					Debug.LogWarning("未找到场景" + this.sceneID + "的相关信息，获取SceneReference失败。");
					return null;
				}
				return info.SceneReference;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00065B20 File Offset: 0x00063D20
		public string DisplayName
		{
			get
			{
				SceneInfoEntry info = this.Info;
				if (info == null)
				{
					return this.sceneID;
				}
				return info.DisplayName;
			}
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x00065B44 File Offset: 0x00063D44
		internal bool TryGetCachedPosition(string locationPath, out Vector3 result)
		{
			result = default(Vector3);
			if (this.cachedLocations == null)
			{
				return false;
			}
			SubSceneEntry.Location location = this.cachedLocations.Find((SubSceneEntry.Location e) => e.path == locationPath);
			if (location == null)
			{
				return false;
			}
			result = location.position;
			return true;
		}

		// Token: 0x040013B9 RID: 5049
		[SceneID]
		public string sceneID;

		// Token: 0x040013BA RID: 5050
		[SerializeField]
		private string overrideAmbientSound = "Default";

		// Token: 0x040013BB RID: 5051
		[SerializeField]
		private bool isInDoor;

		// Token: 0x040013BC RID: 5052
		public List<SubSceneEntry.Location> cachedLocations = new List<SubSceneEntry.Location>();

		// Token: 0x040013BD RID: 5053
		public List<SubSceneEntry.TeleporterInfo> cachedTeleporters = new List<SubSceneEntry.TeleporterInfo>();

		// Token: 0x020005E8 RID: 1512
		[Serializable]
		public class Location
		{
			// Token: 0x1700079B RID: 1947
			// (get) Token: 0x060029B1 RID: 10673 RVA: 0x0009B026 File Offset: 0x00099226
			public string DisplayName
			{
				get
				{
					return this.displayName;
				}
			}

			// Token: 0x1700079C RID: 1948
			// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0009B02E File Offset: 0x0009922E
			// (set) Token: 0x060029B3 RID: 10675 RVA: 0x0009B036 File Offset: 0x00099236
			public string DisplayNameRaw
			{
				get
				{
					return this.displayName;
				}
				set
				{
					this.displayName = value;
				}
			}

			// Token: 0x0400212D RID: 8493
			public string path;

			// Token: 0x0400212E RID: 8494
			public Vector3 position;

			// Token: 0x0400212F RID: 8495
			public bool showInMap;

			// Token: 0x04002130 RID: 8496
			[SerializeField]
			private string displayName;
		}

		// Token: 0x020005E9 RID: 1513
		[Serializable]
		public class TeleporterInfo
		{
			// Token: 0x04002131 RID: 8497
			public Vector3 position;

			// Token: 0x04002132 RID: 8498
			public MultiSceneLocation target;

			// Token: 0x04002133 RID: 8499
			public Vector3 nearestTeleporterPositionToTarget;
		}
	}
}
