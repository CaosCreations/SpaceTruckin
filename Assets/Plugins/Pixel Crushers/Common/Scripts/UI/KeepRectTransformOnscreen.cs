// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Keeps a RectTransform's bounds in view of the main camera. 
    /// Works best on world space panels.
    /// Improvements contribued by Sayezz.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    [RequireComponent(typeof(RectTransform))]
    public class KeepRectTransformOnscreen : MonoBehaviour
    {

        [Tooltip("Padding from edges of screen in viewport units (0 to 1).")]
        public Vector2 padding = new Vector2(0.05f, 0.05f);
        [Tooltip("Rotate RectTransform so it always faces camera.")]
        public bool alwaysFaceCamera = true;

        private Camera mainCamera = null;
        private Transform parentTransform;
        private RectTransform rectTransform;
        private float originalX = 0;
        private bool applied = false;
        private float verticalOffset;

        private void Start()
        {
            mainCamera = Camera.main;
            parentTransform = transform.parent;
            rectTransform = GetComponent<RectTransform>();
            originalX = rectTransform.position.x;
            verticalOffset = transform.localPosition.y;
            if (parentTransform == null || rectTransform == null || mainCamera == null) enabled = false;
        }

        private void OnEnable()
        {
            applied = false;
            RestoreOriginalPosition();
        }

        private void LateUpdate()
        {
            if (alwaysFaceCamera) transform.rotation = mainCamera.transform.rotation;

            // Try to keep at original position when possible:
            Vector3 originalPos = parentTransform.position + Vector3.up * verticalOffset;
            transform.position = originalPos;

            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            // Convert position and corners to screen space:
            Vector3 pos = mainCamera.WorldToViewportPoint(rectTransform.position);
            Vector3 bottomLeft = mainCamera.WorldToViewportPoint(worldCorners[0]);
            Vector3 topRight = mainCamera.WorldToViewportPoint(worldCorners[2]);

            float offsetX = 0f;
            float offsetY = 0f;

            if (topRight.x > (1 - padding.x))
            {
                offsetX = topRight.x - (1 - padding.x);
            }

            else if (bottomLeft.x < padding.x)
            {
                offsetX = bottomLeft.x - padding.x;
            }

            if (topRight.y > (1 - padding.y))
            {
                offsetY = topRight.y - (1 - padding.y);
            }

            else if (bottomLeft.y < padding.y)
            {
                offsetY = bottomLeft.y - padding.y;
            }

            if (offsetX != 0 || offsetY != 0)
            {
                pos.x = Mathf.Clamp(pos.x - offsetX, 0, 1);
                pos.y = Mathf.Clamp(pos.y - offsetY, 0, 1);

                rectTransform.position = mainCamera.ViewportToWorldPoint(pos);
                applied = true;

            }
            else
            {
                if (!applied)
                {
                    RestoreOriginalPosition();
                }
            }
        }

        private void RestoreOriginalPosition()
        {
            if (mainCamera == null || rectTransform == null) return;

            rectTransform.position = new Vector3(originalX, rectTransform.position.y, rectTransform.position.z);
            Vector3 pos1 = mainCamera.WorldToViewportPoint(rectTransform.position);
            rectTransform.position = mainCamera.ViewportToWorldPoint(pos1);
        }
    }

}
