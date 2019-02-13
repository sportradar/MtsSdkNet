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

    /// <summary>
    /// Represents all XML messages (entities) received from the REST API 
    /// </summary>
    public abstract class XmlRestMessage : RestMessage
    {

    }

    [OverrideXmlNamespace(RootElementName = "market_descriptions", IgnoreNamespace = false)]
    public partial class market_descriptions : XmlRestMessage
    {

    }

    namespace ClientApi
    {
        public partial class CcfResponse : RestMessage
        {

        }

        public partial class MaxStakeResponse : RestMessage
        {

        }

        public partial class SportCcf : RestMessage
        {

        }

        public partial class KeycloakAuthorization : RestMessage
        {

        }
    }
}