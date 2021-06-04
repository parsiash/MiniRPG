using UnityEngine;

namespace MiniRPG.Common
{
    public class CameraFit : CommonBehaviour
    {
        private Camera cameraComponent => RetrieveCachedComponent<Camera>();

        [SerializeField] private float minWidth = 15;
        [SerializeField] private float minHeight = 10;

        void Start()
        {
            var aspect = cameraComponent.aspect;
            cameraComponent.orthographicSize = Mathf.Max(minHeight, minWidth / aspect) / 2f;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(minWidth, minHeight, 1));
        }
    }
}