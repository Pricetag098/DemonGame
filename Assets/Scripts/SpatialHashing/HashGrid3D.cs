using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BlakesSpatialHash
{
    public class HashGrid3D<T> where T : AiAgent
    {
        public Dictionary<uint, List<T>> cells;
        public List<Vector3> cellPositions = new List<Vector3>();

        /// <summary>
        /// Max Amount of Hashable Cells
        /// </summary>
        public const uint CellCountMax = 20000; // going over this cell count wont cause errors but will cause OBJECTS to be hashed into duplicate cells
        public uint CellCount { get { return CellCountMax; } }

        private Vector3[] offsets;
        private Vector3 CellSize;
        
        public HashGrid3D(Vector3 cellSize)
        {
            CellSize = cellSize;

            cells = new Dictionary<uint, List<T>>((int)CellCountMax);

            for (uint i = 0; i < CellCountMax; i++)
            {
                cells[i] = new List<T>();
            }

            offsets = new Vector3[26];
            // bottom 9 of the 3x3x3
            offsets[0] = new Vector3(-CellSize.x, -CellSize.y, -CellSize.z);    // 1x1x1
            offsets[1] = new Vector3(-CellSize.x, -CellSize.y, 0);              // 1x1x2
            offsets[2] = new Vector3(-CellSize.x, -CellSize.y, CellSize.z);     // 1x1x3
            offsets[3] = new Vector3(0, -CellSize.y, -CellSize.z);              // 2x1x1
            offsets[4] = new Vector3(0, -CellSize.y, 0);                        // 2x1x2
            offsets[5] = new Vector3(0, -CellSize.y, CellSize.z);               // 2x1x3
            offsets[6] = new Vector3(CellSize.x, -CellSize.y, -CellSize.z);     // 3x1x1
            offsets[7] = new Vector3(CellSize.x, -CellSize.y, 0);               // 3x1x2
            offsets[8] = new Vector3(CellSize.x, -CellSize.y, CellSize.z);      // 3x1x3

            // middle  8 of the 3x3x3, excluding 2x2x2
            offsets[9] = new Vector3(-CellSize.x, 0, -CellSize.z);              // 1x2x1
            offsets[10] = new Vector3(-CellSize.x, 0, 0);                       // 1x2x2
            offsets[11] = new Vector3(-CellSize.x, 0, CellSize.z);              // 1x2x3
            offsets[12] = new Vector3(0, 0, -CellSize.z);                       // 2x2x1
            offsets[13] = new Vector3(0, 0, CellSize.z);                        // 2x2x3
            offsets[14] = new Vector3(CellSize.x, 0, -CellSize.z);              // 3x2x1
            offsets[15] = new Vector3(CellSize.x, 0, 0);                        // 3x2x2
            offsets[16] = new Vector3(CellSize.x, 0, CellSize.z);               // 3x2x3

            // top 9 of the 3x3x3
            offsets[17] = new Vector3(-CellSize.x, CellSize.y, -CellSize.z);    // 1x3x1
            offsets[18] = new Vector3(-CellSize.x, CellSize.y, 0);              // 1x3x2
            offsets[19] = new Vector3(-CellSize.x, CellSize.y, CellSize.z);     // 1x3x3
            offsets[20] = new Vector3(0, CellSize.y, -CellSize.z);              // 2x3x1
            offsets[21] = new Vector3(0, CellSize.y, 0);                        // 2x3x2
            offsets[22] = new Vector3(0, CellSize.y, CellSize.z);               // 2x3x3
            offsets[23] = new Vector3(CellSize.x, CellSize.y, -CellSize.z);     // 3x3x1
            offsets[24] = new Vector3(CellSize.x, CellSize.y, 0);               // 3x3x2
            offsets[25] = new Vector3(CellSize.x, CellSize.y, CellSize.z);      // 3x3x3
        }

        /// <summary>
        /// Initalise OBJECT into Spatial Hash Grid
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(T obj)
        {
            return;
            uint index = obj.Index = GetIndex(obj.GetPosition, out Vector3Int cellPos);

            //if (!cells.ContainsKey(index)) // PERFORMANCE CAN BE GAINED BY GETTING REF TO DICTIONARY VALUE (dont think unity allows unsafe methods)
            //{
            //    CurrentCellCount++;

            //    cells.Add(index, new List<T>());

            //    cellPositions.Add(cellPos); // for drawing cells
            //}

            cells[index].Add(obj);

            if (!cellPositions.Contains(cellPos))
            {
                cellPositions.Add(cellPos); // for drawing cells
            }
        }

        /// <summary>
        /// Removes OBJECT from its current Cell
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(T obj)
        {
            return;
            cells[obj.Index].Remove(obj);
        }

        /// <summary>
        /// Returns all OBJECTS in Current and Touching Cells
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<T> GetNearbySurroundingObjects(T obj)
        {
            List<T> tempList = new List<T>();

            if(obj.Enabled == false) return tempList;

            // Cache object values for performance
            Vector3 position = obj.GetPosition;
            uint index = obj.Index;

            // Get all objects in current cell & removes current obj
            tempList.AddRange(cells[index]);
            tempList.Remove(obj);

            //gets all objects in surrounding cells
            for (int i = 0; i < 26; i++)
            {
                uint tempIndex = GetIndex(position + offsets[i], out Vector3Int pos);

                if (index == tempIndex) { continue; }

                if (cells.TryGetValue(tempIndex, out List<T> cellList))
                {
                    int count = cellList.Count;

                    if (count == 0) continue;

                    for (int j = 0; j < count; j++)
                    {
                        T Object = cellList[j];

                        if (Object.Enabled == true)
                        {
                            tempList.Add(Object);
                        }
                    }
                }
            }

            return tempList;
        }

        /// <summary>
        /// Called once per frame Updates the Cell Index Value
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateIndex(T obj)
        {
            if(obj.Enabled == false) return;

            Vector3 position = obj.GetPosition;

            if (position == obj.GetLastPosition) return;

            uint index = obj.Index;

            uint newIndex = GetIndex(position, out Vector3Int cellPos);

            if(index != newIndex)
            {
                //bool Item = cells.TryGetValue(newIndex, out var cell);

                //if(Item == false) // if cell doesnt exist create it
                //{
                //    CurrentCellCount++;
                //    cells.Add(newIndex, new List<T>());

                //    cellPositions.Add(cellPos); // for drawing cells

                //    //Debug.Log("Current Cell Count: " + CurrentCellCount);
                //}

                cells[index].Remove(obj);
                cells[newIndex].Add(obj);
                
                obj.Index = newIndex;
                obj.GetLastPosition = position;

                if(!cellPositions.Contains(cellPos))
                {
                    cellPositions.Add(cellPos); // for drawing cells
                }
            }
        }

        public List<T> UpdateObjectAndGetSurroudingObjects(T obj)
        {
            UpdateIndex(obj);
            return GetNearbySurroundingObjects(obj);
        }

        private Vector3Int PositionToCellCoord(Vector3 point, Vector3 cellSize, out Vector3Int pos)
        {
            int cellX = FastFloor(point.x / cellSize.x);
            int cellY = FastFloor(point.y / cellSize.y);
            int cellZ = FastFloor(point.z / cellSize.z);

            return pos = new Vector3Int(cellX, cellY, cellZ);
        }


        /// <summary>
        /// Hashes a Vector3Int
        /// </summary>
        /// <param name="cellCoord"></param>
        /// <returns></returns>
        private uint HashCell(Vector3Int cellCoord) 
        {
            var hash = Hash128.Compute(ref cellCoord);
            return (uint)hash.GetHashCode();
        }


        private uint GetIndexFromHash(uint num)
        {
            return num % CellCountMax; // this can be slow for larger numbers
        }

        private uint GetIndex(Vector3 point, out Vector3Int cellPosition)
        {
            uint temp = GetIndexFromHash(HashCell(PositionToCellCoord(point, CellSize, out cellPosition)));
            return temp;
        }

        // Better implementation of Floor, which boosts the floor performance greatly
        private const int _BIG_ENOUGH_INT = 16 * 1024;
        private const double _BIG_ENOUGH_FLOOR = _BIG_ENOUGH_INT + 0.0000;

        private static int FastFloor(float f)
        {
            return (int)(f + _BIG_ENOUGH_FLOOR) - _BIG_ENOUGH_INT;
        }
    }
}