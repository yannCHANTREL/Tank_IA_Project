using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
   public Camera cam;

   public NavMeshAgent agent;

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Ray ray = cam.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;

         if (Physics.Raycast(ray, out hit))
         {
            agent.SetDestination(hit.point);
            NavMeshPath p = agent.path;
            Vector3[] v = p.corners;
         }
      }
   }
}
