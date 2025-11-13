using System;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace CameraSystems
{
	// Token: 0x02000214 RID: 532
	public class CameraPropertiesControl : MonoBehaviour
	{
		// Token: 0x06000FFE RID: 4094 RVA: 0x0003F238 File Offset: 0x0003D438
		private void Awake()
		{
			this.vCam = base.GetComponent<CinemachineVirtualCamera>();
			this.volumeSettings = base.GetComponent<CinemachineVolumeSettings>();
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0003F254 File Offset: 0x0003D454
		private unsafe void Update()
		{
			float num = *Gamepad.current.dpad.x.value;
			if (*Gamepad.current.dpad.y.value != 0f)
			{
				float num2 = -(*Gamepad.current.dpad.y.value);
				if (*Gamepad.current.rightShoulder.value > 0f)
				{
					num2 *= 10f;
				}
				this.vCam.m_Lens.FieldOfView = Mathf.Clamp(this.vCam.m_Lens.FieldOfView + num2 * 5f * Time.deltaTime, 8f, 100f);
			}
		}

		// Token: 0x04000CE2 RID: 3298
		private CinemachineVirtualCamera vCam;

		// Token: 0x04000CE3 RID: 3299
		private CinemachineVolumeSettings volumeSettings;

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		private VolumeProfile volumeProfile;
	}
}
