using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.MasterKeys
{
	// Token: 0x020002E3 RID: 739
	public class MasterKeysManager : MonoBehaviour
	{
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060017B8 RID: 6072 RVA: 0x00057860 File Offset: 0x00055A60
		// (remove) Token: 0x060017B9 RID: 6073 RVA: 0x00057894 File Offset: 0x00055A94
		public static event Action<int> OnMasterKeyUnlocked;

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x000578C7 File Offset: 0x00055AC7
		// (set) Token: 0x060017BB RID: 6075 RVA: 0x000578CE File Offset: 0x00055ACE
		public static MasterKeysManager Instance { get; private set; }

		// Token: 0x060017BC RID: 6076 RVA: 0x000578D8 File Offset: 0x00055AD8
		public static bool SubmitAndActivate(Item item)
		{
			if (MasterKeysManager.Instance == null)
			{
				return false;
			}
			if (item == null)
			{
				return false;
			}
			int typeID = item.TypeID;
			if (MasterKeysManager.IsActive(typeID))
			{
				return false;
			}
			if (item.StackCount > 1)
			{
				int stackCount = item.StackCount;
				item.StackCount = stackCount - 1;
			}
			else
			{
				item.Detach();
				item.DestroyTree();
			}
			MasterKeysManager.Activate(typeID);
			return true;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0005793E File Offset: 0x00055B3E
		public static bool IsActive(int id)
		{
			return !(MasterKeysManager.Instance == null) && MasterKeysManager.Instance.IsActive_Local(id);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0005795A File Offset: 0x00055B5A
		internal static void Activate(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return;
			}
			MasterKeysManager.Instance.Activate_Local(id);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00057975 File Offset: 0x00055B75
		internal static MasterKeysManager.Status GetStatus(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return null;
			}
			return MasterKeysManager.Instance.GetStatus_Local(id);
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060017C0 RID: 6080 RVA: 0x00057991 File Offset: 0x00055B91
		public int Count
		{
			get
			{
				return this.keysStatus.Count;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x000579A0 File Offset: 0x00055BA0
		public static List<int> AllPossibleKeys
		{
			get
			{
				if (MasterKeysManager._cachedKeyItemIds == null)
				{
					MasterKeysManager._cachedKeyItemIds = new List<int>();
					foreach (ItemAssetsCollection.Entry entry in ItemAssetsCollection.Instance.entries)
					{
						Tag[] tags = entry.metaData.tags;
						if (tags.Any((Tag e) => Tag.Match(e, "Key")))
						{
							if (GameMetaData.Instance.IsDemo)
							{
								if (tags.Any((Tag e) => e.name == GameplayDataSettings.Tags.LockInDemoTag.name))
								{
									continue;
								}
							}
							if (!tags.Any((Tag e) => MasterKeysManager.excludeTags.Contains(e.name)))
							{
								MasterKeysManager._cachedKeyItemIds.Add(entry.typeID);
							}
						}
					}
				}
				return MasterKeysManager._cachedKeyItemIds;
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00057AAC File Offset: 0x00055CAC
		private void Awake()
		{
			if (MasterKeysManager.Instance == null)
			{
				MasterKeysManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00057AD8 File Offset: 0x00055CD8
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00057AEB File Offset: 0x00055CEB
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00057AF4 File Offset: 0x00055CF4
		public bool IsActive_Local(int id)
		{
			MasterKeysManager.Status status = MasterKeysManager.GetStatus(id);
			return status != null && status.active;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00057B13 File Offset: 0x00055D13
		private void Activate_Local(int id)
		{
			if (id < 0)
			{
				return;
			}
			if (!MasterKeysManager.AllPossibleKeys.Contains(id))
			{
				return;
			}
			this.GetOrCreateStatus(id).active = true;
			Action<int> onMasterKeyUnlocked = MasterKeysManager.OnMasterKeyUnlocked;
			if (onMasterKeyUnlocked == null)
			{
				return;
			}
			onMasterKeyUnlocked(id);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00057B48 File Offset: 0x00055D48
		public MasterKeysManager.Status GetStatus_Local(int id)
		{
			return this.keysStatus.Find((MasterKeysManager.Status e) => e.id == id);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00057B7C File Offset: 0x00055D7C
		public MasterKeysManager.Status GetOrCreateStatus(int id)
		{
			MasterKeysManager.Status status_Local = this.GetStatus_Local(id);
			if (status_Local != null)
			{
				return status_Local;
			}
			MasterKeysManager.Status status = new MasterKeysManager.Status();
			status.id = id;
			this.keysStatus.Add(status);
			return status;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00057BB0 File Offset: 0x00055DB0
		private void Save()
		{
			SavesSystem.Save<List<MasterKeysManager.Status>>("MasterKeys", this.keysStatus);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00057BC2 File Offset: 0x00055DC2
		private void Load()
		{
			if (SavesSystem.KeyExisits("MasterKeys"))
			{
				this.keysStatus = SavesSystem.Load<List<MasterKeysManager.Status>>("MasterKeys");
				return;
			}
			this.keysStatus = new List<MasterKeysManager.Status>();
		}

		// Token: 0x04001153 RID: 4435
		[SerializeField]
		private List<MasterKeysManager.Status> keysStatus = new List<MasterKeysManager.Status>();

		// Token: 0x04001154 RID: 4436
		private static List<int> _cachedKeyItemIds;

		// Token: 0x04001155 RID: 4437
		private static string[] excludeTags = new string[]
		{
			"SpecialKey"
		};

		// Token: 0x04001156 RID: 4438
		private const string SaveKey = "MasterKeys";

		// Token: 0x02000584 RID: 1412
		[Serializable]
		public class Status
		{
			// Token: 0x04001FE8 RID: 8168
			[ItemTypeID]
			public int id;

			// Token: 0x04001FE9 RID: 8169
			public bool active;
		}
	}
}
