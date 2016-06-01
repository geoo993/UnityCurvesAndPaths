using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwirlPipeSystem : MonoBehaviour 
{

	public SwirlPipe pipePrefab;

	public int pipeCount;

	private SwirlPipe[] pipes;

	private void Awake () 
	{
		
		pipes = new SwirlPipe[pipeCount];
		for (int i = 0; i < pipeCount; i++) 
		{
			pipes[i] = Instantiate<SwirlPipe>(pipePrefab);
			SwirlPipe pipe = pipes[i];

			pipe.transform.SetParent(transform, false);

			if (i > 0) 
			{
				pipe.AlignWith(pipes[i - 1]);
			}
		}


	}



}

