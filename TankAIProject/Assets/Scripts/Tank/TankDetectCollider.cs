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

    [SerializeField] private float m_RayCastBackLength = 1f;
    [SerializeField] private float m_RayCastFrontLength = 5f;
    [SerializeField] private float m_RayCastAnglesOfResearch = 2f;
    [SerializeField] private float m_SensingAltitudeFactor = 0.2f;

    void Start()
    {
        m_Transform = transform;
        Vector3 bounds = gameObject.GetComponent<Collider>().bounds.size / 2;
        
        m_FrontRightSensorLocalPos = new Vector3(bounds.x, bounds.y * 2.0f * m_SensingAltitudeFactor, bounds.z);
        m_BackRightSensorLocalPos = new Vector3(bounds.x, bounds.y * 2.0f * m_SensingAltitudeFactor, -bounds.z);
        m_BackLeftSensorLocalPos = new Vector3(-bounds.x, bounds.y * 2.0f * m_SensingAltitudeFactor, -bounds.z);
        m_FackLeftSensorLocalPos = new Vector3(-bounds.x, bounds.y * 2.0f * m_SensingAltitudeFactor, bounds.z);


    }

    // Update is called once per frame
    void Update()
    {
        // Test BackwardSensing

        /*collisionFeedback cfb = BackwardSensing();
        if (cfb.componentStaticDetected || cfb.AlliedDetected || cfb.EnemyDetected)
        {
            Debug.Log("cfb componentStatic : " + cfb.componentStaticDetected);
            Debug.Log("cfb allied : " + cfb.AlliedDetected);
            Debug.Log("cfb enemy : " + cfb.EnemyDetected);
        }*/


        // Test DirectionnalSensing

        /*collisionFeedback cfb = DirectionnalSensing(new Vector3(-25f, 0f, 5f));
        string team = "blue";
        if (gameObject.GetComponent<TankIndexManager>().m_TeamIndex == 1) team = "red";
        if (cfb.componentStaticDetected || cfb.AllyDetected || cfb.EnemyDetected)
        {
            Debug.Log("cfb " + team + " : " + cfb.componentStaticDetected + " ; " + cfb.AllyDetected + " ; " + cfb.EnemyDetected);
        }*/



        // Test DetectDynamicObstacle

        /*float result = DetectDynamicObstacle();
        string team = "blue";
        if (gameObject.GetComponent<TankIndexManager>().m_TeamIndex == 1) team = "red";
        Debug.Log("cfb " + team + " : " + result);*/


        // Test DetectWhatSideFree

        /*float result = DetectDynamicObstacle();
        if (result < float.MaxValue - 1f)
        {
            WhatSide ws = DetectWhatSideFree();
            string team = "blue";
            if (gameObject.GetComponent<TankIndexManager>().m_TeamIndex == 1) team = "red";
            Debug.Log("ws " + team + " : " + ws);
        }*/
    }

    public struct cardinalsPointsCollisionFeedback
    {
        public Tuple<bool, float> componentStaticDetected;
        public Tuple<bool, float> AlliedDetected;
        public Tuple<bool, float> EnemyDetected;
    }

    // return :
    // componentStaticDetected -> true if detected
    // AlliedDetected -> true if detected before an componentStaticDetected and first tank detected
    // EnemyDetected -> true if detected before an componentStaticDetected and first tank detected
    public struct collisionFeedback
    {
        public bool componentStaticDetected;
        public bool AllyDetected;
        public bool EnemyDetected;
    }

    public enum WhatSide {
        left, right, nothing
    };

    public float DetectDynamicObstacle()
    {
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;
        
        Vector3 frontRightSensorGlobalPos = tankPos + tankRotation * m_FrontRightSensorLocalPos;
        Vector3 frontLeftSensorGlobalPos = tankPos + tankRotation * m_FackLeftSensorLocalPos;
        
        RaycastHit[] rightHits = Physics.RaycastAll(frontRightSensorGlobalPos, m_Transform.forward, m_RayCastFrontLength);
        RaycastHit[] leftHits = Physics.RaycastAll(frontLeftSensorGlobalPos, m_Transform.forward, m_RayCastFrontLength);

        cardinalsPointsCollisionFeedback frontRightTest = AnalysisCollisions(rightHits);
        cardinalsPointsCollisionFeedback frontLeftTest = AnalysisCollisions(leftHits);

        // Calcul tank nearest
        float ret = float.MaxValue;
        if (frontRightTest.AlliedDetected.Item1 && frontRightTest.AlliedDetected.Item2 < ret) ret = frontRightTest.AlliedDetected.Item2;
        if (frontRightTest.EnemyDetected.Item1 && frontRightTest.EnemyDetected.Item2 < ret) ret = frontRightTest.EnemyDetected.Item2;
        if (frontLeftTest.AlliedDetected.Item1 && frontLeftTest.AlliedDetected.Item2 < ret) ret = frontLeftTest.AlliedDetected.Item2;
        if (frontLeftTest.EnemyDetected.Item1 && frontLeftTest.EnemyDetected.Item2 < ret) ret = frontLeftTest.EnemyDetected.Item2;
        
        return ret;
    }

    public WhatSide DetectWhatSideFree()
    {
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;
        
        Vector3 frontRightSensorGlobalPos = tankPos + tankRotation * m_FrontRightSensorLocalPos;
        Vector3 frontLeftSensorGlobalPos = tankPos + tankRotation * m_FackLeftSensorLocalPos;

        Quaternion rightTurnRotation = Quaternion.Euler (0f, m_RayCastAnglesOfResearch, 0f); 
        Quaternion leftTurnRotation = Quaternion.Euler (0f, -m_RayCastAnglesOfResearch, 0f); 

        Vector3 rightSensorDirection = m_Transform.forward;
        Vector3 leftSensorDirection = m_Transform.forward;

        int nbOccurencesMax = 95 / Mathf.CeilToInt(m_RayCastAnglesOfResearch);
        for (int i = 0; i < nbOccurencesMax; i++)
        {
            // Send an raycast in right side of tank
            rightSensorDirection = rightTurnRotation * rightSensorDirection;
            List<cardinalsPointsCollisionFeedback> rightListCollisionFeedback = new List<cardinalsPointsCollisionFeedback>();
            
            RaycastHit[] frontRightHits = Physics.RaycastAll(frontRightSensorGlobalPos,  rightSensorDirection, m_RayCastFrontLength);
            rightListCollisionFeedback.Add(AnalysisCollisions(frontRightHits));

            collisionFeedback rightRaycastCollisionFeedback = AnalysisResultCardinalsPointCollisionFeedback(rightListCollisionFeedback);
            if (!rightRaycastCollisionFeedback.componentStaticDetected && !rightRaycastCollisionFeedback.AllyDetected && !rightRaycastCollisionFeedback.EnemyDetected)
            {
                return WhatSide.right;
            }
            
            // Send an raycast in left side of tank
            leftSensorDirection = leftTurnRotation * leftSensorDirection;
            List<cardinalsPointsCollisionFeedback> leftListCollisionFeedback = new List<cardinalsPointsCollisionFeedback>();

            RaycastHit[] frontLeftHits = Physics.RaycastAll(frontLeftSensorGlobalPos,  leftSensorDirection, m_RayCastFrontLength);
            leftListCollisionFeedback.Add(AnalysisCollisions(frontLeftHits));
            
            collisionFeedback leftRaycastCollisionFeedback = AnalysisResultCardinalsPointCollisionFeedback(leftListCollisionFeedback);
            if (!leftRaycastCollisionFeedback.componentStaticDetected && !leftRaycastCollisionFeedback.AllyDetected && !leftRaycastCollisionFeedback.EnemyDetected)
            {
                return WhatSide.left;
            }
        }

        return WhatSide.nothing;
    }

    public collisionFeedback DirectionnalSensing(Vector3 target)
    {
        gameObject.tag = "mySelf";
            
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;

        target += Vector3.up * m_FrontRightSensorLocalPos.y;

        Vector3 frontRightSensorGlobalPos = tankPos + tankRotation * m_FrontRightSensorLocalPos;
        Vector3 backRightSensorGlobalPos = tankPos + tankRotation * m_BackRightSensorLocalPos;
        Vector3 backLeftSensorGlobalPos = tankPos + tankRotation * m_BackLeftSensorLocalPos;
        Vector3 frontLeftSensorGlobalPos = tankPos + tankRotation * m_FackLeftSensorLocalPos;

        Vector3 frontRightDirection = target - frontRightSensorGlobalPos;
        Vector3 backRightDirection = target - frontRightSensorGlobalPos;
        Vector3 backLeftDirection = target - frontRightSensorGlobalPos;
        Vector3 frontLeftDirection = target - frontRightSensorGlobalPos;

        /*Debug.DrawLine(frontRightSensorGlobalPos, frontRightSensorGlobalPos + frontRightDirection, Color.red);
        Debug.DrawLine(backRightSensorGlobalPos, backRightSensorGlobalPos + backLeftDirection, Color.red);
        Debug.DrawLine(backLeftSensorGlobalPos, backLeftSensorGlobalPos + backLeftDirection, Color.red);
        Debug.DrawLine(frontLeftSensorGlobalPos, frontLeftSensorGlobalPos + frontLeftDirection, Color.red);*/

        RaycastHit[] frontRightHits = Physics.RaycastAll(frontRightSensorGlobalPos, frontRightDirection, frontRightDirection.magnitude);
        RaycastHit[] backRightHits = Physics.RaycastAll(backRightSensorGlobalPos, backRightDirection, backRightDirection.magnitude);
        RaycastHit[] backLeftHits = Physics.RaycastAll(backLeftSensorGlobalPos, backLeftDirection, backLeftDirection.magnitude);
        RaycastHit[] frontLeftHits = Physics.RaycastAll(frontLeftSensorGlobalPos, frontLeftDirection, frontLeftDirection.magnitude);
        
        List<cardinalsPointsCollisionFeedback> listCollisionFeedback = new List<cardinalsPointsCollisionFeedback>();
        listCollisionFeedback.Add(AnalysisCollisions(frontRightHits));
        listCollisionFeedback.Add(AnalysisCollisions(backRightHits));
        listCollisionFeedback.Add(AnalysisCollisions(backLeftHits));
        listCollisionFeedback.Add(AnalysisCollisions(frontLeftHits));
        
        gameObject.tag = "Tank";
        
        return AnalysisResultCardinalsPointCollisionFeedback(listCollisionFeedback);
    }

    public collisionFeedback BackwardSensing()
    {
        Vector3 tankPos = m_Transform.position;
        Quaternion tankRotation = m_Transform.rotation;
        
        Vector3 backRightSensorGlobalPos = tankPos + tankRotation * m_BackRightSensorLocalPos;
        Vector3 backLeftSensorGlobalPos = tankPos + tankRotation * m_BackLeftSensorLocalPos;
        
        RaycastHit[] rightHits = Physics.RaycastAll(backRightSensorGlobalPos, -m_Transform.forward, m_RayCastBackLength);
        RaycastHit[] leftHits = Physics.RaycastAll(backLeftSensorGlobalPos, -m_Transform.forward, m_RayCastBackLength);
        
        List<cardinalsPointsCollisionFeedback> listCollisionFeedback = new List<cardinalsPointsCollisionFeedback>();
        listCollisionFeedback.Add(AnalysisCollisions(rightHits));
        listCollisionFeedback.Add(AnalysisCollisions(leftHits));
        
        return AnalysisResultCardinalsPointCollisionFeedback(listCollisionFeedback);
    }

    public collisionFeedback AnalysisResultCardinalsPointCollisionFeedback(List<cardinalsPointsCollisionFeedback> listCollisionFeedback)
    {
        collisionFeedback ret;
        ret.componentStaticDetected = false;
        ret.AllyDetected = false;
        ret.EnemyDetected = false;

        float distanceComponentStatic = -2f;
        foreach (var collisionFeedback in listCollisionFeedback)
        {
            if (collisionFeedback.componentStaticDetected.Item1 && ((distanceComponentStatic >= -2.01f && distanceComponentStatic <= -1.99f) || collisionFeedback.componentStaticDetected.Item2 < distanceComponentStatic))
            {
                distanceComponentStatic = collisionFeedback.componentStaticDetected.Item2;
                ret.componentStaticDetected = true;
            }
        }
        
        float distanceAlliedTank = -2f;
        float distanceEnemyTank = -2f;
        foreach (var collisionFeedback in listCollisionFeedback)
        {
            if (collisionFeedback.AlliedDetected.Item1 && ((distanceAlliedTank >= -2.01f && distanceAlliedTank <= -1.99f) || collisionFeedback.AlliedDetected.Item2 < distanceAlliedTank))
            {
                distanceAlliedTank = collisionFeedback.AlliedDetected.Item2;
            }
            
            if (collisionFeedback.EnemyDetected.Item1 && ((distanceEnemyTank >= -2.01f && distanceEnemyTank <= -1.99f) || collisionFeedback.EnemyDetected.Item2 < distanceEnemyTank))
            {
                distanceEnemyTank = collisionFeedback.EnemyDetected.Item2;
            }
        }

        if (distanceAlliedTank >= 0 && (distanceEnemyTank <= 0 || distanceAlliedTank < distanceEnemyTank) && (distanceComponentStatic <= 0 || distanceAlliedTank < distanceComponentStatic))
        {
            ret.AllyDetected = true;
        }
        if (distanceEnemyTank >= 0 && (distanceAlliedTank <= 0 || distanceEnemyTank < distanceAlliedTank) && (distanceComponentStatic <= 0 || distanceEnemyTank < distanceComponentStatic))
        {
            ret.EnemyDetected = true;
        }

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
                if (hit.collider.gameObject.tag.Equals("BlockerEnvironment"))
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
                else if (hit.collider.gameObject.tag.Equals("BlockerEnvironment") && hit.distance >= smallestDistanceWithComponentStatic - 0.1f && hit.distance <= smallestDistanceWithComponentStatic)
                {
                    //Debug.Log("Collision detected ! " + hit.distance);
                    cfb.componentStaticDetected = new Tuple<bool, float>(true, hit.distance);
                }
            }
        }

        return cfb;
    }
}
