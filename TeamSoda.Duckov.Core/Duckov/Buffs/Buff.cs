using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Buffs
{
	// Token: 0x02000404 RID: 1028
	public class Buff : MonoBehaviour
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x00081A0C File Offset: 0x0007FC0C
		public Buff.BuffExclusiveTags ExclusiveTag
		{
			get
			{
				return this.exclusiveTag;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x00081A14 File Offset: 0x0007FC14
		public int ExclusiveTagPriority
		{
			get
			{
				return this.exclusiveTagPriority;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x00081A1C File Offset: 0x0007FC1C
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600256A RID: 9578 RVA: 0x00081A24 File Offset: 0x0007FC24
		public CharacterMainControl Character
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				return characterBuffManager.Master;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600256B RID: 9579 RVA: 0x00081A37 File Offset: 0x0007FC37
		private Item CharacterItem
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				CharacterMainControl characterMainControl = characterBuffManager.Master;
				if (characterMainControl == null)
				{
					return null;
				}
				return characterMainControl.CharacterItem;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x00081A55 File Offset: 0x0007FC55
		// (set) Token: 0x0600256D RID: 9581 RVA: 0x00081A5D File Offset: 0x0007FC5D
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x00081A66 File Offset: 0x0007FC66
		// (set) Token: 0x0600256F RID: 9583 RVA: 0x00081A6E File Offset: 0x0007FC6E
		public int CurrentLayers
		{
			get
			{
				return this.currentLayers;
			}
			set
			{
				this.currentLayers = value;
				Action onLayerChangedEvent = this.OnLayerChangedEvent;
				if (onLayerChangedEvent == null)
				{
					return;
				}
				onLayerChangedEvent();
			}
		}

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x06002570 RID: 9584 RVA: 0x00081A88 File Offset: 0x0007FC88
		// (remove) Token: 0x06002571 RID: 9585 RVA: 0x00081AC0 File Offset: 0x0007FCC0
		public event Action OnLayerChangedEvent;

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x00081AF5 File Offset: 0x0007FCF5
		public int MaxLayers
		{
			get
			{
				return this.maxLayers;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x00081AFD File Offset: 0x0007FCFD
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x00081B0A File Offset: 0x0007FD0A
		public string DisplayNameKey
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x00081B12 File Offset: 0x0007FD12
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x00081B1F File Offset: 0x0007FD1F
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x00081B27 File Offset: 0x0007FD27
		public bool LimitedLifeTime
		{
			get
			{
				return this.limitedLifeTime;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x00081B2F File Offset: 0x0007FD2F
		public float TotalLifeTime
		{
			get
			{
				return this.totalLifeTime;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x00081B37 File Offset: 0x0007FD37
		public float CurrentLifeTime
		{
			get
			{
				return Time.time - this.timeWhenStarted;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x00081B45 File Offset: 0x0007FD45
		public float RemainingTime
		{
			get
			{
				if (!this.limitedLifeTime)
				{
					return float.PositiveInfinity;
				}
				return this.totalLifeTime - this.CurrentLifeTime;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00081B62 File Offset: 0x0007FD62
		public bool IsOutOfTime
		{
			get
			{
				return this.limitedLifeTime && this.CurrentLifeTime >= this.totalLifeTime;
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00081B80 File Offset: 0x0007FD80
		internal void Setup(CharacterBuffManager manager)
		{
			this.master = manager;
			this.timeWhenStarted = Time.time;
			base.transform.SetParent(this.CharacterItem.transform, false);
			if (this.buffFxInstance)
			{
				UnityEngine.Object.Destroy(this.buffFxInstance.gameObject);
			}
			if (this.buffFxPfb && manager.Master && manager.Master.characterModel)
			{
				this.buffFxInstance = UnityEngine.Object.Instantiate<GameObject>(this.buffFxPfb);
				Transform transform = manager.Master.characterModel.ArmorSocket;
				if (transform == null)
				{
					transform = manager.Master.transform;
				}
				this.buffFxInstance.transform.SetParent(transform);
				this.buffFxInstance.transform.position = transform.position;
				this.buffFxInstance.transform.localRotation = Quaternion.identity;
			}
			foreach (Effect effect in this.effects)
			{
				effect.SetItem(this.CharacterItem);
			}
			this.OnSetup();
			UnityEvent onSetupEvent = this.OnSetupEvent;
			if (onSetupEvent == null)
			{
				return;
			}
			onSetupEvent.Invoke();
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00081CD8 File Offset: 0x0007FED8
		internal void NotifyUpdate()
		{
			this.OnUpdate();
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00081CE0 File Offset: 0x0007FEE0
		internal void NotifyOutOfTime()
		{
			this.OnNotifiedOutOfTime();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00081CF3 File Offset: 0x0007FEF3
		internal virtual void NotifyIncomingBuffWithSameID(Buff incomingPrefab)
		{
			this.timeWhenStarted = Time.time;
			if (this.CurrentLayers < this.maxLayers)
			{
				this.CurrentLayers += incomingPrefab.CurrentLayers;
			}
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00081D21 File Offset: 0x0007FF21
		protected virtual void OnSetup()
		{
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00081D23 File Offset: 0x0007FF23
		protected virtual void OnUpdate()
		{
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00081D25 File Offset: 0x0007FF25
		protected virtual void OnNotifiedOutOfTime()
		{
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00081D27 File Offset: 0x0007FF27
		private void OnDestroy()
		{
			if (this.buffFxInstance)
			{
				UnityEngine.Object.Destroy(this.buffFxInstance.gameObject);
			}
		}

		// Token: 0x04001979 RID: 6521
		[SerializeField]
		private int id;

		// Token: 0x0400197A RID: 6522
		[SerializeField]
		private int maxLayers = 1;

		// Token: 0x0400197B RID: 6523
		[SerializeField]
		private Buff.BuffExclusiveTags exclusiveTag;

		// Token: 0x0400197C RID: 6524
		[Tooltip("优先级高的代替优先级低的。同优先级，选剩余时间长的。如果一方不限制时长，选后来的")]
		[SerializeField]
		private int exclusiveTagPriority;

		// Token: 0x0400197D RID: 6525
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string displayName;

		// Token: 0x0400197E RID: 6526
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string description;

		// Token: 0x0400197F RID: 6527
		[SerializeField]
		private Sprite icon;

		// Token: 0x04001980 RID: 6528
		[SerializeField]
		private bool limitedLifeTime;

		// Token: 0x04001981 RID: 6529
		[SerializeField]
		private float totalLifeTime;

		// Token: 0x04001982 RID: 6530
		[SerializeField]
		private List<Effect> effects = new List<Effect>();

		// Token: 0x04001983 RID: 6531
		[SerializeField]
		private bool hide;

		// Token: 0x04001984 RID: 6532
		[SerializeField]
		private int currentLayers = 1;

		// Token: 0x04001985 RID: 6533
		private CharacterBuffManager master;

		// Token: 0x04001986 RID: 6534
		public UnityEvent OnSetupEvent;

		// Token: 0x04001988 RID: 6536
		[SerializeField]
		private GameObject buffFxPfb;

		// Token: 0x04001989 RID: 6537
		private GameObject buffFxInstance;

		// Token: 0x0400198A RID: 6538
		[HideInInspector]
		public CharacterMainControl fromWho;

		// Token: 0x0400198B RID: 6539
		public int fromWeaponID;

		// Token: 0x0400198C RID: 6540
		private float timeWhenStarted;

		// Token: 0x02000675 RID: 1653
		public enum BuffExclusiveTags
		{
			// Token: 0x04002369 RID: 9065
			NotExclusive,
			// Token: 0x0400236A RID: 9066
			Bleeding,
			// Token: 0x0400236B RID: 9067
			Starve,
			// Token: 0x0400236C RID: 9068
			Thirsty,
			// Token: 0x0400236D RID: 9069
			Weight,
			// Token: 0x0400236E RID: 9070
			Poison,
			// Token: 0x0400236F RID: 9071
			Pain,
			// Token: 0x04002370 RID: 9072
			Electric,
			// Token: 0x04002371 RID: 9073
			Burning,
			// Token: 0x04002372 RID: 9074
			Space,
			// Token: 0x04002373 RID: 9075
			StormProtection,
			// Token: 0x04002374 RID: 9076
			Nauseous,
			// Token: 0x04002375 RID: 9077
			Stun,
			// Token: 0x04002376 RID: 9078
			Ghost
		}
	}
}
