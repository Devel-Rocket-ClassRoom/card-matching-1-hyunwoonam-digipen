using System;


while (true)
{
    Game g = new Game();

    g.Play();

    Console.Write("새 게임을 하시겠습니까? (Y/N): ");
    string retry = Console.ReadLine().Trim().ToUpper();
    if (retry != "Y")
    {
        Console.WriteLine("게임을 종료합니다.");
        break;
    }
}