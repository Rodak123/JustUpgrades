using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UTDragMovement : MonoBehaviour
    {
        [SerializeField] private bool isInverted;

        [Header("Constraints")]
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY;

        private bool isDragging;
        private Vector2 mouseStart;
        private Vector3 positionStart;

        private void Update()
        {
            if (isDragging)
            {
                UpdateDrag();
                return;
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                isDragging = true;
                mouseStart = Mouse.current.position.value;
                positionStart = transform.position;
            }
        }

        private void UpdateDrag()
        {
            Vector3 worldStart = Camera.main.ScreenToWorldPoint(mouseStart);
            Vector3 worldEnd = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            Vector2 dragDelta = worldEnd - worldStart;

            if (lockX) dragDelta.x = 0;
            if (lockY) dragDelta.y = 0;
            if (isInverted) dragDelta *= -1;

            transform.position = positionStart + new Vector3(dragDelta.x, dragDelta.y, 0);

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                isDragging = false;
            }
        }
    }
}