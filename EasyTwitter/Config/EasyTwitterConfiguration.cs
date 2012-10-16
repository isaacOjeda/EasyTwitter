using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EasyTwitter.Config
{
    public class EasyTwitterConfiguration:ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("consumerKey", IsRequired = true)]
        public string CONSUMER_KEY
        {
            get
            {
                return (string)this["consumerKey"];
            }
            set
            {
                this["consumerKey"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("consumerSecret", IsRequired = true)]
        public string CONSUMER_SECRET
        {
            get
            {
                return (string)this["consumerSecret"];
            }
            set
            {
                this["consumerSecret"] = value;
            }
        }

        [ConfigurationProperty("callbackUri", IsRequired = true)]
        public Uri CallBackUri
        {
            get
            {
                return (Uri) this["callbackUri"];
            }
            set
            {
                this["callbackUri"] = value;
            }
        
        }
    }
}
