using System.Collections.Generic;
using DomainLayer.Entities;
using NUnit.Framework;

namespace TestLibrary
{
    public class DisclaimerBuilder
    {

        [Test]
        public void BuildDescriptionAndSkuData()
        {
            var connProvider = new MyTests.TestConnectionProvider();
            var facebookDisclaimer = new RewardDisclaimer
                {
                    Description = @"Facebook gift cards are the quick and
easy way to get items in your favorite
games and apps on Facebook. Whether
you’re looking to speed up your progress
or play more games with friends,
the Facebook gift card works across
thousands of apps on Facebook.",
                    Disclaimer = @"* Use of this card constitutes acceptance of
the following terms and conditions: This card
may be redeemed on Facebook only by persons
age 13 and older. To redeem this card, visit
www.facebook.com/giftcards/redeem. Internet
access is required (separate fees may apply).
The full value of this card is deducted at PIN
entry. No incremental deductions are allowed.
This card is not redeemable for cash and cannot
be returned for a cash refund, exchanged, or
resold (except where required by law). The date
of issuance of this card is the date of purchase
shown on your sales receipt. No refunds or
credits will be provided for lost, stolen, or
destroyed cards, or for cards used without
permission. Use of this card is governed by the
Facebook Payments Terms, which are available
at www.facebook.com/payments_terms and
subject to change from time to time without
notice to the extent permitted by law. Protect
this card like cash. This card is issued by
Facebook Payments Inc., a Florida corporation.",
                    Sku = "FACE-E-V-STD",
                    TermsAndConditions = @"",
                };
            var fandangoDisclaimer = new RewardDisclaimer
                {
                    Sku = "FAND-E-500-STD",
                    Disclaimer = @"",
                    Description = @"Fandango Gift Cards make the perfect gift
for the special movie fan. Redeemable
for any movie available for ticketing
on Fandango.com or your mobile
device, the nation's leading moviegoer
destination sells tickets to over 21,000
screens nationwide. Fandango entertains
and informs consumers with reviews,
commentary, celebrity interviews and
trailers, and offers the ability to quickly
select a film, plan where and when to
see it, and conveniently buy tickets in
advance. At many theaters, fans can print
their tickets at home or receive them
as a paperless Mobile Ticket on their
smartphones.",
                    TermsAndConditions = @""
                };
            var appleBees = new RewardDisclaimer
                {
                    Sku = "APPLBS-E-500-STD",
                    Disclaimer = @"*Applebee’s is not a sponsor of the rewards or promotion or
otherwise affiliated with PracticeOwl. The logos and other
identifying marks attached are trademarks of and owned by each
represented company and/or its affiliates. © 2013 Applebee's
International, Inc. The Applebee’s logo is a registered trademark
and copyrighted work of Applebee’s International, Inc.",
                    Description = @"Join your neighbors at Applebee's for
delicious food and friendly service that
can't be beat. Stop by today to see
what's new in the neighborhood.",
                    TermsAndConditions = @"Applebee's Gift Cards never expire and they do not decrease in value.
The amount contained on this gift card may be applied toward the
purchase of food, beverage and gratuity at any participating Applebee's
Neighborhood Grill & Bar in the United States and Canada. Alcohol
not included where prohibited by law. This card cannot be replaced if
lost or stolen. This card cannot be redeemed for cash and no change
will be given, except as required by law. Use of this card constitutes
acceptance of these terms and conditions. For restaurant location
information, visit www.applebees.com. For balance inquiries: 1) visit
www.applebees.com or 2) call 1-800-252-6722."
                };
            var bestBuyDislcimer = new RewardDisclaimer
                {
                    Sku = "BSTB-E-500-STD",
                    Disclaimer = @"* Best Buy is not a sponsor of the rewards or otherwise
affiliated with PracticeOwl. The logos and other
identifying marks attached are trademarks of and owned
by each represented company and/or its affiliates.  Please
visit each company's website for additional terms and
conditions.",
                    Description = @"Best Buy is the global leader in consumer
electronics, offering the latest devices
and services all in one place. And at
BestBuy.com, you can shop when and
where you want.",
                    TermsAndConditions = @"All U.S. Gift Cards are redeemable in any U.S. and Puerto
Rico Best Buy retail locations, or online at BestBuy.com
where available, for merchandise or services including
Geek Squad services. No expiration date; no fees. Not
redeemable for cash. Lost, stolen or damaged cards
replaced only with valid proof of purchase to the extent of
remaining card balance. Not a credit or debit card. Not
valid as payment on a Best Buy credit card. Check Gift
Card balance at any U.S. and Puerto Rico Best Buy retail
locations, online at BestBuy.com or call 1-888-716-7994
with Gift Card number. Receipt will show remaining Gift
Card balance. Physical Gift Cards may be reloaded at any
U.S. and Puerto Rico Best Buy retail locations. All terms
enforced except where prohibited by law. Purchases of a
physical Gift Card in any Best Buy retail location or online at
Bestbuy.com are eligible for Reward Zone points, excluding
Best Buy for Business or commercial purchases of Gift
Cards."
                };
            var amazon = new RewardDisclaimer
                {
                    Sku = "AMZN-E-V-STD",
                    Disclaimer = @"* Amazon.com is not a sponsor of this promotion. Except as
required by law, Amazon.com Gift Cards ('GCs') cannot be
transferred for value or redeemed for cash. GCs may be used
only for purchases of eligible goods on Amazon.com or certain
of its affiliated websites. For complete terms and conditions, see
www.amazon.com/gc-legal. GCs are issued by ACI Gift Cards, Inc.,
a Washington corporation. ©,®,™ Amazon.com Inc. and/or its
affiliates, 2013. No expiration date or service fees.",
                    Description = @"Amazon.com Gift Cards* never expire and
can be redeemed towards millions of items at
www.amazon.com.",
                    TermsAndConditions = @""
                };
            var rei = new RewardDisclaimer
                {
                    Sku = "REII-E-500-STD",
                    Disclaimer = @"* REI is not a sponsor of the rewards or otherwise
affiliated with PracticeOwl. The logos and other
identifying marks attached are trademarks of and
owned by each represented company and/or its
affiliates.  Please visit each company's website for
additional terms and conditions.",
                    Description = @"To redeem your REI e-gift card, either print out
a copy of this email and use it in your local REI
store, or use it online at www.rei.com. If you
are shopping online, at step #5 of checkout
enter the 12-character Card Number and PIN
to redeem your e-gift card. If you have any
questions or problems, please visit http://
www.rei.com/rei/giftcards/redeem.html.",
                    TermsAndConditions = @"•

E-gift cards can be redeemed only for future
purchases of merchandise or services (i.e., rentals,
shop services) at REI.com, REI-OUTLET.com, REI
stores nationwide, and via phone at 1-800-426-
4840.

•

Upon redeeming your gift card, if your purchase
exceeds the amount of the gift card, you will need
to pay the balance. (When redeeming online or
by telephone, this balance must be paid by credit
card.)

•

Gift cards cannot be redeemed for cash (except
as noted below or where required by law) and are
not refundable if lost or stolen. When redeemed
at REI retail stores, gift cards containing an unused
balance of $10 or less will be cashed out.

When redeeming gift cards, any unused balance
(over $10) will be available for future purchases of
merchandise or services.

•"
                };
            var restaurantDotCom = new RewardDisclaimer
                {
                    Sku = "RESTDOTCOM-E-2500-STD",
                    Disclaimer = @"* Restaurant.com is not a sponsor of the
rewards or otherwise affiliated with PracticeOwl
. The logos and other identifying marks
attached are trademarks of and owned by each
represented company and/or its affiliates. 
Please visit each company's website for
additional terms and conditions.",
                    Description = @"A Restaurant.com Gift Card gives you
the choice of thousands of participating
restaurants nationwide, as well
as premium, online partners like
LobersterGram, Mrs. Fields, The Fruit
Company and many others.",
                    TermsAndConditions = @"Redeem the Restaurant.com Gift Certificate
online at http://Dine.Restaurant.com before
use. Unredeemed gift certificates not valid
toward purchase at restaurants. Limit one (1)
gift certificate at a given restaurant per party
per month. Minimum spend requirements
and other restrictions may apply. Visit http://
Dine.Restaurant.com for complete terms and
conditions and participating restaurants."
                };
            var sephora = new RewardDisclaimer
                {
                    Sku = "SEPH-E-V-STD",
                    Disclaimer = @"* Sephora is not a sponsor of this promotion or otherwise affiliated
with PracticeOwl. The logos and other identifying marks
attached are trademarks of and owned by each represented
company and/or its affiliates.  Please visit each company's website
for additional terms and conditions.",
                    Description = @"Got a case of the hmm's? Not anymore.
The Sephora eGift Card solves your every
gift-giving indecision. The card can be
redeemed in store, online, or through
any of our catalogs. What's more, it
doesn't expire, and both the balance
and transaction history can be checked
online or in any Sephora store.",
                    TermsAndConditions = @"Use of this Gift Card constitutes acceptance of the following terms: eGift Cards
are redeemable for merchandise sold in U.S. Sephora stores, on Sephora.com
for U.S. orders only, through the Sephora catalog or at Sephora inside JCPenney
stores. eGift Cards are not redeemable for cash (except as required by law).
This Gift Card does not expire and is valid until redeemed. The value of this Gift
Card will not be replaced if the card is lost, stolen, altered or destroyed. Treat
this card as cash. If your purchase exceeds the unused balance of the Gift Card,
you must pay the excess at the time of purchase. For Sephora store locations,
to order, or for card balance, please visit Sephora.com or call 1.877.SEPHORA.
Issued by Sephora USA, Inc.

Up to two (2) eGift Cards may be redeemed per purchase on www.sephora.com
and up to three (3) eGift Cards may be redeemed per purchase by calling
Sephora Customer Service at 1-877-737-4672."
                };
            var target = new RewardDisclaimer
                {
                    Sku = "TRGT-E-500-BULS",
                    Disclaimer = @"The Bullseye Design, Target and Target GiftCards are
registered trademarks of Target Brands Inc. Terms
and conditions are applied to gift cards. Target is not a
participating partner in or sponsor of this offer.",
                    Description = @"A Target eGiftCard is your opportunity
to shop for thousands of items at more
than 1,700 Target and SuperTarget®
stores in the U.S., as well as Target.com.
From home decor, small appliances and
electronics to fashion, accessories and
music, find exactly what you're looking
for at Target. No fees. No expiration. No
kidding.®",
                    TermsAndConditions = @""

                };
            var tango = new RewardDisclaimer
                {
                    Description = @"Tango Card: Redeem for iTunes®,
Amazon.com, Starbucks, or your favorite 
gift card. Use online, in store, or on your 
mobile.",
                    Disclaimer = @"* Tango Card is not a sponsor of the 
rewards or otherwise affiliated with PracticeOwl. The logos and other identifying marks 
attached are trademarks of and owned 
by each represented company and/or its 
affiliates. Please visit each company's website 
for additional terms and conditions.",
                    Sku = "TNGO-E-V-STD"
                };
            
