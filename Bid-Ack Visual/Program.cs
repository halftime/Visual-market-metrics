namespace Bid_Ack_Visual;
internal class Program
{
    static void Main(string[] args)
    {
        MarketMetrics MSmallSpread = new MarketMetrics() { AskPrice = 99.69M, BidPrice = 99.1234M };
        Console.WriteLine(MSmallSpread.ToString());
        Console.WriteLine(MSmallSpread.ToVisualStr(98));

        MarketMetrics MBigSpread = new MarketMetrics() { AskPrice = 103.333M, BidPrice = 98.67M };
        Console.WriteLine(MBigSpread.ToString());
        Console.WriteLine(MBigSpread.ToVisualStr(98));
    }
}
