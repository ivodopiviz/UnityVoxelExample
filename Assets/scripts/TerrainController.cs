/*************************************************************************/
/* Copyright (c) 2016 Iván Vodopiviz					                 */
/*                                                                       */
/* Permission is hereby granted, free of charge, to any person obtaining */
/* a copy of this software and associated documentation files (the       */
/* "Software"), to deal in the Software without restriction, including   */
/* without limitation the rights to use, copy, modify, merge, publish,   */
/* distribute, sublicense, and/or sell copies of the Software, and to    */
/* permit persons to whom the Software is furnished to do so, subject to */
/* the following conditions:                                             */
/*                                                                       */
/* The above copyright notice and this permission notice shall be        */
/* included in all copies or substantial portions of the Software.       */
/*                                                                       */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,       */
/* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF    */
/* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.*/
/* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY  */
/* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,  */
/* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE     */
/* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                */
/*************************************************************************/

using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour 
{
	public GameObject CubeGrass;
	public GameObject CubeRock;
	public GameObject CubeSand;

	private Chunk _currentChunk;

	void Start () 
	{
		_currentChunk = new Chunk();
		_currentChunk.init();
		fillChunk();
		addChunk();
	}

	void fillChunk()
	{
		int maxHeight = _currentChunk.sizeY;
		int octaveCount = 5;
		int _curHeight;
		
		float[][] perlinNoiseArray = new float[_currentChunk.sizeX][];

		perlinNoiseArray = PerlinNoise.GeneratePerlinNoise(_currentChunk.sizeX, _currentChunk.sizeZ, octaveCount);
		
		for(int i = 0; i < _currentChunk.sizeX; i++)
		{
			for(int k = 0; k < _currentChunk.sizeZ; k++)
			{
				_curHeight = (int)(maxHeight * Mathf.Clamp01(perlinNoiseArray[i][k]));

				for(int j = 0; j < _curHeight; j++)
				{
					if(j == _curHeight - 1)
						_currentChunk.blocksArray[i,j,k] = 1; // Grass
					else if(j == _curHeight - 2)
						_currentChunk.blocksArray[i,j,k] = 3; // Sand
					else
						_currentChunk.blocksArray[i,j,k] = 2; // Rock
					
					if(j > 8)
						_currentChunk.blocksArray[i,j,k] = 2; // Grass
					
				}
			}
		}
	}

	void addChunk()
	{
		for(int i = 0; i < _currentChunk.sizeX; i++)
		{
			for(int j = 0; j < _currentChunk.sizeY; j++)
			{
				for(int k = 0; k < _currentChunk.sizeZ; k++)
				{
					if(_currentChunk.blocksArray[i,j,k] != 0)
					{
						createCube(i,j,k, _currentChunk.blocksArray[i,j,k]);
					}
				}
			}
		}
	}
	
	void createCube(int _posX, int _posY, int _posZ, int _blockType)
	{
		GameObject _currentCube = CubeRock;
		
		//1 = Grass, 2 = Rock, 3 = Sand
		if(_blockType == 1)
			_currentCube = CubeGrass;
		else if(_blockType == 2)
			_currentCube = CubeRock;
		else if(_blockType == 3)
			_currentCube = CubeSand;

		GameObject.Instantiate(_currentCube, new Vector3(_posX, _posY, _posZ), Quaternion.identity);
	}
}
