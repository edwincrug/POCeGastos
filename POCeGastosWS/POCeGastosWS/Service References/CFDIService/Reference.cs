﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión del motor en tiempo de ejecución:2.0.50727.5477
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eGastosWS.CFDIService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Extract", Namespace="http://schemas.datacontract.org/2004/07/Entities.Expense")]
    [System.SerializableAttribute()]
    public partial class Extract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double amountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string commerceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string currencyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idExtractField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime operationDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string transactionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double amount {
            get {
                return this.amountField;
            }
            set {
                if ((this.amountField.Equals(value) != true)) {
                    this.amountField = value;
                    this.RaisePropertyChanged("amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string city {
            get {
                return this.cityField;
            }
            set {
                if ((object.ReferenceEquals(this.cityField, value) != true)) {
                    this.cityField = value;
                    this.RaisePropertyChanged("city");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string commerce {
            get {
                return this.commerceField;
            }
            set {
                if ((object.ReferenceEquals(this.commerceField, value) != true)) {
                    this.commerceField = value;
                    this.RaisePropertyChanged("commerce");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string currency {
            get {
                return this.currencyField;
            }
            set {
                if ((object.ReferenceEquals(this.currencyField, value) != true)) {
                    this.currencyField = value;
                    this.RaisePropertyChanged("currency");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idExtract {
            get {
                return this.idExtractField;
            }
            set {
                if ((this.idExtractField.Equals(value) != true)) {
                    this.idExtractField = value;
                    this.RaisePropertyChanged("idExtract");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime operationDate {
            get {
                return this.operationDateField;
            }
            set {
                if ((this.operationDateField.Equals(value) != true)) {
                    this.operationDateField = value;
                    this.RaisePropertyChanged("operationDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string transaction {
            get {
                return this.transactionField;
            }
            set {
                if ((object.ReferenceEquals(this.transactionField, value) != true)) {
                    this.transactionField = value;
                    this.RaisePropertyChanged("transaction");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Xml", Namespace="http://schemas.datacontract.org/2004/07/Entities.Expense")]
    [System.SerializableAttribute()]
    public partial class Xml : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DireccionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InvoiceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double IvaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreEmisorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double SubtotalField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Direccion {
            get {
                return this.DireccionField;
            }
            set {
                if ((object.ReferenceEquals(this.DireccionField, value) != true)) {
                    this.DireccionField = value;
                    this.RaisePropertyChanged("Direccion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Fecha {
            get {
                return this.FechaField;
            }
            set {
                if ((this.FechaField.Equals(value) != true)) {
                    this.FechaField = value;
                    this.RaisePropertyChanged("Fecha");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Invoice {
            get {
                return this.InvoiceField;
            }
            set {
                if ((object.ReferenceEquals(this.InvoiceField, value) != true)) {
                    this.InvoiceField = value;
                    this.RaisePropertyChanged("Invoice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Iva {
            get {
                return this.IvaField;
            }
            set {
                if ((this.IvaField.Equals(value) != true)) {
                    this.IvaField = value;
                    this.RaisePropertyChanged("Iva");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NombreEmisor {
            get {
                return this.NombreEmisorField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreEmisorField, value) != true)) {
                    this.NombreEmisorField = value;
                    this.RaisePropertyChanged("NombreEmisor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Subtotal {
            get {
                return this.SubtotalField;
            }
            set {
                if ((this.SubtotalField.Equals(value) != true)) {
                    this.SubtotalField = value;
                    this.RaisePropertyChanged("Subtotal");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CFDIService.ICFDI")]
    public interface ICFDI {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICFDI/selectExtract", ReplyAction="http://tempuri.org/ICFDI/selectExtractResponse")]
        System.Collections.Generic.List<eGastosWS.CFDIService.Extract> selectExtract(int option, string responsibleEmployeeNum, System.Nullable<System.DateTime> dateFrom, System.Nullable<System.DateTime> dateTo, System.Nullable<double> amountFrom, System.Nullable<double> amountTo, int idRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICFDI/selectDetail", ReplyAction="http://tempuri.org/ICFDI/selectDetailResponse")]
        System.Collections.Generic.List<eGastosWS.CFDIService.Extract> selectDetail(int idExtract);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICFDI/searchInvoices", ReplyAction="http://tempuri.org/ICFDI/searchInvoicesResponse")]
        System.Collections.Generic.List<eGastosWS.CFDIService.Xml> searchInvoices(string responsibleMail, int idRequest, System.Nullable<System.DateTime> dateFrom, System.Nullable<System.DateTime> dateTo, System.Nullable<double> amountFrom, System.Nullable<double> amountTo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ICFDIChannel : eGastosWS.CFDIService.ICFDI, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class CFDIClient : System.ServiceModel.ClientBase<eGastosWS.CFDIService.ICFDI>, eGastosWS.CFDIService.ICFDI {
        
        public CFDIClient() {
        }
        
        public CFDIClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CFDIClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CFDIClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CFDIClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<eGastosWS.CFDIService.Extract> selectExtract(int option, string responsibleEmployeeNum, System.Nullable<System.DateTime> dateFrom, System.Nullable<System.DateTime> dateTo, System.Nullable<double> amountFrom, System.Nullable<double> amountTo, int idRequest) {
            return base.Channel.selectExtract(option, responsibleEmployeeNum, dateFrom, dateTo, amountFrom, amountTo, idRequest);
        }
        
        public System.Collections.Generic.List<eGastosWS.CFDIService.Extract> selectDetail(int idExtract) {
            return base.Channel.selectDetail(idExtract);
        }
        
        public System.Collections.Generic.List<eGastosWS.CFDIService.Xml> searchInvoices(string responsibleMail, int idRequest, System.Nullable<System.DateTime> dateFrom, System.Nullable<System.DateTime> dateTo, System.Nullable<double> amountFrom, System.Nullable<double> amountTo) {
            return base.Channel.searchInvoices(responsibleMail, idRequest, dateFrom, dateTo, amountFrom, amountTo);
        }
    }
}
