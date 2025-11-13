using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000332 RID: 818
	[Serializable]
	public struct MultiSceneLocation
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x00065BC2 File Offset: 0x00063DC2
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x00065BCA File Offset: 0x00063DCA
		public Transform LocationTransform
		{
			get
			{
				return this.GetLocationTransform();
			}
			private set
			{
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x00065BCC File Offset: 0x00063DCC
		// (set) Token: 0x06001BE3 RID: 7139 RVA: 0x00065BD4 File Offset: 0x00063DD4
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
			set
			{
				this.sceneID = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x00065BE0 File Offset: 0x00063DE0
		public SceneReference Scene
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00065C04 File Offset: 0x00063E04
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00065C0C File Offset: 0x00063E0C
		public string LocationName
		{
			get
			{
				return this.locationName;
			}
			set
			{
				this.locationName = value;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00065C15 File Offset: 0x00063E15
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x00065C22 File Offset: 0x00063E22
		public Transform GetLocationTransform()
		{
			if (this.Scene == null)
			{
				return null;
			}
			if (this.Scene.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(this.Scene, this.locationName);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00065C50 File Offset: 0x00063E50
		public bool TryGetLocationPosition(out Vector3 result)
		{
			result = default(Vector3);
			if (MultiSceneCore.Instance == null)
			{
				return false;
			}
			if (MultiSceneCore.Instance.TryGetCachedPosition(this.sceneID, this.locationName, out result))
			{
				return true;
			}
			Transform location = SceneLocationsProvider.GetLocation(this.sceneID, this.locationName);
			if (location != null)
			{
				result = location.transform.position;
				return true;
			}
			return false;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x00065CBD File Offset: 0x00063EBD
		internal string GetDisplayName()
		{
			return this.DisplayName;
		}

		// Token: 0x040013BE RID: 5054
		[SerializeField]
		private string sceneID;

		// Token: 0x040013BF RID: 5055
		[SerializeField]
		private string locationName;

		// Token: 0x040013C0 RID: 5056
		[SerializeField]
		private string displayName;
	}
}
