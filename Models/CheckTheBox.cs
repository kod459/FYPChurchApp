using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Web.Mvc;
using PIMS.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PIMS.Models
{
    public class CheckTheBox
    {
        public string state { get; set; }
        public IEnumerable<SelectListItem> states { get; set; }

        CheckTheBox()
        {
            states = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = false,
                    Text = "Active",
                    Value = "Active"
                },
                new SelectListItem
                {
                    Selected = true,
                    Text = "Obsolete",
                    Value = "Obsolete"
                }
            };
        }
    }
}