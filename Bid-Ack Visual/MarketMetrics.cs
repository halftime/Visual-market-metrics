using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bid_Ack_Visual;

public class MarketMetrics
{
    private decimal _bidPrice;
    private int _bidPriceDecimals;
    public decimal BidPrice
    {
        get { return _bidPrice; }
        set
        {
            if (value > _askPrice) throw new Exception("Bid price higher than ask.");
            _bidPriceDecimals = GetRoundingAccuracy(value);
            _bidPrice = value;
        }
    }
    private decimal _askPrice;
    private int _askPriceDecimals;
    public decimal AskPrice
    {
        get { return _askPrice; }
        set
        {
            if (value < _bidPrice) throw new Exception("Ask price smaller than bid.");
            _askPriceDecimals = GetRoundingAccuracy(value);
            _askPrice = value;
        }
    }
    public decimal MidPrice => (BidPrice + AskPrice) / 2;

    public decimal BidAskSpreadPrice => AskPrice - BidPrice;

    public decimal BidAskSprdPct => AskPrice == 0 ? 1 : (AskPrice - BidPrice) / AskPrice;

    public int RoundingAccuracy => new int[] { _askPriceDecimals, _bidPriceDecimals }.Max();

    public override string ToString()
    {
        return $"[B]id: {BidPrice} \t \t [M]id: {MidPrice} \t \t [A]sk: {AskPrice} \n" +
            $"Spread (ask-bid): {BidAskSpreadPrice} => ~{100 * Math.Round(BidAskSprdPct * 1000) / 1000}%";
    }

    public string ToVisualStr(decimal? lowerLimit)
    {
        // Lower    <->    Higher
        // LL - Bid -- Mid --- Ask
        //
        int totalUnitSpread = 0;
        if (lowerLimit > BidPrice) { throw new Exception("LL may not exceed BidPrice"); }
        if (lowerLimit == null)
        {
            lowerLimit = Math.Floor(BidPrice);
        }
        totalUnitSpread = GetUnitSpread((decimal)lowerLimit, AskPrice);

        decimal LLtoBidPct = (decimal)GetUnitSpread((decimal)lowerLimit, BidPrice) / (decimal)totalUnitSpread;
        decimal BidToMidPct = (decimal)GetUnitSpread(BidPrice, MidPrice) / (decimal)totalUnitSpread;
        decimal MidToAskPct = (decimal)GetUnitSpread(MidPrice, AskPrice) / (decimal)totalUnitSpread;

        string strVisual = "|";
        string strNumbers = lowerLimit.ToString();

        for (int i = 0; i < (int)(LLtoBidPct * 100); i++)
        {
            strVisual += "-";
            if (i <= (int)(LLtoBidPct * 100) - lowerLimit.ToString().Count()) strNumbers += " ";
        }

        strVisual += "B";
        strNumbers += BidPrice.ToString();

        for (int i = 0; i < (int)(BidToMidPct * 100); i++)
        {
            strVisual += "=";
            if (i <= (int)(BidToMidPct * 100) - BidPrice.ToString().Count() - MidPrice.ToString().Count() / 2) strNumbers += " ";
        }
        strVisual += "M";
        strNumbers += MidPrice.ToString();

        for (int i = 0; i < (int)(MidToAskPct * 100); i++)
        {
            strVisual += "=";
            if (i <= (int)(MidToAskPct * 100) - AskPrice.ToString().Count() / 2 - MidPrice.ToString().Count() / 2) strNumbers += " ";
        }
        strVisual += "A";
        strNumbers += AskPrice.ToString();

        return $"{strVisual}\n{strNumbers}\n\n";
    }

    internal int GetRoundingAccuracy(decimal number)
    {
        string numberStr = number.ToString();
        if (!numberStr.Contains('.')) return 0;
        return numberStr.Split('.')[1].TrimEnd('0').Count();
    }

    internal int GetUnitSpread(decimal lower, decimal upper)
    {
        decimal spread = upper - lower;
        return (int)Math.Round(spread * decimal.Parse(Math.Pow(10, RoundingAccuracy).ToString()));
    }
}
