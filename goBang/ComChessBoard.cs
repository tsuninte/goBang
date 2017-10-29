//#define FAIL_CONT_6

using System.Collections;

namespace goBang
{
    partial class mainClass
    {
        public class ComChessBoard
        {
            private int[] board = new int[BOARD_SIZE];
            private int m_uStepsNum;
            private int m_uStride;
            private Stack sLastPlay;

            public ComChessBoard()
            {
                m_uStepsNum = 0;
                m_uStride = BOARD_WIDTH;
                sLastPlay = new Stack();
            }

            public void clear()
            {
                memset(board, 0, BOARD_SIZE);
                while (m_uStepsNum > 0)
                {
                    m_uStepsNum--;
                    sLastPlay.Pop();                    
                }
            }

            public int getStepNums()
            {
                return m_uStepsNum;
            }

            public int[] getBoard()
            {
                return board;
            }

            public bool PlayChess(int x, int y)
            {
                if (board[x * m_uStride + y] != 0)
                    return false;

                board[x * m_uStride + y] = ++m_uStepsNum;
                sLastPlay.Push(x * m_uStride + y);
                return true;
            }

            public bool PlayChess(int idx) { return PlayChess(idx / m_uStride, idx % m_uStride); }

            public bool ShowPlay(ref int ux, ref int uy)
            {
                if (m_uStepsNum == 0)
                    return false;

                int lastPlay = (int)(sLastPlay.Peek());
                ux = lastPlay / m_uStride;
                uy = lastPlay % m_uStride;
                return true;
            }

            public bool ShowPlay(ref int uIdx)
            {
                if (m_uStepsNum == 0)
                    return false;

                uIdx = (int)(sLastPlay.Peek());
                return true;
            }

            public bool GoBack(ref int ux, ref int uy)
            {
                if (m_uStepsNum == 0)
                    return false;

                int lastPlay = (int)(sLastPlay.Pop());
                ux = lastPlay / m_uStride;
                uy = lastPlay % m_uStride;
                board[ux * m_uStride + uy] = 0;
                m_uStepsNum--;

                return true;
            }

            public bool GoBack(ref int uIdx)
            {
                if (m_uStepsNum == 0)
                    return false;

                uIdx = (int)(sLastPlay.Pop());
                board[uIdx] = 0;
                m_uStepsNum--;

                return true;
            }

            public int CheckVictory()
            {
                int lastPlay = (int)(sLastPlay.Peek());
                int x = lastPlay / m_uStride;
                int y = lastPlay % m_uStride;
                short[] dir = ComDir;

                int[] cont = new int[8];
                for (int ui = 0; ui < 8; ui++)
                {
                    for (int ulen = 1; ulen <= 5; ulen++)
                    {
                        int dx = x + ulen * dir[ui << 1], dy = y + ulen * dir[1 + (ui << 1)];
                        if (dx >= 0 && dx < BOARD_WIDTH && dy >= 0 && dy < BOARD_WIDTH && board[dx * m_uStride + dy] > 0 &&
                            (board[dx * m_uStride + dy] & 1) == (board[x * m_uStride + y] & 1))
                        {
                            cont[ui] = ulen;
                        }
                        else break;
                    }
                }

                if (cont[0] + cont[7] == 4 || cont[1] + cont[6] == 4 || cont[2] + cont[5] == 4 || cont[3] + cont[4] == 4)
                    return (board[x * m_uStride + y] % 2 == 1) ? BLACK : WHITE; //? Black plays first

#if  FAIL_CONT_6
                if (cont[0] + cont[7] >= 5 || cont[1] + cont[6] >= 5 || cont[2] + cont[5] >= 5 || cont[3] + cont[4] >= 5)
                    return (board[x * m_uStride + y] % 2 == 1) ? WHITE : BLACK; //? Fail when continuous 6
#endif

                return NONE;
            }
        }
    }
}
