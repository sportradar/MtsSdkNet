/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

// ReSharper disable InconsistentNaming

namespace Sportradar.MTS.SDK.Entities.Internal.REST
{
    /// <summary>
    /// Represents all messages (entities) received from the REST API 
    /// </summary>
    public abstract class RestMessage
    {

    }

    [OverrideXmlNamespace(RootElementName = "market_descriptions", IgnoreNamespace = false)]
    public partial class market_descriptions : RestMessage
    {

    }
}