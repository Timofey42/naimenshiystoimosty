using System;

class Program
{
    static void Main()
    {
        // Указания пользователю
        Console.WriteLine("Программа решает транспортную задачу методом наименьшей стоимости.");
        Console.WriteLine("Матрица должна быть размером 5x3.");

        // Ввод матрицы затрат
        int rows = 3; // Поставщики
        int cols = 5; // Потребители
        int[,] costs = new int[rows, cols];

        Console.WriteLine("Введите матрицу затрат (5 столбцов и 3 строки):");
        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"Введите {cols} значений через пробел для строки {i + 1}:");
            string[] input = Console.ReadLine().Split(' ');
            for (int j = 0; j < cols; j++)
            {
                costs[i, j] = int.Parse(input[j]);
            }
        }

        // Ввод объемов поставок
        int[] supply = new int[rows];
        Console.WriteLine($"Введите объемы поставок для {rows} поставщиков через пробел (не более {rows} значений):");
        string[] supplyInput = Console.ReadLine().Split(' ');
        for (int i = 0; i < rows; i++)
        {
            supply[i] = int.Parse(supplyInput[i]);
        }

        // Ввод объемов потребностей
        int[] demand = new int[cols];
        Console.WriteLine($"Введите объемы потребностей для {cols} потребителей через пробел (не более {cols} значений):");
        string[] demandInput = Console.ReadLine().Split(' ');
        for (int j = 0; j < cols; j++)
        {
            demand[j] = int.Parse(demandInput[j]);
        }

        // Решение задачи
        int[,] result = SolveLeastCostMethod(costs, supply, demand);

        // Вывод результатов
        Console.WriteLine("Результирующая таблица перевозок:");
        PrintMatrix(result);

        // Расчет итогового значения
        int totalCost = CalculateTotalCost(result, costs);
        Console.WriteLine($"Общая стоимость перевозок: {totalCost}");

        // Ожидание перед завершением
        Console.WriteLine("Нажмите Enter, чтобы завершить...");
        Console.ReadLine();
    }

    static int[,] SolveLeastCostMethod(int[,] costs, int[] supply, int[] demand)
    {
        int rows = supply.Length;
        int cols = demand.Length;
        int[,] result = new int[rows, cols];
        bool[,] used = new bool[rows, cols];

        while (true)
        {
            // Найти минимальную стоимость среди неиспользованных ячеек
            int minCost = int.MaxValue;
            int minRow = -1, minCol = -1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!used[i, j] && costs[i, j] < minCost)
                    {
                        minCost = costs[i, j];
                        minRow = i;
                        minCol = j;
                    }
                }
            }

            if (minRow == -1 || minCol == -1)
                break; // Все ячейки использованы

            // Распределить объемы по найденной минимальной стоимости
            int allocation = Math.Min(supply[minRow], demand[minCol]);
            result[minRow, minCol] = allocation;
            supply[minRow] -= allocation;
            demand[minCol] -= allocation;
            used[minRow, minCol] = true;

            // Если поставка или потребность исчерпаны, пометить строку/столбец как использованные
            if (supply[minRow] == 0)
            {
                for (int j = 0; j < cols; j++)
                    used[minRow, j] = true;
            }

            if (demand[minCol] == 0)
            {
                for (int i = 0; i < rows; i++)
                    used[i, minCol] = true;
            }
        }

        return result;
    }

    static int CalculateTotalCost(int[,] result, int[,] costs)
    {
        int totalCost = 0;
        int rows = result.GetLength(0);
        int cols = result.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (result[i, j] != 0)
                {
                    totalCost += result[i, j] * costs[i, j];
                }
            }
        }

        return totalCost;
    }

    static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}
