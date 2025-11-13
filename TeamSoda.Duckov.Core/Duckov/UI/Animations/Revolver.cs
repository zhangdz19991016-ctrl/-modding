using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E6 RID: 998
	public class Revolver : MonoBehaviour
	{
		// Token: 0x06002455 RID: 9301 RVA: 0x0007F074 File Offset: 0x0007D274
		private void Update()
		{
			Quaternion rotation = Quaternion.AngleAxis(Time.deltaTime * this.rPM / 60f * 360f, this.axis);
			Vector3 point = base.transform.localPosition - this.pivot;
			Vector3 b = rotation * point;
			Vector3 localPosition = this.pivot + b;
			base.transform.localPosition = localPosition;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0007F0DC File Offset: 0x0007D2DC
		private void OnDrawGizmosSelected()
		{
			if (base.transform.parent != null)
			{
				Gizmos.matrix = base.transform.parent.localToWorldMatrix;
			}
			Gizmos.DrawLine(this.pivot, base.transform.localPosition);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.pivot, 1f);
		}

		// Token: 0x0400189F RID: 6303
		public Vector3 pivot;

		// Token: 0x040018A0 RID: 6304
		public Vector3 axis = Vector3.forward;

		// Token: 0x040018A1 RID: 6305
		public float rPM;
	}
}
