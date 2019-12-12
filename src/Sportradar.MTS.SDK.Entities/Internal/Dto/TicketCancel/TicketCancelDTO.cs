﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v8.6.6263.34621 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancel
{
#pragma warning disable // Disable all warnings

    /// <summary>Ticket cancel version 2.3 schema</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class TicketCancelDTO : System.ComponentModel.INotifyPropertyChanged
    {
        private Cancel _cancel = new Cancel();
    
        /// <summary>Actual ticket cancel being sent to Sportradar</summary>
        [Newtonsoft.Json.JsonProperty("cancel", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Cancel Cancel
        {
            get { return _cancel; }
            set 
            {
                if (_cancel != value)
                {
                    _cancel = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.Cancel);
        }
        
        public static TicketCancelDTO FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TicketCancelDTO>(data);
        }
    
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Cancel : System.ComponentModel.INotifyPropertyChanged
    {
        private long _timestampUtc;
        private string _ticketId;
        private Sender _sender = new Sender();
        private int _code;
        private int? _cancelPercent;
        private IEnumerable<Anonymous> _betCancel = new Collection<Anonymous>();
        private string _version;
    
        /// <summary>Timestamp of ticket cancel placement (in UNIX time millis)</summary>
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
    
        /// <summary>Ticket id to cancel (in the client's system)</summary>
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
    
        /// <summary>Cancellation code</summary>
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
    
        /// <summary>Cancel percent. Quantity multiplied by 10_000 and rounded to a long value. Only applicable if cancelling whole ticket. Max 100%.</summary>
        [Newtonsoft.Json.JsonProperty("cancelPercent", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000.0)]
        public int? CancelPercent
        {
            get { return _cancelPercent; }
            set 
            {
                if (_cancelPercent != value)
                {
                    _cancelPercent = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>Array of (betId, cancelPercent) pairs, if performing partial cancellation. Applicable only if ticket-level cancel percent is null.</summary>
        [Newtonsoft.Json.JsonProperty("betCancel", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IEnumerable<Anonymous> BetCancel
        {
            get { return _betCancel; }
            set 
            {
                if (_betCancel != value)
                {
                    _betCancel = value; 
                    RaisePropertyChanged();
                }
            }
        }
    
        /// <summary>JSON format version (must be '2.3')</summary>
        [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(3, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^(2\.3)$")]
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
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Cancel FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Cancel>(data);
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
        private int _bookmakerId;
    
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
    public partial class Anonymous : System.ComponentModel.INotifyPropertyChanged
    {
        private string _id;
        private int? _cancelPercent;
    
        /// <summary>Bet id</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
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
    
        /// <summary>Cancel percent. Quantity multiplied by 10_000 and rounded to a long value. Max 100%.</summary>
        [Newtonsoft.Json.JsonProperty("cancelPercent", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1.0, 1000000.0)]
        public int? CancelPercent
        {
            get { return _cancelPercent; }
            set 
            {
                if (_cancelPercent != value)
                {
                    _cancelPercent = value; 
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
}