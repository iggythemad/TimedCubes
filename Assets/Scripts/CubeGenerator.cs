using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeGenerator : MonoBehaviour {

	[SerializeField] RectTransform layout;
	[SerializeField] GameObject cubePrefab;
	[SerializeField] Vector3 offset;
	[SerializeField] int maxGridX, maxGridY;
	int currentX = 0, currentY = 0;
	Queue<GameObject> cubeQueue = new Queue<GameObject>();

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
		//Prepare a cube and assign a new position
		Transform cube = CreateOrReuseCube().transform;
		cube.position = new Vector3(offset.x * currentX, offset.y * currentY, 0) + layout.position - new Vector3(layout.rect.size.x/2, -layout.rect.size.y/2, 0);

		//Prepare next position on grid
		currentX++;
		if(currentX > maxGridX)
		{
			currentX = 0;
			currentY++;
			if (currentY > maxGridY)
				currentY = 0;
		}
	}

	GameObject CreateOrReuseCube()
	{
		if (cubeQueue.Count < 10)
		{
			//Instantiate a new cube and save it as newest
			GameObject go = Instantiate(cubePrefab, layout, false);
			cubeQueue.Enqueue(go);
			go.name = "Cube " + cubeQueue.Count;
			return go;
		}
		else
		{
			//Reuse oldest cube and save it as newest
			GameObject go = cubeQueue.Dequeue();
			cubeQueue.Enqueue(go);
			return go;
		}
	}

	private void OnDestroy()
	{
		//Cleanup if object is destroyed
		InterfaceController.instance.eEvenSecond -= CubeTick;
	}
}
