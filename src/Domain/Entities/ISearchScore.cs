﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
	public interface ISearchScore
	{
		double? SearchScore { get; init; }
	}
}
