﻿A MTS SDK library
For more information please contact support@sportradar.com or visit http://sdk.sportradar.com/mts/net

CHANGE LOG:
2020-09-18 2.3.5.0
Added # to the User ID pattern
Fix: removed problematic Guard checks
Fix: checking for invalid products

2020-04-07 2.3.4.0
Added support for TLS 1.2
Added configuration property ticketResponseTimeoutPrematch
Added TicketResponseTimedOut handler in DemoProject
Added SetTicketResponseTimeoutLive and SetTicketResponseTimeoutPrematch to ISdkConfigurationBuilder
Allow 0 cashout stake when building TicketCashout
Added bookmakerId to the client_properties
Added argument to rabbit queue declare: queue-master-locator
Changed connection heartbeat from 45s to 20s
Default timeout for ticket response for live selections increased from 15s to 17s
Examples updated to use UOF markets
Replaced CodeContracts with Dawn.Guard
Fix: set AutomaticRecovery of rabbit connection to false
Fix: removing empty connection after reconnect

2019-11-07 2.3.3.2
Improved handling of connection to the rabbit server

2019-11-05 2.3.3.1
Fix: set AutomaticRecovery of rabbit connection to false

2019-10-25 2.3.3.0
Added configuration property ticketResponseTimeoutPrematch
Added SetTicketResponseTimeoutLive and SetTicketResponseTimeoutPrematch to ISdkConfigurationBuilder
Default timeout for ticket response for live selections increased from 15s to 17s
Added new distribution channels
Added TicketResponseTimedOut handler in DemoProject
Fix: removing empty connection after reconnect

2019-07-25 2.3.2.0
Made Ticket objects serializable
Added support for TLS 1.2
Removed use of singleton for CustomBetSelectionBuilder
Added Content-Type to AdditionalInfo property of response tickets
Fix: CustomBet can be set without odds

2019-05-30 2.3.1.0
Support for custom bet
Added CustomBetManager to IMtsSdk
Exposed custom bet fields on ITicket

2019-05-10 2.3.0.0
Support for ticket version 2.3
Support for non-Sportradar ticket settlement
Added LastMatchEndTime to ITicket and ITicketBuilder
Added ITicketNonSrSettle and ITicketNonSrSettleBuilder

2019-04-18 1.8.1.0
Fix: reconnecting issue after being disconnected by the server

2019-02-27 1.8.0.0
Added support for Client API - added property ClientApi on IMtsSdk
Added configuration for ticket, ticket cancellation and ticket cashout message timeouts
Improvement: inflation of rabbit channels when many disconnects

2019-02-07 1.7.0.0
Adding acking on consumers message processed
Added AutoAcceptedOdds to ITicketResponse
Added AdditionalInfo to all ticket responses
Fix: settings corrected for sending ticket cancel and reoffer cancel message

2018-11-28 1.6.0.0
Support for ticket version 2.2
Added AutoAcceptedOdd to TicketResponse
Added TotalCombinations to Ticket and TicketBuilder
Added BetCashout to TicketCashout - support for partial cashout
Added BetCancel to TicketCancel - support for partial cancellation
Added AutoAcceptedOdds to ITicketResponse
EndCustomer.Ip type changed from IPAddress to string
Removed deletion of consumer queues on close
Reviewed and updated documentation and properties files
Removed property ExceptionHandlingStrategy from config, builder, ...
Fix: in HandleMarketDescription when adding specifiers

2018-10-05 1.5.0.0
Added MtsSdk.TicketResponseTimedOut event to notify user if the ticket response did not arrive in timely fashion (when sending in non-blocking mode)
Added 'exclusiveConsumer' property to the configuration, indicating should the rabbit consumer channel be exclusive (default is true)
Renamed ClientIteration logger to ClientInteraction (check log4net.config)
Added timeout when fetching MarketDescriptions from API fails (30s)
Improved handling and logging for market description (for UF markets)
Fix: BetBonus value condition - if set, must be greater then zero
Fix: possible memory leak when sending in blocking mode
Minor fixes and improvements

2018-03-26 1.4.0.0
Downgraded librarys target framework to v4.5.1
Added method ToJson() to all tickets (returns json send to or received from MTS)
Fixed Example11

2018-01-17 1.3.0.0
Support for ticket and ticket response v2.1
Selection.Id max length increased to 1000
SumOfWins - can be null or greater then zero
Removed all sender channel specific validation during ticket building
Ticket response reasons internal message marked as obsolete
Added rejection info to ticket response selection detail
Added additional info property to ticket response
Added SSL certification verification (uses TLS 1.2)
Added examples from MTS Ticket Integration v31 documentation

2017-12-21 1.2.1.0
Fix: TicketBuilder for multi-bet tickets with same selections but different odds or different banker value

2017-11-16 1.2.0.0
Added new config property 'port'

2017-11-08 1.1.6.0
Fix: ShopId is not required for Retail sender channel

2017-10-19 1.1.5.0
Sender.Currency property updated to accept also 4-letter sign (i.e. mBTC)
Added SdkConfigurationBuilder for building SdkConfiguration 

2017-09-13 1.1.4.0
Added new config property 'provideAdditionalMarketSpecifiers'
Fix: building selection id with UF specifiers
Fix: removed requirement check for selectionDetails in ticket response
Fix: removed requirement for EndCustomer.Id for Terminal and Retail sender channel

2017-08-28 1.1.3.0
Added TicketAck and TicketCancelAck builders
Exposed property CorrelationId in all tickets and ticket responses
Refined logging within sdk (for feed and rest traffic)
Property SelectionDetails on ticket response changed to optional
Internal: 'selectionRef:[]' removed from json when empty
Internal: added ConsumerTag to consumer channels

2017-07-31 1.1.2.0
Internal: updated how ticket acknowledgements are send
Internal: cleaned ticket's json representation when possible

2017-06-30 1.1.1.0
Changed Sender validation
Added MarketDescriptionCache for UoF markets
Added 'accessToken' attribute to config section (only used for UoF markets)
Changed how tickets are build (through BuilderFactory) and added input validation
Internal: publisher channel settings changed to non-persistent delivery mode

2017-05-15 1.1.0.0
Added Cashout support
Added Cashout example

2017-05-09 1.0.4.0
Added Reoffer support (Reoffer and ReofferCancel ticket)
Added Reoffer example

2017-04-26 1.0.3.0
Fixed builders verification
Added SenderChannel verification
Added builders for ticket reoffer and alternative stake ticket

2017-04-13 1.0.2.0
Fixed issue when adding bet selections with banker

2017-04-04 1.0.1.0
Fixed issue with connection to the MTS servers 

2017-03-13 Official release 1.0.0
Offical release of the MTS SDK (supports MTS tickets v2.0) 

2017-02-14 Release candidate 0.1.0
Support MTS ticket version 2.0