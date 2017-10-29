using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goBang {
    public partial class goBang : Form
    {
        private const short BOARD_WIDTH = 15;
        private const short BLACK = 0;
        private const short WHITE = 1;
        private const short NONE = 0xFF;

        //? 0 for AI, 1 for player
        private bool player1, player2;

        //? Draw picture (Uint: piexls)
        private int chessBoardAbsHeight;
        private int chessBoardAbsWidth;
        private int absOffset;
        private int absRadius;

        private int chessBoardWidth;
        private int chessBoardHeight;
        private int offsetW, offsetH;
        private float rWidth, rHeight;

        private bool gameStart;

        private mainClass.ComChessBoard myChessBoard;
        private mainClass.AIPlayer AIplayer1, AIplayer2;

        public goBang()
        {            
            InitializeComponent();
            InitValue();
            drawLines();
        }

        private void InitValue()
        {
            chessBoardAbsHeight = 630;
            chessBoardAbsWidth = 630;
            absOffset = 25;
            absRadius = 20;

            gameStart = false;

            //? Ratio of width
            rWidth = (float)chessBoard.Width / (chessBoardAbsWidth + 2 * absOffset);// chessBoardWidth;
            rHeight = (float)chessBoard.Height / (chessBoardAbsHeight + 2 * absOffset); // chessBoardHeight;

            chessBoardWidth = chessBoard.Width;
            chessBoardHeight = chessBoard.Height;

            offsetW = (int)(absOffset * rWidth);
            offsetH = (int)(absOffset * rHeight);

            player1 = false;
            player2 = true;

            form = new Dialog();
         }

        private void chessBoard_Click(object sender, EventArgs e)
        {
            if (!gameStart) return;

            short idx = getMousePos();
            if (idx != NONE)
            {
                if (!myChessBoard.PlayChess(idx)) return;
                if (!player1) AIplayer1.PlayChess(idx);
                if (!player2) AIplayer2.PlayChess(idx);

                short choose = myChessBoard.getStepNums() % 2 == 1 ? BLACK : WHITE;                            

                DrawChess(choose, (short)(idx / 15), (short)(idx % 15));
                if (myChessBoard.CheckVictory() != NONE)
                {
                    gameStart = false;
                    //? Confirm victory
                    form.showWinner(myChessBoard.CheckVictory() == WHITE);

                    myChessBoard.clear();
                    clearChess();
                    unlockButton();
                }

                if (choose == BLACK && !player2)
                {
                    idx = (short)AIplayer2.Run();
                    myChessBoard.PlayChess(idx);
                    AIplayer2.PlayChess(idx);
                    DrawChess(WHITE, (short)(idx / 15), (short)(idx % 15));
                }
                else if (choose == WHITE && !player1)
                {
                    idx = (short)AIplayer1.Run();
                    myChessBoard.PlayChess(idx);
                    AIplayer1.PlayChess(idx);
                    DrawChess(BLACK, (short)(idx / 15), (short)(idx % 15));
                }
                else return;

                if (myChessBoard.CheckVictory() != NONE)
                {
                    gameStart = false;
                    //? Confirm victory
                    form.showWinner(myChessBoard.CheckVictory() == WHITE);

                    myChessBoard.clear();
                    clearChess();
                    unlockButton();
                }

            }
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void player1_Click(object sender, EventArgs e)
        {
            player1 = !player1;
            if (player1 == false && player2 == false)
            {
                player2 = !player2;
            }
            button1.Text = player1? "玩家" : "电脑";
            button2.Text = player2 ? "玩家" : "电脑";
        }

        private void player2_Click(object sender, EventArgs e)
        {
            player2 = !player2;
            if (player1 == false && player2 == false)
            {
                player1 = !player1;
            }
            button1.Text = player1 ? "玩家" : "电脑";
            button2.Text = player2 ? "玩家" : "电脑";
        }

        private void start_Click(object sender, EventArgs e)
        {
            lockButton();

            clearChess();
            createChess();
            myChessBoard = new mainClass.ComChessBoard();

            //? (!player1 && !player2) must be false
            if (!player1)
            {
                AIplayer1 = new mainClass.AIPlayer();
                AIplayer1.SetColor(false);
                AIplayer1.SetPredDeep(6);
                AIplayer1.SetPredVCFDeep(0);
            }
            if (!player2)
            {
                AIplayer2 = new mainClass.AIPlayer();
                AIplayer2.SetColor(true);
                AIplayer2.SetPredDeep(6);
                AIplayer2.SetPredVCFDeep(0);
            }

            if (!player1)
            {
                int playIdx = AIplayer1.Run();
                myChessBoard.PlayChess(playIdx);
                AIplayer1.PlayChess(playIdx);
                if (!player2)
                    AIplayer2.PlayChess(playIdx);
                DrawChess(BLACK, (short)playIdx);
            }
            gameStart = true;
        }

        private void fail_Click(object sender, EventArgs e)
        {
            //? Confirm victory
            bool choose = myChessBoard.getStepNums() % 2 == 0;
            form.showWinner(choose);

            gameStart = false;
            unlockButton();
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            if (!gameStart) return;

            int idx = NONE;
            if (myChessBoard.GoBack(ref idx))
            {
                if (!player1) AIplayer1.GoBack(idx);
                if (!player2) AIplayer2.GoBack(idx);

                clearChess();
                createChess();
                int[] board = myChessBoard.getBoard();                
                for (short i = 0; i < BOARD_WIDTH * BOARD_WIDTH; i++)
                {
                    if (board[i] == 0) continue;

                    if (board[i] % 2 == 1)
                        DrawChess(BLACK, i, false);
                    else
                        DrawChess(WHITE, i, false);
                }
            }
        }
    }
}
