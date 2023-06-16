using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform[] MapNodes;
    float mapNodesPlayerDist;
    float step;
    private Vector3 mapTarget = new Vector3(0, 0, 0);
    void Awake()
    {
        mapNodesPlayerDist = Vector3.Distance(MapNodes[0].transform.position, transform.position);
        step = 1 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {
                if (hit.collider.CompareTag("Map Node")) {
                    mapTarget = new Vector3(hit.transform.gameObject.transform.position.x + 1,
                        hit.transform.gameObject.transform.position.y,
                        hit.transform.gameObject.transform.position.z);
                }
            }
        }

        if (transform.position - mapTarget != Vector3.zero && mapTarget != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, mapTarget, step);
        }
    }
}
