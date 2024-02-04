using Celeste64;
using Foster.Framework;
using Sledge.Formats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlatformsMod
{
	internal class RotatingPlatform : Solid, IGlobalTime
	{

		internal RotatingPlatform(float speed)
		{
			m_speed = speed;
		}

		float m_speed;

		public float Time { get => m_time; set => m_time = value; }
		float m_time;

		public override void Update()
		{
			base.Update();

			var currentFacing = Facing;

			//rotate the platform
			Facing = new Vector2(MathF.Cos(m_speed*Time), MathF.Sin(m_speed * Time));

			//calculate the angle between the old rotation and new one
			float dotProduct = Vector2.Dot(currentFacing, Facing);

			float magnitude1 = currentFacing.Length();
			float magnitude2 = Facing.Length();

			float angle = (m_speed > 0 ? 1 : -1) * (float)Math.Acos(dotProduct / (magnitude1 * magnitude2));

			if (HasPlayerRider())
			{
				var playerPos = World.Get<Player>().Position;

				//now lets move the player.
				World.Get<Player>().Position = RotatePointByAngle(new Vector3(Position.X, Position.Y, 0), new Vector3(World.Get<Player>().Position.X, World.Get<Player>().Position.Y, 0), angle) + new Vector3(0, 0, playerPos.Z);
				World.Get<Player>().RidingPlatformSetVelocity((World.Get<Player>().Position - playerPos) * 100);

				if (World.Get<Player>().RelativeMoveInput == Vector2.Zero)
				{
					var playerFacing = World.Get<Player>().Facing;

					//and finally rotate the player
					var facingDelta = RotatePointByAngle(Position, Position + new Vector3(playerFacing.X, playerFacing.Y, 0), angle);

					facingDelta -= new Vector3(Position.X, Position.Y, 0);

					World.Get<Player>().Facing = new Vector2(facingDelta.X, facingDelta.Y);
					World.Get<Player>().SetTargetFacing(World.Get<Player>().Facing);
				}
			}
		}

		Vector3 RotatePointByAngle(Vector3 origin, Vector3 point, float angleRadians)
		{
			Vector3 direction = point - origin;

			float sinAngle = MathF.Sin(angleRadians);
			float cosAngle = MathF.Cos(angleRadians);

			float newX = direction.X * cosAngle - direction.Y * sinAngle;
			float newY = direction.X * sinAngle + direction.Y * cosAngle;

			Vector3 rotatedPoint = new Vector3(newX, newY, 0);
			return origin + rotatedPoint;
		}
	}

}
