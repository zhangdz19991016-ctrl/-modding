using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001AC RID: 428
public class CraftView : View, ISingleSelectionMenu<CraftView_ListEntry>
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00035C68 File Offset: 0x00033E68
	private static CraftView Instance
	{
		get
		{
			return View.GetViewInstance<CraftView>();
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00035C70 File Offset: 0x00033E70
	private PrefabPool<CraftView_ListEntry> ListEntryPool
	{
		get
		{
			if (this._listEntryPool == null)
			{
				this._listEntryPool = new PrefabPool<CraftView_ListEntry>(this.listEntryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._listEntryPool;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00035CA9 File Offset: 0x00033EA9
	private string NotificationFormat
	{
		get
		{
			return this.notificationFormatKey.ToPlainText();
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00035CB8 File Offset: 0x00033EB8
	private PrefabPool<CraftViewFilterBtnEntry> FilterBtnPool
	{
		get
		{
			if (this._filterBtnPool == null)
			{
				this._filterBtnPool = new PrefabPool<CraftViewFilterBtnEntry>(this.filterBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._filterBtnPool;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00035CF1 File Offset: 0x00033EF1
	private CraftView.FilterInfo CurrentFilter
	{
		get
		{
			if (this.currentFilterIndex < 0 || this.currentFilterIndex >= this.filters.Length)
			{
				this.currentFilterIndex = 0;
			}
			return this.filters[this.currentFilterIndex];
		}
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x00035D24 File Offset: 0x00033F24
	public void SetFilter(int index)
	{
		if (index < 0 || index >= this.filters.Length)
		{
			return;
		}
		this.currentFilterIndex = index;
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(this.predicate);
		this.RefreshFilterButtons();
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00035D5C File Offset: 0x00033F5C
	private static bool CheckFilter(CraftingFormula formula, CraftView.FilterInfo filter)
	{
		if (formula.result.id < 0)
		{
			return false;
		}
		if (filter.requireTags.Length == 0)
		{
			return true;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
		foreach (Tag value in filter.requireTags)
		{
			if (metaData.tags.Contains(value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00035DBF File Offset: 0x00033FBF
	protected override void Awake()
	{
		base.Awake();
		this.listEntryTemplate.gameObject.SetActive(false);
		this.craftButton.onClick.AddListener(new UnityAction(this.OnCraftButtonClicked));
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00035DF4 File Offset: 0x00033FF4
	private void OnCraftButtonClicked()
	{
		this.CraftTask().Forget();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x00035E04 File Offset: 0x00034004
	private UniTask CraftTask()
	{
		CraftView.<CraftTask>d__33 <CraftTask>d__;
		<CraftTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CraftTask>d__.<>4__this = this;
		<CraftTask>d__.<>1__state = -1;
		<CraftTask>d__.<>t__builder.Start<CraftView.<CraftTask>d__33>(ref <CraftTask>d__);
		return <CraftTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00035E48 File Offset: 0x00034048
	private void OnCraftFinished(Item item)
	{
		if (item == null)
		{
			return;
		}
		string displayName = item.DisplayName;
		NotificationText.Push(this.NotificationFormat.Format(new
		{
			itemDisplayName = displayName
		}));
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00035E7C File Offset: 0x0003407C
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.SetFilter(0);
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x00035E96 File Offset: 0x00034096
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00035EA9 File Offset: 0x000340A9
	public static void SetupAndOpenView(Predicate<CraftingFormula> predicate)
	{
		if (!CraftView.Instance)
		{
			return;
		}
		CraftView.Instance.SetupAndOpen(predicate);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00035EC4 File Offset: 0x000340C4
	public void SetupAndOpen(Predicate<CraftingFormula> predicate)
	{
		this.predicate = predicate;
		this.detailsFadeGroup.SkipHide();
		this.loadingIndicator.SkipHide();
		this.placeHolderFadeGroup.SkipShow();
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(predicate);
		this.RefreshFilterButtons();
		base.Open(null);
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00035F1C File Offset: 0x0003411C
	private void RefreshList(Predicate<CraftingFormula> predicate)
	{
		this.ListEntryPool.ReleaseAll();
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		CraftView.FilterInfo currentFilter = this.CurrentFilter;
		bool flag = currentFilter.requireTags != null && currentFilter.requireTags.Length != 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, currentFilter)))
				{
					this.ListEntryPool.Get(null).Setup(this, craftingFormula);
				}
			}
		}
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x00035FBC File Offset: 0x000341BC
	private int CountFilter(CraftView.FilterInfo filter)
	{
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		bool flag = filter.requireTags != null && filter.requireTags.Length != 0;
		int num = 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && this.predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, filter)))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x00036040 File Offset: 0x00034240
	private void RefreshFilterButtons()
	{
		this.FilterBtnPool.ReleaseAll();
		int num = 0;
		foreach (CraftView.FilterInfo filterInfo in this.filters)
		{
			if (this.CountFilter(filterInfo) < 1)
			{
				num++;
			}
			else
			{
				this.FilterBtnPool.Get(null).Setup(this, filterInfo, num, num == this.currentFilterIndex);
				num++;
			}
		}
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x000360A8 File Offset: 0x000342A8
	public CraftView_ListEntry GetSelection()
	{
		return this.selectedEntry;
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x000360B0 File Offset: 0x000342B0
	public bool SetSelection(CraftView_ListEntry selection)
	{
		if (this.selectedEntry != null)
		{
			CraftView_ListEntry craftView_ListEntry = this.selectedEntry;
			this.selectedEntry = null;
			craftView_ListEntry.NotifyUnselected();
		}
		this.selectedEntry = selection;
		this.selectedEntry.NotifySelected();
		this.RefreshDetails();
		return true;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000360EB File Offset: 0x000342EB
	private void RefreshDetails()
	{
		this.RefreshTask(this.NewRefreshToken()).Forget();
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x00036100 File Offset: 0x00034300
	private int NewRefreshToken()
	{
		int num;
		do
		{
			num = UnityEngine.Random.Range(0, int.MaxValue);
		}
		while (num == this.refreshTaskToken);
		this.refreshTaskToken = num;
		return num;
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003612C File Offset: 0x0003432C
	private UniTask RefreshTask(int token)
	{
		CraftView.<RefreshTask>d__50 <RefreshTask>d__;
		<RefreshTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<RefreshTask>d__.<>4__this = this;
		<RefreshTask>d__.token = token;
		<RefreshTask>d__.<>1__state = -1;
		<RefreshTask>d__.<>t__builder.Start<CraftView.<RefreshTask>d__50>(ref <RefreshTask>d__);
		return <RefreshTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x00036177 File Offset: 0x00034377
	private void TestShow()
	{
		CraftingManager.UnlockFormula("Biscuit");
		CraftingManager.UnlockFormula("Character");
		this.SetupAndOpen((CraftingFormula e) => true);
	}

	// Token: 0x04000B09 RID: 2825
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private CraftView_ListEntry listEntryTemplate;

	// Token: 0x04000B0B RID: 2827
	private PrefabPool<CraftView_ListEntry> _listEntryPool;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private FadeGroup detailsFadeGroup;

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private ItemDetailsDisplay detailsDisplay;

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private Color crafableColor;

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private Color notCraftableColor;

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private Image buttonImage;

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private Button craftButton;

	// Token: 0x04000B15 RID: 2837
	[LocalizationKey("Default")]
	[SerializeField]
	private string notificationFormatKey;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private CraftViewFilterBtnEntry filterBtnTemplate;

	// Token: 0x04000B17 RID: 2839
	[SerializeField]
	private CraftView.FilterInfo[] filters;

	// Token: 0x04000B18 RID: 2840
	private PrefabPool<CraftViewFilterBtnEntry> _filterBtnPool;

	// Token: 0x04000B19 RID: 2841
	private int currentFilterIndex;

	// Token: 0x04000B1A RID: 2842
	private bool crafting;

	// Token: 0x04000B1B RID: 2843
	private Predicate<CraftingFormula> predicate;

	// Token: 0x04000B1C RID: 2844
	private CraftView_ListEntry selectedEntry;

	// Token: 0x04000B1D RID: 2845
	private int refreshTaskToken;

	// Token: 0x04000B1E RID: 2846
	private Item tempItem;

	// Token: 0x020004D0 RID: 1232
	[Serializable]
	public struct FilterInfo
	{
		// Token: 0x04001CFE RID: 7422
		[LocalizationKey("Default")]
		[SerializeField]
		public string displayNameKey;

		// Token: 0x04001CFF RID: 7423
		[SerializeField]
		public Sprite icon;

		// Token: 0x04001D00 RID: 7424
		[SerializeField]
		public Tag[] requireTags;
	}
}
