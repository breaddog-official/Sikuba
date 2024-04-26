using UnityEngine;

namespace Black.ClientSidePrediction
{
    public struct ClientInput
    {
        public ulong Frame;

        // Add your inputs here.
        public Vector2 MovementAxes;
        public Ray MouseRay;
    }

    public struct ServerResult
    {
        public ulong Frame;
        public byte Buffer;

        // Add your results here.
        public Vector3 RigidbodyPosition;
        public Quaternion RigidbodyRotation;
    }
}