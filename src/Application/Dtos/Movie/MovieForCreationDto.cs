﻿using Cinema.Application.Dtos.Address;
using Cinema.Domain.Entities;
using Cinema.Domain.Enums;

namespace Cinema.Application.Dtos.Movie
{
    public class MovieForCreationDto
	{
		public required string Name { get; init; }

		public required DateOnly ReleaseDate { get; init; }

		public required string Thumbnail { get; init; }

		public required MovieCategory[] Categories { get; init; }

		public required string Synopsis { get; set; }

		public required MovieDirector Director { get; init; }

		public string? ChangedBy { get; init; }
	}
}
