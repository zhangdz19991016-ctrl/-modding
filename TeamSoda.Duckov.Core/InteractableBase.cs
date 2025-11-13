using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov;
using Duckov.MasterKeys;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000DA RID: 218
public class InteractableBase : MonoBehaviour, IProgress
{
	// Token: 0x060006CD RID: 1741 RVA: 0x0001EAE4 File Offset: 0x0001CCE4
	public List<InteractableBase> GetInteractableList()
	{
		this._interactbleList.Clear();
		this._interactbleList.Add(this);
		if (!this.interactableGroup || this.otherInterablesInGroup.Count <= 0)
		{
			return this._interactbleList;
		}
		foreach (InteractableBase interactableBase in this.otherInterablesInGroup)
		{
			if (!(interactableBase == null) && interactableBase.gameObject.activeInHierarchy)
			{
				this._interactbleList.Add(interactableBase);
			}
		}
		return this._interactbleList;
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x0001EB8C File Offset: 0x0001CD8C
	public float InteractTime
	{
		get
		{
			if (this.requireItem && !this.requireItemUsed)
			{
				return this.interactTime + this.unlockTime;
			}
			return this.interactTime;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001EBB2 File Offset: 0x0001CDB2
	// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0001EBD3 File Offset: 0x0001CDD3
	public string InteractName
	{
		get
		{
			if (this.overrideInteractName)
			{
				return this._overrideInteractNameKey.ToPlainText();
			}
			return this.defaultInteractNameKey.ToPlainText();
		}
		set
		{
			this.overrideInteractName = true;
			this._overrideInteractNameKey = value;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001EBE3 File Offset: 0x0001CDE3
	private bool ShowBaseInteractName
	{
		get
		{
			return this.overrideInteractName && this.ShowBaseInteractNameInspector;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001EBF5 File Offset: 0x0001CDF5
	protected virtual bool ShowBaseInteractNameInspector
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
	private ItemMetaData CachedMeta
	{
		get
		{
			if (this._cachedMeta == null)
			{
				this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.requireItemId));
			}
			return this._cachedMeta.Value;
		}
	}

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060006D4 RID: 1748 RVA: 0x0001EC28 File Offset: 0x0001CE28
	// (remove) Token: 0x060006D5 RID: 1749 RVA: 0x0001EC5C File Offset: 0x0001CE5C
	public static event Action<InteractableBase> OnInteractStartStaticEvent;

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001EC8F File Offset: 0x0001CE8F
	protected virtual bool ShowUnityEvents
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001EC92 File Offset: 0x0001CE92
	public bool Interacting
	{
		get
		{
			return this.interactCharacter != null;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
	// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0001ECA8 File Offset: 0x0001CEA8
	public bool MarkerActive
	{
		get
		{
			return this.interactMarkerVisible;
		}
		set
		{
			if (!base.enabled)
			{
				return;
			}
			this.interactMarkerVisible = value;
			if (value)
			{
				this.ActiveMarker();
				return;
			}
			if (this.markerObject)
			{
				this.markerObject.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0001ECE4 File Offset: 0x0001CEE4
	protected virtual void Awake()
	{
		this.requireItemDataKeyCached = this.GetKey();
		if (this.interactCollider == null)
		{
			this.interactCollider = base.GetComponent<Collider>();
			if (this.interactCollider == null)
			{
				this.interactCollider = base.gameObject.AddComponent<BoxCollider>();
				this.interactCollider.enabled = false;
			}
		}
		if (this.interactCollider != null)
		{
			this.interactCollider.gameObject.layer = LayerMask.NameToLayer("Interactable");
		}
		foreach (InteractableBase interactableBase in this.otherInterablesInGroup)
		{
			if (interactableBase)
			{
				interactableBase.MarkerActive = false;
				interactableBase.transform.position = base.transform.position;
				interactableBase.transform.rotation = base.transform.rotation;
				interactableBase.interactMarkerOffset = this.interactMarkerOffset;
			}
		}
		this._interactbleList = new List<InteractableBase>();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0001EDFC File Offset: 0x0001CFFC
	protected virtual void Start()
	{
		object obj;
		if (this.requireItem && MultiSceneCore.Instance && MultiSceneCore.Instance.inLevelData.TryGetValue(this.requireItemDataKeyCached, out obj) && obj is bool)
		{
			bool flag = (bool)obj;
			if (flag)
			{
				this.requireItem = false;
				this.requireItemUsed = true;
				UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
				if (onRequiredItemUsedEvent != null)
				{
					onRequiredItemUsedEvent.Invoke();
				}
			}
		}
		this.MarkerActive = this.interactMarkerVisible;
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0001EE74 File Offset: 0x0001D074
	private void ActiveMarker()
	{
		if (this.markerObject)
		{
			if (!this.markerObject.gameObject.activeInHierarchy)
			{
				this.markerObject.gameObject.SetActive(true);
			}
			return;
		}
		this.markerObject = UnityEngine.Object.Instantiate<InteractMarker>(GameplayDataSettings.Prefabs.InteractMarker, base.transform);
		this.markerObject.transform.localPosition = this.interactMarkerOffset;
		this.CheckInteractable();
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0001EEEA File Offset: 0x0001D0EA
	public void SetMarkerUsed()
	{
		if (!this.markerObject)
		{
			return;
		}
		this.markerObject.MarkAsUsed();
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0001EF08 File Offset: 0x0001D108
	public bool StartInteract(CharacterMainControl _interactCharacter)
	{
		if (!_interactCharacter)
		{
			return false;
		}
		if (this.requireItem && !this.TryGetRequiredItem(_interactCharacter).Item1)
		{
			return false;
		}
		if (this.interactCharacter == _interactCharacter)
		{
			return false;
		}
		if (!this.CheckInteractable())
		{
			return false;
		}
		if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnStartInteract && !this.UseRequiredItem(_interactCharacter))
		{
			this.StopInteract();
			return false;
		}
		this.interactCharacter = _interactCharacter;
		this.interactTimer = 0f;
		this.timeOut = false;
		UnityEvent<CharacterMainControl, InteractableBase> onInteractStartEvent = this.OnInteractStartEvent;
		if (onInteractStartEvent != null)
		{
			onInteractStartEvent.Invoke(_interactCharacter, this);
		}
		Action<InteractableBase> onInteractStartStaticEvent = InteractableBase.OnInteractStartStaticEvent;
		if (onInteractStartStaticEvent != null)
		{
			onInteractStartStaticEvent(this);
		}
		try
		{
			this.OnInteractStart(_interactCharacter);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			if (CharacterMainControl.Main)
			{
				CharacterMainControl.Main.PopText("OnInteractStart开始失败，Log Error", -1f);
			}
			return false;
		}
		return true;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
	public InteractableBase GetInteractableInGroup(int index)
	{
		if (index == 0)
		{
			return this;
		}
		List<InteractableBase> interactableList = this.GetInteractableList();
		if (index >= interactableList.Count)
		{
			return null;
		}
		return interactableList[index];
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0001F023 File Offset: 0x0001D223
	public void InternalStopInteract()
	{
		this.interactCharacter = null;
		this.lastStopTime = Time.time;
		this.OnInteractStop();
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0001F040 File Offset: 0x0001D240
	public void StopInteract()
	{
		CharacterMainControl characterMainControl = this.interactCharacter;
		if (characterMainControl && characterMainControl.interactAction.Running && characterMainControl.interactAction.InteractingTarget == this)
		{
			this.interactCharacter.interactAction.StopAction();
			return;
		}
		this.InternalStopInteract();
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0001F094 File Offset: 0x0001D294
	public void UpdateInteract(CharacterMainControl _interactCharacter, float deltaTime)
	{
		this.interactTimer += deltaTime;
		this.OnUpdate(_interactCharacter, deltaTime);
		if (!this.timeOut && this.interactTimer >= this.InteractTime)
		{
			if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnTimeOut && !this.UseRequiredItem(_interactCharacter))
			{
				this.StopInteract();
				return;
			}
			if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.None && !this.requireItemUsed)
			{
				this.requireItemUsed = true;
				UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
				if (onRequiredItemUsedEvent != null)
				{
					onRequiredItemUsedEvent.Invoke();
				}
				if (MultiSceneCore.Instance)
				{
					MultiSceneCore.Instance.inLevelData[this.requireItemDataKeyCached] = true;
					Debug.Log("设置使用过物品为true");
				}
			}
			this.timeOut = true;
			this.OnTimeOut();
			UnityEvent<CharacterMainControl, InteractableBase> onInteractTimeoutEvent = this.OnInteractTimeoutEvent;
			if (onInteractTimeoutEvent != null)
			{
				onInteractTimeoutEvent.Invoke(_interactCharacter, this);
			}
			if (this.finishWhenTimeOut)
			{
				this.FinishInteract(_interactCharacter);
			}
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0001F184 File Offset: 0x0001D384
	public void FinishInteract(CharacterMainControl _interactCharacter)
	{
		if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnFinshed && !this.UseRequiredItem(_interactCharacter))
		{
			this.StopInteract();
			return;
		}
		try
		{
			this.OnInteractFinished();
			UnityEvent<CharacterMainControl, InteractableBase> onInteractFinishedEvent = this.OnInteractFinishedEvent;
			if (onInteractFinishedEvent != null)
			{
				onInteractFinishedEvent.Invoke(_interactCharacter, this);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		this.StopInteract();
		if (this.disableOnFinish)
		{
			base.enabled = false;
			if (this.markerObject)
			{
				this.markerObject.gameObject.SetActive(false);
			}
			if (this.interactCollider)
			{
				this.interactCollider.enabled = false;
			}
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0001F234 File Offset: 0x0001D434
	protected virtual void OnUpdate(CharacterMainControl _interactCharacter, float deltaTime)
	{
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0001F236 File Offset: 0x0001D436
	protected virtual void OnTimeOut()
	{
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0001F238 File Offset: 0x0001D438
	private bool UseRequiredItem(CharacterMainControl interactCharacter)
	{
		Debug.Log("尝试使用");
		ValueTuple<bool, Item> valueTuple = this.TryGetRequiredItem(interactCharacter);
		Item item = valueTuple.Item2;
		if (!valueTuple.Item1 || valueTuple.Item2 == null)
		{
			return false;
		}
		if (item.UseDurability)
		{
			Debug.Log("尝试消耗耐久");
			item.Durability -= 1f;
			if (item.Durability <= 0f)
			{
				item.Detach();
				item.DestroyTree();
			}
		}
		else if (!item.Stackable)
		{
			Debug.Log("尝试直接消耗掉");
			item.Detach();
			item.DestroyTree();
		}
		else
		{
			Debug.Log("尝试消耗堆叠");
			item.StackCount--;
		}
		if (this.requireOnce)
		{
			this.requireItem = false;
			this.requireItemUsed = true;
			UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
			if (onRequiredItemUsedEvent != null)
			{
				onRequiredItemUsedEvent.Invoke();
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.Instance.inLevelData[this.requireItemDataKeyCached] = true;
				Debug.Log("设置使用过物品为true");
			}
		}
		return true;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0001F348 File Offset: 0x0001D548
	public bool CheckInteractable()
	{
		if (this.interactCharacter != null)
		{
			if (!(this.interactCharacter.interactAction.InteractingTarget != this))
			{
				return false;
			}
			this.StopInteract();
		}
		return (Time.time - this.lastStopTime >= this.coolTime || this.coolTime <= 0f || this.lastStopTime <= 0f) && this.IsInteractable();
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0001F3BB File Offset: 0x0001D5BB
	protected virtual bool IsInteractable()
	{
		return true;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0001F3BE File Offset: 0x0001D5BE
	protected virtual void OnInteractStart(CharacterMainControl interactCharacter)
	{
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
	protected virtual void OnInteractStop()
	{
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0001F3C2 File Offset: 0x0001D5C2
	protected virtual void OnInteractFinished()
	{
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
	public string GetInteractName()
	{
		if (this.overrideInteractName)
		{
			return this.InteractName;
		}
		return "UI_Interact".ToPlainText();
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
	public string GetRequiredItemName()
	{
		if (!this.requireItem)
		{
			return null;
		}
		return this.CachedMeta.DisplayName;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0001F405 File Offset: 0x0001D605
	public Sprite GetRequireditemIcon()
	{
		if (!this.requireItem)
		{
			return null;
		}
		return this.CachedMeta.icon;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0001F41C File Offset: 0x0001D61C
	protected virtual void OnDestroy()
	{
		if (this.Interacting)
		{
			this.StopInteract();
		}
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0001F42C File Offset: 0x0001D62C
	public virtual Progress GetProgress()
	{
		Progress result = default(Progress);
		if (this.Interacting && this.InteractTime > 0f)
		{
			result.inProgress = true;
			result.total = this.InteractTime;
			result.current = this.interactTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0001F484 File Offset: 0x0001D684
	[return: TupleElementNames(new string[]
	{
		"hasItem",
		"ItemInstance"
	})]
	public ValueTuple<bool, Item> TryGetRequiredItem(CharacterMainControl fromCharacter)
	{
		if (!this.requireItem)
		{
			return new ValueTuple<bool, Item>(false, null);
		}
		if (!fromCharacter)
		{
			return new ValueTuple<bool, Item>(false, null);
		}
		if (MasterKeysManager.IsActive(this.requireItemId))
		{
			return new ValueTuple<bool, Item>(true, null);
		}
		foreach (Slot slot in fromCharacter.CharacterItem.Slots)
		{
			if (slot.Content && slot.Content.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, slot.Content);
			}
		}
		foreach (Item item in fromCharacter.CharacterItem.Inventory)
		{
			if (item.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, item);
			}
			if (item.Slots != null && item.Slots.Count > 0)
			{
				foreach (Slot slot2 in item.Slots)
				{
					if (slot2.Content != null && slot2.Content.TypeID == this.requireItemId)
					{
						return new ValueTuple<bool, Item>(true, slot2.Content);
					}
				}
			}
		}
		foreach (Item item2 in LevelManager.Instance.PetProxy.Inventory)
		{
			if (item2.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, item2);
			}
			if (item2.Slots && item2.Slots.Count > 0)
			{
				foreach (Slot slot3 in item2.Slots)
				{
					if (slot3.Content != null && slot3.Content.TypeID == this.requireItemId)
					{
						return new ValueTuple<bool, Item>(true, slot3.Content);
					}
				}
			}
		}
		return new ValueTuple<bool, Item>(false, null);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0001F714 File Offset: 0x0001D914
	private int GetKey()
	{
		if (this.overrideItemUsedKey)
		{
			return this.overrideItemUsedSaveKey.GetHashCode();
		}
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("Intact_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0001F78C File Offset: 0x0001D98C
	public void InteractWithMainCharacter()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.Interact(this);
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0001F79E File Offset: 0x0001D99E
	private void OnDrawGizmos()
	{
		if (!this.interactMarkerVisible)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.TransformPoint(this.interactMarkerOffset), 0.1f);
	}

	// Token: 0x0400068E RID: 1678
	public bool interactableGroup;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private List<InteractableBase> otherInterablesInGroup;

	// Token: 0x04000690 RID: 1680
	public bool zoomIn = true;

	// Token: 0x04000691 RID: 1681
	private List<InteractableBase> _interactbleList = new List<InteractableBase>();

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private float interactTime;

	// Token: 0x04000693 RID: 1683
	public bool finishWhenTimeOut = true;

	// Token: 0x04000694 RID: 1684
	private float interactTimer;

	// Token: 0x04000695 RID: 1685
	public Vector3 interactMarkerOffset;

	// Token: 0x04000696 RID: 1686
	public bool overrideInteractName;

	// Token: 0x04000697 RID: 1687
	[LocalizationKey("Default")]
	private string defaultInteractNameKey = "UI_Interact";

	// Token: 0x04000698 RID: 1688
	[LocalizationKey("Interact")]
	public string _overrideInteractNameKey;

	// Token: 0x04000699 RID: 1689
	public Collider interactCollider;

	// Token: 0x0400069A RID: 1690
	public bool requireItem;

	// Token: 0x0400069B RID: 1691
	public bool requireOnce = true;

	// Token: 0x0400069C RID: 1692
	[ItemTypeID]
	public int requireItemId;

	// Token: 0x0400069D RID: 1693
	public float unlockTime;

	// Token: 0x0400069E RID: 1694
	public bool overrideItemUsedKey;

	// Token: 0x0400069F RID: 1695
	public string overrideItemUsedSaveKey;

	// Token: 0x040006A0 RID: 1696
	public InteractableBase.WhenToUseRequireItemTypes whenToUseRequireItem;

	// Token: 0x040006A1 RID: 1697
	public UnityEvent OnRequiredItemUsedEvent;

	// Token: 0x040006A2 RID: 1698
	private int requireItemDataKeyCached;

	// Token: 0x040006A3 RID: 1699
	private bool requireItemUsed;

	// Token: 0x040006A4 RID: 1700
	private ItemMetaData? _cachedMeta;

	// Token: 0x040006A5 RID: 1701
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractStartEvent;

	// Token: 0x040006A6 RID: 1702
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractTimeoutEvent;

	// Token: 0x040006A7 RID: 1703
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractFinishedEvent;

	// Token: 0x040006A9 RID: 1705
	public bool disableOnFinish;

	// Token: 0x040006AA RID: 1706
	public float coolTime;

	// Token: 0x040006AB RID: 1707
	private float lastStopTime = -1f;

	// Token: 0x040006AC RID: 1708
	protected CharacterMainControl interactCharacter;

	// Token: 0x040006AD RID: 1709
	private bool timeOut;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private bool interactMarkerVisible = true;

	// Token: 0x040006AF RID: 1711
	private InteractMarker markerObject;

	// Token: 0x0200046A RID: 1130
	public enum WhenToUseRequireItemTypes
	{
		// Token: 0x04001B60 RID: 7008
		None,
		// Token: 0x04001B61 RID: 7009
		OnFinshed,
		// Token: 0x04001B62 RID: 7010
		OnTimeOut,
		// Token: 0x04001B63 RID: 7011
		OnStartInteract
	}
}
