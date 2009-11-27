using System;
using System.Web;
using Ploosqva.WebUtils.Communication;

namespace Ploosqva.WebUtils.Platnosci.pl
{
    ///<summary>
    /// Class used to commit online payment using Platnosci.pl
    ///</summary>
    public class OnlinePayment
    {
        private const string POST_URL_BASE = "https://www.platnosci.pl/paygw/{0}/NewPayment";

        #region Platnosci.pl mandatory params
        private double amount;
        private string client_ip;
        private string desc;
        private string email;
        private string first_name;
        private string last_name;
        private string pos_auth_key;
        private int pos_id;
        private Guid sessionId = Guid.NewGuid();

        private int PosId
        {
            get
            {
                if (pos_id == 0)
                    throw new InvalidParamException("pos_id");

                return pos_id;
            }
            set
            {
                pos_id = value;
            }
        }

        private string PosAuthKey
        {
            get
            {
                if (string.IsNullOrEmpty(pos_auth_key) || pos_auth_key.Length != 7)
                    throw new InvalidParamException("pos_auth_key");

                return pos_auth_key;
            }
            set
            {
                pos_auth_key = value;
            }
        }

        private double Amount
        {
            get
            {
                if (amount == Double.MinValue)
                    throw new InvalidParamException("amount");

                return amount;
            }
            set
            {
                amount = value;
            }
        }

        private string Desc
        {
            get
            {
                if (string.IsNullOrEmpty(desc))
                    throw new InvalidParamException("desc");

                return desc;
            }
            set
            {
                desc = value;
            }
        }

        private string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(first_name) || first_name.Length > 100)
                    throw new InvalidParamException("first_name");

