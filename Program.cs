using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("수식을 입력해주세요 :");
        string input = Console.ReadLine();
        string current = input;

        while (true) 
        { 
            char[] chars = current.ToCharArray();

            int startGalho = 0, endGalho = 0;

            for (int i = 0; i < current.Length; i++)
            {
                if (chars[i] == '(') 
                {
                    startGalho = i;
                }
                else if (chars[i] == ')')
                {
                    endGalho = i;
                    string galhoString = current.Substring(startGalho + 1, endGalho - startGalho - 1); // 괄호 안에 문자열 추출
                    decimal galhoResult = StringMagicSolve(galhoString); // 추출한 문자열 계산
                    // 괄호 있던 자리에 계산한 결과로 바꾸기
                    current = current.Replace(current.Substring(startGalho, endGalho - startGalho + 1), galhoResult.ToString());
                    startGalho = 0;
                    endGalho = 0;
                    i = 0;
                }
            }
            if (chars.Contains('(') && chars.Contains(')')) continue;
            break;
        }

        decimal result = StringMagicSolve(current);
        Console.WriteLine("결과: " + result);
    }

    // 문자열 받아서 *, / 먼저 계산 후 +, - 계산하는 마법의 함수
    static decimal StringMagicSolve(string input)
    {
        decimal solveValue = 0;

        input = input.Replace(" ", string.Empty); // 공백 제거
        if (input.StartsWith("+")) input = input.Substring(1);
        List<string> solveList = Regex.Split(input, @"([+\-*/])").ToList();
        solveList = MinusChangeList(solveList);

        while (true)
        {
            for (int i = 0; i < solveList.Count; i++) // 1순위 곱하기 나누기
            {
                if (solveList[i] == "*") // 곱하기
                {
                    decimal value = decimal.Parse(solveList[i - 1]) * decimal.Parse(solveList[i + 1]);
                    solveList[i - 1] = value.ToString();
                    solveList.RemoveAt(i);
                    solveList.RemoveAt(i);
                    continue;
                }
                else if (solveList[i] == "/") // 나누기
                {
                    decimal value = decimal.Parse(solveList[i - 1]) / decimal.Parse(solveList[i + 1]);
                    solveList[i - 1] = value.ToString();
                    solveList.RemoveAt(i);
                    solveList.RemoveAt(i);
                    continue;
                }
            }
            if (!solveList.Contains("*") && !solveList.Contains("/")) break;
        }
        while (true)
        {
            for (int i = 0; i < solveList.Count; i++) // 2순위 더하기 빼기 
            {
                if (solveList[i] == "+") // 더하기
                {
                    decimal value = decimal.Parse(solveList[i - 1]) + decimal.Parse(solveList[i + 1]);
                    solveList[i - 1] = value.ToString();
                    solveList.RemoveAt(i);
                    solveList.RemoveAt(i);
                    continue;
                }
                else if (solveList[i] == "-") //빼기
                {
                    decimal value = decimal.Parse(solveList[i - 1]) - decimal.Parse(solveList[i + 1]);
                    solveList[i - 1] = value.ToString();
                    solveList.RemoveAt(i);
                    solveList.RemoveAt(i);
                    continue;
                }
            }
            if (solveList.Count == 1) break;
        }
        solveValue = decimal.Parse(solveList[0]);
        return solveValue;
    }

    static List<string> MinusChangeList(List<string> input) // 숫자를 음수로 처리할 수 있게하는 함수
    {
        List<string> resultList = input;
        if (resultList[0] == string.Empty && resultList[1] == "-") // 첫 번째 값 -일 때 변경
        {
            resultList.RemoveAt(0);
            resultList[0] = (-decimal.Parse(resultList[1])).ToString();
            resultList.RemoveAt(1);

        }

        for (int i = 0; i < resultList.Count; i++) // 앞에 다른 부호 있을 시 숫자를 -로 변경
        {
            if(i < 2) continue;
            if (resultList[i] == "-" && AnotherBuho(resultList[i - 2])) // 연산자 뒤에 -가 올 때
            {
                resultList.RemoveAt(i - 1);
                resultList[i - 1] = (-decimal.Parse(resultList[i])).ToString();
                resultList.RemoveAt(i);
            }
        }

        return resultList;
    }

    static bool AnotherBuho(string input) // -이외에 다른 부호가 있는지 확인하는 함수
    {
        if (input == "+" || input == "*" || input == "/") return true;
        else return false;
    }
}