using System;
using Celeste64;
namespace SinePlatformMod
{
    public class SinePlatformMod : GameMod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();

						Map.ActorFactory factory = new((map, entity) => new SinePlatform(entity.GetFloatProperty("phase", 0), entity.GetStringProperty("direction", "X"))) { IsSolidGeometry = true };
						AddActorFactory("SinePlatform", factory);
        }
    }
}
