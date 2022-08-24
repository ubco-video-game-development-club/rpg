using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float followTime = 0.2f;

        private Transform target;
        private Vector2 currVelocity;

        private void Awake()
        {
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            SetTarget(GameManager.Player.transform);
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                Vector2 pos = Vector2.SmoothDamp(transform.position, target.position, ref currVelocity, followTime);
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
