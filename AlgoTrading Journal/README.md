Resources (hummingbot)
- https://hummingbot.org/academy-content/guide-to-the-avellaneda--stoikov-strategy/
- https://hummingbot.org/academy-content/how-to-get-good-at-market-making/
- https://hummingbot.org/faq/


- # Definitions:
    - Trading capital : <= sum of the value of trading inventory available, expressed in stable value eg: 1000 euro = 1017 USDC
        - Use this value to calculate inventory %s 
    - Minimal bidask spread (%): >= 2x min maker fee (%), so that profit >= 1/2 spread = maker fee
        - Generally the MM collects half the bidask spread % as profit on provided liquidity (in the long run)
        - The MM is incentivised by exchange to provide volume => more volome => lower maker fees (Providing volume at breakeven is fine)
    - Defining a continues market process by parameters:
      - Ticker: [LTC-EUR] => market between 2 assets: LTC & EUR
         - LTC (Litecoin) : (crypto)
         - EUR (Euro)     : (stable)
      - Orderbook is a sorted collection of (Price, Qty) limit orders, orderbook entires are generally anonymous - trades (orders hit) can be public on blockchain based markets (eg PolyMarket)
        - Bid Orderbook (Buy offers)  : max(BidPrice) => Bid price
        - Ask Orderbook (Sell offers) : min(AskPrice) => Ask price
        - Orderbook liquidity factor (κ): derived from orderbook depth ~∑(Price * Quantity)
            - Should be weighted inverse to the spread of each order : ~1/(abs(Price - MarketMidPrice))
            - High spread orders eg:+- 5% from midprice are more unlikely to get hit
      - Mid Price = (Ask Price + Bid Price) / 2
          - could be skewed towards higher volume side
      - Spread = lowest(Ask price) - highest(Bid price)
          - Ask (+AskSpread) > MidPrice > (-BidSpread) Bid
          - scales with risk/volality
      - Volatility (σ)
          - Calculate based on rolling timeframe(s): eg: σ_1min , σ_5min, σ_15min
             - Upscaling eg (tgt 300s limit order timeframe): σ_1min to 5min =~ sqrt(5) * σ_1min
          - Combining volatility (EUR - USD): σ_combined​ = sqrt(σ_EUR²​ + σ_USD² + 2 * p * σ_EUR​ * σ_USD​)
             - if p < 0 (negative corr) eg: EUR - USD(C) => reduces combined volatility
             - if p =~ 0 (no corr)
             - if p > 0 (correlated) => increases combined volatility
               - crypto <> crypto : highly correlated (p =~ 0.7) eg: σ(LTC-BTC) =~ sqrt(σ_LTC² + σ_BTC² + 2 * 0.7 * σ_LTC * σ_BTC)
​

- # General MM flow
- API request open limit orders
  - cancell all open orders

- (Load inventory targets (%))
- API request current inventory units
- (API request market prices)
  - calculate inventory (%)
  - calculate inventory target (units)

# Spread

- Markets will always price the bidask-spread based on (historical) volatility σ
    - If the price drops off a cliff or shoots up, the spread will widen
    - Generally: bid spread = ask spread = 1/2 * bidask spread
    - Stoikov optimal bidask spread = γ * σ² + 1/2 * γ * log(1 + γ/κ)
      - γ inventory risk aversion param
      - σ market volatility
      - κ orderbook liquidity param
    

# Inventory

In following example the inventory is known, as is target inventory

![actual inventory](https://github.com/user-attachments/assets/1ff5773c-cce4-48ed-acec-a196ed7f004f)

Bottom graphs represent respective Bid(buy) / Ask (Sell) orders in units required to reach target inventory

![image](https://github.com/user-attachments/assets/aa3305b0-4e06-4759-8c1f-7c74fd5f7c17)




 
