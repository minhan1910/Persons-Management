﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        // default: nvarchar(max)
        [StringLength(40)] // nvarchar(40)
        public string? PersonName { get; set; }
        
        [StringLength(40)] // nvarchar(40)
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)] // nvarchar(10)
        public string? Gender { get; set; }

        // Unique Identifier
        public Guid? CountryID { get; set; }
        
        [StringLength(200)] // nvarchar(200)
        public string? Country { get; set; }

        [StringLength(200)] // nvarchar(200)
        public string? Address { get; set; }

        // bit
        public bool ReceiveNewsLetters { get; set; }

        public static Person Create() => new Person();
    }
}
