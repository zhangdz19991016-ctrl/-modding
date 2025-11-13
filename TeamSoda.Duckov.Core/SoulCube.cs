using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class SoulCube : MonoBehaviour
{
	// Token: 0x060005EE RID: 1518 RVA: 0x0001A8AC File Offset: 0x00018AAC
	public void Init(SoulCollector collectorTarget)
	{
		this.target = collectorTarget;
		this.direction = UnityEngine.Random.insideUnitSphere + Vector3.up;
		this.direction.Normalize();
		this.spawnSpeed = UnityEngine.Random.Range(this.speedRange.x, this.speedRange.y);
		this.roatePart.transform.localRotation = Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 360f);
		this.rotateAxis = UnityEngine.Random.insideUnitSphere;
		this.rotateSpeed = UnityEngine.Random.Range(this.rotateSpeedRange.x, this.rotateSpeedRange.y);
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001A954 File Offset: 0x00018B54
	private void Update()
	{
		this.roatePart.Rotate(this.rotateSpeed * this.rotateAxis * Time.deltaTime);
		if (this.target == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.stateTimer += Time.deltaTime;
		SoulCube.States states = this.currentState;
		if (states != SoulCube.States.spawn)
		{
			if (states != SoulCube.States.goToTarget)
			{
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.target.transform.position, this.toTargetSpeed * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, this.target.transform.position) < 0.3f)
			{
				this.AddCube();
			}
		}
		else
		{
			this.velocity = this.spawnSpeed * this.direction * this.spawnSpeedCurve.Evaluate(Mathf.Clamp01(this.stateTimer / this.spawnTime));
			base.transform.position += this.velocity * Time.deltaTime;
			if (this.stateTimer > this.spawnTime)
			{
				this.currentState = SoulCube.States.goToTarget;
				return;
			}
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001AA9B File Offset: 0x00018C9B
	private void AddCube()
	{
		if (this.target)
		{
			this.target.AddCube();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400056B RID: 1387
	private SoulCube.States currentState;

	// Token: 0x0400056C RID: 1388
	private SoulCollector target;

	// Token: 0x0400056D RID: 1389
	private Vector3 direction;

	// Token: 0x0400056E RID: 1390
	private float stateTimer;

	// Token: 0x0400056F RID: 1391
	public Vector2 speedRange;

	// Token: 0x04000570 RID: 1392
	private float spawnSpeed;

	// Token: 0x04000571 RID: 1393
	public float spawnTime;

	// Token: 0x04000572 RID: 1394
	public float toTargetSpeed;

	// Token: 0x04000573 RID: 1395
	public AnimationCurve spawnSpeedCurve;

	// Token: 0x04000574 RID: 1396
	private Vector3 velocity;

	// Token: 0x04000575 RID: 1397
	public Transform roatePart;

	// Token: 0x04000576 RID: 1398
	public Vector2 rotateSpeedRange = new Vector2(300f, 1000f);

	// Token: 0x04000577 RID: 1399
	private float rotateSpeed;

	// Token: 0x04000578 RID: 1400
	private Vector3 rotateAxis;

	// Token: 0x02000465 RID: 1125
	private enum States
	{
		// Token: 0x04001B4F RID: 6991
		spawn,
		// Token: 0x04001B50 RID: 6992
		goToTarget
	}
}
