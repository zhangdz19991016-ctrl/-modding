using System;
using Duckov.Utilities;
using Saves;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class CustomFaceManager : MonoBehaviour
{
	// Token: 0x0600088F RID: 2191 RVA: 0x000266FD File Offset: 0x000248FD
	public void SaveSettingToMainCharacter(CustomFaceSettingData setting)
	{
		this.SaveSetting("CustomFace_MainCharacter", setting);
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002670B File Offset: 0x0002490B
	public CustomFaceSettingData LoadMainCharacterSetting()
	{
		return this.LoadSetting("CustomFace_MainCharacter");
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00026718 File Offset: 0x00024918
	private void SaveSetting(string key, CustomFaceSettingData setting)
	{
		setting.savedSetting = true;
		SavesSystem.Save<CustomFaceSettingData>(key, setting);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002672C File Offset: 0x0002492C
	private CustomFaceSettingData LoadSetting(string key)
	{
		CustomFaceSettingData customFaceSettingData = SavesSystem.Load<CustomFaceSettingData>(key);
		if (!customFaceSettingData.savedSetting)
		{
			customFaceSettingData = GameplayDataSettings.CustomFaceData.DefaultPreset.settings;
		}
		return customFaceSettingData;
	}
}
