using System;
using MinD.SO.Game;
using UnityEditor;
using UnityEngine;

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
    
    
    private void OnDrawGizmos()
    {
        void ShowUserData(Vector3 position, UserInformationSo data)
        {
            Handles.Label(
                position,
                $"(Total Play Time:{data.totalPlayTime:F2}, DamageRatio:{data.damageRatio:F2}, HealingCount:{data.healingUsed})"
                );    
        }
        
        if (example == null)
        {
            return;
        }
        
        Vector3 predictedPosition = CalculatePosition(example);
        
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(predictedPosition, 0.5f);
        
        foreach (var reference in referenceUserInfo)
        {
            Gizmos.color = reference.color;
            Gizmos.DrawSphere(reference.targetPoint.position, 0.3f);
            Gizmos.DrawLine(reference.targetPoint.position, predictedPosition);
            
            Handles.color = reference.color;
            Handles.Label(reference.targetPoint.position + HandleUtility.GetHandleSize(reference.targetPoint.position) * Vector3.up * 0.5f, $"{reference.userInformationSo.name}\n Weight: {reference.weight:F2}");   
            ShowUserData(reference.targetPoint.position + HandleUtility.GetHandleSize(reference.targetPoint.position) * Vector3.up * 0.2f, reference.userInformationSo);

        }
        
//        if (referenceUserInfo != null && referenceUserInfo.Length > 0)
//        {
//            Vector3 predictedPosition = Vector3.zero;
//
//            // 입력 벡터와 유사도를 기반으로 최종 위치 도출
//            if (example != null)
//            {
//                UserInformationSo userInfo = example; // 임시로 첫 번째 사용자 정보 사용
//                predictedPosition = CalculatePosition(userInfo);
//            }
//
//            // predictedPosition을 그리기 전에, 각 Reference와 그려지는 선의 색상을 계산
//            foreach (var reference in referenceUserInfo)
//            {
//                // Reference와 예측된 위치 간에 선을 그리기 전에, 가중치와 색상 계산
//                Gizmos.color = reference.color; // 각 reference의 색상
//                Gizmos.DrawWireSphere(reference.targetPoint.position, 0.5f); // targetPoint의 위치에 원 그리기
//
//                // 예측된 위치와 연결된 선 그리기 (색상 보간)
//                Color lineColor = Color.Lerp(reference.color, Color.green, reference.weight); // 가중치에 따른 색상 보간
//                Gizmos.color = lineColor;
//                Gizmos.DrawLine(reference.targetPoint.position, predictedPosition); // 예측된 위치와 연결되는 선 그리기
//
//                // 참조 위치 근처에 텍스트로 색상/가중치/데미지 효율성 표시
//                Handles.Label(reference.targetPoint.position + Vector3.up, $"{reference.userInformationSo.name}\n Weight: {reference.weight:F2}");
//            }
//
//            // 예측된 위치 표시
//            Gizmos.color = new Color(0, 1, 0, 0.3f); // 예측된 위치는 초록색
//            Gizmos.DrawSphere(predictedPosition, HandleUtility.GetHandleSize(predictedPosition) * 0.3f); // 예측된 위치에 구 그리기
//
//            Handles.Label(predictedPosition + Vector3.up * HandleUtility.GetHandleSize(predictedPosition) * 1f, $"Predicted Position"); // 예측된 위치 레이블
//        }
    }
}