                return first_name;
            }
            set
            {
                first_name = value;
            }
        }

        private string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(last_name) || last_name.Length > 100)
                    throw new InvalidParamException("last_name");

                return last_name;
            }
            set
            {
                last_name = value;
            }
        }

        private string Email
        {
            get
            {
                if (string.IsNullOrEmpty(email) || email.Length > 100)
                    throw new InvalidParamException("email");

                return email;
            }
            set
            {
                email = value;
            }
        }

        private string ClientIp
        {
            get
            {
                client_ip = GetClientIp();

                if (string.IsNullOrEmpty(client_ip))
                    throw new InvalidParamException("client_ip");

                return client_ip;
            }
        }

        #endregion

        #region Platnosci.pl optional fields
        ///<summary>
        /// Payment type (ie. credit card, mBank). 
        /// This parameter is deprecated.
        /// See platnosci.pl docs for more info.
        ///</summary>
        public string PayType { private get; set; }
        ///<summary>
        /// If true, pay_type will be send with this payment
        ///</summary>
        public bool PostPayType;
        ///<summary>
        /// Current order's id
        ///</summary>
        public string OrderId { private get; set; }
        ///<summary>
        /// If true, order_id will be send with this payment
        ///</summary>
        public bool PostOrderId;
        ///<summary>
        /// Additional description
        ///</summary>
        public string Desc2 { private get; set; }
        ///<summary>
        /// If true, desc2 will be send with this payment
        ///</summary>
        public bool PostDesc2;
        ///<summary>
        /// Description used by credit card payments
        ///</summary>
        public string TrsDesc { private get; set; }
        ///<summary>
        /// If true, trsDesc will be send with this payment
        ///</summary>
        public bool PostTrsDesc;
        ///<summary>
        /// Client's Street number
        ///</summary>
        public string StreetAn { private get; set; }
        ///<summary>
        /// If true, street_an will be send with this payment
        ///</summary>
        public bool PostStreetAn;
        ///<summary>
        /// Client's Flat number
        ///</summary>
        public string StreetHn { private get; set; }
        ///<summary>
        /// If true, street_hn will be send with this payment
        ///</summary>
        public bool PostStreetHn;
        ///<summary>
        /// Client's Street name
        ///</summary>
        public string Street { private get; set; }
        ///<summary>
        /// If true, street will be send with this payment
        ///</summary>
        public bool PostStreet;
        ///<summary>
        /// Client's Post (zip) code
        ///</summary>
        public string PostCode { private get; set; }
        ///<summary>
        /// If true, post_code will be send with this payment
        ///</summary>
        public bool PostPostCode;
        ///<summary>
        /// Client's City
        ///</summary>
        public string City { private get; set; }
        ///<summary>
        /// If true, city will be send with this payment
        ///</summary>
        public bool PostCity;
        ///<summary>
        /// Client's Country
        ///</summary>
        public string Country { private get; set; }
        ///<summary>
        /// If true, country will be send with this payment
        ///</summary>
        public bool PostCountry;
        ///<summary>
        /// Client's phone number
        ///</summary>
        public string Phone { private get; set; }
        ///<summary>
        /// If true, phone will be send with this payment
        ///</summary>
        public bool PostPhone;
        /// <summary>
        /// Client's language
        /// </summary>
        public string Language { private get; set; }
        ///<summary>
        /// If true, language will be send with this payment
        ///</summary>
        public bool PostLanguage;
        /// <summary>
        /// Set to 1, if client's browser supports javascript
        /// </summary>
        public string Js { private get; set; }
        ///<summary>
        /// If true, js will be send with this payment
        ///</summary>
        public bool PostJs;
        /// <summary>
        /// Login of payback user, who will receive points
        /// </summary>
        public string PaybackLogin { private get; set; }
        ///<summary>
        /// If true, payback_login will be send with this payment
        ///</summary>
        public bool PostPaybackLogin;

        #endregion

        ///<summary>
        /// Creates a new instance of OnlinePayment class, which is then ready
        /// to post to payment processign website
        ///</summary>
        ///<param name="posId">platnosci.pl issued param</param>
        ///<param name="posAuthKey">platnosci.pl issued param</param>
        ///<param name="amount">billing amount</param>
        ///<param name="desc">payment description</param>
        ///<param name="firstName">client name</param>
        ///<param name="lastName">client surname</param>
        ///<param name="email">client email</param>
        public OnlinePayment(int posId, string posAuthKey, double amount,
            string desc, string firstName, string lastName, string email)
        {
            PosId = posId;
            PosAuthKey = posAuthKey;
            Amount = amount;
            Desc = desc;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        ///<summary>
        /// Fills the payment form, and posts it to platnosci.pl
        ///</summary>
        ///<param name="encoding">encoding used to post this payment</param>
        public RemotePost CreateRemotePostForm(CharacterEncoding encoding)
        {
            string postUrl = string.Format(POST_URL_BASE, encoding);

            RemotePost paymentPost = new RemotePost(HttpPostMethod.Post, "payment")
                 {
                     Url = new Uri(postUrl)
                 };

            FillMandatoryFields(paymentPost);
            FillOptionalFields(paymentPost);

            return paymentPost;
        }

        private void FillOptionalFields(RemotePost paymentPost)
        {
            if (PostPayType) paymentPost.Add("pay_type", PayType);
            if (PostOrderId) paymentPost.Add("order_id", OrderId);
            if (PostDesc2) paymentPost.Add("desc2", Desc2);
            if (PostTrsDesc) paymentPost.Add("trsDesc", TrsDesc);
            if (PostStreet) paymentPost.Add("street", Street);
            if (PostStreetAn) paymentPost.Add("street_an", StreetAn);
            if (PostStreetHn) paymentPost.Add("street_hn", StreetHn);
            if (PostPostCode) paymentPost.Add("post_code", PostCode);
            if (PostCity) paymentPost.Add("city", City);
            if (PostCountry) paymentPost.Add("country", Country);
            if (PostPhone) paymentPost.Add("phone", Phone);
            if (PostLanguage) paymentPost.Add("language", Language);
            if (PostJs) paymentPost.Add("js", Js);
            if (PostPaybackLogin) paymentPost.Add("payback_login", PaybackLogin);
        }

        private void FillMandatoryFields(RemotePost paymentPost)
        {
            paymentPost.Add("amount", Math.Round(Amount * 100).ToString());
            paymentPost.Add("desc", Desc);
            paymentPost.Add("first_name", FirstName);
            paymentPost.Add("last_name", LastName);
            paymentPost.Add("email", Email);
            paymentPost.Add("client_ip", ClientIp);
            paymentPost.Add("pos_id", PosId.ToString());
            paymentPost.Add("pos_auth_key", PosAuthKey);
            paymentPost.Add("session_id", sessionId.ToString());
        }

        private static string GetClientIp()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }
    }
}
