using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeGenerator : MonoBehaviour {

	/* Class controls the Cube generation. Main features:
	 * - display cubes in a grid
	 * - utilise old cubes to save resources
	 */

	[Header("Variables for Inspector")]
	[SerializeField] RectTransform _layout;
	[SerializeField] GameObject _cubePrefab;
	[SerializeField] Vector3 _offset;
	[SerializeField] int _maxGridX, _maxGridY;

	int _currentX = 0, _currentY = 0;
	Queue<GameObject> _cubeQueue = new Queue<GameObject>();

	private void Start()
	{
		//Subscribe and react to even second events
		InterfaceController.instance.eEvenSecond += CubeTick;
	}

	/// <summary>
	/// Create and position a cube on Canvas
	/// </summary>
	void CubeTick()
	{
		//Prepare a cube and assign a new position on the layout
		Transform cube = CreateOrReuseCube().transform;
		cube.position = new Vector3(_offset.x * _currentX, _offset.y * _currentY, 0) + _layout.position - new Vector3(_layout.rect.size.x/2, -_layout.rect.size.y/2, 0);

		//Prepare next position on grid
		_currentX++;
		if(_currentX > _maxGridX)
		{
			_currentX = 0;
			_currentY++;
			if (_currentY > _maxGridY)
				_currentY = 0;
		}
	}

	GameObject CreateOrReuseCube()
	{
		if (_cubeQueue.Count < 10)
		{
			//Instantiate a new cube and save it as newest
			GameObject go = Instantiate(_cubePrefab, _layout, false);
			_cubeQueue.Enqueue(go);
			go.name = "Cube " + _cubeQueue.Count;
			return go;
		}
		else
		{
			//Reuse oldest cube and save it as newest
			GameObject go = _cubeQueue.Dequeue();
			_cubeQueue.Enqueue(go);
			return go;
		}
	}

	private void OnDestroy()
	{
		//Cleanup if object is destroyed
		InterfaceController.instance.eEvenSecond -= CubeTick;
	}
}
