using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class MainCharacterFace : MonoBehaviour
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x00031B60 File Offset: 0x0002FD60
	private void Start()
	{
		CustomFaceSettingData saveData = this.customFaceManager.LoadMainCharacterSetting();
		this.customFace.LoadFromData(saveData);
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00031B85 File Offset: 0x0002FD85
	private void Update()
	{
	}

	// Token: 0x040009F9 RID: 2553
	public CustomFaceManager customFaceManager;

	// Token: 0x040009FA RID: 2554
	public CustomFaceInstance customFace;
}
