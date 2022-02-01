using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDetectCollider : MonoBehaviour
{
    private Transform m_Transform;
    private Vector3 m_FrontRightSensorLocalPos;
    private Vector3 m_BackRightSensorLocalPos;
    private Vector3 m_BackLeftSensorLocalPos;
    private Vector3 m_FackLeftSensorLocalPos;

    [SerializeField]
    private float m_RayCastLength = 5f;
    void Start()
    {
        m_Transform = transform;
        Vector3 bounds = gameObject.GetComponent<Collider>().bounds.size / 2;
        m_FrontRightSensorLocalPos = bounds;
        m_BackRightSensorLocalPos = new Vector3(bounds.x, bounds.y, -bounds.z);
        m_BackLeftSensorLocalPos = new Vector3(-bounds.x, bounds.y, -bounds.z);
        m_FackLeftSensorLocalPos = new Vector3(-bounds.x, bounds.y, bounds.z);
    }

    // Update is called once per frame
    void Update()
    {
        /*collisionFeedback cfb = BackwardSensing();
        if (cfb.componentStaticDetected || cfb.AlliedDetected || cfb.EnemyDetected)
        {
            Debug.Log("cfb componentStatic : " + cfb.componentStaticDetected);
            Debug.Log("cfb allied : " + cfb.AlliedDetected);
            Debug.Log("cfb enemy : " + cfb.EnemyDetected);
        }*/
    }

    public struct cardinalsPointsCollisionFeedback
    {
        public Tuple<bool,float> componentStaticDetected;
        public Tuple<bool,float> AlliedDetected;
        public Tuple<bool,float> EnemyDetected;
    }
    
    // return :
    // componentStaticDetected -> true if detected
    // AlliedDetected -> true if detected before an componentStaticDetected and first tank detected
    // EnemyDetected -> true if detected before an componentStaticDetected and first tank detected
    public struct collisionFeedback
    {
        public bool componentStaticDetected;
        public bool AlliedDetected;
        public bool EnemyDetected;
    }

    public collisionFeedback DirectionnalSensing()
    {
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;
        
        Vector3 frontRightSensorGlobalPos = tankPos + tankRotation * m_FrontRightSensorLocalPos;
        Vector3 backRightSensorGlobalPos = tankPos + tankRotation * m_BackRightSensorLocalPos;
        Vector3 backLeftSensorGlobalPos = tankPos + tankRotation * m_BackLeftSensorLocalPos;
        Vector3 frontLeftSensorGlobalPos = tankPos + tankRotation * m_FackLeftSensorLocalPos;
        
        /*Debug.DrawLine(tankPos, frontRightSensorGlobalPos, Color.magenta);
        Debug.DrawLine(tankPos, backRightSensorGlobalPos, Color.magenta);
        Debug.DrawLine(tankPos, backLeftSensorGlobalPos, Color.magenta);
        Debug.DrawLine(tankPos, frontLeftSensorGlobalPos, Color.magenta);*/
        
        collisionFeedback cfb;
        cfb.componentStaticDetected = false;
        cfb.AlliedDetected = false;
        cfb.EnemyDetected = false;
        return cfb;
    }

    public collisionFeedback BackwardSensing()
    {
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;
        
        Vector3 backRightSensorGlobalPos = tankPos + tankRotation * m_BackRightSensorLocalPos;
        Vector3 backLeftSensorGlobalPos = tankPos + tankRotation * m_BackLeftSensorLocalPos;

        RaycastHit[] rightHits = Physics.RaycastAll(backRightSensorGlobalPos, -transform.forward, m_RayCastLength);
        cardinalsPointsCollisionFeedback rightCfb =  AnalysisCollisions(rightHits);
        
        RaycastHit[] leftHits = Physics.RaycastAll(backLeftSensorGlobalPos, -transform.forward, m_RayCastLength);
        cardinalsPointsCollisionFeedback leftCfb =  AnalysisCollisions(leftHits);

        collisionFeedback ret;
        ret.componentStaticDetected = rightCfb.componentStaticDetected.Item1 || leftCfb.componentStaticDetected.Item1;
        ret.AlliedDetected = ((rightCfb.AlliedDetected.Item1 && (!leftCfb.componentStaticDetected.Item1 || rightCfb.AlliedDetected.Item2 < leftCfb.componentStaticDetected.Item2)) ||
                              (leftCfb.AlliedDetected.Item1 && (!rightCfb.componentStaticDetected.Item1 || leftCfb.AlliedDetected.Item2 < rightCfb.componentStaticDetected.Item2)));
        ret.EnemyDetected = ((rightCfb.EnemyDetected.Item1 && (!leftCfb.componentStaticDetected.Item1 || rightCfb.EnemyDetected.Item2 < leftCfb.componentStaticDetected.Item2)) ||
                             (leftCfb.EnemyDetected.Item1 && (!rightCfb.componentStaticDetected.Item1 || leftCfb.EnemyDetected.Item2 < rightCfb.componentStaticDetected.Item2)));
        
        return ret;
    }

    public cardinalsPointsCollisionFeedback AnalysisCollisions(RaycastHit[] hits)
    {
        cardinalsPointsCollisionFeedback cfb;
        cfb.componentStaticDetected = new Tuple<bool, float>(false, -1);
        cfb.AlliedDetected = new Tuple<bool, float>(false, -1);
        cfb.EnemyDetected = new Tuple<bool, float>(false, -1);
        
        float smallestDistanceWithComponentStatic = -1;
        float smallestDistanceWithTank = -1;
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (!hit.collider.gameObject.tag.Equals("Tank"))
                {
                    if ((smallestDistanceWithComponentStatic >= -1.01f && smallestDistanceWithComponentStatic <= -0.99f) || hit.distance < smallestDistanceWithComponentStatic)
                    {
                        smallestDistanceWithComponentStatic = hit.distance;
                    }
                }
                else
                {
                    if ((smallestDistanceWithTank >= -1.01f && smallestDistanceWithTank <= -0.99f) || hit.distance < smallestDistanceWithTank)
                    {
                        smallestDistanceWithTank = hit.distance;
                    }
                }
            }

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag.Equals("Tank") && hit.distance >= smallestDistanceWithTank - 0.1f && hit.distance <= smallestDistanceWithTank + 0.1f && 
                    ((smallestDistanceWithComponentStatic >= -1.01f && smallestDistanceWithComponentStatic <= -0.99f) || smallestDistanceWithTank < smallestDistanceWithComponentStatic))
                {
                    if (gameObject.GetComponent<TankIndexManager>().m_TeamIndex == hit.collider.gameObject.GetComponent<TankIndexManager>().m_TeamIndex)
                    {
                        cfb.AlliedDetected = new Tuple<bool, float>(true, hit.distance);
                    }
                    else
                    {
                        cfb.EnemyDetected = new Tuple<bool, float>(true, hit.distance);
                    }
                }
                else if (!hit.collider.gameObject.tag.Equals("Tank") && hit.distance >= smallestDistanceWithComponentStatic - 0.1f && hit.distance <= smallestDistanceWithComponentStatic)
                {
                    cfb.componentStaticDetected = new Tuple<bool, float>(true, hit.distance);
                }
            }
        }

        return cfb;
    }
}
