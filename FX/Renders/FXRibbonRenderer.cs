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

        // Parameters

        public Vector3 PreviousPosition { get; set; }
        public Vector3 Position { get; set; }

        public override FXScript Clone()
        {
            var ribbonRenderer = new FXRibbonRenderer(System, Emitter);

            ribbonRenderer.PreviousPosition = PreviousPosition;
            ribbonRenderer.Position = Position;

            return ribbonRenderer;
        }

        public override void ParticleRender(FXParticle particle)
        {
            var curPosition = particle.Position;

            lock (this)
            {
                // temporary render
                CoordStruct lastLocation = new CoordStruct((int)PreviousPosition.X, (int)PreviousPosition.Y, (int)PreviousPosition.Z);
                CoordStruct nextLocation = new CoordStruct((int)curPosition.X, (int)curPosition.Y, (int)curPosition.Z);
                if (lastLocation.DistanceFrom(nextLocation) > 100)
                {
                    ColorStruct innerColor = new ColorStruct(208, 10, 203);
                    ColorStruct outerColor = new ColorStruct(88, 0, 88);
                    ColorStruct outerSpread = new ColorStruct(10, 10, 10);
                    int duration = 10;
                    int thickness = 10;
                    Pointer<LaserDrawClass> pLaser = YRMemory.Create<LaserDrawClass>(lastLocation, nextLocation, innerColor, outerColor, outerSpread, duration);
                    pLaser.Ref.Thickness = thickness;
                    pLaser.Ref.IsHouseColor = true;
                }
            }

            PreviousPosition = Position;
            Position = curPosition;
        }
    }
}
