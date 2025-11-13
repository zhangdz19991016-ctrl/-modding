using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BA RID: 186
public class BulletTypeHUD : MonoBehaviour
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001B318 File Offset: 0x00019518
	private PrefabPool<BulletTypeSelectButton> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<BulletTypeSelectButton>(this.originSelectButton, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001B354 File Offset: 0x00019554
	private bool CanOpenList
	{
		get
		{
			return this.characterMainControl && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running) && InputManager.InputActived;
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0001B3A0 File Offset: 0x000195A0
	private void Awake()
	{
		this.selectionsHUD = new List<BulletTypeSelectButton>();
		this.originSelectButton.gameObject.SetActive(false);
		WeaponButton.OnWeaponButtonSelected += this.OnWeaponButtonSelected;
		this.typeList.SetActive(false);
		InputManager.OnSwitchBulletTypeInput += this.OnSwitchInput;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0001B3F8 File Offset: 0x000195F8
	private void OnDestroy()
	{
		WeaponButton.OnWeaponButtonSelected -= this.OnWeaponButtonSelected;
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
		}
		InputManager.OnSwitchBulletTypeInput -= this.OnSwitchInput;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001B44C File Offset: 0x0001964C
	private void OnWeaponButtonSelected(WeaponButton button)
	{
		Transform transform = this.canvasGroup.transform as RectTransform;
		RectTransform rectTransform = button.transform as RectTransform;
		transform.position = rectTransform.position + (rectTransform.rect.center + (rectTransform.rect.height / 2f + 8f) * rectTransform.up) * rectTransform.lossyScale;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0001B4DC File Offset: 0x000196DC
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
			}
		}
		if (this.gunAgent == null)
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		if (this.bulletTypeText != null && this.gunAgent.GunItemSetting != null)
		{
			int targetBulletID = this.gunAgent.GunItemSetting.TargetBulletID;
			if (this.bulletTpyeID != targetBulletID)
			{
				this.bulletTpyeID = targetBulletID;
				if (this.bulletTpyeID >= 0)
				{
					this.bulletTypeText.text = this.gunAgent.GunItemSetting.CurrentBulletName;
					this.bulletTypeText.color = Color.black;
					this.background.color = this.normalColor;
				}
				else
				{
					this.bulletTypeText.text = "UI_Bullet_NotAssigned".ToPlainText();
					this.bulletTypeText.color = Color.white;
					this.background.color = this.emptyColor;
				}
				UnityEvent onTypeChangeEvent = this.OnTypeChangeEvent;
				if (onTypeChangeEvent != null)
				{
					onTypeChangeEvent.Invoke();
				}
			}
		}
		if (this.listOpen && !this.CanOpenList)
		{
			this.CloseList();
		}
		if (CharacterInputControl.GetChangeBulletTypeWasPressed())
		{
			if (!this.listOpen)
			{
				this.OpenList();
				return;
			}
			if (this.selectIndex < this.selectionsHUD.Count && this.selectionsHUD[this.selectIndex] != null)
			{
				this.SetBulletType(this.selectionsHUD[this.selectIndex].BulletTypeID);
			}
			this.CloseList();
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001B6E8 File Offset: 0x000198E8
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgent = null;
		}
		this.gunAgent = (newAgent as ItemAgent_Gun);
		this.CloseList();
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001B70C File Offset: 0x0001990C
	private void OnSwitchInput(int dir)
	{
		if (!this.listOpen)
		{
			return;
		}
		this.selectIndex -= dir;
		if (this.totalSelctionCount == 0)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex >= this.totalSelctionCount)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex < 0)
		{
			this.selectIndex = this.totalSelctionCount - 1;
		}
		for (int i = 0; i < this.selectionsHUD.Count; i++)
		{
			this.selectionsHUD[i].SetSelection(i == this.selectIndex);
		}
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001B7A0 File Offset: 0x000199A0
	private void OpenList()
	{
		Debug.Log("OpenList");
		if (!this.CanOpenList)
		{
			return;
		}
		if (this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(true);
		this.listOpen = true;
		this.indicator.SetActive(false);
		this.RefreshContent();
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001B7EE File Offset: 0x000199EE
	public void CloseList()
	{
		if (!this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(false);
		this.listOpen = false;
		this.indicator.SetActive(true);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001B818 File Offset: 0x00019A18
	private void RefreshContent()
	{
		this.selectionsHUD.Clear();
		this.Selections.ReleaseAll();
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		ItemSetting_Gun gunItemSetting = this.gunAgent.GunItemSetting;
		if (gunItemSetting != null)
		{
			dictionary = gunItemSetting.GetBulletTypesInInventory(this.characterMainControl.CharacterItem.Inventory);
		}
		if (this.bulletTpyeID > 0 && !dictionary.ContainsKey(this.bulletTpyeID))
		{
			BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
			bulletTypeInfo.bulletTypeID = this.bulletTpyeID;
			bulletTypeInfo.count = 0;
			dictionary.Add(this.bulletTpyeID, bulletTypeInfo);
		}
		if (dictionary.Count <= 0)
		{
			dictionary.Add(-1, new BulletTypeInfo
			{
				bulletTypeID = -1,
				count = 0
			});
		}
		this.totalSelctionCount = dictionary.Count;
		int num = 0;
		this.selectIndex = 0;
		foreach (KeyValuePair<int, BulletTypeInfo> keyValuePair in dictionary)
		{
			BulletTypeSelectButton bulletTypeSelectButton = this.Selections.Get(this.typeList.transform);
			bulletTypeSelectButton.gameObject.SetActive(true);
			bulletTypeSelectButton.transform.SetAsLastSibling();
			bulletTypeSelectButton.Init(keyValuePair.Value.bulletTypeID, keyValuePair.Value.count);
			if (this.bulletTpyeID == keyValuePair.Value.bulletTypeID)
			{
				bulletTypeSelectButton.SetSelection(true);
				this.selectIndex = num;
			}
			this.selectionsHUD.Add(bulletTypeSelectButton);
			Debug.Log(string.Format("BUlletType {0}:{1}", this.selectIndex, keyValuePair.Value.bulletTypeID));
			num++;
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0001B9DC File Offset: 0x00019BDC
	public void SetBulletType(int typeID)
	{
		this.CloseList();
		if (!this.gunAgent || !this.gunAgent.GunItemSetting)
		{
			return;
		}
		bool flag = this.gunAgent.GunItemSetting.TargetBulletID != typeID;
		this.gunAgent.GunItemSetting.SetTargetBulletType(typeID);
		if (flag)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x040005A1 RID: 1441
	private CharacterMainControl characterMainControl;

	// Token: 0x040005A2 RID: 1442
	private ItemAgent_Gun gunAgent;

	// Token: 0x040005A3 RID: 1443
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private TextMeshProUGUI bulletTypeText;

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	private ProceduralImage background;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private Color normalColor;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	private Color emptyColor;

	// Token: 0x040005A8 RID: 1448
	private int bulletTpyeID = -2;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	private GameObject typeList;

	// Token: 0x040005AA RID: 1450
	public UnityEvent OnTypeChangeEvent;

	// Token: 0x040005AB RID: 1451
	public GameObject indicator;

	// Token: 0x040005AC RID: 1452
	private int selectIndex;

	// Token: 0x040005AD RID: 1453
	private int totalSelctionCount;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private BulletTypeSelectButton originSelectButton;

	// Token: 0x040005AF RID: 1455
	private List<BulletTypeSelectButton> selectionsHUD;

	// Token: 0x040005B0 RID: 1456
	private PrefabPool<BulletTypeSelectButton> _selectionsCache;

	// Token: 0x040005B1 RID: 1457
	private bool listOpen;
}
