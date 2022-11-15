using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    
    private Camera _camera;
    private LayerMask _cubeMask;
    private LayerMask _pointMask;
    private bool _rotating;
    private Vector3 _initialPos;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = Camera.main;
        _cubeMask = LayerMask.GetMask("Cube", "Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, _cubeMask))
                return;
            
            if(hit.collider.CompareTag("Cube"))
            {
                _initialPos = Input.mousePosition;
                _rotating = true;
            }
            else if(hit.collider.CompareTag("Point"))
            {
                _rotating = false;
                
                var direction = hit.transform.localPosition;
                var distance = 
                    Vector3.Distance(transform.position, hit.transform.position);

                foreach (var p in _points)
                {
                    p.transform.SetParent(transform.parent);
                }
                transform.localScale += new Vector3(
                    Mathf.Abs(direction.x / transform.localScale.x),
                    Mathf.Abs(direction.y / transform.localScale.y),
                    Mathf.Abs(direction.z / transform.localScale.z));
                
                foreach (var p in _points)
                {
                    p.transform.SetParent(transform);
                }
            }
        }
        if (Input.GetMouseButton(0) && _rotating)
        {
            var diff = Input.mousePosition - _initialPos;
            _initialPos = Input.mousePosition;
            transform.Rotate(Vector3.up, -diff.x, Space.World);
            transform.Rotate(Vector3.right, diff.y, Space.World);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _rotating = false;
        }

    }
}
