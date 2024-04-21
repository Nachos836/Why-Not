using System.ComponentModel.DataAnnotations;
using Unity.Entities;
using Unity.Physics;

namespace WhyNot.Input
{
    internal struct InputBuffer : IBufferElementData
    {
        [Required] public RaycastInput Raycast;
    }
}
