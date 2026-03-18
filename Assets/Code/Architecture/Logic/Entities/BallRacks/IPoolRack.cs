using System.Collections.Generic;
using System.Numerics;

namespace Architecture.Logic.Entities.BallRacks
{
    public interface IPoolRack
    {
        List<Vector3> WhiteRack  { get; }
        List<Vector3> BlackRack  { get; }
        List<Vector3> StripeRack { get; }
        List<Vector3> SolidRack  { get; }
    }
}