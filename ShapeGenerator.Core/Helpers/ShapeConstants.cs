
using System.Collections.Generic;

namespace ShapeGenerator.Core.Helpers
{
    public static class ShapeConstants
    {
        public static class ShapeTypes
        {
            public const string IsoscelesTriangle = "Isosceles Triangle";
            public const string ScaleneTriangle = "Scalene Triangle";
            public const string EquilateralTriangle = "Equilateral Triangle";
            public const string Rectangle = "Rectangle";
            public const string Square = "Square";
            public const string Parallelogram = "Parallelogram";
            public const string Pentagon = "Pentagon";
            public const string Hexagon = "Hexagon";
            public const string Heptagon = "Heptagon";
            public const string Octagon = "Octagon";
            public const string Circle = "Circle";
            public const string Oval = "Oval";
        }

        public static class MeasurementType
        {
            public const string Radius = "radius";
            public const string Height = "height";
            public const string Width = "width";
            public const string SideLength = "side length";
        }

        public static List<string> SupportedShapesList = new List<string> {
            ShapeTypes.IsoscelesTriangle, ShapeTypes.ScaleneTriangle, ShapeTypes.EquilateralTriangle,
            ShapeTypes.Rectangle, ShapeTypes.Square, ShapeTypes.Parallelogram, ShapeTypes.Pentagon,
            ShapeTypes.Hexagon, ShapeTypes.Heptagon, ShapeTypes.Octagon, ShapeTypes.Circle, ShapeTypes.Oval};

        public static List<string> MeasurementList = new List<string> { MeasurementType.Height,
            MeasurementType.Radius, MeasurementType.Width, MeasurementType.SideLength};
    }
}
