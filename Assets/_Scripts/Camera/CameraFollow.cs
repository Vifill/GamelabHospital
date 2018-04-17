using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	private class Square
	{
		public float minX = float.MaxValue;
		public float maxX = float.MinValue;
		public float minZ = float.MaxValue;
		public float maxZ = float.MinValue;

		public Vector3 center;
	}

	[SerializeField]
	private Vector2 CameraCage = new Vector2();
	[SerializeField]
	private Vector2 Offset = new Vector2 ();
	[SerializeField]
	private float Speed = 1;
	[SerializeField]
	private float MaxTraveldistance = 1;
	[SerializeField]
	private float CameraOffset = 9;


	private ActionableActioner[] Players;
	private Vector3 LevelCenter;

	private void Start ()
	{
		Players = FindObjectsOfType <ActionableActioner>();

	}

	private void LateUpdate ()
	{
		Move();
	}

	private void Move()
	{
		LevelCenter = new Vector3 (Offset.x, 1, Offset.y);
		Vector3 targetPosition = GetGroupCenter() - LevelCenter;

		if(targetPosition.sqrMagnitude > MaxTraveldistance * MaxTraveldistance)
		{
			targetPosition = targetPosition.normalized * MaxTraveldistance;
		}
		targetPosition += LevelCenter;

		targetPosition.y = transform.position.y;
		targetPosition.z -= CameraOffset;

		transform.position = Vector3.Lerp(transform.position, targetPosition, Speed * Time.deltaTime);
	}

	private Vector3 GetGroupCenter()
	{
		Square square = GetGroupSquare();
		Vector3 center = Vector3.zero;
        int length = Players?.Length ?? 0;

        if (length > 1) 
		{
			Vector2 leftTopCorner = new Vector2(square.minX,square.maxZ);
			Vector2 rightBottomCorner = new Vector2(square.maxX, square.minZ);
			Vector2 diaCenter = (rightBottomCorner - leftTopCorner) / 2 + leftTopCorner;

			center = new Vector3(diaCenter.x , 1 ,diaCenter.y);
		}
		else if (length == 1)
		{
			center = Players[0].transform.position;
			center.y = 1f;
		}
		return center;
	}

	private Vector3 GetGroupRectangleSize()
	{
		Square square = GetGroupSquare ();
		Vector3 size;
        int length = Players?.Length ?? 0;

        if (length > 1) 
		{
			float sizeX =  Mathf.Abs(square.maxX - square.minX);
			float sizeZ =  Mathf.Abs(square.maxZ - square.minZ);
			size = new Vector3 (sizeX, 1 ,sizeZ);
		} 
		else 
		{
			size = new Vector3 (2,1,2);
		}

		return size;
	}

	private Square GetGroupSquare()
	{
		Square square = new Square();
        int length = Players?.Length ?? 0;
        //make rectangle out of extremes
        for (int i = 0; i < length; i++)
        { 

            Vector3 playerpos = Vector3.zero;
            if (Players[i] != null)
            {
                playerpos = Players[i]?.transform?.position ?? Vector3.zero;
            }
         
			//Vector3 playerpos = Players[i]?.transform?.position ?? Vector3.zero;

			if (playerpos.x <= square.minX)
				square.minX = playerpos.x;

			if (playerpos.x > square.maxX)
				square.maxX = playerpos.x;

			if (playerpos.z <= square.minZ)
				square.minZ = playerpos.z;

			if (playerpos.z > square.maxZ) 
				square.maxZ = playerpos.z;
		}
		return square;
	}

	private void OnDrawGizmos()
	{

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(new Vector3(Offset.x, 1 , Offset.y), new Vector3(CameraCage.x, 1,CameraCage.y));

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(GetGroupCenter(),GetGroupRectangleSize());
		Gizmos.DrawWireSphere (GetGroupCenter(), 0.5f);
	}
}