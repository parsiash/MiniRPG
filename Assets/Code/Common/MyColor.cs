using UnityEngine;

namespace MiniRPG.Common
{
    public class MyColor
    {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }
        public float a { get; set; }

        public MyColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color GetUnityColor()
        {
            return new Color(r, g, b, a);
        }

        public static MyColor FromUnityColor(Color color)
        {
            return new MyColor(color.r, color.g, color.b, color.a);
        }
    }
}