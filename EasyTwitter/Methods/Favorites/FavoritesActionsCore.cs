/*
 *  FavoritesACtionsCore.cs
 *  
 *  Copyright 2012 Isaac Ojeda <isaacoq(at)gmail(dot)com>
 * 
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 * 
 *  This progam is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace EasyTwitter
{
    public class FavoritesActionsCore:MethodsCore
    {

        public FavoritesActionsCore(OAuthTokens tokens)
            : base("https://api.twitter.com/1.1/favorites/")
        {
            this.Tokens = tokens;
        }


        protected override TwitterResponse<string> BeginRequest(HTTPVerb overrideVerb = HTTPVerb.GET)
        {
            return this.ExecuteRequest(overrideVerb);
        }
    }
}
