using Blogifier.Core.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogifier.Widgets
{
    public partial class Users
    {
        [Inject]
        protected IDataService DataService { get; set; }
    }
}
