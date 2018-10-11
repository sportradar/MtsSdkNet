echo off
cls
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket /input:ticket-2.1-schema.json /output:Ticket\TicketDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketAckDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketAck /input:ticket-ack-2.0-schema.json /output:TicketAck\TicketAckDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketCancelDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancel /input:ticket-cancel-2.0-schema.json /output:TicketCancel\TicketCancelDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketCancelAckDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancelAck /input:ticket-cancel-ack-2.0-schema.json /output:TicketCancelAck\TicketCancelAckDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketResponseDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketResponse /input:ticket-response-2.1-schema.json /output:TicketResponse\TicketResponseDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketCancelResponseDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancelResponse /input:ticket-cancel-response-2.0-schema.json /output:TicketCancelResponse\TicketCancelResponseDTO.cs

..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketReofferCancelDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketReofferCancel /input:reoffer-cancel-2.0-schema.json /output:TicketReofferCancel\TicketReofferCancelDTO.cs

..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketCashoutDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCashout /input:ticket-cashout-2.0-schema.json /output:TicketCashout\TicketCashoutDTO.cs
..\..\build\tools\nswag\nswag.exe jsonschema2csclient /arraytype:IEnumerable /name:TicketCashoutResponseDTO /namespace:Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCashoutResponse /input:ticket-cashout-response-2.0-schema.json /output:TicketCashoutResponse\TicketCashoutResponseDTO.cs

echo.
echo Replacing 'double' to 'long' ...
..\..\build\tools\fart.exe -r *.cs  " double " " long "

echo Replacing 'double?' to 'long?' ...
..\..\build\tools\fart.exe -r *.cs  " double? " " long? "

echo.
echo Replacing 'new IEnumerable^<' to 'new Collection^<' ...
..\..\build\tools\fart.exe -r *.cs  "= new IEnumerable<" "= new Collection<"

echo.
echo.
echo TODO: Add the following into TicketDTO.cs and TicketCancelDTO.cs:
echo using System.Collections.Generic;
echo using System.Collections.ObjectModel;

set /p id=