using System;
using System.Collections.Generic;
using Core.Infrastructure.Enums;
using Core.Mono;
using Core.Node.Panel;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;

namespace Core.Mechanics.Gesture
{
    public class RecognitionPicture : MonoBehaviour
    {
        [SerializeField] private RectTransform pointsParent;
        [SerializeField] private float pointsAccuracy;
        [SerializeField] private float deltaAccuracy;

        private ScriptableGameSettings _gameSettings;

        private List<RectTransform> _picturePoints;

        public void Init(ScriptableGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _picturePoints = new List<RectTransform>(pointsParent.childCount - 1);
            
            for(var i = 0; i < pointsParent.childCount; i++)
            {
                var pictureHolder = pointsParent.GetChild(i).GetComponent<PointHolder>();
                
                if (pictureHolder)
                {
                    _picturePoints.Add(pictureHolder.RectTransform);
                }
            }
        }

        public float Recognize(List<Vector3> points)
        {
            int pointsCompleted = 0;
            int totalCount = 1;

            for (int i = 0; i < _picturePoints.Count - 1; i++)
            {
                var checkPoints = new List<Vector2>();
                var plusVector = _picturePoints[i+1].anchoredPosition - _picturePoints[i].anchoredPosition;
                
                if (plusVector.magnitude > 2 * pointsAccuracy)
                {
                    var count = Mathf.CeilToInt(plusVector.magnitude / pointsAccuracy);
                    totalCount += count;
                    
                    for (int j = 0; j < count + 1; j++)
                    {
                        checkPoints.Add(_picturePoints[i].anchoredPosition + plusVector * j / (count - 1));
                    }
                }
                else
                {
                    checkPoints.Add(_picturePoints[i+1].anchoredPosition);
                    totalCount ++;
                }

                for (int j = 0; j < checkPoints.Count - 1; j++)
                {
                    foreach (var pointWorld in points)
                    {
                        var temp = transform.InverseTransformPoint(pointWorld);
                        var point = new Vector2(temp.x, temp.y);
                        
                        var vectorAB = checkPoints[j+1] - checkPoints[j];
                        var vectorAC = point - checkPoints[j];
                        var vectorBC = point - checkPoints[j+1];
                
                        var sin = Mathf.Sin(Vector3.Angle(vectorAB, vectorAC) * Mathf.PI / 180f);
                        var delta = vectorAC.magnitude * sin;
                        
                        if(vectorAC.magnitude > vectorAB.magnitude || vectorBC.magnitude > vectorAB.magnitude)
                        {
                            continue;
                        }
                        
                        if (delta < deltaAccuracy)
                        {
                            pointsCompleted++;
                            break;
                        }
                    }
                }
            }
            
            return pointsCompleted / (totalCount - 1f);
        }
        public void Activate(NodeData moveNodeData, Action callback)
        {
            if(moveNodeData.arrivalType == ArrivalType.Instant)
            {
                transform.localPosition = moveNodeData.spritePosition;
                transform.localScale = new Vector3(moveNodeData.scaleAndRotation.x,
                    moveNodeData.scaleAndRotation.y, 1);
                callback.Invoke();
            }
            else if(moveNodeData.arrivalType == ArrivalType.ShowUp)
            {
                transform.localPosition = moveNodeData.spritePosition;
                transform.localScale = new Vector3(0, 0, 1);
                transform.DOScale(new Vector3(moveNodeData.scaleAndRotation.x, moveNodeData.scaleAndRotation.y, 1),
                    _gameSettings.bubbleShowUpDuration).OnComplete( 
                    () =>
                    {
                        callback?.Invoke();
                    });
            }
            else
            {
                var startPosition = moveNodeData.spritePosition + moveNodeData.movePosition;
                transform.localPosition = startPosition;
                transform.DOLocalMove(new Vector3(moveNodeData.spritePosition.x, moveNodeData.spritePosition.y, 0),
                    _gameSettings.dialogScreenArrivalMoveDuration).OnComplete(() =>
                {
                    callback?.Invoke();
                });
            }
        }
    }
}