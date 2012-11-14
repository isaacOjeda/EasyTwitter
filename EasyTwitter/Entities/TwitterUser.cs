using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTwitter.Entities
{
    public class TwitterUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProfileImageUrl {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public string Location {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public decimal FavouritesCount {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public decimal Id {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public decimal FollowersCount {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public string Description {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public string ScreenName {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public bool Verified { get; set; }
    }
}
