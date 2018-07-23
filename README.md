# Plutus [![Build Status](https://travis-ci.com/umutseven92/Plutus.svg?token=FcSswCqpWzu5VpELPryw&branch=master)](https://travis-ci.com/umutseven92/Plutus)

## Main Loops

### Buy Loop

* Check price of XXX (PRICE = P)
* Determine if its going up or down
* Do price prediction
    * If its going down (Bearish);
        * Go to step 1
    * If its going up (Bullish);
        * Determine buy amount (AMOUNT = X)
        * Check balance for cost (COST = XP)
        * If balance is sufficent;
            * Buy X amount at price P 
        * If balance is insufficent;
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

## Libraries Used
[Binance.NET](https://github.com/sonvister/Binance) (MIT)

[FluentScheduler](https://github.com/fluentscheduler/FluentScheduler) (BSD)

[Trady](https://github.com/lppkarl/Trady) (Apache License 2.0)

[StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) (MIT)
