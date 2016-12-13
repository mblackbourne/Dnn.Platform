// DotNetNukeŽ - http://www.dotnetnuke.com
// Copyright (c) 2002-2017
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System;
using System.Net.Http;

namespace DotNetNuke.Web.Api.Internal
{
// ReSharper disable InconsistentNaming
    public class IFrameSupportedValidateAntiForgeryTokenAttribute : ValidateAntiForgeryTokenAttribute
// ReSharper restore InconsistentNaming
    {
        public override bool IsAuthorized(AuthFilterContext context)
        {
            if (base.IsAuthorized(context)) return true;
            context.AuthFailureMessage = null;

            try
            {
                var queryString = context.ActionContext.Request.GetQueryNameValuePairs();
                var token = string.Empty;
                foreach(var kvp in queryString)
                {
                    if (kvp.Key == "__RequestVerificationToken"){
                        token = kvp.Value;
                        break;
                    }
                }
                string cookieValue = GetAntiForgeryCookieValue(context);

                AntiForgery.Instance.Validate(cookieValue, token );
            }
            catch (Exception e)
            {
                context.AuthFailureMessage = e.Message;
                return false;
            }

            return true;
        }

        public override bool AllowMultiple
        {
            get { return false; }
        }
    }
}