            var xbox = new RewardDisclaimer
            {
                Sku = "XBOX-E-500-STD",
                Disclaimer = @"*Games and media content sold separately. Additional
subscriptions and/or requirements apply for some features.
Requirements and available features vary across consoles.
See xbox.com/live.",
                Description = @"With Xbox Live Gold, experience unrivaled multiplayer gaming and watch
HD movies, TV shows, live events, music and sports. Plus, enjoy premier
entertainment apps, Internet Explorer and member deals.",
                TermsAndConditions = @"Redeem your code to your U.S. Microsoft account.
Simply login to your account and enter the 25-digit
card number. To create a new account, visit https://
commerce.microsoft.com. You must be 13+. The full code
value will be applied to your Microsoft account and may be
used for eligible purchases (exclusions apply) made directly
at Xbox Games, Xbox Music, Xbox Video, and other select
Microsoft online stores.

Geography limitations and balance restrictions apply.
Eligible purchases and prices may vary by region, device,
and over time.

NO EXPIRATION DATE OR SERVICE FEES.

Taxes may apply. Internet access and a Microsoft account
are required (connect time charges may apply).

Xbox Live required to redeem on console. Original Xbox®
excluded. Paid subscriptions required for some content.
Except as required by law, codes cannot be redeemed or
exchanged for cash and are not reloadable or refundable.
Microsoft is not responsible if your code is lost, stolen,
destroyed, or used without permission. Subject to full terms
and conditions at www.microsoft.com/en-US/giftcard,
which may change without notice. Void where prohibited or
restricted by law."
            };

