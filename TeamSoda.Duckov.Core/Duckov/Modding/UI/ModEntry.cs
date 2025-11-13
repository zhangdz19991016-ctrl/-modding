using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x02000272 RID: 626
	public class ModEntry : MonoBehaviour
	{
		// Token: 0x060013B5 RID: 5045 RVA: 0x00049D90 File Offset: 0x00047F90
		private void Awake()
		{
			this.toggleButton.onClick.AddListener(new UnityAction(this.OnToggleButtonClicked));
			this.uploadButton.onClick.AddListener(new UnityAction(this.OnUploadButtonClicked));
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Combine(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
			this.failedIndicator.SetActive(false);
			this.btnReorderDown.onClick.AddListener(new UnityAction(this.OnButtonReorderDownClicked));
			this.btnReorderUp.onClick.AddListener(new UnityAction(this.OnButtonReorderUpClicked));
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00049E39 File Offset: 0x00048039
		private void OnButtonReorderUpClicked()
		{
			ModManager.Reorder(this.index, this.index - 1);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00049E4F File Offset: 0x0004804F
		private void OnButtonReorderDownClicked()
		{
			ModManager.Reorder(this.index, this.index + 1);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00049E65 File Offset: 0x00048065
		private void OnDestroy()
		{
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Remove(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00049E87 File Offset: 0x00048087
		private void OnModLoadingFailed(string dllPath, string message)
		{
			if (dllPath != this.info.dllPath)
			{
				return;
			}
			Debug.LogError(message);
			this.failedIndicator.SetActive(true);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00049EAF File Offset: 0x000480AF
		private void OnUploadButtonClicked()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.BeginUpload(this.info).Forget();
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00049ED8 File Offset: 0x000480D8
		private void OnToggleButtonClicked()
		{
			if (ModManager.Instance == null)
			{
				Debug.LogError("ModManager.Instance Not Found");
				return;
			}
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			if (flag && flag2)
			{
				ModManager.Instance.DeactivateMod(this.info);
				return;
			}
			ModManager.Instance.ActivateMod(this.info);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00049F5C File Offset: 0x0004815C
		private void OnEnable()
		{
			ModManager.OnModStatusChanged += this.OnModStatusChanged;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00049F6F File Offset: 0x0004816F
		private void OnDisable()
		{
			ModManager.OnModStatusChanged -= this.OnModStatusChanged;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00049F82 File Offset: 0x00048182
		private void OnModStatusChanged()
		{
			this.RefreshStatus();
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00049F8C File Offset: 0x0004818C
		private void RefreshStatus()
		{
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			bool active = flag && !flag2;
			this.activeIndicator.SetActive(flag2);
			this.nameCollisionIndicator.SetActive(active);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00049FF4 File Offset: 0x000481F4
		private void RefreshInfo()
		{
			this.textTitle.text = this.info.displayName;
			this.textName.text = this.info.name;
			this.textDescription.text = this.info.description;
			this.preview.texture = this.info.preview;
			this.steamItemIndicator.SetActive(this.info.isSteamItem);
			this.notSteamItemIndicator.SetActive(!this.info.isSteamItem);
			bool flag = SteamWorkshopManager.IsOwner(this.info);
			this.steamItemOwnerIndicator.SetActive(flag);
			bool active = flag || !this.info.isSteamItem;
			this.uploadButton.gameObject.SetActive(active);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004A0C6 File Offset: 0x000482C6
		public void Setup(ModManagerUI master, ModInfo modInfo, int index)
		{
			this.master = master;
			this.info = modInfo;
			this.index = index;
			this.RefreshInfo();
			this.RefreshStatus();
		}

		// Token: 0x04000E99 RID: 3737
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x04000E9A RID: 3738
		[SerializeField]
		private TextMeshProUGUI textName;

		// Token: 0x04000E9B RID: 3739
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x04000E9C RID: 3740
		[SerializeField]
		private RawImage preview;

		// Token: 0x04000E9D RID: 3741
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x04000E9E RID: 3742
		[SerializeField]
		private GameObject nameCollisionIndicator;

		// Token: 0x04000E9F RID: 3743
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04000EA0 RID: 3744
		[SerializeField]
		private GameObject steamItemIndicator;

		// Token: 0x04000EA1 RID: 3745
		[SerializeField]
		private GameObject steamItemOwnerIndicator;

		// Token: 0x04000EA2 RID: 3746
		[SerializeField]
		private GameObject notSteamItemIndicator;

		// Token: 0x04000EA3 RID: 3747
		[SerializeField]
		private Button uploadButton;

		// Token: 0x04000EA4 RID: 3748
		[SerializeField]
		private GameObject failedIndicator;

		// Token: 0x04000EA5 RID: 3749
		[SerializeField]
		private Button btnReorderUp;

		// Token: 0x04000EA6 RID: 3750
		[SerializeField]
		private Button btnReorderDown;

		// Token: 0x04000EA7 RID: 3751
		[SerializeField]
		private int index;

		// Token: 0x04000EA8 RID: 3752
		private ModManagerUI master;

		// Token: 0x04000EA9 RID: 3753
		private ModInfo info;
	}
}
