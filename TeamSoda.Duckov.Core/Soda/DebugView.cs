using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Soda
{
	// Token: 0x02000229 RID: 553
	public class DebugView : MonoBehaviour
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00041CD9 File Offset: 0x0003FED9
		public DebugView Instance
		{
			get
			{
				return this.instance;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x00041CE1 File Offset: 0x0003FEE1
		public bool EdgeLightActive
		{
			get
			{
				return this.edgeLightActive;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x060010E9 RID: 4329 RVA: 0x00041CEC File Offset: 0x0003FEEC
		// (remove) Token: 0x060010EA RID: 4330 RVA: 0x00041D20 File Offset: 0x0003FF20
		public static event Action<DebugView> OnDebugViewConfigChanged;

		// Token: 0x060010EB RID: 4331 RVA: 0x00041D53 File Offset: 0x0003FF53
		private void Awake()
		{
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00041D55 File Offset: 0x0003FF55
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnlevelInited;
			SceneManager.activeSceneChanged -= this.OnSceneLoaded;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00041D7C File Offset: 0x0003FF7C
		private void InitFromData()
		{
			if (PlayerPrefs.HasKey("ResMode"))
			{
				this.resMode = (ResModes)PlayerPrefs.GetInt("ResMode");
			}
			else
			{
				this.resMode = ResModes.R720p;
			}
			if (PlayerPrefs.HasKey("TexMode"))
			{
				this.texMode = (TextureModes)PlayerPrefs.GetInt("TexMode");
			}
			else
			{
				this.texMode = TextureModes.High;
			}
			if (PlayerPrefs.HasKey("InputDevice"))
			{
				this.inputDevice = PlayerPrefs.GetInt("InputDevice");
			}
			else
			{
				this.inputDevice = 1;
			}
			if (PlayerPrefs.HasKey("BloomActive"))
			{
				this.bloomActive = (PlayerPrefs.GetInt("BloomActive") != 0);
			}
			else
			{
				this.bloomActive = true;
			}
			if (PlayerPrefs.HasKey("EdgeLightActive"))
			{
				this.edgeLightActive = (PlayerPrefs.GetInt("EdgeLightActive") != 0);
			}
			else
			{
				this.edgeLightActive = true;
			}
			if (PlayerPrefs.HasKey("AOActive"))
			{
				this.aoActive = (PlayerPrefs.GetInt("AOActive") != 0);
			}
			else
			{
				this.aoActive = false;
			}
			if (PlayerPrefs.HasKey("DofActive"))
			{
				this.dofActive = (PlayerPrefs.GetInt("DofActive") != 0);
			}
			else
			{
				this.dofActive = false;
			}
			if (PlayerPrefs.HasKey("ReporterActive"))
			{
				this.reporterActive = (PlayerPrefs.GetInt("ReporterActive") != 0);
				return;
			}
			this.reporterActive = false;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00041EC0 File Offset: 0x000400C0
		private void Update()
		{
			this.deltaTimes[this.frameIndex] = Time.deltaTime;
			this.frameIndex++;
			if (this.frameIndex >= this.frameSampleCount)
			{
				this.frameIndex = 0;
				float num = 0f;
				for (int i = 0; i < this.frameSampleCount; i++)
				{
					num += this.deltaTimes[i];
				}
				int num2 = Mathf.RoundToInt((float)this.frameSampleCount / Mathf.Max(0.0001f, num));
				this.fpsText1.text = num2.ToString();
				this.fpsText2.text = num2.ToString();
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00041F64 File Offset: 0x00040164
		public void SetInputDevice(int type)
		{
			if (!true)
			{
				InputManager.SetInputDevice(InputManager.InputDevices.touch);
				this.inputDeviceText.text = "触摸";
				PlayerPrefs.SetInt("InputDevice", 0);
				return;
			}
			InputManager.SetInputDevice(InputManager.InputDevices.mouseKeyboard);
			this.inputDeviceText.text = "键鼠";
			PlayerPrefs.SetInt("InputDevice", 1);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00041FBA File Offset: 0x000401BA
		public void SetRes(int resModeIndex)
		{
			this.SetRes((ResModes)resModeIndex);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00041FC4 File Offset: 0x000401C4
		public void SetRes(ResModes mode)
		{
			this.resMode = mode;
			this.screenRes.x = (float)Display.main.systemWidth;
			this.screenRes.y = (float)Display.main.systemHeight;
			PlayerPrefs.SetInt("ResMode", (int)mode);
			int num = 1;
			int num2 = 1;
			switch (this.resMode)
			{
			case ResModes.Source:
				num = Mathf.RoundToInt(this.screenRes.x);
				num2 = Mathf.RoundToInt(this.screenRes.y);
				break;
			case ResModes.HalfRes:
				num = Mathf.RoundToInt(this.screenRes.x / 2f);
				num2 = Mathf.RoundToInt(this.screenRes.y / 2f);
				break;
			case ResModes.R720p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 720f);
				num2 = 720;
				break;
			case ResModes.R480p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 480f);
				num2 = 480;
				break;
			}
			this.resText.text = string.Format("{0}x{1}", num, num2);
			Screen.SetResolution(num, num2, FullScreenMode.FullScreenWindow);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00042119 File Offset: 0x00040319
		public void SetTexture(int texModeIndex)
		{
			this.SetTexture((TextureModes)texModeIndex);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00042124 File Offset: 0x00040324
		public void SetTexture(TextureModes mode)
		{
			this.texMode = mode;
			QualitySettings.globalTextureMipmapLimit = (int)this.texMode;
			switch (this.texMode)
			{
			case TextureModes.High:
				this.texText.text = "高";
				break;
			case TextureModes.Middle:
				this.texText.text = "中";
				break;
			case TextureModes.Low:
				this.texText.text = "低";
				break;
			case TextureModes.VeryLow:
				this.texText.text = "极低";
				break;
			}
			PlayerPrefs.SetInt("TexMode", (int)this.texMode);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000421C8 File Offset: 0x000403C8
		private void OnlevelInited()
		{
			this.SetInvincible(this.invincible);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000421D8 File Offset: 0x000403D8
		private void OnSceneLoaded(Scene s1, Scene s2)
		{
			this.SetShadow().Forget();
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000421F4 File Offset: 0x000403F4
		private UniTaskVoid SetShadow()
		{
			DebugView.<SetShadow>d__49 <SetShadow>d__;
			<SetShadow>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SetShadow>d__.<>4__this = this;
			<SetShadow>d__.<>1__state = -1;
			<SetShadow>d__.<>t__builder.Start<DebugView.<SetShadow>d__49>(ref <SetShadow>d__);
			return <SetShadow>d__.<>t__builder.Task;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00042237 File Offset: 0x00040437
		public void ToggleBloom()
		{
			this.bloomActive = !this.bloomActive;
			this.SetBloom(this.bloomActive);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00042254 File Offset: 0x00040454
		private void SetBloom(bool active)
		{
			Bloom bloom;
			bool flag = this.volumeProfile.TryGet<Bloom>(out bloom);
			this.bloomText.text = (active ? "开" : "关");
			if (flag)
			{
				bloom.active = active;
			}
			this.bloomActive = active;
			PlayerPrefs.SetInt("BloomActive", this.bloomActive ? 1 : 0);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000422BE File Offset: 0x000404BE
		public void ToggleEdgeLight()
		{
			this.edgeLightActive = !this.edgeLightActive;
			this.SetEdgeLight(this.edgeLightActive);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000422DC File Offset: 0x000404DC
		private void SetEdgeLight(bool active)
		{
			this.edgeLightText.text = (active ? "开" : "关");
			this.edgeLightActive = active;
			PlayerPrefs.SetInt("EdgeLightActive", this.edgeLightActive ? 1 : 0);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.supportsCameraDepthTexture = active;
			}
			this.SetShadow();
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00042358 File Offset: 0x00040558
		public void ToggleAO()
		{
			this.aoActive = !this.aoActive;
			this.SetAO(this.aoActive);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00042375 File Offset: 0x00040575
		public void ToggleDof()
		{
			this.dofActive = !this.dofActive;
			this.SetDof(this.dofActive);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00042392 File Offset: 0x00040592
		public void ToggleInvincible()
		{
			this.invincible = !this.invincible;
			this.SetInvincible(this.invincible);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000423AF File Offset: 0x000405AF
		private void SetReporter(bool active)
		{
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000423B1 File Offset: 0x000405B1
		public void ToggleReporter()
		{
			this.SetReporter(!this.reporterActive);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000423C4 File Offset: 0x000405C4
		private void SetAO(bool active)
		{
			ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature a) => a.name == "ScreenSpaceAmbientOcclusion");
			if (scriptableRendererFeature != null)
			{
				scriptableRendererFeature.SetActive(active);
				this.aoText.text = (active ? "开" : "关");
				PlayerPrefs.SetInt("AOActive", active ? 1 : 0);
			}
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0004244C File Offset: 0x0004064C
		private void SetDof(bool active)
		{
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0004244E File Offset: 0x0004064E
		private void SetInvincible(bool active)
		{
			this.invincibleText.text = (active ? "开" : "关");
			this.invincible = active;
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00042484 File Offset: 0x00040684
		public void CreateItem()
		{
			this.CreateItemTask().Forget();
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000424A0 File Offset: 0x000406A0
		private UniTaskVoid CreateItemTask()
		{
			DebugView.<CreateItemTask>d__63 <CreateItemTask>d__;
			<CreateItemTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<CreateItemTask>d__.<>4__this = this;
			<CreateItemTask>d__.<>1__state = -1;
			<CreateItemTask>d__.<>t__builder.Start<DebugView.<CreateItemTask>d__63>(ref <CreateItemTask>d__);
			return <CreateItemTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000D54 RID: 3412
		private DebugView instance;

		// Token: 0x04000D55 RID: 3413
		private Vector2 screenRes;

		// Token: 0x04000D56 RID: 3414
		private ResModes resMode;

		// Token: 0x04000D57 RID: 3415
		private TextureModes texMode;

		// Token: 0x04000D58 RID: 3416
		public TextMeshProUGUI resText;

		// Token: 0x04000D59 RID: 3417
		public TextMeshProUGUI texText;

		// Token: 0x04000D5A RID: 3418
		public TextMeshProUGUI fpsText1;

		// Token: 0x04000D5B RID: 3419
		public TextMeshProUGUI fpsText2;

		// Token: 0x04000D5C RID: 3420
		public TextMeshProUGUI inputDeviceText;

		// Token: 0x04000D5D RID: 3421
		public TextMeshProUGUI bloomText;

		// Token: 0x04000D5E RID: 3422
		public TextMeshProUGUI edgeLightText;

		// Token: 0x04000D5F RID: 3423
		public TextMeshProUGUI aoText;

		// Token: 0x04000D60 RID: 3424
		public TextMeshProUGUI dofText;

		// Token: 0x04000D61 RID: 3425
		public TextMeshProUGUI invincibleText;

		// Token: 0x04000D62 RID: 3426
		public TextMeshProUGUI reporterText;

		// Token: 0x04000D63 RID: 3427
		public UniversalRendererData rendererData;

		// Token: 0x04000D64 RID: 3428
		private float[] deltaTimes;

		// Token: 0x04000D65 RID: 3429
		private int frameIndex;

		// Token: 0x04000D66 RID: 3430
		public int frameSampleCount = 30;

		// Token: 0x04000D67 RID: 3431
		public GameObject openButton;

		// Token: 0x04000D68 RID: 3432
		public GameObject panel;

		// Token: 0x04000D69 RID: 3433
		public VolumeProfile volumeProfile;

		// Token: 0x04000D6A RID: 3434
		private bool bloomActive;

		// Token: 0x04000D6B RID: 3435
		private bool edgeLightActive;

		// Token: 0x04000D6C RID: 3436
		private bool aoActive;

		// Token: 0x04000D6D RID: 3437
		private int inputDevice;

		// Token: 0x04000D6E RID: 3438
		private bool dofActive;

		// Token: 0x04000D6F RID: 3439
		private bool invincible;

		// Token: 0x04000D70 RID: 3440
		private bool reporterActive;

		// Token: 0x04000D71 RID: 3441
		private Light light;

		// Token: 0x04000D72 RID: 3442
		[ItemTypeID]
		public int createItemID;
	}
}
