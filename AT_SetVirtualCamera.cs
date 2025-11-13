using System;
using Cinemachine;
using NodeCanvas.Framework;

// Token: 0x020001B5 RID: 437
public class AT_SetVirtualCamera : ActionTask
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00036B22 File Offset: 0x00034D22
	protected override string info
	{
		get
		{
			return "Set camera :" + ((this.target.value == null) ? "Empty" : this.target.value.name);
		}
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x00036B58 File Offset: 0x00034D58
	protected override void OnExecute()
	{
		base.OnExecute();
		if (AT_SetVirtualCamera.cachedVCam != null)
		{
			AT_SetVirtualCamera.cachedVCam.gameObject.SetActive(false);
		}
		if (this.target.value != null)
		{
			this.target.value.gameObject.SetActive(true);
			AT_SetVirtualCamera.cachedVCam = this.target.value;
		}
		else
		{
			AT_SetVirtualCamera.cachedVCam = null;
		}
		base.EndAction();
	}

	// Token: 0x04000B46 RID: 2886
	private static CinemachineVirtualCamera cachedVCam;

	// Token: 0x04000B47 RID: 2887
	public BBParameter<CinemachineVirtualCamera> target;
}
