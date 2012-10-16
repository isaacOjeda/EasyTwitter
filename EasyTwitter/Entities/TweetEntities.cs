using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTwitter.Entities
{
    public class TweetEntities
    {
        /// <summary>
        /// 
        /// </summary>
        public List<EntitieHashTag> HTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<EntitieUrl> Urls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> UserMentions { get; set; }
    }
}
