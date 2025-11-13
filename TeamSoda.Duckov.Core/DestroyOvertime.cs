using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class DestroyOvertime : MonoBehaviour
{
	// Token: 0x06000A41 RID: 2625 RVA: 0x0002C575 File Offset: 0x0002A775
	private void Awake()
	{
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0002C58F File Offset: 0x0002A78F
	private void Update()
	{
		this.life -= Time.deltaTime;
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0002C5BB File Offset: 0x0002A7BB
	private void OnValidate()
	{
		this.ProcessParticleSystem();
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0002C5C4 File Offset: 0x0002A7C4
	private void ProcessParticleSystem()
	{
		float num = 0f;
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		if (!component)
		{
			return;
		}
		if (component != null)
		{
			ParticleSystem.MainModule main = component.main;
			main.stopAction = ParticleSystemStopAction.None;
			if (main.startLifetime.constant > num)
			{
				num = main.startLifetime.constant;
			}
		}
		ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem.MainModule main2 = componentsInChildren[i].main;
			main2.stopAction = ParticleSystemStopAction.None;
			if (main2.startLifetime.constant > num)
			{
				num = main2.startLifetime.constant;
			}
		}
		this.life = num + 0.2f;
	}

	// Token: 0x04000905 RID: 2309
	public float life = 1f;
}
