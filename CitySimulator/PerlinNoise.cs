﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator {
    class PerlinNoise {
        public int Octaves = 4;
        public float Persistence = 0.5f;
        public float Scale = 10f;

        public int Seed;
        
        float Noise1(int x, int y) {
            var n = x + y * 57 + Seed;
            n = (n<<13) ^ n;
            return ( 1.0f - ( (n* (n* n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);    
        }
        
        /// <summary>
        /// Function to linearly interpolate between a0 and a1
        /// </summary>
        /// <param name="a0">The return value if w = 0</param>
        /// <param name="a1">The return value if w = 1</param>
        /// <param name="w">Weight. Should be in the range [0.0, 1.0]</param>
        /// <returns>The interpolation value</returns>
        float lerp(float a0, float a1, float w) {
            return (1.0f - w) * a0 + w * a1;
        }
        
        float InterpolatedNoise(float x, float y) {
            var xInt = (int)Math.Floor(x);
            var xFrac = x - xInt;

            var yInt = (int)Math.Floor(y);
            var yFrac = y - yInt;

            var v1 = Noise1(xInt, yInt);
            var v2 = Noise1(xInt + 1, yInt);
            var v3 = Noise1(xInt, yInt + 1);
            var v4 = Noise1(xInt + 1, yInt + 1);

            var i1 = lerp(v1, v2, xFrac);
            var i2 = lerp(v3, v4, xFrac);

            return lerp(i1, i2, yFrac);
        }

        internal float Get(float x, float y) {
            var total = 0f;

            for(var i = 0; i < Octaves; i++) {
                float frequency = (float)Math.Pow(2,i) / Scale;
                float amplitude = (float)Math.Pow(Persistence, i);

                total += InterpolatedNoise(x * frequency, y * frequency) * amplitude;
            }
            return total;
        }
    }
}
