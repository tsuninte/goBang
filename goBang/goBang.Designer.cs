using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goBang
{
    partial class goBang
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PictureBox chessBoard;
        private Button startButton;
        private Button goBackButton;
        private Button failButton;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private Image chesses;
        
        private Dialog form;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chessBoard = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.goBackButton = new System.Windows.Forms.Button();
            this.failButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chessBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // chessBoard
            // 
            this.chessBoard.BackColor = System.Drawing.Color.Peru;
            this.chessBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chessBoard.Location = new System.Drawing.Point(0, 0);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(680, 680);
            this.chessBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chessBoard.TabIndex = 0;
            this.chessBoard.TabStop = false;
            this.chessBoard.Click += new System.EventHandler(this.chessBoard_Click);
            // 
            // startButton
            // 
            this.startButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startButton.Font = new System.Drawing.Font("仿宋_GB2312", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startButton.Location = new System.Drawing.Point(710, 357);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(107, 51);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "开始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.start_Click);
            // 
            // goBackButton
            // 
            this.goBackButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goBackButton.Enabled = false;
            this.goBackButton.Font = new System.Drawing.Font("仿宋_GB2312", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.goBackButton.Location = new System.Drawing.Point(710, 277);
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(107, 51);
            this.goBackButton.TabIndex = 3;
            this.goBackButton.Text = "悔棋";
            this.goBackButton.UseVisualStyleBackColor = true;
            this.goBackButton.Click += new System.EventHandler(this.goBackButton_Click);
            // 
            // failButton
            // 
            this.failButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.failButton.Enabled = false;
            this.failButton.Font = new System.Drawing.Font("仿宋_GB2312", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.failButton.Location = new System.Drawing.Point(710, 195);
            this.failButton.Name = "failButton";
            this.failButton.Size = new System.Drawing.Size(107, 51);
            this.failButton.TabIndex = 3;
            this.failButton.Text = "认输";
            this.failButton.UseVisualStyleBackColor = true;
            this.failButton.Click += new System.EventHandler(this.fail_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Sienna;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(686, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(54, 23);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "黑方:";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.textBox2.BackColor = System.Drawing.Color.Sienna;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox2.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox2.Location = new System.Drawing.Point(686, 93);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ShortcutsEnabled = false;
            this.textBox2.Size = new System.Drawing.Size(54, 23);
            this.textBox2.TabIndex = 4;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "白方:";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("仿宋_GB2312", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(710, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "电脑";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.player1_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("仿宋_GB2312", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(710, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 51);
            this.button2.TabIndex = 1;
            this.button2.Text = "玩家";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.player2_Click);
            // 
            // goBang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Sienna;
            this.ClientSize = new System.Drawing.Size(848, 680);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.failButton);
            this.Controls.Add(this.goBackButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.chessBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "goBang";
            this.ShowIcon = false;
            this.Text = "五子棋";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chessBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void drawLines()
        {
            Color color = Color.Black;
            float penWid = 1.0f;
            Pen pen = new Pen(color, penWid);

            float gapX = (float)chessBoardWidth / (BOARD_WIDTH - 1);
            float gapY = (float)chessBoardHeight / (BOARD_WIDTH - 1);
            Image lines = new Bitmap(chessBoardWidth + 2 * offsetW, chessBoardHeight + 2 * offsetH);
            Graphics boardLines = Graphics.FromImage(lines);
            boardLines.DrawRectangle(pen, offsetW, offsetH, chessBoardWidth, chessBoardHeight);

            for (int i = 1; i < BOARD_WIDTH - 1; i++)
            {
                boardLines.DrawLine(pen, offsetW, offsetH + i * gapY, offsetW + chessBoardWidth, offsetH + i * gapY);
                boardLines.DrawLine(pen, offsetW + i * gapX, offsetH, offsetW + i * gapX, offsetH + chessBoardHeight);
            }
            chessBoard.BackgroundImage = lines;
        }

        //Type: 0 for black, 1 white
        public void DrawChess(short blackOrWhite, short y, short x, bool fresh = true)
        {
            int radiusW = (int)(absRadius * rWidth);
            int radiusH = (int)(absRadius * rHeight);
           
            int newX = offsetW + x * chessBoardWidth / (BOARD_WIDTH - 1) - radiusW;
            int newY = offsetH + y * chessBoardHeight / (BOARD_WIDTH - 1) - radiusH;

            Graphics chessEclipse = Graphics.FromImage(chesses);
            Pen pen = new Pen(Color.Black, 1.0f);
            Brush brush = new SolidBrush(Color.Black);
            if (blackOrWhite == WHITE)
            {
                pen.Color = Color.White;
                brush = new SolidBrush(Color.White);
            }

            chessEclipse.DrawEllipse(pen, newX, newY, 2 * radiusW, 2 * radiusH);
            chessEclipse.FillEllipse(brush, newX, newY, 2 * radiusW, 2 * radiusH);
            chessBoard.Image = chesses;
            if (fresh)
                chessBoard.Refresh(); //? Refresh immediately
        }

        public void DrawChess(short blackOrWhite, short idx, bool fresh = true)
        {
            DrawChess(blackOrWhite, (short)(idx / BOARD_WIDTH), (short)(idx % BOARD_WIDTH), fresh);
        }

        private short getMousePos()
        {
            Point mousePos = chessBoard.PointToClient(MousePosition);
            short x = -1, y = -1;
            int gap = chessBoardAbsHeight / (BOARD_WIDTH - 1);
            int absX = (int)(mousePos.X / rWidth);
            int absY = (int)(mousePos.Y / rHeight);

            if ((absX - absOffset) % gap < absRadius)
                x = (short)((absX - absOffset) / gap);
            else if ((absX - absOffset) % gap > gap - absRadius)
                x = (short)((absX - absOffset) / gap + 1);           

            if ((absY - absOffset) % gap < absRadius)
                y = (short)((absY - absOffset) / gap);
            else if ((absY - absOffset) % gap > gap - absRadius)
                y = (short)((absY - absOffset) / gap + 1);

            if (x >= 0 && y >= 0)
                return (short)(x + y * BOARD_WIDTH);

            return NONE;
        }

        private void createChess()
        {
            chesses = new Bitmap(chessBoardWidth + 2 * offsetW, chessBoardHeight + 2 * offsetH);
        }

        private void clearChess()
        {
            chessBoard.Image = null;
            if (chesses!=null)
                chesses.Dispose();
        }

        private void lockButton()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            startButton.Enabled = false;

            goBackButton.Enabled = true;
            failButton.Enabled = true;
        }

        private void unlockButton()
        {
            startButton.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;

            goBackButton.Enabled = false;
            failButton.Enabled = false;
        }

    }
}

