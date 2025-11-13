using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001B8 RID: 440
public class DevCam : MonoBehaviour
{
	// Token: 0x06000D15 RID: 3349 RVA: 0x00036D51 File Offset: 0x00034F51
	private void Awake()
	{
		this.root.gameObject.SetActive(false);
		Shader.SetGlobalFloat("DevCamOn", 0f);
		DevCam.devCamOn = false;
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00036D7C File Offset: 0x00034F7C
	private void Toggle()
	{
		this.active = true;
		DevCam.devCamOn = this.active;
		Shader.SetGlobalFloat("DevCamOn", this.active ? 1f : 0f);
		this.root.gameObject.SetActive(this.active);
		for (int i = 0; i < Display.displays.Length; i++)
		{
			if (i == 1 && this.active)
			{
				Display.displays[i].Activate();
			}
		}
		UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = 500f;
		}
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00036E18 File Offset: 0x00035018
	private void OnDestroy()
	{
		DevCam.devCamOn = false;
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00036E20 File Offset: 0x00035020
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (Gamepad.all.Count <= 0)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f)
		{
			this.timer = 0f;
			this.pressCounter = 0;
		}
		if (Gamepad.current.leftStickButton.isPressed && Gamepad.current.rightStickButton.wasPressedThisFrame)
		{
			this.pressCounter++;
			this.timer = 1.5f;
			Debug.Log("Toggle Dev Cam");
			if (this.pressCounter >= 2)
			{
				this.pressCounter = 0;
				this.Toggle();
			}
		}
		if (CharacterMainControl.Main != null)
		{
			this.postTarget.position = CharacterMainControl.Main.transform.position;
		}
	}

	// Token: 0x04000B51 RID: 2897
	public Camera devCamera;

	// Token: 0x04000B52 RID: 2898
	public Transform postTarget;

	// Token: 0x04000B53 RID: 2899
	private bool active;

	// Token: 0x04000B54 RID: 2900
	public Transform root;

	// Token: 0x04000B55 RID: 2901
	public static bool devCamOn;

	// Token: 0x04000B56 RID: 2902
	private float timer = 1.5f;

	// Token: 0x04000B57 RID: 2903
	private int pressCounter;
}
