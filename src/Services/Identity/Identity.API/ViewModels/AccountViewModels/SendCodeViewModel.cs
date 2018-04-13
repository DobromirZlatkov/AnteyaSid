﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AnteyaSidOnContainers.Services.Identity.API.ViewModels.AccountViewModels
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