            var gap = new RewardDisclaimer
            {
                Sku = "GAPP-E-1000-STD",
                Disclaimer = @"*Gap is not a sponsor of the rewards
otherwise affiliated with [company name].
The logos and other identifying marks
attached are trademarks of and owned
by each represented company and/or its
affiliates.  Please visit each company's website
for additional terms and conditions.",
                TermsAndConditions = @"Disclaimer to be used when Gap is the only brand presented on a page or in an email. Otherwise, if it’s one of two or more brands being shown, you may use the General Merchant disclaimer."
            };

            var itunes = new RewardDisclaimer
            {
                Sku = "APPL-E-1000-STD",
                Disclaimer = @"iTunes® is a registered trademark of Apple Inc.  All rights
reserved.  Apple is not a participant in or sponsor of this promotion.",
                                     Description = @"iTunes® Gift Cards are perfect for anyone
who enjoys one-stop entertainment.
Each card is redeemable for music,
movies, TV shows, apps, games, books,
and more on the iTunes® Store, the App
Store, the iBookstore, or the Mac App
Store.",
                TermsAndConditions = @"* Valid only on iTunes® Store for U.S. Requires iTunes® account and prior
acceptance of license and usage terms. To open an account you must
be 13+ and in the U.S. Compatible software, hardware, and Internet
access required. Not redeemable for cash, no refunds or exchanges
(except as required by law). Code may not be used to purchase any other
merchandise, allowances or iTunes® gifting. Data collection and use
subject to Apple Customer Privacy Policy, see www.apple.com/privacy,
unless stated otherwise. Risk of loss and title for code passes to purchaser
on transfer. Codes are issued and managed by Apple Value Services, LLC
(“Issuer”). Neither Apple nor Issuer is responsible for any loss or damage
resulting from lost or stolen codes or use without permission. Apple
and its licensees, affiliates, and licensors make no warranties, express
or implied, with respect to code or the iTunes® Store and disclaim any
warranty to the fullest extent available. These limitations may not apply
to you. Void where prohibited. Not for resale. Subject to full terms and
conditions, see www.apple.com/legal/itunes/us/gifts.html. Content and
pricing subject to availability at the time of actual download. Content
purchased from the iTunes® Store is for personal lawful use only. Don’t
steal music. Subject to full terms and conditions, see www.apple.com/
legal/itunes/us/gifts.html.   © 2013 Apple Inc. All rights reserved.  iTunes®"
            };

