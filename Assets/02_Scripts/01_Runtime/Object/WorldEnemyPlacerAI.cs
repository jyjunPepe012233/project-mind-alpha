using System;
using MinD.SO.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldEnemyPlacerAI : MonoBehaviour
{
    
    [Serializable]
    public struct ReferenceUserInfo
    {
        public Color color;
        public UserInformationSo userInformationSo;
        public float weight;
        public Transform targetPoint;
    }
    
    [SerializeField] private GameObject enemyPrefab; 
    [SerializeField] private ReferenceUserInfo[] referenceUserInfo;

    [Header("[ Editor ]")] 
    public UserInformationSo example;
    
    
    

    private Vector3 CalculatePosition(UserInformationSo userInfo)
    {
        float[] inputVector = userInfo.GetDataForReference();
        Vector3 weightedPositionSum = Vector3.zero;
        float totalWeight = 0f;

        foreach (var reference in referenceUserInfo)
        {
            if (reference.userInformationSo == null || reference.targetPoint == null)
            {
                continue;
            } 
            
            float[] refVec = reference.userInformationSo.GetDataForReference();
            float similarity = CalculateCosineSimilarity(inputVector, refVec);
            similarity = Mathf.Pow(similarity, 4);
            Debug.Log(similarity);
            float weightedSimilarity = similarity * reference.weight;

            weightedPositionSum += reference.targetPoint.position * weightedSimilarity;
            totalWeight += weightedSimilarity;
        }
        
        var result = totalWeight == 0 ? Vector3.zero : weightedPositionSum / totalWeight;
        return result;
    }

    private float CalculateCosineSimilarity(float[] a, float[] b)
    {
        float dot = 0;
        float magA = 0;
        float magB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        if (magA == 0 || magB == 0)
        {
            return 0;
        }

        return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB));
    }
    
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        void ShowUserData(Vector3 position, UserInformationSo data, GUIStyle style)
        {
            Handles.Label(
                position,
                $"(Total Play Time:{data.totalPlayTime:F2}, DamageRatio:{data.damageRatio:F2}, HealingCount:{data.healingUsed})",
                style
                );
        }
        
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        
        if (example == null)
        {
            return;
        }
        
        Vector3 predictedPosition = CalculatePosition(example);
        
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(predictedPosition, 0.5f);
        Handles.Label(predictedPosition + HandleUtility.GetHandleSize(predictedPosition) * Vector3.up * 0.45f, "Predicted Position", style);

        
        foreach (var reference in referenceUserInfo)
        {
            Gizmos.color = reference.color;
            Gizmos.DrawSphere(reference.targetPoint.position, 0.3f);
            Gizmos.DrawLine(reference.targetPoint.position, predictedPosition);
            
            Handles.color = reference.color;
            style.normal.textColor = reference.color;
            Handles.Label(Vector3.Lerp(predictedPosition, reference.targetPoint.position, 0.5f), $"Weight: {reference.weight:F2}", style); 
            
            Handles.Label(reference.targetPoint.position + HandleUtility.GetHandleSize(reference.targetPoint.position) * Vector3.up * 0.6f, $"{reference.userInformationSo.name}\n", style);   
            ShowUserData(reference.targetPoint.position + HandleUtility.GetHandleSize(reference.targetPoint.position) * Vector3.up * 0.5f, reference.userInformationSo, style);

        }
    }
    #endif
}
