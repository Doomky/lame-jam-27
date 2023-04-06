using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererRaycaster : MonoBehaviour
{
    private LineRenderer _lineRenderer = null;

    [SerializeField] private float _raycastDistance = 5;
    [SerializeField] private LayerMask _raycastMask;

    protected void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    protected void Update()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.right, _raycastDistance, _raycastMask);
        _lineRenderer.SetPosition(1, hit2D.distance * Vector3.right);
    }
}
