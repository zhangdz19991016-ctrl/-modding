using System;
using Duckov.Aquariums;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class DummyFish : MonoBehaviour, IAquariumContent
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00033BFE File Offset: 0x00031DFE
	private Vector3 TargetPosition
	{
		get
		{
			return this.target.position;
		}
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00033C0B File Offset: 0x00031E0B
	private void Awake()
	{
		this.rigidbody.useGravity = false;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00033C19 File Offset: 0x00031E19
	public void Setup(Aquarium master)
	{
		this.master = master;
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00033C24 File Offset: 0x00031E24
	private void FixedUpdate()
	{
		Vector3 up = Vector3.up;
		Vector3 forward = base.transform.forward;
		Vector3 right = base.transform.right;
		Vector3 vector = this.TargetPosition - this.rigidbody.position;
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = Vector3.Cross(up, normalized);
		float b = Vector3.Dot(normalized, forward);
		float num = Mathf.Max(0f, b);
		this.swim = ((vector.magnitude > this.deadZone) ? 1f : (vector.magnitude / this.deadZone)) * num;
		Vector3 a = -(Vector3.Dot(vector2, this.rigidbody.velocity) * vector2);
		this.rigidbody.velocity += forward * this.swimForce * this.swim * Time.deltaTime + a * 0.5f;
		this.rigidbody.angularVelocity = Vector3.zero;
		Vector3 vector3 = vector;
		vector3.y = 0f;
		float num2 = Mathf.Clamp01(vector3.magnitude / this.deadZone - 0.5f);
		Vector3 normalized2 = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
		this._debug_projectedForward = normalized2;
		Vector3 vector4 = Vector3.Lerp(normalized2, normalized, num2);
		this._debug_idealRotForward = vector4;
		float num3 = Vector3.SignedAngle(forward, vector4, right);
		float num4 = Vector3.SignedAngle(forward, vector4, Vector3.up);
		float num5 = this.rotateForce * num3;
		float num6 = this.rotateForce * num4;
		this.rotVelocityX += num5 * Time.fixedDeltaTime;
		this.rotVelocityY += num6 * Time.fixedDeltaTime * num2;
		this.rotVelocityX *= 1f - this.rotationDamping;
		this.rotVelocityY *= 1f - this.rotationDamping;
		Vector3 eulerAngles = this.rigidbody.rotation.eulerAngles;
		eulerAngles.y += this.rotVelocityY * Time.deltaTime;
		eulerAngles.x += this.rotVelocityX * Time.deltaTime;
		if (eulerAngles.x < -179f)
		{
			eulerAngles.x += 360f;
		}
		if (eulerAngles.x > 179f)
		{
			eulerAngles.x -= 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -45f, 45f);
		eulerAngles.z = 0f;
		Quaternion rot = Quaternion.Euler(eulerAngles);
		this.rigidbody.MoveRotation(rot);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00033EDC File Offset: 0x000320DC
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_idealRotForward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_projectedForward);
	}

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	private float rotateForce = 10f;

	// Token: 0x04000A98 RID: 2712
	[SerializeField]
	private float swimForce = 10f;

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private float deadZone = 2f;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private float rotationDamping = 0.1f;

	// Token: 0x04000A9B RID: 2715
	[Header("Control")]
	[SerializeField]
	private Transform target;

	// Token: 0x04000A9C RID: 2716
	[Range(0f, 1f)]
	[SerializeField]
	private float swim;

	// Token: 0x04000A9D RID: 2717
	private float rotVelocityX;

	// Token: 0x04000A9E RID: 2718
	private float rotVelocityY;

	// Token: 0x04000A9F RID: 2719
	private Aquarium master;

	// Token: 0x04000AA0 RID: 2720
	private Vector3 _debug_idealRotForward;

	// Token: 0x04000AA1 RID: 2721
	private Vector3 _debug_projectedForward;
}
