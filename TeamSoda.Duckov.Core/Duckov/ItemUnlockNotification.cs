using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x02000243 RID: 579
	public class ItemUnlockNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00045D09 File Offset: 0x00043F09
		public string MainTextFormat
		{
			get
			{
				return this.mainTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00045D16 File Offset: 0x00043F16
		private string SubTextFormat
		{
			get
			{
				return this.subTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x00045D23 File Offset: 0x00043F23
		// (set) Token: 0x06001219 RID: 4633 RVA: 0x00045D2A File Offset: 0x00043F2A
		public static ItemUnlockNotification Instance { get; private set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00045D32 File Offset: 0x00043F32
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x00045D42 File Offset: 0x00043F42
		public static bool Showing
		{
			get
			{
				return !(ItemUnlockNotification.Instance == null) && ItemUnlockNotification.Instance.showing;
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00045D5D File Offset: 0x00043F5D
		private void Awake()
		{
			if (ItemUnlockNotification.Instance == null)
			{
				ItemUnlockNotification.Instance = this;
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00045D72 File Offset: 0x00043F72
		private void Update()
		{
			if (!this.showing && ItemUnlockNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00045D8F File Offset: 0x00043F8F
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00045DA0 File Offset: 0x00043FA0
		private UniTask ShowTask()
		{
			ItemUnlockNotification.<ShowTask>d__26 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<ItemUnlockNotification.<ShowTask>d__26>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00045DE4 File Offset: 0x00043FE4
		private UniTask DisplayContent(int itemTypeID)
		{
			ItemUnlockNotification.<DisplayContent>d__27 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.itemTypeID = itemTypeID;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<ItemUnlockNotification.<DisplayContent>d__27>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00045E30 File Offset: 0x00044030
		private void Setup(int itemTypeID)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			string displayName = metaData.DisplayName;
			Sprite icon = metaData.icon;
			this.image.sprite = icon;
			this.textMain.text = this.MainTextFormat.Format(new
			{
				itemDisplayName = displayName
			});
			this.textSub.text = this.SubTextFormat;
			DisplayQuality displayQuality = metaData.displayQuality;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(displayQuality).Apply(this.shadow);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00045EA9 File Offset: 0x000440A9
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointerClicked = true;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00045EB2 File Offset: 0x000440B2
		public static void Push(int itemTypeID)
		{
			ItemUnlockNotification.pending.Add(itemTypeID);
		}

		// Token: 0x04000DE2 RID: 3554
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000DE3 RID: 3555
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000DE4 RID: 3556
		[SerializeField]
		private Image image;

		// Token: 0x04000DE5 RID: 3557
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000DE6 RID: 3558
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000DE7 RID: 3559
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		[LocalizationKey("Default")]
		private string mainTextFormatKey = "UI_ItemUnlockNotification";

		// Token: 0x04000DEA RID: 3562
		[SerializeField]
		[LocalizationKey("Default")]
		private string subTextFormatKey = "UI_ItemUnlockNotification_Sub";

		// Token: 0x04000DEB RID: 3563
		private static List<int> pending = new List<int>();

		// Token: 0x04000DED RID: 3565
		private UniTask showingTask;

		// Token: 0x04000DEE RID: 3566
		private bool pointerClicked;
	}
}
