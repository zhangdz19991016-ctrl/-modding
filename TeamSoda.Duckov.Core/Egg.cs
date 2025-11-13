using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class Egg : MonoBehaviour
{
	// Token: 0x06000557 RID: 1367 RVA: 0x00017FDA File Offset: 0x000161DA
	private void Start()
	{
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00017FDC File Offset: 0x000161DC
	public void Init(Vector3 spawnPosition, Vector3 spawnVelocity, CharacterMainControl _fromCharacter, CharacterRandomPreset preset, float _life)
	{
		this.characterPreset = preset;
		base.transform.position = spawnPosition;
		if (this.rb)
		{
			this.rb.position = spawnPosition;
			this.rb.velocity = spawnVelocity;
		}
		this.fromCharacter = _fromCharacter;
		this.life = _life;
		this.inited = true;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00018038 File Offset: 0x00016238
	private UniTaskVoid Spawn()
	{
		Egg.<Spawn>d__10 <Spawn>d__;
		<Spawn>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Spawn>d__.<>4__this = this;
		<Spawn>d__.<>1__state = -1;
		<Spawn>d__.<>t__builder.Start<Egg.<Spawn>d__10>(ref <Spawn>d__);
		return <Spawn>d__.<>t__builder.Task;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001807C File Offset: 0x0001627C
	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.life && !this.spawned)
		{
			this.spawned = true;
			this.Spawn().Forget();
		}
	}

	// Token: 0x040004C7 RID: 1223
	public GameObject spawnFx;

	// Token: 0x040004C8 RID: 1224
	public CharacterMainControl fromCharacter;

	// Token: 0x040004C9 RID: 1225
	public Rigidbody rb;

	// Token: 0x040004CA RID: 1226
	private float life;

	// Token: 0x040004CB RID: 1227
	private CharacterRandomPreset characterPreset;

	// Token: 0x040004CC RID: 1228
	private bool inited;

	// Token: 0x040004CD RID: 1229
	private float timer;

	// Token: 0x040004CE RID: 1230
	private bool spawned;
}
