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
        private RectTransform rectTransform;
        private float originalX = 0;
        private bool applied = false;
        private Camera mainCamera = null;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originalX = rectTransform.position.x;
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            applied = false;
            RestoreOriginalPosition();
        }

        private void LateUpdate()
        {
            if (mainCamera == null || rectTransform == null) return;

            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            // Convert position and corners to screen space:
            Vector3 pos = mainCamera.WorldToViewportPoint(rectTransform.position);
            Vector3 bottomLeft = mainCamera.WorldToViewportPoint(worldCorners[0]);
            Vector3 topRight = mainCamera.WorldToViewportPoint(worldCorners[2]);

            float offsetX = 0f;

            if (topRight.x > 1)
            {
                offsetX = topRight.x - 1;
            }

            else if (bottomLeft.x < 0)
            {
                offsetX = bottomLeft.x;
            }

            if (offsetX != 0)
            {
                pos.x = Mathf.Clamp(pos.x - offsetX, 0, 1);

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
