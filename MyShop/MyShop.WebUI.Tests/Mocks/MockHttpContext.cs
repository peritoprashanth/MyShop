using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {

        private MockRequest request;
        private MockResponse response;
        private HttpCookieCollection cookies;        

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            response = new MockResponse(cookies);
            request = new MockRequest(cookies);
        }

        //public override HttpResponseBase Response => base.Response;
        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }
    }

    
    public class MockResponse: HttpResponseBase
    {
        private readonly HttpCookieCollection ourCustomCookie;
        public MockResponse(HttpCookieCollection cookies)
        {
            this.ourCustomCookie = cookies;
        }
        public override HttpCookieCollection Cookies
        {
            get
            {
                return ourCustomCookie;
            }
        }        
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection ourCustomCookie;
        public MockRequest(HttpCookieCollection cookies)
        {
            this.ourCustomCookie = cookies;
        }
        public override HttpCookieCollection Cookies
        {
            get
            {
                return ourCustomCookie;
            }            
        }
    }
}
