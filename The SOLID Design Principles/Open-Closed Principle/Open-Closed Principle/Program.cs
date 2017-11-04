using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
Open-Closed Principle:states the software entities(classes, modules, functions, etc.) 
should be open for extension, but closed for modification. 
*/
namespace Open_Closed_Principle
{
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public enum Size
    {
        Small,
        Medium,
        Large,
        Yuge
    }


    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;
            Color = color;
            Size = size;
        }
    }

    #region  Bad approach

    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
            {
                if (p.Size == size && p.Color == color)
                {
                    yield return p;
                }
            }
        }
    }

    #endregion


    #region Better approach 

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }


    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }


    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }


    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    #endregion


    class Program
    {
        public static void Main(string[] args)
        {
            //Bad approach
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var pf = new ProductFilter();

            Console.WriteLine("Green products (old)");

            foreach (var product in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($"-{product.Name} is green");
            }

            //Better approach
            var bf = new BetterFilter();
            Console.WriteLine("Green products (new):");
            foreach (var product in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($"-{product.Name} is green");
            }

            Console.WriteLine("Large blue items");
            foreach (var product in bf.Filter(
                products,
                new AndSpecification<Product>(
                    new ColorSpecification(Color.Blue),
                    new SizeSpecification(Size.Large)
                )
            ))
            {
                Console.WriteLine($" -{product.Name} is big and blue");
            }
        }
    }
}