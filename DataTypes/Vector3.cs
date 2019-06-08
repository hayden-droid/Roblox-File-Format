﻿using System;
using RobloxFiles.Enums;

namespace RobloxFiles.DataTypes
{
    public class Vector3
    {
        public readonly float X, Y, Z;

        public float Magnitude
        {
            get
            {
                float product = Dot(this);
                double magnitude = Math.Sqrt(product);

                return (float)magnitude;
            }
        }
        
        public Vector3 Unit
        {
            get { return this / Magnitude; }
        }

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float[] coords)
        {
            X = coords.Length > 0 ? coords[0] : 0;
            Y = coords.Length > 1 ? coords[1] : 0;
            Z = coords.Length > 2 ? coords[2] : 0;
        }

        public static Vector3 FromAxis(Axis axis)
        {
            float[] coords = new float[3] { 0f, 0f, 0f };

            int index = (int)axis;
            coords[index] = 1f;

            return new Vector3(coords);
        }

        public static Vector3 FromNormalId(NormalId normalId)
        {
            float[] coords = new float[3] { 0f, 0f, 0f };

            int index = (int)normalId;
            coords[index % 3] = (index > 2 ? -1f : 1f);

            return new Vector3(coords);
        }

        private delegate Vector3 Operator(Vector3 a, Vector3 b);
        
        private static Vector3 upcastFloatOp(Vector3 vec, float num, Operator upcast)
        {
            Vector3 numVec = new Vector3(num, num, num);
            return upcast(vec, numVec);
        }

        private static Vector3 upcastFloatOp(float num, Vector3 vec, Operator upcast)
        {
            Vector3 numVec = new Vector3(num, num, num);
            return upcast(numVec, vec);
        }

        private static Operator add = new Operator((a, b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z));
        private static Operator sub = new Operator((a, b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z));
        private static Operator mul = new Operator((a, b) => new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z));
        private static Operator div = new Operator((a, b) => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z));

        public static Vector3 operator +(Vector3 a, Vector3 b) => add(a, b);
        public static Vector3 operator +(Vector3 v, float n)   => upcastFloatOp(v, n, add);
        public static Vector3 operator +(float n, Vector3 v)   => upcastFloatOp(n, v, add);

        public static Vector3 operator -(Vector3 a, Vector3 b) => sub(a, b);
        public static Vector3 operator -(Vector3 v, float n)   => upcastFloatOp(v, n, sub);
        public static Vector3 operator -(float n, Vector3 v)   => upcastFloatOp(n, v, sub);

        public static Vector3 operator *(Vector3 a, Vector3 b) => mul(a, b);
        public static Vector3 operator *(Vector3 v, float n)   => upcastFloatOp(v, n, mul);
        public static Vector3 operator *(float n, Vector3 v)   => upcastFloatOp(n, v, mul);

        public static Vector3 operator /(Vector3 a, Vector3 b) => div(a, b);
        public static Vector3 operator /(Vector3 v, float n)   => upcastFloatOp(v, n, div);
        public static Vector3 operator /(float n, Vector3 v)   => upcastFloatOp(n, v, div);

        public static Vector3 Zero  => new Vector3(0, 0, 0);
        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Up    => new Vector3(0, 1, 0);
        public static Vector3 Back  => new Vector3(0, 0, 1);

        public override string ToString()
        {
            return string.Join(", ", X, Y, Z);
        }

        public float Dot(Vector3 other)
        {
            float dotX = X * other.X;
            float dotY = Y * other.Y;
            float dotZ = Z * other.Z;

            return dotX + dotY + dotZ;
        }

        public Vector3 Cross(Vector3 other)
        {
            float crossX = Y * other.Z - other.Y * Z;
            float crossY = Z * other.X - other.Z * X;
            float crossZ = X * other.Y - other.X * Y;

            return new Vector3(crossX, crossY, crossZ);
        }

        public Vector3 Lerp(Vector3 other, float t)
        {
            return this + (other - this) * t;
        }

        public bool IsClose(Vector3 other, float epsilon = 0.0f)
        {
            return (other - this).Magnitude <= Math.Abs(epsilon);
        }

        public int ToNormalId()
        {
            int result = -1;

            for (int i = 0; i < 6; i++)
            {
                NormalId normalId = (NormalId)i;
                Vector3 normal = FromNormalId(normalId);

                float dotProd = normal.Dot(this);

                if (Math.Abs(dotProd - 1f) < 10e-5f)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
