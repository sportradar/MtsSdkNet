﻿//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v8.6.6263.34621 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketResponse
{
    #pragma warning disable // Disable all warnings

    /// <summary>Object carrying information about rejection cause</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Reason : System.ComponentModel.INotifyPropertyChanged
    {
        private int _code;
        private string _message;
    
        /// <summary>Rejection code</summary>
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
        public int Code
        {
            get { return _code; }
            set 
            {
                if (_code != value)
                {
                    _code = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Rejection reason description</summary>
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message
        {
            get { return _message; }
            set 
            {
                if (_message != value)
                {
                    _message = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Reason FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Reason>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>Acceptance status - accepted, rejected</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum Status
    {
        [System.Runtime.Serialization.EnumMember(Value = "accepted")]
        Accepted = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "rejected")]
        Rejected = 1,
    
    }
    
    /// <summary>Ticket response 2.2 schema</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class TicketResponseDTO : System.ComponentModel.INotifyPropertyChanged
    {
        private Result _result = new Result();
        private IEnumerable<Anonymous> _autoAcceptedOdds = new Collection<Anonymous>();
        private string _version;
        private string _signature;
        private long _exchangeRate;
    
        [Newtonsoft.Json.JsonProperty("result", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Result Result
        {
            get { return _result; }
            set 
            {
                if (_result != value)
                {
                    _result = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Contains odds auto-acceptance information</summary>
        [Newtonsoft.Json.JsonProperty("autoAcceptedOdds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IEnumerable<Anonymous> AutoAcceptedOdds
        {
            get { return _autoAcceptedOdds; }
            set 
            {
                if (_autoAcceptedOdds != value)
                {
                    _autoAcceptedOdds = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>JSON format version (must be '2.2')</summary>
        [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(3, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^(2\.2)$")]
        public string Version
        {
            get { return _version; }
            set 
            {
                if (_version != value)
                {
                    _version = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Response signature (previous betAcceptanceId)</summary>
        [Newtonsoft.Json.JsonProperty("signature", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Signature
        {
            get { return _signature; }
            set 
            {
                if (_signature != value)
                {
                    _signature = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>The exchange rate used when converting currencies to EUR. Double multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("exchangeRate", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000000000000000.0)]
        public long ExchangeRate
        {
            get { return _exchangeRate; }
            set 
            {
                if (_exchangeRate != value)
                {
                    _exchangeRate = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static TicketResponseDTO FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TicketResponseDTO>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Result : System.ComponentModel.INotifyPropertyChanged
    {
        private string _ticketId;
        private Status _status;
        private Reason _reason = new Reason();
        private IEnumerable<Anonymous2> _betDetails = new Collection<Anonymous2>();
    
        /// <summary>External ticket id</summary>
        [Newtonsoft.Json.JsonProperty("ticketId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string TicketId
        {
            get { return _ticketId; }
            set 
            {
                if (_ticketId != value)
                {
                    _ticketId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Status Status
        {
            get { return _status; }
            set 
            {
                if (_status != value)
                {
                    _status = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        [Newtonsoft.Json.JsonProperty("reason", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Reason Reason
        {
            get { return _reason; }
            set 
            {
                if (_reason != value)
                {
                    _reason = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Bet-level rejection details</summary>
        [Newtonsoft.Json.JsonProperty("betDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IEnumerable<Anonymous2> BetDetails
        {
            get { return _betDetails; }
            set 
            {
                if (_betDetails != value)
                {
                    _betDetails = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Result FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>Odds auto-acceptance selection information</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous : System.ComponentModel.INotifyPropertyChanged
    {
        private int _selectionIndex;
        private int _requestedOdds;
        private int _usedOdds;
    
        [Newtonsoft.Json.JsonProperty("selectionIndex", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int SelectionIndex
        {
            get { return _selectionIndex; }
            set 
            {
                if (_selectionIndex != value)
                {
                    _selectionIndex = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Odds with which the ticket was placed</summary>
        [Newtonsoft.Json.JsonProperty("requestedOdds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(10000.0, 1000000000.0)]
        public int RequestedOdds
        {
            get { return _requestedOdds; }
            set 
            {
                if (_requestedOdds != value)
                {
                    _requestedOdds = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Odds with which the ticket was accepted</summary>
        [Newtonsoft.Json.JsonProperty("usedOdds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(10000.0, 1000000000.0)]
        public int UsedOdds
        {
            get { return _usedOdds; }
            set 
            {
                if (_usedOdds != value)
                {
                    _usedOdds = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Anonymous FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Anonymous>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>Reason on bet level</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous2 : System.ComponentModel.INotifyPropertyChanged
    {
        private string _betId;
        private Reason _reason = new Reason();
        private IEnumerable<Anonymous3> _selectionDetails = new Collection<Anonymous3>();
        private Reoffer _reoffer = new Reoffer();
        private AlternativeStake _alternativeStake = new AlternativeStake();
    
        /// <summary>Bet id</summary>
        [Newtonsoft.Json.JsonProperty("betId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string BetId
        {
            get { return _betId; }
            set 
            {
                if (_betId != value)
                {
                    _betId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        [Newtonsoft.Json.JsonProperty("reason", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Reason Reason
        {
            get { return _reason; }
            set 
            {
                if (_reason != value)
                {
                    _reason = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Per-selection rejection reasons</summary>
        [Newtonsoft.Json.JsonProperty("selectionDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IEnumerable<Anonymous3> SelectionDetails
        {
            get { return _selectionDetails; }
            set 
            {
                if (_selectionDetails != value)
                {
                    _selectionDetails = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Bet reoffer details, mutually exclusive with alternativeStake</summary>
        [Newtonsoft.Json.JsonProperty("reoffer", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Reoffer Reoffer
        {
            get { return _reoffer; }
            set 
            {
                if (_reoffer != value)
                {
                    _reoffer = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Alternative stake, mutually exclusive with reoffer</summary>
        [Newtonsoft.Json.JsonProperty("alternativeStake", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AlternativeStake AlternativeStake
        {
            get { return _alternativeStake; }
            set 
            {
                if (_alternativeStake != value)
                {
                    _alternativeStake = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Anonymous2 FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Anonymous2>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>Reason on selection level</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous3 : System.ComponentModel.INotifyPropertyChanged
    {
        private int _selectionIndex;
        private Reason _reason = new Reason();
        private RejectionInfo _rejectionInfo = new RejectionInfo();
    
        [Newtonsoft.Json.JsonProperty("selectionIndex", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int SelectionIndex
        {
            get { return _selectionIndex; }
            set 
            {
                if (_selectionIndex != value)
                {
                    _selectionIndex = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        [Newtonsoft.Json.JsonProperty("reason", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Reason Reason
        {
            get { return _reason; }
            set 
            {
                if (_reason != value)
                {
                    _reason = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Rejection information on selection level</summary>
        [Newtonsoft.Json.JsonProperty("rejectionInfo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public RejectionInfo RejectionInfo
        {
            get { return _rejectionInfo; }
            set 
            {
                if (_rejectionInfo != value)
                {
                    _rejectionInfo = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Anonymous3 FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Anonymous3>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Reoffer : System.ComponentModel.INotifyPropertyChanged
    {
        private long _stake;
        private ReofferType _type;
    
        /// <summary>Reoffer stake. Double multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("stake", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000000000000000.0)]
        public long Stake
        {
            get { return _stake; }
            set 
            {
                if (_stake != value)
                {
                    _stake = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Reoffer type, if auto then stake will be present. If manual you should wait for reoffer stake over Reply channel</summary>
        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ReofferType Type
        {
            get { return _type; }
            set 
            {
                if (_type != value)
                {
                    _type = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Reoffer FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Reoffer>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class AlternativeStake : System.ComponentModel.INotifyPropertyChanged
    {
        private long _stake;
    
        /// <summary>Alternative stake. Double multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("stake", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000000000000000.0)]
        public long Stake
        {
            get { return _stake; }
            set 
            {
                if (_stake != value)
                {
                    _stake = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static AlternativeStake FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AlternativeStake>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class RejectionInfo : System.ComponentModel.INotifyPropertyChanged
    {
        private string _eventId;
        private string _id;
        private int? _odds;
    
        /// <summary>Rejected selection's related Betradar event (match or outright) id</summary>
        [Newtonsoft.Json.JsonProperty("eventId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 1)]
        public string EventId
        {
            get { return _eventId; }
            set 
            {
                if (_eventId != value)
                {
                    _eventId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Rejected selection's related Selection id</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 1)]
        public string Id
        {
            get { return _id; }
            set 
            {
                if (_id != value)
                {
                    _id = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Rejected selection's related Odds</summary>
        [Newtonsoft.Json.JsonProperty("odds", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(10000.0, 1000000000.0)]
        public int? Odds
        {
            get { return _odds; }
            set 
            {
                if (_odds != value)
                {
                    _odds = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static RejectionInfo FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RejectionInfo>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum ReofferType
    {
        [System.Runtime.Serialization.EnumMember(Value = "auto")]
        Auto = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "manual")]
        Manual = 1,
    
    }
}