            var footlocker = new RewardDisclaimer
            {
                Sku = "FTLCKR-E-2500-STD",
                Disclaimer = @"* Foot Locker® is not affiliated with the
program, the program sponsor, or the
program administrator, nor are they sponsors
or co-sponsors of the program. All merchant
names, logos, trademarks or other marks
herein are used with permission. The
registered owners of the names, logos,
trademarks or other marks retain all rights
therein. Merchants may have additional
terms and conditions, which may be found
on the gift card, gift certificate, voucher, or
merchant’s website. Merchants listed are
subject to change without notice.",
                Description = @"Foot Locker is the world's largest athletic
specialty store retailer, offering a broad
selection of footwear and apparel to meet
today's ever-changing needs. Whether it is
function or fashion, Foot Locker, Lady Foot
Locker, and Kids Foot Locker have what
everyone is looking for! The Foot Locker
Gift Card is redeemable for the latest in
athletic footwear and apparel at over 2,000
Foot Locker, Lady Foot Locker and Kids Foot
Locker stores in the U.S. and Canada. For the
Foot Locker store location nearest you go to
our Dealer Locator on the website. You can
access all Foot Locker locations. Foot Locker
Gift Cards are also redeemable through our
catalogs and online.",
                TermsAndConditions = @"PROTECT THIS CARD LIKE CASH. GiftCard may
be applied towards any purchase at Foot
Locker, Kids Foot Locker or Lady Foot Locker
stores in the US or online at footlocker.com,
kidsfootlocker.com or ladyfootlocker.com.
Card may not be exchanged for cash & will not
be replaced if lost or stolen. No variance from
the terms & conditions will be allowed except
where legally required. For card balance call
1.877.254.3333 (Toll Free). GiftCard is issued
by Foot Locker Card Services LLC. STORE
LOCATOR CALL: Foot Locker 1.800.991.6681;
Lady Foot Locker 1.800.877.5239; Kids Foot
Locker 1.800.991.6684."
            };

