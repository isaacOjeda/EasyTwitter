using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTwitter.Entities
{
    public class Tweet
    {

        /// <summary>
        /// 
        /// </summary>
        public string CreatedAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TweetEntities Entities { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TwitterUser User { get; set; }
    }
}
