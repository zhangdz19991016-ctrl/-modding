using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class CharacterPosInShader : MonoBehaviour
{
	// Token: 0x0600088D RID: 2189 RVA: 0x000266B7 File Offset: 0x000248B7
	private void Update()
	{
		if (!CharacterMainControl.Main)
		{
			return;
		}
		Shader.SetGlobalVector(this.characterPosHash, CharacterMainControl.Main.transform.position);
	}

	// Token: 0x040007D1 RID: 2001
	private int characterPosHash = Shader.PropertyToID("CharacterPos");
}
