using System.Collections.Generic;
using Core.Base.Classes;
using Core.Infrastructure.Services;
using Core.Infrastructure.Utils;
using Scriptables.Settings;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace Core.Mechanics.Gesture
{
    public class GestureMechanics : BaseMechanics
    {
	    [SerializeField] private RecognitionPicture recognitionPicture;
	    [SerializeField] private RectTransform drawArea;
	    [SerializeField] private Button button;
	    
	    [SerializeField] private float distanceBetweenPoints = 10f;
	    [SerializeField] private int minimumPointsToRecognize = 10;
	    [SerializeField] private float accuracy;

	    [SerializeField] private Material lineMaterial;
	    [SerializeField] private float startThickness = 0.1f;
	    [SerializeField] private Color startColor = new(0, 0.67f, 1f);
	    [SerializeField] private Color endColor = new(0.48f, 0.83f, 1f);

	    private readonly List<Vector3> _worldPoints = new();
	    private readonly List<GameObject> _strokes = new();
	   
	    private LineRenderer _currentStrokeRenderer;

	    private Vector2 _lastPoint = Vector2.zero;
	    private bool _strokeStarted;
	    private float _result;
	    
	    private Camera _mainCamera;
	    
	    [Inject]
	    public void Construct(Camera mainCamera, ScriptableGameSettings gameSettings)
	    {
		    _mainCamera = mainCamera;
		    
		    recognitionPicture.Init(gameSettings);
		    button.onClick.AddListener(Recognize);
	    }

	    public override void ManualUpdate()
	    {
		    if (IsBusy)
		    {
			    return;
		    }

		    var input = InputService.GetInput();
			    
		    CheckLine(input);
	    }

	    private void CheckLine(InputService.Input input)
	    {
		    var point = input.Position;
		    
		    if (RectTransformUtility.RectangleContainsScreenPoint(drawArea, point, _mainCamera))
		    {
			    if(InputService.GetInput().InputType == InputService.InputType.None)
			    {
				    _strokeStarted = false;
			    }
			    else
			    {
				    if (!_strokeStarted)
				    {
					    _strokeStarted = true;
					    _lastPoint = Vector2.zero;
					    
					    AddStroke();
				    }
				    else if (Vector2.Distance(point, _lastPoint) > distanceBetweenPoints)
				    {
					    _lastPoint = point;

					    var localPosition = new Vector3(
						    point.x - (float)Screen.width / 2,
						    point.y - (float)Screen.height / 2,
						    -30);
					    var worldPosition = transform.TransformPoint(localPosition);
					    
					    _worldPoints.Add(worldPosition);

					    var positionCount = _currentStrokeRenderer.positionCount;
					    positionCount++;
					    _currentStrokeRenderer.positionCount = positionCount;
					    _currentStrokeRenderer.SetPosition(positionCount - 1, worldPosition);
					    _currentStrokeRenderer.sortingOrder = 2;
				    }
			    }
		    }
		    else
		    {
			    _strokeStarted = false;
		    }
	    }

	    private void Recognize()
	    {
		    if (_worldPoints.Count > minimumPointsToRecognize)
		    {
			    _result = recognitionPicture.Recognize(_worldPoints);
			    this.Log("Result of gesture: " + _result);
			    
			    if (_result  > accuracy)
			    {
				    Deactivate();
			    }
		    }
	    }
	    private void AddStroke()
	    {
		    GameObject newStroke = new GameObject
		    {
			    transform =
			    {
				    parent = recognitionPicture.transform
			    }
		    };

		    _currentStrokeRenderer = newStroke.AddComponent<LineRenderer>();
		    _currentStrokeRenderer.positionCount = 0;
		    _currentStrokeRenderer.material = lineMaterial;
		    _currentStrokeRenderer.startColor = startColor;
		    _currentStrokeRenderer.endColor = endColor;
		    _currentStrokeRenderer.startWidth = startThickness;
		    _currentStrokeRenderer.endWidth = startThickness;
		    _currentStrokeRenderer.useWorldSpace = false;
		    _currentStrokeRenderer.shadowCastingMode = ShadowCastingMode.Off;
		    _currentStrokeRenderer.receiveShadows = false;
		    _currentStrokeRenderer.transform.localScale = new Vector3(_currentStrokeRenderer.transform.localScale.x, _currentStrokeRenderer.transform.localScale.y, 1);
		    _strokes.Add(newStroke);
	    }

	    protected override void Deactivate()
	    {
		    if (recognitionPicture == null)
		    {
			    return;
		    }
		    
		    base.Deactivate();
	    }
    }
}