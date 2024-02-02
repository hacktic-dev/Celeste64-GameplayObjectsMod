using Celeste64;
using Foster.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlatformsMod
{
	internal class CyclePlatform : Solid
	{

		internal CyclePlatform(Vector3 secondPositionDelta, float position1MoveTime, float position1RestTime, float position2MoveTime, float position2RestTime)
		{
			m_firstPosition = Position;
			m_secondPosition = m_firstPosition + secondPositionDelta;
			m_position1RestTime = position1RestTime;
			m_position1MoveTime = position1MoveTime + m_position1RestTime;
			m_position2RestTime = position2RestTime + m_position1MoveTime;
			m_position2MoveTime = position2MoveTime + m_position2RestTime;
			m_cycle = m_position2MoveTime;
		}

		Vector3 m_firstPosition;
		Vector3 m_secondPosition;
		float m_position1MoveTime;
		float m_position1RestTime;
		float m_position2MoveTime;
		float m_position2RestTime;
		float m_cycle;

		float m_time;

		public override void Update()
		{
			base.Update();

			m_time += Foster.Framework.Time.Delta;
			if (m_time > m_cycle)
				m_time -= m_cycle;

			var distance = Vector3.Distance(m_firstPosition, m_secondPosition);
			var speed = distance / m_position1MoveTime;
			var offset = new System.Numerics.Vector3(0, 0, 0);

			if(m_time < m_position1RestTime)
			{
				// do nothing
			}
			else if(m_time >m_position1RestTime && m_time < m_position1MoveTime)
			{
				// move from position 1 to position 2
				offset = speed * Foster.Framework.Time.Delta * (m_secondPosition - m_firstPosition);
			}
			else if(m_time > m_position1MoveTime && m_time < m_position2RestTime)
			{
				// do nothing
			}
			else if(m_time > m_position2RestTime && m_time < m_position2MoveTime)
			{
				// move from position 2 to position 1
				offset = speed * Foster.Framework.Time.Delta * (m_firstPosition - m_secondPosition);
			}

			Log.Info(offset);
			Position += offset;
			if (HasPlayerRider())
			{
				World.Get<Player>().RidingPlatformMoved(offset);
			}
		}

	}
}
