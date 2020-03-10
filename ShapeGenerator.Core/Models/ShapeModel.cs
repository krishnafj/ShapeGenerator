using System;
using System.Collections.Generic;

namespace ShapeGenerator.Core.Models
{
    public class ShapeModel
    {
        public string ShapeType { get; set; }
        public double Radius { get; set; }
        public List<double> SideLength { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string ErrorMessage { get; set; }
    }
}
