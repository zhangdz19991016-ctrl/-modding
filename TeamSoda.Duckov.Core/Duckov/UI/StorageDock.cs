using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B5 RID: 949
	public class StorageDock : View
	{
		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000778C1 File Offset: 0x00075AC1
		public static StorageDock Instance
		{
			get
			{
				return View.GetViewInstance<StorageDock>();
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000778C8 File Offset: 0x00075AC8
		private int TotalItemCount
		{
			get
			{
				if (PlayerStorage.IncomingItemBuffer == null)
				{
					return 0;
				}
				return PlayerStorage.IncomingItemBuffer.Count;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002237 RID: 8759 RVA: 0x000778DD File Offset: 0x00075ADD
		private int MaxPage
		{
			get
			{
				return this.TotalItemCount / 24;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x000778E8 File Offset: 0x00075AE8
		private PrefabPool<StorageDockEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StorageDockEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00077924 File Offset: 0x00075B24
		protected override void Awake()
		{
			base.Awake();
			this.entryTemplate.gameObject.SetActive(false);
			this.btnNextPage.onClick.AddListener(new UnityAction(this.NextPage));
			this.btnPrevPage.onClick.AddListener(new UnityAction(this.PrevPage));
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00077980 File Offset: 0x00075B80
		private void OnEnable()
		{
			PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00077993 File Offset: 0x00075B93
		private void OnDisable()
		{
			PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000779A6 File Offset: 0x00075BA6
		private void OnTakeBufferItem()
		{
			this.Refresh();
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000779AE File Offset: 0x00075BAE
		protected override void OnOpen()
		{
			base.OnOpen();
			if (PlayerStorage.Instance == null)
			{
				base.Close();
				return;
			}
			this.fadeGroup.Show();
			this.Setup();
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000779DB File Offset: 0x00075BDB
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000779EE File Offset: 0x00075BEE
		private void Setup()
		{
			this.Refresh();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000779F8 File Offset: 0x00075BF8
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			List<ItemTreeData> incomingItemBuffer = PlayerStorage.IncomingItemBuffer;
			int num = this.page * 24;
			int num2 = num + 24;
			if (num2 > incomingItemBuffer.Count)
			{
				num2 = incomingItemBuffer.Count;
			}
			for (int i = num; i < num2; i++)
			{
				ItemTreeData itemTreeData = incomingItemBuffer[i];
				if (itemTreeData != null)
				{
					this.EntryPool.Get(null).Setup(i, itemTreeData);
				}
			}
			this.placeHolder.gameObject.SetActive(incomingItemBuffer.Count <= 0);
			this.pageText.text = string.Format("{0}/{1}", this.page + 1, this.MaxPage + 1);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00077AA8 File Offset: 0x00075CA8
		public void NextPage()
		{
			this.GotoPage(this.page + 1);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00077AB8 File Offset: 0x00075CB8
		public void PrevPage()
		{
			this.GotoPage(this.page - 1);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00077AC8 File Offset: 0x00075CC8
		public void GotoPage(int page)
		{
			if (page < 0)
			{
				page = 0;
			}
			if (page > this.MaxPage)
			{
				page = this.MaxPage;
			}
			this.page = page;
			this.Refresh();
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00077AEF File Offset: 0x00075CEF
		internal static void Show()
		{
			if (StorageDock.Instance == null)
			{
				return;
			}
			StorageDock.Instance.Open(null);
		}

		// Token: 0x04001710 RID: 5904
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001711 RID: 5905
		[SerializeField]
		private StorageDockEntry entryTemplate;

		// Token: 0x04001712 RID: 5906
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x04001713 RID: 5907
		[SerializeField]
		private TextMeshProUGUI pageText;

		// Token: 0x04001714 RID: 5908
		[SerializeField]
		private Button btnNextPage;

		// Token: 0x04001715 RID: 5909
		[SerializeField]
		private Button btnPrevPage;

		// Token: 0x04001716 RID: 5910
		private const int itemsPerPage = 24;

		// Token: 0x04001717 RID: 5911
		private int page;

		// Token: 0x04001718 RID: 5912
		private PrefabPool<StorageDockEntry> _entryPool;
	}
}
