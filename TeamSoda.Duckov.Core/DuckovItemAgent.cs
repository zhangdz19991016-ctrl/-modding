using System;
using System.Collections.Generic;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E4 RID: 228
public class DuckovItemAgent : ItemAgent
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x00020913 File Offset: 0x0001EB13
	public CharacterMainControl Holder
	{
		get
		{
			return this.holder;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x0002091C File Offset: 0x0001EB1C
	private Dictionary<string, Transform> SocketsDic
	{
		get
		{
			if (this._socketsDic == null)
			{
				this._socketsDic = new Dictionary<string, Transform>();
				foreach (Transform transform in this.socketsList)
				{
					this._socketsDic.Add(transform.name, transform);
				}
			}
			return this._socketsDic;
		}
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00020994 File Offset: 0x0001EB94
	public Transform GetSocket(string socketName, bool createNew)
	{
		Transform transform;
		bool flag = this.SocketsDic.TryGetValue(socketName, out transform);
		if (flag && transform == null)
		{
			this.SocketsDic.Remove(socketName);
		}
		if (!flag && createNew)
		{
			transform = new GameObject(socketName).transform;
			transform.SetParent(base.transform);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			this.SocketsDic.Add(socketName, transform);
		}
		return transform;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00020A0B File Offset: 0x0001EC0B
	public void SetHolder(CharacterMainControl _holder)
	{
		this.holder = _holder;
		if (this.setActiveIfMainCharacter)
		{
			this.setActiveIfMainCharacter.SetActive(_holder.IsMainCharacter);
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00020A32 File Offset: 0x0001EC32
	public CharacterMainControl GetHolder()
	{
		return this.holder;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00020A3A File Offset: 0x0001EC3A
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.InitInterfaces();
		UnityEvent onInitializdEvent = this.OnInitializdEvent;
		if (onInitializdEvent == null)
		{
			return;
		}
		onInitializdEvent.Invoke();
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00020A58 File Offset: 0x0001EC58
	private void InitInterfaces()
	{
		this.usableInterface = (this as IAgentUsable);
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00020A66 File Offset: 0x0001EC66
	public IAgentUsable UsableInterface
	{
		get
		{
			return this.usableInterface;
		}
	}

	// Token: 0x040006E7 RID: 1767
	public HandheldSocketTypes handheldSocket = HandheldSocketTypes.normalHandheld;

	// Token: 0x040006E8 RID: 1768
	public HandheldAnimationType handAnimationType = HandheldAnimationType.normal;

	// Token: 0x040006E9 RID: 1769
	private CharacterMainControl holder;

	// Token: 0x040006EA RID: 1770
	public UnityEvent OnInitializdEvent;

	// Token: 0x040006EB RID: 1771
	[SerializeField]
	private List<Transform> socketsList = new List<Transform>();

	// Token: 0x040006EC RID: 1772
	public GameObject setActiveIfMainCharacter;

	// Token: 0x040006ED RID: 1773
	private Dictionary<string, Transform> _socketsDic;

	// Token: 0x040006EE RID: 1774
	private IAgentUsable usableInterface;
}
