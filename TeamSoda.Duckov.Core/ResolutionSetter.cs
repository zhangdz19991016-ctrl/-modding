using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Options;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class ResolutionSetter : MonoBehaviour
{
	// Token: 0x06000DC2 RID: 3522 RVA: 0x00038D5C File Offset: 0x00036F5C
	private void Test()
	{
		this.debugDisplayRes = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
		this.debugmMaxRes = new Vector2Int(ResolutionSetter.MaxResolution.width, ResolutionSetter.MaxResolution.height);
		this.debugScreenRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
		this.testRes = ResolutionSetter.GetResolutions();
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00038DD8 File Offset: 0x00036FD8
	public static DuckovResolution MaxResolution
	{
		get
		{
			Resolution[] resolutions = Screen.resolutions;
			resolutions.Sort(delegate(Resolution A, Resolution B)
			{
				if (A.height > B.height)
				{
					return -1;
				}
				if (A.height < B.height)
				{
					return 1;
				}
				if (A.width > B.width)
				{
					return -1;
				}
				if (A.width < B.width)
				{
					return 1;
				}
				return 0;
			});
			Resolution res = default(Resolution);
			res.width = Screen.currentResolution.width;
			res.height = Screen.currentResolution.height;
			Resolution res2 = Screen.resolutions[resolutions.Length - 1];
			DuckovResolution duckovResolution;
			if (res.width > res2.width)
			{
				duckovResolution = new DuckovResolution(res);
			}
			else
			{
				duckovResolution = new DuckovResolution(res2);
			}
			if ((float)duckovResolution.width / (float)duckovResolution.height < 1.4f)
			{
				duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
			}
			return duckovResolution;
		}
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00038EA4 File Offset: 0x000370A4
	public static Resolution GetResByHeight(int height, DuckovResolution maxRes)
	{
		return new Resolution
		{
			height = height,
			width = (int)((float)maxRes.width * (float)height / (float)maxRes.height)
		};
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00038EDC File Offset: 0x000370DC
	public static DuckovResolution[] GetResolutions()
	{
		DuckovResolution maxResolution = ResolutionSetter.MaxResolution;
		List<Resolution> list = Screen.resolutions.ToList<Resolution>();
		list.Add(ResolutionSetter.GetResByHeight(1080, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(900, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(720, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(540, maxResolution));
		List<DuckovResolution> list2 = new List<DuckovResolution>();
		bool flag = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Window) != ResolutionSetter.screenModes.Window;
		foreach (Resolution res in list)
		{
			DuckovResolution duckovResolution = new DuckovResolution(res);
			if (!list2.Contains(duckovResolution) && (float)duckovResolution.width / (float)duckovResolution.height >= 1.4f && (!flag || duckovResolution.CheckRotioFit(duckovResolution, maxResolution)))
			{
				list2.Add(duckovResolution);
			}
		}
		list2.Sort(delegate(DuckovResolution A, DuckovResolution B)
		{
			if (A.height > B.height)
			{
				return -1;
			}
			if (A.height < B.height)
			{
				return 1;
			}
			if (A.width > B.width)
			{
				return -1;
			}
			if (A.width < B.width)
			{
				return 1;
			}
			return 0;
		});
		return list2.ToArray();
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00038FFC File Offset: 0x000371FC
	private void Update()
	{
		this.UpdateFullScreenCheck();
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00039004 File Offset: 0x00037204
	private void UpdateFullScreenCheck()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer -= Time.unscaledDeltaTime;
		if (ResolutionSetter.fullScreenChangeCheckCoolTimer > 0f)
		{
			return;
		}
		if (ResolutionSetter.currentFullScreen != (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen))
		{
			ResolutionSetter.currentFullScreen = !ResolutionSetter.currentFullScreen;
			OptionsManager.Save<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.currentFullScreen ? ResolutionSetter.screenModes.Borderless : ResolutionSetter.screenModes.Window);
			ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		}
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00039074 File Offset: 0x00037274
	public static void UpdateResolutionAndScreenMode()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		DuckovResolution duckovResolution = OptionsManager.Load<DuckovResolution>(ResolutionSetter.Key_Resolution, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1]));
		if ((float)duckovResolution.width / (float)duckovResolution.height < 1.3666667f)
		{
			duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
		}
		ResolutionSetter.screenModes screenModes = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Borderless);
		ResolutionSetter.currentFullScreen = (screenModes == ResolutionSetter.screenModes.Borderless);
		Screen.SetResolution(duckovResolution.width, duckovResolution.height, ResolutionSetter.ScreenModeToFullScreenMode(screenModes));
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00039109 File Offset: 0x00037309
	private static FullScreenMode ScreenModeToFullScreenMode(ResolutionSetter.screenModes screenMode)
	{
		if (screenMode == ResolutionSetter.screenModes.Borderless)
		{
			return FullScreenMode.FullScreenWindow;
		}
		if (screenMode != ResolutionSetter.screenModes.Window)
		{
			return FullScreenMode.ExclusiveFullScreen;
		}
		return FullScreenMode.Windowed;
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x0003911C File Offset: 0x0003731C
	public static string[] GetScreenModes()
	{
		return new string[]
		{
			("Option_ScreenMode_" + ResolutionSetter.screenModes.Borderless.ToString()).ToPlainText(),
			("Option_ScreenMode_" + ResolutionSetter.screenModes.Window.ToString()).ToPlainText()
		};
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00039171 File Offset: 0x00037371
	public static string ScreenModeToName(ResolutionSetter.screenModes mode)
	{
		return ("Option_ScreenMode_" + mode.ToString()).ToPlainText();
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0003918F File Offset: 0x0003738F
	private void Awake()
	{
		ResolutionSetter.UpdateResolutionAndScreenMode();
		OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x000391A7 File Offset: 0x000373A7
	private void OnDestroy()
	{
		OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x000391BA File Offset: 0x000373BA
	private void OnOptionsChanged(string key)
	{
		if (key == ResolutionSetter.Key_Resolution || key == ResolutionSetter.Key_ScreenMode)
		{
			ResolutionSetter.UpdateResolutionAndScreenMode();
		}
	}

	// Token: 0x04000BB4 RID: 2996
	public static string Key_Resolution = "Resolution";

	// Token: 0x04000BB5 RID: 2997
	public static string Key_ScreenMode = "ScreenMode";

	// Token: 0x04000BB6 RID: 2998
	public static bool currentFullScreen = false;

	// Token: 0x04000BB7 RID: 2999
	private static float fullScreenChangeCheckCoolTimer = 1f;

	// Token: 0x04000BB8 RID: 3000
	private static float fullScreenChangeCheckCoolTime = 1f;

	// Token: 0x04000BB9 RID: 3001
	public Vector2Int debugDisplayRes = new Vector2Int(0, 0);

	// Token: 0x04000BBA RID: 3002
	public Vector2Int debugScreenRes = new Vector2Int(0, 0);

	// Token: 0x04000BBB RID: 3003
	public Vector2Int debugmMaxRes = new Vector2Int(0, 0);

	// Token: 0x04000BBC RID: 3004
	public DuckovResolution[] testRes;

	// Token: 0x020004E0 RID: 1248
	public enum screenModes
	{
		// Token: 0x04001D44 RID: 7492
		Borderless,
		// Token: 0x04001D45 RID: 7493
		Window
	}
}
