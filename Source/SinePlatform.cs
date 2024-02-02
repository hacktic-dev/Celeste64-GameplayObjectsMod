using Celeste64;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformsMod
{
	internal class SinePlatform : Solid
	{

		internal SinePlatform(float phase, string direction) { m_phase = phase; m_direction = direction; }

		float m_phase;
		float m_time;
		string m_direction;
		float m_multiplier = 0.5f;

		public override void Update()
		{
			base.Update();

			m_time += Foster.Framework.Time.Delta;

			var offset = new System.Numerics.Vector3(0, 0, 0);

			if (m_direction == "X")
				offset = new System.Numerics.Vector3(m_multiplier * MathF.Cos(m_time + m_phase), 0, 0);
			else if (m_direction == "Y")
				offset = new System.Numerics.Vector3(0, m_multiplier * MathF.Cos(m_time + m_phase), 0);
			else
				offset = new System.Numerics.Vector3(0, 0, m_multiplier * MathF.Cos(m_time + m_phase));

			Position += offset;
			if (HasPlayerRider())
			{
				World.Get<Player>().RidingPlatformMoved(offset);
				World.Get<Player>().RidingPlatformSetVelocity(offset * 100);
			}
		}

	}
}
