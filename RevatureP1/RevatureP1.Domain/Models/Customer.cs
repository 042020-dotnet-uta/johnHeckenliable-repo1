﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Models
{
	public class Customer
	{
		#region Properties
		[Key]
		public int CustomerId { get; set; }

		private string firstName;
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		private string email;
		public string Email
		{
			get { return email; }
			set { email = value; }
		}
		#endregion

		#region Constructors
		public Customer()
		{ }

		public Customer(string fName, string lName, string email)
		{
			this.FirstName = fName;
			this.LastName = lName;
			this.email = email;
		}
		#endregion

		#region Methods
		#endregion
	}
}
