using System;
using Celeste64;
namespace PlatformsMod
{
	public class PlatformsMod : GameMod
	{
		public override void OnModLoaded()
		{
			base.OnModLoaded();

			Map.ActorFactory sinePlatformFactory = new((map, entity) => new SinePlatform(entity.GetFloatProperty("phase", 0), entity.GetStringProperty("direction", "X"))) { IsSolidGeometry = true };
			AddActorFactory("SinePlatform", sinePlatformFactory);

			Map.ActorFactory cyclePlatformFactory = new((map, entity) =>
				new CyclePlatform(entity.GetVectorProperty("secondPositionDelta", new System.Numerics.Vector3(0, 0, 0)),
													entity.GetFloatProperty("position1MoveTime", 1),
													entity.GetFloatProperty("position1RestTime", 1),
													entity.GetFloatProperty("position2MoveTime", 1),
													entity.GetFloatProperty("position2RestTime", 1)))
													{ IsSolidGeometry = true };
			AddActorFactory("CyclePlatform", cyclePlatformFactory);
		}
	}
}
