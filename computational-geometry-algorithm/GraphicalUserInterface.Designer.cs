using System.Windows.Forms;
using Graphics = System.Drawing.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace computational_geometry_algorithm
{
    partial class GraphicalUserInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.testConvexHullButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numberOfPointsTextBox = new System.Windows.Forms.TextBox();
            this.xSizeTextBox = new System.Windows.Forms.TextBox();
            this.ySizeTextBox = new System.Windows.Forms.TextBox();
            this.testSingleObstacleAvoidance = new System.Windows.Forms.Button();
            this.testMultipleObstacleAvoidance = new System.Windows.Forms.Button();
            this.numberOfPolygonsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.testDCHull = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testConvexHullButton
            // 
            this.testConvexHullButton.Location = new System.Drawing.Point(742, 562);
            this.testConvexHullButton.Name = "testConvexHullButton";
            this.testConvexHullButton.Size = new System.Drawing.Size(177, 23);
            this.testConvexHullButton.TabIndex = 0;
            this.testConvexHullButton.Text = "Test Convex Hull";
            this.testConvexHullButton.UseVisualStyleBackColor = true;
            this.testConvexHullButton.Click += new System.EventHandler(this.TestConvexHullButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(723, 477);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of points:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(723, 503);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "X Size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(723, 529);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y Size:";
            // 
            // numberOfPointsTextBox
            // 
            this.numberOfPointsTextBox.Location = new System.Drawing.Point(819, 474);
            this.numberOfPointsTextBox.Name = "numberOfPointsTextBox";
            this.numberOfPointsTextBox.Size = new System.Drawing.Size(100, 20);
            this.numberOfPointsTextBox.TabIndex = 4;
            // 
            // xSizeTextBox
            // 
            this.xSizeTextBox.Location = new System.Drawing.Point(819, 500);
            this.xSizeTextBox.Name = "xSizeTextBox";
            this.xSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.xSizeTextBox.TabIndex = 5;
            // 
            // ySizeTextBox
            // 
            this.ySizeTextBox.Location = new System.Drawing.Point(819, 526);
            this.ySizeTextBox.Name = "ySizeTextBox";
            this.ySizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.ySizeTextBox.TabIndex = 6;
            // 
            // testSingleObstacleAvoidance
            // 
            this.testSingleObstacleAvoidance.Location = new System.Drawing.Point(742, 591);
            this.testSingleObstacleAvoidance.Name = "testSingleObstacleAvoidance";
            this.testSingleObstacleAvoidance.Size = new System.Drawing.Size(177, 23);
            this.testSingleObstacleAvoidance.TabIndex = 7;
            this.testSingleObstacleAvoidance.Text = "Test Single Obstacle Avoidance";
            this.testSingleObstacleAvoidance.UseVisualStyleBackColor = true;
            this.testSingleObstacleAvoidance.Click += new System.EventHandler(this.TestSingleObstacleAvoidanceClick);
            // 
            // testMultipleObstacleAvoidance
            // 
            this.testMultipleObstacleAvoidance.Location = new System.Drawing.Point(937, 562);
            this.testMultipleObstacleAvoidance.Name = "testMultipleObstacleAvoidance";
            this.testMultipleObstacleAvoidance.Size = new System.Drawing.Size(207, 23);
            this.testMultipleObstacleAvoidance.TabIndex = 8;
            this.testMultipleObstacleAvoidance.Text = "Test Multiple Obstacle Avoidance";
            this.testMultipleObstacleAvoidance.UseVisualStyleBackColor = true;
            this.testMultipleObstacleAvoidance.Click += new System.EventHandler(this.TestMultipleObstacleAvoidanceClick);
            // 
            // numberOfPolygonsTextBox
            // 
            this.numberOfPolygonsTextBox.Location = new System.Drawing.Point(1044, 474);
            this.numberOfPolygonsTextBox.Name = "numberOfPolygonsTextBox";
            this.numberOfPolygonsTextBox.Size = new System.Drawing.Size(100, 20);
            this.numberOfPolygonsTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(934, 477);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Number of polygons:";
            // 
            // testDCHull
            // 
            this.testDCHull.Location = new System.Drawing.Point(742, 621);
            this.testDCHull.Name = "testDCHull";
            this.testDCHull.Size = new System.Drawing.Size(177, 23);
            this.testDCHull.TabIndex = 11;
            this.testDCHull.Text = "Test DCHull";
            this.testDCHull.UseVisualStyleBackColor = true;
            this.testDCHull.Click += new System.EventHandler(this.testDCHull_Click);
            // 
            // GraphicalUserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 672);
            this.Controls.Add(this.testDCHull);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numberOfPolygonsTextBox);
            this.Controls.Add(this.testMultipleObstacleAvoidance);
            this.Controls.Add(this.testSingleObstacleAvoidance);
            this.Controls.Add(this.ySizeTextBox);
            this.Controls.Add(this.xSizeTextBox);
            this.Controls.Add(this.numberOfPointsTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.testConvexHullButton);
            this.Name = "GraphicalUserInterface";
            this.Text = "GraphicalUserInterface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button testConvexHullButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox numberOfPointsTextBox;
        private TextBox xSizeTextBox;
        private TextBox ySizeTextBox;
        private Button testSingleObstacleAvoidance;
        private Button testMultipleObstacleAvoidance;
        private TextBox numberOfPolygonsTextBox;
        private Label label4;
        private Button testDCHull;
    }
}