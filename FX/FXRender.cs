﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public abstract class FXRender : FXScript
    {
        protected FXRender(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        public virtual void ParticleRender(FXParticle particle)
        {
            throw new NotImplementedException($"{this} has not implement ParticleUpdate!");
        }
    }
}
