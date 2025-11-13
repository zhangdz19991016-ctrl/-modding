using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023A RID: 570
	[CreateAssetMenu(menuName = "Settings/MetaData")]
	public class GameMetaData : ScriptableObject
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00045708 File Offset: 0x00043908
		public VersionData Version
		{
			get
			{
				if (GameMetaData.Instance == null)
				{
					return default(VersionData);
				}
				return GameMetaData.Instance.versionData.versionData;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x0004573B File Offset: 0x0004393B
		public bool IsDemo
		{
			get
			{
				return this.isDemo;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00045743 File Offset: 0x00043943
		public bool IsTestVersion
		{
			get
			{
				return this.isTestVersion;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00045750 File Offset: 0x00043950
		public static GameMetaData Instance
		{
			get
			{
				if (GameMetaData._instance == null)
				{
					GameMetaData._instance = Resources.Load<GameMetaData>("MetaData");
				}
				return GameMetaData._instance;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00045773 File Offset: 0x00043973
		public static bool BloodFxOn
		{
			get
			{
				return GameMetaData.Instance.bloodFxOn;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0004577F File Offset: 0x0004397F
		// (set) Token: 0x060011EA RID: 4586 RVA: 0x00045787 File Offset: 0x00043987
		public Platform Platform
		{
			get
			{
				return this.platform;
			}
			set
			{
				this.platform = value;
			}
		}

		// Token: 0x04000DC6 RID: 3526
		[SerializeField]
		private GameVersionData versionData;

		// Token: 0x04000DC7 RID: 3527
		[SerializeField]
		private bool isTestVersion;

		// Token: 0x04000DC8 RID: 3528
		[SerializeField]
		private bool isDemo;

		// Token: 0x04000DC9 RID: 3529
		[SerializeField]
		private Platform platform;

		// Token: 0x04000DCA RID: 3530
		[SerializeField]
		private bool bloodFxOn = true;

		// Token: 0x04000DCB RID: 3531
		private static GameMetaData _instance;
	}
}
