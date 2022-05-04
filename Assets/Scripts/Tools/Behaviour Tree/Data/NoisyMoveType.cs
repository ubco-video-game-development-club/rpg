using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [CreateAssetMenu(fileName = "NoisyMoveType", menuName = "Move Types/Noisy", order = 65)]
    public class NoisyMoveType : DynamicMoveType
    {
        private const float PERLIN_OFFSET_X = 1.4129837f;
        private const float PERLIN_OFFSET_Y = 1293.1298f;

        [Tooltip("The proportion of noisy movement applied perpendicular to the move direction (higher = more wavy).")]
        [SerializeField] private float noiseFactor = 0.2f;

        [Tooltip("The scale of perlin noise used to determine move offset (higher = more chaotic).")]
        [SerializeField] private float noiseScale = 0.05f;

        public override bool UpdateMove(Tree<Behaviour>.Node node, BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed)
        {
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;
            Vector2 moveDir = (endPos - startPos).normalized;
            Vector2 noiseDir = Vector2.Perpendicular(moveDir);
            float noiseVal = Mathf.PerlinNoise(Time.time * noiseScale + PERLIN_OFFSET_X, PERLIN_OFFSET_Y) * 2 - 1;
            float noiseSpeed = speed * noiseFactor * noiseVal;
            rigidbody2D.velocity = moveDir * speed + noiseDir * noiseSpeed;
            return true;
        }
    }
}
