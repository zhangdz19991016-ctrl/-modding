using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002FD RID: 765
	public class EndowmentSelectionPanel : View
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x0005B690 File Offset: 0x00059890
		private PrefabPool<EndowmentSelectionEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<EndowmentSelectionEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, delegate(EndowmentSelectionEntry e)
					{
						e.onClicked = (Action<EndowmentSelectionEntry, PointerEventData>)Delegate.Combine(e.onClicked, new Action<EndowmentSelectionEntry, PointerEventData>(this.OnEntryClicked));
					});
				}
				return this._pool;
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0005B6D4 File Offset: 0x000598D4
		protected override void Awake()
		{
			base.Awake();
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0005B714 File Offset: 0x00059914
		protected override void OnCancel()
		{
			base.OnCancel();
			this.canceled = true;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0005B723 File Offset: 0x00059923
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0005B72C File Offset: 0x0005992C
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0005B735 File Offset: 0x00059935
		private void OnEntryClicked(EndowmentSelectionEntry entry, PointerEventData data)
		{
			if (entry.Locked)
			{
				return;
			}
			this.Select(entry);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0005B748 File Offset: 0x00059948
		private void Select(EndowmentSelectionEntry entry)
		{
			this.Selection = entry;
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				endowmentSelectionEntry.SetSelection(endowmentSelectionEntry == entry);
			}
			this.RefreshDescription();
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x0005B7AC File Offset: 0x000599AC
		// (set) Token: 0x0600190E RID: 6414 RVA: 0x0005B7B4 File Offset: 0x000599B4
		public EndowmentSelectionEntry Selection { get; private set; }

		// Token: 0x0600190F RID: 6415 RVA: 0x0005B7C0 File Offset: 0x000599C0
		public void Setup()
		{
			if (EndowmentManager.Instance == null)
			{
				return;
			}
			this.Pool.ReleaseAll();
			foreach (EndowmentEntry endowmentEntry in EndowmentManager.Instance.Entries)
			{
				if (!(endowmentEntry == null))
				{
					this.Pool.Get(null).Setup(endowmentEntry);
				}
			}
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				if (endowmentSelectionEntry.Target.Index == EndowmentManager.SelectedIndex)
				{
					this.Select(endowmentSelectionEntry);
					break;
				}
			}
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0005B894 File Offset: 0x00059A94
		private void RefreshDescription()
		{
			if (this.Selection == null)
			{
				this.descriptionText.text = "-";
			}
			this.descriptionText.text = this.Selection.DescriptionAndEffects;
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0005B8CA File Offset: 0x00059ACA
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Execute().Forget();
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0005B8DD File Offset: 0x00059ADD
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0005B8F0 File Offset: 0x00059AF0
		public UniTask Execute()
		{
			EndowmentSelectionPanel.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<EndowmentSelectionPanel.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0005B934 File Offset: 0x00059B34
		private UniTask WaitForConfirm()
		{
			EndowmentSelectionPanel.<WaitForConfirm>d__25 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<EndowmentSelectionPanel.<WaitForConfirm>d__25>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0005B977 File Offset: 0x00059B77
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0005B994 File Offset: 0x00059B94
		public static void Show()
		{
			EndowmentSelectionPanel viewInstance = View.GetViewInstance<EndowmentSelectionPanel>();
			if (viewInstance == null)
			{
				return;
			}
			viewInstance.Open(null);
		}

		// Token: 0x0400122A RID: 4650
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400122B RID: 4651
		[SerializeField]
		private EndowmentSelectionEntry entryTemplate;

		// Token: 0x0400122C RID: 4652
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x0400122D RID: 4653
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400122F RID: 4655
		private PrefabPool<EndowmentSelectionEntry> _pool;

		// Token: 0x04001231 RID: 4657
		private bool confirmed;

		// Token: 0x04001232 RID: 4658
		private bool canceled;
	}
}
