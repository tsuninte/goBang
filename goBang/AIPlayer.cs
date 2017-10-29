using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goBang
{
    partial class mainClass
    {
        public class AIPlayer
        {
            private int[] board = new int[(BOARD_WIDTH + 2 * BOUNDRY) * (BOARD_WIDTH + 2 * BOUNDRY)];

            private int offset;
            private int[] m_board;
            private int[] m_scoreBoard = new int[BOARD_SIZE];
            private int m_uStrideB;       //? Stride of Board
            private int m_uStride;        //? Stride of scoreBoard
            private int m_uStepsNum;
            private int m_deep;
            private bool m_isWhite;
            private int[] m_value = new int[MAX_EST_DEEP + 1];
            private int[] m_idxList = new int[BOARD_SIZE];
            private int m_predDeep;
            private int m_predVCFDeep;
            private int predIdx;

            private int[] transTable = new int[BOARD_SIZE];   //? Transform scoreBoard index to board index
            private int[] randomTable = new int[BOARD_SIZE];

            public AIPlayer()
            {
                m_uStepsNum = 0;
                m_deep = 0;

                m_predDeep = 4; //? Must no more than MAX_EST_DEEP
                m_predVCFDeep = 8;

                m_uStrideB = BOARD_WIDTH + 2 * BOUNDRY;
                m_uStride = BOARD_WIDTH;

                memset(board, NONE, (BOARD_WIDTH + 2 * BOUNDRY) * (BOARD_WIDTH + 2 * BOUNDRY));
                memset(m_scoreBoard, 0, BOARD_SIZE);
                memset(m_value, 0, (m_predDeep + 1));

                m_board = board;
                offset = BOUNDRY * m_uStrideB + BOUNDRY;
                for (int uX = 0; uX < BOARD_WIDTH; uX++)
                {
                    for (int uY = 0; uY < BOARD_WIDTH; uY++)
                    {
                        int uIdxB = offset + uX * m_uStrideB + uY;
                        m_board[uIdxB] = 0;
                        transTable[uX * m_uStride + uY] = uIdxB;
                    }
                }

                Random ro = new Random();
                for (int ui = 0; ui < BOARD_SIZE; ui++)
                    randomTable[ui] = ro.Next() % 100000;
            }

            private int xUpdateValue(int x, int y)
            {
                bool turn = (m_uStepsNum + m_deep) % 2 == 1;
                bool gainOrLost = turn != m_isWhite;
                int gain = 0;
                short[] dir = ComDir;

                //? Get four patterns
                int[][] pattern = new int[4][];
                int patternHalfLen = 5;

                for (int ui = 0; ui < 4; ui++)
                {
                    pattern[ui] = new int[MAX_PATTERN_LEN];
                    for (int ulen = 1; ulen <= patternHalfLen; ulen++)
                    {
                        int dx = x + ulen * dir[ui << 1], dy = y + ulen * dir[1 + (ui << 1)];
                        pattern[ui][patternHalfLen - ulen] = m_board[offset + dx * m_uStrideB + dy];
                    }
                    pattern[ui][patternHalfLen] = m_board[offset + x * m_uStrideB + y];
                    for (int ulen = 1; ulen <= patternHalfLen; ulen++)
                    {
                        int dx = x + ulen * dir[(7 - ui) << 1], dy = y + ulen * dir[1 + ((7 - ui) << 1)];
                        pattern[ui][patternHalfLen + ulen] = m_board[offset + dx * m_uStrideB + dy];
                    }
                }

                for (int idir = 0; idir < 4; idir++)
                {
                    gain += xCheckPattern(pattern[idir], patternHalfLen, turn);
                    pattern[idir][patternHalfLen] = 0;
                    gain -= xCheckPattern(pattern[idir], patternHalfLen, turn);
                }

                return (gainOrLost ? gain : -gain);
            }

            private bool xCmp(int idxA, int idxB)
            {
                if (m_scoreBoard[idxA] != m_scoreBoard[idxB])
                    return m_scoreBoard[idxA] > m_scoreBoard[idxB];
                else if (randomTable[idxA] != randomTable[idxB])
                    return randomTable[idxA] > randomTable[idxB];
                else
                    return idxA > idxB;
            }

            private void xSort(int[] list, int fr, int to)
            {
                if (fr + 1 >= to) return;

                int mid = (fr + to) / 2;
                xSort(list, fr, mid);
                xSort(list, mid, to);

                int idx = fr, idx1 = fr, idx2 = mid;

                while (idx1 != mid && idx2 != to)
                    list[idx++] = (xCmp(m_idxList[idx1], m_idxList[idx2]) ? m_idxList[idx1++] : m_idxList[idx2++]);
                while (idx1 != mid)
                    list[idx++] = m_idxList[idx1++];
                while (idx2 != to)
                    list[idx++] = m_idxList[idx2++];
                //? idx must equal 'to'

                for (idx = fr; idx < to; idx++)
                    m_idxList[idx] = list[idx];
            }

            private int xCheckPattern(int[] pattern, int halfLen, bool myTurn)
            {
                int value = 0;
                int leftA1, leftAB, leftA2, leftA3, rightA1, rightAB, rightA2, rightA3;
                //?		A		B		O		A		A		O		A		O		A		B		B
                //?				|		|		|		|		|		|				|				|
                //?				leftA3	leftAB	leftA2	leftA1	now		rightA1			rightAB&rightA2	rightA3

                //? Parse pattern
                bool isfindA = false, isfindB = false;
                leftA1 = rightA1 = 0;
                leftAB = rightAB = 0;
                rightA2 = NONE;
                rightA3 = 2 * halfLen;
                for (int uIdx = halfLen; uIdx <= 2 * halfLen; uIdx++)
                {
                    if (pattern[uIdx] == NONE)
                    {
                        rightA3 = uIdx - 1;
                        break;
                    }
                    if (!isfindB)
                    {
                        if (pattern[uIdx] != 0 && (pattern[uIdx] % 2 == 1) == myTurn)
                        {
                            rightAB = uIdx;
                            rightA2 = uIdx;
                            if (!isfindA)
                            {
                                rightA1 = uIdx;
                                isfindA = true;
                            }
                        }
                        else if (pattern[uIdx] == 0)
                            rightAB = uIdx;
                        else
                            isfindB = true;
                    }
                    else if ((pattern[uIdx] != 0 && (pattern[uIdx] % 2 == 1) == myTurn))
                    {
                        rightA3 = uIdx - 1;
                        break;
                    }
                }

                isfindA = false;
                isfindB = false;
                leftA2 = NONE;
                leftA3 = 0;
                for (int uIdx = halfLen; uIdx >= 0; uIdx--)
                {
                    if (pattern[uIdx] == NONE)
                    {
                        leftA3 = uIdx + 1;
                        break;
                    }
                    if (!isfindB)
                    {
                        if (pattern[uIdx] != 0 && (pattern[uIdx] % 2 == 1) == myTurn)
                        {
                            leftAB = uIdx;
                            leftA2 = uIdx;
                            if (!isfindA)
                            {
                                leftA1 = uIdx;
                                isfindA = true;
                            }
                        }
                        else if (pattern[uIdx] == 0)
                            leftAB = uIdx;
                        else
                            isfindB = true;
                    }
                    else if (pattern[uIdx] != 0 && (pattern[uIdx] % 2 == 1) == myTurn)
                    {
                        leftA3 = uIdx + 1;
                        break;
                    }
                }

                //? Calculate my gain sroce (Attack score)
                value += xGetPateernValue(pattern, halfLen, leftAB, rightAB);

                //? Calculate competitor's lost score (Defend score)
                if (leftA2 == NONE && rightA2 == NONE)
                    value -= xGetPateernValue(pattern, halfLen, leftA3, rightA3);
                else if (leftA2 == NONE)
                    value -= xGetPateernValue(pattern, halfLen, leftA3, rightA1 - 1);
                else if (rightA2 == NONE)
                    value -= xGetPateernValue(pattern, halfLen, leftA1 + 1, rightA3);
                else
                {
                    if (leftA2 == halfLen)
                        value -= xGetPateernValue(pattern, halfLen, leftA3, leftA2 - 1);
                    if (rightA2 == halfLen)
                        value -= xGetPateernValue(pattern, halfLen, rightA2 + 1, rightA3);
                }

                return value;
            }

            private int xGetPateernValue(int[] pattern, int halfLen, int st, int ed)
            {
                int value = 0;
                if (ed - st + 1 >= 5)
                {
                    int gain = 0;
                    int num = 0;
                    bool isRightBound, isLeftBound;
                    for (int uIdx = st; uIdx < st + 4; uIdx++)
                        if (pattern[uIdx] != 0) num++;

                    for (int uIdx = st; uIdx <= ed - 4; uIdx++)
                    {
                        if (pattern[uIdx + 4] != 0) num++;

                        isLeftBound = (uIdx == st);
                        isRightBound = (uIdx + 4 == ed);
                        switch (num)
                        {
                            case 5:
                                gain += (int)Score.AAAAA;
                                break;
                            case 4:
                                if (pattern[uIdx] == 0)
                                    gain += (isRightBound ? (int)Score.AAAAB : (int)Score.AAAAO);
                                else if (pattern[uIdx + 4] == 0)
                                    gain += (isLeftBound ? (int)Score.AAAAB : (int)Score.AAAAO);
                                else
                                    gain += (int)Score.AAOAA;
                                break;
                            case 3:
                                if (pattern[uIdx] == 0 && pattern[uIdx] == 0)
                                    gain += (int)Score.AAAOO;
                                else if (pattern[uIdx] == 0)
                                {
                                    if (pattern[uIdx + 1] == 0)
                                        gain += (isRightBound ? (int)Score.AAABO : (int)Score.AAAOO);
                                    else
                                        gain += (isRightBound ? (int)Score.AOAAB : (int)Score.AOAAO);
                                }
                                else if (pattern[uIdx + 4] == 0)
                                {
                                    if (pattern[uIdx + 3] == 0)
                                        gain += (isLeftBound ? (int)Score.AAABO : (int)Score.AAAOO);
                                    else
                                        gain += (isLeftBound ? (int)Score.AOAAB : (int)Score.AOAAO);
                                }
                                else
                                    gain += pattern[uIdx + 2] != 0 ? (int)Score.AOAOA : (int)Score.AOOAA;
                                break;
                            case 2:
                                if (pattern[uIdx] != 0 && pattern[uIdx + 4] != 0)
                                    gain += (int)Score.AOOOA;
                                else if (pattern[uIdx] != 0)
                                {
                                    if (pattern[uIdx + 1] != 0)
                                        gain += (isLeftBound ? (int)Score.AABOO : (int)Score.AAOOO);
                                    else if (pattern[uIdx + 2] != 0)
                                        gain += (isLeftBound ? (int)Score.AOABO : (int)Score.AOAOO);
                                    else
                                        gain += (isLeftBound ? (int)Score.AOOAB : (int)Score.AOOAO);
                                }
                                else if (pattern[uIdx + 4] != 0)
                                {
                                    if (pattern[uIdx + 3] != 0)
                                        gain += (isRightBound ? (int)Score.AABOO : (int)Score.AAOOO);
                                    else if (pattern[uIdx + 2] != 0)
                                        gain += (isRightBound ? (int)Score.AOABO : (int)Score.AOAOO);
                                    else
                                        gain += (isRightBound ? (int)Score.AOOAB : (int)Score.AOOAO);
                                }
                                else
                                    gain += (pattern[uIdx + 2] != 0 ? (int)Score.AAOOO : (int)Score.AOAOO);
                                break;
                            case 1:
                                if (pattern[uIdx] != 0)
                                    gain += isLeftBound ? (int)Score.ABOOO : (int)Score.AOOOO;
                                else if (pattern[uIdx + 4] != 0)
                                    gain += isRightBound ? (int)Score.ABOOO : (int)Score.AOOOO;
                                else
                                    gain += (int)Score.AOOOO;
                                break;
                            default:
                                //? None
                                break;
                        }

                        if (pattern[uIdx] != 0) num--;
                    }

                    value += gain;
                }
                //? else: No gain

                return value;
            }

            private void xUpdateScoreTable(int x, int y)
            {
                short[] dir = ComDir;
                int dx, dy, ddx, ddy;
                int left, right;
                int[] score = { 2, 1 };

                for (int ui = 0; ui < 4; ui++)
                {
                    right = left = 0;
                    ddx = dir[ui << 1];
                    ddy = dir[1 + (ui << 1)];
                    dx = x;
                    dy = y;
                    for (int ulen = 1; ulen < 5; ulen++)
                    {
                        dx += ddx;
                        dy += ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] != 0 && m_board[offset + dx * m_uStrideB + dy] != NONE &&
                            (m_board[offset + dx * m_uStrideB + dy] % 2 == 1) == (m_board[offset + x * m_uStrideB + y] % 2 == 1))
                            right = ulen;
                        else break;
                    }

                    dx = x;
                    dy = y;
                    for (int ulen = 1; ulen < 5; ulen++)
                    {
                        dx -= ddx;
                        dy -= ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] != 0 && m_board[offset + dx * m_uStrideB + dy] != NONE &&
                            (m_board[offset + dx * m_uStrideB + dy] % 2 == 1) == (m_board[offset + x * m_uStrideB + y] % 2 == 1))
                            left = ulen;
                        else break;
                    }

                    dx = x + (right + 1) * ddx;
                    dy = y + (right + 1) * ddy;
                    if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        m_scoreBoard[dx * m_uStride + dy] += (1 << 1 + right + left) - (right != 0 ? (1 << right) : 0);

                    dx = x - (left + 1) * ddx;
                    dy = y - (left + 1) * ddy;
                    if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        m_scoreBoard[dx * m_uStride + dy] += (1 << 1 + right + left) - (left != 0 ? (1 << left) : 0);

                    for (int ulen = right + 2; ulen <= SEARCH_RANGE; ulen++)
                    {
                        dx = x + ulen * ddx;
                        dy = y + ulen * ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] == 0)
                            m_scoreBoard[dx * m_uStride + dy] += score[ulen - 1];
                        else break;
                    }

                    for (int ulen = left + 2; ulen <= SEARCH_RANGE; ulen++)
                    {
                        dx = x - ulen * ddx;
                        dy = y - ulen * ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        {
                            m_scoreBoard[dx * m_uStride + dy] += score[ulen - 1];
                        }
                        else break;
                    }
                }
            }

            private void xRestoreScoreTable(int x, int y)
            {
                short[] dir = ComDir;
                int dx, dy, ddx, ddy;
                int left, right;
                int[] score = { 2, 1 };

                for (int ui = 0; ui < 4; ui++)
                {
                    right = left = 0;
                    ddx = dir[ui << 1];
                    ddy = dir[1 + (ui << 1)];
                    dx = x;
                    dy = y;
                    for (int ulen = 1; ulen < 5; ulen++)
                    {
                        dx += ddx;
                        dy += ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] != 0 && m_board[offset + dx * m_uStrideB + dy] != NONE &&
                            (m_board[offset + dx * m_uStrideB + dy] % 2 == 1) == (m_board[offset + x * m_uStrideB + y] % 2 == 1))
                            right = ulen;
                        else break;
                    }
                    dx = x;
                    dy = y;
                    for (int ulen = 1; ulen < 5; ulen++)
                    {
                        dx -= ddx;
                        dy -= ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] != 0 && m_board[offset + dx * m_uStrideB + dy] != NONE &&
                            (m_board[offset + dx * m_uStrideB + dy] % 2 == 1) == (m_board[offset + x * m_uStrideB + y] % 2 == 1))
                            left = ulen;
                        else break;
                    }

                    dx = x + (right + 1) * ddx;
                    dy = y + (right + 1) * ddy;
                    if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        m_scoreBoard[dx * m_uStride + dy] -= (1 << 1 + right + left) - (right != 0 ? (1 << right) : 0);

                    dx = x - (left + 1) * ddx;
                    dy = y - (left + 1) * ddy;
                    if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        m_scoreBoard[dx * m_uStride + dy] -= (1 << 1 + right + left) - (left != 0 ? (1 << left) : 0);

                    for (int ulen = right + 2; ulen <= SEARCH_RANGE; ulen++)
                    {
                        dx = x + ulen * ddx;
                        dy = y + ulen * ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] == 0)
                            m_scoreBoard[dx * m_uStride + dy] -= score[ulen - 1];
                        else break;
                    }

                    for (int ulen = left + 2; ulen <= SEARCH_RANGE; ulen++)
                    {
                        dx = x - ulen * ddx;
                        dy = y - ulen * ddy;
                        if (m_board[offset + dx * m_uStrideB + dy] == 0)
                        {
                            m_scoreBoard[dx * m_uStride + dy] -= score[ulen - 1];
                        }
                        else break;
                    }
                }
            }

            private int xAlpha(int alpha, int beta, int deep)
            {
                int value = GetValue(deep);
                if (value <= -(int)Score.AAAAA / 2)
                    return -((int)Score.AAAAA + m_predDeep - deep);
                if (deep == m_predDeep) return value;

                int[] idxList = new int[BOARD_SIZE];
                int listNum = GetChoice(idxList);

                for (int ui = 0; ui < listNum; ui++)
                {
                    int playIdx = idxList[ui];
                    AddChess(playIdx / m_uStride, playIdx % m_uStride);
                    int score = xBeta(alpha, beta, deep + 1);
                    DeleteChess(playIdx / m_uStride, playIdx % m_uStride);
                    if (score > alpha)
                    {
                        alpha = score;
                        if (deep == 0) predIdx = playIdx;
                    }

                    if (alpha >= beta) break;
                }

                return alpha;
            }

            private int xBeta(int alpha, int beta, int deep)
            {
                int value = GetValue(deep);
                if (value >= (int)Score.AAAAA / 2) return (int)Score.AAAAA + m_predDeep - deep;
                if (deep == m_predDeep) return value;

                int[] idxList = new int[BOARD_SIZE];
                int listNum = GetChoice(idxList);

                for (int ui = 0; ui < listNum; ui++)
                {
                    int playIdx = idxList[ui];
                    AddChess(playIdx / m_uStride, playIdx % m_uStride);
                    int score = xAlpha(alpha, beta, deep + 1);
                    DeleteChess(playIdx / m_uStride, playIdx % m_uStride);

                    if (score < beta)
                        beta = score;

                    if (alpha >= beta) break;
                }

                return beta;
            }

            private int xVCFAlpha(int alpha, int beta, int deep, bool attackOrDefend)
            {
                int value = GetValue(deep);
                if (value <= -(int)Score.AAAAA / 2)
                    return -((int)Score.AAAAA + m_predVCFDeep - deep);
                if (deep == m_predVCFDeep) return value;

                int[] idxList = new int[BOARD_SIZE];
                int threshold = 7;
                int listNum = GetChoice(idxList, threshold);

                for (int ui = 0; ui < listNum; ui++)
                {
                    int playIdx = idxList[ui];
                    AddChess(playIdx / m_uStride, playIdx % m_uStride);
                    int score = xVCFBeta(alpha, beta, deep + 1, attackOrDefend);
                    DeleteChess(playIdx / m_uStride, playIdx % m_uStride);

                    if (score > alpha)
                    {
                        alpha = score;
                        if (deep == 0) predIdx = playIdx;
                    }

                    if (alpha >= beta) break;
                }

                return alpha;
            }

            private int xVCFBeta(int alpha, int beta, int deep, bool attackOrDefend)
            {
                int value = GetValue(deep);
                if (value >= (int)Score.AAAAA / 2) return (int)Score.AAAAA + m_predVCFDeep - deep;
                if (deep == m_predVCFDeep) return value;

                int[] idxList = new int[BOARD_SIZE];
                int threshold = 7;
                int listNum = GetChoice(idxList, threshold);

                for (int ui = 0; ui < listNum; ui++)
                {
                    int playIdx = idxList[ui];
                    AddChess(playIdx / m_uStride, playIdx % m_uStride);
                    int score = xVCFAlpha(alpha, beta, deep + 1, attackOrDefend);
                    DeleteChess(playIdx / m_uStride, playIdx % m_uStride);

                    if (score < beta)
                        beta = score;

                    if (alpha >= beta) break;
                }

                return beta;
            }

            public int GetValue(int deep) { return m_value[deep]; }

            public int GetChoice(int[] idxList, int threshold = 0)
            {
                if ((m_uStepsNum + m_deep) == 0 && threshold == 0)
                {
                    idxList[0] = BOARD_WIDTH / 2 * (BOARD_WIDTH + 1);
                    return 1;
                }

                int listNum = 0;
                for (int uIdx = 0; uIdx < BOARD_SIZE; uIdx++)
                    if (m_board[transTable[uIdx]] == 0 && m_scoreBoard[uIdx] > threshold)
                        m_idxList[listNum++] = uIdx;

                xSort(idxList, 0, listNum);
                return listNum;
            }

            public void SetPredDeep(int deep) { m_predDeep = deep; }

            public void SetPredVCFDeep(int deep) { m_predVCFDeep = deep; }

            public void SetColor(bool isWhite = false) { m_isWhite = isWhite; }

            public void PlayChess(int x, int y)
            {
                m_board[offset + x * m_uStrideB + y] = ++m_uStepsNum;
                //? m_deep must be 0
                m_value[m_deep] += xUpdateValue(x, y);
                xUpdateScoreTable(x, y);
            }

            public void PlayChess(int uIdx) { PlayChess(uIdx / m_uStride, uIdx % m_uStride); }

            public void GoBack(int x, int y)
            {
                //? m_deep must be 0
                m_value[m_deep] -= xUpdateValue(x, y);
                xRestoreScoreTable(x, y);
                m_board[offset + x * m_uStrideB + y] = 0;
                --m_uStepsNum;
            }

            public void GoBack(int uIdx) { GoBack(uIdx / m_uStride, uIdx % m_uStride); }

            public void AddChess(int x, int y)
            {
                m_board[offset + x * m_uStrideB + y] = m_uStepsNum + (++m_deep);
                //? m_deep must no greater than MAX_EST_DEEP
                m_value[m_deep] = m_value[m_deep - 1] + xUpdateValue(x, y);
                xUpdateScoreTable(x, y);
            }

            public void DeleteChess(int x, int y)
            {
                xRestoreScoreTable(x, y);
                m_board[offset + x * m_uStrideB + y] = 0;
                m_deep--;
            }

            public int Run()
            {
                int tmpDeep = m_predVCFDeep;
                int value;

                //for (m_predVCFDeep = 2; m_predVCFDeep <= tmpDeep; m_predVCFDeep += 2)
                //{
                //    value = xVCFAlpha(MIN_INT, MAX_INT, 0, ATTACK);
                //    if (value >= (int)Score.AAAAA)
                //    {
                //        m_predVCFDeep = tmpDeep;
                //        return predIdx;
                //    }
                //}
                //m_predVCFDeep = tmpDeep;
                //? Assert(m_predVCFDeep == tmpDeep)

                tmpDeep = m_predDeep;
                for (m_predDeep = 2; m_predDeep < tmpDeep; m_predDeep += 2)
                {
                    value = xAlpha(MIN_INT, MAX_INT, 0);
                    if (value >= (int)Score.AAAAA)
                    {
                        m_predDeep = tmpDeep;
                        return predIdx;
                    }
                }

                //? Assert(m_predDeep == tmpDeep)

                xAlpha(MIN_INT, MAX_INT, 0);
                return predIdx;
            }
        }
    }
}