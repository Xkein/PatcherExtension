using Extension.FX.Definitions;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Renders
{
    public class FXRibbonRenderer : FXRenderer
    {
        public FXRibbonRenderer(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        public override FXScript Clone(FXSystem system = null, FXEmitter emitter = null)
        {
            var ribbonRenderer = new FXRibbonRenderer(system ?? System, emitter ?? Emitter);

            return ribbonRenderer;
        }

        public override void ParticleRender(FXParticle particle)
        {
            Vector3 curPosition = particle.Position;
            Vector3 previousPosition = particle.Map.GetValueOrDefault("PreviousPosition", curPosition);

            // temporary render
            CoordStruct lastLocation = new CoordStruct((int)previousPosition.X, (int)previousPosition.Y, (int)previousPosition.Z);
            CoordStruct nextLocation = new CoordStruct((int)curPosition.X, (int)curPosition.Y, (int)curPosition.Z);
            if (lastLocation.DistanceFrom(nextLocation) > 128)
            {
                ColorStruct innerColor = new ColorStruct(208, 10, 203);
                ColorStruct outerColor = new ColorStruct(88, 0, 88);
                ColorStruct outerSpread = new ColorStruct(10, 10, 10);
                int duration = 10;
                int thickness = 3;
                lock (this)
                {
                    Pointer<LaserDrawClass> pLaser = YRMemory.Create<LaserDrawClass>(lastLocation, nextLocation, innerColor, outerColor, outerSpread, duration);
                    pLaser.Ref.Thickness = thickness;
                    pLaser.Ref.IsHouseColor = true;
                }

                particle.Map.SetValue("PreviousPosition", curPosition);
            }
        }
    }
}
