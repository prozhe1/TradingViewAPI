using System;
using TradingViewAPI;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("نماد موردنظر را وارد کنید: ");
            string symbol = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(symbol))
            {
                Console.WriteLine("⚠️ لطفاً یک نماد معتبر وارد کنید.");
                return;
            }

            Console.Write("تایم‌فریم را وارد کنید (مثلاً 1D یا 5m): ");
            string interval = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(interval))
            {
                Console.WriteLine("⚠️ لطفاً یک تایم‌فریم معتبر وارد کنید.");
                return;
            }

            string result = APIClient.GetSymbolData(symbol, interval);

            Console.WriteLine("\n📌 خروجی API:");
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ خطای برنامه: {ex.Message}");
        }
    }    
}
