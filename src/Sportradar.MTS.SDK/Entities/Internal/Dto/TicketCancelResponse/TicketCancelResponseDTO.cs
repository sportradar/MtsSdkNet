﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v8.6.6263.34621 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketCancelResponse
{
    #pragma warning disable // Disable all warnings

    /// <summary>Object carrying information about cancellation response</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Reason : System.ComponentModel.INotifyPropertyChanged
    {
        private int _code;
        private string _message;
    
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
    
        /// <summary>Cancellation rejection reason description</summary>
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
    
    /// <summary>Cancellation status - cancelled, not_cancelled</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum Status
    {
        [System.Runtime.Serialization.EnumMember(Value = "not_cancelled")]
        Not_cancelled = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "cancelled")]
        Cancelled = 1,
    
    }
    
    /// <summary>Ticket cancel response 2.3 schema</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class TicketCancelResponseDTO : System.ComponentModel.INotifyPropertyChanged
    {
        private Result _result = new Result();
        private string _signature;
        private string _version;
    
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
        
        public static TicketCancelResponseDTO FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TicketCancelResponseDTO>(data);
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
    
        /// <summary>Ticket id</summary>
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
}