using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotFuzzy
{
    /// <summary>
    /// Represents a membership function.
    /// </summary>

    public class MembershipFunction
    {

        private string name = String.Empty;
        private double x0 = 0;
        private double x1 = 0;
        private double x2 = 0;
        private double x3 = 0;
        private double value = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 

        public MembershipFunction()
        {
        }

        public MembershipFunction(string name)
        {
            this.Name = name;
        }


        public MembershipFunction(string name, double x0, double x1, double x2, double x3)
        {
            this.Name = name;
            this.X0 = x0;
            this.X1 = x1;
            this.X2 = x2;
            this.X3 = x3;
        }


        /// <summary>
        /// The name that identificates the membership function.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The value of the (x0, 0) point.
        /// </summary>
        public double X0
        {
            get { return x0; }
            set { x0 = value; }
        }

        /// <summary>
        /// The value of the (x1, 1) point.
        /// </summary>
        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        /// <summary>
        /// The value of the (x2, 1) point.
        /// </summary>
        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        /// <summary>
        /// The value of the (x3, 0) point.
        /// </summary>
        public double X3
        {
            get { return x3; }
            set { x3 = value; }
        }

        /// <summary>
        /// The value of membership function after evaluation process.
        /// </summary>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }


        /// <summary>
        /// Calculate the centroid of a trapezoidal membership function.
        /// </summary>
        /// <returns>The value of centroid.</returns>
        public double Centorid()
        {
            double a = this.x2 - this.x1;
            double b = this.x3 - this.x0;
            double c = this.x1 - this.x0;

            return ((2 * a * c) + (a * a) + (c * b) + (a * b) + (b * b)) / (3 * (a + b)) + this.x0; 
        }

        /// <summary>
        /// Calculate the area of a trapezoidal membership function.
        /// </summary>
        /// <returns>The value of area.</returns>
        public double Area()
        {
            double a = this.Centorid() - this.x0;
            double b = this.x3 - this.x0;

            return (this.value * (b + (b - (a * this.value)))) / 2;
        }
    }
}
