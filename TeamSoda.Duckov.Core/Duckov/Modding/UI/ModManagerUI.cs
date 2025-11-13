using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x02000273 RID: 627
	public class ModManagerUI : MonoBehaviour
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0004A0F1 File Offset: 0x000482F1
		private ModManager Master
		{
			get
			{
				return ModManager.Instance;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0004A0F8 File Offset: 0x000482F8
		private PrefabPool<ModEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<ModEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0004A134 File Offset: 0x00048334
		private void Awake()
		{
			this.agreementBtn.onClick.AddListener(new UnityAction(this.OnAgreementBtnClicked));
			this.quitBtn.onClick.AddListener(new UnityAction(this.Quit));
			this.rejectBtn.onClick.AddListener(new UnityAction(this.OnRejectBtnClicked));
			this.needRebootIndicator.SetActive(false);
			ModManager.OnReorder += this.OnReorder;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0004A1B2 File Offset: 0x000483B2
		private void OnDestroy()
		{
			ModManager.OnReorder -= this.OnReorder;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004A1C5 File Offset: 0x000483C5
		private void OnReorder()
		{
			this.Refresh();
			this.needRebootIndicator.SetActive(true);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0004A1D9 File Offset: 0x000483D9
		private void OnRejectBtnClicked()
		{
			ModManager.AllowActivatingMod = false;
			this.Quit();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004A1E7 File Offset: 0x000483E7
		private void OnAgreementBtnClicked()
		{
			ModManager.AllowActivatingMod = true;
			this.agreementFadeGroup.Hide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004A205 File Offset: 0x00048405
		private void Show()
		{
			this.mainFadeGroup.Show();
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004A214 File Offset: 0x00048414
		private void OnEnable()
		{
			ModManager.Rescan();
			this.Refresh();
			this.uploaderFadeGroup.SkipHide();
			if (!ModManager.AllowActivatingMod)
			{
				this.contentFadeGroup.SkipHide();
				this.agreementFadeGroup.Show();
				return;
			}
			this.agreementFadeGroup.SkipHide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004A26C File Offset: 0x0004846C
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			int num = 0;
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				this.Pool.Get(null).Setup(this, modInfo, num);
				num++;
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004A2DC File Offset: 0x000484DC
		private void Hide()
		{
			this.mainFadeGroup.Hide();
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004A2E9 File Offset: 0x000484E9
		private void Quit()
		{
			UnityEvent unityEvent = this.onQuit;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.Hide();
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0004A304 File Offset: 0x00048504
		internal UniTask BeginUpload(ModInfo info)
		{
			ModManagerUI.<BeginUpload>d__27 <BeginUpload>d__;
			<BeginUpload>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BeginUpload>d__.<>4__this = this;
			<BeginUpload>d__.info = info;
			<BeginUpload>d__.<>1__state = -1;
			<BeginUpload>d__.<>t__builder.Start<ModManagerUI.<BeginUpload>d__27>(ref <BeginUpload>d__);
			return <BeginUpload>d__.<>t__builder.Task;
		}

		// Token: 0x04000EAA RID: 3754
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000EAB RID: 3755
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000EAC RID: 3756
		[SerializeField]
		private FadeGroup agreementFadeGroup;

		// Token: 0x04000EAD RID: 3757
		[SerializeField]
		private FadeGroup uploaderFadeGroup;

		// Token: 0x04000EAE RID: 3758
		[SerializeField]
		private ModUploadPanel uploadPanel;

		// Token: 0x04000EAF RID: 3759
		[SerializeField]
		private Button rejectBtn;

		// Token: 0x04000EB0 RID: 3760
		[SerializeField]
		private Button agreementBtn;

		// Token: 0x04000EB1 RID: 3761
		[SerializeField]
		private ModEntry entryTemplate;

		// Token: 0x04000EB2 RID: 3762
		[SerializeField]
		private Button quitBtn;

		// Token: 0x04000EB3 RID: 3763
		[SerializeField]
		private GameObject needRebootIndicator;

		// Token: 0x04000EB4 RID: 3764
		public UnityEvent onQuit;

		// Token: 0x04000EB5 RID: 3765
		private PrefabPool<ModEntry> _pool;

		// Token: 0x04000EB6 RID: 3766
		private bool uploading;
	}
}
