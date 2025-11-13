using System;
using Duckov.Options;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class ResolutionOptions : OptionsProviderBase
{
	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00038C72 File Offset: 0x00036E72
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_Resolution;
		}
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x00038C7C File Offset: 0x00036E7C
	public override string GetCurrentOption()
	{
		return OptionsManager.Load<DuckovResolution>(this.Key, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1])).ToString();
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x00038CBC File Offset: 0x00036EBC
	public override string[] GetOptions()
	{
		this.avaliableResolutions = ResolutionSetter.GetResolutions();
		string[] array = new string[this.avaliableResolutions.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.avaliableResolutions[i].ToString();
		}
		return array;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00038D0C File Offset: 0x00036F0C
	public override void Set(int index)
	{
		if (this.avaliableResolutions == null || index >= this.avaliableResolutions.Length)
		{
			Debug.Log("设置分辨率流程错误");
			index = 0;
		}
		DuckovResolution obj = this.avaliableResolutions[index];
		OptionsManager.Save<DuckovResolution>(this.Key, obj);
	}

	// Token: 0x04000BB3 RID: 2995
	private DuckovResolution[] avaliableResolutions;
}
