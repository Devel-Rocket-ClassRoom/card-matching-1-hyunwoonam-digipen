using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

enum Difficulty
{
    easy = 1,
    normal,
    hard,
}

class Game
{
    deck d;

    public Game()
    {
        Difficulty();
    }

    public void Play()
    {
        d.shuffle();
        while (d.tryCount < d.canTry)
        {
            Console.Clear();
            d.PrintBoard();

            d.choose("첫",out int r1, out int c1, -1, -1);

            Console.Clear();
            d.PrintBoard(r1,c1);

            d.choose("두", out int r2, out int c2, r1, c1);

            d.tryCount++;

            Console.Clear();
            d.PrintBoard(r1, c1, r2, c2);

            if (d.board[r1, c1] == d.board[r2, c2])
            {
                Console.WriteLine("짝을 찾았습니다!");
                d.matchBoard[r1, c1] = true;
                d.matchBoard[r2, c2] = true;
                d.PairCount++;
                
                Thread.Sleep(1500);
            }
            else
            {
                Console.WriteLine("짝이 맞지 않습니다...\n");
                Thread.Sleep(1500);
                continue;
            }

            if (d.PairCount == d.pair)
            {
                Console.WriteLine("=== 게임 클리어! 축하합니다! ===");
                break;
            }
        }

        Console.WriteLine("=== 게임 오버! 시도 횟수를 모두 소진했습니다. ===");
        Console.WriteLine($"찾은 쌍: {d.PairCount}/{d.pair}\n");
    }

    public void Difficulty()
    {
        Console.WriteLine("난이도를 선택하세요: ");
        Console.WriteLine("1.쉬움(2x4)");
        Console.WriteLine("2.보통(4x4)");
        Console.WriteLine("3.어려움(4x6)\n");

        while (true)
        {
            Console.Write("선택: ");

            if (int.TryParse(Console.ReadLine(), out int value) &&
                Enum.IsDefined(typeof(Difficulty), value))
            {
                d = new deck(value);
                break;
            }
            else
            {
                Console.WriteLine("1, 2, 3 중 하나를 입력하세요.");
                continue;
            }
        }
    }

}

class deck
{
    public int[,] board {get; private set; }
    public bool[,] matchBoard { get; set; }
    public int pair { get; private set; }
    public int PairCount { get; set; }
    public int row { get; private set; }
    public int col { get; private set; }
    public int canTry { get; private set; }
    public int tryCount { get; set; }
    public int[] cards { get; private set; }

    public deck(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                row = 2;
                canTry = 1;
                break;
            case 2:
                row = 4;
                canTry = 20;
                break;
            case 3:
                row = 6;
                canTry = 30;
                break;
        }

        col = 4;
        board = new int[row, col];
        matchBoard = new bool[row, col];
        pair = (row * col) / 2;
        cards = new int[(row * col)];
        tryCount = 0;
        PairCount = 0;
    }
    
    public void shuffle()
    {
        Console.Clear();

        Random rand = new Random();

        int input = 1;
        int index = 0;

        for (int i = 0; i < cards.Length; i++)
        {
            if(i % 2 == 0)
            {
                input++;
            }
            cards[i] = input;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            int randomIdx = rand.Next(i, cards.Length);

            int temp = cards[i];
            cards[i] = cards[randomIdx];
            cards[randomIdx] = temp;
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                board[i, j] = cards[index++];
            }
        }

        Console.WriteLine("카드를 섞는 중...");
        Thread.Sleep(1500);
    }

    public void PrintBoard(int r1 = -1, int c1 = -1, int r2 = -1, int c2 = -1)
    {
        Console.WriteLine("    1열 2열 3열 4열");
        for (int i = 0; i < board.GetLength(0); i++)
        {
            Console.Write($"{i + 1}행  ");

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (matchBoard[i,j])
                {
                    Console.Write($"{board[i, j]}  ");
                }
                else if ((i == r1 && j == c1) || (i == r2 && j == c2))
                {
                    Console.Write($"[{board[i, j]}]  ");
                }
                else
                {
                    Console.Write($"**  ");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine($"시도 횟수: {tryCount}/{canTry} | 찾은 쌍: {PairCount}/{pair}\n");
    }

    public void choose(string s,out int indexR, out int indexC, int prevR, int prevC)
    {

        while (true)
        {
            Console.Write($"{s} 번째 카드를 선택하세요 (행 열): ");

            string input = Console.ReadLine();

            string[] parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts == null || parts.Length != 2 ||
                !int.TryParse(parts[0], out int r) || !int.TryParse(parts[1], out int c))
            {
                Console.WriteLine("행과 열을 공백으로 구분하여 입력하세요. (예: 1 3)");
                continue;
            }

            if (r < 1 || r > row || c < 1 || c > col)
            {
                Console.WriteLine($"행은 1~{row}, 열은 1~{col} 범위로 입력하세요.");
                continue;
            }

            indexR = r - 1;
            indexC = c - 1;

            if (matchBoard[indexR, indexC] == true)
            {
                Console.WriteLine("이미 짝을 찾은 카드입니다. 다른 카드를 선택하세요.");
                continue;
            }

            if (indexR == prevR && indexC == prevC)
            {
                Console.WriteLine("방금 선택한 카드입니다. 다른 카드를 선택하세요.");
                continue;
            }

            break;
        }

    }
}