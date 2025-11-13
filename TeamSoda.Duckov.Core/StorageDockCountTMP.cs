using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class StorageDockCountTMP : MonoBehaviour
{
	// Token: 0x06000F20 RID: 3872 RVA: 0x0003C7A6 File Offset: 0x0003A9A6
	private void Awake()
	{
		PlayerStorage.OnItemAddedToBuffer += this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished += this.OnLoadingFinished;
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0003C7DB File Offset: 0x0003A9DB
	private void OnDestroy()
	{
		PlayerStorage.OnItemAddedToBuffer -= this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished -= this.OnLoadingFinished;
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0003C810 File Offset: 0x0003AA10
	private void OnLoadingFinished()
	{
		this.Refresh();
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0003C818 File Offset: 0x0003AA18
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0003C820 File Offset: 0x0003AA20
	private void OnTakeBufferItem()
	{
		this.Refresh();
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0003C828 File Offset: 0x0003AA28
	private void OnItemAddedToBuffer(Item item)
	{
		this.Refresh();
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0003C830 File Offset: 0x0003AA30
	private void Refresh()
	{
		int count = PlayerStorage.IncomingItemBuffer.Count;
		this.tmp.text = string.Format("{0}", count);
		if (this.setActiveFalseWhenCountIsZero)
		{
			base.gameObject.SetActive(count > 0);
		}
	}

	// Token: 0x04000C7A RID: 3194
	[SerializeField]
	private TextMeshPro tmp;

	// Token: 0x04000C7B RID: 3195
	[SerializeField]
	private bool setActiveFalseWhenCountIsZero;
}
