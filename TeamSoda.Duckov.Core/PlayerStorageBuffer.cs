using System;
using System.Collections.Generic;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class PlayerStorageBuffer : MonoBehaviour
{
	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000867 RID: 2151 RVA: 0x00025C3B File Offset: 0x00023E3B
	// (set) Token: 0x06000868 RID: 2152 RVA: 0x00025C42 File Offset: 0x00023E42
	public static PlayerStorageBuffer Instance { get; private set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x00025C4A File Offset: 0x00023E4A
	public static List<ItemTreeData> Buffer
	{
		get
		{
			return PlayerStorageBuffer.incomingItemBuffer;
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00025C51 File Offset: 0x00023E51
	private void Awake()
	{
		PlayerStorageBuffer.Instance = this;
		PlayerStorageBuffer.LoadBuffer();
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00025C6F File Offset: 0x00023E6F
	private void OnCollectSaveData()
	{
		PlayerStorageBuffer.SaveBuffer();
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00025C78 File Offset: 0x00023E78
	public static void SaveBuffer()
	{
		List<ItemTreeData> list = new List<ItemTreeData>();
		foreach (ItemTreeData itemTreeData in PlayerStorageBuffer.incomingItemBuffer)
		{
			if (itemTreeData != null)
			{
				list.Add(itemTreeData);
			}
		}
		SavesSystem.Save<List<ItemTreeData>>("PlayerStorage_Buffer", list);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00025CE0 File Offset: 0x00023EE0
	public static void LoadBuffer()
	{
		PlayerStorageBuffer.incomingItemBuffer.Clear();
		List<ItemTreeData> list = SavesSystem.Load<List<ItemTreeData>>("PlayerStorage_Buffer");
		if (list != null)
		{
			if (list.Count <= 0)
			{
				Debug.Log("tree data is empty");
			}
			using (List<ItemTreeData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemTreeData item = enumerator.Current;
					PlayerStorageBuffer.incomingItemBuffer.Add(item);
				}
				return;
			}
		}
		Debug.Log("Tree Data is null");
	}

	// Token: 0x040007A5 RID: 1957
	private const string bufferSaveKey = "PlayerStorage_Buffer";

	// Token: 0x040007A6 RID: 1958
	private static List<ItemTreeData> incomingItemBuffer = new List<ItemTreeData>();
}
