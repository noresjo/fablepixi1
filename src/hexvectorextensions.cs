using System;
using System.Collections.Generic;
using System.Linq;
using Beta.Core;
using UnityEngine;
using Vectrosity;

namespace Beta.UnityExtensions
{
    public static class HexVectorExtensions
    {
        public static float SCALE = 40.0f;
        private static readonly float sqrt3 = (float)Math.Sqrt(3);
        /// <summary>
        /// Returns the x-coordinate of the hexagon, based on HX,HY,HZ; in 2d space with y downwards.
        /// </summary>
        public static float GetPositionX(this HexVector hex)
        {
            return (float)(hex.HX * sqrt3) * SCALE;
        }

        public static Vector2 GetVector2(this HexVector hex)
        {
            return new Vector2(hex.GetPositionX(), hex.GetPositionY());
        }

        /// <summary>
        /// Returns the y-coordinate of the hexagon, based on HX,HY,HZ; in 2d space with y downwards.
        /// </summary>
        public static float GetPositionY(this HexVector hex)
        {
            return (float)(hex.HZ - hex.HY) * SCALE;
        }

        public static Vector3 GetGuiPositionForHexVector(this HexVector hexVector, Vector3 gridPosition)
        {
            return hexVector.GetGuiPositionForHexVector(HexVector.UP_RIGHT, gridPosition);
        }

        public static Vector3 GetGuiPositionForHexVector(this HexVector hexVector, HexVector addedVector, Vector3 gridPosition)
        {
            var hexBase = hexVector.Add(addedVector);
            var guiScreenPosition = Camera.main.WorldToScreenPoint(
                new Vector3(hexBase.GetPositionX(), hexBase.GetPositionY(), 0) + gridPosition);
            guiScreenPosition.y = Screen.height - guiScreenPosition.y;

            return guiScreenPosition;
        }

        public static HexVector GetFromScreenPosition(Vector3 pos, Vector2 origo)
        {
            var x = (pos.x - origo.x);
            var y = (pos.y - origo.y);

            float hx = x / SCALE / sqrt3 / 1;
            float hy = (x / sqrt3 - y) / SCALE / 2f;
            float hz = (x / sqrt3 + y) / SCALE / 2f;

            var intHx = Mathf.RoundToInt(hx);
            var intHy = Mathf.RoundToInt(hy);
            var intHz = Mathf.RoundToInt(hz);

            var result = new HexVector(
                intHx,
                intHy,
                intHz);

            if (result.IsValid)
            {
                //Debug.Log("Found valid hexVector " + result);
                return result;
            }
            else
            {
                //Debug.Log("Found Invalid hexVector " + result + ", Will try automatic fix.");

                var absHx = Mathf.Abs(hx - intHx);
                var absHy = Mathf.Abs(hy - intHy);
                var absHz = Mathf.Abs(hz - intHz);

                if (absHx > absHy && absHx > absHz)
                {
                    intHx = intHy + intHz;
                }
                else if (absHy > absHx && absHy > absHx)
                {
                    intHy = intHx - intHz;
                }
                else
                {
                    intHz = intHx - intHy;
                }

                result = new HexVector(intHx, intHy, intHz);
                if (!result.IsValid)
                {
                    Debug.LogError("Still Invalid after fix " + result);
                }
                return result;
            }
        }

        public static IEnumerable<HexVector> GetNeighboursNotInRegion(this HexVector hex, IEnumerable<HexVector> regionHexes)
        {
            return hex.GetNeighbours().Except(regionHexes);
        }

        public static IEnumerable<Vector3> GetNeighbourLinePoints(this HexVector hex, IEnumerable<HexVector> neighboursNotInRegion)
        {
            var result = new List<Vector3>();
            var vertices = GetVertices();

            var notInRegion = neighboursNotInRegion.NotNull();
            if (notInRegion.Contains(hex.Add(HexVector.UP)))
            {
                result.Add(vertices[0]);
                result.Add(vertices[1]);
            }
            if (notInRegion.Contains(hex.Add(HexVector.UP_RIGHT)))
            {
                result.Add(vertices[1]);
                result.Add(vertices[2]);
            }
            if (notInRegion.Contains(hex.Add(HexVector.DOWN_RIGHT)))
            {
                result.Add(vertices[2]);
                result.Add(vertices[3]);
            }
            if (notInRegion.Contains(hex.Add(HexVector.DOWN)))
            {
                result.Add(vertices[3]);
                result.Add(vertices[4]);
            }
            if (notInRegion.Contains(hex.Add(HexVector.DOWN_LEFT)))
            {
                result.Add(vertices[4]);
                result.Add(vertices[5]);
            }
            if (notInRegion.Contains(hex.Add(HexVector.UP_LEFT)))
            {
                result.Add(vertices[5]);
                result.Add(vertices[0]);
            }

            return result.Select(v => v + new Vector3(hex.GetPositionX(), hex.GetPositionY(), 0));
        }

        static readonly float Sqrt3 = Mathf.Sqrt(3.0f);
        static readonly float side = 2f / Sqrt3;
        static readonly float h = side / 2.0f;
        static readonly float r = side * Sqrt3 / 2.0f;
        static readonly Vector3 p0 = new Vector3(h, 0);
        static readonly Vector3 p1 = new Vector3(h + side, 0);
        static readonly Vector3 p2 = new Vector3(h + side + h, r);
        static readonly Vector3 p3 = new Vector3(h + side, r + r);
        static readonly Vector3 p4 = new Vector3(h, r + r);
        static readonly Vector3 p5 = new Vector3(0, r);
        static readonly Vector3[] basicPoints = new[] { p0, p1, p2, p3, p4, p5 };

        public static float HexWidth { get { return SCALE*(h + side + h); } }

        public static Vector3 ScaleVector
        {
            get { return Vector3.one * HexVectorExtensions.SCALE;; }
        }

        public static readonly Vector3 hotspotShiftVector = -new Vector3(p2.x / 2f, p3.y / 2f);
        public static Vector3[] GetVertices()
        {
            var points = basicPoints.ToArray();

            for (int i = 0; i < 6; i++)
            {
                // make sure the "hotspot" of the vertices are in the center of the hexagon.
                points[i] += hotspotShiftVector;

                //Scale local space so that a flat hex (I.e.) flat up is 2*side*sqrt(3) wide
                //This makes local coordinates compatible with hx,hy,hz system
                points[i].Scale(ScaleVector);

                // Flip y because coordinates above are specified in old c64 style
                points[i].y = -points[i].y;

            }

            return points;

        }

        public static VectorLine CreateVectorLine(this HexVector vector)
        {
            Vector3[] hexLinePairs = vector.CreateLinePairVerts();
            return new VectorLine("HexagonLine", hexLinePairs, Color.white, null, 6.0f, LineType.Discrete);
        }

        public static Vector3[] CreateLinePairVerts(this HexVector vector)
        {
            var positionOffset = new Vector3(vector.GetPositionX(), vector.GetPositionY());
            return CreateLinePairVerts().Select(vector3 => vector3 + positionOffset).ToArray();
        }

        public static Vector3[] CreateLinePairVerts()
        {
            var verts = HexVectorExtensions.GetVertices();
            return CreateLinePairVerts(verts);
        }

        public static Vector3[] CreateLinePairVerts(Vector3[] verts)
        {
            return new[]
                       {
                           verts[0],
                           verts[1],
                           verts[1],
                           verts[2],
                           verts[2],
                           verts[3],
                           verts[3],
                           verts[4],
                           verts[4],
                           verts[5],
                           verts[5],
                           verts[0],
                       };
        }


    }
}