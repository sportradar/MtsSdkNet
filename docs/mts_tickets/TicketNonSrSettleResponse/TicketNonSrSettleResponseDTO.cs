﻿//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v8.6.6263.34621 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

namespace Sportradar.MTS.SDK.Entities.Internal.Dto.TicketNonSrSettleResponse
{
    #pragma warning disable // Disable all warnings

    /// <summary>Object carrying information about settle response</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class Reason : System.ComponentModel.INotifyPropertyChanged
    {
        private int _code;
        private string _message;
    
        /// <summary>Settle response code</summary>
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
    
        /// <summary>Settle rejection reason description</summary>
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
    
    /// <summary>Settle status - accepted, rejected</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public enum Status
    {
        [System.Runtime.Serialization.EnumMember(Value = "rejected")]
        Rejected = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "accepted")]
        Accepted = 1,
    
    }
    
    /// <summary>Ticket non-Sportradar settle response v2 schema</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "8.6.6263.34621")]
    public partial class TicketNonSrSettleResponseDTO : System.ComponentModel.INotifyPropertyChanged
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
    
        /// <summary>Response signature</summary>
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
    
        /// <summary>JSON format version (must be '2.4')</summary>
        [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(3, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^(2\.4)$")]
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
        
        public static TicketNonSrSettleResponseDTO FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TicketNonSrSettleResponseDTO>(data);
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