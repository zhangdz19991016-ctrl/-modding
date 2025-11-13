using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x0200027C RID: 636
	public class MiniMapCompass : MonoBehaviour
	{
		// Token: 0x0600143B RID: 5179 RVA: 0x0004B1C8 File Offset: 0x000493C8
		private void SetupRotation()
		{
			Vector3 from = LevelManager.Instance.GameCamera.mainVCam.transform.up.ProjectOntoPlane(Vector3.up);
			Vector3 forward = Vector3.forward;
			float num = Vector3.SignedAngle(from, forward, Vector3.up);
			this.arrow.localRotation = Quaternion.Euler(0f, 0f, -num);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004B226 File Offset: 0x00049426
		private void Update()
		{
			this.SetupRotation();
		}

		// Token: 0x04000ED9 RID: 3801
		[SerializeField]
		private Transform arrow;
	}
}
