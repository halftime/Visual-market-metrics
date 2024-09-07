## Resources Hummingbot
- https://hummingbot.org/academy-content/guide-to-the-avellaneda--stoikov-strategy/
- https://hummingbot.org/academy-content/how-to-get-good-at-market-making/
- https://hummingbot.org/faq/
## Resources CoinMetro 
- https://documenter.getpostman.com/view/3653795/SVfWN6KS
- https://help.coinmetro.com/hc/en-gb/articles/6844029270685-How-to-Use-Coinmetro-s-Trading-API


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
          - scales with risk/Volatility
      - Volatility (σ)
          - Calculate based on rolling timeframe(s): eg: σ_1min , σ_5min, σ_15min
             - Upscaling eg (tgt 300s limit order timeframe): σ_1min to 5min =~ sqrt(5) * σ_1min
          - Combining volatility (EUR - USD): σ_combined​ = sqrt(σ_EUR²​ + σ_USD² + 2 * p * σ_EUR​ * σ_USD​)
             - if p < 0 (negative corr) eg: EUR - USD(C) => reduces combined volatility
             - if p =~ 0 (no corr)
             - if p > 0 (correlated) => increases combined volatility
               - crypto <> crypto : (assume) highly correlated (p =~ 0.7) eg: σ(LTC-BTC) =~ sqrt(σ_LTC² + σ_BTC² + 2 * 0.7 * σ_LTC * σ_BTC)
               - [crypto correlation matrix](https://www.blockchaincenter.net/en/crypto-correlation-tool)
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

## Target inventory
For single pair MM, usually 50:50% on each side makes sense

For more broad trading inventories...

A 60 stable(or fiat) / 40 crypto inventory (vs 50:50), could have benifits:

- More liquidity free to move around (expect to get stuck with some illiquid crypto inventory, esp in high spread, small markets)
- The stable-stable markets (EG USDC-USD) are very liquid and fairly low risk (inventory) to MM
- However.. in stress scenarios stablecoins can depeg from NAV
  - Choppy market that revert to mean? ✔

## Example

***inventory example - freshly funded with euros from bank***
![363998068-f98f4afa-289d-4910-9483-134e2a0835c1](https://github.com/user-attachments/assets/e3fda990-d751-4503-9018-b11203f5e774)

***inventory example - During market making, inventories moving towards target %***
![afbeelding](https://github.com/user-attachments/assets/82fb5a3e-6dfa-4760-92ab-7fd0ab5fa033)


On this exchange all fiat currencies are traded against USDC, amongst others:
```
 BTC-USDC
 USDC-USD
 USDC-USDT
 ETH-USDC
 SOL-USDC
 XRP-USDC
 USDC-EUR
 ... (85 pairs in total)
```

In this case it could be good to overweight USDC as it stands on the crossroad of 3 fiat currencies (EUR, USD, GBP)
- Any asset can be over- or underweighted to balance inventory (risk) or accumulate specified assets

***Inventory overview spreadsheet***

![afbeelding](https://github.com/user-attachments/assets/cb253a7e-13d0-48e5-b015-0bfb856c996c)

*Make sure your the sum total inventory targets <= 100%* 

# Rest API & Limits
During testing w order placement received this reply from exchange REST API:

```{"message":"This endpoint is rate limited to 20 attempts per 60 seconds. Limits can be increased by contacting Customer Support."}```

Turns out there's a lot of limits on API usage.. Long story short, stay under the damn limit.

## Limit Tips:
- Place orders further out (expiry time) & modify existing orders instead of replacing them
- Only place new orders for expired/closed orders


# Websockets
MM can receive instantanious websocket stream which pushes tickbased market data, orderupdates, balance updates, ...
- Market tickets
      - Current bid & ask price (& qty)
- Order Updates
    - Example initial order (unfilled):
```
OrderType (str): "type:limit"
BuyingCurrency (str): "USDC" 
SellingCurrency (str): "AAVE"
buyingQty (dec): 0
boughtQty (dec): 0
sellingQty (dec): 0.1937
soldQty (dec): 0
completionTime (long): none (timestamp if order done -> remove order from list)
creationTime (long): (timestamp creation)
expirationTime (long): (future timestamp of expiry)
```
 
