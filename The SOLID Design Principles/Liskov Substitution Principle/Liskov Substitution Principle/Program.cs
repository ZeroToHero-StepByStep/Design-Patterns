using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
Liskov Substitution Principle: The Subtypes must be substitutable for their base types 
*/

namespace Liskov_Substitution_Principle
{
    public class Rectangle
    {
        //        Bad approach
        //        public int Width { get; set; }
        //        public int Height { get; set;}


        //Better approach
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {
        }

        public Rectangle(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}:{Width}, {nameof(Height)}:{Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Height = base.Height = value; }
        }
    }

    class Program
    {
        public static int Area(Rectangle rect) => rect.Width * rect.Height;

        static void Main(string[] args)
        {
            Rectangle rect = new Rectangle(2, 3);
            Console.WriteLine($"{rect} has {Area(rect)}");

            Rectangle sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area{Area(sq)}");
        }
    }
}