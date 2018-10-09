//using AutoMapper;
//using DAL.Models;
//using DAL.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewComponents;
//using OnlineStore.Models.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OnlineStore.ViewComponents
//{
//    public class MoviesViewComponent : ViewComponent
//    {
//        private readonly IMovieRepository _movieRepo;
//        private readonly IMovieGenreRepository _movieGenreRepo;
//        private readonly MapperConfiguration _mapperConfiguration;

//        public MoviesViewComponent(IMovieRepository movieRepo, IMovieGenreRepository movieGenreRepo)
//        {
//            _movieRepo = movieRepo;
//            _movieGenreRepo = movieGenreRepo;
//            _mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<Movie, MovieViewModel>();
//                cfg.CreateMap<MovieViewModel, Movie>().AfterMap((src, dest)
//                    => dest.MovieGenre = _movieGenreRepo.Find(x => x.GenreName == src.Genre));
//            });
//        }

//        public async Task<IViewComponentResult> InvokeAsync(string movieGenre, string searchString)
//        {
//            var movies = _movieRepo.GetMovies(movieGenre, searchString);
//            if(movies?.Count == 0)
//            {
//                return new ContentViewComponentResult("No movies available");
//            }
//            var vms = _mapperConfiguration.CreateMapper().Map<IList<MovieViewModel>>(movies);
//            return View("MoviesView", vms);
//        }
//    }
//}
