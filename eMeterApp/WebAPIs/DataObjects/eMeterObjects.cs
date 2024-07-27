using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestService.DataObjects
{
    public class eMeterObjects
    {
        [DataContract]
        public class UserCredentials
        {
            [DataMember]
            public string UserName { get; set; }

            [DataMember]
            public string Password { get; set; }

            [DataMember]
            public string OldPassword { get; set; }

            [DataMember]
            public string NewPassword { get; set; }

        }

        [DataContract]
        public class ResponseMessage
        {
            [DataMember]
            public int Code { get; set; }

            [DataMember]
            public string Message { get; set; }
        }

        [DataContract]
        public class Customer
        {
            [DataMember]
            public string CustomerId { get; set; }

            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string Address { get; set; }
        }

        [DataContract]
        public class Meter
        {
            [DataMember]
            public string MeterId { get; set; }

            [DataMember]
            public string CustomerId { get; set; }
        }

        [DataContract]
        public class MeterReading
        {
            [DataMember]
            public string MeterId { get; set; }

            [DataMember]
            public DateTime ReadingDate { get; set; }

            [DataMember]
            public double ReadingValue { get; set; }
        }

        [DataContract]
        public class User
        {
            [DataMember]
            public int CustomerID { get; set; }

            [DataMember]
            public string FullName { get; set; }

            [DataMember]
            public string EmailAddress { get; set; }

            [DataMember]
            public string MobileNo { get; set; }

            [DataMember]
            public string CnicNo { get; set; }

            [DataMember]
            public string FullAddress { get; set; }

            [DataMember]
            public string CityName { get; set; }

            [DataMember]
            public bool ActiveInd { get; set; }

            [DataMember]
            public string CreatedBy { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }
        }

        [DataContract]
        public class Credentials
        {
            [DataMember]
            public string UserName { get; set; }

            [DataMember]
            public string Password { get; set; }

            [DataMember]
            public int CustomerID { get; set; }

            [DataMember]
            public bool ActiveInd { get; set; }

            [DataMember]
            public DateTime ActiveDate { get; set; }

            [DataMember]
            public string CreatedBy { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }

        }

        [DataContract]
        public class MeterType
        {
            [DataMember]
            public int TypeID { get; set; }

            [DataMember]
            public string TypeName { get; set; }

            [DataMember]
            public bool ActiveInd { get; set; }

            [DataMember]
            public string CreatedBy { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }
        }

        [DataContract]
        public class MeterDetails
        {
            [DataMember]
            public int MeterID { get; set; }

            [DataMember]
            public int CustomerID { get; set; }

            [DataMember]
            public string MeterNo { get; set; }

            [DataMember]
            public string RefNo { get; set; }

            [DataMember]
            public string OldRefNo { get; set; }

            [DataMember]
            public DateTime ConnectionDate { get; set; }

            [DataMember]
            public int StatusID { get; set; }

            [DataMember]
            public int MeterLoad { get; set; }

            [DataMember]
            public bool ActiveInd { get; set; }

            [DataMember]
            public string CreatedBy { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }
        }
        [DataContract]
        public class RecordReadingRequest
        {
            [DataMember]
            public BillMain BillMain { get; set; }

            [DataMember]
            public List<BillDetails> BillDetailsList { get; set; }
        }

        [DataContract]
        public class BillMain
        {
            [DataMember]
            public int MeterID { get; set; }

            [DataMember]
            public int CustomerID { get; set; }

            [DataMember]
            public DateTime IssueDate { get; set; }

            [DataMember]
            public DateTime DueDate { get; set; }

            [DataMember]
            public int StatusID { get; set; }

            [DataMember]
            public DateTime BillMonth { get; set; }

            [DataMember]
            public DateTime ReadingDate { get; set; }

            [DataMember]
            public int UnitsConsumed { get; set; }

            [DataMember]
            public string CreatedBy { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }
        }

        [DataContract]
        public class BillDetails
        {
            [DataMember]
            public int BillID { get; set; }

            [DataMember]
            public int SlabID { get; set; }

            [DataMember]
            public decimal SlabRate { get; set; }

            [DataMember]
            public int UnitsApplied { get; set; }

            [DataMember]
            public decimal SlabAmount { get; set; }
        }

    }
}
