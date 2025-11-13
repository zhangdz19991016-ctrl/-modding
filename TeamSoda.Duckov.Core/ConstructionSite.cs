using System;
using Duckov.Economy;
using Duckov.Scenes;
using Saves;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D4 RID: 212
public class ConstructionSite : MonoBehaviour
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001DBCB File Offset: 0x0001BDCB
	private Color KeyFieldColor
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				return Color.red;
			}
			return Color.white;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
	private string SaveKey
	{
		get
		{
			return "ConstructionSite_" + this._key;
		}
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0001DBFC File Offset: 0x0001BDFC
	private void Awake()
	{
		this.costTaker.onPayed += this.OnBuilt;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
		this.costTaker.SetCost(this.cost);
		this.RefreshGameObjects();
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0001DC4E File Offset: 0x0001BE4E
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0001DC64 File Offset: 0x0001BE64
	private void Save()
	{
		if (this.dontSave)
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			if (MultiSceneCore.Instance.inLevelData.ContainsKey(inLevelDataKey))
			{
				MultiSceneCore.Instance.inLevelData[inLevelDataKey] = this.wasBuilt;
				return;
			}
			MultiSceneCore.Instance.inLevelData.Add(inLevelDataKey, this.wasBuilt);
			return;
		}
		else
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
				return;
			}
			SavesSystem.Save<bool>(this.SaveKey, this.wasBuilt);
			return;
		}
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0001DD00 File Offset: 0x0001BF00
	private int GetInLevelDataKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return ("ConstSite" + vector3Int.ToString()).GetHashCode();
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0001DD6C File Offset: 0x0001BF6C
	private void Load()
	{
		if (!this.dontSave)
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
			}
			this.wasBuilt = SavesSystem.Load<bool>(this.SaveKey);
		}
		else
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			object obj;
			MultiSceneCore.Instance.inLevelData.TryGetValue(inLevelDataKey, out obj);
			if (obj != null)
			{
				this.wasBuilt = (bool)obj;
			}
		}
		if (this.wasBuilt)
		{
			this.OnActivate();
			return;
		}
		this.OnDeactivate();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
	private void Start()
	{
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0001DDF8 File Offset: 0x0001BFF8
	private void OnBuilt(CostTaker taker)
	{
		this.wasBuilt = true;
		UnityEvent<ConstructionSite> unityEvent = this.onBuilt;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
		foreach (GameObject gameObject in this.setActiveOnBuilt)
		{
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.Save();
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0001DE52 File Offset: 0x0001C052
	private void OnActivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onActivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0001DE6C File Offset: 0x0001C06C
	private void OnDeactivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onDeactivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0001DE88 File Offset: 0x0001C088
	public void RefreshGameObjects()
	{
		this.costTaker.gameObject.SetActive(!this.wasBuilt);
		foreach (GameObject gameObject in this.notBuiltGameObjects)
		{
			if (gameObject)
			{
				gameObject.SetActive(!this.wasBuilt);
			}
		}
		foreach (GameObject gameObject2 in this.builtGameObjects)
		{
			if (gameObject2)
			{
				gameObject2.SetActive(this.wasBuilt);
			}
		}
	}

	// Token: 0x0400065D RID: 1629
	[SerializeField]
	private string _key;

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private bool dontSave;

	// Token: 0x0400065F RID: 1631
	private bool saveInMultiSceneCore;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private Cost cost;

	// Token: 0x04000661 RID: 1633
	[SerializeField]
	private CostTaker costTaker;

	// Token: 0x04000662 RID: 1634
	[SerializeField]
	private GameObject[] notBuiltGameObjects;

	// Token: 0x04000663 RID: 1635
	[SerializeField]
	private GameObject[] builtGameObjects;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private GameObject[] setActiveOnBuilt;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	private UnityEvent<ConstructionSite> onBuilt;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private UnityEvent<ConstructionSite> onActivate;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private UnityEvent<ConstructionSite> onDeactivate;

	// Token: 0x04000668 RID: 1640
	private bool wasBuilt;
}
