using System;
using Celeste64;
using Foster.Framework;
using Sledge.Formats.Map.Objects;
using static Foster.Framework.Aseprite;
namespace PlatformsMod
{
	public class PlatformsMod : GameMod
	{
		float m_time = 0;
		List<CyclePlatform> m_platforms = new List<CyclePlatform>();

		public override void OnModLoaded()
		{
			base.OnModLoaded();

			Map.ActorFactory sinePlatformFactory = new((map, entity) => new SinePlatform(entity.GetFloatProperty("phase", 0), entity.GetStringProperty("direction", "X"))) { IsSolidGeometry = true };
			AddActorFactory("SinePlatform", sinePlatformFactory);

			Map.ActorFactory conveyerBeltFactory = new((map, entity) => new ConveyerBelt(entity.GetStringProperty("direction", "X"), entity.GetFloatProperty("speed", 1))) { IsSolidGeometry = true };
			AddActorFactory("ConveyerBelt", conveyerBeltFactory);

			Map.ActorFactory cyclePlatformFactory = new((map, entity) =>
				new CyclePlatform(entity.GetVectorProperty("secondPositionDelta", new System.Numerics.Vector3(0, 0, 0)),
													entity.GetFloatProperty("position1MoveTime", 1),
													entity.GetFloatProperty("position1RestTime", 1),
													entity.GetFloatProperty("position2MoveTime", 1),
													entity.GetFloatProperty("position2RestTime", 1)))
													{ IsSolidGeometry = true };
			AddActorFactory("CyclePlatform", cyclePlatformFactory);
		}

		public override void OnMapLoaded(Map map)
		{
			m_platforms.Clear();
		}

		public override void OnActorAdded(Actor actor)
		{
			base.OnActorAdded(actor);
			if (actor.GetType() == typeof(CyclePlatform))
			{
				m_platforms.Add((CyclePlatform)actor);
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			m_time += deltaTime;

			foreach(var platform in m_platforms)
			{
				platform.Time = m_time;
			}
		}
	}
}
