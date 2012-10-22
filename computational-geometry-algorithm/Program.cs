using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            //GraphicalUserInterface gui = new GraphicalUserInterface();
            Application.EnableVisualStyles();
            Application.Run(new GraphicalUserInterface());

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

           
            map.SolveMap();
        

            //Convex Hull algorithm testing
            ConvexHullTesting.RandomisedTests(3, 20, 20, 20);

            //Single obstacle avoidance test
            //ConvexHullTesting.PathPlannerSingleObstacleRandomisedTest(1, 20, 20, 20);

            
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