            var lowes = new RewardDisclaimer
            {
                Sku = "LOWES-E-1000-STD",
                Disclaimer = @"* Lowe’s is not a sponsor of the rewards or
otherwise affiliated with [company name]. The
logos and other identifying marks attached are
trademarks of and owned by each represented
company and/or its affiliates.  Please visit each
company's website for additional terms and
conditions.",
                Description = @"Find quality service, superior products
and helpful advice for all your home
improvement needs at Lowe's. Shop
for appliances, paint, patio furniture,
tools, flooring hardware and more. With
40,000 products in stock, 250,000 items
available online at Lowes.com and more
than 500,000 more products available
by Special Order, Lowe’s exists to help
you improve and maintain your biggest
asset – your home. Lowe’s – never stop
improving.",
                TermsAndConditions = @"This is not a credit/debit card and has no implied
warranties.  This card is not redeemable for cash
unless required by law and cannot be used to
make payments on any charge account.  Lost
or stolen Gift Cards can only be replaced upon
presentation of original sales receipt for any
remaining balance.  It will be void if altered or
defaced.  To check the balance of your Lowe's®
Gift Card, call 1-800-560-7172 or visit the
Customer Service Desk in any Lowe's® store."
            };


            var nike = new RewardDisclaimer
            {
                Sku = "NIKE-E-1000-STD",
                Disclaimer = @"* Nike is not a sponsor of the rewards or
affiliated with [company name]. The logos and other
identifying marks attached are trademarks of and
owned by each represented company and/or its
affiliates.  Please visit each company's website for
additional terms and conditions.",
                Description = @"Nike, Inc. is the world’s leading innovator
in athletic footwear, apparel, equipment
and accessories. If you have a body, you
are an athlete and Nike gifts always fit.
Just do it.",
                TermsAndConditions = @"Nike Digital Gift Cards are redeemable online at
Nike.com and NIKEiD.com and in person at NIKETOWN
and NikeFactoryStores retail locations in the USA,
Puerto Rico, and the District of Columbia. Gift Cards are
nontransferable and nonrefundable. When redeeming
a Gift Card online at Nike.com or NIKEiD.com, and
the order is more than the Gift Card amount, the
remaining balance must be paid for by credit card.
Upon redemption, any unused balance is carried over
and is available for furture nike.com purchases. Gift
cards are not redeemable for cash. Sales tax, where
applicable, will be applied at the time of redemption.
We are not responsible for lost or stolen Gift Cards. We
reserve the right to refuse, cancel or hold for review gift
certificates and orders for suspected fraud or for Gift
Cards mistakenly issued in an incorrect denomination.
For further information, visit Nike.com."
            };

            var nordstrom = new RewardDisclaimer
            {
                Sku = "NRDSTRM-E-2500-STD",
                Disclaimer = @"*Nordstrom is not a sponsor of the promotion or
otherwise affiliated with [company name]. The
logos and other identifying marks attached are
trademarks of and owned by each represented
company and/or its affiliates. Please visit each
company's website for additional terms and
conditions.",
                Description = @"Always the perfect fit - the Nordstrom Gift Card.
Nordstrom gift cards are redeemable at any
Nordstrom or Nordstrom Rack store, and at
nordstrom.com. Perfect for the fashion lover.",
                TermsAndConditions = @"This prepaid gift card is redeemable at any
Nordstrom or Nordstrom Rack store and at
nordstrom.com. To use, present this card to
salesperson at time of purchase. All purchases
made with a e-Gift Card require the access
number. This card may not be returned or applied
as payment on any account, and it may not be
redeemed for cash except as required by law. If
it is lost, stolen or damaged it can be replaced
with a new gift card for the remaining value with
satisfactory proof of purchase. Verify remaining
balance by calling 1.877.283.4045, online at
nordstrom.com or inquire at any store register."
            };

            var disclaimers = new List<RewardDisclaimer>
                {
                    //amazon,
                    //appleBees,
                    //bestBuyDislcimer,
                    //facebookDisclaimer,
                    //fandangoDisclaimer,
                    //rei,
                    //restaurantDotCom,
                    //sephora,
                    //target,
                    //tango,
                    //xbox
                    //gap,
                    itunes,
                    footlocker,
                    lowes,
                    nike,
                    nordstrom



                };

            foreach (var dislcaimer in disclaimers)
            {
                connProvider.CreateConnection().Save(dislcaimer);
            }
        }
    }
}