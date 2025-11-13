using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000396 RID: 918
	public class InventoryDisplay : MonoBehaviour, IPoolable
	{
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x000705E7 File Offset: 0x0006E7E7
		private bool shortcuts
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x000705EA File Offset: 0x0006E7EA
		public bool UsePages
		{
			get
			{
				return this.usePages;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x000705F2 File Offset: 0x0006E7F2
		// (set) Token: 0x06002010 RID: 8208 RVA: 0x000705FA File Offset: 0x0006E7FA
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x00070603 File Offset: 0x0006E803
		// (set) Token: 0x06002012 RID: 8210 RVA: 0x0007060B File Offset: 0x0006E80B
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00070614 File Offset: 0x0006E814
		// (set) Token: 0x06002014 RID: 8212 RVA: 0x0007061C File Offset: 0x0006E81C
		public bool Movable { get; private set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x00070625 File Offset: 0x0006E825
		// (set) Token: 0x06002016 RID: 8214 RVA: 0x0007062D File Offset: 0x0006E82D
		public Inventory Target { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x00070638 File Offset: 0x0006E838
		private PrefabPool<InventoryEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null && this.entryPrefab != null)
				{
					this._entryPool = new PrefabPool<InventoryEntry>(this.entryPrefab, this.contentLayout.transform, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06002018 RID: 8216 RVA: 0x0007068C File Offset: 0x0006E88C
		// (remove) Token: 0x06002019 RID: 8217 RVA: 0x000706C4 File Offset: 0x0006E8C4
		public event Action<InventoryDisplay, InventoryEntry, PointerEventData> onDisplayDoubleClicked;

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x0600201A RID: 8218 RVA: 0x000706FC File Offset: 0x0006E8FC
		// (remove) Token: 0x0600201B RID: 8219 RVA: 0x00070734 File Offset: 0x0006E934
		public event Action onPageInfoRefreshed;

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x00070769 File Offset: 0x0006E969
		public Func<Item, bool> Func_ShouldHighlight
		{
			get
			{
				return this._func_ShouldHighlight;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00070771 File Offset: 0x0006E971
		public Func<Item, bool> Func_CanOperate
		{
			get
			{
				return this._func_CanOperate;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x00070779 File Offset: 0x0006E979
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x00070781 File Offset: 0x0006E981
		public bool ShowSortButton
		{
			get
			{
				return this.showSortButton;
			}
			internal set
			{
				this.showSortButton = value;
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0007078C File Offset: 0x0006E98C
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onContentChanged += this.OnTargetContentChanged;
			this.Target.onInventorySorted += this.OnTargetSorted;
			this.Target.onSetIndexLock += this.OnTargetSetIndexLock;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000707F4 File Offset: 0x0006E9F4
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onContentChanged -= this.OnTargetContentChanged;
			this.Target.onInventorySorted -= this.OnTargetSorted;
			this.Target.onSetIndexLock -= this.OnTargetSetIndexLock;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00070858 File Offset: 0x0006EA58
		private void OnTargetSetIndexLock(Inventory inventory, int index)
		{
			foreach (InventoryEntry inventoryEntry in this.entries)
			{
				if (!(inventoryEntry == null) && inventoryEntry.isActiveAndEnabled && inventoryEntry.Index == index)
				{
					inventoryEntry.Refresh();
				}
			}
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000708C4 File Offset: 0x0006EAC4
		private void OnTargetSorted(Inventory inventory)
		{
			if (this.filter == null)
			{
				using (List<InventoryEntry>.Enumerator enumerator = this.entries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InventoryEntry inventoryEntry = enumerator.Current;
						inventoryEntry.Refresh();
					}
					return;
				}
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00070928 File Offset: 0x0006EB28
		private void OnTargetContentChanged(Inventory inventory, int position)
		{
			if (this.Target.Loading)
			{
				return;
			}
			if (this.filter != null)
			{
				this.RefreshCapacityText();
				this.LoadEntriesTask().Forget();
				return;
			}
			this.RefreshCapacityText();
			InventoryEntry inventoryEntry = this.entries.Find((InventoryEntry e) => e != null && e.Index == position);
			if (!inventoryEntry)
			{
				return;
			}
			InventoryEntry inventoryEntry2 = inventoryEntry;
			inventoryEntry2.Refresh();
			inventoryEntry2.Punch();
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000709A0 File Offset: 0x0006EBA0
		private void RefreshCapacityText()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.capacityText)
			{
				return;
			}
			this.capacityText.text = string.Format(this.capacityTextFormat, this.Target.Capacity, this.Target.GetItemCount());
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00070A00 File Offset: 0x0006EC00
		public void Setup(Inventory target, Func<Item, bool> funcShouldHighLight = null, Func<Item, bool> funcCanOperate = null, bool movable = false, Func<Item, bool> filter = null)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			if (funcShouldHighLight == null)
			{
				this._func_ShouldHighlight = ((Item e) => false);
			}
			else
			{
				this._func_ShouldHighlight = funcShouldHighLight;
			}
			if (funcCanOperate == null)
			{
				this._func_CanOperate = ((Item e) => true);
			}
			else
			{
				this._func_CanOperate = funcCanOperate;
			}
			this.displayNameText.text = target.DisplayName;
			this.Movable = movable;
			this.cachedCapacity = target.Capacity;
			this.filter = filter;
			this.RefreshCapacityText();
			this.RegisterEvents();
			this.sortButton.gameObject.SetActive(this.editable && this.showSortButton);
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00070B04 File Offset: 0x0006ED04
		private void RefreshGridLayoutPreferredHeight()
		{
			if (this.Target == null)
			{
				this.placeHolder.gameObject.SetActive(true);
				return;
			}
			int num = this.cachedIndexesToDisplay.Count;
			if (this.usePages && num > 0)
			{
				int num2 = this.cachedSelectedPage * this.itemsEachPage;
				int num3 = Mathf.Min(num2 + this.itemsEachPage, this.cachedIndexesToDisplay.Count);
				num = Mathf.Max(0, num3 - num2);
			}
			float preferredHeight = (float)Mathf.CeilToInt((float)num / (float)this.contentLayout.constraintCount) * this.contentLayout.cellSize.y + (float)this.contentLayout.padding.top + (float)this.contentLayout.padding.bottom;
			this.gridLayoutElement.preferredHeight = preferredHeight;
			this.placeHolder.gameObject.SetActive(num <= 0);
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x00070BE8 File Offset: 0x0006EDE8
		public int MaxPage
		{
			get
			{
				return this.cachedMaxPage;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x00070BF0 File Offset: 0x0006EDF0
		public int SelectedPage
		{
			get
			{
				return this.cachedSelectedPage;
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00070BF8 File Offset: 0x0006EDF8
		public void SetPage(int page)
		{
			this.cachedSelectedPage = page;
			Action action = this.onPageInfoRefreshed;
			if (action != null)
			{
				action();
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00070C20 File Offset: 0x0006EE20
		public void NextPage()
		{
			int num = this.cachedSelectedPage + 1;
			if (num >= this.cachedMaxPage)
			{
				num = 0;
			}
			this.SetPage(num);
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00070C48 File Offset: 0x0006EE48
		public void PreviousPage()
		{
			int num = this.cachedSelectedPage - 1;
			if (num < 0)
			{
				num = this.cachedMaxPage - 1;
			}
			this.SetPage(num);
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00070C74 File Offset: 0x0006EE74
		private void CacheIndexesToDisplay()
		{
			this.cachedIndexesToDisplay.Clear();
			int i = 0;
			while (i < this.Target.Capacity)
			{
				if (this.filter == null)
				{
					goto IL_32;
				}
				Item itemAt = this.Target.GetItemAt(i);
				if (this.filter(itemAt))
				{
					goto IL_32;
				}
				IL_3E:
				i++;
				continue;
				IL_32:
				this.cachedIndexesToDisplay.Add(i);
				goto IL_3E;
			}
			int count = this.cachedIndexesToDisplay.Count;
			this.cachedMaxPage = count / this.itemsEachPage + ((count % this.itemsEachPage > 0) ? 1 : 0);
			if (this.cachedSelectedPage >= this.cachedMaxPage)
			{
				this.cachedSelectedPage = Mathf.Max(0, this.cachedMaxPage - 1);
			}
			Action action = this.onPageInfoRefreshed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00070D30 File Offset: 0x0006EF30
		private UniTask LoadEntriesTask()
		{
			InventoryDisplay.<LoadEntriesTask>d__76 <LoadEntriesTask>d__;
			<LoadEntriesTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadEntriesTask>d__.<>4__this = this;
			<LoadEntriesTask>d__.<>1__state = -1;
			<LoadEntriesTask>d__.<>t__builder.Start<InventoryDisplay.<LoadEntriesTask>d__76>(ref <LoadEntriesTask>d__);
			return <LoadEntriesTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00070D73 File Offset: 0x0006EF73
		public void SetFilter(Func<Item, bool> filter)
		{
			this.filter = filter;
			this.cachedSelectedPage = 0;
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00070D8E File Offset: 0x0006EF8E
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			this.entries.Clear();
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00070DA6 File Offset: 0x0006EFA6
		private void Awake()
		{
			this.sortButton.onClick.AddListener(new UnityAction(this.OnSortButtonClicked));
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00070DC4 File Offset: 0x0006EFC4
		private void OnSortButtonClicked()
		{
			if (!this.Editable)
			{
				return;
			}
			if (!this.Target)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			this.Target.Sort();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00070DF6 File Offset: 0x0006EFF6
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00070DFE File Offset: 0x0006EFFE
		private void OnDisable()
		{
			this.UnregisterEvents();
			this.activeTaskToken++;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00070E14 File Offset: 0x0006F014
		private void Update()
		{
			if (this.Target && this.cachedCapacity != this.Target.Capacity)
			{
				this.OnCapacityChanged();
			}
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00070E3C File Offset: 0x0006F03C
		private void OnCapacityChanged()
		{
			if (this.Target == null)
			{
				return;
			}
			this.cachedCapacity = this.Target.Capacity;
			this.RefreshCapacityText();
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00070E6F File Offset: 0x0006F06F
		public bool IsShortcut(int index)
		{
			return this.shortcuts && index >= this.shortcutsRange.x && index <= this.shortcutsRange.y;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x00070E9C File Offset: 0x0006F09C
		private InventoryEntry GetNewInventoryEntry()
		{
			return this.EntryPool.Get(null);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00070EAA File Offset: 0x0006F0AA
		internal void NotifyItemDoubleClicked(InventoryEntry inventoryEntry, PointerEventData data)
		{
			Action<InventoryDisplay, InventoryEntry, PointerEventData> action = this.onDisplayDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, inventoryEntry, data);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00070EBF File Offset: 0x0006F0BF
		public void NotifyPooled()
		{
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00070EC1 File Offset: 0x0006F0C1
		public void NotifyReleased()
		{
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00070EC4 File Offset: 0x0006F0C4
		public void DisableItem(Item item)
		{
			foreach (InventoryEntry inventoryEntry in from e in this.entries
			where e.Content == item
			select e)
			{
				inventoryEntry.Disabled = true;
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00070F30 File Offset: 0x0006F130
		internal bool EvaluateShouldHighlight(Item content)
		{
			if (this.Func_ShouldHighlight != null && this.Func_ShouldHighlight(content))
			{
				return true;
			}
			content == null;
			return false;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00070FB9 File Offset: 0x0006F1B9
		[CompilerGenerated]
		private bool <LoadEntriesTask>g__TaskValid|76_0(ref InventoryDisplay.<>c__DisplayClass76_0 A_1)
		{
			return Application.isPlaying && A_1.token == this.activeTaskToken;
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00070FD4 File Offset: 0x0006F1D4
		[CompilerGenerated]
		private List<int> <LoadEntriesTask>g__GetRange|76_1(int begin, int end_exclusive, List<int> list, ref InventoryDisplay.<>c__DisplayClass76_0 A_4)
		{
			if (begin < 0)
			{
				begin = 0;
			}
			if (end_exclusive < 0)
			{
				end_exclusive = 0;
			}
			A_4.indexes = new List<int>();
			if (end_exclusive > list.Count)
			{
				end_exclusive = list.Count;
			}
			if (begin >= end_exclusive)
			{
				return A_4.indexes;
			}
			for (int i = begin; i < end_exclusive; i++)
			{
				A_4.indexes.Add(list[i]);
			}
			return A_4.indexes;
		}

		// Token: 0x040015DE RID: 5598
		[SerializeField]
		private InventoryEntry entryPrefab;

		// Token: 0x040015DF RID: 5599
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040015E0 RID: 5600
		[SerializeField]
		private TextMeshProUGUI capacityText;

		// Token: 0x040015E1 RID: 5601
		[SerializeField]
		private string capacityTextFormat = "({1}/{0})";

		// Token: 0x040015E2 RID: 5602
		[SerializeField]
		private FadeGroup loadingIndcator;

		// Token: 0x040015E3 RID: 5603
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x040015E4 RID: 5604
		[SerializeField]
		private GridLayoutGroup contentLayout;

		// Token: 0x040015E5 RID: 5605
		[SerializeField]
		private LayoutElement gridLayoutElement;

		// Token: 0x040015E6 RID: 5606
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x040015E7 RID: 5607
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x040015E8 RID: 5608
		[SerializeField]
		private Button sortButton;

		// Token: 0x040015E9 RID: 5609
		[SerializeField]
		private Vector2Int shortcutsRange = new Vector2Int(0, 3);

		// Token: 0x040015EA RID: 5610
		[SerializeField]
		private bool editable = true;

		// Token: 0x040015EB RID: 5611
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x040015EC RID: 5612
		[SerializeField]
		private bool showSortButton;

		// Token: 0x040015ED RID: 5613
		[SerializeField]
		private bool usePages;

		// Token: 0x040015EE RID: 5614
		[SerializeField]
		private int itemsEachPage = 30;

		// Token: 0x040015EF RID: 5615
		public Func<Item, bool> filter;

		// Token: 0x040015F2 RID: 5618
		[SerializeField]
		private List<InventoryEntry> entries = new List<InventoryEntry>();

		// Token: 0x040015F3 RID: 5619
		private PrefabPool<InventoryEntry> _entryPool;

		// Token: 0x040015F6 RID: 5622
		private Func<Item, bool> _func_ShouldHighlight;

		// Token: 0x040015F7 RID: 5623
		private Func<Item, bool> _func_CanOperate;

		// Token: 0x040015F8 RID: 5624
		private int cachedCapacity = -1;

		// Token: 0x040015F9 RID: 5625
		private int activeTaskToken;

		// Token: 0x040015FA RID: 5626
		private int cachedMaxPage = 1;

		// Token: 0x040015FB RID: 5627
		private int cachedSelectedPage;

		// Token: 0x040015FC RID: 5628
		private List<int> cachedIndexesToDisplay = new List<int>();
	}
}
