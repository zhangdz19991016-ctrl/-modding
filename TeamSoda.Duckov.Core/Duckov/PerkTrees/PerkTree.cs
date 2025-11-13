using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000254 RID: 596
	public class PerkTree : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x000472ED File Offset: 0x000454ED
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x000472EB File Offset: 0x000454EB
		[LocalizationKey("Perks")]
		private string perkTreeName
		{
			get
			{
				return this.displayNameKey;
			}
			set
			{
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x000472F5 File Offset: 0x000454F5
		public string ID
		{
			get
			{
				return this.perkTreeID;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x000472FD File Offset: 0x000454FD
		private string displayNameKey
		{
			get
			{
				return "PerkTree_" + this.ID;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x0004730F File Offset: 0x0004550F
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0004731C File Offset: 0x0004551C
		public bool Horizontal
		{
			get
			{
				return this.horizontal;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060012BF RID: 4799 RVA: 0x00047324 File Offset: 0x00045524
		// (remove) Token: 0x060012C0 RID: 4800 RVA: 0x0004735C File Offset: 0x0004555C
		public event Action<PerkTree> onPerkTreeStatusChanged;

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00047391 File Offset: 0x00045591
		public ReadOnlyCollection<Perk> Perks
		{
			get
			{
				if (this.perks_ReadOnly == null)
				{
					this.perks_ReadOnly = this.perks.AsReadOnly();
				}
				return this.perks_ReadOnly;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x000473B2 File Offset: 0x000455B2
		public PerkTreeRelationGraphOwner RelationGraphOwner
		{
			get
			{
				return this.relationGraphOwner;
			}
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x000473BA File Offset: 0x000455BA
		private void Awake()
		{
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000473E4 File Offset: 0x000455E4
		private void Start()
		{
			foreach (Perk perk in this.perks)
			{
				if (!(perk == null) && perk.DefaultUnlocked)
				{
					perk.ForceUnlock();
				}
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00047448 File Offset: 0x00045648
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0004746C File Offset: 0x0004566C
		public object GenerateSaveData()
		{
			return new PerkTree.SaveData(this);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00047474 File Offset: 0x00045674
		public void SetupSaveData(object data)
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			PerkTree.SaveData saveData = data as PerkTree.SaveData;
			if (saveData == null)
			{
				return;
			}
			using (List<Perk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Perk cur = enumerator.Current;
					if (!(cur == null))
					{
						PerkTree.SaveData.Entry entry = saveData.entries.Find((PerkTree.SaveData.Entry e) => e != null && e.perkName == cur.name);
						if (entry != null)
						{
							cur.Unlocked = entry.unlocked;
							cur.unlocking = entry.unlocking;
							cur.unlockingBeginTimeRaw = entry.unlockingBeginTime;
						}
					}
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00047574 File Offset: 0x00045774
		private string SaveKey
		{
			get
			{
				return "PerkTree_" + this.perkTreeID;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00047586 File Offset: 0x00045786
		public void Save()
		{
			SavesSystem.Save<PerkTree.SaveData>(this.SaveKey, this.GenerateSaveData() as PerkTree.SaveData);
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x000475A0 File Offset: 0x000457A0
		public void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			PerkTree.SaveData data = SavesSystem.Load<PerkTree.SaveData>(this.SaveKey);
			this.SetupSaveData(data);
			this.loaded = true;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000475D8 File Offset: 0x000457D8
		public void ReapplyPerks()
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			foreach (Perk perk2 in this.perks)
			{
				perk2.Unlocked = perk2.Unlocked;
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00047670 File Offset: 0x00045870
		internal bool AreAllParentsUnlocked(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return false;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetIncomingNodes(relatedNode))
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null) && !relatedNode2.Unlocked)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000476F8 File Offset: 0x000458F8
		internal void NotifyChildStateChanged(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetOutgoingNodes(relatedNode))
			{
				perkRelationNode.NotifyIncomingStateChanged();
			}
			Action<PerkTree> action = this.onPerkTreeStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00047778 File Offset: 0x00045978
		private void Collect()
		{
			this.perks.Clear();
			Perk[] componentsInChildren = base.transform.GetComponentsInChildren<Perk>();
			Perk[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Master = this;
			}
			this.perks.AddRange(componentsInChildren);
		}

		// Token: 0x04000E3F RID: 3647
		[SerializeField]
		private string perkTreeID = "DefaultPerkTree";

		// Token: 0x04000E40 RID: 3648
		[SerializeField]
		private bool horizontal;

		// Token: 0x04000E41 RID: 3649
		[SerializeField]
		private PerkTreeRelationGraphOwner relationGraphOwner;

		// Token: 0x04000E42 RID: 3650
		[SerializeField]
		internal List<Perk> perks = new List<Perk>();

		// Token: 0x04000E44 RID: 3652
		private ReadOnlyCollection<Perk> perks_ReadOnly;

		// Token: 0x04000E45 RID: 3653
		private bool loaded;

		// Token: 0x0200053E RID: 1342
		[Serializable]
		private class SaveData
		{
			// Token: 0x06002832 RID: 10290 RVA: 0x00093814 File Offset: 0x00091A14
			public SaveData(PerkTree perkTree)
			{
				this.entries = new List<PerkTree.SaveData.Entry>();
				for (int i = 0; i < perkTree.perks.Count; i++)
				{
					Perk perk = perkTree.perks[i];
					if (!(perk == null))
					{
						this.entries.Add(new PerkTree.SaveData.Entry(perk));
					}
				}
			}

			// Token: 0x04001EC7 RID: 7879
			public List<PerkTree.SaveData.Entry> entries;

			// Token: 0x02000682 RID: 1666
			[Serializable]
			public class Entry
			{
				// Token: 0x06002B22 RID: 11042 RVA: 0x000A35C5 File Offset: 0x000A17C5
				public Entry(Perk perk)
				{
					this.perkName = perk.name;
					this.unlocked = perk.Unlocked;
					this.unlocking = perk.Unlocking;
					this.unlockingBeginTime = perk.unlockingBeginTimeRaw;
				}

				// Token: 0x0400238A RID: 9098
				public string perkName;

				// Token: 0x0400238B RID: 9099
				public bool unlocking;

				// Token: 0x0400238C RID: 9100
				public long unlockingBeginTime;

				// Token: 0x0400238D RID: 9101
				public bool unlocked;
			}
		}
	}
}
