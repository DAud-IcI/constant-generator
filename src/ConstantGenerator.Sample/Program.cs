using System;
using Routes = ConstantGenerator.Sample.Constants.Routes;

namespace ConstantGenerator.Sample
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine(Routes.Management.New.User);
        }
    }
}