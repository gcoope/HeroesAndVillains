using System;

namespace smoothstudio.heroesandvillains.player.projectiles
{
	public interface IProjectile
	{
		void TidyUp();
		void Init(BasePlayerInfo player);
	}
}

