using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //.RandomisedTests(5,10, 20, 20);

            /*
            String map =  "  # # #          \n" +         
                          " #  #   #  #    #\n" +
                          "     #     #     \n" +
                          "         #   #   ";
            var dat = DataSet.GetDataFromString(map);

            ConvexHullTesting.TestData(dat);
             * */

            Map map = new Map("                       a      a                    \n" +
                              "                                                   \n" +
                              "                       a              a            \n" + 
                              "               a                      a            \n" +
                              "                     a          a                  \n" +
                              "                  a             a                  \n" +
                              "                                                   \n" +
                              "       b        b                 c       c        \n" +
                              "A b                  b                          C  \n" +
                              "  b             b                 c           c    \n" +
                              "    b        b                 c              c    \n" +
                              "   b                 b     B                       \n");

            return;
        }
    }
}
