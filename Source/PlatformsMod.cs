﻿using System;
using Celeste64;
using Foster.Framework;
using Sledge.Formats.Map.Objects;
using static Foster.Framework.Aseprite;
namespace PlatformsMod
{
	public class PlatformsMod : GameMod
	{
		float m_time = 0;
		List<IGlobalTime> m_globalTimePlatforms;

		public override void OnModLoaded()
		{
			base.OnModLoaded();

			m_globalTimePlatforms = new List<IGlobalTime>();

			Map.ActorFactory sinePlatformFactory = new((map, entity) => new SinePlatform(entity.GetFloatProperty("phase", 0), entity.GetStringProperty("direction", "X"))) { IsSolidGeometry = true };
			AddActorFactory("SinePlatform", sinePlatformFactory);

			Map.ActorFactory conveyerBeltFactory = new((map, entity) => new ConveyorBelt(entity.GetStringProperty("direction", "X"), entity.GetFloatProperty("speed", 1))) { IsSolidGeometry = true };
			AddActorFactory("ConveyorBelt", conveyerBeltFactory);

			Map.ActorFactory cyclePlatformFactory = new((map, entity) =>
				new CyclePlatform(map.FindTargetNodeFromParam(entity, "target"),
													entity.GetFloatProperty("position1MoveTime", 1),
													entity.GetFloatProperty("position1RestTime", 1),
													entity.GetFloatProperty("position2MoveTime", 1),
													entity.GetFloatProperty("position2RestTime", 1)))
													{ IsSolidGeometry = true };
			AddActorFactory("CyclePlatform", cyclePlatformFactory);

			Map.ActorFactory rotatingPlatformFactory = new((map, entity) => new RotatingPlatform(entity.GetFloatProperty("speed", 1))) { IsSolidGeometry = true };
			AddActorFactory("RotatingPlatform", rotatingPlatformFactory);
		}

		public override void OnMapLoaded(Map map)
		{
			m_time = 0;
		}

		public override void OnActorAdded(Actor actor)
		{
			base.OnActorAdded(actor);
			if (actor is IGlobalTime)
			{
				m_globalTimePlatforms.Add((IGlobalTime)actor);
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			m_time += deltaTime;

			foreach(var platform in m_globalTimePlatforms)
			{
				platform.Time = m_time;
			}
		}
	}
}
