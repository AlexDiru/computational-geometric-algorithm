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

            /*Map map = new Map("                       a      a                    \n" +
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

            map.SolveMap();
            */

            ConvexHullTesting.TestData(DataSet.GetDataFromString(
                "          #                #           \n" +
                "                                       \n" +
                "               #       #               \n" +
                "         #                  #          \n" +
                "                  #                    \n" +
                "             #            #            \n"));

            
        }
    }
}

/* Examples
 
 * Convex Hull Custom Test 
 
   ConvexHullTesting.TestData(DataSet.GetDataFromString(
                "          #                #           \n" +
                "                                       \n" +
                "               #       #               \n" +
                "         #                  #          \n" +
                "                  #                    \n" +
                "             #            #            \n")); 

 * Convex Hull Randomised Test

   ConvexHull.RandomisedTests(5,10, 20, 20);

 * Single Obstacle Avoidance

    Map map = new Map("                       a      a                    \n" +
                      "          A                                        \n" +
                      "                       a              a            \n" +
                      "               a                      a     C      \n" +
                      "                     a          a                  \n" +
                      "                  a             a                  \n" +
                      "                                                   \n");

    map.SolveMap();
    return;

*/