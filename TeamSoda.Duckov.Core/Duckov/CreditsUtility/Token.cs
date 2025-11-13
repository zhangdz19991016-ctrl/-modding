using System;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000300 RID: 768
	public struct Token
	{
		// Token: 0x0600192B RID: 6443 RVA: 0x0005BFD4 File Offset: 0x0005A1D4
		public Token(TokenType type, string text = null)
		{
			this.type = type;
			this.text = text;
		}

		// Token: 0x04001246 RID: 4678
		public TokenType type;

		// Token: 0x04001247 RID: 4679
		public string text;
	}
}
