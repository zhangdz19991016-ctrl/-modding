using System;
using System.Text;
using ItemStatsSystem;
using Sirenix.OdinInspector;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024E RID: 590
	public class Perk : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x00046AB7 File Offset: 0x00044CB7
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00046ABF File Offset: 0x00044CBF
		public DisplayQuality DisplayQuality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00046AC7 File Offset: 0x00044CC7
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00046AD1 File Offset: 0x00044CD1
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00046ACF File Offset: 0x00044CCF
		[LocalizationKey("Perks")]
		private string description
		{
			get
			{
				if (!this.hasDescription)
				{
					return string.Empty;
				}
				return this.displayName + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00046AF1 File Offset: 0x00044CF1
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x00046B00 File Offset: 0x00044D00
		public string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string value = this.description.ToPlainText();
				if (!string.IsNullOrEmpty(value))
				{
					stringBuilder.AppendLine(value);
				}
				PerkBehaviour[] components = base.GetComponents<PerkBehaviour>();
				for (int i = 0; i < components.Length; i++)
				{
					string description = components[i].Description;
					if (!string.IsNullOrEmpty(description))
					{
						stringBuilder.AppendLine(description);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00046B66 File Offset: 0x00044D66
		public PerkRequirement Requirement
		{
			get
			{
				return this.requirement;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00046B6E File Offset: 0x00044D6E
		public bool DefaultUnlocked
		{
			get
			{
				return this.defaultUnlocked;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00046B78 File Offset: 0x00044D78
		private DateTime UnlockingBeginTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.unlockingBeginTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x00046BBD File Offset: 0x00044DBD
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x00046BC5 File Offset: 0x00044DC5
		public bool Unlocked
		{
			get
			{
				return this._unlocked;
			}
			internal set
			{
				this._unlocked = value;
				Action<Perk, bool> action = this.onUnlockStateChanged;
				if (action == null)
				{
					return;
				}
				action(this, value);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00046BE0 File Offset: 0x00044DE0
		public bool Unlocking
		{
			get
			{
				return this.unlocking;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00046BE8 File Offset: 0x00044DE8
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00046BF0 File Offset: 0x00044DF0
		public PerkTree Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00046BF9 File Offset: 0x00044DF9
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00046C01 File Offset: 0x00044E01
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001285 RID: 4741 RVA: 0x00046C0C File Offset: 0x00044E0C
		// (remove) Token: 0x06001286 RID: 4742 RVA: 0x00046C44 File Offset: 0x00044E44
		public event Action<Perk, bool> onUnlockStateChanged;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001287 RID: 4743 RVA: 0x00046C7C File Offset: 0x00044E7C
		// (remove) Token: 0x06001288 RID: 4744 RVA: 0x00046CB0 File Offset: 0x00044EB0
		public static event Action<Perk> OnPerkUnlockConfirmed;

		// Token: 0x06001289 RID: 4745 RVA: 0x00046CE3 File Offset: 0x00044EE3
		public bool AreAllParentsUnlocked()
		{
			return this.master.AreAllParentsUnlocked(this);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00046CF1 File Offset: 0x00044EF1
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<PerkTree>();
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00046D0D File Offset: 0x00044F0D
		private bool CheckAndPay()
		{
			return this.requirement == null || (EXPManager.Level >= this.requirement.level && this.requirement.cost.Pay(true, true));
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00046D44 File Offset: 0x00044F44
		public bool SubmitItemsAndBeginUnlocking()
		{
			if (this.Unlocked)
			{
				Debug.LogError("Perk " + this.displayName + " already unlocked!");
				return false;
			}
			if (!this.CheckAndPay())
			{
				return false;
			}
			this.unlocking = true;
			this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
			this.master.NotifyChildStateChanged(this);
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action != null)
			{
				action(this, this._unlocked);
			}
			return true;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00046DC0 File Offset: 0x00044FC0
		public bool ConfirmUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			if (!this.unlocking)
			{
				return false;
			}
			if (DateTime.UtcNow - this.UnlockingBeginTime < this.requirement.RequireTime)
			{
				return false;
			}
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			Action<Perk> onPerkUnlockConfirmed = Perk.OnPerkUnlockConfirmed;
			if (onPerkUnlockConfirmed != null)
			{
				onPerkUnlockConfirmed(this);
			}
			return true;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00046E31 File Offset: 0x00045031
		public bool ForceUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			Debug.Log("Unlock default:" + this.displayName);
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			return true;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00046E70 File Offset: 0x00045070
		public TimeSpan GetRemainingTime()
		{
			if (this.Unlocked)
			{
				return TimeSpan.Zero;
			}
			if (!this.unlocking)
			{
				return TimeSpan.Zero;
			}
			TimeSpan t = DateTime.UtcNow - this.UnlockingBeginTime;
			TimeSpan timeSpan = this.requirement.RequireTime - t;
			if (timeSpan < TimeSpan.Zero)
			{
				return TimeSpan.Zero;
			}
			return timeSpan;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00046ED0 File Offset: 0x000450D0
		public float GetProgress01()
		{
			TimeSpan remainingTime = this.GetRemainingTime();
			double totalSeconds = this.requirement.RequireTime.TotalSeconds;
			if (totalSeconds <= 0.0)
			{
				return 1f;
			}
			double totalSeconds2 = remainingTime.TotalSeconds;
			return 1f - (float)(totalSeconds2 / totalSeconds);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00046F1C File Offset: 0x0004511C
		public void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddWarning("未指定PerkTree");
			}
			if (this.master)
			{
				if (!this.master.Perks.Contains(this))
				{
					result.AddError("PerkTree未包含此Perk").WithFix(delegate()
					{
						this.master.perks.Add(this);
					}, true);
				}
				PerkTree perkTree = this.master;
				bool flag;
				if (perkTree == null)
				{
					flag = (null != null);
				}
				else
				{
					PerkTreeRelationGraphOwner relationGraphOwner = perkTree.RelationGraphOwner;
					flag = (((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null) != null);
				}
				if (!flag)
				{
					result.AddError("未在Graph中指定技能的关系");
				}
			}
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00046FAE File Offset: 0x000451AE
		internal Vector2 GetLayoutPosition()
		{
			if (this.master == null)
			{
				return Vector2.zero;
			}
			PerkTreeRelationGraphOwner relationGraphOwner = this.master.RelationGraphOwner;
			return ((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null).cachedPosition;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00046FE1 File Offset: 0x000451E1
		internal void NotifyParentStateChanged()
		{
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Unlocked);
		}

		// Token: 0x04000E28 RID: 3624
		[SerializeField]
		private PerkTree master;

		// Token: 0x04000E29 RID: 3625
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x04000E2A RID: 3626
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000E2B RID: 3627
		[SerializeField]
		private DisplayQuality quality;

		// Token: 0x04000E2C RID: 3628
		[LocalizationKey("Perks")]
		[SerializeField]
		private string displayName = "未命名技能";

		// Token: 0x04000E2D RID: 3629
		[SerializeField]
		private bool hasDescription;

		// Token: 0x04000E2E RID: 3630
		[SerializeField]
		private PerkRequirement requirement;

		// Token: 0x04000E2F RID: 3631
		[SerializeField]
		private bool defaultUnlocked;

		// Token: 0x04000E30 RID: 3632
		[SerializeField]
		internal bool unlocking;

		// Token: 0x04000E31 RID: 3633
		[DateTime]
		[SerializeField]
		internal long unlockingBeginTimeRaw;

		// Token: 0x04000E32 RID: 3634
		[SerializeField]
		private bool _unlocked;
	}
}
