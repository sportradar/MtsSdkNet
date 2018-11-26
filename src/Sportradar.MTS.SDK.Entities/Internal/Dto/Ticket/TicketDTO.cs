﻿//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v8.6.6263.34621 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket
{
#pragma warning disable // Disable all warnings

    /// <summary>Ticket version 2.2 schema</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class TicketDTO : System.ComponentModel.INotifyPropertyChanged
    {
        private Ticket _ticket = new Ticket();
    
        /// <summary>Actual ticket being sent to Sportradar</summary>
        [Newtonsoft.Json.JsonProperty("ticket", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Ticket Ticket
        {
            get { return _ticket; }
            set 
            {
                if (_ticket != value)
                {
                    _ticket = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.Ticket);
        }
        
        public static TicketDTO FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TicketDTO>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Ticket : System.ComponentModel.INotifyPropertyChanged
    {
        private long _timestampUtc;
        private IEnumerable<Anonymous> _bets = new Collection<Anonymous>();
        private string _ticketId;
        private IEnumerable<Anonymous2> _selections = new Collection<Anonymous2>();
        private Sender _sender = new Sender();
        private string _reofferRefId;
        private string _altStakeRefId;
        private string _version;
        private bool _testSource;
        private TicketOddsChange? _oddsChange = Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket.TicketOddsChange.None;
        private int? _totalCombinations;
    
        /// <summary>Timestamp of ticket placement (in UNIX time millis)</summary>
        [Newtonsoft.Json.JsonProperty("timestampUtc", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 9223372036854775807.0)]
        public long TimestampUtc
        {
            get { return _timestampUtc; }
            set 
            {
                if (_timestampUtc != value)
                {
                    _timestampUtc = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Collection of all bets</summary>
        [Newtonsoft.Json.JsonProperty("bets", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public IEnumerable<Anonymous> Bets
        {
            get { return _bets; }
            set 
            {
                if (_bets != value)
                {
                    _bets = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Unique ticket id (in the client's system)</summary>
        [Newtonsoft.Json.JsonProperty("ticketId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(128, MinimumLength = 1)]
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
    
        /// <summary>Array of all selections. Order is very important as they can be referenced by index in 'ticket.bets.selectionRefs'</summary>
        [Newtonsoft.Json.JsonProperty("selections", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public IEnumerable<Anonymous2> Selections
        {
            get { return _selections; }
            set 
            {
                if (_selections != value)
                {
                    _selections = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Identification and settings of the ticket sender</summary>
        [Newtonsoft.Json.JsonProperty("sender", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Sender Sender
        {
            get { return _sender; }
            set 
            {
                if (_sender != value)
                {
                    _sender = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Reoffer reference ticket id</summary>
        [Newtonsoft.Json.JsonProperty("reofferRefId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(128, MinimumLength = 1)]
        public string ReofferRefId
        {
            get { return _reofferRefId; }
            set 
            {
                if (_reofferRefId != value)
                {
                    _reofferRefId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Alternative stake reference ticket id</summary>
        [Newtonsoft.Json.JsonProperty("altStakeRefId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(128, MinimumLength = 1)]
        public string AltStakeRefId
        {
            get { return _altStakeRefId; }
            set 
            {
                if (_altStakeRefId != value)
                {
                    _altStakeRefId = value; 
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
    
        [Newtonsoft.Json.JsonProperty("testSource", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool TestSource
        {
            get { return _testSource; }
            set 
            {
                if (_testSource != value)
                {
                    _testSource = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Accept change in odds (optional, default none) none: default behaviour, any: any odds change accepted, higher: accept higher odds</summary>
        [Newtonsoft.Json.JsonProperty("oddsChange", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TicketOddsChange? OddsChange
        {
            get { return _oddsChange; }
            set 
            {
                if (_oddsChange != value)
                {
                    _oddsChange = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Expected total number of generated combinations on this ticket (optional, default null). If present is used to validate against actual number of generated combinations.</summary>
        [Newtonsoft.Json.JsonProperty("totalCombinations", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, double.MaxValue)]
        public int? TotalCombinations
        {
            get { return _totalCombinations; }
            set 
            {
                if (_totalCombinations != value)
                {
                    _totalCombinations = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Ticket FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Ticket>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous : System.ComponentModel.INotifyPropertyChanged
    {
        private Bonus _bonus = new Bonus();
        private Stake _stake = new Stake();
        private string _id;
        private IEnumerable<int> _selectedSystems = new Collection<int>();
        private IEnumerable<Anonymous3> _selectionRefs = new Collection<Anonymous3>();
        private string _reofferRefId;
        private long? _sumOfWins;
    
        /// <summary>Bonus of the bet (optional, default null)</summary>
        [Newtonsoft.Json.JsonProperty("bonus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Bonus Bonus
        {
            get { return _bonus; }
            set 
            {
                if (_bonus != value)
                {
                    _bonus = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Stake of the bet</summary>
        [Newtonsoft.Json.JsonProperty("stake", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Stake Stake
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
    
        /// <summary>Bet id (optional)</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(128, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z:\-_]{1,128}$")]
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
    
        /// <summary>Array of all the systems (mandatory, [0] is not allowed, use [fold] instead)</summary>
        [Newtonsoft.Json.JsonProperty("selectedSystems", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public IEnumerable<int> SelectedSystems
        {
            get { return _selectedSystems; }
            set 
            {
                if (_selectedSystems != value)
                {
                    _selectedSystems = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Array of selection references which form the bet (optional, if missing then all selections are used)</summary>
        [Newtonsoft.Json.JsonProperty("selectionRefs", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IEnumerable<Anonymous3> SelectionRefs
        {
            get { return _selectionRefs; }
            set 
            {
                if (_selectionRefs != value)
                {
                    _selectionRefs = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Reoffer reference bet id</summary>
        [Newtonsoft.Json.JsonProperty("reofferRefId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(128, MinimumLength = 1)]
        public string ReofferRefId
        {
            get { return _reofferRefId; }
            set 
            {
                if (_reofferRefId != value)
                {
                    _reofferRefId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Sum of all wins for all generated combinations for this bet (in ticket currency, used in validation)</summary>
        [Newtonsoft.Json.JsonProperty("sumOfWins", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 9223372036854775800.0)]
        public long? SumOfWins
        {
            get { return _sumOfWins; }
            set 
            {
                if (_sumOfWins != value)
                {
                    _sumOfWins = value; 
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
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous2 : System.ComponentModel.INotifyPropertyChanged
    {
        private string _eventId;
        private string _id;
        private int _odds;
    
        /// <summary>Betradar event (match or outright) id</summary>
        [Newtonsoft.Json.JsonProperty("eventId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
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
    
        /// <summary>Selection id, should be composed according to specification</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
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
    
        /// <summary>Odds multiplied by 10_000 and rounded to int value</summary>
        [Newtonsoft.Json.JsonProperty("odds", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(10000.0, 1000000000.0)]
        public int Odds
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
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Sender : System.ComponentModel.INotifyPropertyChanged
    {
        private string _currency;
        private string _terminalId;
        private SenderChannel _channel;
        private string _shopId;
        private int _bookmakerId;
        private EndCustomer _endCustomer = new EndCustomer();
        private int _limitId;
    
        /// <summary>3 or 4 letter currency code (4 letters only apply to mBTC)</summary>
        [Newtonsoft.Json.JsonProperty("currency", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(4, MinimumLength = 3)]
        public string Currency
        {
            get { return _currency; }
            set 
            {
                if (_currency != value)
                {
                    _currency = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Terminal id (optional)</summary>
        [Newtonsoft.Json.JsonProperty("terminalId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(36, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z\-_]{1,36}$")]
        public string TerminalId
        {
            get { return _terminalId; }
            set 
            {
                if (_terminalId != value)
                {
                    _terminalId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Communication channel</summary>
        [Newtonsoft.Json.JsonProperty("channel", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SenderChannel Channel
        {
            get { return _channel; }
            set 
            {
                if (_channel != value)
                {
                    _channel = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Shop id (optional)</summary>
        [Newtonsoft.Json.JsonProperty("shopId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(36, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z\-_]{1,36}$")]
        public string ShopId
        {
            get { return _shopId; }
            set 
            {
                if (_shopId != value)
                {
                    _shopId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Client's id (provided by Sportradar to the client)</summary>
        [Newtonsoft.Json.JsonProperty("bookmakerId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 2147483647.0)]
        public int BookmakerId
        {
            get { return _bookmakerId; }
            set 
            {
                if (_bookmakerId != value)
                {
                    _bookmakerId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Identification of the end user (customer)</summary>
        [Newtonsoft.Json.JsonProperty("endCustomer", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public EndCustomer EndCustomer
        {
            get { return _endCustomer; }
            set 
            {
                if (_endCustomer != value)
                {
                    _endCustomer = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Client's limit id (provided by Sportradar to the client)</summary>
        [Newtonsoft.Json.JsonProperty("limitId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 2147483647.0)]
        public int LimitId
        {
            get { return _limitId; }
            set 
            {
                if (_limitId != value)
                {
                    _limitId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Sender FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Sender>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum TicketOddsChange
    {
        [System.Runtime.Serialization.EnumMember(Value = "none")]
        None = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "any")]
        Any = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = "higher")]
        Higher = 2,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Bonus : System.ComponentModel.INotifyPropertyChanged
    {
        private long _value;
        private BonusType? _type = Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket.BonusType.Total;
        private BonusMode? _mode = Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket.BonusMode.All;
    
        /// <summary>Quantity multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(0.0, 1000000000000000000.0)]
        public long Value
        {
            get { return _value; }
            set 
            {
                if (_value != value)
                {
                    _value = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Type (optional, default total)</summary>
        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public BonusType? Type
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
    
        /// <summary>Payout mode (optional, default all). Relevant mostly for system bets. All: all bets must win for bonus to be paid out.</summary>
        [Newtonsoft.Json.JsonProperty("mode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public BonusMode? Mode
        {
            get { return _mode; }
            set 
            {
                if (_mode != value)
                {
                    _mode = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Bonus FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Bonus>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Stake : System.ComponentModel.INotifyPropertyChanged
    {
        private long _value;
        private StakeType? _type = Sportradar.MTS.SDK.Entities.Internal.Dto.Ticket.StakeType.Total;
    
        /// <summary>Quantity multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000000000000000.0)]
        public long Value
        {
            get { return _value; }
            set 
            {
                if (_value != value)
                {
                    _value = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Type of stake (optional, default total)</summary>
        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StakeType? Type
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
        
        public static Stake FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Stake>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>Array of selection references to form the bet (optional, if missing all selections are taken)</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Anonymous3 : System.ComponentModel.INotifyPropertyChanged
    {
        private int _selectionIndex;
        private bool _banker = false;
    
        /// <summary>Selection index from 'ticket.selections' array (zero based)</summary>
        [Newtonsoft.Json.JsonProperty("selectionIndex", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(0.0, 62.0)]
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
    
        /// <summary>Flag if selection is banker (optional, default false)</summary>
        [Newtonsoft.Json.JsonProperty("banker", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Banker
        {
            get { return _banker; }
            set 
            {
                if (_banker != value)
                {
                    _banker = value; 
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
    public enum SenderChannel
    {
        [System.Runtime.Serialization.EnumMember(Value = "internet")]
        Internet = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "retail")]
        Retail = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = "terminal")]
        Terminal = 2,
    
        [System.Runtime.Serialization.EnumMember(Value = "mobile")]
        Mobile = 3,
    
        [System.Runtime.Serialization.EnumMember(Value = "phone")]
        Phone = 4,
    
        [System.Runtime.Serialization.EnumMember(Value = "sms")]
        Sms = 5,
    
        [System.Runtime.Serialization.EnumMember(Value = "callCentre")]
        CallCentre = 6,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class EndCustomer : System.ComponentModel.INotifyPropertyChanged
    {
        private string _ip;
        private string _languageId;
        private string _deviceId;
        private string _id;
        private long? _confidence;
    
        /// <summary>End user's ip</summary>
        [Newtonsoft.Json.JsonProperty("ip", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^(((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])))$")]
        public string Ip
        {
            get { return _ip; }
            set 
            {
                if (_ip != value)
                {
                    _ip = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>ISO 639-1 language code</summary>
        [Newtonsoft.Json.JsonProperty("languageId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(2, MinimumLength = 2)]
        public string LanguageId
        {
            get { return _languageId; }
            set 
            {
                if (_languageId != value)
                {
                    _languageId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>End user's device id</summary>
        [Newtonsoft.Json.JsonProperty("deviceId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(36, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z\-_]{1,36}$")]
        public string DeviceId
        {
            get { return _deviceId; }
            set 
            {
                if (_deviceId != value)
                {
                    _deviceId = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>End user's unique id (in client's system)</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(36, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z\-_]{1,36}$")]
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
    
        /// <summary>Suggested CCF of the customer multiplied by 10_000 and rounded to a long value</summary>
        [Newtonsoft.Json.JsonProperty("confidence", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 9223372036854775807.0)]
        public long? Confidence
        {
            get { return _confidence; }
            set 
            {
                if (_confidence != value)
                {
                    _confidence = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static EndCustomer FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EndCustomer>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum BonusType
    {
        [System.Runtime.Serialization.EnumMember(Value = "total")]
        Total = 0,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum BonusMode
    {
        [System.Runtime.Serialization.EnumMember(Value = "all")]
        All = 0,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum StakeType
    {
        [System.Runtime.Serialization.EnumMember(Value = "total")]
        Total = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "unit")]
        Unit = 1,
    
    }
}