using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C0 RID: 960
	public class MapSelectionView : View
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x0007A82F File Offset: 0x00078A2F
		public static MapSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<MapSelectionView>();
			}
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0007A836 File Offset: 0x00078A36
		protected override void Awake()
		{
			base.Awake();
			this.btnConfirm.onClick.AddListener(delegate()
			{
				this.confirmButtonClicked = true;
			});
			this.btnCancel.onClick.AddListener(delegate()
			{
				this.cancelButtonClicked = true;
			});
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0007A876 File Offset: 0x00078A76
		protected override void OnOpen()
		{
			base.OnOpen();
			this.confirmIndicatorFadeGroup.SkipHide();
			this.mainFadeGroup.Show();
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0007A894 File Offset: 0x00078A94
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0007A8A8 File Offset: 0x00078AA8
		internal void NotifyEntryClicked(MapSelectionEntry mapSelectionEntry, PointerEventData eventData)
		{
			if (!mapSelectionEntry.Cost.Enough)
			{
				return;
			}
			AudioManager.Post(this.sfx_EntryClicked);
			string sceneID = mapSelectionEntry.SceneID;
			LevelManager.loadLevelBeaconIndex = mapSelectionEntry.BeaconIndex;
			this.loading = true;
			this.LoadTask(sceneID, mapSelectionEntry.Cost).Forget();
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0007A900 File Offset: 0x00078B00
		private UniTask LoadTask(string sceneID, Cost cost)
		{
			MapSelectionView.<LoadTask>d__18 <LoadTask>d__;
			<LoadTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadTask>d__.<>4__this = this;
			<LoadTask>d__.sceneID = sceneID;
			<LoadTask>d__.cost = cost;
			<LoadTask>d__.<>1__state = -1;
			<LoadTask>d__.<>t__builder.Start<MapSelectionView.<LoadTask>d__18>(ref <LoadTask>d__);
			return <LoadTask>d__.<>t__builder.Task;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0007A954 File Offset: 0x00078B54
		private UniTask<bool> WaitForConfirm()
		{
			MapSelectionView.<WaitForConfirm>d__21 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<MapSelectionView.<WaitForConfirm>d__21>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0007A998 File Offset: 0x00078B98
		private void SetupSceneInfo(SceneInfoEntry info)
		{
			if (info == null)
			{
				return;
			}
			string displayName = info.DisplayName;
			this.destinationDisplayNameText.text = displayName;
			this.destinationDisplayNameText.color = Color.white;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0007A9CC File Offset: 0x00078BCC
		internal override void TryQuit()
		{
			if (!this.loading)
			{
				base.Close();
			}
		}

		// Token: 0x040017AD RID: 6061
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x040017AE RID: 6062
		[SerializeField]
		private FadeGroup confirmIndicatorFadeGroup;

		// Token: 0x040017AF RID: 6063
		[SerializeField]
		private TextMeshProUGUI destinationDisplayNameText;

		// Token: 0x040017B0 RID: 6064
		[SerializeField]
		private CostDisplay confirmCostDisplay;

		// Token: 0x040017B1 RID: 6065
		private string sfx_EntryClicked = "UI/confirm";

		// Token: 0x040017B2 RID: 6066
		private string sfx_ShowDestination = "UI/destination_show";

		// Token: 0x040017B3 RID: 6067
		private string sfx_ConfirmDestination = "UI/destination_confirm";

		// Token: 0x040017B4 RID: 6068
		[SerializeField]
		private ColorPunch confirmColorPunch;

		// Token: 0x040017B5 RID: 6069
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x040017B6 RID: 6070
		[SerializeField]
		private Button btnCancel;

		// Token: 0x040017B7 RID: 6071
		[SerializeField]
		private SceneReference overrideLoadingScreen;

		// Token: 0x040017B8 RID: 6072
		private bool loading;

		// Token: 0x040017B9 RID: 6073
		private bool confirmButtonClicked;

		// Token: 0x040017BA RID: 6074
		private bool cancelButtonClicked;
	}
}
