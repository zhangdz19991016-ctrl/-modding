using System;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class MultiInteractionMenu : MonoBehaviour
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0003C136 File Offset: 0x0003A336
	// (set) Token: 0x06000EFA RID: 3834 RVA: 0x0003C13D File Offset: 0x0003A33D
	public static MultiInteractionMenu Instance { get; private set; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0003C148 File Offset: 0x0003A348
	private PrefabPool<MultiInteractionMenuButton> ButtonPool
	{
		get
		{
			if (this._buttonPool == null)
			{
				this._buttonPool = new PrefabPool<MultiInteractionMenuButton>(this.buttonTemplate, this.buttonTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				this.buttonTemplate.gameObject.SetActive(false);
			}
			return this._buttonPool;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0003C1A1 File Offset: 0x0003A3A1
	public MultiInteraction Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0003C1A9 File Offset: 0x0003A3A9
	private void Awake()
	{
		if (MultiInteractionMenu.Instance == null)
		{
			MultiInteractionMenu.Instance = this;
		}
		this.buttonTemplate.gameObject.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0003C1DC File Offset: 0x0003A3DC
	private void Setup(MultiInteraction target)
	{
		this.target = target;
		ReadOnlyCollection<InteractableBase> interactables = target.Interactables;
		this.ButtonPool.ReleaseAll();
		foreach (InteractableBase x in interactables)
		{
			if (!(x == null))
			{
				MultiInteractionMenuButton multiInteractionMenuButton = this.ButtonPool.Get(null);
				multiInteractionMenuButton.Setup(x);
				multiInteractionMenuButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0003C25C File Offset: 0x0003A45C
	private int CreateNewToken()
	{
		this.currentTaskToken = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		return this.currentTaskToken;
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0003C279 File Offset: 0x0003A479
	private bool TokenChanged(int token)
	{
		return token != this.currentTaskToken;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0003C288 File Offset: 0x0003A488
	public UniTask SetupAndShow(MultiInteraction target)
	{
		MultiInteractionMenu.<SetupAndShow>d__17 <SetupAndShow>d__;
		<SetupAndShow>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SetupAndShow>d__.<>4__this = this;
		<SetupAndShow>d__.target = target;
		<SetupAndShow>d__.<>1__state = -1;
		<SetupAndShow>d__.<>t__builder.Start<MultiInteractionMenu.<SetupAndShow>d__17>(ref <SetupAndShow>d__);
		return <SetupAndShow>d__.<>t__builder.Task;
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0003C2D4 File Offset: 0x0003A4D4
	public UniTask Hide()
	{
		MultiInteractionMenu.<Hide>d__18 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<MultiInteractionMenu.<Hide>d__18>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x04000C68 RID: 3176
	[SerializeField]
	private MultiInteractionMenuButton buttonTemplate;

	// Token: 0x04000C69 RID: 3177
	[SerializeField]
	private float delayEachButton = 0.25f;

	// Token: 0x04000C6A RID: 3178
	private PrefabPool<MultiInteractionMenuButton> _buttonPool;

	// Token: 0x04000C6B RID: 3179
	private MultiInteraction target;

	// Token: 0x04000C6C RID: 3180
	private int currentTaskToken;
}
