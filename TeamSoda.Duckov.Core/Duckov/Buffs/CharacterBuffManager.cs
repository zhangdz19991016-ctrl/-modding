using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.Buffs
{
	// Token: 0x02000405 RID: 1029
	public class CharacterBuffManager : MonoBehaviour
	{
		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002585 RID: 9605 RVA: 0x00081D67 File Offset: 0x0007FF67
		public CharacterMainControl Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x00081D6F File Offset: 0x0007FF6F
		public ReadOnlyCollection<Buff> Buffs
		{
			get
			{
				if (this._readOnlyBuffsCollection == null)
				{
					this._readOnlyBuffsCollection = new ReadOnlyCollection<Buff>(this.buffs);
				}
				return this._readOnlyBuffsCollection;
			}
		}

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x06002587 RID: 9607 RVA: 0x00081D90 File Offset: 0x0007FF90
		// (remove) Token: 0x06002588 RID: 9608 RVA: 0x00081DC8 File Offset: 0x0007FFC8
		public event Action<CharacterBuffManager, Buff> onAddBuff;

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06002589 RID: 9609 RVA: 0x00081E00 File Offset: 0x00080000
		// (remove) Token: 0x0600258A RID: 9610 RVA: 0x00081E38 File Offset: 0x00080038
		public event Action<CharacterBuffManager, Buff> onRemoveBuff;

		// Token: 0x0600258B RID: 9611 RVA: 0x00081E6D File Offset: 0x0008006D
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<CharacterMainControl>();
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00081E8C File Offset: 0x0008008C
		public void AddBuff(Buff buffPrefab, CharacterMainControl fromWho, int overrideWeaponID = 0)
		{
			if (buffPrefab == null)
			{
				return;
			}
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffPrefab.ID);
			if (buff)
			{
				buff.NotifyIncomingBuffWithSameID(buffPrefab);
				return;
			}
			Buff buff2 = UnityEngine.Object.Instantiate<Buff>(buffPrefab);
			buff2.Setup(this);
			buff2.fromWho = fromWho;
			if (overrideWeaponID > 0)
			{
				buff2.fromWeaponID = overrideWeaponID;
			}
			this.buffs.Add(buff2);
			Action<CharacterBuffManager, Buff> action = this.onAddBuff;
			if (action == null)
			{
				return;
			}
			action(this, buff2);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00081F28 File Offset: 0x00080128
		public void RemoveBuff(int buffID, bool removeOneLayer)
		{
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffID);
			if (buff != null)
			{
				this.RemoveBuff(buff, removeOneLayer);
			}
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00081F6C File Offset: 0x0008016C
		public void RemoveBuffsByTag(Buff.BuffExclusiveTags buffTag, bool removeOneLayer)
		{
			if (buffTag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return;
			}
			foreach (Buff buff in this.buffs.FindAll((Buff e) => e.ExclusiveTag == buffTag))
			{
				if (buff != null)
				{
					this.RemoveBuff(buff, removeOneLayer);
				}
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00081FF0 File Offset: 0x000801F0
		public bool HasBuff(int buffID)
		{
			return this.buffs.Find((Buff e) => e.ID == buffID) != null;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00082028 File Offset: 0x00080228
		public Buff GetBuffByTag(Buff.BuffExclusiveTags tag)
		{
			if (tag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return null;
			}
			return this.buffs.Find((Buff e) => e.ExclusiveTag == tag);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00082064 File Offset: 0x00080264
		public void RemoveBuff(Buff toRemove, bool oneLayer)
		{
			if (oneLayer && toRemove.CurrentLayers > 1)
			{
				toRemove.CurrentLayers--;
				if (toRemove.CurrentLayers >= 1)
				{
					return;
				}
			}
			if (this.buffs.Remove(toRemove))
			{
				Action<CharacterBuffManager, Buff> action = this.onRemoveBuff;
				if (action != null)
				{
					action(this, toRemove);
				}
				UnityEngine.Object.Destroy(toRemove.gameObject);
			}
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000820C4 File Offset: 0x000802C4
		private void Update()
		{
			bool flag = false;
			foreach (Buff buff in this.buffs)
			{
				if (buff == null)
				{
					flag = true;
				}
				else if (buff.IsOutOfTime)
				{
					buff.NotifyOutOfTime();
					this.outOfTimeBuffsBuffer.Add(buff);
				}
				else
				{
					buff.NotifyUpdate();
				}
			}
			if (this.outOfTimeBuffsBuffer.Count > 0)
			{
				foreach (Buff buff2 in this.outOfTimeBuffsBuffer)
				{
					if (buff2 != null)
					{
						this.RemoveBuff(buff2, false);
					}
				}
				this.outOfTimeBuffsBuffer.Clear();
			}
			if (flag)
			{
				this.buffs.RemoveAll((Buff e) => e == null);
			}
		}

		// Token: 0x0400198D RID: 6541
		[SerializeField]
		private CharacterMainControl master;

		// Token: 0x0400198E RID: 6542
		private List<Buff> buffs = new List<Buff>();

		// Token: 0x0400198F RID: 6543
		private ReadOnlyCollection<Buff> _readOnlyBuffsCollection;

		// Token: 0x04001992 RID: 6546
		private List<Buff> outOfTimeBuffsBuffer = new List<Buff>();
	}
}
