﻿A MTS SDK library
For more information please contact support@sportradar.com or visit http://sdk.sportradar.com/mts/net

CHANGE LOG:
2019-05-10 2.3.0.0
Added support for ticket 2.3
Exposed property LastMatchEndTime on ITicket
Added support for Non-SR content (new ticket type and ticket builder)

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