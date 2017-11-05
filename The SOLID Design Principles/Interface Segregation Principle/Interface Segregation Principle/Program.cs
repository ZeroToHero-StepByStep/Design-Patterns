using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Interface Segregation Principle: Clients should not be forced to depend on methods they do not use,
meaning interfaces should be small, cohesive interfaces to 'fat'. 
*/
namespace Interface_Segregation_Principle
{

    public class Document
    {
        
    }

    #region  Bad approach
    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class OldFashionedPrinter :IMachine
    {
        public void Print(Document d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }

        public void Fax(Document d)
        {
            throw new NotImplementedException();
        }
    }
    #endregion


    #region Better approach

    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    //if we want a multi-interface 
    public interface IMultiFunctionDevice : IScanner, IPrinter //...
    {
        
    }

    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        //        we can implment the interface functions 
        //        public void Scan(Document d)
        //        {
        //            throw new NotImplementedException();
        //        }
        //
        //        public void Print(Document d)
        //        {
        //            throw new NotImplementedException();
        //        }


        // or we can use delegation pattern

        // compose this out of several modules
        private IPrinter printer;

        private IScanner scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            if (printer == null)
            {
                throw new ArgumentNullException(paramName: nameof(printer));
            }
            if (scanner == null)
            {
                throw new ArgumentNullException(paramName: nameof(scanner));
            }
            this.printer = printer;
            this.scanner = scanner;
        }

        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }

    }

    #endregion

    class Program
    {

        static void Main(string[] args)
        {
        }
    }
}
