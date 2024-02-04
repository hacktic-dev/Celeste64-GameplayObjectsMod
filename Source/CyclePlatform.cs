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
	internal class CyclePlatform : Solid, IGlobalTime
	{

		internal CyclePlatform(Vector3 secondPosition, float position1MoveTime, float position1RestTime, float position2MoveTime, float position2RestTime)
		{
			m_secondPosition = secondPosition;
			m_position1DepartureTime = position1RestTime;
			m_position2ArrivalTime = position1MoveTime + m_position1DepartureTime;
			m_position2DepartureTime = position2RestTime + m_position2ArrivalTime;
			m_position1ArrivalTime = position2MoveTime + m_position2DepartureTime;
			m_cycle = m_position1ArrivalTime;

			m_position1MoveTime = position1MoveTime;
			m_position2MoveTime = position2MoveTime;
		}

		Vector3 m_firstPosition;
		Vector3 m_secondPosition;
		float m_position2ArrivalTime;
		float m_position1DepartureTime;
		float m_position1ArrivalTime;
		float m_position2DepartureTime;

		float m_position1MoveTime;
		float m_position2MoveTime;

		bool m_isFirstPositionSet = false;

		float m_cycle;

		public float Time { get => m_time; set => m_time = value; }
		float m_time;

		public override void Update()
		{
			base.Update();

			if(!m_isFirstPositionSet)
			{
				m_firstPosition = Position;
				m_isFirstPositionSet = true;
			}

			float cycleTime = Time % m_cycle;

			var newPos = new Vector3(0, 0, 0);

			if(cycleTime < m_position1DepartureTime)
			{
				newPos = m_firstPosition;
			}
			else if(cycleTime > m_position1DepartureTime && cycleTime < m_position2ArrivalTime)
			{
				var totalMovementTime = m_position2ArrivalTime - m_position1DepartureTime;
				var t = (cycleTime - m_position1DepartureTime) / totalMovementTime;
				// move from position 1 to position 2
				newPos = Vector3.Lerp(m_firstPosition, m_secondPosition, t);
			}
			else if((cycleTime > m_position2ArrivalTime && cycleTime < m_position2DepartureTime))
			{
				newPos = m_secondPosition;
			}
			else if(cycleTime > m_position2DepartureTime && cycleTime < m_position1ArrivalTime)
			{
				var totalMovementTime = m_position1ArrivalTime - m_position2DepartureTime;
				var t = (cycleTime - m_position2DepartureTime) / totalMovementTime;
				// move from position 2 to position 1
				newPos = Vector3.Lerp(m_secondPosition, m_firstPosition, t);
			}

			var offset = newPos - Position;
			Position = newPos;
			if (HasPlayerRider())
			{
				World.Get<Player>().RidingPlatformMoved(offset);
				World.Get<Player>().RidingPlatformSetVelocity(offset * 100);
			}
		}

	}
}
