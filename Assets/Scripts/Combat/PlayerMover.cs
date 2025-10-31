using UnityEngine;

namespace BeatMayhem.Combat
{
    /// <summary>
    /// Basic WASD movement controller for the arena.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        public float moveSpeed = 9f;
        public float gravity = -20f;

        private CharacterController _characterController;
        private Vector3 _velocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            input = Vector3.ClampMagnitude(input, 1f);

            Vector3 movement = (transform.right * input.x + transform.forward * input.z) * moveSpeed;

            _velocity.y += gravity * Time.deltaTime;

            _characterController.Move((movement + _velocity) * Time.deltaTime);

            if (_characterController.isGrounded)
            {
                _velocity.y = 0f;
            }
        }
    }
}
