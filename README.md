# Plutus [![Build Status](https://travis-ci.org/umutseven92/Plutus.svg?branch=master)](https://travis-ci.org/umutseven92/Plutus) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/4461a03d867f449a914bf1f20eec0404)](https://www.codacy.com/project/umutseven92/Plutus/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=umutseven92/Plutus&amp;utm_campaign=Badge_Grade_Dashboard)

## Overview
Plutus is a customizable, exchange agnostic cryptocurrency trading bot. You configure:

* Which cryptocurrencies to buy & sell
* Profit amount & loss tolerance
* Which cryptocurrency exchanges you want to buy & sell from

It does the rest by predicting if the price is going up or down for a given coin and buying and selling according to your profit and loss stops.

Plutus is currently just a prototype. All contributions are welcome.

## Main Loops

### Buy Loop

* Check price of XX (PRICE = P)
* Determine if its going up or down
* Do price prediction
    * If its going down (Bearish);
        * Go to step 1
    * If its going up (Bullish);
        * Determine buy amount (AMOUNT = X)
        * Check balance for cost (COST = XP)
        * If balance is sufficient;
            * Buy X amount at price P 
        * If balance is insufficient;
            * Go to step 1
* Record buy order
* Go to step 1

### Sell Loop

* Check database for open buy orders
* For each open order;
   * Check new price of order (NEW PRICE = NP, PROFIT STOP = PS, LOSS STOP = LS),
       * If NP > P + PS, 
           * Sell (Profit)
           * Record sell order
       * If NP <= P - LS
           * Sell (Loss)
           * Record sell order
* Go to step 1

## Requirements

* .NET Core 2 or above
* Redis

## Configuration

### Plutus.Server/appsettings.json

This is the main configuration file.

```json
{
  "Exchanges": [
    {
      "Name": "Binance",
      "ApiKey": "API_KEY",
      "SecretKey": "SECRET_KEY"
    }
  ],
  "AppConfig": {
    "RedisUrl": "REDIS_URL",
    "BuyInterval": "0",
    "SellInterval": "0",
    "Test": "true"
  }
}
```

#### Exchanges
Add your exchanges to the Exchanges array, like shown above. You will need to put your **API_KEY** and **SECRET_KEY** in there. Please note that currently, only Binance is supported.

#### Redis
Replace **REDIS_URL** with the URL your Redis instance is running on. Redis is used for recording buy & sell orders. Persistence is not currently supported.

#### Test Run
If the Test value is true, than Plutus will only do *test* buys and sells, therefore not losing you real money. Its better to keep this true until you're sure you know what you are doing.

### Plutus.Server/orders.json

This is where you define the coin you want to buy & sell.

```json
{
  "Orders": [
    {
      "Base": "BTC",
      "Symbol": "XRP",
      "PLSymbol": "USDT",
      "ProfitStop": "10",
      "LossStop": "10",
      "BuyAmount": "1"
    }
  ]
}
```
#### Coins
Add your coins to the Orders array like shown above.

* Base: The coin you want to buy & sell with.
* Symbol: The actualy currency you want to buy & sell.

For example, if your base is **BTC**, and your Symbol is **XRP**, you will buy **XRP** with **BTC**, and you will sell **XRP** for **BTC**.

* PLSymbol: The currency symbol used to calculate loss & profit stops.
* ProfitStop: The amount of profit that needs to be reached to sell, **in PLSymbol**.
* LossStop: The amount of loss that needs to be reached to sell , **in PLSymbol**. 
* BuyAmount: The amount to buy.

For example, if your **PLSymbol** is **USD**, your **ProfitStop** is **20**, your **LossStop** is **10** and **BuyAmount** is **40**, Plutus will buy **40 BTC**'s worth of **XRP**, and will sell if your **profit** is **20 USD** or if your **loss** is **10 USD**.


## How to Use

In the root directory,

```bash
dotnet build
cd Plutus.Server
dotnet run
```

## Warning
<aside class="warning">
Plutus is in pre-alpha. Many things are yet to be implemented. It has only been used by me, with very small amounts for testing. Please be responsible with your money and don't trust Plutus (or any trading bot) with more money than you can afford to lose. </aside>

## What Is Implemented
* Main buy & sell loops
* Binance support
* [SMA](https://www.investopedia.com/university/movingaverage/) for price prediction

## What Is Missing
* Support for many more exhanges
* Support for many more price indicators
* Persistance for buy and sell orders


## Libraries Used
[Binance.NET](https://github.com/sonvister/Binance) (MIT)

[FluentScheduler](https://github.com/fluentscheduler/FluentScheduler) (BSD)

[StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) (MIT)
