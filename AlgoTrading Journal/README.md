Resources (hummingbot)
- https://hummingbot.org/academy-content/guide-to-the-avellaneda--stoikov-strategy/
- https://hummingbot.org/academy-content/how-to-get-good-at-market-making/
- https://hummingbot.org/faq/

# General process flow
- API request open limit orders
  - cancell all open orders

- (Load inventory targets (%))
- API request current inventory units
- (API request market prices)
  - calculate inventory (%)
  - calculate inventory target (units)

# Inventory

In following example the inventory is known, as is target inventory

![actual inventory](https://github.com/user-attachments/assets/1ff5773c-cce4-48ed-acec-a196ed7f004f)

Bottom graphs represent respective Bid(buy) / Ask (Sell) orders in units required to reach target inventory

![image](https://github.com/user-attachments/assets/aa3305b0-4e06-4759-8c1f-7c74fd5f7c17)




 
