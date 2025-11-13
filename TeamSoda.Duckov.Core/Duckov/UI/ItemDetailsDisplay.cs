using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000398 RID: 920
	public class ItemDetailsDisplay : MonoBehaviour
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x00071B47 File Offset: 0x0006FD47
		private string DurabilityToolTipsFormat
		{
			get
			{
				return this.durabilityToolTipsFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x00071B54 File Offset: 0x0006FD54
		public ItemSlotCollectionDisplay SlotCollectionDisplay
		{
			get
			{
				return this.slotCollectionDisplay;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x00071B5C File Offset: 0x0006FD5C
		private PrefabPool<ItemVariableEntry> VariablePool
		{
			get
			{
				if (this._variablePool == null)
				{
					this._variablePool = new PrefabPool<ItemVariableEntry>(this.variableEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._variablePool;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x00071B9C File Offset: 0x0006FD9C
		private PrefabPool<ItemStatEntry> StatPool
		{
			get
			{
				if (this._statPool == null)
				{
					this._statPool = new PrefabPool<ItemStatEntry>(this.statEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._statPool;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x00071BDC File Offset: 0x0006FDDC
		private PrefabPool<ItemModifierEntry> ModifierPool
		{
			get
			{
				if (this._modifierPool == null)
				{
					this._modifierPool = new PrefabPool<ItemModifierEntry>(this.modifierEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._modifierPool;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x00071C1C File Offset: 0x0006FE1C
		private PrefabPool<ItemEffectEntry> EffectPool
		{
			get
			{
				if (this._effectPool == null)
				{
					this._effectPool = new PrefabPool<ItemEffectEntry>(this.effectEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._effectPool;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x00071C5A File Offset: 0x0006FE5A
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00071C64 File Offset: 0x0006FE64
		internal void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Clear();
			if (target == null)
			{
				return;
			}
			this.target = target;
			this.icon.sprite = target.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayName.text = target.DisplayName;
			this.itemID.text = string.Format("#{0}", target.TypeID);
			this.description.text = target.Description;
			this.countContainer.SetActive(target.Stackable);
			this.count.text = target.StackCount.ToString();
			this.tagsDisplay.Setup(target);
			this.usageUtilitiesDisplay.Setup(target);
			this.usableIndicator.gameObject.SetActive(target.UsageUtilities != null);
			this.RefreshDurability();
			this.slotCollectionDisplay.Setup(target, false);
			this.registeredIndicator.SetActive(target.IsRegistered());
			this.RefreshWeightText();
			this.SetupGunDisplays();
			this.SetupVariables();
			this.SetupConstants();
			this.SetupStats();
			this.SetupModifiers();
			this.SetupEffects();
			this.RegisterEvents();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00071DE3 File Offset: 0x0006FFE3
		private void Awake()
		{
			this.SlotCollectionDisplay.onElementDoubleClicked += this.OnElementDoubleClicked;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00071DFC File Offset: 0x0006FFFC
		private void OnElementDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (!collectionDisplay.Editable)
			{
				return;
			}
			Item item = slotDisplay.GetItem();
			if (item == null)
			{
				return;
			}
			ItemUtilities.SendToPlayer(item, false, PlayerStorage.Instance != null);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00071E35 File Offset: 0x00070035
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x00071E3D File Offset: 0x0007003D
		private void Clear()
		{
			this.tagsDisplay.Clear();
			this.VariablePool.ReleaseAll();
			this.StatPool.ReleaseAll();
			this.ModifierPool.ReleaseAll();
			this.EffectPool.ReleaseAll();
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00071E78 File Offset: 0x00070078
		private void SetupGunDisplays()
		{
			Item item = this.Target;
			ItemSetting_Gun itemSetting_Gun = (item != null) ? item.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(itemSetting_Gun.TargetBulletID);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00071ED8 File Offset: 0x000700D8
		private void SetupVariables()
		{
			if (this.target.Variables == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Variables)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00071F5C File Offset: 0x0007015C
		private void SetupConstants()
		{
			if (this.target.Constants == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Constants)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00071FE0 File Offset: 0x000701E0
		private void SetupStats()
		{
			if (this.target.Stats == null)
			{
				return;
			}
			foreach (Stat stat in this.target.Stats)
			{
				if (stat.Display)
				{
					ItemStatEntry itemStatEntry = this.StatPool.Get(this.propertiesParent);
					itemStatEntry.Setup(stat);
					itemStatEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x0007206C File Offset: 0x0007026C
		private void SetupModifiers()
		{
			if (this.target.Modifiers == null)
			{
				return;
			}
			foreach (ModifierDescription modifierDescription in this.target.Modifiers)
			{
				if (modifierDescription.Display)
				{
					ItemModifierEntry itemModifierEntry = this.ModifierPool.Get(this.propertiesParent);
					itemModifierEntry.Setup(modifierDescription);
					itemModifierEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000720F8 File Offset: 0x000702F8
		private void SetupEffects()
		{
			foreach (Effect effect in this.target.Effects)
			{
				if (effect.Display)
				{
					ItemEffectEntry itemEffectEntry = this.EffectPool.Get(this.propertiesParent);
					itemEffectEntry.Setup(effect);
					itemEffectEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00072174 File Offset: 0x00070374
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy += this.OnTargetDestroy;
			this.target.onChildChanged += this.OnTargetChildChanged;
			this.target.onSetStackCount += this.OnTargetSetStackCount;
			this.target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000721EC File Offset: 0x000703EC
		private void RefreshWeightText()
		{
			this.weightText.text = string.Format(this.weightFormat, this.target.TotalWeight);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00072214 File Offset: 0x00070414
		private void OnTargetSetStackCount(Item item)
		{
			this.RefreshWeightText();
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0007221C File Offset: 0x0007041C
		private void OnTargetChildChanged(Item obj)
		{
			this.RefreshWeightText();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00072224 File Offset: 0x00070424
		internal void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy -= this.OnTargetDestroy;
			this.target.onChildChanged -= this.OnTargetChildChanged;
			this.target.onSetStackCount -= this.OnTargetSetStackCount;
			this.target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0007229C File Offset: 0x0007049C
		private void OnTargetDurabilityChanged(Item item)
		{
			this.RefreshDurability();
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000722A4 File Offset: 0x000704A4
		private void RefreshDurability()
		{
			bool useDurability = this.target.UseDurability;
			this.durabilityContainer.SetActive(useDurability);
			if (useDurability)
			{
				float durability = this.target.Durability;
				float maxDurability = this.target.MaxDurability;
				float maxDurabilityWithLoss = this.target.MaxDurabilityWithLoss;
				string lossPercentage = string.Format("{0:0}%", this.target.DurabilityLoss * 100f);
				float num = durability / maxDurability;
				this.durabilityText.text = string.Format("{0:0} / {1:0}", durability, maxDurabilityWithLoss);
				this.durabilityToolTips.text = this.DurabilityToolTipsFormat.Format(new
				{
					curDurability = durability,
					maxDurability = maxDurability,
					maxDurabilityWithLoss = maxDurabilityWithLoss,
					lossPercentage = lossPercentage
				});
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityColorOverT.Evaluate(num);
				this.durabilityLoss.fillAmount = this.target.DurabilityLoss;
			}
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00072396 File Offset: 0x00070596
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x0400160C RID: 5644
		[SerializeField]
		private Image icon;

		// Token: 0x0400160D RID: 5645
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x0400160E RID: 5646
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x0400160F RID: 5647
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x04001610 RID: 5648
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04001611 RID: 5649
		[SerializeField]
		private GameObject countContainer;

		// Token: 0x04001612 RID: 5650
		[SerializeField]
		private TextMeshProUGUI count;

		// Token: 0x04001613 RID: 5651
		[SerializeField]
		private GameObject durabilityContainer;

		// Token: 0x04001614 RID: 5652
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x04001615 RID: 5653
		[SerializeField]
		private TooltipsProvider durabilityToolTips;

		// Token: 0x04001616 RID: 5654
		[SerializeField]
		[LocalizationKey("Default")]
		private string durabilityToolTipsFormatKey = "UI_DurabilityToolTips";

		// Token: 0x04001617 RID: 5655
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x04001618 RID: 5656
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private Gradient durabilityColorOverT;

		// Token: 0x0400161A RID: 5658
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x0400161B RID: 5659
		[SerializeField]
		private ItemSlotCollectionDisplay slotCollectionDisplay;

		// Token: 0x0400161C RID: 5660
		[SerializeField]
		private RectTransform propertiesParent;

		// Token: 0x0400161D RID: 5661
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x0400161E RID: 5662
		[SerializeField]
		private TagsDisplay tagsDisplay;

		// Token: 0x0400161F RID: 5663
		[SerializeField]
		private GameObject usableIndicator;

		// Token: 0x04001620 RID: 5664
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x04001621 RID: 5665
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x04001622 RID: 5666
		[SerializeField]
		private ItemVariableEntry variableEntryPrefab;

		// Token: 0x04001623 RID: 5667
		[SerializeField]
		private ItemStatEntry statEntryPrefab;

		// Token: 0x04001624 RID: 5668
		[SerializeField]
		private ItemModifierEntry modifierEntryPrefab;

		// Token: 0x04001625 RID: 5669
		[SerializeField]
		private ItemEffectEntry effectEntryPrefab;

		// Token: 0x04001626 RID: 5670
		[SerializeField]
		private string weightFormat = "{0:0.#} kg";

		// Token: 0x04001627 RID: 5671
		private Item target;

		// Token: 0x04001628 RID: 5672
		private PrefabPool<ItemVariableEntry> _variablePool;

		// Token: 0x04001629 RID: 5673
		private PrefabPool<ItemStatEntry> _statPool;

		// Token: 0x0400162A RID: 5674
		private PrefabPool<ItemModifierEntry> _modifierPool;

		// Token: 0x0400162B RID: 5675
		private PrefabPool<ItemEffectEntry> _effectPool;
	}
}
