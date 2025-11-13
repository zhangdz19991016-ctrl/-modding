using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Modding;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001C6 RID: 454
public class ModUploadPanel : MonoBehaviour
{
	// Token: 0x06000D9A RID: 3482 RVA: 0x0003887B File Offset: 0x00036A7B
	private void Awake()
	{
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCancelBtnClick));
		this.btnUpload.onClick.AddListener(new UnityAction(this.OnUploadBtnClick));
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x000388B5 File Offset: 0x00036AB5
	private void OnUploadBtnClick()
	{
		this.uploadClicked = true;
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x000388BE File Offset: 0x00036ABE
	private void OnCancelBtnClick()
	{
		this.cancelClicked = true;
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x000388C8 File Offset: 0x00036AC8
	public UniTask Execute(ModInfo info)
	{
		ModUploadPanel.<Execute>d__29 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.info = info;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ModUploadPanel.<Execute>d__29>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x00038914 File Offset: 0x00036B14
	private void Update()
	{
		if (this.waitingForUpload)
		{
			this.progressBarFill.fillAmount = SteamWorkshopManager.UploadingProgress;
			ulong punBytesProcess = SteamWorkshopManager.punBytesProcess;
			ulong punBytesTotal = SteamWorkshopManager.punBytesTotal;
			this.progressText.text = ModUploadPanel.FormatBytes(punBytesProcess) + " / " + ModUploadPanel.FormatBytes(punBytesTotal);
		}
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x00038968 File Offset: 0x00036B68
	private static string FormatBytes(ulong bytes)
	{
		if (bytes < 1024UL)
		{
			return string.Format("{0}bytes", bytes);
		}
		if (bytes < 1048576UL)
		{
			return string.Format("{0:0.0}KB", bytes / 1024f);
		}
		if (bytes < 1073741824UL)
		{
			return string.Format("{0:0.0}MB", bytes / 1048576f);
		}
		return string.Format("{0:0.0}GB", bytes / 1.0737418E+09f);
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x000389EC File Offset: 0x00036BEC
	private void Clean()
	{
		this.fgLoading.SkipHide();
		this.fgContent.SkipHide();
		this.indicatorNew.SetActive(false);
		this.indicatorUpdate.SetActive(false);
		this.indicatorOwnershipWarning.SetActive(false);
		this.indicatorInvalidContent.SetActive(false);
		this.txtPublishedFileID.text = "-";
		this.txtPath.text = "-";
		this.fgButtonMain.SkipHide();
		this.fgProgressBar.SkipHide();
		this.fgSucceed.SkipHide();
		this.fgFailed.SkipHide();
		this.waitingForUpload = false;
	}

	// Token: 0x04000B98 RID: 2968
	[SerializeField]
	private FadeGroup fgMain;

	// Token: 0x04000B99 RID: 2969
	[SerializeField]
	private FadeGroup fgLoading;

	// Token: 0x04000B9A RID: 2970
	[SerializeField]
	private FadeGroup fgContent;

	// Token: 0x04000B9B RID: 2971
	[SerializeField]
	private TextMeshProUGUI txtTitle;

	// Token: 0x04000B9C RID: 2972
	[SerializeField]
	private TextMeshProUGUI txtDescription;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private RawImage preview;

	// Token: 0x04000B9E RID: 2974
	[SerializeField]
	private TextMeshProUGUI txtModName;

	// Token: 0x04000B9F RID: 2975
	[SerializeField]
	private TextMeshProUGUI txtPath;

	// Token: 0x04000BA0 RID: 2976
	[SerializeField]
	private TextMeshProUGUI txtPublishedFileID;

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private GameObject indicatorNew;

	// Token: 0x04000BA2 RID: 2978
	[SerializeField]
	private GameObject indicatorUpdate;

	// Token: 0x04000BA3 RID: 2979
	[SerializeField]
	private GameObject indicatorOwnershipWarning;

	// Token: 0x04000BA4 RID: 2980
	[SerializeField]
	private GameObject indicatorInvalidContent;

	// Token: 0x04000BA5 RID: 2981
	[SerializeField]
	private Button btnUpload;

	// Token: 0x04000BA6 RID: 2982
	[SerializeField]
	private Button btnCancel;

	// Token: 0x04000BA7 RID: 2983
	[SerializeField]
	private FadeGroup fgButtonMain;

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	private FadeGroup fgProgressBar;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private TextMeshProUGUI progressText;

	// Token: 0x04000BAA RID: 2986
	[SerializeField]
	private Image progressBarFill;

	// Token: 0x04000BAB RID: 2987
	[SerializeField]
	private FadeGroup fgSucceed;

	// Token: 0x04000BAC RID: 2988
	[SerializeField]
	private FadeGroup fgFailed;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private float closeAfterSeconds = 2f;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	private Texture2D defaultPreviewTexture;

	// Token: 0x04000BAF RID: 2991
	private bool cancelClicked;

	// Token: 0x04000BB0 RID: 2992
	private bool uploadClicked;

	// Token: 0x04000BB1 RID: 2993
	private bool waitingForUpload;
}
