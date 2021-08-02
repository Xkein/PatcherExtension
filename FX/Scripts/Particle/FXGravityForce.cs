using Extension.FX.Definitions;

namespace Extension.FX.Scripts.Particle
{
    public class FXGravityForce : FXScript
    {
        public FXGravityForce(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        // Input Parameters

        public Vector3 Gravity { get; set; } = new Vector3(0, 0, -980);

        public override FXScript Clone()
        {
            var gravityForce = new FXGravityForce(System, Emitter);

            gravityForce.Gravity = Gravity;

            return gravityForce;
        }

        public override void ParticleUpdate(FXParticle particle)
        {
            particle.PhysicsForce += Gravity * particle.Mass;
        }
    }
}
