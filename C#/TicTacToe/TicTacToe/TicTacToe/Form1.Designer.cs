namespace TicTacToe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            CPUTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(104, 21);
            label1.TabIndex = 0;
            label1.Text = "Player Wins:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(363, 9);
            label2.Name = "label2";
            label2.Size = new Size(88, 21);
            label2.TabIndex = 1;
            label2.Text = "CPU Wins:";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(76, 64);
            button1.Name = "button1";
            button1.Size = new Size(100, 100);
            button1.TabIndex = 2;
            button1.Text = "?";
            button1.UseVisualStyleBackColor = true;
            button1.Click += PlayerClick;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(182, 64);
            button2.Name = "button2";
            button2.Size = new Size(100, 100);
            button2.TabIndex = 3;
            button2.Text = "?";
            button2.UseVisualStyleBackColor = true;
            button2.Click += PlayerClick;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(288, 64);
            button3.Name = "button3";
            button3.Size = new Size(100, 100);
            button3.TabIndex = 4;
            button3.Text = "?";
            button3.UseVisualStyleBackColor = true;
            button3.Click += PlayerClick;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button4.Location = new Point(76, 170);
            button4.Name = "button4";
            button4.Size = new Size(100, 100);
            button4.TabIndex = 5;
            button4.Text = "?";
            button4.UseVisualStyleBackColor = true;
            button4.Click += PlayerClick;
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button5.Location = new Point(182, 170);
            button5.Name = "button5";
            button5.Size = new Size(100, 100);
            button5.TabIndex = 6;
            button5.Text = "?";
            button5.UseVisualStyleBackColor = true;
            button5.Click += PlayerClick;
            // 
            // button6
            // 
            button6.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button6.Location = new Point(288, 170);
            button6.Name = "button6";
            button6.Size = new Size(100, 100);
            button6.TabIndex = 7;
            button6.Text = "?";
            button6.UseVisualStyleBackColor = true;
            button6.Click += PlayerClick;
            // 
            // button7
            // 
            button7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button7.Location = new Point(76, 276);
            button7.Name = "button7";
            button7.Size = new Size(100, 100);
            button7.TabIndex = 8;
            button7.Text = "?";
            button7.UseVisualStyleBackColor = true;
            button7.Click += PlayerClick;
            // 
            // button8
            // 
            button8.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button8.Location = new Point(182, 276);
            button8.Name = "button8";
            button8.Size = new Size(100, 100);
            button8.TabIndex = 9;
            button8.Text = "?";
            button8.UseVisualStyleBackColor = true;
            button8.Click += PlayerClick;
            // 
            // button9
            // 
            button9.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            button9.Location = new Point(288, 276);
            button9.Name = "button9";
            button9.Size = new Size(100, 100);
            button9.TabIndex = 10;
            button9.Text = "?";
            button9.UseVisualStyleBackColor = true;
            button9.Click += PlayerClick;
            // 
            // button10
            // 
            button10.Location = new Point(196, 408);
            button10.Name = "button10";
            button10.Size = new Size(75, 23);
            button10.TabIndex = 11;
            button10.Text = "Restart";
            button10.UseVisualStyleBackColor = true;
            button10.Click += RestartGame;
            // 
            // CPUTimer
            // 
            CPUTimer.Interval = 1000;
            CPUTimer.Tick += CPUmove;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Tic Tac Toe Game";
            ResumeLayout(false);
            PerformLayout();
        }

        private void RestartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private System.Windows.Forms.Timer CPUTimer;
    }
}