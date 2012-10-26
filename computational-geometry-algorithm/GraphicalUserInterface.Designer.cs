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
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.stepThroughCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.githubHyperlink = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.testAllFarthestSegments = new System.Windows.Forms.Button();
            this.sizeMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // testConvexHullButton
            // 
            this.testConvexHullButton.Location = new System.Drawing.Point(742, 503);
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
            this.label1.Location = new System.Drawing.Point(723, 418);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of points:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(723, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "X Size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(723, 470);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y Size:";
            // 
            // numberOfPointsTextBox
            // 
            this.numberOfPointsTextBox.Location = new System.Drawing.Point(819, 415);
            this.numberOfPointsTextBox.Name = "numberOfPointsTextBox";
            this.numberOfPointsTextBox.Size = new System.Drawing.Size(100, 20);
            this.numberOfPointsTextBox.TabIndex = 4;
            // 
            // xSizeTextBox
            // 
            this.xSizeTextBox.Location = new System.Drawing.Point(819, 441);
            this.xSizeTextBox.Name = "xSizeTextBox";
            this.xSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.xSizeTextBox.TabIndex = 5;
            // 
            // ySizeTextBox
            // 
            this.ySizeTextBox.Location = new System.Drawing.Point(819, 467);
            this.ySizeTextBox.Name = "ySizeTextBox";
            this.ySizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.ySizeTextBox.TabIndex = 6;
            // 
            // testSingleObstacleAvoidance
            // 
            this.testSingleObstacleAvoidance.Location = new System.Drawing.Point(742, 532);
            this.testSingleObstacleAvoidance.Name = "testSingleObstacleAvoidance";
            this.testSingleObstacleAvoidance.Size = new System.Drawing.Size(177, 23);
            this.testSingleObstacleAvoidance.TabIndex = 7;
            this.testSingleObstacleAvoidance.Text = "Test Single Obstacle Avoidance";
            this.testSingleObstacleAvoidance.UseVisualStyleBackColor = true;
            this.testSingleObstacleAvoidance.Click += new System.EventHandler(this.TestSingleObstacleAvoidanceClick);
            // 
            // testMultipleObstacleAvoidance
            // 
            this.testMultipleObstacleAvoidance.Location = new System.Drawing.Point(937, 503);
            this.testMultipleObstacleAvoidance.Name = "testMultipleObstacleAvoidance";
            this.testMultipleObstacleAvoidance.Size = new System.Drawing.Size(207, 23);
            this.testMultipleObstacleAvoidance.TabIndex = 8;
            this.testMultipleObstacleAvoidance.Text = "Test Multiple Obstacle Avoidance";
            this.testMultipleObstacleAvoidance.UseVisualStyleBackColor = true;
            this.testMultipleObstacleAvoidance.Click += new System.EventHandler(this.TestMultipleObstacleAvoidanceClick);
            // 
            // numberOfPolygonsTextBox
            // 
            this.numberOfPolygonsTextBox.Location = new System.Drawing.Point(1044, 415);
            this.numberOfPolygonsTextBox.Name = "numberOfPolygonsTextBox";
            this.numberOfPolygonsTextBox.Size = new System.Drawing.Size(100, 20);
            this.numberOfPolygonsTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(934, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Number of polygons:";
            // 
            // testDCHull
            // 
            this.testDCHull.Location = new System.Drawing.Point(742, 562);
            this.testDCHull.Name = "testDCHull";
            this.testDCHull.Size = new System.Drawing.Size(177, 23);
            this.testDCHull.TabIndex = 11;
            this.testDCHull.Text = "Test DCHull";
            this.testDCHull.UseVisualStyleBackColor = true;
            this.testDCHull.Click += new System.EventHandler(this.testDCHull_Click);
            // 
            // debugTextBox
            // 
            this.debugTextBox.Location = new System.Drawing.Point(726, 27);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.Size = new System.Drawing.Size(418, 342);
            this.debugTextBox.TabIndex = 12;
            // 
            // stepThroughCheckBox
            // 
            this.stepThroughCheckBox.AutoSize = true;
            this.stepThroughCheckBox.Location = new System.Drawing.Point(937, 566);
            this.stepThroughCheckBox.Name = "stepThroughCheckBox";
            this.stepThroughCheckBox.Size = new System.Drawing.Size(91, 17);
            this.stepThroughCheckBox.TabIndex = 13;
            this.stepThroughCheckBox.Text = "Step Through";
            this.stepThroughCheckBox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1066, 650);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Alexander Spedding";
            // 
            // githubHyperlink
            // 
            this.githubHyperlink.AutoSize = true;
            this.githubHyperlink.Location = new System.Drawing.Point(921, 650);
            this.githubHyperlink.Name = "githubHyperlink";
            this.githubHyperlink.Size = new System.Drawing.Size(139, 13);
            this.githubHyperlink.TabIndex = 15;
            this.githubHyperlink.TabStop = true;
            this.githubHyperlink.Text = "https://github.com/AlexDiru";
            this.githubHyperlink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.githubHyperlink_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(937, 590);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(215, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "To move to the next step focus the console ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(939, 607);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "window and press any key";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(436, 46);
            this.label8.TabIndex = 18;
            this.label8.Text = "Convex Hull Algorithms";
            // 
            // testAllFarthestSegments
            // 
            this.testAllFarthestSegments.Location = new System.Drawing.Point(742, 592);
            this.testAllFarthestSegments.Name = "testAllFarthestSegments";
            this.testAllFarthestSegments.Size = new System.Drawing.Size(177, 23);
            this.testAllFarthestSegments.TabIndex = 19;
            this.testAllFarthestSegments.Text = "All Farthest Segments";
            this.testAllFarthestSegments.UseVisualStyleBackColor = true;
            this.testAllFarthestSegments.Click += new System.EventHandler(this.testAllFarthestSegments_Click);
            // 
            // sizeMultiplierTextBox
            // 
            this.sizeMultiplierTextBox.Location = new System.Drawing.Point(1044, 441);
            this.sizeMultiplierTextBox.Name = "sizeMultiplierTextBox";
            this.sizeMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.sizeMultiplierTextBox.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(934, 444);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Size Multiplier:";
            // 
            // GraphicalUserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 672);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.sizeMultiplierTextBox);
            this.Controls.Add(this.testAllFarthestSegments);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.githubHyperlink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.stepThroughCheckBox);
            this.Controls.Add(this.debugTextBox);
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
            this.Text = "Convex Hull Algorithms";
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
        private TextBox debugTextBox;
        private CheckBox stepThroughCheckBox;
        private Label label5;
        private LinkLabel githubHyperlink;
        private Label label6;
        private Label label7;
        private Label label8;
        private Button testAllFarthestSegments;
        private TextBox sizeMultiplierTextBox;
        private Label label9;
    }
}