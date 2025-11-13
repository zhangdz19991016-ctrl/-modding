using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class ItemSetting_Gun : ItemSettingBase
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x00023EB1 File Offset: 0x000220B1
	public int TargetBulletID
	{
		get
		{
			return this.targetBulletID;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x00023EBC File Offset: 0x000220BC
	public string CurrentBulletName
	{
		get
		{
			if (this.TargetBulletID < 0)
			{
				return "UI_Bullet_NotAssigned".ToPlainText();
			}
			return ItemAssetsCollection.GetMetaData(this.TargetBulletID).DisplayName;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x00023EF0 File Offset: 0x000220F0
	public int BulletCount
	{
		get
		{
			if (this.loadingBullets)
			{
				return -1;
			}
			if (this.bulletCount < 0)
			{
				this.bulletCount = this.GetBulletCount();
			}
			return this.bulletCount;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060007FE RID: 2046 RVA: 0x00023F3C File Offset: 0x0002213C
	// (set) Token: 0x060007FD RID: 2045 RVA: 0x00023F17 File Offset: 0x00022117
	private int bulletCount
	{
		get
		{
			return this._bulletCountCache;
		}
		set
		{
			this._bulletCountCache = value;
			base.Item.Variables.SetInt(this.bulletCountHash, this._bulletCountCache);
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060007FF RID: 2047 RVA: 0x00023F44 File Offset: 0x00022144
	public int Capacity
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemSetting_Gun.CapacityHash));
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000800 RID: 2048 RVA: 0x00023F5B File Offset: 0x0002215B
	public bool LoadingBullets
	{
		get
		{
			return this.loadingBullets;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x00023F63 File Offset: 0x00022163
	public bool LoadBulletsSuccess
	{
		get
		{
			return this.loadBulletsSuccess;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x00023F74 File Offset: 0x00022174
	// (set) Token: 0x06000802 RID: 2050 RVA: 0x00023F6B File Offset: 0x0002216B
	public Item PreferdBulletsToLoad
	{
		get
		{
			return this.preferedBulletsToLoad;
		}
		set
		{
			this.preferedBulletsToLoad = value;
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00023F7C File Offset: 0x0002217C
	public void SetTargetBulletType(Item bulletItem)
	{
		if (bulletItem != null)
		{
			this.SetTargetBulletType(bulletItem.TypeID);
			return;
		}
		this.SetTargetBulletType(-1);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00023F9C File Offset: 0x0002219C
	public void SetTargetBulletType(int typeID)
	{
		bool flag = false;
		if (this.TargetBulletID != typeID && this.TargetBulletID != -1)
		{
			flag = true;
		}
		this.targetBulletID = typeID;
		if (flag)
		{
			this.TakeOutAllBullets();
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00023FCF File Offset: 0x000221CF
	public override void Start()
	{
		base.Start();
		this.AutoSetTypeInInventory(null);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00023FE0 File Offset: 0x000221E0
	public void UseABullet()
	{
		if (LevelManager.Instance.IsBaseLevel)
		{
			return;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null) && item.StackCount >= 1)
			{
				item.StackCount--;
				break;
			}
		}
		this.bulletCount--;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00024068 File Offset: 0x00022268
	public bool IsFull()
	{
		return this.bulletCount >= this.Capacity;
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x0002407C File Offset: 0x0002227C
	public bool IsValidBullet(Item newBulletItem)
	{
		if (newBulletItem == null)
		{
			return false;
		}
		if (!newBulletItem.Tags.Contains(GameplayDataSettings.Tags.Bullet))
		{
			return false;
		}
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null && currentLoadedBullet.TypeID == newBulletItem.TypeID && this.bulletCount >= this.Capacity)
		{
			return false;
		}
		string @string = newBulletItem.Constants.GetString(this.caliberHash, null);
		string string2 = base.Item.Constants.GetString(this.caliberHash, null);
		return !(@string != string2);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00024110 File Offset: 0x00022310
	public bool LoadSpecificBullet(Item newBulletItem)
	{
		Debug.Log("尝试安装指定弹药");
		if (!this.IsValidBullet(newBulletItem))
		{
			return false;
		}
		Debug.Log("指定弹药判定通过");
		ItemAgent_Gun itemAgent_Gun = base.Item.ActiveAgent as ItemAgent_Gun;
		if (!(itemAgent_Gun != null))
		{
			Inventory inventory = base.Item.InInventory;
			if (inventory != null && inventory != CharacterMainControl.Main.CharacterItem.Inventory)
			{
				inventory = null;
			}
			this.preferedBulletsToLoad = newBulletItem;
			this.LoadBulletsFromInventory(inventory).Forget();
			return true;
		}
		if (itemAgent_Gun.Holder != null)
		{
			bool flag = itemAgent_Gun.CharacterReload(newBulletItem);
			Debug.Log(string.Format("角色reload:{0}", flag));
			return true;
		}
		return false;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000241CC File Offset: 0x000223CC
	public UniTaskVoid LoadBulletsFromInventory(Inventory inventory)
	{
		ItemSetting_Gun.<LoadBulletsFromInventory>d__45 <LoadBulletsFromInventory>d__;
		<LoadBulletsFromInventory>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<LoadBulletsFromInventory>d__.<>4__this = this;
		<LoadBulletsFromInventory>d__.inventory = inventory;
		<LoadBulletsFromInventory>d__.<>1__state = -1;
		<LoadBulletsFromInventory>d__.<>t__builder.Start<ItemSetting_Gun.<LoadBulletsFromInventory>d__45>(ref <LoadBulletsFromInventory>d__);
		return <LoadBulletsFromInventory>d__.<>t__builder.Task;
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00024218 File Offset: 0x00022418
	public bool AutoSetTypeInInventory(Inventory inventory)
	{
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null)
		{
			this.SetTargetBulletType(currentLoadedBullet);
			return false;
		}
		if (inventory == null)
		{
			return false;
		}
		foreach (Item item in inventory)
		{
			if (item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				this.SetTargetBulletType(item);
				break;
			}
		}
		return this.targetBulletID != -1;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000242D4 File Offset: 0x000224D4
	public int GetBulletCount()
	{
		int num = 0;
		if (base.Item == null)
		{
			return 0;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00024344 File Offset: 0x00022544
	public Item GetCurrentLoadedBullet()
	{
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				return item;
			}
		}
		return null;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x000243A0 File Offset: 0x000225A0
	public int GetBulletCountofTypeInInventory(int bulletItemTypeID, Inventory inventory)
	{
		if (this.targetBulletID == -1)
		{
			return 0;
		}
		int num = 0;
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.TypeID == bulletItemTypeID)
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0002440C File Offset: 0x0002260C
	public void TakeOutAllBullets()
	{
		if (base.Item == null)
		{
			return;
		}
		if (LevelManager.Instance == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				list.Add(item);
			}
		}
		CharacterMainControl characterMainControl = base.Item.GetCharacterMainControl();
		if (base.Item.InInventory && base.Item.InInventory == LevelManager.Instance.PetProxy.Inventory)
		{
			characterMainControl = LevelManager.Instance.MainCharacter;
		}
		else if (base.Item.PluggedIntoSlot != null && base.Item.PluggedIntoSlot.Master != null && base.Item.PluggedIntoSlot.Master.InInventory && base.Item.PluggedIntoSlot.Master.InInventory == LevelManager.Instance.PetProxy.Inventory)
		{
			characterMainControl = LevelManager.Instance.MainCharacter;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Item item2 = list[i];
			if (!(item2 == null))
			{
				if (characterMainControl)
				{
					item2.Drop(characterMainControl, true);
					characterMainControl.PickupItem(item2);
				}
				else
				{
					bool flag = false;
					Inventory inInventory = base.Item.InInventory;
					if (inInventory)
					{
						flag = inInventory.AddAndMerge(item2, 0);
					}
					if (!flag)
					{
						item2.Detach();
						item2.DestroyTree();
					}
				}
			}
		}
		this.bulletCount = 0;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x000245D0 File Offset: 0x000227D0
	public Dictionary<int, BulletTypeInfo> GetBulletTypesInInventory(Inventory inventory)
	{
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				if (!dictionary.ContainsKey(item.TypeID))
				{
					BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
					bulletTypeInfo.bulletTypeID = item.TypeID;
					bulletTypeInfo.count = item.StackCount;
					dictionary.Add(bulletTypeInfo.bulletTypeID, bulletTypeInfo);
				}
				else
				{
					dictionary[item.TypeID].count += item.StackCount;
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x000246C8 File Offset: 0x000228C8
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsGun", true, true);
	}

	// Token: 0x04000778 RID: 1912
	private int targetBulletID = -1;

	// Token: 0x04000779 RID: 1913
	public ADSAimMarker adsAimMarker;

	// Token: 0x0400077A RID: 1914
	public GameObject muzzleFxPfb;

	// Token: 0x0400077B RID: 1915
	public Projectile bulletPfb;

	// Token: 0x0400077C RID: 1916
	public string shootKey = "Default";

	// Token: 0x0400077D RID: 1917
	public string reloadKey = "Default";

	// Token: 0x0400077E RID: 1918
	private int bulletCountHash = "BulletCount".GetHashCode();

	// Token: 0x0400077F RID: 1919
	private int _bulletCountCache = -1;

	// Token: 0x04000780 RID: 1920
	private static int CapacityHash = "Capacity".GetHashCode();

	// Token: 0x04000781 RID: 1921
	private bool loadingBullets;

	// Token: 0x04000782 RID: 1922
	private bool loadBulletsSuccess;

	// Token: 0x04000783 RID: 1923
	private int caliberHash = "Caliber".GetHashCode();

	// Token: 0x04000784 RID: 1924
	public ItemSetting_Gun.TriggerModes triggerMode;

	// Token: 0x04000785 RID: 1925
	public ItemSetting_Gun.ReloadModes reloadMode;

	// Token: 0x04000786 RID: 1926
	public bool autoReload;

	// Token: 0x04000787 RID: 1927
	public ElementTypes element;

	// Token: 0x04000788 RID: 1928
	public Buff buff;

	// Token: 0x04000789 RID: 1929
	private Item preferedBulletsToLoad;

	// Token: 0x02000473 RID: 1139
	public enum TriggerModes
	{
		// Token: 0x04001B88 RID: 7048
		auto,
		// Token: 0x04001B89 RID: 7049
		semi,
		// Token: 0x04001B8A RID: 7050
		bolt
	}

	// Token: 0x02000474 RID: 1140
	public enum ReloadModes
	{
		// Token: 0x04001B8C RID: 7052
		fullMag,
		// Token: 0x04001B8D RID: 7053
		singleBullet
	}
}
