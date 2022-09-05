using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public struct ProjectileData
    {
        public LayerMask hitMask;
        public float speed;
        public Vector2 direction;
        public float range;
        public int damage;
        public bool dieOnCollide;
        public Actor source;
    }
}
