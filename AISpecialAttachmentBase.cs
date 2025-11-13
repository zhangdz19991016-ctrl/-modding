using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class AISpecialAttachmentBase : MonoBehaviour
{
	// Token: 0x060004EE RID: 1262 RVA: 0x0001650A File Offset: 0x0001470A
	public void Init(AICharacterController _ai, CharacterMainControl _character)
	{
		this.aiCharacterController = _ai;
		this.character = _character;
		this.OnInited();
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00016520 File Offset: 0x00014720
	protected virtual void OnInited()
	{
	}

	// Token: 0x04000423 RID: 1059
	public AICharacterController aiCharacterController;

	// Token: 0x04000424 RID: 1060
	public CharacterMainControl character;
}
