using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
   public Camera m_Cam;

   public NavMeshAgent m_Agent;

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Ray ray = m_Cam.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;

         if (Physics.Raycast(ray, out hit))
         {
            m_Agent.SetDestination(hit.point);
            NavMeshPath p = m_Agent.path;
            Vector3[] v = p.corners;
         }
      }
   }
}
