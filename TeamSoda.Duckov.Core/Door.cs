using System;
using System.Collections.Generic;
using Duckov;
using Duckov.Scenes;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D8 RID: 216
public class Door : MonoBehaviour
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001E412 File Offset: 0x0001C612
	public bool IsOpen
	{
		get
		{
			return !this.closed;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x0001E41D File Offset: 0x0001C61D
	public bool NoRequireItem
	{
		get
		{
			return !this.interact || !this.interact.requireItem;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001E43C File Offset: 0x0001C63C
	public InteractableBase Interact
	{
		get
		{
			return this.interact;
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0001E444 File Offset: 0x0001C644
	private void Start()
	{
		if (this._doorClosedDataKeyCached == -1)
		{
			this._doorClosedDataKeyCached = this.GetKey();
		}
		object obj;
		if (!this.ignoreInLevelData && MultiSceneCore.Instance && MultiSceneCore.Instance.inLevelData.TryGetValue(this._doorClosedDataKeyCached, out obj) && obj is bool)
		{
			bool flag = (bool)obj;
			Debug.Log(string.Format("存在门存档信息：{0}", flag));
			this.closed = flag;
		}
		this.targetLerpValue = (this.closedLerpValue = (this.closed ? 1f : 0f));
		this.SyncNavmeshCut();
		this.SetPartsByLerpValue(true);
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0001E4EE File Offset: 0x0001C6EE
	private void OnEnable()
	{
		if (this.doorCollider)
		{
			this.doorCollider.isTrigger = true;
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0001E509 File Offset: 0x0001C709
	private void OnDisable()
	{
		if (this.doorCollider)
		{
			this.doorCollider.isTrigger = false;
		}
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0001E524 File Offset: 0x0001C724
	private void SyncNavmeshCut()
	{
		if (!this.closed)
		{
			bool enabled = this.activeNavmeshCutWhenDoorIsOpen;
			foreach (NavmeshCut navmeshCut in this.navmeshCuts)
			{
				if (navmeshCut)
				{
					navmeshCut.enabled = enabled;
				}
			}
			return;
		}
		if (this.NoRequireItem)
		{
			return;
		}
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
	private void Update()
	{
		this.targetLerpValue = (this.closed ? 1f : 0f);
		if (this.targetLerpValue == this.closedLerpValue)
		{
			base.enabled = false;
		}
		this.closedLerpValue = Mathf.MoveTowards(this.closedLerpValue, this.targetLerpValue, Time.deltaTime / this.lerpTime);
		this.SetPartsByLerpValue(this.targetLerpValue == this.closedLerpValue);
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0001E613 File Offset: 0x0001C813
	public void Switch()
	{
		this.SetClosed(!this.closed, true);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0001E625 File Offset: 0x0001C825
	public void Open()
	{
		this.SetClosed(false, true);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0001E62F File Offset: 0x0001C82F
	public void Close()
	{
		this.SetClosed(true, true);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0001E639 File Offset: 0x0001C839
	public void ForceSetClosed(bool _closed, bool triggerEvent)
	{
		this.SetClosed(_closed, triggerEvent);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0001E644 File Offset: 0x0001C844
	private void SetClosed(bool _closed, bool triggerEvent = true)
	{
		if (!LevelManager.LevelInited)
		{
			Debug.LogError("在关卡没有初始化时，不能对门进行设置");
			return;
		}
		if (triggerEvent)
		{
			if (_closed)
			{
				UnityEvent onCloseEvent = this.OnCloseEvent;
				if (onCloseEvent != null)
				{
					onCloseEvent.Invoke();
				}
			}
			else
			{
				UnityEvent onOpenEvent = this.OnOpenEvent;
				if (onOpenEvent != null)
				{
					onOpenEvent.Invoke();
				}
			}
		}
		Debug.Log(string.Format("Set Door Closed:{0}", _closed));
		if (this._doorClosedDataKeyCached == -1)
		{
			this._doorClosedDataKeyCached = this.GetKey();
		}
		this.closed = _closed;
		this.targetLerpValue = (this.closed ? 1f : 0f);
		if (this.closedLerpValue != this.targetLerpValue)
		{
			base.enabled = true;
		}
		if (this.hasSound)
		{
			AudioManager.Post(_closed ? this.closeSound : this.openSound, base.gameObject);
		}
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.Instance.inLevelData[this._doorClosedDataKeyCached] = this.closed;
		}
		else
		{
			Debug.Log("没有MultiScene Core，无法存储data");
		}
		this.SyncNavmeshCut();
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0001E750 File Offset: 0x0001C950
	private List<Door.DoorTransformInfo> GetCurrentTransformInfos()
	{
		List<Door.DoorTransformInfo> list = new List<Door.DoorTransformInfo>();
		foreach (Transform transform in this.doorParts)
		{
			Door.DoorTransformInfo item = default(Door.DoorTransformInfo);
			if (transform != null)
			{
				item.target = transform;
				item.localPosition = transform.localPosition;
				item.localRotation = transform.localRotation;
				item.activation = transform.gameObject.activeSelf;
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
	public void SetParts(List<Door.DoorTransformInfo> transforms)
	{
		for (int i = 0; i < transforms.Count; i++)
		{
			Door.DoorTransformInfo doorTransformInfo = transforms[i];
			if (!(doorTransformInfo.target == null))
			{
				doorTransformInfo.target.localPosition = doorTransformInfo.localPosition;
				doorTransformInfo.target.localRotation = doorTransformInfo.localRotation;
				doorTransformInfo.target.gameObject.SetActive(doorTransformInfo.activation);
			}
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0001E868 File Offset: 0x0001CA68
	private void SetPartsByLerpValue(bool setActivation)
	{
		if (this.doorParts.Count != this.closeTransforms.Count || this.doorParts.Count != this.openTransforms.Count)
		{
			return;
		}
		for (int i = 0; i < this.openTransforms.Count; i++)
		{
			Door.DoorTransformInfo doorTransformInfo = this.openTransforms[i];
			Door.DoorTransformInfo doorTransformInfo2 = this.closeTransforms[i];
			if (!(doorTransformInfo.target == null) && !(doorTransformInfo.target != doorTransformInfo2.target))
			{
				doorTransformInfo.target.localPosition = Vector3.Lerp(doorTransformInfo.localPosition, doorTransformInfo2.localPosition, this.closedLerpValue);
				doorTransformInfo.target.localRotation = Quaternion.Lerp(doorTransformInfo.localRotation, doorTransformInfo2.localRotation, this.closedLerpValue);
				if (setActivation)
				{
					if (this.closedLerpValue >= 1f)
					{
						doorTransformInfo.target.gameObject.SetActive(doorTransformInfo2.activation);
					}
					else
					{
						doorTransformInfo.target.gameObject.SetActive(doorTransformInfo.activation);
					}
				}
			}
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0001E990 File Offset: 0x0001CB90
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("Door_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400067A RID: 1658
	private bool closed = true;

	// Token: 0x0400067B RID: 1659
	private float closedLerpValue;

	// Token: 0x0400067C RID: 1660
	private float targetLerpValue;

	// Token: 0x0400067D RID: 1661
	[SerializeField]
	private float lerpTime = 0.5f;

	// Token: 0x0400067E RID: 1662
	[SerializeField]
	private List<Transform> doorParts;

	// Token: 0x0400067F RID: 1663
	[SerializeField]
	private List<Door.DoorTransformInfo> closeTransforms;

	// Token: 0x04000680 RID: 1664
	[SerializeField]
	private List<Door.DoorTransformInfo> openTransforms;

	// Token: 0x04000681 RID: 1665
	[SerializeField]
	private DoorTrigger doorTrigger;

	// Token: 0x04000682 RID: 1666
	[SerializeField]
	private Collider doorCollider;

	// Token: 0x04000683 RID: 1667
	[SerializeField]
	private List<NavmeshCut> navmeshCuts = new List<NavmeshCut>();

	// Token: 0x04000684 RID: 1668
	[SerializeField]
	private bool activeNavmeshCutWhenDoorIsOpen = true;

	// Token: 0x04000685 RID: 1669
	[SerializeField]
	private bool ignoreInLevelData;

	// Token: 0x04000686 RID: 1670
	private int _doorClosedDataKeyCached = -1;

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private InteractableBase interact;

	// Token: 0x04000688 RID: 1672
	public bool hasSound;

	// Token: 0x04000689 RID: 1673
	public string openSound = "SFX/Actions/door_normal_open";

	// Token: 0x0400068A RID: 1674
	public string closeSound = "SFX/Actions/door_normal_close";

	// Token: 0x0400068B RID: 1675
	public UnityEvent OnOpenEvent;

	// Token: 0x0400068C RID: 1676
	public UnityEvent OnCloseEvent;

	// Token: 0x02000469 RID: 1129
	[Serializable]
	public struct DoorTransformInfo
	{
		// Token: 0x04001B5B RID: 7003
		public Transform target;

		// Token: 0x04001B5C RID: 7004
		public Vector3 localPosition;

		// Token: 0x04001B5D RID: 7005
		public quaternion localRotation;

		// Token: 0x04001B5E RID: 7006
		public bool activation;
	}
}
