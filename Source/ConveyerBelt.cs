using Celeste64;
using Foster.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformsMod
{
	internal class ConveyerBelt : Solid
	{

		internal ConveyerBelt(string direction, float speed) { m_speed = speed; m_direction = direction; }

		float m_speed;
		string m_direction;

		public override void Update()
		{
			base.Update();

			if (HasPlayerRider())
			{
				var offset = new System.Numerics.Vector3(0, 0, 0);

				if (m_direction == "X")
					offset = new System.Numerics.Vector3(m_speed, 0, 0);
				else
					offset = new System.Numerics.Vector3(0, m_speed, 0);

				World.Get<Player>().RidingPlatformMoved(offset);
				World.Get<Player>().RidingPlatformSetVelocity(offset*100);
			}
		}

	}
}
