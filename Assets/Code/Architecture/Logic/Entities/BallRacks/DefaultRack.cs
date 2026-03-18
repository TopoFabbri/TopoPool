using System.Collections.Generic;
using System.Numerics;

namespace Architecture.Logic.Entities.BallRacks
{
    internal sealed class DefaultRack : IPoolRack
    {
        public List<Vector3> WhiteRack { get; } = new() { new Vector3(-.3f, 0, 0) };

        public List<Vector3> BlackRack { get; } = new() { new Vector3(.5f, 0, 0f) };

        public List<Vector3> StripeRack { get; } = new()
        {
            new Vector3(.3f, 0, 0),
            new Vector3(.4f, 0, .05f),
            new Vector3(.5f, 0, -.1f),
            new Vector3(.6f, 0, -.05f),
            new Vector3(.6f, 0, .15f),
            new Vector3(.7f, 0, -.2f),
            new Vector3(.7f, 0, .1f),
        };

        public List<Vector3> SolidRack { get; } = new()
        {
            new Vector3(.4f, 0, -.05f),
            new Vector3(.5f, 0, .1f),
            new Vector3(.6f, 0, -.15f),
            new Vector3(.6f, 0, .05f),
            new Vector3(.7f, 0, -.1f),
            new Vector3(.7f, 0, 0f),
            new Vector3(.7f, 0, .2f),
        };
    }
}